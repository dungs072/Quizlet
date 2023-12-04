namespace QuizletAchivement.ViewModels
{
    public class AchieveStatistics
    {
        public int NumberTitle { get; set; }
        public int NumberModule { get; set; }
        public int NumberTerms { get; set; }

        public int LongestSquence { get; set; }

        public int TotalClass { get; set; }

    }
    public class AchieveBadge
    {
        public int UserId { get; set; }
        public int AchievementId { get; set; }
    }
    public class AchieveLibrary
    {
        public int NumberTitle { get; set; }
        public int NumberModule { get; set; }
        public int NumberTerms { get; set; }
    }
    public class AchieveClass
    {
        public int TotalClass { get; set; }
    }
}
