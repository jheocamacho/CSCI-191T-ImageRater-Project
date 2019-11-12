using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace ImageRater
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
        private Model.Post NewPost = new Model.Post();

		public MainPage()
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
            var placemark = await Model.Location.ReverseGeocodeCurrentLocation(location);
            string address = placemark.FeatureName;

            // assign values to UI elements
            DateTimeLabel.Text = dateTime;
            LocationLabel.Text =  address;
            CategoryLabel.Text = category;

            // assign values to new Post object
            NewPost.DateTime = dateTime;
            NewPost.Location = address;            
            NewPost.Category = category;
            NewPost.Photo = PhotoImage;
        }
    }
}
