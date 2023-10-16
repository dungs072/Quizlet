using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using QuizletClass.Models;

namespace QuizletClass.ViewModels
{
    public class ModuleDetailWithList
    {
        public int LearningModuleId { get; set; } = -1;
        public string LearningModuleName { get; set; } = "";
        public string? Describe { get; set; }
        public int TitleId { get; set; }
        public bool IsChoose { get; set; } = false;
        public void Copy(HOCPHAN hOCPHAN)
        {
            LearningModuleId = hOCPHAN.LearningModuleId;
            LearningModuleName = hOCPHAN.LearningModuleName;
            Describe = hOCPHAN.Describe;
            TitleId = hOCPHAN.chude.TitleId;
        }
    }
}
