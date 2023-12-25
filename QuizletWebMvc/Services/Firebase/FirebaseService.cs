using Firebase.Auth;
using Firebase.Storage;
using System.Runtime.InteropServices;

namespace QuizletWebMvc.Services.Firebase
{
    public class FirebaseService:IFirebaseService
    {
        private readonly string apiKey = "AIzaSyDdwQpFpqzK-c4emQlK5Sy6pTDMVnh5qiY";
        private readonly string bucket = "quizlet-c9cab.appspot.com";
        private readonly string gmail = "sa1235@gmail.com";
        private readonly string password = "123456";
        private FirebaseStorage firebaseStorage = null ;

        public FirebaseService() 
        {
            InitialFireBase();
        }

       

        public async Task InitialFireBase()
        {
            var cancellation = new CancellationTokenSource();
            // Initialize Firebase Storage
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

            firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
            });
        }

        public async Task<string> StoreImage(IFormFile imageFile,string folderPath)
        {
            if(firebaseStorage==null)
            {
                await InitialFireBase();
            }
            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream);
                var cancellation = new CancellationTokenSource();
                // Initialize Firebase Storage
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var authLink = await auth.SignInWithEmailAndPasswordAsync(gmail, password);

                var firebaseStorage = new FirebaseStorage(bucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
                });

                // Specify the path in Firebase Storage
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                string path = $"{folderPath}/{fileName}";
                stream.Seek(0, SeekOrigin.Begin);
                await firebaseStorage.Child(path).PutAsync(stream, cancellation.Token);
                return await firebaseStorage.Child(path).GetDownloadUrlAsync();
            }
        }

        public async Task DeleteImage(string imageUrl,string folderPath)
        {
            string fileNameDelete = ExtractFileNameFromUrl(imageUrl);
            string deletePath = $"{folderPath}/{fileNameDelete}";
            if(firebaseStorage==null)
            {
                await InitialFireBase();
            }
            await firebaseStorage.Child(deletePath).DeleteAsync();
        }
        private string ExtractFileNameFromUrl(string url)
        {
            // Use Uri to parse the URL
            Uri uri = new Uri(url);

            // Get the filename from the URL using Path.GetFileName
            string fileName = Path.GetFileName(uri.LocalPath);

            return fileName;
        }
    }
}
