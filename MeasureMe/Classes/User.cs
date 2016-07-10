using System;
using SQLite;

namespace MeasureMe
{
	public enum Gender
	{
		Male,
		Female
	};

	public class User
	{
		public static string[] ActivityLevel = { "Sedentary", "Light Activity", "Moderately Active", "Very Active" };
		public static float[] ActivityLevelMultipliers = { 1.2f, 1.375f, 1.550f, 1.725f};
		public static string[] HeightInFeetOptions = { "3", "4", "5", "6", "7" };
		public static string[] HeightInInchesOptions = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11" };

		[PrimaryKey]
		private int ID { get; set; }

		public string Activity { get; set; }

		public float ActivityMultiplier { get; set; }

        public int Weight { get; set; }

		public int Age { get; set; }

		public Gender MaleOrFemale { get; set; }

		public int HeightInFeet { get; set; }

		public int HeightInInches { get; set; }

		[Ignore]
		private int _TDEE { get; set; }

		public int TDEE
		{
			get
			{
				UpdateTDEE();
				return _TDEE;
			}
			set
			{
				value = _TDEE;
			}
		}


		/// <summary>
		/// Updates the TDEE calculation
		/// </summary>
		/// <returns>Total energy expenditure guess</returns>
		private int UpdateTDEE()
		{
			int tdee = 0;
			float HeightInCM = GetHeightinCM();

			if (HeightInCM <= 0)
				throw new ArgumentException(nameof(HeightInCM), "Height must be a positive value!");

			float maleOrFemale = MaleOrFemale == Gender.Female ? -161 : 5;
			float eqn = (10 * Weight) + (6.25f * HeightInCM) - 5 * (Age) + maleOrFemale;

			// Truncating
			tdee = (int)eqn;

			return tdee;

		}

		private float GetHeightinCM()
		{
			if (HeightInFeet <= 0 || HeightInInches < 0)
				throw new ArgumentException((nameof(HeightInFeet) + " and " + nameof(HeightInInches)), "Height must be positive.");

			const float feetToCM = 30.48f;
			const float InchToCM = 2.54f;

			return (((float)HeightInFeet * feetToCM) + ((float)HeightInInches * InchToCM));
        }

		public User()
		{
		}

		public User(int Weight, int Age, Gender MaleOrFemale, int HeightInFeet, int HeightInInches)
		{
			this.Weight = Weight;
			this.MaleOrFemale = MaleOrFemale;
			this.HeightInFeet = HeightInFeet;
			this.HeightInInches = HeightInInches;
		}

		public override string ToString()
		{
			return string.Format("[User: Activity={0}, ActivityMultiplier={1}, Weight={2}, Age={3}, MaleOrFemale={4}, HeightInFeet={5}, " +
			                     "HeightInInches={6}, TDEE={7}]", Activity, ActivityMultiplier, Weight, Age, MaleOrFemale, HeightInFeet, HeightInInches, TDEE);
		}
	}
}

