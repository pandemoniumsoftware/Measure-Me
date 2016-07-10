using System;

namespace MeasureMe
{
	public class ViewPagerFragmentAdapter : Android.Support.V4.App.FragmentPagerAdapter
	{
		private Android.Support.V4.App.Fragment[] _fragments;
		private readonly string[] _titles = { "Log", "Favorites", "User" };

		public ViewPagerFragmentAdapter(Android.Support.V4.App.FragmentManager fm, Android.Support.V4.App.Fragment[] fragments) : base(fm)
		{
			this._fragments = fragments;
		}

		public override int Count
		{
			get
			{
				return _fragments.Length;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return _fragments[position];
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
		{
			return new Java.Lang.String(_titles[position]);
		}


	}
}

