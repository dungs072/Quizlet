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
    public class LearningModuleController : ControllerBase
    {
        private readonly ITermRespository termRespository;
        public LearningModuleController(ITermRespository termRespository)
        {
            this.termRespository = termRespository;
        }
        [HttpPost("ClassLearningModule")]
        public IEnumerable<ClassLearningModuleViewModel> GetHOCPHANByListId([FromBody]LearningModuleIdList idList)
        {
            var result = termRespository.GetHOCPHANByListId(idList);
            return result;
        }
        [HttpGet("CountTerms/{learningModuleId}")]
        public async Task<int> CountTerms(int learningModuleId)
        {
            return await termRespository.CountTerms(learningModuleId);
        }
        [HttpGet("{TitleId}")]
        public IEnumerable<HOCPHAN> GetHOCPHANByTitleId(int TitleId)
        {
            return termRespository.GetHOCPHANByTitleId(TitleId);
        }
        [HttpGet("find/{learningModuleId}")]
        public async Task<ActionResult<HOCPHAN>> GetHOCPHANByLearningModuleId(int learningModuleId)
        {
            return await termRespository.GetHOCPHANByLearningModuleId(learningModuleId);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateHOCPHAN(HOCPHAN hocphan)
        {
            var result = await termRespository.CreateHOCPHAN(hocphan);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{learningModuleId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteHOCPHAN(int learningModuleId)
        {
            var result = await termRespository.DeleteHOCPHAN(learningModuleId);
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
        public async Task<ActionResult> UpdateHOCPHAN(HOCPHAN hocphan)
        {
            var result = await termRespository.UpdateHOCPHAN(hocphan);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("GetLearningModuleOfUser/{userId}")]
        public async Task<List<HOCPHAN>> GetHOCPHANOfUser(int userId)
        {
            return await termRespository.GetHOCPHANOfUser(userId);
        }

        [HttpPost("CopyModule")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> CopyModule(CopyViewModel model)
        {
            var result = await termRespository.CopyModule(model);
            if(result==2)
            {
                return BadRequest();
            }
            else if(result==1)
            {
                return NoContent();
            }
            else
            {
                return Ok();
            }
        }
    }
}
