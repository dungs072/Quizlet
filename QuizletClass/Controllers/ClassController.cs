using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletClass.DBContexts;
using QuizletClass.Models;

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

        [HttpGet]
        public ActionResult<IEnumerable<LOP>> GetLOP()
        {
            return dBContext.lops;
        }
        [HttpGet("{ClassId}")]
        public async Task<ActionResult<LOP>> GetLOPById(int ClassId)
        {
            var LOP = await dBContext.lops.FindAsync(ClassId);
            return LOP;
        }
        [HttpPost]
        public async Task<ActionResult> CreateLOP(LOP lop)
        {

            await dBContext.lops.AddAsync(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateLOP(LOP lop)
        {
            dBContext.lops.Update(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{ClassId}")]
        public async Task<ActionResult> DeleteLOP(int ClassId)
        {
            var lop = await dBContext.lops.FindAsync(ClassId);
            dBContext.lops.Remove(lop);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
    }
}
