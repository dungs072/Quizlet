using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletAchivement.DBContexts;
using QuizletAchivement.Models;
using QuizletAchivement.ViewModels;

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
        #region Achivement

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

        #endregion

        #region UserAchieve
        [HttpGet("UserAchieve/{UserId}")]
        public async Task<List<LevelTerms>> GetLevelTerms(int userId)
        {
            List<LevelTerms> levelTerms = new List<LevelTerms>();
            var levelghinhos = await dBContext.levelghinhos.ToListAsync();
            foreach(var levelghinho in levelghinhos)
            {
                LevelTerms levelterm = new LevelTerms();
                levelterm.LevelName = levelghinho.LevelName;
                levelterm.NumberTermsInLevel = await CountNumberTermsForLevel(levelghinho.LevelId,userId);
                levelTerms.Add(levelterm);
            }
            return levelTerms;
        }
        private async Task<int> CountNumberTermsForLevel(int levelId, int userId)
        {
            var thuatngus = await dBContext.thethuatngus
            .Where(a => a.hocphan.chuDe.nguoiDUNG.UserId == userId && a.levelghinho.LevelId == levelId)
            .ToListAsync();

            return thuatngus.Count;
        }
        #endregion
    }
}
