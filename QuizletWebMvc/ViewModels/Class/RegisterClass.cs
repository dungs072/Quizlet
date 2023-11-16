namespace QuizletWebMvc.ViewModels.Class
{
    public class RegisterClass
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescribe { get; set; }
        public int LearningModuleId { get; set; }
        public string LearningModuleName { get; set; }
        public string LearningModuleDescirbe { get; set; }
        public string OwnerFullName { get; set; }
        public int NumberTerms { get; set; }
        public string TypeUser { get; set; }
    }
    public class ListRegisterClass
    {
        public string GlobalSearch { get; set; }
        public List<RegisterClass> RegisterClasses { get; set; }
    }
}
