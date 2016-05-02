using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Typeform_Attendance
{
    class Main
    {
        private static Rootobject rootObject;
        public static void getRootobject(string url)
        {
            Task<Rootobject> task = DownloadData(url);
            task.Wait();
            Rootobject temp = task.Result;
            rootObject = temp;
        }

        async private static Task<Rootobject> DownloadData(string url)
        {
            using (var w = new HttpClient())
            {
                // attempt to download JSON data as a string
                string responseBody = String.Empty;
                try
                {
                    var response = await w.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false);
                   response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                System.Diagnostics.Debug.WriteLine(responseBody);
                return !string.IsNullOrEmpty(responseBody) ? JsonConvert.DeserializeObject<Rootobject>(responseBody) : new Rootobject(); ;
            }
        }
    }
}
