namespace QuizletClass.Api
{
    public class Api
    {
        public static string UserUrl { get; } = "/api/User";
        public static string LearningModuleUrl { get; } = "/api/LearningModule";
        public static string LearningModuleFindUrl { get; } = LearningModuleUrl + "/find/";
        public static string LearningModuleOfUser { get; } = LearningModuleUrl + "/GetLearningModuleOfUser/";
        public static string LearningModuleCountTerm { get; } = LearningModuleUrl + "/CountTerms/";

        public static string TitleUrl { get; } = "api/Title";
        public static string TitleBaseUserUrl { get; } = TitleUrl+"/user/";

        public static string LearningModuleClassUrl { get; } = LearningModuleUrl + "/ClassLearningModule";
    }
}
