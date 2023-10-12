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

        public void Copy(CHITIETHOCPHAN cthp)
        {
            LearningModuleId = cthp.LearningModuleId;
            LearningModuleName = cthp.LearningModule.LearningModuleName;
            Describe = cthp.LearningModule.Describe;
            AddedDate = cthp.CreatedDate;
            NumberTerms = cthp.LearningModule.thethuatngus.Count;
        }
    }
}
