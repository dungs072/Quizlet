using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletWebAPI.DBContexts;
using QuizletWebAPI.Models;

namespace QuizletWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NGUOIDUNGController : ControllerBase
    {
        private readonly ApplicationDBContext applicationDBContext;
        public NGUOIDUNGController(ApplicationDBContext applicationDBContext) 
        {
            this.applicationDBContext = applicationDBContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<NGUOIDUNG>> GetNGUOIDUNG()
        {
            return applicationDBContext.nguoidungs;
        }
        [HttpGet("{UserId}")]
        public async Task<ActionResult<NGUOIDUNG>> GetByMA_USER(int UserId)
        {
            var NGUOIDUNG = await applicationDBContext.nguoidungs.FindAsync(UserId);
            return NGUOIDUNG;
        }
        [HttpGet("{Gmail}/{Password}")]
        public async Task<ActionResult<NGUOIDUNG>> GetUserByLogin(string Gmail, string Password)
        {
            var NGUOIDUNG =  applicationDBContext.nguoidungs.FirstOrDefault(u => (u.Gmail == Gmail) && u.Password == Password);
            return NGUOIDUNG;
        }
        [HttpGet("check/{Gmail}")]
        public async Task<ActionResult<bool>> HasDuplicateEmail(string Gmail)
        {
            var NGUOIDUNG = applicationDBContext.nguoidungs.FirstOrDefault(u => (u.Gmail == Gmail));
            return NGUOIDUNG!=null;
        }
        [HttpPost]
        public async Task<ActionResult> Create(NGUOIDUNG nguoidung)
        {

            await applicationDBContext.nguoidungs.AddAsync(nguoidung);
            await applicationDBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> Update(NGUOIDUNG nguoidung)
        {
            applicationDBContext.nguoidungs.Update(nguoidung);
            await applicationDBContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{MA_USER}")]
        public async Task<ActionResult> Delete(int MA_USER)
        {
            var NGUOIDUNG = await applicationDBContext.nguoidungs.FindAsync(MA_USER);
            applicationDBContext.nguoidungs.Remove(NGUOIDUNG);
            await applicationDBContext.SaveChangesAsync();
            return Ok();
        }


    }
}
