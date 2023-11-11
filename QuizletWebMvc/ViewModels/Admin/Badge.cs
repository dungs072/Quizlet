using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Admin
{
    public class ListBadges
    {
        public List<Badge> ModuleBadges { get; set; }
        public List<Badge> TermBadges { get; set; }
        public List<Badge> ParticipantBadges { get; set; }
    }
    public class Badge
    {
        public int AchivementId { get; set; }
        [Required(ErrorMessage ="Badge name is not blank")]
        public string AchivementName { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "Condition must be a number.")]
        public int Condition { get; set; }
        public string? Image { get; set; }
        public string TypeBadge { get; set; }
    }
}
