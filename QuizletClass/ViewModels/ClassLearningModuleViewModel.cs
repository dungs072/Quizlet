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

        public void Copy(CHITIETHOCPHAN cthp,HOCPHAN hOCPHAN,int numberTerms)
        {
            LearningModuleId = cthp.hocphan.LearningModuleId;
            LearningModuleName = hOCPHAN.LearningModuleName;
            Describe = hOCPHAN.Describe;
            AddedDate = cthp.CreatedDate;
            NumberTerms = numberTerms; /*cthp.LearningModule.CountNumeberModulesPerClass(classId);*/
        }
    }
}
