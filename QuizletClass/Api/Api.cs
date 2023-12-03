namespace QuizletClass.Api
{
    public class Api
    {
        public static string LearningModuleUrl { get; } = "/api/LearningModule";
        public static string LearningModuleFindUrl { get; } = LearningModuleUrl + "/find/";

        public static string TitleUrl { get; } = "api/Title";
        public static string TitleBaseUserUrl { get; } = TitleUrl+"/user/";

        public static string LearningModuleClassUrl { get; } = LearningModuleUrl + "/ClassLearningModule";
    }
}
