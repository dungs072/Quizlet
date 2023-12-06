using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletAchivement.DBContexts;
using QuizletAchivement.Models;
using QuizletAchivement.ViewModels;

namespace QuizletAchivement.Respository
{
    public class AchieveRespository : IAchieveRespository
    {
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa123@gmail.com";
        private readonly string password = "123456";
        private readonly AchivementDBContext dBContext;
        private readonly HttpClient client;
        public AchieveRespository(AchivementDBContext dBContext, HttpClient client)
        {
            this.dBContext = dBContext;
            this.client = client;
        }


        #region Achivement
        public async Task<bool> CreateTHANHTUU(THANHTUU thanhtuu)
        {
            try
            {
                await dBContext.thanhtuus.AddAsync(thanhtuu);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTHANHTUU(int AchivementId)
        {
            try
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string ExtractFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }

        public async Task<List<THANHTUU>> GetTHANHTUU()
        {
            try
            {
                List<THANHTUU> thanhtuus = await dBContext.thanhtuus.ToListAsync();
                List<CHITIETTHANHTUU> chitietthanhtuus = await dBContext.chitietthanhtuus.ToListAsync();
                foreach (var thanhtuu in thanhtuus)
                {
                    foreach (var item in chitietthanhtuus)
                    {
                        if (item.thanhtuu == thanhtuu)
                        {
                            thanhtuu.CanDelete = false;
                            break;
                        }

                    }
                }
                return await dBContext.thanhtuus.ToListAsync();
            }
            catch(Exception ex)
            {
                return new List<THANHTUU>();
            }
        }

        public async Task<THANHTUU> GetTHANHTUUById(int AchivementId)
        {
            var THANHTUU = await dBContext.thanhtuus.FindAsync(AchivementId);
            return THANHTUU;
        }

        public async Task<bool> UpdateTHANHTUU(THANHTUU thanhtuu)
        {
            try
            {
                dBContext.thanhtuus.Update(thanhtuu);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region UserAchieve
        public async Task<bool> AddUpdateBadge(AchieveBadge achieveBadge)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                CHITIETTHANHTUU chitietthanhtuu = new CHITIETTHANHTUU();
                chitietthanhtuu.UserId = achieveBadge.UserId; /*await dBContext.nguoidungs.FindAsync(achieveBadge.UserId);*/
                chitietthanhtuu.AchieveDate = currentDate;
                chitietthanhtuu.thanhtuu = await dBContext.thanhtuus.FindAsync(achieveBadge.AchievementId);
                await dBContext.chitietthanhtuus.AddAsync(chitietthanhtuu);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool CheckIsExistBadge(int userId, int badgeId)
        {
            var exists = dBContext.chitietthanhtuus
               .Any(ctt => ctt.UserId == userId && ctt.thanhtuu.AchivementId == badgeId);

            return exists;
        }
        public async Task<AchieveStatistics> GetAchieveStatistics(int userId)
        {
            AchieveStatistics achiveStatistics = new AchieveStatistics();
            await GetLibraryStatistics(achiveStatistics, userId);
            await GetClassStatistics(achiveStatistics, userId);
            await GetSequenceStatistics(achiveStatistics, userId);
            return achiveStatistics;
        }

        public async Task<List<Badge>> GetBadges(int userId)
        {
            try
            {
                List<Badge> badgeList = new List<Badge>();
                var badges = await dBContext.thanhtuus.ToListAsync();
                foreach (var b in badges)
                {
                    var badge = new Badge();
                    badge.NameBadge = b.AchivementName;
                    badge.IsAchieved = IsAchieve(b.AchivementId, userId, badge);
                    badge.Image = b.Image;
                    badgeList.Add(badge);
                }
                return badgeList;
            }catch(Exception ex) 
            {
                return new List<Badge>();
            }
          
        }

        public async Task GetClassStatistics(AchieveStatistics statistics, int userId)
        {
            try
            {
                AchieveClass achieve = await client.GetFromJsonAsync<AchieveClass>(Api.Api.ClassAchieveClass + $"{userId}");
                statistics.TotalClass = achieve.TotalClass;
            }
            catch (Exception ex)
            {
                statistics.TotalClass = -1;
            }
        }

        public async Task<int> GetLength()
        {
            try
            {
                var badges = await dBContext.thanhtuus.ToListAsync();
                return badges.Count;
            }
            catch(Exception ex)
            {
                return -1;
            }
           
        }

        public async Task GetLibraryStatistics(AchieveStatistics statistics, int userId)
        {
            try
            {
                AchieveLibrary achieve = await client.GetFromJsonAsync<AchieveLibrary>(Api.Api.TermAchieveLibrary + $"{userId}");
                statistics.NumberTitle = achieve.NumberTitle;
                statistics.NumberModule = achieve.NumberModule;
                statistics.NumberTerms = achieve.NumberTerms;
            }
            catch (Exception ex)
            {
                statistics.NumberTitle = -1;
                statistics.NumberModule = -1;
                statistics.NumberTerms = -1;
            }
        }

        public async Task<int> GetParticipantInAllClass(int userId)
        {
            int count = await client.GetFromJsonAsync<int>(Api.Api.ClassTotalJoin + $"{userId}");
            return count;
        }

        public async Task<List<string>> GetSequenceCalender(int userId)
        {
            var sequenceDates = from a in dBContext.chitietchuois
                                where a.UserId == userId
                                select a.LearningDay.ToString("yyyy-MM-dd");
            return await sequenceDates.ToListAsync();
        }

        public async Task GetSequenceStatistics(AchieveStatistics statistics, int userId)
        {
            var chuois = await dBContext.chitietchuois.Where(a => a.UserId == userId).ToListAsync();
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
            statistics.LongestSquence = maxCount + 1;
        }
        public async Task<AchivementBadge> GetUpdateBadge(int userId, string typeBadge)
        {
            try
            {
                AchieveStatistics achiveStatistics = new AchieveStatistics();
                await GetLibraryStatistics(achiveStatistics, userId);
                var badges = await dBContext.thanhtuus.OrderBy(a => a.Condition).ToListAsync();
                int number = -1;
                if (typeBadge == "participants")
                {
                    number = await GetParticipantInAllClass(userId);
                }
                foreach (var badge in badges)
                {
                    if (badge.AchivementName.Contains(typeBadge))
                    {
                        if (typeBadge == "modules")
                        {
                            if (badge.Condition <= achiveStatistics.NumberModule)
                            {

                                if (!CheckIsExistBadge(userId, badge.AchivementId))
                                {
                                    return new AchivementBadge { AchivementId = badge.AchivementId, AchivementName = badge.AchivementName };
                                }

                            }
                        }
                        if (typeBadge == "terms")
                        {
                            if (badge.Condition <= achiveStatistics.NumberTerms)
                            {

                                if (!CheckIsExistBadge(userId, badge.AchivementId))
                                {
                                    return new AchivementBadge { AchivementId = badge.AchivementId, AchivementName = badge.AchivementName };
                                }

                            }
                        }
                        if (typeBadge == "participants" && number != -1)
                        {
                            if (badge.Condition <= number)
                            {
                                if (!CheckIsExistBadge(userId, badge.AchivementId))
                                {
                                    return new AchivementBadge { AchivementId = badge.AchivementId, AchivementName = badge.AchivementName };
                                }

                            }
                        }
                    }

                }
                return null;
            }catch(Exception ex)
            {
                return null;
            }
         
        }

        public bool IsAchieve(int achievementId, int userId, Badge badge)
        {
            var check = dBContext.chitietthanhtuus.FirstOrDefault(a => a.UserId == userId && a.thanhtuu.AchivementId == achievementId);
            if (check != null)
            {
                badge.DateAchieved = check.AchieveDate.ToString("dd/MM/yyyy");
            }
            return check != null;
        }

        public bool IsMarked(int userId)
        {
            DateTime currentDate = DateTime.Now;
            var value = dBContext.chitietchuois.FirstOrDefault(a => a.UserId == userId && a.LearningDay.Date == currentDate.Date);
            return value != null;
        }

        public async Task<bool> MarkAttendance(MarkAttendance mark)
        {
           try
            {
                if (IsMarked(mark.UserId))
                {
                    return false;
                }
                DateTime currentDate = DateTime.Now;
                CHITIETCHUOI chitietchuoi = new CHITIETCHUOI();
                chitietchuoi.UserId = mark.UserId;
                chitietchuoi.LearningDay = currentDate;
                chitietchuoi.SequenceId = 0;
                await dBContext.chitietchuois.AddAsync(chitietchuoi);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}
