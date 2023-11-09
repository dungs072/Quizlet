namespace QuizletWebMvc.ViewModels.Achivement
{
    public class Badge
    {
        public string NameBadge { get; set; }
        public bool IsAchieved { get; set; }

        public string DateAchieved { get; set; } 
    }
    public class AchivementBadge
    {
        public int AchivementId { get; set; }
        public string AchivementName { get; set; }
      
    }
}
