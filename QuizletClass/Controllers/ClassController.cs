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
        [HttpGet("{userId}")]
        public async Task<IEnumerable<ClassViewModel>> GetLOP(int userId)
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();
            var lops = await dBContext.lops.Where(e => e.NGUOIDUNG.UserId == userId).ToListAsync();
            foreach(var lop in lops)
            {
                ClassViewModel model = new ClassViewModel();
                model.Copy(lop);
                model.NumberParticipants = lop.chitietdangkilop.Count; //(await GetCHITIETDANGKILOPS(lop.ClassId)).Count;
                model.NumberLearningModules = lop.chitiethocphan.Count; //(await GetCHITIETHOCPHANS(lop.ClassId)).Count;
                classes.Add(model);
            }
            return classes;
        }
        private async Task<List<CHITIETDANGKILOP>> GetCHITIETDANGKILOPS(int classId)
        {
            return await dBContext.chitietdangkilops.Where(a=>a.lop.ClassId==classId).ToListAsync();
        }
        private async Task<List<CHITIETHOCPHAN>> GetCHITIETHOCPHANS(int classId) 
        {
            return await dBContext.chitiethocphans.Where(a => a.lop.ClassId == classId).ToListAsync();
        }
        [HttpGet("find/{ClassId}")]
        public async Task<ActionResult<ClassViewModel>> GetLOPById(int ClassId)
        {
            var LOP = await dBContext.lops.FindAsync(ClassId);
            ClassViewModel classViewModel = new ClassViewModel();
            classViewModel.Copy(LOP);
            return classViewModel;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateLOP(LOP lop)
        {
            if(HasDuplicateClassName(lop.NGUOIDUNG.UserId,lop.ClassName))
            {
                return BadRequest();
            }
            lop.CreatedDate = DateTime.Now;
            lop.NGUOIDUNG = await dBContext.nguoidungs.FindAsync(lop.NGUOIDUNG.UserId);
            await dBContext.lops.AddAsync(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateClassName(int userId, string className)
        {
            var lop = dBContext.lops.FirstOrDefault(u => (u.NGUOIDUNG.UserId == userId && u.ClassName == className));
            return lop != null;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLOP(LOP lop)
        {
            if (HasDuplicateClassNameForUpdate(lop.NGUOIDUNG.UserId,lop.ClassId, lop.ClassName))
            {
                return BadRequest();
            }
            lop.NGUOIDUNG = await dBContext.nguoidungs.FindAsync(lop.NGUOIDUNG.UserId);
            var prevClass = await dBContext.lops.FindAsync(lop.NGUOIDUNG.UserId);
            lop.CreatedDate = prevClass.CreatedDate;
            dBContext.lops.Update(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateClassNameForUpdate(int userId, int classId, string className)
        {
            var cl = dBContext.lops.FirstOrDefault(u => (u.NGUOIDUNG.UserId == userId && u.ClassName == className && u.ClassId != classId));
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
            var hocphans = await dBContext.chitiethocphans.Where(e => e.lop.ClassId == classId).ToListAsync();
            foreach(var item in hocphans)
            {
                var hocphan = item.hocphan;
                ClassLearningModuleViewModel model = new ClassLearningModuleViewModel();
                int count = item.hocphan.thethuatngus.Count;
                model.Copy(item,hocphan,count);
                models.Add(model);
            }
            return models;
        }

        [HttpGet("DetailTitle/{userId}")]
        public IEnumerable<TitleViewModel> GetYourTitleData(int userId)
        {
            var chudes = dBContext.chudes.Where(e=>e.nguoidung.UserId == userId).ToList();
            List<TitleViewModel> titles = new List<TitleViewModel>();
            foreach(var chude in chudes)
            {
                TitleViewModel titleViewModel = new TitleViewModel();
                titleViewModel.Copy(chude);
                titles.Add(titleViewModel);
            }
            return titles;
        }
        [HttpGet("DetailModule/{classId}/{titleId}")]
        public IEnumerable<ModuleDetailWithList> GetYourModuleData(int classId,int titleId)
        {
            List<ModuleDetailWithList> modules = new List<ModuleDetailWithList>();
            var hocphans = dBContext.hocphans.Where(e => e.chude.TitleId == titleId).ToList();
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
            return dBContext.chitiethocphans.FirstOrDefault(a=>a.lop.ClassId==classId && a.hocphan.LearningModuleId==learningModuleId)!=null;
        }

        [HttpPost("ModuleAdd")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddModulesForClass(LearningModuleDetail learningModuleDetail)
        {
            CHITIETHOCPHAN chitiethocphan = new CHITIETHOCPHAN();
            HOCPHAN hocphan = await dBContext.hocphans.FindAsync(learningModuleDetail.LearningModuleId);
            LOP lop = await dBContext.lops.FindAsync(learningModuleDetail.ClassId);
            chitiethocphan.hocphan = hocphan;
            chitiethocphan.lop = lop;
            chitiethocphan.CreatedDate = DateTime.Now;
            await dBContext.chitiethocphans.AddAsync(chitiethocphan);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("ModuleAdd/{classId}/{learningModuleId}")]
        public async Task<ActionResult> DeleteModuleDetail(int classId,int learningModuleId)
        {
            var chitiethocphan = dBContext.chitiethocphans.FirstOrDefault(a=>a.lop.ClassId==classId && a.hocphan.LearningModuleId ==learningModuleId);
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
            var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.lop.ClassId == classId).ToListAsync();
            foreach (var item in chitietdangkilops)
            {
                if (!item.IsAccepted) { continue; }
                var nguoidung = await GetNGUOIDUNG(item.nguoidung.UserId);
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
            var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.lop.ClassId == classId).ToListAsync();
            foreach (var item in chitietdangkilops)
            {
                if (item.IsAccepted) { continue; }
                var nguoidung = await GetNGUOIDUNG(item.nguoidung.UserId);
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
            return dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.nguoidung.UserId == userId) != null;
        }
        [HttpPost("UserParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddParticipant(RegisterClassDetail registerClassDetail)
        {
            CHITIETDANGKILOP chitietdangkilop = new CHITIETDANGKILOP();
            chitietdangkilop.lop = await dBContext.lops.FindAsync(registerClassDetail.ClassId);
            chitietdangkilop.nguoidung = await dBContext.nguoidungs.FindAsync(registerClassDetail.UserId);
            chitietdangkilop.IsAccepted = registerClassDetail.IsAccepted;
            chitietdangkilop.RegisterDate = DateTime.Now;
            await dBContext.chitietdangkilops.AddAsync(chitietdangkilop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("UserParticipant/{classId}/{userId}")]
        public async Task<ActionResult> DeleteUserParticipant(int classId, int userId)
        {
            var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.nguoidung.UserId == userId);
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
            var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.nguoidung.UserId == userId);
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
                    NGUOIDUNG nguoidung = await GetNGUOIDUNG(lop.NGUOIDUNG.UserId);
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
            var result = from a in (from c in dBContext.chudes where c.nguoidung.UserId !=userId select c)
                         join b in dBContext.hocphans on a.TitleId equals b.chude.TitleId
                         select b;
            return result.ToList();
        }
        private async Task<List<LOP>> GetLOPOfModule(int learningModuleId,int userId)
        {
            var result = from a in (from c in dBContext.chitiethocphans where c.hocphan.LearningModuleId == learningModuleId select c)
                         join b in dBContext.lops on a.lop.ClassId equals b.ClassId
                         select b;
            var result2 = from c in dBContext.chitietdangkilops where c.nguoidung.UserId == userId select c;
            var excludedIds = result2.Select(c => c.lop.ClassId);
            var finalResult = result.Where(a => !excludedIds.Contains(a.ClassId));
            return finalResult.ToList();
        }
        private async Task<int> GetNumberTermsInModules(int learningModuleId)
        {
            return (await dBContext.thethuatngus.Where(a=>a.hocphan.LearningModuleId==learningModuleId).ToListAsync()).Count;
        }
        #endregion

        #region JoinClass
        [HttpGet("JoinClass/{userId}")]
        public async Task<IEnumerable<ClassViewModel>> GetJoinClass(int userId)
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();
            var lops = await GetJoinLOP(userId);
            foreach(var lop in lops)
            {
                int numberParticipants = (await GetCHITIETDANGKILOPS(lop.ClassId)).Count;
                int numberModules = (await GetCHITIETHOCPHANS(lop.ClassId)).Count;
                ClassViewModel classView = new ClassViewModel();
                classView.Copy(lop);
                classView.NumberParticipants= numberParticipants;
                classView.NumberLearningModules= numberModules;
                classes.Add(classView);
            }
            return classes;
        }
        public async Task<List<LOP>> GetJoinLOP(int userId)
        {
            var result = from a in dBContext.chitietdangkilops where a.nguoidung.UserId == userId select a.lop.ClassId;
            var result2 = dBContext.lops.Where(a=>result.Contains(a.ClassId));
            return await result2.ToListAsync();
        }
        #endregion
    }
}
