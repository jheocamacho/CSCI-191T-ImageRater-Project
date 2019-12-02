using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text;
using Xamarin.Forms;
using ImageRater.Model;
namespace ImageRater.ViewModel
{
    class PostViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<Post> posts;
        public List<Post> Posts
        {
            get { return posts; }
            set
            {
                posts = value;
                OnPropertyChange();
            }
        }

        private string currentTags;
        public string CurrentTags
        {
            get { return currentTags; }
            set
            {
                currentTags = value;
                OnPropertyChange();
            }
        }

        private Image currentPhoto;
        public Image CurrentPhoto
        {
            get { return currentPhoto; }
            set
            {
                currentPhoto = value;
                OnPropertyChange();
            }
        }
               
        public PostViewModel()
        {
            // Sample data to check if CollectionView binding in BrowsePage is working
            Posts = new List<Post>() {
                new Post()
                {
                    ID = 1,
                    DateTime = "November 30, 2019 8:08pm",
                    Location = "Fresno, CA",
                    Tags = "Animal",
                    Rating = 3
                },

                new Post()
                {
                    ID = 2,
                    DateTime = "December 1, 2019 12:00pm",
                    Location = "Clovis, CA",
                    Tags = "Landscape",
                    Rating = 4
                }
            };

            CreatePostCommand = new Command(async () => await CreateNewPost());
            TakePhotoCommand = new Command(async () => await Camera.TakePhoto(CurrentPhoto = new Image()));
            UploadPhotoCommand = new Command(async () => await Camera.UploadPhoto(CurrentPhoto = new Image()));
        }
        
        public Command CreatePostCommand { get; }
        public Command TakePhotoCommand { get; }
        public Command UploadPhotoCommand { get; }

        public async Task CreateNewPost()
        {
            // get tag info
            string tags = CurrentTags;

            // get current time for post
            var dt = System.DateTime.Now;
            string dateTime = String.Format("{0:f}", dt);

            // get current location for post
            var location = await Location.GetCurrentPosition();
            var placemark = await Location.ReverseGeocodeLocation(location);
            string address = placemark.FeatureName;

            // Exception throw during this next statement at line 76:
            // System.AggregateException: 'One or more errors occurred. (Don't know about Xamarin.Forms.Image)'
            // Inner Exception
            // NotSupportedException: Don't know about Xamarin.Forms.Image
            await App.Database.SavePostAsync(new Post
            {
                Location = address,
                DateTime = dateTime,
                Tags = tags,
                //Photo = CurrentPhoto,
                Rating = 0
            });
            
            Posts = await App.Database.GetPostAsync();            
        }        
    }
}
