using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public ApiClient(string uri)
        {
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        //Makes a GET request to the given path and returns a object with the result
        public async Task<object> GetAsync(string path)
        {
            object obj = new object();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse("{\'data\' : " + jsonString + '}');
                obj = result.data;
            }

            return obj;
        }
    }
}
