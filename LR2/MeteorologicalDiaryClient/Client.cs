using System.Net.Http;
using System.Text;

namespace MeteorologicalDiaryClient
{
    public class Client
    {
        private readonly string _url;
        public Client(string url)
        {
            _url = url;
        }
        public string ProcessRequest(string path, HttpMethod method, string content)
        {
            using (var client = new HttpClient())
            {
                var message = new HttpRequestMessage(method, _url + path);

                if (content != null)
                {
                    message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                }

                var response = client.SendAsync(message);
                var resultAsync = response.Result.Content.ReadAsStringAsync();
                resultAsync.Wait();

                return resultAsync.Result;
            }
        }
    }
}
