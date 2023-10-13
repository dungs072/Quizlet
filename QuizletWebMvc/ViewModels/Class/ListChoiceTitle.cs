
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizletWebMvc.ViewModels.Class
{
    public class ListChoiceTitle
    {
        public int ClassId { get; set; }
        public List<TitleChoiceViewModel> TitleChoiceViewModels { get; set; }
    }
    public class TitleChoiceViewModel
    {
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string? Describe { get; set; }
        
    }
    
}
