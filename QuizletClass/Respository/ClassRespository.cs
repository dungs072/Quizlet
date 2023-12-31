﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizletClass.DBContexts;
using QuizletClass.Models;
using QuizletClass.ViewModels;

namespace QuizletClass.Respository
{
    public class ClassRespository : IClassRespository
    {
        private readonly ClassDBContext dBContext;
        private readonly HttpClient client;
        public ClassRespository(ClassDBContext dBContext, HttpClient client)
        {
            this.dBContext = dBContext;
            this.client = client;
        }
        #region ForService
        public async Task<int> CountAllParticipants(int userId)
        {
            try
            {
                int count = await dBContext.chitietdangkilops.CountAsync(a => a.lop.UserId == userId);
                return count;
            }
            catch (Exception ex)
            {
                return -1;
            }
            
        }
        public async Task<AchieveClass> GetAchieveClass(int userId)
        {
            AchieveClass achieveClass = new AchieveClass();
            try
            {
                achieveClass.TotalClass = await dBContext.lops.Where(a => a.UserId == userId).CountAsync();
                return achieveClass;
            }
            catch (Exception ex)
            {
                achieveClass.TotalClass = -1;
                return achieveClass;
            }
        }
        #endregion

        #region Class
        public async Task<bool> CreateLOP(ClassViewModel classView)
        {
            try
            {
                if (HasDuplicateClassName(classView.UserId, classView.ClassName))
                {
                    return false;
                }
                LOP lop = new LOP();
                UserViewModel nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{classView.UserId}");
                lop.UserId = nguoidung.UserId;
                lop.CreatedDate = DateTime.Now;
                lop.ClassName = classView.ClassName;
                lop.Describe = classView.Describe;
                lop.ClassId = classView.ClassId;
                await dBContext.lops.AddAsync(lop);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteLOP(int ClassId)
        {
            try
            {
                var lop = await dBContext.lops.FindAsync(ClassId);
                int NumberParticipants = lop.chitietdangkilop.Count();
                if (NumberParticipants > 0)
                {
                    return false;
                }
                int NumberLearningModules = lop.chitiethocphan.Count;
                if (NumberLearningModules > 0)
                {
                    return false;
                }
                dBContext.lops.Remove(lop);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<List<CHITIETDANGKILOP>> GetCHITIETDANGKILOPS(int classId)
        {
            return await dBContext.chitietdangkilops.Where(a => a.lop.ClassId == classId).ToListAsync();
        }

        public async Task<List<CHITIETHOCPHAN>> GetCHITIETHOCPHANS(int classId)
        {
            return await dBContext.chitiethocphans.Where(a => a.lop.ClassId == classId).ToListAsync();
        }

        public async Task<IEnumerable<ClassViewModel>> GetLOP(int userId)
        {
            List<ClassViewModel> classes = new List<ClassViewModel>();
            try
            {
                var lops = await dBContext.lops.Where(e => e.UserId == userId).ToListAsync();
                foreach (var lop in lops)
                {
                    ClassViewModel model = new ClassViewModel();
                    model.Copy(lop);
                    model.NumberParticipants = (lop.chitietdangkilop.Where(a => a.IsAccepted)).Count();
                    model.NumberLearningModules = lop.chitiethocphan.Count;
                    classes.Add(model);
                }
                return classes;
            }
            catch(Exception ex)
            {
                return classes;
            }
        }

        public async Task<ClassViewModel> GetLOPById(int ClassId)
        {
            ClassViewModel classViewModel = new ClassViewModel();
            try
            {
                var LOP = await dBContext.lops.FindAsync(ClassId);
               
                classViewModel.Copy(LOP);
                return classViewModel;
            }
            catch(Exception ex)
            {
                return classViewModel;
            }
        }

        public bool HasDuplicateClassName(int userId, string className)
        {
            var lop = dBContext.lops.FirstOrDefault(u => (u.UserId == userId && u.ClassName == className));
            return lop != null;
        }

        public bool HasDuplicateClassNameForUpdate(int userId, int classId, string className)
        {
            var cl = dBContext.lops.FirstOrDefault(u => (u.ClassId != classId && u.UserId == userId && u.ClassName == className));
            return cl != null;
        }

        public async Task<bool> UpdateLOP(ClassViewModel lop)
        {
            try
            {
                if (HasDuplicateClassNameForUpdate(lop.UserId, lop.ClassId, lop.ClassName))
                {
                    return false;
                }
                var prevClass = await dBContext.lops.FindAsync(lop.ClassId);
                prevClass.ClassName = lop.ClassName;
                prevClass.Describe = lop.Describe;
                dBContext.lops.Update(prevClass);
                await dBContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }



        #endregion

        #region DetailLearningModuleClass
        public async Task<IEnumerable<ClassLearningModuleViewModel>> GetLearningModuleClassDetail(int classId)
        {
            try
            {
                LearningModuleIdList listId = new LearningModuleIdList();
                var hocphans = await dBContext.chitiethocphans.Where(e => e.lop.ClassId == classId).ToListAsync();
                foreach (var item in hocphans)
                {
                    listId.Ids.Add(item.LearningModuleId);
                    LearningModuleClass model = new LearningModuleClass();
                    model.CreatedDates = item.CreatedDate;
                    model.Ids = item.LearningModuleId;
                    listId.CreatedDates.Add(model);
                }
                HttpResponseMessage response = await client.PostAsJsonAsync(Api.Api.LearningModuleClassUrl, listId);
                if (response.IsSuccessStatusCode)
                {
                    List<ClassLearningModuleViewModel> models = await response.Content.ReadFromJsonAsync<List<ClassLearningModuleViewModel>>();
                    return models;

                }
                else
                {
                    return new List<ClassLearningModuleViewModel>();
                }
            }
            catch(Exception ex)
            {
                return new List<ClassLearningModuleViewModel>();
            }
        }

        public IEnumerable<TitleViewModel> GetYourTitleData(int userId)
        {
            try
            {
                var chudes = client.GetFromJsonAsync<List<TitleViewModel>>(Api.Api.TitleBaseUserUrl + $"{userId}").Result;
                return chudes;
            }
            catch (Exception ex)
            {
                return new List<TitleViewModel>();
            }
        }

        public async Task<IEnumerable<ModuleDetailWithList>> GetYourModuleData(int classId, int titleId)
        {
            List<ModuleDetailWithList> modules = new List<ModuleDetailWithList>();
            //var hocphans = dBContext.hocphans.Where(e => e.chude.TitleId == titleId).ToList();
            try
            {
                var hocphans = await client.GetFromJsonAsync<List<LearningModuleViewModel>>(Api.Api.LearningModuleUrl + $"/{titleId}");
                foreach (var item in hocphans)
                {
                    ModuleDetailWithList module = new ModuleDetailWithList();
                    module.Copy(item, titleId);
                    modules.Add(module);
                    var check = CheckLearningModuleIsRegistered(classId, item.LearningModuleId);
                    module.IsChoose = check;
                }
                return modules;
            }
            catch (HttpRequestException ex)
            {
                return modules;
            }
        }

        public bool CheckLearningModuleIsRegistered(int classId, int learningModuleId)
        {
            return dBContext.chitiethocphans.FirstOrDefault(a => a.lop.ClassId == classId && a.LearningModuleId == learningModuleId) != null;
        }

        public async Task<bool> AddModulesForClass(LearningModuleDetail learningModuleDetail)
        {
            try
            {
                CHITIETHOCPHAN chitiethocphan = new CHITIETHOCPHAN();
                LOP lop = await dBContext.lops.FindAsync(learningModuleDetail.ClassId);
                chitiethocphan.LearningModuleId = learningModuleDetail.LearningModuleId;
                chitiethocphan.lop = lop;
                chitiethocphan.CreatedDate = DateTime.Now;
                await dBContext.chitiethocphans.AddAsync(chitiethocphan);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteModuleDetail(int classId, int learningModuleId)
        {
            try
            {
                var chitiethocphan = dBContext.chitiethocphans.FirstOrDefault(a => a.lop.ClassId == classId && a.LearningModuleId == learningModuleId);
                dBContext.chitiethocphans.Remove(chitiethocphan);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        #endregion

        #region Participants
        public async Task<IEnumerable<Participant>> GetParticipant(int classId)
        {
            
            try
            {
                List<Participant> models = new List<Participant>();
                var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.lop.ClassId == classId).ToListAsync();
                foreach (var item in chitietdangkilops)
                {
                    if (!item.IsAccepted) { continue; }
                    var nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{item.UserId}"); //GetNGUOIDUNG(item.nguoidung.UserId);
                    Participant model = new Participant();
                    model.Copy(item);
                    model.Gmail = nguoidung.Gmail;
                    model.FirstName = nguoidung.FirstName;
                    model.LastName = nguoidung.LastName;
                    model.Image = nguoidung.Image;
                    models.Add(model);
                }
                return models;
            }
            catch(Exception ex)
            {
                return new List<Participant>();
            }
        }

        public async Task<IEnumerable<Participant>> GetPendingParticipant(int classId)
        {
            try
            {
                List<Participant> models = new List<Participant>();
                var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.lop.ClassId == classId).ToListAsync();
                foreach (var item in chitietdangkilops)
                {
                    if (item.IsAccepted) { continue; }
                    var nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{item.UserId}");
                    Participant model = new Participant();
                    model.Copy(item);
                    model.Gmail = nguoidung.Gmail;
                    model.FirstName = nguoidung.FirstName;
                    model.LastName = nguoidung.LastName;
                    model.Image = nguoidung.Image;
                    models.Add(model);
                }
                return models;
            }
            catch (Exception ex)
            {
                return new List<Participant>();
            }
        }

        public async Task<IEnumerable<MessageClassRegistration>> GetMessagePendingParticipant(int userId)
        {
            try
            {
                List<MessageClassRegistration> registers = new List<MessageClassRegistration>();
                var chitietdangkilops = await dBContext.chitietdangkilops.Where(e => e.lop.UserId == userId && !e.IsAccepted).OrderBy(e => e.lop.ClassName).ToListAsync();
                foreach (var item in chitietdangkilops)
                {
                    MessageClassRegistration register = new MessageClassRegistration();
                    var user = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{item.UserId}");
                    register.NameRegister = user.FirstName;
                    register.ImageUrl = user.Image;
                    register.ClassName = item.lop.ClassName;
                    register.ClassId = item.lop.ClassId;
                    register.DateRegister = item.RegisterDate.ToString("dd/MM/yyyy");
                    registers.Add(register);
                }
                return registers;
            }
            catch (Exception ex)
            {
                return new List<MessageClassRegistration>();
            }
        }

        public async Task<IEnumerable<UserParticipant>> GetParticipants(int classId, string search, int currentUserId)
        {
            try
            {
                var models = new List<UserParticipant>();

                List<UserViewModel> nguoidungs = await client.GetFromJsonAsync<List<UserViewModel>>(Api.Api.UserUrl);
                foreach (var nguoidung in nguoidungs)
                {
                    if(nguoidung.TypeAccount== "Admin") { continue; }
                    if (!nguoidung.Gmail.Contains(search, StringComparison.OrdinalIgnoreCase)) { continue; }
                    if (nguoidung.UserId == currentUserId) { continue; }
                    if (CheckUserHasRegisterToClass(classId, nguoidung.UserId))
                    {
                        continue;
                    }
                    UserParticipant user = new UserParticipant();
                    user.Copy(nguoidung);
                    models.Add(user);
                }
                return models;
            }
            catch(Exception ex)
            {
                return new List<UserParticipant>();
            }
        }

        public bool CheckUserHasRegisterToClass(int classId, int userId)
        {
            return dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.UserId == userId) != null;
        }

        public async Task<bool> AddParticipant(RegisterClassDetail registerClassDetail)
        {
            try
            {
                CHITIETDANGKILOP chitietdangkilop = new CHITIETDANGKILOP();
                chitietdangkilop.lop = await dBContext.lops.FindAsync(registerClassDetail.ClassId);
                chitietdangkilop.UserId = registerClassDetail.UserId;/*nguoidung = await client.GetFromJsonAsync<NGUOIDUNG>(Api.Api.UserUrl + $"/{registerClassDetail.UserId}");*/
                chitietdangkilop.IsAccepted = registerClassDetail.IsAccepted;
                chitietdangkilop.RegisterDate = DateTime.Now;
                await dBContext.chitietdangkilops.AddAsync(chitietdangkilop);
                await dBContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserParticipant(int classId, int userId)
        {
            try
            {
                var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.UserId == userId);
                dBContext.chitietdangkilops.Remove(chitietdangki);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCHITIETDANGKI(RegisterClassDetail registerClassDetail)
        {
            try
            {
                CHITIETDANGKILOP chitietdangkilop = await dBContext.chitietdangkilops.FindAsync(registerClassDetail.RegisterDetailClassId);
                chitietdangkilop.IsAccepted = registerClassDetail.IsAccepted;
                dBContext.chitietdangkilops.Update(chitietdangkilop);
                await dBContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<RegisterClassDetail> GetCHITIETDANGKILOP(int classId, int userId)
        {
            try
            {
                var chitietdangki = dBContext.chitietdangkilops.FirstOrDefault(a => a.lop.ClassId == classId && a.UserId == userId);
                RegisterClassDetail register = new RegisterClassDetail();
                register.Copy(chitietdangki);
                return register;
            }catch(Exception ex)
            {
                return new RegisterClassDetail();
            }
          
        }


        #endregion

        #region Register class
        public async Task<IEnumerable<RegisterClass>> GetRegisterClass(int userId, string search)
        {
            try
            {
                // need to improve here
                var models = new List<RegisterClass>();
                List<LearningModuleViewModel> hocphans = await client.GetFromJsonAsync<List<LearningModuleViewModel>>(Api.Api.LearningModuleOfUser + $"{userId}");//await GetHOCPHANOfUser(userId);

                foreach (var hocphan in hocphans)
                {
                    if (!hocphan.LearningModuleName.Contains(search, StringComparison.OrdinalIgnoreCase)) { continue; }
                    List<LOP> lops = await GetLOPOfModule(hocphan.LearningModuleId, userId);
                    foreach (LOP lop in lops)
                    {
                        UserViewModel nguoidung = await client.GetFromJsonAsync<UserViewModel>(Api.Api.UserUrl + $"/{lop.UserId}");
                        int numberTerms = await GetNumberTermsInModules(hocphan.LearningModuleId);
                        if (numberTerms == 0) { continue; }
                        RegisterClass registerClass = new RegisterClass()
                        {
                            ClassId = lop.ClassId,
                            ClassName = lop.ClassName,
                            ClassDescribe = lop.Describe,
                            LearningModuleId = hocphan.LearningModuleId,
                            LearningModuleName = hocphan.LearningModuleName,
                            OwnerFullName = nguoidung.LastName + " " + nguoidung.FirstName,
                            NumberTerms = numberTerms,
                            TypeUser = nguoidung.TypeAccount

                        };
                        models.Add(registerClass);
                    }

                }
                return models;

            }
            catch(Exception ex)
            {
                return new List<RegisterClass>();
            }
        }

        public async Task<List<LOP>> GetLOPOfModule(int learningModuleId, int userId)
        {
            try
            {
                //need to improve here
                var result = from a in (from c in dBContext.chitiethocphans where c.LearningModuleId == learningModuleId select c)
                             join b in dBContext.lops on a.lop.ClassId equals b.ClassId
                             select b;
                var result2 = from c in dBContext.chitietdangkilops where c.UserId == userId select c;
                var excludedIds = result2.Select(c => c.lop.ClassId);
                var finalResult = result.Where(a => !excludedIds.Contains(a.ClassId));
                return finalResult.ToList();
            }
            catch(Exception ex)
            {
                return new List<LOP>();
            }
        }

        public async Task<int> GetNumberTermsInModules(int learningModuleId)
        {
            try
            {
                return await client.GetFromJsonAsync<int>(Api.Api.LearningModuleCountTerm + $"{learningModuleId}");
            }catch(Exception ex)
            {
                return -1;
            }
            
        }

        #endregion

        #region JoinClass

        public async Task<IEnumerable<ClassViewModel>> GetJoinClass(int userId)
        {
            try
            {
                List<ClassViewModel> classes = new List<ClassViewModel>();
                var lops = await GetJoinLOP(userId);
                foreach (var lop in lops)
                {
                    int numberParticipants = (await GetCHITIETDANGKILOPS(lop.ClassId)).Count;
                    int numberModules = (await GetCHITIETHOCPHANS(lop.ClassId)).Count;
                    ClassViewModel classView = new ClassViewModel();
                    classView.Copy(lop);
                    classView.NumberParticipants = numberParticipants;
                    classView.NumberLearningModules = numberModules;
                    classes.Add(classView);
                }
                return classes;
            }
            catch(Exception ex)
            {
                return new List<ClassViewModel>();
            }
        }

        public async Task<List<LOP>> GetJoinLOP(int userId)
        {
            try
            {
                var result = from a in dBContext.chitietdangkilops where a.UserId == userId select a.lop.ClassId;
                var result2 = dBContext.lops.Where(a => result.Contains(a.ClassId));
                return await result2.ToListAsync();

            }
            catch(Exception ex)
            {
                return new List<LOP>();
            }
        }
        #endregion

        #region Delete
        public string CanDeleteLearningModule(int learningModuleId, int userId)
        {
            var data = dBContext.chitiethocphans.FirstOrDefault(a => a.lop.UserId == userId && a.LearningModuleId == learningModuleId);
            return data == null ? "yes" : data.lop.ClassName;
        }
        #endregion
    }
}
