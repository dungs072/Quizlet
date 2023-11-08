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
        public static string TermTest { get { return TermUrl + "/test"; } }

        public static string ClassUrl { get { return $"/api/Class"; } }
        public static string ClassUrlFind { get { return ClassUrl + "/find/"; } }
        public static string ClassDetailOwn { get { return ClassUrl + "/DetailOwnClass/"; } }
        public static string ClassTitleDetailOwn { get { return ClassUrl + "/DetailTitle/"; } }
        public static string ClassModuleDetailOwn { get { return ClassUrl + "/DetailModule/"; } }
        public static string ClassModuleAdd { get { return ClassUrl + "/ModuleAdd"; } }
        public static string ClassParticipant { get { return ClassUrl + "/DetailParticipant"; } }
        public static string ClassParticipantSearch { get { return ClassUrl + "/SearchUser"; } }
        public static string ClassParticipantAdd { get { return ClassUrl + "/UserParticipant"; } }
        public static string ClassPendingParticipant { get { return ClassUrl + "/DetailPendingParticipant"; } }
        public static string ClassRegister { get { return ClassUrl + "/GlobalSearch"; } }

        public static string ClassJoin { get { return ClassUrl + "/JoinClass"; } }

        public static string AchivementUrl { get { return $"/api/Achivement"; } }
        public static string AchivementUser { get { return AchivementUrl + "/UserAchieve"; } }
        public static string AchieveStatistics { get { return AchivementUrl + "/AchieveStatistics"; } }
        public static string SequenceCalender { get { return AchivementUrl + "/GetSequenceCalender"; } }
        public static string MarkAttendance { get { return AchivementUrl + "/MarkAttendance"; } }
        public static string Badges { get { return AchivementUrl + "/GetBadges"; } }
    }
}
