using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Hadi.Cms.Infrastructure.Helpers
{
    public static class HttpHelper
    {
        public static byte[] PostByWebClient(string address, NameValueCollection values)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(address, values);
            }
            return response;
        }

        public static string PostByHttpClient(string address, FormUrlEncodedContent content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(address);

                var result = client.PostAsync("", content);
                string resultContent = result.Result.ToString();

                return resultContent;
            }
        }

        public static string PostByHttpWebRequest(string address, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();

            return result;
        }
    }
}
