namespace QuizletWebMvc.ViewModels.Terminology
{
    public class ListObjectivePack
    {
        public IEnumerable<ObjectivePack> ObjectivePacks { get; set; }
        public int LearningModuleId { get; set; }
        public bool IsOwned { get; set; } = true;
    }
}
