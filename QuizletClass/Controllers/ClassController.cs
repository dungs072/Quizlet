using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletClass.DBContexts;
using QuizletClass.Models;
using QuizletClass.ViewModels;

namespace QuizletClass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ClassDBContext dBContext;
        public ClassController(ClassDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        #region Class
        [HttpGet]
        public ActionResult<IEnumerable<ClassViewModel>> GetLOP()
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();
            foreach(var lop in dBContext.lops)
            {
                ClassViewModel model = new ClassViewModel();
                model.Copy(lop);
                model.NumberParticipants = GetCHITIETDANGKILOPS(lop.ClassId).Count;
                model.NumberLearningModules = GetCHITIETHOCPHANS(lop.ClassId).Count;
                classes.Add(model);
            }
            return classes;
        }
        private List<CHITIETDANGKILOP> GetCHITIETDANGKILOPS(int classId)
        {
            return dBContext.chitietdangkilops.Where(a=>a.ClassId==classId).ToList();
        }
        private List<CHITIETHOCPHAN> GetCHITIETHOCPHANS(int classId) 
        {
            return dBContext.chitiethocphans.Where(a => a.ClassId == classId).ToList();
        }
        [HttpGet("{userId}")]
        public IEnumerable<LOP> GetLOPByUserId(int userId)
        {
            var lops = dBContext.lops.Where(e => e.UserId == userId).ToList();
            return lops;
        }
        [HttpGet("find/{ClassId}")]
        public async Task<ActionResult<LOP>> GetLOPById(int ClassId)
        {
            var LOP = await dBContext.lops.FindAsync(ClassId);
            return LOP;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateLOP(LOP lop)
        {
            if(HasDuplicateClassName(lop.UserId,lop.ClassName))
            {
                return BadRequest();
            }
            lop.CreatedDate = DateTime.Now;
            lop.NGUOIDUNG = await dBContext.nguoidungs.FindAsync(lop.UserId);
            await dBContext.lops.AddAsync(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateClassName(int userId, string className)
        {
            var lop = dBContext.lops.FirstOrDefault(u => (u.UserId == userId && u.ClassName == className));
            return lop != null;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLOP(LOP lop)
        {
            if (HasDuplicateClassNameForUpdate(lop.UserId,lop.ClassId, lop.ClassName))
            {
                return BadRequest();
            }
            lop.NGUOIDUNG = await dBContext.nguoidungs.FindAsync(lop.UserId);
            var prevClass = await dBContext.lops.FindAsync(lop.UserId);
            lop.CreatedDate = prevClass.CreatedDate;
            dBContext.lops.Update(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateClassNameForUpdate(int userId, int classId, string className)
        {
            var cl = dBContext.lops.FirstOrDefault(u => (u.UserId == userId && u.ClassName == className && u.ClassId != classId));
            return cl != null;
        }
        [HttpDelete("{ClassId}")]
        public async Task<ActionResult> DeleteLOP(int ClassId)
        {
            var lop = await dBContext.lops.FindAsync(ClassId);
            dBContext.lops.Remove(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region DetailLearningModuleClass
        [HttpGet("DetailOwnClass/{classId}")]
        public async Task<IEnumerable<ClassLearningModuleViewModel>> GetLearningModuleClassDetail(int classId)
        {

            List<ClassLearningModuleViewModel> models = new List<ClassLearningModuleViewModel>();
            var hocphans = await dBContext.chitiethocphans.Where(e => e.ClassId == classId).ToListAsync();
            foreach(var item in hocphans)
            {
                var hocphan = await GetHOCPHAN(item.LearningModuleId);
                ClassLearningModuleViewModel model = new ClassLearningModuleViewModel();
                int count = (await GetTHETHUATNGUS(item.LearningModuleId)).Count;
                model.Copy(item,hocphan,count);
                models.Add(model);
            }
            return models;
        }
        private async Task<List<THETHUATNGU>> GetTHETHUATNGUS(int learningModuleId)
        {
            return await dBContext.thethuatngus.Where(a => a.LearningModuleId == learningModuleId).ToListAsync();
        }

        private async Task<HOCPHAN> GetHOCPHAN(int learningModuleId)
        {
            return await dBContext.hocphans.FindAsync(learningModuleId);
        }

        [HttpGet("DetailTitle/{userId}")]
        public IEnumerable<CHUDE> GetYourTitleData(int userId)
        {
            var chudes = dBContext.chudes.Where(e=>e.UserId == userId).ToList();
            return chudes;
        }
        [HttpGet("DetailModule/{classId}/{titleId}")]
        public IEnumerable<ModuleDetailWithList> GetYourModuleData(int classId,int titleId)
        {
            List<ModuleDetailWithList> modules = new List<ModuleDetailWithList>();
            var hocphans = dBContext.hocphans.Where(e => e.TitleId == titleId).ToList();
            foreach(var item in hocphans)
            {
                ModuleDetailWithList module = new ModuleDetailWithList();
                module.Copy(item);
                modules.Add(module);
                var check = CheckLearningModuleIsRegistered(classId,item.LearningModuleId);
                module.IsChoose = check;
            }
            return modules;
        }
        private bool CheckLearningModuleIsRegistered(int classId,int learningModuleId)
        {
            return dBContext.chitiethocphans.FirstOrDefault(a=>a.ClassId==classId && a.LearningModuleId==learningModuleId)!=null;
        }

        [HttpPost("ModuleAdd")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddModulesForClass(CHITIETHOCPHAN chitiethocphan)
        {
            chitiethocphan.CreatedDate = DateTime.Now;
            await dBContext.chitiethocphans.AddAsync(chitiethocphan);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("ModuleAdd/{classId}/{learningModuleId}")]
        public async Task<ActionResult> DeleteModuleDetail(int classId,int learningModuleId)
        {
            var chitiethocphan = dBContext.chitiethocphans.FirstOrDefault(a=>a.ClassId==classId && a.LearningModuleId ==learningModuleId);
            dBContext.chitiethocphans.Remove(chitiethocphan);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Participants
        [HttpGet("DetailParticipant/{classId}")]
        public async Task<IEnumerable<Participant>> GetParticipant(int classId)
        {
            List<Participant> models = new List<Participant>();
            var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.ClassId == classId).ToListAsync();
            foreach (var item in chitietdangkilops)
            {
                if (!item.IsAccepted) { continue; }
                var nguoidung = await GetNGUOIDUNG(item.UserId);
                Participant model = new Participant();
                model.Copy(item);
                model.Gmail = nguoidung.Gmail;
                model.FirstName = nguoidung.FirstName;
                model.LastName = nguoidung.LastName;
                models.Add(model);
            }
            return models;
        }
        [HttpGet("DetailPendingParticipant/{classId}")]
        public async Task<IEnumerable<Participant>> GetPendingParticipant(int classId)
        {
            List<Participant> models = new List<Participant>();
            var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.ClassId == classId).ToListAsync();
            foreach (var item in chitietdangkilops)
            {
                if (item.IsAccepted) { continue; }
                var nguoidung = await GetNGUOIDUNG(item.UserId);
                Participant model = new Participant();
                model.Copy(item);
                model.Gmail = nguoidung.Gmail;
                model.FirstName = nguoidung.FirstName;
                model.LastName = nguoidung.LastName;
                models.Add(model);
            }
            return models;
        }
        private async Task<NGUOIDUNG> GetNGUOIDUNG(int userId)
        {
            return await dBContext.nguoidungs.FindAsync(userId);
        }
        [HttpGet("SearchUser/{classId}/{search}/{currentUserId}")]
        public async Task<IEnumerable<UserParticipant>> GetParticipants(int classId,string search,int currentUserId)
        {
            var models = new List<UserParticipant>();

            List<NGUOIDUNG> nguoidungs =  dBContext.nguoidungs.ToList<NGUOIDUNG>();
            foreach(var nguoidung in nguoidungs)
            {
                if (!nguoidung.Gmail.Contains(search,StringComparison.OrdinalIgnoreCase)) { continue; }
                if(nguoidung.UserId==currentUserId) { continue; }
                if (CheckUserHasRegisterToClass(classId, nguoidung.UserId))
                {
                    continue;
                }
                UserParticipant user = new UserParticipant();
                user.Copy(nguoidung);
                models.Add(user);
            }
            return models;
        }
        private bool CheckUserHasRegisterToClass(int classId, int userId)
        {
            return dBContext.chitietdangkilops.FirstOrDefault(a => a.ClassId == classId && a.UserId == userId) != null;
        }
        [HttpPost("UserParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddParticipant(CHITIETDANGKILOP chitietdangkilop)
        {
            chitietdangkilop.RegisterDate = DateTime.Now;
            await dBContext.chitietdangkilops.AddAsync(chitietdangkilop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("UserParticipant/{classId}/{userId}")]
        public async Task<ActionResult> DeleteUserParticipant(int classId, int userId)
        {
            var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.ClassId == classId && a.UserId == userId);
            dBContext.chitietdangkilops.Remove(chitietdangki);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("DetailPendingParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCHITIETDANGKI(CHITIETDANGKILOP chitietdangkilop)
        {
            dBContext.chitietdangkilops.Update(chitietdangkilop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("DetailPendingParticipant/{classId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<CHITIETDANGKILOP> GetCHITIETDANGKILOP(int classId, int userId)
        {
            var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.ClassId == classId && a.UserId == userId);
            return chitietdangki;
        }
        #endregion

        #region Register class
        [HttpGet("GlobalSearch/{userId}/{search}")]
        public async Task<IEnumerable<RegisterClass>> GetRegisterClass(int userId,string search)
        {
            var models = new List<RegisterClass>();
            List<HOCPHAN> hocphans = await GetHOCPHANOfUser(userId);
            
            foreach (var hocphan in hocphans)
            {
                if (!hocphan.LearningModuleName.Contains(search,StringComparison.OrdinalIgnoreCase)) { continue; }
                List<LOP> lops = await GetLOPOfModule(hocphan.LearningModuleId,userId);
                foreach(LOP lop in lops)
                {
                    NGUOIDUNG nguoidung = await GetNGUOIDUNG(lop.UserId);
                    int numberTerms = await GetNumberTermsInModules(hocphan.LearningModuleId);
                    if (numberTerms == 0) { continue; }
                    RegisterClass registerClass = new RegisterClass()
                    {
                        ClassId = lop.ClassId,
                        ClassName = lop.ClassName,
                        ClassDescribe = lop.Describe,
                        LearningModuleId = hocphan.LearningModuleId,
                        LearningModuleName = hocphan.LearningModuleName,
                        OwnerFullName = nguoidung.LastName + " " + nguoidung.FirstName,
                        NumberTerms = numberTerms

                    };
                    models.Add(registerClass);
                }

            }
            return models;
        }
        private async Task<List<HOCPHAN>> GetHOCPHANOfUser(int userId)
        {
            var result = from a in (from c in dBContext.chudes where c.UserId !=userId select c)
                         join b in dBContext.hocphans on a.TitleId equals b.TitleId
                         select b;
            return result.ToList();
        }
        private async Task<List<LOP>> GetLOPOfModule(int learningModuleId,int userId)
        {
            var result = from a in (from c in dBContext.chitiethocphans where c.LearningModuleId == learningModuleId select c)
                         join b in dBContext.lops on a.ClassId equals b.ClassId
                         select b;
            var result2 = from c in dBContext.chitietdangkilops where c.UserId == userId select c;
            var excludedIds = result2.Select(c => c.ClassId);
            var finalResult = result.Where(a => !excludedIds.Contains(a.ClassId));
            return finalResult.ToList();
        }
        private async Task<int> GetNumberTermsInModules(int learningModuleId)
        {
            return (await dBContext.thethuatngus.Where(a=>a.LearningModuleId==learningModuleId).ToListAsync()).Count;
        }
        #endregion
    }
}
