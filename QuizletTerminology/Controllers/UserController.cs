using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("UserManager")]
        public async Task<ActionResult<IEnumerable<UserManagerViewModel>>> GetUserManagers()
        {
            var users = await dbContext.nguoidungs.OrderBy(a=>a.FirstName).ToListAsync();
            List<UserManagerViewModel> usersManagers = new List<UserManagerViewModel>();
            foreach(var user in users)
            {
                if (user.TypeAccount == "Admin") { continue; }
                UserManagerViewModel temp = new UserManagerViewModel();
                temp.Copy(user);
                usersManagers.Add(temp);
            }
            return usersManagers;
        }
        [HttpPut("UserState")]
        public async Task<ActionResult> UpdateUserState(UserState user)
        {
            var userState = await dbContext.nguoidungs.FindAsync(user.UserId);
            userState.State = user.State;
            dbContext.nguoidungs.Update(userState);
            await dbContext.SaveChangesAsync();
            return Ok();
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
            var NGUOIDUNG = dbContext.nguoidungs.FirstOrDefault(u => (u.Gmail == Gmail));
            if(!NGUOIDUNG.State)
            {
                return Ok(new NGUOIDUNG() { UserId = 123 });
            }
            if (NGUOIDUNG != null && VerifyPassword(NGUOIDUNG.Password, Password))
            {
                return NGUOIDUNG;
            }
            else
            {
                return Ok(new NGUOIDUNG() { UserId = 0});
            }
            
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
            nguoidung.Password = HashPassword(nguoidung.Password);
            nguoidung.State = true;
            await dbContext.nguoidungs.AddAsync(nguoidung);

            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> Update(NGUOIDUNG nguoidung)
        {
            nguoidung.State = true;
            dbContext.nguoidungs.Update(nguoidung);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await dbContext.nguoidungs.FindAsync(model.UserId);
            if(!VerifyPassword(user.Password,model.OldPassword))
            {
                return NoContent();
            }
            else
            {
                user.Password = HashPassword(model.NewPassword);
                dbContext.nguoidungs.Update(user);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            
        }
        static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        static bool VerifyPassword(string hashedPassword, string userInput)
        {
            // Hash the user input and compare it with the stored hashed password
            return HashPassword(userInput) == hashedPassword;
        }

        [HttpDelete("{MA_USER}")]
        public async Task<ActionResult> Delete(int MA_USER)
        {
            var NGUOIDUNG = await dbContext.nguoidungs.FindAsync(MA_USER);
            dbContext.nguoidungs.Remove(NGUOIDUNG);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        private NGUOIDUNG IsEmailExist(string email)
        {
            var NGUOIDUNG = dbContext.nguoidungs.FirstOrDefault(u => (u.Gmail == email));
            return NGUOIDUNG;
        }
        static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            char[] password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }

        #region Handle Email

        [HttpGet("EmailExist/{email}")]
        public ActionResult<string> CreateGmailCode(string email)
        {
            Random random = new Random();
            string randomDigits = random.Next(100000, 999999).ToString();
            HandleSendingDataToEmail(email, "Your Quizlet email code", "Please enter email code: " + randomDigits + " to register new account");
            return randomDigits;
        }
        [HttpPut("ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> HandleForgetPassword(ForgetPasswordViewModel model)
        {
            var user =  IsEmailExist(model.Email);
            if(user!=null)
            {
                string randomPassword = GenerateRandomPassword(6);
                HandleSendingDataToEmail(model.Email, "Quizlet - Forget Password", "Your new password is: " + randomPassword);
                user.Password = HashPassword(randomPassword);
                dbContext.nguoidungs.Update(user);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
        private void HandleSendingDataToEmail(string toEmail, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("dungoc1235@gmail.com", "ohcvbrqttevxjjpp");
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("dungoc1235@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            smtpClient.Send(mailMessage);

        }
        #endregion
    }
}
