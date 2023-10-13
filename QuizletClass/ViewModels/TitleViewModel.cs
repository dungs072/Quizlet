using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using QuizletClass.Models;

namespace QuizletClass.ViewModels
{
    public class TitleViewModel
    {
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string? Describe { get; set; }

        public void Copy(CHUDE chude)
        {
            TitleId= chude.TitleId;
            TitleName= chude.TitleName;
            Describe= chude.Describe;
        }
    }
}
