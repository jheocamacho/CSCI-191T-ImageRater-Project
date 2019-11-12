using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ImageRater.Model
{
    class Post
    {
        public string Location { get; set; }
        public string DateTime { get; set; }
        public string Category { get; set; }
        public float Rating { get; set; }
        public Image Photo { get; set; }

        public Post()
        {
            Location = "";
            DateTime = "";
            Category = "";
            Rating = 0;
        }

        public Post(string l, string dt, string c, float r)
        {
            Location = l;
            DateTime = dt;
            Category = c;
            Rating = r;
        }
    }
}
