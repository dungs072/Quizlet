using Microsoft.EntityFrameworkCore;
using QuizletTerminology.DBContexts;
using QuizletTerminology.Models;
using QuizletTerminology.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Firebase.Auth;
using Firebase.Storage;
using System.Reflection;
using System;

namespace QuizletTerminology.Respository
{
    public class TermRespository:ITermRespository
    {
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa123@gmail.com";
        private readonly string password = "123456";

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
                if(NGUOIDUNG==null)
                {
                    return new NGUOIDUNG() { UserId = 0 };
                }
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

        public async Task<List<UserManagerViewModel>> GetUserManagers()
        { 
            try
            {
                var users = await dbContext.nguoidungs.OrderBy(a => a.FirstName).ToListAsync();
                List<UserManagerViewModel> usersManagers = new List<UserManagerViewModel>();
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
                return new List<UserManagerViewModel>();
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

        public async Task<List<CHUDE>> GetCHUDEByUserId(int UserId)
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
                return new List<CHUDE>();
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
            
            try
            {
                List<ClassLearningModuleViewModel> models = new List<ClassLearningModuleViewModel>();

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
                return new List<ClassLearningModuleViewModel>();
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
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HOCPHAN ON");
                HOCPHAN module = new HOCPHAN();
                List<THETHUATNGU> thethuatngus = new List<THETHUATNGU>();
                CopyLearningModule(hocphan, module);
                int maxKey = await dbContext.hocphans.MaxAsync(a => a.LearningModuleId);
                module.LearningModuleId = maxKey + 1;
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
                dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HOCPHAN OFF");
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

        #region Term
        public async Task<List<LevelTerms>> GetLevelTerms(int userId)
        {
            List<LevelTerms> levelTerms = new List<LevelTerms>();
            try
            {
                
                var levelghinhos = await dbContext.levelghinhos.ToListAsync();
                foreach (var levelghinho in levelghinhos)
                {
                    LevelTerms levelterm = new LevelTerms();
                    levelterm.LevelName = levelghinho.LevelName;
                    levelterm.NumberTermsInLevel = await CountNumberTermsForLevel(levelghinho.LevelId, userId);
                    levelTerms.Add(levelterm);
                }
                return levelTerms;
            }
            catch (Exception ex)
            {
                return levelTerms;
            }
        }

        public async Task<int> CountNumberTermsForLevel(int levelId, int userId)
        {
            try
            {
                var titles = dbContext.chudes.Where(a => a.UserId == userId).Select(t => t.TitleId).ToList();
                var modules = dbContext.hocphans
                            .Where(h => titles.Contains(h.TitleId)).Select(t => t.LearningModuleId)
                            .ToList();
                var count = await dbContext.thethuatngus
                .CountAsync(h => modules.Contains(h.LearningModuleId) && h.LevelId == levelId);

                return count;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public IEnumerable<THETHUATNGU> GetTHETHUATNGUByTitleId(int learningModuleId)
        {
            try
            {
                var thuatngu = dbContext.thethuatngus.Where(e => e.LearningModuleId == learningModuleId).ToList();
                return thuatngu;
            }catch (Exception ex)
            {
                return Enumerable.Empty<THETHUATNGU>();
            }
        }

        public async Task<THETHUATNGU> GetTHUATNGUByLearningModuleId(int termId)
        {
            var thuatngu = await dbContext.thethuatngus.FindAsync(termId);

            return thuatngu;
        }

        public async Task<bool> CreateTHUATNGU(THETHUATNGU thuatngu)
        {
            try
            {
                if (HasDuplicatedTermNamePerLearningModule(thuatngu.LearningModuleId, thuatngu.TermName))
                {
                    return false;
                }
                await dbContext.thethuatngus.AddAsync(thuatngu);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool HasDuplicatedTermNamePerLearningModule(int learningModuleId, string termName)
        {
            var thuatngu = dbContext.thethuatngus.FirstOrDefault(u => (u.LearningModuleId == learningModuleId && u.TermName == termName));
            return thuatngu != null;
        }

        public async Task<bool> DeleteTHUATNGU(int termId)
        {
            try
            {
                var THUATNGU = await dbContext.thethuatngus.FindAsync(termId);
                if(THUATNGU == null) { return false; }
                if (THUATNGU.Image != null)
                {
                    var cancellation = new CancellationTokenSource();
                    // Initialize Firebase Storage
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

                    var firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
                    });
                    string fileNameDelete = ExtractFileNameFromUrl(THUATNGU.Image);
                    string deletePath = $"images/{fileNameDelete}";
                    await firebaseStorage.Child(deletePath).DeleteAsync();
                }
                dbContext.thethuatngus.Remove(THUATNGU);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTHUATNGU(THETHUATNGU thuatngu)
        {
            try
            {
                if (HasDuplicateTermNamePerLearningModuleForUpdate(thuatngu.TermId, thuatngu.LearningModuleId, thuatngu.TermName))
                {
                    return false;
                }

                dbContext.thethuatngus.Update(thuatngu);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }

        public bool HasDuplicateTermNamePerLearningModuleForUpdate(int termId, int learningModuleId, string termName)
        {
            var thuatngu = dbContext.thethuatngus.FirstOrDefault(u => (u.LearningModuleId == learningModuleId && u.TermName == termName && u.TermId != termId));
            return thuatngu != null;
        }

        public void Shuffle<T>(List<T> list)
        {
            Random random = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                // Generate a random index between 0 and i (inclusive).
                int randomIndex = random.Next(0, i + 1);

                // Swap elements at randomIndex and i.
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public List<ObjectivePack> GetObjectiveList(int learningModuleId)
        {
            
            try
            {
                List<ObjectivePack> objectivePacks = new List<ObjectivePack>();
                var thuatngus = dbContext.thethuatngus.Where(e => e.LearningModuleId == learningModuleId).ToList();


                Random random = new Random();
                for (int i = 0; i < thuatngus.Count; i++)
                {
                    ObjectivePack objectivePack = new ObjectivePack();
                    objectivePacks.Add(objectivePack);
                    objectivePack.Question = thuatngus[i].TermName;
                    objectivePack.TermId = thuatngus[i].TermId;
                    int randomNumber = -1;
                    List<int> exclusions = new List<int>();
                    if (thuatngus.Count > 4)
                    {

                        exclusions.Add(i);
                        for (int j = 0; j < 3; j++)
                        {
                            do
                            {
                                randomNumber = random.Next(0, thuatngus.Count);
                            } while (exclusions.Contains(randomNumber));
                            exclusions.Add(randomNumber);
                        }
                        for (int k = 0; k < 4; k++)
                        {
                            int index = exclusions[random.Next(0, exclusions.Count)];
                            exclusions.Remove(index);
                            if (k == 0)
                            {
                                if (index == i)
                                {
                                    objectivePack.Answer = "A";
                                }
                                objectivePack.ChoiceA = thuatngus[index].Explaination;
                            }
                            if (k == 1)
                            {
                                if (index == i)
                                {
                                    objectivePack.Answer = "B";
                                }
                                objectivePack.ChoiceB = thuatngus[index].Explaination;
                            }
                            if (k == 2)
                            {
                                if (index == i)
                                {
                                    objectivePack.Answer = "C";
                                }
                                objectivePack.ChoiceC = thuatngus[index].Explaination;
                            }
                            if (k == 3)
                            {
                                if (index == i)
                                {
                                    objectivePack.Answer = "D";
                                }
                                objectivePack.ChoiceD = thuatngus[index].Explaination;
                            }
                        }

                    }
                    else
                    {
                        objectivePack.Answer = "D";
                        objectivePack.ChoiceD = thuatngus[i].Explaination;
                        exclusions.Add(i);
                        for (int j = 0; j < thuatngus.Count; j++)
                        {
                            if (j == 0)
                            {
                                if (!exclusions.Contains(j))
                                {
                                    exclusions.Add(j);
                                    objectivePack.ChoiceA = thuatngus[j].Explaination;
                                }
                            }
                            if (j == 1)
                            {
                                if (!exclusions.Contains(j))
                                {
                                    exclusions.Add(j);
                                    objectivePack.ChoiceB = thuatngus[j].Explaination;
                                }
                            }
                            if (j == 2)
                            {
                                if (!exclusions.Contains(j))
                                {
                                    exclusions.Add(j);
                                    objectivePack.ChoiceC = thuatngus[j].Explaination;
                                }
                            }
                        }

                    }
                }
                Shuffle<ObjectivePack>(objectivePacks);
                return objectivePacks;
            }catch(Exception ex)
            {
                return new List<ObjectivePack>();
            }
            
        }

        public async Task<bool> UpdateTHUATNGUTest(ResultQuestion resultQuestion)
        {
            try
            {
                var thuatngu = await dbContext.thethuatngus.FindAsync(resultQuestion.TermId);
                if (thuatngu == null) { return false; }
                if (resultQuestion.IsRightAnswer)
                {
                    thuatngu.Accumulate++;
                }
                else
                {
                    thuatngu.Accumulate = Math.Max(thuatngu.Accumulate - 1, 0);
                }
                foreach (var level in dbContext.levelghinhos)
                {
                    if (thuatngu.Accumulate >= level.Condition)
                    {
                        thuatngu.LevelId = level.LevelId;
                    }
                }
                dbContext.thethuatngus.Update(thuatngu);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public string ExtractFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }

        public async Task<AchieveLibrary> GetAchieveLibrary(int userId)
        {
            AchieveLibrary achieveLibrary = new AchieveLibrary();
            try
            {
                var titles = await dbContext.chudes.Where(a => a.UserId == userId).Select(t => t.TitleId).ToListAsync();
                var modules = await dbContext.hocphans
                            .Where(h => titles.Contains(h.TitleId)).Select(t => t.LearningModuleId)
                            .ToListAsync();
                var countTerms = await dbContext.thethuatngus
                .CountAsync(h => modules.Contains(h.LearningModuleId));
                achieveLibrary.NumberTitle = titles.Count;
                achieveLibrary.NumberModule = modules.Count;
                achieveLibrary.NumberTerms = countTerms;
                return achieveLibrary;
            }catch(Exception ex)
            {
                achieveLibrary.NumberTitle = -1;
                achieveLibrary.NumberModule = -1;
                achieveLibrary.NumberTerms = -1;
                return achieveLibrary;
            }
            
        
        }

        public async Task<List<LEVELGHINHO>> GetListLEVELGHINHO()
        {
            return await dbContext.levelghinhos.ToListAsync();
        }

        public async Task<LEVELGHINHO> GetLEVELGHINHO(int levelId)
        {
            return await dbContext.levelghinhos.FindAsync(levelId);
        }

        public async Task<bool> UpdateLEVELGHINHO(LEVELGHINHO level)
        {
            try
            {
                dbContext.levelghinhos.Update(level);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}
