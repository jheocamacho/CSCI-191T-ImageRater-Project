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
    public class PostViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // list of posts to be used by the view and synced with the database
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

        // holds the current post, in the BrowsePage and PhotoPage,
        // meant to be used by Like, Dislike, and RemovePost functions
        private Post currentPost;
        public Post CurrentPost
        {
            get { return currentPost; }
            set
            {
                currentPost = value;
                OnPropertyChange();
            }
        }

        // holds the new post that is created in the PostPage
        private Post newPost;
        public Post NewPost
        {
            get { return newPost; }
            set
            {
                newPost = value;
                OnPropertyChange();
            }
        }

        // holds a single string of tags associated with a post, used by post page
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

        // holds single photo path, used by post page
        private string currentPhotoPath;
        public string CurrentPhotoPath
        {
            get { return currentPhotoPath; }
            set
            {
                currentPhotoPath = value;
                OnPropertyChange();
            }
        }

        // update/sync Posts list with database
        private async Task UpdatePosts()
        {
            Posts = await App.Database.GetPostAsync();
        }

        public PostViewModel()
        {
            UpdatePosts();

            CreatePostCommand = new Command(async () => await CreatePost());
            RemovePostCommand = new Command(async () => await RemovePost());
            TakePhotoCommand = new Command(async () => CurrentPhotoPath = await Camera.TakePhoto());
            UploadPhotoCommand = new Command(async () => CurrentPhotoPath = await Camera.UploadPhoto());
            SwipeLeftCommand = new Command(SwipeLeft);
            SwipeRightCommand = new Command(SwipeRight);
            SwipeModeCommand = new Command(SwitchMode);
        }

        // commands for creating or removing a post
        public Command CreatePostCommand { get; }
        public Command RemovePostCommand { get; }

        // commands for taking or uploading a photo
        public Command TakePhotoCommand { get; }
        public Command UploadPhotoCommand { get; }

        // commands for swiping left or right (like/dislike, set/unset tags)
        public Command SwipeLeftCommand { get; }
        public Command SwipeRightCommand { get; }

        // switch between Rate and Tag mode (for when the user swipes)
        public Command SwipeModeCommand { get; }

        // flag to determine what mode page is on, true means rate mode, false means tag mode
        private bool rateMode = true;
        public bool RateMode
        {
            get { return rateMode; }
            set
            {
                rateMode = value;
                OnPropertyChange();
            }
        }

        // used in view to display which mode page is currently on
        private string swipeMode = "Swipe to Rate Mode";
        public string SwipeMode
        {
            get { return swipeMode; }
            set
            {
                swipeMode = value;
                OnPropertyChange();
            }
        }

        // switch between rate mode and tag mode, adjusts bool then string
        public void SwitchMode()
        {
            RateMode = !RateMode;
            if (RateMode == true)
            {
                SwipeMode = "Swipe to Rate Mode";
            }
            else
            {
                SwipeMode = "Swipe to Tag Mode";
            }
            System.Diagnostics.Debug.WriteLine("RateMode is set to: " + RateMode);
        }
         
        // function to handle whether to like/dislike, or set/unset tags, to be used in command
        public void SwipeRight()
        {
            if (RateMode == true)
            {
                Like();
                System.Diagnostics.Debug.WriteLine("Swiped right with RateMode on");
            }
            else
                /* set tags functions */
                System.Diagnostics.Debug.WriteLine("Swiped right with RateMode off");
        }

        // function to handle whether to like/dislike, or set/unset tags, to be used in command
        public void SwipeLeft()
        {
            if (RateMode == true)
            {
                Dislike();
                System.Diagnostics.Debug.WriteLine("Swiped left with RateMode on");
            }
            else
                /* set tags functions */
                System.Diagnostics.Debug.WriteLine("Swiped left with RateMode off");
        }

        // like a post function, increments rating
        public void Like()
        {
            CurrentPost.Rating++;
        }

        // dislike a post function, decrements rating
        public void Dislike()
        {
            CurrentPost.Rating--;
        }

        // used to create a post and add to database
        public async Task CreatePost()
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

            // assign data to new post (which will be used in the PostPage view)
            NewPost = new Post {
                DateTime = dateTime,
                Location = address,
                Tags = tags,
                Photo = CurrentPhotoPath,
                Rating = 0        
            };

            // save new post to database
            await App.Database.SavePostAsync(NewPost);

            // update Post list (seen in BrowsePage)
            Posts = await App.Database.GetPostAsync();
        }

        // used to remove a post from the database
        public async Task RemovePost()
        {
            await App.Database.RemovePostAsync(CurrentPost);
            Posts = await App.Database.GetPostAsync();
        }
    }
}
