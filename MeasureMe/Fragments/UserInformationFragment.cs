﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;

namespace MeasureMe
{
	public class UserInformationFragment : Android.Support.V4.App.Fragment
	{
		// Spinner Objects
		private Spinner spinnerHeightinFeet;
		private Spinner spinnerHeightinInches;
		private Spinner spinnerActivityLevel;

		// User entered Objects - to be saved to database with user information
		//private EditText nameText;
		private RadioButton maleGender;
		private RadioButton femaleGender;
		private EditText ageText;
		private EditText weightText;

		// Button Objects
		private Button saveChanges;

		public async override void OnCreate(Bundle savedInstanceState)
		{
			try
			{
				base.OnCreate(savedInstanceState);

				HasOptionsMenu = true;

				// If the user already has data, pull into Cache.
				if (await SQLiteConnection.DoesUserDataExist())
				{
					// Populate user data, otherwise do nothing
					User currentUser = await SQLiteConnection.GetUserData();
					SetUIElements(currentUser);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Exception occurred during creation of fragment..." + ex.ToString());
			}
        }


		private void SetUIElements(User user)
		{

			// Set Age and Weight Texts
			ageText.Text = user.Age.ToString();
			weightText.Text = user.Weight.ToString();

			// Set the Height spinners
			spinnerHeightinFeet.SetSelection(Array.IndexOf(User.HeightInFeetOptions, user.HeightInFeet.ToString()));
			spinnerHeightinInches.SetSelection(Array.IndexOf(User.HeightInInchesOptions, user.HeightInInches.ToString()));

			// Set the activity spinner
			spinnerActivityLevel.SetSelection(Array.IndexOf(User.ActivityLevel, user.Activity));

			// Set the gender radio button
			bool UserIsMale = (user.MaleOrFemale == Gender.Male);

			if (UserIsMale)
			{
				maleGender.Checked = true;
				femaleGender.Checked = false;
			}
			else
			{
				maleGender.Checked = false;
				femaleGender.Checked = true;
			}
		}



		public enum SpinnerOptions
		{
			HeightInFeet,
			HeightInInches,
			ActivityLevel
		};

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			var view = inflater.Inflate(Resource.Layout.UserInformation, container, false);

			//nameText = view.FindViewById<EditText>(Resource.Id.editText_Name);
			maleGender = view.FindViewById<RadioButton>(Resource.Id.radioButton_Male);
			femaleGender = view.FindViewById<RadioButton>(Resource.Id.radioButton_Female);
			ageText = view.FindViewById<EditText>(Resource.Id.editText_Age);
			weightText = view.FindViewById<EditText>(Resource.Id.editText_Weight);
			saveChanges = view.FindViewById<Button>(Resource.Id.button_SaveChanges);

			spinnerHeightinFeet = view.FindViewById<Spinner>(Resource.Id.spinner_Height_Feet);
			spinnerHeightinInches = view.FindViewById<Spinner>(Resource.Id.spinner_Height_Inches);
			spinnerActivityLevel = view.FindViewById<Spinner>(Resource.Id.spinner_ActivityLevel);

			var adapter_Feet = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, User.HeightInFeetOptions);
			var adapter_Inches = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, User.HeightInInchesOptions);
			var adapter_ActivityLevel = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, User.ActivityLevel);

			adapter_Feet.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapter_Inches.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			adapter_ActivityLevel.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

			spinnerHeightinFeet.Adapter = adapter_Feet;
			spinnerHeightinInches.Adapter = adapter_Inches;
			spinnerActivityLevel.Adapter = adapter_ActivityLevel;

			saveChanges.Click += SaveChanges;

			return view;


		}

		private async void SaveChanges(object sender, EventArgs e)
		{
			if (await SaveUserIsSuccessfull())
			{
				var saveToast = Toast.MakeText(Context, Resource.String.toastString, ToastLength.Long);
				System.Diagnostics.Debug.WriteLine("This code has been called!");
				saveToast.Show();
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("There were errors found!");
			}
		}

		private async Task<bool> SaveUserIsSuccessfull()
		{
			bool isSuccess = false;

			if (UserValuesAreValid())
			{
				User user = new User();
				user.Age = int.Parse(ageText.Text);

				user.MaleOrFemale = maleGender.Checked ? Gender.Male : Gender.Female;
				user.Weight = int.Parse(weightText.Text);
				user.Activity = User.ActivityLevel[spinnerActivityLevel.SelectedItemPosition];
				user.ActivityMultiplier = User.ActivityLevelMultipliers[spinnerActivityLevel.SelectedItemPosition];
				user.HeightInFeet = int.Parse(User.HeightInFeetOptions[spinnerHeightinFeet.SelectedItemPosition]);
				user.HeightInInches = int.Parse(User.HeightInInchesOptions[spinnerHeightinInches.SelectedItemPosition]);


				// Save to the database
				await SQLiteConnection.CreateOrUpdateObject(user);

				var lastUser = await SQLiteConnection.GetUserData();

				System.Diagnostics.Debug.WriteLine("Latest user information: " + lastUser.ToString());
				isSuccess = true;
			}
			else
			{
				isSuccess = false;
			}

			return isSuccess;
		}

		private bool UserValuesAreValid()
		{
			try
			{
				bool UserIsValid = true;

				// Check if values are null or empty
				CheckTextForNullOrEmpty(ageText);
				CheckTextForNullOrEmpty(weightText);

				if (!string.IsNullOrEmpty(ageText.Error) || !string.IsNullOrEmpty(weightText.Error))
				{
					System.Diagnostics.Debug.WriteLine("An error is present");
					UserIsValid = false;
				}

				// Check for 'stupid' input
				int age = int.Parse(ageText.Text);
				int weight = int.Parse(weightText.Text);

				if (age > 100)
				{
					ageText.Error = "Are you sure you're over 100?";
					UserIsValid = false;
				}

				if (age < 8)
				{
					ageText.Error = "Are you sure you're only 8?";
					UserIsValid = false;
				}

				if (weight > 900)
				{
					weightText.Error = "Are you sure you're over 900lbs?";
					UserIsValid = false;
				}

				if (weight < 80)
				{
					weightText.Error = "Are you sure you're under 80lbs?";
					UserIsValid = false;
				}


				return UserIsValid;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("An error has occurred while validating the user's input:" + ex.ToString());
				return false;
			}

		}

		public void CheckTextForNullOrEmpty(EditText textBoxText)
		{
			if (string.IsNullOrEmpty(textBoxText.Text))
			{
				textBoxText.Error = "This field can not be blank";
			}
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate(Resource.Menu.TestMenu, menu);
		}

	}
}

