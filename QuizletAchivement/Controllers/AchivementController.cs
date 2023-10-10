using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletAchivement.DBContexts;
using QuizletAchivement.Models;

namespace QuizletAchivement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchivementController : ControllerBase
    {
        private readonly AchivementDBContext dBContext;
        public AchivementController(AchivementDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<THANHTUU>> GetTHANHTUU()
        {
            return dBContext.thanhtuus;
        }
        [HttpGet("{AchivementId}")]
        public async Task<ActionResult<THANHTUU>> GetTHANHTUUById(int AchivementId)
        {
            var THANHTUU = await dBContext.thanhtuus.FindAsync(AchivementId);
            return THANHTUU;
        }
        [HttpPost]
        public async Task<ActionResult> CreateTHANHTUU(THANHTUU thanhtuu)
        {

            await dBContext.thanhtuus.AddAsync(thanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateTHANHTUU(THANHTUU thanhtuu)
        {
            dBContext.thanhtuus.Update(thanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{AchivementId}")]
        public async Task<ActionResult> DeleteTHANHTUU(int AchivementId)
        {
            var thanhtuu = await dBContext.nguoidungs.FindAsync(AchivementId);
            dBContext.nguoidungs.Remove(thanhtuu);
            await dBContext.SaveChangesAsync();
            return Ok();
        }
    }
}
