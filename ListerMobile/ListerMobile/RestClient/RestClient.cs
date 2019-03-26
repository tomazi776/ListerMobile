using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ListerMobile.RestClient
{
    public class RestClient<T>
    {
        private const string WEB_SERVICE_PATH = "/api/ShoppingLists/";
        private const string WEB_SERVICE_URI = "http://localhost:";
        private const int PORT = 56085;

        public async Task<List<T>> GetAsync()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = await client.GetStringAsync(WEB_SERVICE_PATH);
                    var dupal = "aaaaa";
                    var taskModels = JsonConvert.DeserializeObject<List<T>>(json);
                    return taskModels;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            //var httpClient = new HttpClient();

            //var json = await httpClient.GetStringAsync(WEB_SERVICE_PATH);
            //var dupal = "aaaaa";
            //var taskModels = JsonConvert.DeserializeObject<List<T>>(json);

            //return taskModels;
            return null;
        }


        public async Task<bool> PostAsync(T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WEB_SERVICE_PATH, httpContent);

            return result.IsSuccessStatusCode;
        }


        public async Task<bool> PutAsync(int id, T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(WEB_SERVICE_PATH + id, httpContent);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, T t)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(WEB_SERVICE_PATH + id);

            return response.IsSuccessStatusCode;
        }

    }
}
