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
        private readonly HttpClient client;
        public ClassController(ClassDBContext dBContext, HttpClient client)
        {
            this.dBContext = dBContext;
            this.client = client;
        }
        #region ForService
        [HttpGet("AchieveClass/{userId}")]
        public async Task<AchieveClass> GetAchieveClass(int userId)
        {
            AchieveClass achieveClass = new AchieveClass();
            achieveClass.TotalClass = await dBContext.lops.Where(a => a.UserId == userId).CountAsync();
            return achieveClass;
        }
        [HttpGet("TotalAttendee/{userId}")]
        public async Task<int> CountAllParticipants(int userId)
        {
            int count = await dBContext.chitietdangkilops.CountAsync(a => a.lop.UserId == userId);
            return count;
        }
        #endregion
        
        #region Class
        [HttpGet("{userId}")]
        public async Task<IEnumerable<ClassViewModel>> GetLOP(int userId)
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();
            var lops = await dBContext.lops.Where(e => e.UserId == userId).ToListAsync();
            foreach(var lop in lops)
            {
                ClassViewModel model = new ClassViewModel();
                model.Copy(lop);
                model.NumberParticipants = (lop.chitietdangkilop.Where(a=>a.IsAccepted)).Count();
                model.NumberLearningModules = lop.chitiethocphan.Count;
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
        public async Task<ActionResult> CreateLOP(ClassViewModel classView)
        {

            if(HasDuplicateClassName(classView.UserId,classView.ClassName))
            {
                return BadRequest();
            }
            LOP lop = new LOP();
            UserViewModel nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{classView.UserId}");
            lop.UserId = nguoidung.UserId;
            lop.CreatedDate = DateTime.Now;
            lop.ClassName = classView.ClassName;
            lop.Describe = classView.Describe;
            lop.ClassId = classView.ClassId;  
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
        public async Task<ActionResult> UpdateLOP(ClassViewModel lop)
        {
            if (HasDuplicateClassNameForUpdate(lop.UserId,lop.ClassId, lop.ClassName))
            {
                return BadRequest();
            }
            var prevClass = await dBContext.lops.FindAsync(lop.ClassId);
            prevClass.ClassName = lop.ClassName;
            prevClass.Describe = lop.Describe; 
            dBContext.lops.Update(prevClass);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateClassNameForUpdate(int userId, int classId, string className)
        {
            var cl = dBContext.lops.FirstOrDefault(u => (u.ClassId != classId && u.UserId == userId && u.ClassName == className));
            return cl != null;
        }
        [HttpDelete("{ClassId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteLOP(int ClassId)
        {
            var lop = await dBContext.lops.FindAsync(ClassId);
            int NumberParticipants = lop.chitietdangkilop.Count();
            if(NumberParticipants>0)
            {
                return NoContent();
            }
            int NumberLearningModules = lop.chitiethocphan.Count;
            if(NumberLearningModules>0)
            {
                return NoContent();
            }
            dBContext.lops.Remove(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region DetailLearningModuleClass
        [HttpGet("DetailOwnClass/{classId}")]
        public async Task<IEnumerable<ClassLearningModuleViewModel>> GetLearningModuleClassDetail(int classId)
        {
            LearningModuleIdList listId = new LearningModuleIdList();
            var hocphans = await dBContext.chitiethocphans.Where(e => e.lop.ClassId == classId).ToListAsync();
            foreach(var item in hocphans)
            {
                listId.Ids.Add(item.LearningModuleId);
                LearningModuleClass model = new LearningModuleClass();
                model.CreatedDates = item.CreatedDate;
                model.Ids = item.LearningModuleId;
                listId.CreatedDates.Add(model);
            }
            HttpResponseMessage response = await client.PostAsJsonAsync(Api.Api.LearningModuleClassUrl,listId);
            if (response.IsSuccessStatusCode)
            {
                List<ClassLearningModuleViewModel> models = await response.Content.ReadFromJsonAsync<List<ClassLearningModuleViewModel>>();
                return models;
   
            }
            else
            {
                return new List<ClassLearningModuleViewModel>();
            }
            
        }

        [HttpGet("DetailTitle/{userId}")]
        public IEnumerable<TitleViewModel> GetYourTitleData(int userId)
        {
            try
            {
                var chudes = client.GetFromJsonAsync<List<TitleViewModel>>(Api.Api.TitleBaseUserUrl + $"{userId}").Result;
                return chudes;
            }
            catch(Exception ex)
            {
                return new List<TitleViewModel>();
            }
            
        }
        [HttpGet("DetailModule/{classId}/{titleId}")]
        public async Task<IEnumerable<ModuleDetailWithList>> GetYourModuleData(int classId,int titleId)
        {
            List<ModuleDetailWithList> modules = new List<ModuleDetailWithList>();
            //var hocphans = dBContext.hocphans.Where(e => e.chude.TitleId == titleId).ToList();
            try
            {
                var hocphans = await client.GetFromJsonAsync<List<LearningModuleViewModel>>(Api.Api.LearningModuleUrl + $"/{titleId}");
                foreach (var item in hocphans)
                {
                    ModuleDetailWithList module = new ModuleDetailWithList();
                    module.Copy(item, titleId);
                    modules.Add(module);
                    var check = CheckLearningModuleIsRegistered(classId, item.LearningModuleId);
                    module.IsChoose = check;
                }
                return modules;
            }
            catch (HttpRequestException ex)
            {
                return modules;
            }
          
        }
        private bool CheckLearningModuleIsRegistered(int classId,int learningModuleId)
        {
            return dBContext.chitiethocphans.FirstOrDefault(a=>a.lop.ClassId==classId && a.LearningModuleId==learningModuleId)!=null;
        }

        [HttpPost("ModuleAdd")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddModulesForClass(LearningModuleDetail learningModuleDetail)
        {
            CHITIETHOCPHAN chitiethocphan = new CHITIETHOCPHAN();
            LOP lop = await dBContext.lops.FindAsync(learningModuleDetail.ClassId);
            chitiethocphan.LearningModuleId = learningModuleDetail.LearningModuleId;
            chitiethocphan.lop = lop;
            chitiethocphan.CreatedDate = DateTime.Now;
            await dBContext.chitiethocphans.AddAsync(chitiethocphan);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("ModuleAdd/{classId}/{learningModuleId}")]
        public async Task<ActionResult> DeleteModuleDetail(int classId,int learningModuleId)
        {
            var chitiethocphan = dBContext.chitiethocphans.FirstOrDefault(a=>a.lop.ClassId==classId && a.LearningModuleId ==learningModuleId);
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
                var nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{item.UserId}"); //GetNGUOIDUNG(item.nguoidung.UserId);
                Participant model = new Participant();
                model.Copy(item);
                model.Gmail = nguoidung.Gmail;
                model.FirstName = nguoidung.FirstName;
                model.LastName = nguoidung.LastName;
                model.Image = nguoidung.Image;
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
                var nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{item.UserId}");
                Participant model = new Participant();
                model.Copy(item);
                model.Gmail = nguoidung.Gmail;
                model.FirstName = nguoidung.FirstName;
                model.LastName = nguoidung.LastName;
                model.Image = nguoidung.Image;
                models.Add(model);
            }
            return models;
        }
        [HttpGet("MessagePendingParticipant/{userId}")]
        public async Task<IEnumerable<MessageClassRegistration>> GetMessagePendingParticipant(int userId)
        {
            List<MessageClassRegistration> registers = new List<MessageClassRegistration>();
            var chitietdangkilops = await dBContext.chitietdangkilops.Where(e=>e.lop.UserId == userId && !e.IsAccepted).OrderBy(e=>e.lop.ClassName).ToListAsync();
            foreach(var item in chitietdangkilops)
            {
                MessageClassRegistration register = new MessageClassRegistration();
                var user = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{item.UserId}");
                register.NameRegister = user.FirstName;
                register.ImageUrl = user.Image;
                register.ClassName = item.lop.ClassName;
                register.ClassId = item.lop.ClassId;
                register.DateRegister = item.RegisterDate.ToString("dd/MM/yyyy");
                registers.Add(register);
            }
            return registers;
        }
        [HttpGet("SearchUser/{classId}/{search}/{currentUserId}")]
        public async Task<IEnumerable<UserParticipant>> GetParticipants(int classId,string search,int currentUserId)
        {
            var models = new List<UserParticipant>();

            List<UserViewModel> nguoidungs = await client.GetFromJsonAsync<List<UserViewModel>>(Api.Api.UserUrl);
            foreach (var nguoidung in nguoidungs)
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
            return dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.UserId == userId) != null;
        }
        [HttpPost("UserParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddParticipant(RegisterClassDetail registerClassDetail)
        {
            CHITIETDANGKILOP chitietdangkilop = new CHITIETDANGKILOP();
            chitietdangkilop.lop = await dBContext.lops.FindAsync(registerClassDetail.ClassId);
            chitietdangkilop.UserId = registerClassDetail.UserId;/*nguoidung = await client.GetFromJsonAsync<NGUOIDUNG>(Api.Api.UserUrl + $"/{registerClassDetail.UserId}");*/
            chitietdangkilop.IsAccepted = registerClassDetail.IsAccepted;
            chitietdangkilop.RegisterDate = DateTime.Now;
            await dBContext.chitietdangkilops.AddAsync(chitietdangkilop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("UserParticipant/{classId}/{userId}")]
        public async Task<ActionResult> DeleteUserParticipant(int classId, int userId)
        {
            var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.UserId == userId);
            dBContext.chitietdangkilops.Remove(chitietdangki);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("DetailPendingParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCHITIETDANGKI(RegisterClassDetail registerClassDetail)
        {
            CHITIETDANGKILOP chitietdangkilop = await dBContext.chitietdangkilops.FindAsync(registerClassDetail.RegisterDetailClassId);
            chitietdangkilop.IsAccepted = registerClassDetail.IsAccepted;
            dBContext.chitietdangkilops.Update(chitietdangkilop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("DetailPendingParticipant/{classId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<RegisterClassDetail> GetCHITIETDANGKILOP(int classId, int userId)
        {
            var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.UserId == userId);
            RegisterClassDetail register = new RegisterClassDetail();
            register.Copy(chitietdangki);
            return register;
        }
        #endregion

        #region Register class
        [HttpGet("GlobalSearch/{userId}/{search}")]
        public async Task<IEnumerable<RegisterClass>> GetRegisterClass(int userId,string search)
        {
            // need to improve here
            var models = new List<RegisterClass>();
            List<LearningModuleViewModel> hocphans = await client.GetFromJsonAsync<List<LearningModuleViewModel>>(Api.Api.LearningModuleOfUser + $"{userId}");//await GetHOCPHANOfUser(userId);

            foreach (var hocphan in hocphans)
            {
                if (!hocphan.LearningModuleName.Contains(search,StringComparison.OrdinalIgnoreCase)) { continue; }
                List<LOP> lops = await GetLOPOfModule(hocphan.LearningModuleId,userId);
                foreach(LOP lop in lops)
                {
                    UserViewModel nguoidung =  await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{lop.UserId}");
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
                        NumberTerms = numberTerms,
                        TypeUser = nguoidung.TypeAccount

                    };
                    models.Add(registerClass);
                }

            }
            return models;
        }
        //private async Task<List<HOCPHAN>> GetHOCPHANOfUser(int userId)
        //{
        //    var result = from a in dBContext.hocphans where a.chude.nguoidung.UserId != userId select a;
        //    //var result = from a in (from c in dBContext.chudes where c.nguoidung.UserId !=userId select c)
        //    //             join b in dBContext.hocphans on a.TitleId equals b.chude.TitleId
        //    //             select b;
        //    return result.ToList();
        //}
        private async Task<List<LOP>> GetLOPOfModule(int learningModuleId,int userId)
        {
            //need to improve here
            var result = from a in (from c in dBContext.chitiethocphans where c.LearningModuleId == learningModuleId select c)
                         join b in dBContext.lops on a.lop.ClassId equals b.ClassId
                         select b;
            var result2 = from c in dBContext.chitietdangkilops where c.UserId == userId select c;
            var excludedIds = result2.Select(c => c.lop.ClassId);
            var finalResult = result.Where(a => !excludedIds.Contains(a.ClassId));
            return finalResult.ToList();
        }
        private async Task<int> GetNumberTermsInModules(int learningModuleId)
        {
            return await client.GetFromJsonAsync<int>(Api.Api.LearningModuleCountTerm + $"{learningModuleId}");
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
            var result = from a in dBContext.chitietdangkilops where a.UserId == userId select a.lop.ClassId;
            var result2 = dBContext.lops.Where(a=>result.Contains(a.ClassId));
            return await result2.ToListAsync();
        }
        #endregion

        #region Delete
        [HttpGet("CheckDelete/{learningModuleId}/{userId}")]
        public string CanDeleteLearningModule(int learningModuleId, int userId)
        {
            var data =  dBContext.chitiethocphans.FirstOrDefault(a => a.lop.UserId == userId && a.LearningModuleId == learningModuleId);
            return data==null?"yes":data.lop.ClassName;
        }
        #endregion

    }
}
