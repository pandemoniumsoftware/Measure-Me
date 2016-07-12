
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace MeasureMe
{
	public class HomeFragment : Android.Support.V4.App.Fragment
	{


		private AppCompatTextView dateText;
		private AppCompatTextView TDEEText;

		// Non graphical behind the scenes here--all graphics / UI related goes into onCreateView
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			System.Diagnostics.Debug.WriteLine("EXITING ONCREATE: " + MeasureMe.CurrentUser);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			System.Diagnostics.Debug.WriteLine("OnCreateView: " + MeasureMe.UserDataExists);
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			var view = inflater.Inflate(Resource.Layout.Home, container, false);

			dateText = view.FindViewById<AppCompatTextView>(Resource.Id.dateText);
			TDEEText = view.FindViewById<AppCompatTextView>(Resource.Id.TDEEText);

			dateText.Text = "Today's Date: " + DateTime.Now.ToShortDateString();

			if (MeasureMe.UserDataExists)
			{
				TDEEText.Text = "Your calorie target: " + MeasureMe.CurrentUser.TDEE;
			}
			else
			{
				TDEEText.Text = "Your calorie target: N/A";
			}
			return view;
		}



	}
}

