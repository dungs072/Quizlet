using Microsoft.EntityFrameworkCore;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using QuizletTerminology.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace QuizletTerminology.Respository
{
    public class TermRespository:ITermRespository
    {
        private readonly TerminologyDBContext dbContext;
        public TermRespository(TerminologyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region User
        public async Task<bool> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await dbContext.nguoidungs.FindAsync(model.UserId);
            if (user == null) { return false; }
            if (!VerifyPassword(user.Password, model.OldPassword))
            {
                return false;
            }
            else
            {
                user.Password = HashPassword(model.NewPassword);
                dbContext.nguoidungs.Update(user);
                await dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Create(NGUOIDUNG nguoidung)
        {
            try
            {
                nguoidung.Password = HashPassword(nguoidung.Password);
                nguoidung.State = true;
                await dbContext.nguoidungs.AddAsync(nguoidung);

                await dbContext.SaveChangesAsync();
                return true;
            }catch(Exception e)
            {
                return false;
            }
            
        }

        public string CreateGmailCode(string email)
        {
            Random random = new Random();
            string randomDigits = random.Next(100000, 999999).ToString();
            HandleSendingDataToEmail(email, "Your Quizlet email code", "Please enter email code: " + randomDigits + " to register new account");
            return randomDigits;
        }

        public async Task<bool> Delete(int MA_USER)
        {
            try
            {
                var NGUOIDUNG = await dbContext.nguoidungs.FindAsync(MA_USER);
                dbContext.nguoidungs.Remove(NGUOIDUNG);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public string GenerateRandomPassword(int length)
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

        public async Task<NGUOIDUNG> GetByMA_USER(int UserId)
        {
            var NGUOIDUNG = await dbContext.nguoidungs.FindAsync(UserId);
            return NGUOIDUNG;
        }


        public IEnumerable<NGUOIDUNG> GetNGUOIDUNG()
        {
            return dbContext.nguoidungs;
        }

        public async Task<NGUOIDUNG> GetUserByLogin(string Gmail, string Password)
        {
            try
            {
                var NGUOIDUNG = dbContext.nguoidungs.FirstOrDefault(u => (u.Gmail == Gmail));
                if (!NGUOIDUNG.State)
                {
                    return new NGUOIDUNG() { UserId = 123 };
                }
                if (NGUOIDUNG != null && VerifyPassword(NGUOIDUNG.Password, Password))
                {
                    return NGUOIDUNG;
                }
                else
                {
                    return new NGUOIDUNG() { UserId = 0 };
                }
            }
            catch(Exception ex)
            {
                return new NGUOIDUNG() { UserId = 0 };
            }
        }

        public async Task<IEnumerable<UserManagerViewModel>> GetUserManagers()
        {
            List<UserManagerViewModel> usersManagers = new List<UserManagerViewModel>();
            try
            {
                var users = await dbContext.nguoidungs.OrderBy(a => a.FirstName).ToListAsync();

                foreach (var user in users)
                {
                    if (user.TypeAccount == "Admin") { continue; }
                    UserManagerViewModel temp = new UserManagerViewModel();
                    temp.Copy(user);
                    usersManagers.Add(temp);
                }
                return usersManagers;
            }
            catch(Exception ex)
            {
                return usersManagers;
            }
            
        }

        public async Task<bool> HandleForgetPassword(ForgetPasswordViewModel model)
        {
            try
            {
                var user = IsEmailExist(model.Email);
                if (user != null)
                {
                    string randomPassword = GenerateRandomPassword(6);
                    HandleSendingDataToEmail(model.Email, "Quizlet - Forget Password", "Your new password is: " + randomPassword);
                    user.Password = HashPassword(randomPassword);
                    dbContext.nguoidungs.Update(user);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                return false;
            }
           
        }

        public void HandleSendingDataToEmail(string toEmail, string subject, string body)
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

        public async Task<bool> HasDuplicateEmail(string Gmail)
        {
            try
            {
                var NGUOIDUNG = await dbContext.nguoidungs.FirstOrDefaultAsync(u => (u.Gmail == Gmail));
                return NGUOIDUNG != null;
            }catch
            {
                return true;
            }
          
        }

        public string HashPassword(string password)
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

        public NGUOIDUNG IsEmailExist(string email)
        {
            var NGUOIDUNG = dbContext.nguoidungs.FirstOrDefault(u => (u.Gmail == email));
            return NGUOIDUNG;
        }

        public async Task<bool> Update(NGUOIDUNG nguoidung)
        {
            try
            {
                nguoidung.State = true;
                dbContext.nguoidungs.Update(nguoidung);
                await dbContext.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
           
        }

    
        public async Task<bool> UpdateUserState(UserState user)
        {
            try
            {
                var userState = await dbContext.nguoidungs.FindAsync(user.UserId);
                if (userState == null) { return false; }
                userState.State = user.State;
                dbContext.nguoidungs.Update(userState);
                await dbContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
          
        }

        public bool VerifyPassword(string hashedPassword, string userInput)
        {
            return HashPassword(userInput) == hashedPassword;
        }
        #endregion

        #region Title
        public async Task<bool> CreateCHUDE(CHUDE chude)
        {
            try
            {
                var result = await HasDuplicateTitleNamePerUser(chude.UserId, chude.TitleName);
                if (result)
                {
                    return false;
                }
                await dbContext.chudes.AddAsync(chude);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }
        public async Task<bool> DeleteCHUDE(int TitleId)
        {
            try
            {
                var CHUDE = await dbContext.chudes.FindAsync(TitleId);
                var HOCPHAN = await dbContext.hocphans.FirstOrDefaultAsync(a => a.TitleId == TitleId);
                if (HOCPHAN != null)
                {
                    return false;
                }
                dbContext.chudes.Remove(CHUDE);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }
        public IEnumerable<CHUDE> GetCHUDE()
        {
            return dbContext.chudes;
        }

        public async Task<CHUDE> GetCHUDEByTitleId(int TitleId)
        {
            try
            {
                var chude = await dbContext.chudes.FindAsync(TitleId);
                return chude;
            }
            catch(Exception ex)
            {
                return new CHUDE();
            }
            
        }

        public async Task<IEnumerable<CHUDE>> GetCHUDEByUserId(int UserId)
        {
            try
            {
                var chudes = dbContext.chudes.Where(e => e.UserId == UserId).ToList();
                foreach (var chude in chudes)
                {
                    var HOCPHAN = await dbContext.hocphans.FirstOrDefaultAsync(a => a.TitleId == chude.TitleId);
                    if (HOCPHAN != null)
                    {
                        chude.IsEmpty = false;
                    }
                }
                return chudes;
            }catch(Exception ex)
            {
                return Enumerable.Empty<CHUDE>();
            }
           
        }
        public async Task<bool> UpdateCHUDE(CHUDE chude)
        {
            try
            {
                if (HasDuplicateTitleNamePerUserForUpdatee(chude.TitleId, chude.UserId, chude.TitleName))
                {
                    return false;
                }
                dbContext.chudes.Update(chude);
                await dbContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool HasDuplicateTitleNamePerUserForUpdatee(int titleId, int userId, string titleName)
        {
            var chude = dbContext.chudes.FirstOrDefault(u => (u.UserId == userId && u.TitleName == titleName && u.TitleId != titleId));
            return chude != null;
        }

        public async Task<bool> HasDuplicateTitleNamePerUser(int userId, string titleName)
        {
            try
            {
                var chude = await dbContext.chudes.FirstOrDefaultAsync(u => (u.UserId == userId && u.TitleName == titleName));
                return chude != null;
            }catch (Exception ex)
            {
                return true;
            }
           
        }

        public async Task<bool> HasDuplicateTitleNamePerUserForUpdate(int titleId, int userId, string titleName)
        {
            try
            {
                var chude = await dbContext.chudes.FirstOrDefaultAsync(u => (u.UserId == userId && u.TitleName == titleName && u.TitleId != titleId));
                return chude != null;
            }
            catch(Exception ex)
            {
                return true;
            }
        }

        #endregion

        #region Module
        public IEnumerable<ClassLearningModuleViewModel> GetHOCPHANByListId(LearningModuleIdList idList)
        {
            List<ClassLearningModuleViewModel> models = new List<ClassLearningModuleViewModel>();
            try
            {
               
                var hocphans = dbContext.hocphans.Where(a => idList.Ids.Contains(a.LearningModuleId)).ToList();

                foreach (var hocphan in hocphans)
                {
                    ClassLearningModuleViewModel model = new ClassLearningModuleViewModel();
                    models.Add(model);
                    foreach (var date in idList.CreatedDates)
                    {
                        if (date.Ids == hocphan.LearningModuleId)
                        {
                            model.AddedDate = date.CreatedDates;
                        }
                    }
                    model.LearningModuleId = hocphan.LearningModuleId;
                    model.LearningModuleName = hocphan.LearningModuleName;
                    model.NumberTerms = CountTerms(hocphan.LearningModuleId).Result;
                }
                return models;
            }
            catch (Exception ex)
            {
                return models;
            }
        }

        public async Task<int> CountTerms(int learningModuleId)
        {
            try
            {
                return await dbContext.thethuatngus.CountAsync(a => a.LearningModuleId == learningModuleId);
            }catch(Exception ex)
            {
                return -1;
            }
            
        }

        public IEnumerable<HOCPHAN> GetHOCPHANByTitleId(int TitleId)
        {
            try
            {
                var hocphans = dbContext.hocphans.Where(e => e.TitleId == TitleId).ToList();
                foreach (var hocphan in hocphans)
                {
                    var terms = dbContext.thethuatngus.Where(e => e.LearningModuleId == hocphan.LearningModuleId).ToList();
                    hocphan.NumberTerms = terms.Count;
                }
                return hocphans;
            }
            catch(Exception ex)
            {
                return Enumerable.Empty<HOCPHAN>();
            }
        }

        public async Task<HOCPHAN> GetHOCPHANByLearningModuleId(int learningModuleId)
        {
            try
            {
                var hocphan = await dbContext.hocphans.FindAsync(learningModuleId);

                return hocphan;
            }catch(Exception ex)
            {
                return null;
            }
          
        }

        public async Task<bool> CreateHOCPHAN(HOCPHAN hocphan)
        {
            try
            {
                if (HasDuplicatedTitleNamePerUser(hocphan.TitleId, hocphan.LearningModuleName))
                {
                    return false;
                }

                await dbContext.hocphans.AddAsync(hocphan);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool HasDuplicatedTitleNamePerUser(int titleId, string learningModuleName)
        {
            var hocphan = dbContext.hocphans.FirstOrDefault(u => (u.TitleId == titleId && u.LearningModuleName == learningModuleName));
            return hocphan != null;
        }

        public async Task<bool> DeleteHOCPHAN(int learningModuleId)
        {
            try
            {
                if (HasTerminologies(learningModuleId))
                {
                    return false;
                }
                var HOCPHAN = await dbContext.hocphans.FindAsync(learningModuleId);
                dbContext.hocphans.Remove(HOCPHAN);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool HasTerminologies(int learningModuleId)
        {
            var thuatngu = dbContext.thethuatngus.FirstOrDefault(u => (u.LearningModuleId == learningModuleId));
            return thuatngu != null;
        }

        public async Task<bool> UpdateHOCPHAN(HOCPHAN hocphan)
        {
            try
            {
                var result = HasDuplicateModuleNamePerUserForUpdate(hocphan.LearningModuleId, hocphan.TitleId, hocphan.LearningModuleName);
                if (result)
                {
                    return false;
                }
                dbContext.hocphans.Update(hocphan);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool HasDuplicateModuleNamePerUserForUpdate(int learningModuleId, int titleId, string learningModuleName)
        {
            var hocphan = dbContext.hocphans.FirstOrDefault(u => (u.TitleId == titleId && u.LearningModuleName == learningModuleName && u.LearningModuleId != learningModuleId));
            return hocphan != null;
        }

        public async Task<List<HOCPHAN>> GetHOCPHANOfUser(int userId)
        {
            try
            {
                var result = from a in (from c in dbContext.chudes where c.UserId != userId select c)
                             join b in dbContext.hocphans on a.TitleId equals b.TitleId
                             select b;
                return result.ToList();
            }
            catch(Exception ex) 
            { 
                return new List<HOCPHAN> { };
            }
        }

        public async Task<int> CopyModule(CopyViewModel model)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                HOCPHAN hocphan = await dbContext.hocphans.FindAsync(model.ModuleId);
                if (hocphan == null)
                {
                    return 2; //bad request
                }
                if (CheckDuplicateModuleName(model.TitleId, hocphan))
                {
                    return 1; // no content
                }
                HOCPHAN module = new HOCPHAN();
                List<THETHUATNGU> thethuatngus = new List<THETHUATNGU>();
                CopyLearningModule(hocphan, module);
                var terms = await dbContext.thethuatngus.Where(a => a.LearningModuleId == model.ModuleId).ToListAsync();
                foreach (var thuatngu in terms)
                {
                    THETHUATNGU temp = new THETHUATNGU();
                    thethuatngus.Add(temp);
                    temp.LearningModuleId = module.LearningModuleId;
                    CopyTerminology(thuatngu, temp);
                }
                module.TitleId = model.TitleId;

                await dbContext.hocphans.AddAsync(module);
                await dbContext.thethuatngus.AddRangeAsync(thethuatngus);

                //dbContext.chudes.Update(chude);

                await dbContext.SaveChangesAsync();
                transaction.Commit();
                return 0; // Ok()
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 2; // bad request
            }
        }

        public bool CheckDuplicateModuleName(int titleId, HOCPHAN hocphan)
        {
            foreach (var hp in dbContext.hocphans.Where(a => a.TitleId == titleId))
            {
                if (hp.LearningModuleName.Trim() == hocphan.LearningModuleName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyLearningModule(HOCPHAN fromHOCPHAN, HOCPHAN toHOCPHAN)
        {
            toHOCPHAN.LearningModuleName = fromHOCPHAN.LearningModuleName;
            toHOCPHAN.Describe = fromHOCPHAN.Describe;
        }

        public void CopyTerminology(THETHUATNGU fromTHETHUATNGU, THETHUATNGU toTHETHUATNGU)
        {
            toTHETHUATNGU.TermName = fromTHETHUATNGU.TermName;
            toTHETHUATNGU.Explaination = fromTHETHUATNGU.Explaination;
        }
        #endregion

    }
}
