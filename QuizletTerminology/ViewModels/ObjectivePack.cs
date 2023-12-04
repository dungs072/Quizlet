namespace QuizletTerminology.ViewModels
{
    public class ObjectivePack
    {
        public string Question { get; set; }
        public string ChoiceA { get; set; } = "";
        public string ChoiceB { get; set; } = "";
        public string ChoiceC { get; set; } = "";
        public string ChoiceD { get; set; } = "";
        public string Answer { get; set; }
        public int TermId { get; set; }


    }
}
