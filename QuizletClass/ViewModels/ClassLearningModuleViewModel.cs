using QuizletClass.Models;

namespace QuizletClass.ViewModels
{
    public class ClassLearningModuleViewModel
    {
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string Describe { get; set; }
        public DateTime AddedDate { get; set; }
        public int NumberTerms { get; set; }

        public void Copy(CHITIETHOCPHAN cthp,HOCPHAN hOCPHAN)
        {
            LearningModuleId = cthp.LearningModuleId;
            LearningModuleName = hOCPHAN.LearningModuleName;
            Describe = hOCPHAN.Describe;
            AddedDate = cthp.CreatedDate;
            NumberTerms = -1; /*cthp.LearningModule.CountNumeberModulesPerClass(classId);*/
        }
    }
}
