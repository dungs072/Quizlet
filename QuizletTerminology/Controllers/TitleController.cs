using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var chude = dbContext.chudes.Where(e => e.UserId == UserId).ToList();
            return chude;
        }
        [HttpGet("find/{TitleId}")]
        public async Task<CHUDE> GetCHUDEByTitleId(int TitleId)
        {
            var chude = await dbContext.chudes.FindAsync(TitleId);
            return chude;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateCHUDE(CHUDE chude)
        {
            dbContext.chudes.Update(chude);
            await dbContext.SaveChangesAsync();
            return Ok();
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
        public async Task<ActionResult> CreateCHUDE(CHUDE chude)
        {
            await dbContext.chudes.AddAsync(chude);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{TitleId}")]
        public async Task<ActionResult> DeleteCHUDE(int TitleId)
        {
            var CHUDE = await dbContext.chudes.FindAsync(TitleId);
            dbContext.chudes.Remove(CHUDE);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
