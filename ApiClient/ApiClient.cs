using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiClient
{
    public class ApiClient
    {
        static HttpClient client = new HttpClient();

        public static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://apidev.gewaer.io/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<List<Lead>> GetLeadsAsync(string path)
        {
            List<Lead> leads = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                leads = JsonConvert.DeserializeObject<List<Lead>>(jsonString);
            }

            return leads;
        }
    }
}
