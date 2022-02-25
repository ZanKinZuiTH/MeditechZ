using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShareLibrary
{
    public static class IcheckupHelper
    {
        public readonly static string BaseAddress;
        public readonly static int timeOut = 3;
        static IcheckupHelper()
        {
            BaseAddress = System.Configuration.ConfigurationManager.AppSettings["IcheckupAddress"];
        }

        public static T Get<T>(string requestUrl) where T : class
        {
            T result = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = client.GetAsync(requestUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {

                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                }

            }
            return result;
        }

        public static object Post<T>(string requestUrl, T obj) where T : class
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                using (var response = client.PostAsJsonAsync(requestUrl, obj).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {
                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                    return response.Content.ReadAsAsync<object>().Result;
                }

            }
        }

        public static object Post<T>(string requestUrl, int timeOut, T obj) where T : class
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                using (var response = client.PostAsJsonAsync(requestUrl, obj).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {
                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                    return response.Content.ReadAsAsync<object>().Result;
                }

            }
        }

        public static TResult Post<T, TResult>(string requestUrl, T obj) where T : class
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                using (var response = client.PostAsJsonAsync(requestUrl, obj).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {
                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                    return response.Content.ReadAsAsync<TResult>().Result;
                }

            }
        }

        public static async Task SendAsync<T>(string requestUrl, T obj) where T : class
        {

            using (var client = new HttpClient())
            {
                var serializer = new JsonSerializer();
                var header = new MediaTypeWithQualityHeaderValue("application/json");

                client.DefaultRequestHeaders.Accept.Add(header);
                client.BaseAddress = new Uri(BaseAddress);
                string requestApi = requestUrl;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestApi);
                var json = JsonConvert.SerializeObject(obj);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = stringContent;

                await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ContinueWith(
                    (response) =>
                    {

                        using (var stream = response.Result.Content.ReadAsStreamAsync().Result)
                        using (var sr = new StreamReader(stream))
                        using (var jr = new JsonTextReader(sr))
                        {
                            while (jr.Read())
                            {
                                // Don't worry about commas.
                                // JSON reader will handle them for us.
                                //if (jr.TokenType != JsonToken.StartArray && jr.TokenType != JsonToken.EndArray)
                                //{

                                //}
                            }
                        }

                    });

            }
        }
        public static void Put<T>(string requestUrl, T obj)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                using (var response = client.PutAsJsonAsync(requestUrl, obj).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {
                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                }

            }
        }

        public static void Put(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                using (var response = client.PutAsync(requestUrl, null).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {
                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                }

            }
        }

        public static void Delete(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(timeOut);
                client.BaseAddress = new Uri(BaseAddress);
                using (var response = client.DeleteAsync(requestUrl).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = string.Empty;
                        var errors = response.Content.ReadAsAsync<HttpError>().Result;
                        foreach (var e in errors)
                        {
                            msg += string.Format("{0}: {1}", e.Key, e.Value) + Environment.NewLine;
                        }
                        throw new Exception(msg);
                    }
                }

            }
        }

    }
}
