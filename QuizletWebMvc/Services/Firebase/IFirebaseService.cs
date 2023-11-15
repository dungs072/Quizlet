namespace QuizletWebMvc.Services.Firebase
{
    public interface IFirebaseService
    {
        Task<string> StoreImage(IFormFile imageFile, string folderPath);
        Task DeleteImage(string imageUrl, string folderPath);
    }
}
