using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace Theatre.DBcontext
{
    public class Converter : ApiController
    {

        public static async Task<ObservableCollection<T>> Getter<T>(string table)
        {
            string response = await GetRequest(table);
            return (ObservableCollection<T>)JsonConvert.DeserializeObject(response, typeof(ObservableCollection<T>));
        }

        public static async Task<ObservableCollection<T>> Creatter<T>(string table, object model)
        {

            string json = JsonConvert.SerializeObject(model);
            string response = await PostRequest(table, json);
            if (response.Contains("Ошибка: "))
            {
                return null;
            }
            return await Getter<T>(table);
        }

        public static async Task<string> Updatter(string table, object model, int id)
        {
            string json = JsonConvert.SerializeObject(model);
            string response = await PutRequest(table,json,id);
            if (response == HttpStatusCode.NoContent.ToString())
            {
                return "Данные успешно обновлены!";
            }
            else
            {
                return "Данные не обновлены!";
            }
        }

        public static async Task<string> Deletter(string table, int id)
        {
            string response = await DeleteRequest(table, id);
            if (response == HttpStatusCode.NoContent.ToString())
            {
                return "Данные успешно удалены!";
            }
            else
            {
                return "Данные не удалены!";
            }
        }

    }
}
