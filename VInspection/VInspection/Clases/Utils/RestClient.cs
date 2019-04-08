using Android.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VInspection.Clases.Utils
{
    /// <summary>
    /// RestClient implements methods for calling CRUD operations using HTTP
    /// </summary>

    public class RestClient<T>
    {
        public const string TAG = "DEBUG LOG";

        string WebServiceUrl;

        public async Task<List<T>> GetAsync(string tipo)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl);

            Log.Info(TAG, "La data recibida en JSON es: \n");
            Log.Info(TAG, json);

            var taskModel = JsonConvert.DeserializeObject<List<T>>(json);
            return taskModel;
        }

        public async Task<T> GetAsyncById(string tipo, int id)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            Log.Info(TAG, "El Id del " + tipo + " a obtener es: \n");
            Log.Info(TAG, id.ToString());

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + id);

            Log.Info(TAG, "La data recibida en JSON es: \n");
            Log.Info(TAG, json);

            var taskModel = JsonConvert.DeserializeObject<T>(json);
            return taskModel;
        }

        public async Task<bool> PostAsync(string tipo, T t)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            Log.Info(TAG, "La data a enviar en JSON es: \n");
            Log.Info(TAG, json);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);

            Log.Info(TAG, "La respuesta es: \n");
            Log.Info(TAG, result.ToString());
            
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string tipo, int id, T t)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            Log.Info(TAG, "La data a actualizar en JSON es: \n");
            Log.Info(TAG, json);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(WebServiceUrl + id, httpContent);

            Log.Info(TAG, "La respuesta es: \n");
            Log.Info(TAG, result.ToString());

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string tipo, int id)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            Log.Info(TAG, "El Id del " + tipo + " a eliminar es: \n");
            Log.Info(TAG, id.ToString());

            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(WebServiceUrl + id);

            return response.IsSuccessStatusCode;
        }

        
        public async Task<T> GetUserByAccount(string tipo, string account)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + "GetUserByAccount/" + account);

            Log.Info(TAG, "El usuario recibido en JSON es: \n");
            Log.Info(TAG, json);

            var taskModel = JsonConvert.DeserializeObject<T>(json);
            return taskModel;
        }

        public async Task<List<T>> GetUsersName(string tipo)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + "GetUsersName");

            Log.Info(TAG, "Los usuarios recibido en JSON es: \n");
            Log.Info(TAG, json);

            var taskModel = JsonConvert.DeserializeObject<List<T>>(json);
            return taskModel;
        }

        public async Task<T> GetDocumentsByPlate(string tipo, string placa)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + "GetDocumentsByPlate/" + placa);

            Log.Info(TAG, "Los documentos del Vehiculo en JSON es: \n");
            Log.Info(TAG, json);

            var taskModel = JsonConvert.DeserializeObject<T>(json);
            return taskModel;
        }

        public async Task<List<T>> GetCheckListSummariesByBrowser(string tipo, string dato)
        {
            WebServiceUrl = UServices.getUrl(tipo);

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + "GetCheckListSummariesByBrowser/" + dato);

            Log.Info(TAG, "Los preusos del día en JSON son: \n");
            Log.Info(TAG, json);

            var taskModel = JsonConvert.DeserializeObject<List<T>>(json);
            return taskModel;
        }
    }
}