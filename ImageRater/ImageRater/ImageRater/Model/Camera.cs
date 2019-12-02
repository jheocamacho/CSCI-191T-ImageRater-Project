using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace ImageRater.Model
{
    static class Camera
    {
        public static async Task TakePhoto(Image Element)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = true
            });

            if (file == null)
                return;

            Element.Source = ImageSource.FromStream(() => file.GetStream());
        }

        public static async Task UploadPhoto(Image Element)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Upload", "Picking a photo is not supported.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            Element.Source = ImageSource.FromStream(() => file.GetStream());
        }
    }
}
