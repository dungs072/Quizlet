using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletWebAPI.DBContexts;
using QuizletWebAPI.Models;

namespace QuizletWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CHUDEController : ControllerBase
    {
        private readonly ApplicationDBContext applicationDBContext;
        public CHUDEController(ApplicationDBContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CHUDE>> GetCHUDE()
        {
            return applicationDBContext.chudes;
        }
        [HttpGet("UserId")]
        public ActionResult<IEnumerable<CHUDE>> GetCHUDEByUserId(int UserId)
        {
            return applicationDBContext.chudes
            .Where(t => t.UserId == UserId)
            .ToList();
        }
    }
}
