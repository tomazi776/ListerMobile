using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ListerMobile.RestClient
{
    public class RestClient<T>
    {
        private const string WEB_SERVICE_PATH = "/api/ShoppingLists/";
        private const string WEB_SERVICE_URI = "http://sample1app.azurewebsites.net/";
        private const int PORT = 56085;

        public async Task<ObservableCollection<T>> GetAsync()       // Zmienić typ Taska na generyczny dla resty klasy
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = await client.GetStringAsync(WEB_SERVICE_PATH);
                    var taskModels = JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
                    return taskModels;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }


        public async Task<bool> PostAsync(T t)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = JsonConvert.SerializeObject(t);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await client.PostAsync(WEB_SERVICE_PATH, httpContent);

                    return result.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }

        }


        public async Task<bool> PutAsync(int id, T t)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = JsonConvert.SerializeObject(t);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await client.PutAsync(WEB_SERVICE_PATH + id, httpContent);

                    return result.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var client = new HttpClient())

            {
                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT + WEB_SERVICE_PATH);


                    var response = await client.DeleteAsync(WEB_SERVICE_PATH + id);

                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }


            }

        }
    }
}
