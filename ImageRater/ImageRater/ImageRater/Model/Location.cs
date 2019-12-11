using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ImageRater.Model
{
    static class Location
    {
        public static async Task<Xamarin.Essentials.Location> GetCurrentPosition()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                return location;
            }
            else
            {
                return null;
            }
        }

        public static async Task<Xamarin.Essentials.Location> GeocodeLocation(string address)
        {
            var locations = await Geocoding.GetLocationsAsync(address);

            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                return location;
            }
            else
            {
                return null;
            }
        }

        public static async Task<Placemark> ReverseGeocodeLocation(Xamarin.Essentials.Location currentLocation)
        {
            var lat = currentLocation.Latitude;
            var lon = currentLocation.Longitude;

            var placemarks = await Geocoding.GetPlacemarksAsync(lat, lon);
            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                var geocodeAddress =
                    $"AdminArea:       {placemark.AdminArea}\n" +
                    $"CountryCode:     {placemark.CountryCode}\n" +
                    $"CountryName:     {placemark.CountryName}\n" +
                    $"FeatureName:     {placemark.FeatureName}\n" +
                    $"Locality:        {placemark.Locality}\n" +
                    $"PostalCode:      {placemark.PostalCode}\n" +
                    $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                    $"SubLocality:     {placemark.SubLocality}\n" +
                    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    $"Thoroughfare:    {placemark.Thoroughfare}\n";

                Debug.WriteLine(geocodeAddress);

                return placemark;
            }
            else
            {
                return null;
            }
        }

        // receive the post address string and return the distance in miles as a double between there and the user
        public static double DistanceFromUser(string postAddress)
        {
            // get user's location
            var userLocation = GetCurrentPosition().Result;

            // get post's location (from geocoding address string)
            var postLocation = GeocodeLocation(postAddress).Result;

            // built-in function to determine the distance in miles
            double distance = Xamarin.Essentials.Location.CalculateDistance(userLocation, postLocation, DistanceUnits.Miles);
            
            return distance;
        }
    }
}