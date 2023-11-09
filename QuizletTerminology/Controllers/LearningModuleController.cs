using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;

namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningModuleController : ControllerBase
    {
        private readonly TerminologyDBContext dbContext;
        public LearningModuleController(TerminologyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet("{TitleId}")]
        public IEnumerable<HOCPHAN> GetHOCPHANByTitleId(int TitleId)
        {
            var hocphan =  dbContext.hocphans.Where(e => e.TitleId == TitleId).ToList();
            
            return hocphan;
        }
        [HttpGet("find/{learningModuleId}")]
        public async Task<ActionResult<HOCPHAN>> GetHOCPHANByLearningModuleId(int learningModuleId)
        {
            var hocphan = await dbContext.hocphans.FindAsync(learningModuleId);

            return hocphan;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateHOCPHAN(HOCPHAN hocphan)
        {
            if(HasDuplicatedTitleNamePerUser(hocphan.TitleId,hocphan.LearningModuleName))
            {
                return BadRequest();
            }

            await dbContext.hocphans.AddAsync(hocphan);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicatedTitleNamePerUser(int titleId, string learningModuleName)
        {
            var hocphan = dbContext.hocphans.FirstOrDefault(u => (u.TitleId == titleId && u.LearningModuleName == learningModuleName));
            return hocphan != null;
        }
      
        [HttpDelete("{learningModuleId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteHOCPHAN(int learningModuleId)
        {
            if(HasTerminologies(learningModuleId))
            {
                return BadRequest();
            }
            var HOCPHAN = await dbContext.hocphans.FindAsync(learningModuleId);
            dbContext.hocphans.Remove(HOCPHAN);
            await dbContext.SaveChangesAsync();
            return Ok();

        }
        private bool HasTerminologies(int learningModuleId)
        {
            var thuatngu = dbContext.thethuatngus.FirstOrDefault(u => (u.LearningModuleId == learningModuleId));
            return thuatngu != null;

        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateHOCPHAN(HOCPHAN hocphan)
        {
            if(HasDuplicateTitleNamePerUserForUpdate(hocphan.LearningModuleId,hocphan.TitleId,hocphan.LearningModuleName))
            {
                return BadRequest();
            }
            dbContext.hocphans.Update(hocphan);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        private bool HasDuplicateTitleNamePerUserForUpdate(int learningModuleId, int titleId, string learningModuleName)
        {
            var hocphan = dbContext.hocphans.FirstOrDefault(u => (u.TitleId == titleId && u.LearningModuleName == learningModuleName && u.LearningModuleId != learningModuleId));
            return hocphan != null;
        }
    }
}
