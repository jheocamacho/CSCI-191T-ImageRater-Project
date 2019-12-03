using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using SQLite;
using System.Runtime.CompilerServices;

namespace ImageRater.Model
{
    public class Post : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ID for each new post, saved in database
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private string location;
        private string datetime;
        private string tags;
        private int rating;
        private string photo;

        public Post()
        {
            location = "Default location";
            datetime = "Default datetime";
            tags = "Default tags";
            rating = 0;
            photo = "Default photo";
        }

        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChange(); }
        }

        public string DateTime
        {
            get { return datetime; }
            set { datetime = value; OnPropertyChange(); }
        }

        public string Tags
        {
            get { return tags; }
            set { tags = value; OnPropertyChange(); }
        }

        public int Rating
        {
            get { return rating; }
            set { rating = value; OnPropertyChange(); }
        }

        public string Photo
        {
            get { return photo; }
            set { photo = value; OnPropertyChange(); }
        }

        //public string Base64Image { get; set; }

        /*
        public void SerializePhoto(object PhotoImage)
        {
            if (PhotoImage == null)
                return;

            string imageBase64String = (string)PhotoImage;
            byte[] imageAsBytes = Convert.FromBase64String(imageBase64String);

            using (var ms = new MemoryStream(imageAsBytes))
            {
                var decoder = System.Windows.Media.Imaging.BitmapDecoder.Create(ms, BitmapCreateOptions.None, BitmapCacheOptions.OnLoad);

                Base64Image = decoder.Frames[0];
            }
        }
        */
    }
}
