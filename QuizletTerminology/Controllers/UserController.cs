using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TerminologyDBContext dbContext;
        public UserController(TerminologyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<NGUOIDUNG>> GetNGUOIDUNG()
        {
            return dbContext.nguoidungs;
        }
        [HttpGet("{UserId}")]
        public async Task<ActionResult<NGUOIDUNG>> GetByMA_USER(int UserId)
        {
            var NGUOIDUNG = await dbContext.nguoidungs.FindAsync(UserId);
            return NGUOIDUNG;
        }
        [HttpGet("{Gmail}/{Password}")]
        public async Task<ActionResult<NGUOIDUNG>> GetUserByLogin(string Gmail, string Password)
        {
            var NGUOIDUNG = dbContext.nguoidungs.FirstOrDefault(u => (u.Gmail == Gmail) && u.Password == Password);
            if(NGUOIDUNG==null)
            {
                return Ok(new NGUOIDUNG());
            }
            return NGUOIDUNG;
        }
        [HttpGet("check/{Gmail}")]
        public async Task<ActionResult<bool>> HasDuplicateEmail(string Gmail)
        {
            var NGUOIDUNG = dbContext.nguoidungs.FirstOrDefault(u => (u.Gmail == Gmail));
            return NGUOIDUNG != null;
        }
        [HttpPost]
        public async Task<ActionResult> Create(NGUOIDUNG nguoidung)
        {

            await dbContext.nguoidungs.AddAsync(nguoidung);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> Update(NGUOIDUNG nguoidung)
        {
            dbContext.nguoidungs.Update(nguoidung);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await dbContext.nguoidungs.FindAsync(model.UserId);
            if(user.Password.Trim()!=model.OldPassword)
            {
                return NoContent();
            }
            else
            {
                user.Password = model.NewPassword;
                dbContext.nguoidungs.Update(user);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            
        }
        [HttpDelete("{MA_USER}")]
        public async Task<ActionResult> Delete(int MA_USER)
        {
            var NGUOIDUNG = await dbContext.nguoidungs.FindAsync(MA_USER);
            dbContext.nguoidungs.Remove(NGUOIDUNG);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
