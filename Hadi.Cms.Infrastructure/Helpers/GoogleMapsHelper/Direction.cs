using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Hadi.Cms.Infrastructure.Helpers.GoogleMapsHelper
{
    public class Direction
    {
        public static ResponseDirection GetDirection(string originLocation, string destinationLocation, string travelMode = "DRIVING", string apiKey = "AIzaSyBT7sC0O_yIKwYPoTV7NVHgNAzCxvJCbpw")
        {
            WebClient webClient = new WebClient();

            webClient.QueryString.Add("origin", originLocation);
            webClient.QueryString.Add("destination", destinationLocation);
            webClient.QueryString.Add("mode", travelMode);
            webClient.QueryString.Add("key", apiKey);

            string jsonResult = webClient.DownloadString("https://maps.googleapis.com/maps/api/directions/json");

            ResponseDirection result = JsonConvert.DeserializeObject<ResponseDirection>(jsonResult);

            result.completedPath = new List<Location>();

            foreach (var step in result.routes[0].legs[0].steps)
            {
                var locations = PolylineProvider.DecodePolylinePoints(step.polyline.points);

                foreach (var location in locations)
                {
                    result.completedPath.Add(new Location()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    });
                }
            }

            result.jsonResult = jsonResult;
            result.totalDistance = result.routes[0].legs[0].distance.text;
            result.totalDuration = result.routes[0].legs[0].duration.text;
            result.totalDistanceInMeters = result.routes[0].legs[0].distance.value;
            result.totalDurationInSeconds = result.routes[0].legs[0].duration.value;

            return result;
        }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class EndLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class StartLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Distance2
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration2
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class EndLocation2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Polyline
    {
        public string points { get; set; }
    }

    public class StartLocation2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Step
    {
        public Distance2 distance { get; set; }
        public Duration2 duration { get; set; }
        public EndLocation2 end_location { get; set; }
        public string html_instructions { get; set; }
        public Polyline polyline { get; set; }
        public StartLocation2 start_location { get; set; }
        public string travel_mode { get; set; }
        public string maneuver { get; set; }
    }

    public class Leg
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string end_address { get; set; }
        public EndLocation end_location { get; set; }
        public string start_address { get; set; }
        public StartLocation start_location { get; set; }
        public List<Step> steps { get; set; }
        public List<object> via_waypoint { get; set; }
    }

    public class OverviewPolyline
    {
        public string points { get; set; }
    }

    public class Route
    {
        public Bounds bounds { get; set; }
        public string copyrights { get; set; }
        public List<Leg> legs { get; set; }
        public OverviewPolyline overview_polyline { get; set; }
        public string summary { get; set; }
        public List<object> warnings { get; set; }
        public List<object> waypoint_order { get; set; }
    }

    public class ResponseDirection
    {
        public List<Route> routes { get; set; }

        public string status { get; set; }

        public string jsonResult { get; set; }

        public List<Location> completedPath { get; set; }

        public string totalDistance { get; set; }

        public string totalDuration { get; set; }

        public int totalDistanceInMeters { get; set; }

        public int totalDurationInSeconds { get; set; }
    }
}
