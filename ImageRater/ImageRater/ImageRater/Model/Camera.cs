using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Media;
using Xamarin.Forms;

namespace ImageRater.Model
{
    static class Camera
    {
        private static readonly Page page;

        public static async void TakePhoto(Image Element)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await page.DisplayAlert("No Camera", "No camera available.", "OK");
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

        public static async void UploadPhoto(Image Element)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await page.DisplayAlert("No Upload", "Picking a photo is not supported.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            Element.Source = ImageSource.FromStream(() => file.GetStream());
        }
    }
}
