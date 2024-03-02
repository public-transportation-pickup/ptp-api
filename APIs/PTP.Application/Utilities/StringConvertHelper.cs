namespace PTP.Application.Utilities;
public static class StringConvertHelper
{
	public static bool CheckDayActive(this string dates)
	{
		if (string.IsNullOrEmpty(dates)) return false;
		var applyDates = dates.Split(",").ToList().ConvertAll(x => x.Trim());
		string result = "";
		applyDates.ForEach(x =>
		{
			switch (x)
			{
				case "T2":
					result += DayOfWeek.Monday.ToString();
					break;
				case "T3":
					result += DayOfWeek.Tuesday.ToString();
					break;
				case "T4":
					result += DayOfWeek.Wednesday.ToString();
					break;
				case "T5":
					result += DayOfWeek.Thursday.ToString();
					break;
				case "T6":
					result += DayOfWeek.Friday.ToString();
					break;
				case "T7":
					result += DayOfWeek.Saturday.ToString();
					break;
				case "CN":
					result += DayOfWeek.Sunday.ToString();
					break;
				default:
					break;
			}
		});
		return result.Contains(DateTime.Now.DayOfWeek.ToString());
	}
	public static List<TimeSpan> ConvertToTimeSpanList(this string s)
	{
		ArgumentException.ThrowIfNullOrEmpty(s);
		return s.Trim().Split("-").ToList().ConvertAll(TimeSpan.Parse).ToList();
	}
	public static double ConvertAverageTime(this string s)
	{
		if (s is null) return 0;
		if (s.Length > 2)
		{
			var arrTime = s.Trim().Split("-").ToList().ConvertAll(x => double.Parse(x));

			double result = 0;
			foreach (var time in arrTime)
			{
				result += time;
			}
			return result / 2;

		}
		else
			return double.Parse(s);
	}

}