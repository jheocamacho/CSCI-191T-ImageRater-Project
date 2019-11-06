using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void SubmitButton_Clicked(object sender, EventArgs e)
        {
            // get category info
            //string category = CategoryPicker.SelectedItem.ToString();

            // get current time for post
            var dt = DateTime.Now;
            string dateTime = String.Format("{0:f}", dt);

            // get current location for post
            var position = Model.Location.GetCurrentPosition();
            var location = Model.Location.ReverseGeocodeCurrentLocation(position.Result);
            string locality = location.Result.Locality;

            // assign values to UI elements
            DateTimeLabel.Text = dateTime;
            LocationLabel.Text = locality;
            //CategoryLabel.Text = category;

            // assign values to new Post object
            NewPost.DateTime = dateTime;
            NewPost.Location = locality;            
            //NewPost.Category = category;
            NewPost.Photo = PhotoImage;
        }
    }
}
