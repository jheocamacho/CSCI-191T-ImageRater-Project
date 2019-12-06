using System;
using System.Collections.Generic;
using System.Text;

namespace ImageRater.ViewModel
{
    public static class ViewModelLocator
    {
        // to allow for the same viewmodel object to be used by multiple pages
        private static PostViewModel mainViewModel = new PostViewModel();
        public static PostViewModel MainViewModel
        {
            get
            {
                return mainViewModel;
            }
        }
    }
}
