using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MeteorologicalDiaryServer
{
    class Program
    {
        private const int Port = 8080;
        private const string Address = "127.0.0.1";
        private static TcpListener _server;
        static void Main(string[] args)
        {
            try
            {
                _server = new TcpListener(IPAddress.Parse(Address), Port);
                _server.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    var client = _server.AcceptTcpClient();
                    var clientObject = new RequestProcessing(client);

                    var clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
