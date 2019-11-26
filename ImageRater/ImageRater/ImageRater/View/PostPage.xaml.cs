using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageRater.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostPage : ContentPage
    {
        private Model.Post NewPost = new Model.Post();

        public PostPage()
        {
            InitializeComponent();
        }

        private void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            Model.Camera.TakePhoto(PhotoImage);
            
        }

        private void UploadPhotoButton_Clicked(object sender, EventArgs e)
        {
            Model.Camera.UploadPhoto(PhotoImage);
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            // get category info
            string category = CategoryEntry.Text;

            // get current time for post
            var dt = DateTime.Now;
            string dateTime = String.Format("{0:f}", dt);

            // get current location for post
            var location = await Model.Location.GetCurrentPosition();
            var placemark = await Model.Location.ReverseGeocodeLocation(location);
            string address = placemark.FeatureName;
            await Model.Location.GeocodeLocation(address);

            // assign values to UI elements
            DateTimeLabel.Text = dateTime;
            LocationLabel.Text = address;
            CategoryLabel.Text = category;

            // assign values to new Post object
            NewPost.DateTime = dateTime;
            NewPost.Location = placemark;
            NewPost.Category = category;
            // NewPost.Base64Image = function (PhotoImage);
            NewPost.Photo = PhotoImage;

            //string json = JsonConvert.SerializeObject(NewPost, Formatting.Indented);
            //System.Diagnostics.Debug.WriteLine(json);
        }
    }
}