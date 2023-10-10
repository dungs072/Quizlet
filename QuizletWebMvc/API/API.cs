namespace QuizletWebMvc.API
{
    public static class API
    {
        public static string UserUrl { get { return $"/api/User"; } }
        public static string UserUrlCheck { get { return UserUrl + "/check/"; } }

        public static string TitleUrl { get { return $"/api/Title"; } }
        public static string TitleUrlCheck { get { return TitleUrl + "/check/"; } }
        public static string TitleUrlUser { get { return TitleUrl + "/user/"; } }
        public static string TitleUrlFind { get { return TitleUrl + "/find/"; } }

        public static string LearningModuleUrl { get { return $"/api/LearningModule"; } }
        public static string LearningModuleUrlFind { get { return LearningModuleUrl + "/find/"; } }

        public static string TermUrl { get { return $"/api/Term"; } }
        public static string TermUrlFind { get { return TermUrl + "/find/"; } }
        public static string TermUrlObjective { get { return TermUrl + "/objective/"; } }

    }
}
