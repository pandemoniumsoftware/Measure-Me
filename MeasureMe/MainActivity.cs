using Android.App;
using Android.Widget;
using Android.OS;

namespace MeasureMe
{
	[Activity(Label = "Measure Me", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Android.Support.V4.App.FragmentActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var fragments = new Android.Support.V4.App.Fragment[] {
				new FavoritesFragment(),
				new LogFragment(),
				new UserInformationFragment()
			};

			var viewPager = FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.viewPager);

			viewPager.Adapter = new ViewPagerFragmentAdapter(base.SupportFragmentManager, fragments);
		}
	}
}


