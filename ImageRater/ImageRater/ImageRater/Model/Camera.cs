using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace ImageRater.Model
{
    static class Camera
    {
        public static async Task<string> TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
                return "Error: photo is not supported";
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                SaveMetaData = true
            });

            if (file == null)
                return "Error: file is null";
            /*
            System.Diagnostics.Debug.WriteLine("Before image stream");
            System.Diagnostics.Debug.WriteLine(file.GetStream().Read(new byte[4000], 0, 4000));            
            System.Diagnostics.Debug.WriteLine("After image stream");
            */
            //Element.Source = ImageSource.FromStream(() => file.GetStream());
            return file.Path.ToString();

        }

        public static async Task<string> UploadPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Upload", "Picking a photo is not supported.", "OK");
                return "Error: photo is not supported";
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return "Error: file is null";
            /*
            System.Diagnostics.Debug.WriteLine("Before image stream");
            System.Diagnostics.Debug.WriteLine(file.GetStream().ToString());
            System.Diagnostics.Debug.WriteLine("After image stream");
            */
            //Element.Source = ImageSource.FromStream(() => file.GetStream());
            return file.Path.ToString();
        }
    }
}
