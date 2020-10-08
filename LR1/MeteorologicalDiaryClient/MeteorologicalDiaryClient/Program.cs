using Library;
using System;

namespace MeteorologicalDiaryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new ClientRequest();
            while (true)
            {
                var response = menu.InputProcessing();
                PrintResult(response);
            }
        }
        private static void PrintResult(Response response)
        {
            if (response != null)
            {
                if (!response.IsSuccess)
                {
                    Console.WriteLine("Delete error");
                }
                else if (response.Diary != null)
                {
                    foreach (var record in response.Diary)
                    {
                        Console.WriteLine(record);
                    }
                }
            }
        }
    }
}
