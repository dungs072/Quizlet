using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly TerminologyDBContext dbContext;
        public TitleController(TerminologyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet("user/{UserId}")]
        public async Task<IEnumerable<CHUDE>> GetCHUDEByUserId(int UserId)
        {
            var chudes = dbContext.chudes.Where(e => e.UserId == UserId).ToList();
            foreach(var chude in chudes)
            {
                var HOCPHAN = await dbContext.hocphans.FirstOrDefaultAsync(a => a.TitleId == chude.TitleId);
                if(HOCPHAN!=null)
                {
                    chude.IsEmpty = false;
                }
            }
            return chudes;
        }
        [HttpGet("find/{TitleId}")]
        public async Task<CHUDE> GetCHUDEByTitleId(int TitleId)
        {
            var chude = await dbContext.chudes.FindAsync(TitleId);
            return chude;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCHUDE(CHUDE chude)
        {
            if(HasDuplicateTitleNamePerUserForUpdatee(chude.TitleId,chude.UserId,chude.TitleName))
            {
                return BadRequest();
            }
            dbContext.chudes.Update(chude);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        public bool HasDuplicateTitleNamePerUserForUpdatee(int titleId, int userId, string titleName)
        {
            var chude = dbContext.chudes.FirstOrDefault(u => (u.UserId == userId && u.TitleName == titleName && u.TitleId != titleId));
            return chude != null;
        }
        [HttpGet("check/{userId}/{titleName}")]
        public async Task<ActionResult<bool>> HasDuplicateTitleNamePerUser(int userId, string titleName)
        {
            var chude = dbContext.chudes.FirstOrDefault(u => (u.UserId==userId && u.TitleName == titleName));
            return chude != null;
        }
        [HttpGet("check/{titleId}/{userId}/{titleName}")]
        public async Task<ActionResult<bool>> HasDuplicateTitleNamePerUserForUpdate(int titleId,int userId, string titleName)
        {
            var chude = dbContext.chudes.FirstOrDefault(u => (u.UserId == userId && u.TitleName == titleName && u.TitleId!=titleId));
            return chude != null;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CHUDE>> GetCHUDE()
        {
            return dbContext.chudes;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateCHUDE(CHUDE chude)
        {
            if(HasDuplicateTitleNamePerUserr(chude.UserId,chude.TitleName))
            {
                return BadRequest();
            }
            await dbContext.chudes.AddAsync(chude);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        public bool HasDuplicateTitleNamePerUserr(int userId, string titleName)
        {
            var chude = dbContext.chudes.FirstOrDefault(u => (u.UserId == userId && u.TitleName == titleName));
            return chude != null;
        }
        [HttpDelete("{TitleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCHUDE(int TitleId)
        {
            var CHUDE = await dbContext.chudes.FindAsync(TitleId);
            var HOCPHAN = await dbContext.hocphans.FirstOrDefaultAsync(a=>a.TitleId== TitleId);
            if(HOCPHAN!=null)
            {
                return NoContent();
            }
            dbContext.chudes.Remove(CHUDE);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
