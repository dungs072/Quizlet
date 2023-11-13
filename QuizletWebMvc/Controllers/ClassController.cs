using Microsoft.AspNetCore.Mvc;
using QuizletWebMvc.Services.Class;
using QuizletWebMvc.Services.Terminology;
using QuizletWebMvc.ViewModels.Class;
using QuizletWebMvc.ViewModels.Terminology;

namespace QuizletWebMvc.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassService classService;
        private readonly ITerminologyService terminologyService;
        public ClassController(IClassService classService,ITerminologyService terminologyService)
        {
            this.terminologyService = terminologyService;
            this.classService = classService;
        }
        public async Task<IActionResult> YourOwnClass()
        {
            ListClassViewModel listClassViewModel = new ListClassViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var classes = await classService.GetClassesByUser(userId);
                listClassViewModel.Classes = classes;
            }
            return View(listClassViewModel);
        }

        public IActionResult CreateYourOwnClass()
        {
            ClassViewModel cla = new ClassViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                cla.UserId = userId;
            }
            return View(cla);
        }
        public async Task<IActionResult> HandleCreateClass(ClassViewModel classModel)
        {
            ModelState.Remove("LearningModule");
            if (!ModelState.IsValid)
            {
                return View("CreateYourOwnClass", classModel);
            }
            var canCreate = await classService.CreateClass(classModel);
            if (!canCreate)
            {
                TempData["Error"] = "Duplicate class name. Please fix it!!";
                return View("CreateYourOwnClass", classModel);
            }

            TempData["Success"] = "Create a class successfully";
            return RedirectToAction("YourOwnClass");
        }
        public async Task<IActionResult> EditYourOwnClass(int classId)
        {
            ClassViewModel cla = await classService.GetClass(classId);
            return View(cla);
        }
        public async Task<IActionResult> HandleEditYourOwnClass(ClassViewModel cla)
        {
            //ModelState.Remove("LearningModule");
            if (!ModelState.IsValid) return View("EditYourOwnClass", cla);
            var canUpdate = await classService.UpdateClass(cla);
            if (!canUpdate)
            {
                TempData["Error"] = "Duplicate class name. Please fix it!!";
                return View("EditYourOwnClass", cla);
            }
            TempData["Success"] = "Update class sucessfully";
            return RedirectToAction("YourOwnClass");
        }
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var canDelete = await classService.DeleteClass(classId);

            if (!canDelete)
            {
                TempData["Error"] = "Delete this class failed because it contains links to other datas";
            }
            else
            {
                TempData["Success"] = "Delete a class sucessfully";
            }

            return RedirectToAction("YourOwnClass");
        }
        public async Task<IActionResult> ShowDetailOwnClassLearningModule(int classId)
        {
            HttpContext.Session.SetString("CurrentClassId",classId.ToString());
            List<ClassLearningModuleViewModel> models = await classService.GetDetailLearningModuleClass(classId);
            ClassViewModel cla = await classService.GetClass(classId);
            ListClassLearningModuleViewModel listModels = new ListClassLearningModuleViewModel();
            listModels.LearningModules = models;
            listModels.Copy(cla);
            listModels.ClassId = classId;
            return View("DetailOwnClass", listModels);

        }
        public async Task<IActionResult> TitleSelection(int classId)
        {
            ListChoiceTitle listChoiceTitle = new ListChoiceTitle();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                listChoiceTitle.TitleChoiceViewModels = await classService.GetTitleDatas(userId);
                listChoiceTitle.ClassId = classId;
            }
            return View(listChoiceTitle);
        }
        public async Task<IActionResult> LearningModuleSelection(int titleId)
        {
            ViewModels.Class.ListLearningModuleViewModel listLearningModule = new ViewModels.Class.ListLearningModuleViewModel();
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                listLearningModule.Modules = await classService.GetModuleDatas(classId,titleId);
            }
            listLearningModule.TitleId = titleId;
            return View(listLearningModule);
        }
        public async Task<IActionResult> AddModuleToClass(int learningModuleId,int titleId)
        {
            if(int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                LearningModuleDetail detail = new LearningModuleDetail();
                detail.ClassId = classId;
                detail.LearningModuleId = learningModuleId;
                
                var check = await classService.AddModuleToClass(detail);
                if(!check)
                {
                    TempData["Error"] = "Cannot add module to class. Server error";
                   
                }
                else
                {
                    TempData["Success"] = "Add module to class successfully";
                }
            }
            
            return RedirectToAction("LearningModuleSelection", new {titleId = titleId});
        }
        public async Task<IActionResult> DeleteModuleFromClass(int learningModuleId, int titleId,string typePage = "")
        {
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                var check = await classService.DeleteModuleDetail(classId,learningModuleId);
                if (!check)
                {
                    TempData["Error"] = "Cannot delete module from class. Server error";

                }
                else
                {
                    TempData["Success"] = "Delete module from class successfully";
                }
            }
            if(typePage=="choose")
            {
                return RedirectToAction("LearningModuleSelection", new { titleId = titleId });
            }
            else
            {
                return RedirectToAction("ShowDetailOwnClassLearningModule", new { classId = classId });
            }
            
        }

        public async Task<IActionResult> ShowDetailOwnClassParticipant(int classId)
        {
            HttpContext.Session.SetString("CurrentClassId", classId.ToString());
            List<Participant> models = await classService.GetDetailParticipantClass(classId);
            ClassViewModel cla = await classService.GetClass(classId);
            ListParticipant listModels = new ListParticipant();
            listModels.Participants = models;
            listModels.Copy(cla);
            return View("ParticipantInYourOwnClass", listModels);
        }

        public async Task<IActionResult> SearchAddUserParticipant(string search = "")
        {
            ListUserParticipant listModels = new ListUserParticipant();
            HttpContext.Session.SetString("Search", search);
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                List<UserParticipant> models = await classService.GetUserParticipant(classId,search,userId);
                listModels.UserParticipants = models;
                listModels.ClassId = classId;
            }
            return View("AddParticipant", listModels);
        }
        [HttpGet]
        public IActionResult SearchUser(ListUserParticipant listUserParticipant)
        {
            return RedirectToAction("SearchAddUserParticipant",new { search = listUserParticipant.Search});
        }

        public async Task<IActionResult> AddUserToClass(int userId,string search)
        {
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                RegisterDetailClass registerDetailClass = new RegisterDetailClass();
                registerDetailClass.UserId = userId;
                registerDetailClass.ClassId = classId;
                registerDetailClass.IsAccepted = true;

                var check = await classService.AddParticipantToClass(registerDetailClass);
                if (!check)
                {
                    TempData["Error"] = "Cannot add this user to class. Server error";

                }
                else
                {
                    TempData["Success"] = "Add user to class successfully";
                }
            }

            return RedirectToAction("SearchAddUserParticipant", new { search = search });
        }
        public async Task<IActionResult> DeleteParticipantFromClass(int userId)
        {
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                var check = await classService.DeleteParticipantFromClass(classId, userId);
                if (!check)
                {
                    TempData["Error"] = "Cannot kick out paticipant from class. Server error";

                }
                else
                {
                    TempData["Success"] = "Kick out participant from class successfully";
                }
            }
            return RedirectToAction("ShowDetailOwnClassParticipant", new { classId = classId });

        }
        public async Task<IActionResult> ShowDetailOwnClassPendingParticipant(int classId)
        {
            HttpContext.Session.SetString("CurrentClassId", classId.ToString());
            List<Participant> models = await classService.GetDetailPendingParticipantClass(classId);
            ClassViewModel cla = await classService.GetClass(classId);
            ListParticipant listModels = new ListParticipant();
            listModels.Participants = models;
            listModels.Copy(cla);
            return View("PendingParticipant", listModels);
        }

        public async Task<IActionResult> UpdateRegisterDetail(int userId)
        {
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {
                var registerDetail = await classService.GetDetailPendingParticipant(classId, userId);
                registerDetail.IsAccepted = true;
                var canUpdate = await classService.UpdateRegisterDetail(registerDetail);
                if (!canUpdate)
                {
                    TempData["Error"] = "Cannot handle your request. Server error";
                }
                else
                {
                    TempData["Success"] = "Accept participant sucessfully";
                }
            }
            return RedirectToAction("ShowDetailOwnClassPendingParticipant", new { classId = classId });
        }
        public async Task<IActionResult> RejectRegistration(int classId, int userId)
        {
            var check = await classService.DeleteParticipantFromClass(classId, userId);
            if (!check)
            {
                TempData["Error"] = "Cannot handle your request. Server error";

            }
            else
            {
                TempData["Success"] = "reject successfully";
            }
            return RedirectToAction("ShowDetailOwnClassPendingParticipant", new { classId = classId });

        }


        public async Task<IActionResult> FindRegisterClass(string search)
        {
            HttpContext.Session.SetString("GlobalSearch",search);
            ListRegisterClass listRegisterClass = new ListRegisterClass();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                List<RegisterClass> registerClasses = await classService.GetRegisterClass(userId, search);
                listRegisterClass.RegisterClasses = registerClasses;
            }
            return View("RegisterClass", listRegisterClass);
        }
        public async Task<IActionResult> ReadFirstTerms(int learningModuleId,string learningModuleName,int classId)
        {
            ListTermViewModel listTermViewModel = new ListTermViewModel();
            List<TermViewModel> models = await terminologyService.GetTermByLearningModuleId(learningModuleId);
            listTermViewModel.LearningModuleName = learningModuleName;
            listTermViewModel.Terms = models;
            listTermViewModel.ClassId = classId;
            string globalSearch = HttpContext.Session.GetString("GlobalSearch");
            listTermViewModel.GlobalSearch = globalSearch;
            return View("ReadFirstTerms", listTermViewModel);
        }

        public async Task<IActionResult> RegisterClass(int classId)
        {
            RegisterDetailClass registerDetailClass = new RegisterDetailClass();
            registerDetailClass.ClassId = classId;
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                registerDetailClass.UserId = userId;
            }
            registerDetailClass.IsAccepted = false;
            bool canRegister = await classService.AddParticipantToClass(registerDetailClass);
            if (!canRegister)
            {
                TempData["Error"] = "Cannot handle your request. Server error";
            }
            else
            {
                TempData["Success"] = "Register this class sucessfully";
            }
            string globalSearch = HttpContext.Session.GetString("GlobalSearch");
            return RedirectToAction("FindRegisterClass","Class", new { search = globalSearch });


        }

        public async Task<IActionResult> YourJoinClass()
        {
            ListClassViewModel listClassViewModel = new ListClassViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var classes = await classService.GetJoinClass(userId);
                listClassViewModel.Classes = classes;
            }
            return View(listClassViewModel);
        }
        public async Task<IActionResult> ShowDetailJoinClassLearningModule(int classId)
        {
            HttpContext.Session.SetString("CurrentClassId", classId.ToString());
            List<ClassLearningModuleViewModel> models = await classService.GetDetailLearningModuleClass(classId);
            ClassViewModel cla = await classService.GetClass(classId);
            ListClassLearningModuleViewModel listModels = new ListClassLearningModuleViewModel();
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId)) 
            {
                List<TitleChoiceViewModel> titles = await classService.GetTitleDatas(userId);
               
                listModels.Titles = titles;
            }
            listModels.LearningModules = models;
            listModels.Copy(cla);
            listModels.ClassId = classId;
            return View("DetailJoinClass", listModels);

        }
        public IActionResult BackToShowDetailJoinClassLearningModule()
        {
            if (int.TryParse(HttpContext.Session.GetString("CurrentClassId"), out int classId))
            {

            }
            return RedirectToAction("ShowDetailJoinClassLearningModule", new { classId = classId });
        }
        public async Task<IActionResult> QuitJoinClass(int classId)
        {
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                var check = await classService.DeleteParticipantFromClass(classId, userId);
                if (!check)
                {
                    TempData["Error"] = "Cannot quit from class. Server error";

                }
                else
                {
                    TempData["Success"] = "Quit from class successfully";
                }
            }
            return RedirectToAction("YourJoinClass");

        }

        public async Task<IActionResult> CopyModule(int titleId,int moduleId, int classId)
        {
            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                CopyViewModel model = new CopyViewModel();
                model.TitleId = titleId;
                model.ModuleId = moduleId;
                var check = await classService.CopyModule(model);
                if (!check)
                {
                    TempData["Error"] = "Cannot copy the module to your title. Server error";

                }
                else
                {
                    TempData["Success"] = "Copy the module successfully";
                }
            }
            return RedirectToAction("ShowDetailJoinClassLearningModule", new { classId = classId });

        }

    }
}
