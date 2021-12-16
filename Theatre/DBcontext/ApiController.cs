using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.DBcontext
{
    public class ApiController
    {

        private const string connect = "https://filmapiplan.azurewebsites.net/api";

        public static async Task<string> GetRequest(string table)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage message = await client.GetAsync(connect + "/" + table);
            message.EnsureSuccessStatusCode();
            return await message.Content.ReadAsStringAsync();
        }


        public static async Task<string> PostRequest(string table, string json)
        {
            try
            {

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json,Encoding.UTF8,"application/json");
            HttpResponseMessage message = await client.PostAsync(connect + "/" + table,content);
            message.EnsureSuccessStatusCode();
            return await message.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { return "Ошибка: " + ex.Message; }
        }

        public static async Task<string> PutRequest(string table, string json, int id)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await client.PutAsync(connect + "/" + table + "/" + id, content);
            return message.StatusCode.ToString();
        }

        public static async Task<string> DeleteRequest(string table, int id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage message = await client.DeleteAsync(connect + "/" + table + "/" + id);
            return message.StatusCode.ToString();
        }

    }
}
