namespace QuizletAchivement.ViewModels
{
    public class Badge
    {
        public string NameBadge { get; set; }
        public bool IsAchieved { get; set; }
    }
    public class BadgeList
    {
        public List<Badge> Badges  { get; set; }
    }
}
