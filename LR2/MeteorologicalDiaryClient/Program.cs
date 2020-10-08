namespace MeteorologicalDiaryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("https://localhost:44327");
            var menu = new Menu(client);
            menu.InputProcessing();
        }
    }
}
