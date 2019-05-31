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

        private const string WEB_SERVICE_URI = "http://sample1app.azurewebsites.net/";
        private const int PORT = 56085;

        public async Task<ObservableCollection<T>> GetAsync(string servicePath)       // Zmienić typ Taska na generyczny dla resty klasy
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = await client.GetStringAsync(servicePath);        //replace with func call
                    var taskModels = JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
                    return taskModels;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TU MASZ BŁĄD TUMOKU:" + ex.Message);
            }
            return null;
        }


        public async Task<bool> PostAsync(T t, string servicePath)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = JsonConvert.SerializeObject(t);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await client.PostAsync(servicePath, httpContent);      //replace with func call

                    return result.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }

        }


        public async Task<bool> PutAsync(int id, T t, string servicePath)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT);

                    var json = JsonConvert.SerializeObject(t);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await client.PutAsync(servicePath + id, httpContent);      //replace with func call

                    return result.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }

        }

        public async Task<bool> DeleteAsync(int id, string servicePath)
        {
            using (var client = new HttpClient())

            {
                try
                {
                    client.BaseAddress = new Uri(WEB_SERVICE_URI + PORT + servicePath);     //replace with func call


                    var response = await client.DeleteAsync(servicePath + id);      //replace with func call

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
