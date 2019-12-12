using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text;
using Xamarin.Forms;
using ImageRater.Model;
using Xamarin.Forms.Maps;
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
		private string postTags = "";
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

        private string searchTag;
        public string SearchTag
        {
            get { return searchTag; }
            set
            {
                searchTag = value;
                OnPropertyChange();
            }
        }

        // Map object used in MapPage, needed to add post pins
        private Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map()
        {
            IsShowingUser = true,
            HeightRequest = 1000,
            WidthRequest = 1000
        };
        public Xamarin.Forms.Maps.Map Map
        {
            get { return map; }
            set
            {
                map = value;
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
			EnterTagCommand = new Command(EnterTag);
			SwipeLeftCommand = new Command(SwipeLeft);
            SwipeRightCommand = new Command(SwipeRight);
            SwipeModeCommand = new Command(SwitchMode);
            SearchTagsCommand = new Command(organizeByTheSearchTag);
            ResetSearchCommand = new Command(async () => await UpdatePosts());
            SortByTimeCommand = new Command(organizeByDateTime);
            SortByDistanceCommand = new Command(organizeByDistance);
            SortByRatingCommand = new Command(organizeByRating);
        }

        // commands for creating or removing a post
        public Command CreatePostCommand { get; }
        public Command RemovePostCommand { get; }

        // commands for taking or uploading a photo
        public Command TakePhotoCommand { get; }
        public Command UploadPhotoCommand { get; }

		// commands for adding new or existing tags to a photo
		public Command EnterTagCommand { get; }

		// commands for swiping left or right (like/dislike, set/unset tags)
		public Command SwipeLeftCommand { get; }
        public Command SwipeRightCommand { get; }

        // switch between Rate and Tag mode (for when the user swipes)
        public Command SwipeModeCommand { get; }

        // search the posts for those with the given tags or reset them
        public Command SearchTagsCommand { get; }
        public Command ResetSearchCommand { get; }

        // sorting commands for buttons in browse
        public Command SortByTimeCommand { get; }
        public Command SortByDistanceCommand { get; }
        public Command SortByRatingCommand { get; }

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
			string tags = postTags;

            // reset tags after post is submitted
			postTags = "";

            // get current time for post
            var dt = System.DateTime.Now;
            string dateTime = String.Format("{0:f}", dt);

            // get current location for post
            var location = await Location.GetCurrentPosition();
            var placemark = await Location.ReverseGeocodeLocation(location);
            string address = placemark.FeatureName;

            // add pin to map
            Pin postPin = new Pin
            {
                Label = tags,
                Address = address,
                Type = PinType.Place,
                Position = new Position(location.Latitude, location.Longitude)
            };
            map.Pins.Add(postPin);

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

        // used to create a new tag and add it to the database
        public void EnterTag()
		{
			string newTag = CurrentTags;
            // if first tag, no bar in the beginning, otherwise append a bar to the end of it and the new tag right after
			postTags += ((postTags == "") ? "" : "|") + newTag;
		}

        // given a post and a string, it checks if that string is a tag of the post
        public bool containsTag(Post p, string str)	
		{
			bool candidateFound = false;				//Bool indicating if the string is found.
			bool searchForBar = false;					//When the current tag doesn't qualify as the string, we skip the rest of the tag until we go to the next tag.
			int strIndex = 0;							//Starting index of the string.
			int lastIndex = str.Length - 1;				//Ending index of the string.

			for (int i = 0; i < p.Tags.Length; i++)		//Itterating across all the tags in the post, character by character.
			{
				if (p.Tags[i] == '|')					//If it's a bar
				{
					if (strIndex > lastIndex) break;	//Break if we're found our string
					searchForBar = false;				//Otherwise mark the bar as found
					continue;							//And skip to the next char
				}
				if ((strIndex > lastIndex)	||			//If the next index after the string isn't a bar
					(p.Tags[i] != str[strIndex]))		//or if the current char in the tag doesn't match the same indexed char in the string
				{
					candidateFound = false;				//Then this tag isn't a candidate
					searchForBar = true;				//Skip the rest of the tag, find the nearest bar
					strIndex = 0;						//Start over looking for the string from scratch.
				}
				else									//Otherwise if all characters i nthe tag thus far line up with the string
				{
					strIndex++;							//continue to compare it to the next index of the string
					candidateFound = true;				//and we have a running candidate.
				}
			}

            if (strIndex <= lastIndex) candidateFound = false;    //If we didn't read the full string, then it's false.

            return candidateFound;	//If by the end we found a candidate, the function returns true. Otherwise it returns false.
		}

		public void organizeByTheSearchTag()	//Sets the posts in the browse page to be the same list of posts but sorted by a given tage
		{
			List<Post> containingList = new List<Post>();		//The list of all posts that contain the tag, otherwise in the same order
			List<Post> notContainingList = new List<Post>();	//The list of all posts that do not contain the tag, otherwise in the same order

			for (int i = 0; i < posts.Count; i++)	//For all posts
			{
				if (containsTag(posts[i], SearchTag)) containingList.Add(posts[i]);	//Put the ones with the tag in one list
				else notContainingList.Add(posts[i]);								//and the others to the other
			}

			for (int i = 0; i < notContainingList.Count; i++)	//For all posts that do not contain the tag,
				containingList.Add(notContainingList[i]);		//append them to the end of the list with the tags.

			Posts = containingList;					//Set the posts that are in the browse page to be the posts in the new sorted order.
		}

		public void organizeByDateTime()
		{
//			Posts.Sort();	//I overloaded the < operator of Post in its class definition. This sorts by time, where earlier time points have higher precedence.
            Posts.Sort((x, y) => x.getStarDate().CompareTo(y.getStarDate()));

            List<Post> Temp = Posts;
            Posts = null;
            Posts = Temp;
		}

		public void organizeByDistance()    //This function sorts by distance from the user, using a lambda function that invokes the DistanceFromUser function in the Location class of the model.
		{
 //            Posts.Sort((x, y) => (Location.DistanceFromUser(x.Location) < Location.DistanceFromUser(y.Location)) ? -1 : ((Location.DistanceFromUser(x.Location) > Location.DistanceFromUser(y.Location)) ? 1 : 0));
            Posts.Sort((x, y) => Location.DistanceFromUser(x.Location).Result.CompareTo(Location.DistanceFromUser(y.Location).Result));

            List<Post> Temp = Posts;
            Posts = null;
            Posts = Temp;
        }

		public void organizeByRating()    //This function sorts posts by their rating relative to one another, uses a lambda implementation.
		{
			Posts.Sort((x, y) => x.Rating.CompareTo(y.Rating));

          List<Post> Temp = Posts;
			Posts = null;
			Posts = Temp;
		}
	}
}	
