using Library;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace MeteorologicalDiaryServer
{
    public class RequestProcessing
    {
        private readonly TcpClient _client;
        private readonly FileProcessing _file;

        public RequestProcessing(TcpClient client)
        {
            _client = client;
            _file = new FileProcessing("text.txt");
        }

        public void Process()
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                var buffer = new byte[2048];
                while (true)
                {
                    // получаем сообщение
                    var builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(buffer, 0, buffer.Length);
                        builder.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    var message = builder.ToString();
                    var request = JsonConvert.DeserializeObject<Request>(message);

                    var response = ProcessRequest(request);
                    var serializedResponse = JsonConvert.SerializeObject(response);

                    buffer = Encoding.UTF8.GetBytes(serializedResponse);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_client != null)
                    _client.Close();
            }
        }

        private Response ProcessRequest(Request request)
        {
            if (request is null)
            {
                return null;
            }

            switch (request.Action)
            {
                case CRUDEnum.Create:
                    _file.Create(JsonConvert.DeserializeObject<DiaryNote>(request.JsonRequest));
                    return new Response() { IsSuccess = true };

                case CRUDEnum.Read:
                    return new Response() { IsSuccess = true, Diary = _file.ReadFromFile() };

                case CRUDEnum.Update:
                    _file.Update(JsonConvert.DeserializeObject<DiaryNote>(request.JsonRequest));
                    return new Response() { IsSuccess = true };

                case CRUDEnum.Delete:
                    return new Response() { IsSuccess = _file.Delete(int.Parse(request.JsonRequest)) };

                case CRUDEnum.Filter:
                    return new Response() { IsSuccess = true, Diary = _file.Filter(request.Direction) };

                default:
                    break;
            }

            return null;
        }
    }
}
