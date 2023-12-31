﻿namespace QuizletWebMvc.ViewModels.Achivement
{
    public class LevelTerms
    {
        public string LevelName { get; set; }
        public int NumberTermsInLevel { get; set; }
    }
    public class UserAchivement
    {
        public List<LevelTerms> LevelTerms { get; set; }
        public List<string> SequenceDates { get; set; }
        public List<Badge> moduleBadges { get; set; }
        public List<Badge> termBadges { get; set; }
        public List<Badge> participantBadges { get; set; }
        public AchieveStatistics AchieveStatistics { get; set; }

    }
}
