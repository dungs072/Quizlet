using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletAchivement.DBContexts;
using QuizletAchivement.Models;
using QuizletAchivement.ViewModels;
using System;

namespace QuizletAchivement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchivementController : ControllerBase
    {
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa123@gmail.com";
        private readonly string password = "123456";
        private readonly AchivementDBContext dBContext;
        public AchivementController(AchivementDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        #region Achivement

        [HttpGet]
        public async Task<ActionResult<IEnumerable<THANHTUU>>> GetTHANHTUU()
        {
            List<THANHTUU> thanhtuus = await dBContext.thanhtuus.ToListAsync();
            List<CHITIETTHANHTUU> chitietthanhtuus = await dBContext.chitietthanhtuus.ToListAsync();
            foreach(var thanhtuu in thanhtuus)
            {
                foreach(var item in chitietthanhtuus)
                {
                    if(item.thanhtuu==thanhtuu)
                    {
                        thanhtuu.CanDelete = false;
                        break;
                    }
                    
                }
            }
            return await dBContext.thanhtuus.ToListAsync();
        }
        [HttpGet("{AchivementId}")]
        public async Task<ActionResult<THANHTUU>> GetTHANHTUUById(int AchivementId)
        {
            var THANHTUU = await dBContext.thanhtuus.FindAsync(AchivementId);
            return THANHTUU;
        }
        [HttpPost]
        public async Task<ActionResult> CreateTHANHTUU(THANHTUU thanhtuu)
        {

            await dBContext.thanhtuus.AddAsync(thanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateTHANHTUU(THANHTUU thanhtuu)
        {
            dBContext.thanhtuus.Update(thanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{AchivementId}")]
        public async Task<ActionResult> DeleteTHANHTUU(int AchivementId)
        {
            var thanhtuu = await dBContext.thanhtuus.FindAsync(AchivementId);
            if (thanhtuu.Image != null)
            {
                var cancellation = new CancellationTokenSource();
                // Initialize Firebase Storage
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

                var firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
                });
                string fileNameDelete = ExtractFileNameFromUrl(thanhtuu.Image);
                string deletePath = $"admin/{fileNameDelete}";
                await firebaseStorage.Child(deletePath).DeleteAsync();
            }
            dBContext.thanhtuus.Remove(thanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }

        static string ExtractFileNameFromUrl(string url)
        {
            // Use Uri to parse the URL
            Uri uri = new Uri(url);

            // Get the filename from the URL using Path.GetFileName
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }

        #endregion

        #region UserAchieve
        [HttpGet("UserAchieve/{UserId}")]
        public async Task<List<LevelTerms>> GetLevelTerms(int userId)
        {
            List<LevelTerms> levelTerms = new List<LevelTerms>();
            var levelghinhos = await dBContext.levelghinhos.ToListAsync();
            foreach (var levelghinho in levelghinhos)
            {
                LevelTerms levelterm = new LevelTerms();
                levelterm.LevelName = levelghinho.LevelName;
                levelterm.NumberTermsInLevel = await CountNumberTermsForLevel(levelghinho.LevelId, userId);
                levelTerms.Add(levelterm);
            }
            return levelTerms;
        }
        private async Task<int> CountNumberTermsForLevel(int levelId, int userId)
        {
            var thuatngus = await dBContext.thethuatngus
            .Where(a => a.hocphan.chude.nguoidung.UserId == userId && a.levelghinho.LevelId == levelId)
            .ToListAsync();

            return thuatngus.Count;
        }
        [HttpGet("AchieveStatistics/{userId}")]
        public async Task<AchieveStatistics> GetAchieveStatistics(int userId)
        {
            AchieveStatistics achiveStatistics = new AchieveStatistics();
            await GetLibraryStatistics(achiveStatistics, userId);
            await GetClassStatistics(achiveStatistics, userId);
            await GetSequenceStatistics(achiveStatistics, userId);
            return achiveStatistics;
        }
        public async Task GetLibraryStatistics(AchieveStatistics statistics, int userId)
        {
            var titles = (await dBContext.chudes.Where(a => a.nguoidung.UserId == userId).ToListAsync());
            int numberModules = 0;
            int numberTerms = 0;
            foreach (var title in titles)
            {
                numberModules += title.hocphans.Count;
                foreach (var module in title.hocphans)
                {
                    numberTerms += module.thethuatngus.Count;
                }
            }
            statistics.NumberTitle = titles.Count;
            statistics.NumberModule = numberModules;
            statistics.NumberTerms = numberTerms;

        }
        public async Task GetClassStatistics(AchieveStatistics statistics, int userId)
        {
            int count = await dBContext.lops.Where(a => a.NGUOIDUNG.UserId == userId).CountAsync();
            statistics.TotalClass = count;
        }
        public async Task GetSequenceStatistics(AchieveStatistics statistics, int userId)
        {
            var chuois = await dBContext.chitietchuois.Where(a => a.nguoidung.UserId == userId).ToListAsync();
            int count = 0;
            int maxCount = 0;
            for (int i = 0; i < chuois.Count - 1; i++)
            {
                var timeSpan = chuois[i + 1].LearningDay - chuois[i].LearningDay;
                double days = timeSpan.TotalDays;
                if (days == 1)
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
                if (count > maxCount)
                {
                    maxCount = count;
                }
            }
            statistics.LongestSquence = maxCount+1;
        }

        [HttpGet("GetSequenceCalender/{userId}")]
        public async Task<List<string>> GetSequenceCalender(int userId)
        {
            var sequenceDates = from a in dBContext.chitietchuois
                         where a.nguoidung.UserId == userId
                         select a.LearningDay.ToString("yyyy-MM-dd");
            return await sequenceDates.ToListAsync();
        }
        [HttpPost("MarkAttendance")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> MarkAttendance(MarkAttendance mark)
        {
            if (IsMarked(mark.UserId))
            {
                return NoContent();
            }
            DateTime currentDate = DateTime.Now;
            CHITIETCHUOI chitietchuoi = new CHITIETCHUOI();
            chitietchuoi.nguoidung = await dBContext.nguoidungs.FindAsync(mark.UserId);
            chitietchuoi.LearningDay = currentDate;
            chitietchuoi.SequenceId = 0;
            await dBContext.chitietchuois.AddAsync(chitietchuoi);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool IsMarked(int userId)
        {
            DateTime currentDate = DateTime.Now;
            var value = dBContext.chitietchuois.FirstOrDefault(a=>a.nguoidung.UserId==userId && a.LearningDay.Date == currentDate.Date);
            return value != null;
        }

        [HttpGet("GetBadges/{userId}")]
        public async Task<List<Badge>> GetBadges(int userId)
        {
            List<Badge> badgeList = new List<Badge>();
            var badges = await dBContext.thanhtuus.ToListAsync();
            foreach(var b in badges)
            {
                var badge = new Badge();
                badge.NameBadge = b.AchivementName;
                badge.IsAchieved = IsAchieve(b.AchivementId, userId,badge);
                badge.Image = b.Image;
                badgeList.Add(badge);
            }
            return badgeList;
        }
        //[HttpGet("GetBadges/{userId}")]
        public async Task<int> GetLength()
        {
            var badges = await dBContext.thanhtuus.ToListAsync();
            return badges.Count;
        }
        private bool IsAchieve(int achievementId,int userId,Badge badge)
        {
            var check = dBContext.chitietthanhtuus.FirstOrDefault(a => a.nguoidung.UserId == userId && a.thanhtuu.AchivementId == achievementId);
            if(check!=null)
            {
                badge.DateAchieved = check.AchieveDate.ToString("dd/MM/yyyy");
            }
            return check != null;
        }
        [HttpGet("UpdateBadge/{userId}/{typeBadge}")]
        public async Task<AchivementBadge> GetUpdateBadge(int userId, string typeBadge)
        {
            AchieveStatistics achiveStatistics = new AchieveStatistics();
            await GetLibraryStatistics(achiveStatistics, userId);
            var badges = await dBContext.thanhtuus.ToListAsync();
            foreach (var badge in badges)
            {
                if(badge.AchivementName.Contains(typeBadge))
                {
                    if (typeBadge == "modules")
                    {
                        if (badge.Condition < achiveStatistics.NumberModule)
                        {
                          
                            if(!CheckIsExistBadge(userId, badge.AchivementId))
                            {
                                return new AchivementBadge { AchivementId = badge.AchivementId,AchivementName = badge.AchivementName};
                            }
                            
                        }
                    }
                    if(typeBadge=="terms")
                    {
                        if (badge.Condition < achiveStatistics.NumberTerms)
                        {

                            if (!CheckIsExistBadge(userId, badge.AchivementId))
                            {
                                return new AchivementBadge { AchivementId = badge.AchivementId, AchivementName = badge.AchivementName };
                            }

                        }
                    }
                    //if(typeBadge=="participants")
                    //{
                    //    if (badge.Condition < achiveStatistics.)
                    //    {

                    //        if (!CheckIsExistBadge(userId, badge.AchivementId))
                    //        {
                    //            return new AchivementBadge { AchivementId = badge.AchivementId, AchivementName = badge.AchivementName };
                    //        }

                    //    }
                    //}
                }
              
            }
            return null;
        }
        //private async int GetParticipantInAllClass(int userId)
        //{
        //    int count = 0;
        //    List<LOP> lops = await dBContext.lops.Where(a => a.NGUOIDUNG.UserId == userId).ToListAsync();
        //    foreach(var lop in lops)
        //    {
        //        lo
        //    }
        //}
        private bool CheckIsExistBadge(int userId, int badgeId)
        {
            var exists = dBContext.chitietthanhtuus
                .Any(ctt => ctt.nguoidung.UserId == userId && ctt.thanhtuu.AchivementId == badgeId);

            return exists;
        }
        [HttpPost("UpdateBadge")]
        public async Task<ActionResult> AddUpdateBadge(AchieveBadge achieveBadge)
        {
            DateTime currentDate = DateTime.Now;
            CHITIETTHANHTUU chitietthanhtuu = new CHITIETTHANHTUU();
            chitietthanhtuu.nguoidung = await dBContext.nguoidungs.FindAsync(achieveBadge.UserId);
            chitietthanhtuu.AchieveDate = currentDate;
            chitietthanhtuu.thanhtuu = await dBContext.thanhtuus.FindAsync(achieveBadge.AchievementId);
            await dBContext.chitietthanhtuus.AddAsync(chitietthanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region Admin
        [HttpGet("Admin/LevelTerm")]
        public async Task<ActionResult<IEnumerable<LEVELGHINHO>>> GetListLEVELGHINHO()
        {
            return await dBContext.levelghinhos.ToListAsync();
        }
        [HttpGet("Admin/LevelTerm/{levelId}")]
        public async Task<ActionResult<LEVELGHINHO>> GetLEVELGHINHO(int levelId)
        {
            return await dBContext.levelghinhos.FindAsync(levelId);
        }
        [HttpPut("Admin/LevelTerm")]
        public async Task<ActionResult> UpdateLEVELGHINHO(LEVELGHINHO level)
        {
            dBContext.levelghinhos.Update(level);
            await dBContext.SaveChangesAsync();
            return Ok();
        }


        #endregion

    }
}
