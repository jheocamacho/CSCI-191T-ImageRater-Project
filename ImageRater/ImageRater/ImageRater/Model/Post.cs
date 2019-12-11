using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using SQLite;
using System.Runtime.CompilerServices;

namespace ImageRater.Model
{
    public class Post : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ID for each new post, saved in database
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private string location;
        private string datetime;
        private string tags;
        private int rating;
        private string photo;

        public Post()
        {
            location = "Default location";
            datetime = "Default datetime";
			tags = "Default tag";
            rating = 0;
            photo = "Default photo";
        }

        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChange(); }
        }

        public string DateTime
        {
            get { return datetime; }
            set { datetime = value; OnPropertyChange(); }
        }

         public string Tags
         {
             get { return tags; }
             set { tags = value; OnPropertyChange(); }
         }

		public int Rating
        {
            get { return rating; }
            set { rating = value; OnPropertyChange(); }
        }

        public string Photo
        {
            get { return photo; }
            set { photo = value; OnPropertyChange(); }
        }

		public static bool operator< (Post a, Post b)
		{
			//Example string: "Thursday, December 5, 2019 11:27 AM"
			//Weekday is Phase 1, we throw it away. Month is Phase 2, day is Phase 3, year is phase 4, time is phase 5, meridiem is phase 6.

			string aYear = "";
			string aMonth = "";
			string aDay = "";
			string aTime = "";
			string aMeridiem = "";

			string bYear = "";
			string bMonth = "";
			string bDay = "";
			string bTime = "";
			string bMeridiem = "";

			int phase = 1;
			for (int i = 0; i < a.DateTime.Length; i++)
			{
				if (phase == 1)
				{
					if (a.DateTime[i] == ' ') phase = 2;
				}
				else if (phase == 2)
				{
					if (a.DateTime[i] == ' ') phase = 3;
					else if (a.DateTime[i] == ',') continue;
					else aMonth += a.DateTime[i];
				}
				else if (phase == 3)
				{
					if (a.DateTime[i] == ' ') phase = 4;
					else if (a.DateTime[i] == ',') continue;
					else aDay += a.DateTime[i];
				}
				else if (phase == 4)
				{
					if (a.DateTime[i] == ' ') phase = 5;
					else aYear += a.DateTime[i];
				}
				else if (phase == 5)
				{
					if (a.DateTime[i] == ' ') phase = 6;
					else if (a.DateTime[i] == ':') aTime += ',';
					else aTime += a.DateTime[i];
				}
				else if (phase == 6)
				{
					if (a.DateTime[i] == ' ') break;
					else aMeridiem += a.DateTime[i];
				}
			}

			phase = 1;
			for (int i = 0; i < b.DateTime.Length; i++)
			{
				if (phase == 1)
				{
					if (b.DateTime[i] == ' ') phase = 2;
				}
				else if (phase == 2)
				{
					if (b.DateTime[i] == ' ') phase = 3;
					else if (b.DateTime[i] == ',') continue;
					else bMonth += b.DateTime[i];
				}
				else if (phase == 3)
				{
					if (b.DateTime[i] == ' ') phase = 4;
					else if (b.DateTime[i] == ',') continue;
					else bDay += b.DateTime[i];
				}
				else if (phase == 4)
				{
					if (b.DateTime[i] == ' ') phase = 5;
					else bYear += b.DateTime[i];
				}
				else if (phase == 5)
				{
					if (b.DateTime[i] == ' ') phase = 6;
					else if (b.DateTime[i] == ':') bTime += ',';
					else bTime += b.DateTime[i];
				}
				else if (phase == 6)
				{
					if (b.DateTime[i] == ' ') break;
					else bMeridiem += b.DateTime[i];
				}
			}

			int aYearValue =		Int32.Parse(aYear);
			int aMonthValue =		(aMonth == "January") ? 1 : ((aMonth == "February") ? 2 : ((aMonth == "March") ? 3 : ((aMonth == "April") ? 4 : ((aMonth == "May") ? 5 : ((aMonth == "June") ? 6 : ((aMonth == "July") ? 7 : ((aMonth == "August") ? 8 : ((aMonth == "September") ? 9 : ((aMonth == "October") ? 10 : ((aMonth == "November") ? 11 : 12))))))))));
			int aDayValue =			Int32.Parse(aDay);
			double aTimeValue =		Convert.ToDouble(aTime);
			if (aMeridiem == "PM")	aTimeValue += (double)12.00;

			int bYearValue = Int32.Parse(bYear);
			int bMonthValue = (bMonth == "January") ? 1 : ((bMonth == "February") ? 2 : ((bMonth == "March") ? 3 : ((bMonth == "April") ? 4 : ((bMonth == "May") ? 5 : ((bMonth == "June") ? 6 : ((bMonth == "July") ? 7 : ((bMonth == "August") ? 8 : ((bMonth == "September") ? 9 : ((bMonth == "October") ? 10 : ((bMonth == "November") ? 11 : 12))))))))));
			int bDayValue = Int32.Parse(bDay);
			double bTimeValue = Convert.ToDouble(bTime);
			if (bMeridiem == "PM") bTimeValue += (double)12.00;

			if (aYearValue != bYearValue)			return aYearValue < bYearValue;
			else if (aMonthValue != bMonthValue)	return aMonthValue < bMonthValue;
			else if (aDayValue != bDayValue)		return aDayValue < bDayValue;
			else if (aTimeValue != bTimeValue)		return aTimeValue < bTimeValue;
			else System.Diagnostics.Debug.WriteLine("Error in the Post < Post operator overload function. The inputs were: (" + a.DateTime + ") and (" + b.DateTime + ")");

			return false;
		}

		public static bool operator> (Post a, Post b)
		{
			return (!(a < b));
		}

		public double getStarDate()
		{
			//Example string: "Thursday, December 5, 2019 11:27 AM"
			//Weekday is Phase 1, we throw it away. Month is Phase 2, day is Phase 3, year is phase 4, time is phase 5, meridiem is phase 6.

			string aYear = "";
			string aMonth = "";
			string aDay = "";
			string aHours = "";
			string aMinutes = "";
			string aMeridiem = "";

			int phase = 1;
			for (int i = 0; i < DateTime.Length; i++)
			{
				if (phase == 1)
				{
					if (DateTime[i] == ' ') phase = 2;
				}
				else if (phase == 2)
				{
					if (DateTime[i] == ' ') phase = 3;
					else if (DateTime[i] == ',') continue;
					else aMonth += DateTime[i];
				}
				else if (phase == 3)
				{
					if (DateTime[i] == ' ') phase = 4;
					else if (DateTime[i] == ',') continue;
					else aDay += DateTime[i];
				}
				else if (phase == 4)
				{
					if (DateTime[i] == ' ') phase = 5;
					else aYear += DateTime[i];
				}
				else if (phase == 5)
				{
					if (DateTime[i] == ':') phase = 6;
					else aHours += DateTime[i];
				}
				else if (phase == 6)
				{
					if (DateTime[i] == ' ') phase = 7;
					else aMinutes += DateTime[i];
				}
				else if (phase == 7)
				{
					if (DateTime[i] == ' ') break;
					else aMeridiem += DateTime[i];
				}
			}

			double aTimeValue = (Convert.ToDouble(aHours) + (Convert.ToDouble(aMinutes) / 60d)) / 24d;
			if (aMeridiem == "PM") aTimeValue += (double)0.5;

			int aYearValue = Int32.Parse(aYear);

			int aMonthValue = (aMonth == "January") ? 0 : ((aMonth == "February") ? 31 : ((aMonth == "March") ? 59 : ((aMonth == "April") ? 90 : ((aMonth == "May") ? 120 : ((aMonth == "June") ? 151 : ((aMonth == "July") ? 181 : ((aMonth == "August") ? 212 : ((aMonth == "September") ? 243 : ((aMonth == "October") ? 273 : ((aMonth == "November") ? 304 : 334))))))))));
			if ((aYearValue % 4 == 0) && (aMonthValue >= 59)) aMonthValue++;	//Adding an extra day for leap years.

			int aDayValue = Int32.Parse(aDay);

			double timeOfYear = (aTimeValue + aDayValue + aMonthValue) / ((aYearValue % 4 == 0) ? 366 : 365);

			return aYearValue + timeOfYear;
		}
	}
}
