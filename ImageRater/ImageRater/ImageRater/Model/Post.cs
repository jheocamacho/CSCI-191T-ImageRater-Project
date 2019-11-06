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
        //public 
        public Image Photo { get; set; }

        public Post()
        {
            Location = "";
            DateTime = "";
            Category = "";
        }

        public Post(string l, string dt, string c)
        {
            Location = l;
            DateTime = dt;
            Category = c;
        }
    }
}
