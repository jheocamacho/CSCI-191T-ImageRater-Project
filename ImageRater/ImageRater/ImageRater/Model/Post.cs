using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;

namespace ImageRater.Model
{
    [Serializable]
    class Post
    {
        public Placemark Location { get; set; }
        public string DateTime { get; set; }
        public string Category { get; set; }
        public float Rating { get; set; }        
        public Image Photo { get; set; }
        //public string Base64Image { get; set; }

        public Post()
        {
            Location = null;
            DateTime = "";
            Category = "";
            Rating = 0;
        }

        public Post(Placemark l, string dt, string c, float r)
        {
            Location = l;
            DateTime = dt;
            Category = c;
            Rating = r;
        }
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
