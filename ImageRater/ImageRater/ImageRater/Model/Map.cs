using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace ImageRater.Model
{
    static class Map
    {
        public static async Task OpenPlacemarkOnMap(Placemark placemark)
        {
            await placemark.OpenMapsAsync();
        }
    }
}
