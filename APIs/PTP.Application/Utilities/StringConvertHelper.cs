namespace PTP.Application.Utilities;
public static class StringConvertHelper
{
    public static double ConvertAverageTime(this string s)
    {
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