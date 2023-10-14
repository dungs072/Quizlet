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

        // error there
        #region DetailLearningModuleClass
        [HttpGet("DetailOwnClass/{classId}")]
        public async Task<IEnumerable<ClassLearningModuleViewModel>> GetLearningModuleClassDetail(int classId)
        {
            List<ClassLearningModuleViewModel> models = new List<ClassLearningModuleViewModel>();
            var hocphans = dBContext.chitiethocphans.Where(e => e.ClassId == classId).ToList();
            foreach(var item in hocphans)
            {
                var hocphan = GetHOCPHAN(item.LearningModuleId);
                ClassLearningModuleViewModel model = new ClassLearningModuleViewModel();
                int count = (await GetTHETHUATNGUS(item.LearningModuleId)).Count;
                model.Copy(item,hocphan.Result,count);
                models.Add(model);
            }
            return models;
        }
        private async Task<List<THETHUATNGU>> GetTHETHUATNGUS(int learningModuleId)
        {
            return await dBContext.thethuatngus.Where(a => a.LearningModuleId == learningModuleId).ToListAsync();
        }
        //error above

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
    }
}
