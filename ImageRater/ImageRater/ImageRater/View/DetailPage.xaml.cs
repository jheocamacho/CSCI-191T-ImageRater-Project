using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageRater.Model;
using ImageRater.ViewModel;

namespace ImageRater.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.MainViewModel;   
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // the viewmodel command will remove the post from the database
            // this clicked function will handle going back to the previous page afterwards
            Navigation.PopAsync();
        }
    }
}