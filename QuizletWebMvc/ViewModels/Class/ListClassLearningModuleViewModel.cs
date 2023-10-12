namespace QuizletWebMvc.ViewModels.Class
{
    public class ListClassLearningModuleViewModel
    {
        public IEnumerable<ClassLearningModuleViewModel> LearningModules { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Describe { get; set; }
        public DateTime CreatedDate { get; set; }

        public void Copy(ClassViewModel cla)
        {
            ClassName = cla.ClassName;
            Describe = cla.Describe;
            CreatedDate = cla.CreatedDate;
        }
    }
}
