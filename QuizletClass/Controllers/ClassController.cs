using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletClass.DBContexts;
using QuizletClass.Models;
using QuizletClass.Respository;
using QuizletClass.ViewModels;

namespace QuizletClass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassRespository classRespository;
        public ClassController(IClassRespository classRespository)
        {
            this.classRespository = classRespository;
        }
        #region ForService
        [HttpGet("AchieveClass/{userId}")]
        public async Task<AchieveClass> GetAchieveClass(int userId)
        {
            return await classRespository.GetAchieveClass(userId);
        }
        [HttpGet("TotalAttendee/{userId}")]
        public async Task<int> CountAllParticipants(int userId)
        {
            return await classRespository.CountAllParticipants(userId);
        }
        #endregion
        
        #region Class
        [HttpGet("{userId}")]
        public async Task<IEnumerable<ClassViewModel>> GetLOP(int userId)
        {
            var result = await classRespository.GetLOP(userId);
            return result;
        }
        private async Task<List<CHITIETDANGKILOP>> GetCHITIETDANGKILOPS(int classId)
        {
            return await classRespository.GetCHITIETDANGKILOPS(classId);
        }
        private async Task<List<CHITIETHOCPHAN>> GetCHITIETHOCPHANS(int classId) 
        {
            return await classRespository.GetCHITIETHOCPHANS(classId);
        }
        [HttpGet("find/{ClassId}")]
        public async Task<ActionResult<ClassViewModel>> GetLOPById(int ClassId)
        {
            return await classRespository.GetLOPById(ClassId);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateLOP(ClassViewModel classView)
        {

            var result = await classRespository.CreateLOP(classView);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLOP(ClassViewModel lop)
        {
            var result = await classRespository.UpdateLOP(lop);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{ClassId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteLOP(int ClassId)
        {
            var result = await classRespository.DeleteLOP(ClassId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region DetailLearningModuleClass
        [HttpGet("DetailOwnClass/{classId}")]
        public async Task<IEnumerable<ClassLearningModuleViewModel>> GetLearningModuleClassDetail(int classId)
        {
            return await classRespository.GetLearningModuleClassDetail(classId);
        }

        [HttpGet("DetailTitle/{userId}")]
        public IEnumerable<TitleViewModel> GetYourTitleData(int userId)
        {
            return classRespository.GetYourTitleData(userId);
        }
        [HttpGet("DetailModule/{classId}/{titleId}")]
        public async Task<IEnumerable<ModuleDetailWithList>> GetYourModuleData(int classId,int titleId)
        {
            return await classRespository.GetYourModuleData(classId, titleId);
        }

        [HttpPost("ModuleAdd")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddModulesForClass(LearningModuleDetail learningModuleDetail)
        {
            var result = await classRespository.AddModulesForClass(learningModuleDetail);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("ModuleAdd/{classId}/{learningModuleId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteModuleDetail(int classId,int learningModuleId)
        {
            var result = await classRespository.DeleteModuleDetail(classId, learningModuleId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

        #region Participants
        [HttpGet("DetailParticipant/{classId}")]
        public async Task<IEnumerable<Participant>> GetParticipant(int classId)
        {
            return await classRespository.GetParticipant(classId);
        }
        [HttpGet("DetailPendingParticipant/{classId}")]
        public async Task<IEnumerable<Participant>> GetPendingParticipant(int classId)
        {
            return await classRespository.GetPendingParticipant(classId);
        }
        [HttpGet("MessagePendingParticipant/{userId}")]
        public async Task<IEnumerable<MessageClassRegistration>> GetMessagePendingParticipant(int userId)
        {
            return await classRespository.GetMessagePendingParticipant(userId);
        }
        [HttpGet("SearchUser/{classId}/{search}/{currentUserId}")]
        public async Task<IEnumerable<UserParticipant>> GetParticipants(int classId,string search,int currentUserId)
        {
            return await classRespository.GetParticipants(classId,search,currentUserId);
        }
        [HttpPost("UserParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddParticipant(RegisterClassDetail registerClassDetail)
        {
            var result = await classRespository.AddParticipant(registerClassDetail);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("UserParticipant/{classId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserParticipant(int classId, int userId)
        {
            var result = await classRespository.DeleteUserParticipant(classId,userId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("DetailPendingParticipant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCHITIETDANGKI(RegisterClassDetail registerClassDetail)
        {
            var result = await classRespository.UpdateCHITIETDANGKI(registerClassDetail);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("DetailPendingParticipant/{classId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<RegisterClassDetail> GetCHITIETDANGKILOP(int classId, int userId)
        {
            return await classRespository.GetCHITIETDANGKILOP(classId, userId);
        }
        #endregion

        #region Register class
        [HttpGet("GlobalSearch/{userId}/{search}")]
        public async Task<IEnumerable<RegisterClass>> GetRegisterClass(int userId,string search)
        {
           return await classRespository.GetRegisterClass(userId,search);
        }
        #endregion

        #region JoinClass
        [HttpGet("JoinClass/{userId}")]
        public async Task<IEnumerable<ClassViewModel>> GetJoinClass(int userId)
        {
            return await classRespository.GetJoinClass(userId);
        }
        #endregion

        #region Delete
        [HttpGet("CheckDelete/{learningModuleId}/{userId}")]
        public string CanDeleteLearningModule(int learningModuleId, int userId)
        {
            return classRespository.CanDeleteLearningModule(learningModuleId, userId);
        }
        #endregion

    }
}
