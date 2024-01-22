using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;

namespace PTP.Application.Commons;
public static class FirebaseUtility
    {
        public static async Task<FileUploadModel> UploadFileAsync(this IFormFile fileUpload,string folder,AppSettings appSettings)
        {
            if (fileUpload.Length > 0)
            {
                var fs = fileUpload.OpenReadStream();
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey:appSettings.FirebaseSettings.ApiKeY));
                var user = await auth.GetUserAsync(firebaseToken: string.Empty);
    
                var a = await auth.SignInWithEmailAndPasswordAsync(email:appSettings.FirebaseSettings.AuthEmail, password:appSettings.FirebaseSettings.AuthPassword);
                var cancellation = new FirebaseStorage(
                    appSettings.FirebaseSettings.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true

                    }
                    ).Child("assets/"+folder).Child(fileUpload.FileName)
                    .PutAsync(fs, CancellationToken.None);
                try
                {
                    var result = await cancellation;

                    return new FileUploadModel
                    {
                        FileName = fileUpload.FileName,
                        URL = result
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }

            }
            else throw new Exception("File is not existed!");
        }

        public static async Task<bool> RemoveFileAsync(this string fileName,string folder,AppSettings appSettings)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey:appSettings.FirebaseSettings.ApiKeY));
            var loginInfo = await auth.SignInWithEmailAndPasswordAsync(email:appSettings.FirebaseSettings.AuthEmail, password:appSettings.FirebaseSettings.AuthPassword);
            var storage = new FirebaseStorage(appSettings.FirebaseSettings.Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(loginInfo.FirebaseToken),
                ThrowOnCancel = true
            });
            await storage.Child("assets/"+folder).Child(fileName).DeleteAsync();
            return true;

        }
    }

      public class FileUploadModel
    {
        public string URL { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }