using System;
using System.Collections.Generic;
using System.Text;

namespace Hadi.Cms.Infrastructure.Helpers.GoogleMapsHelper
{
    public class PolylineProvider
    {
        public static List<Location> DecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints)) return null;
            List<Location> poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location();
                    p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                    p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // logo it
            }
            return poly;
        }

        public static string EncodePoints(string points)
        {
            if (string.IsNullOrEmpty(points))
            {
                return "";
            }

            var str = new StringBuilder();

            var encodeDiff = (Action<int>)(diff =>
            {
                int shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;
                int rem = shifted;
                while (rem >= 0x20)
                {
                    str.Append((char)((0x20 | (rem & 0x1f)) + 63));
                    rem >>= 5;
                }
                str.Append((char)(rem + 63));
            });

            string[] stringSeparators = new string[] { "\r\n" };
            var locations = points.Replace("\t\t\t\t\t", "").Split(stringSeparators, StringSplitOptions.None);

            int lastLat = 0;
            int lastLng = 0;
            foreach (var location in locations)
            {
                if (!string.IsNullOrEmpty(location))
                {
                    int lat = (int)Math.Round(double.Parse(location.Split(',')[0].Replace(".", "/"), System.Globalization.NumberStyles.Float) * 1E5);
                    int lng = (int)Math.Round(double.Parse(location.Split(',')[1].Replace(".", "/"), System.Globalization.NumberStyles.Float) * 1E5);
                    encodeDiff(lat - lastLat);
                    encodeDiff(lng - lastLng);
                    lastLat = lat;
                    lastLng = lng;
                }
            }
            return str.ToString();
        }
    }
}
