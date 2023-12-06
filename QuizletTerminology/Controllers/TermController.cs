using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using QuizletTerminology.Respository;
using QuizletTerminology.ViewModels;

namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermController : ControllerBase
    {
        private readonly ITermRespository termRespository;
        public TermController(ITermRespository termRespository)
        {
            this.termRespository = termRespository;
        }
        #region Except

        [HttpGet("UserAchieve/{UserId}")]
        public async Task<List<LevelTerms>> GetLevelTerms(int userId)
        {
            var result = await termRespository.GetLevelTerms(userId);
            return result;
        }
        #endregion

        [HttpGet("{learningModuleId}")]
        public IEnumerable<THETHUATNGU> GetTHETHUATNGUByTitleId(int learningModuleId)
        {
            return termRespository.GetTHETHUATNGUByTitleId(learningModuleId);
        }
        [HttpGet("find/{termId}")]
        public async Task<ActionResult<THETHUATNGU>> GetTHUATNGUByLearningModuleId(int termId)
        {
            return await termRespository.GetTHUATNGUByLearningModuleId(termId);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTHUATNGU(THETHUATNGU thuatngu)
        {
            var result = await termRespository.CreateTHUATNGU(thuatngu);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{termId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTHUATNGU(int termId)
        {
            var result = await termRespository.DeleteTHUATNGU(termId);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTHUATNGU(THETHUATNGU thuatngu)
        {
            var result = await termRespository.UpdateTHUATNGU(thuatngu);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("objective/{learningModuleId}")]
        public IEnumerable<ObjectivePack> GetObjectiveList(int learningModuleId)
        {
            var result = termRespository.GetObjectiveList(learningModuleId);
            return result;
            
        }

        [HttpPut("test")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTHUATNGUTest(ResultQuestion resultQuestion)
        {
            var result = await termRespository.UpdateTHUATNGUTest(resultQuestion);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("AchieveLibrary/{userId}")]
        public async Task<AchieveLibrary> GetAchieveLibrary(int userId)
        {
            return await termRespository.GetAchieveLibrary(userId);
        }

        #region Admin
        [HttpGet("Admin/LevelTerm")]
        public async Task<ActionResult<IEnumerable<LEVELGHINHO>>> GetListLEVELGHINHO()
        {
            return await termRespository.GetListLEVELGHINHO();
        }
        [HttpGet("Admin/LevelTerm/{levelId}")]
        public async Task<ActionResult<LEVELGHINHO>> GetLEVELGHINHO(int levelId)
        {
            return await termRespository.GetLEVELGHINHO(levelId);
        }
        [HttpPut("Admin/LevelTerm")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateLEVELGHINHO(LEVELGHINHO level)
        {
            var result = await termRespository.UpdateLEVELGHINHO(level);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}
