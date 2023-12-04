namespace QuizletAchivement.Api
{
    public class Api
    {
        public static string TermUrl { get; } = "/api/Term/";
        public static string TermAchieveLibrary { get; } = TermUrl + "AchieveLibrary/";

        public static string ClassUrl { get; } = "/api/Class/";
        public static string ClassAchieveClass { get; } = ClassUrl + "AchieveClass/";
        public static string ClassTotalJoin { get; } = ClassUrl + "TotalAttendee/";
    }
}
