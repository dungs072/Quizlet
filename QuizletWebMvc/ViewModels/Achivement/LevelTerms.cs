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
    }
}