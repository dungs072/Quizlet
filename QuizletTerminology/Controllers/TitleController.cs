using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using QuizletTerminology.Respository;
namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly ITermRespository termRespository;
        public TitleController(ITermRespository termRespository)
        {
            this.termRespository = termRespository;
        }
        [HttpGet("user/{UserId}")]
        public async Task<IEnumerable<CHUDE>> GetCHUDEByUserId(int UserId)
        {
            var chudes = await termRespository.GetCHUDEByUserId(UserId);
            return chudes;
        }
        [HttpGet("find/{TitleId}")]
        public async Task<CHUDE> GetCHUDEByTitleId(int TitleId)
        {
            var chude = await termRespository.GetCHUDEByTitleId(TitleId);
            return chude;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCHUDE(CHUDE chude)
        {
            var result = await termRespository.UpdateCHUDE(chude);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("check/{userId}/{titleName}")]
        public async Task<ActionResult<bool>> HasDuplicateTitleNamePerUser(int userId, string titleName)
        {
            return await termRespository.HasDuplicateTitleNamePerUser(userId, titleName);
        }
        [HttpGet("check/{titleId}/{userId}/{titleName}")]
        public async Task<ActionResult<bool>> HasDuplicateTitleNamePerUserForUpdate(int titleId,int userId, string titleName)
        {
            return await termRespository.HasDuplicateTitleNamePerUserForUpdate(titleId, userId, titleName);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CHUDE>> GetCHUDE()
        {
            return termRespository.GetCHUDE().ToList();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateCHUDE(CHUDE chude)
        {
            var result = await termRespository.CreateCHUDE(chude);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{TitleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCHUDE(int TitleId)
        {
            var result = await termRespository.DeleteCHUDE(TitleId);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
    }
}
