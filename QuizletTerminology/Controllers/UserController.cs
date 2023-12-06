using Microsoft.AspNetCore.Mvc;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using QuizletTerminology.ViewModels;
using QuizletTerminology.Respository;

namespace QuizletTerminology.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITermRespository termRespository;
        public UserController(ITermRespository termRespository)
        {
            this.termRespository = termRespository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<NGUOIDUNG>> GetNGUOIDUNG()
        {
            return termRespository.GetNGUOIDUNG().ToList();
        }
        [HttpGet("UserManager")]
        public async Task<ActionResult<IEnumerable<UserManagerViewModel>>> GetUserManagers()
        {
            var userManagers = await termRespository.GetUserManagers();
            return userManagers;
        }
        [HttpPut("UserState")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateUserState(UserState user)
        {
            var result = await termRespository.UpdateUserState(user);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet("{UserId}")]
        public async Task<ActionResult<NGUOIDUNG>> GetByMA_USER(int UserId)
        {
            var nguoidung = await termRespository.GetByMA_USER(UserId);
            return nguoidung;
        }
        [HttpGet("{Gmail}/{Password}")]
        public async Task<ActionResult<NGUOIDUNG>> GetUserByLogin(string Gmail, string Password)
        {
            var NGUOIDUNG = await termRespository.GetUserByLogin(Gmail, Password);
            return Ok(NGUOIDUNG);
        }

        [HttpGet("check/{Gmail}")]
        public async Task<ActionResult<bool>> HasDuplicateEmail(string Gmail)
        {
            var result = await termRespository.HasDuplicateEmail(Gmail);
            return result;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Create(NGUOIDUNG nguoidung)
        {
            var result = await termRespository.Create(nguoidung);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(NGUOIDUNG nguoidung)
        {
            var result = await termRespository.Update(nguoidung);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        [HttpPut("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var result = await termRespository.ChangePassword(model);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
            
        }

        [HttpDelete("{MA_USER}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int MA_USER)
        {
            var result = await termRespository.Delete(MA_USER);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        #region Handle Email

        [HttpGet("EmailExist/{email}")]
        public ActionResult<string> CreateGmailCode(string email)
        {
            string result = termRespository.CreateGmailCode(email);
            return result;
        }
        [HttpPut("ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> HandleForgetPassword(ForgetPasswordViewModel model)
        {
            var result = await termRespository.HandleForgetPassword(model);
            if(result)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
