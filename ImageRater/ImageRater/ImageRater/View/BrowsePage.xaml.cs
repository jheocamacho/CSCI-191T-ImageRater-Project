using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageRater.ViewModel;
using ImageRater.Model;

namespace ImageRater.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowsePage : ContentPage
    {
        public BrowsePage()
        {
            InitializeComponent();            
        }

        async private void PostList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // event handler will take in the Post object that's provided by the listview
            Post selectedPost = e.Item as Post;

            // then assign it to the viewmodel's CurrentPost
            ((PostViewModel)(this.BindingContext)).CurrentPost = selectedPost;

            // finally, view that post's page
            await Navigation.PushAsync(new DetailPage());            
        }
    }
}