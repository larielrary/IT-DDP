using Library;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace MeteorologicalDiaryClient
{
    class ClientRequest
    {
        private const int Port = 8080;
        private const string Server = "127.0.0.1";

        public Response InputProcessing()
        {
            Console.WriteLine("1.Add note");
            Console.WriteLine("2.Delete note");
            Console.WriteLine("3.Update note");
            Console.WriteLine("4.Get all notes");
            Console.WriteLine("5.Filter");

            var choise = int.Parse(Console.ReadLine());

            switch (choise)
            {
                case 1:
                    return Sending(new Request { Action = CRUDEnum.Create, JsonRequest = JsonConvert.SerializeObject(Edit()) });

                case 2:
                    return Sending(new Request { Action = CRUDEnum.Delete, JsonRequest = GetId() });

                case 3:
                    return Sending(new Request { Action = CRUDEnum.Update, JsonRequest = JsonConvert.SerializeObject(Edit()) });

                case 4:
                    return Sending(new Request { Action = CRUDEnum.Read });

                case 5:
                    return Sending(GetFilterValue());

                default:
                    return null;
            }
        }

        private Response Sending(Request request)
        {
            TcpClient client = new TcpClient();
            client.Connect(Server, Port);

            NetworkStream stream = client.GetStream();
            var serializedRequest = JsonConvert.SerializeObject(request);
            stream.Write(Encoding.UTF8.GetBytes(serializedRequest));

            if (request.Action == CRUDEnum.Read || request.Action == CRUDEnum.Filter)
            {
                var buffer = new byte[2048];
                var builder = new StringBuilder();
                do
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                }
                while (stream.DataAvailable);

                var message = builder.ToString();
                var response = JsonConvert.DeserializeObject<Response>(message);

                return response;
            }

            return new Response { IsSuccess = true };
        }

        public static DiaryNote Edit()
        {
            Console.WriteLine("Input id");
            var id = int.Parse(Console.ReadLine());

            Console.WriteLine("Input date");
            var date = DateTime.Parse(Console.ReadLine());
            
            Console.WriteLine("Input pressure");
            var pressure = int.Parse(Console.ReadLine());

            Console.WriteLine("Input cloudness");
            CloudnessEnum cloudness = (CloudnessEnum)Enum.Parse(typeof(CloudnessEnum),Console.ReadLine());

            Console.WriteLine("Input wind direction");
            var windDirection = Console.ReadLine();

            return new DiaryNote
            {
                Id = id,
                Date = date,
                Pressure = pressure,
                Cloudness = cloudness,
                WindDirection = windDirection
            };
        }

        private static Request GetFilterValue()
        {
            Console.WriteLine("Input wind direction");
            var direction = Console.ReadLine();

            return new Request { Action = CRUDEnum.Filter, Direction = direction};
        }

        private static string GetId()
        {
            Console.WriteLine("Input id");

            return Console.ReadLine();
        }
    }
}
