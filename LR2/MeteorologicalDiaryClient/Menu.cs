using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace MeteorologicalDiaryClient
{
    public class Menu
    {
        private readonly Client _client;

        public Menu(Client client)
        {
            _client = client;
        }

        public void InputProcessing()
        {
            while (true)
            {
                Console.WriteLine("1.Get diary");
                Console.WriteLine("2.Get note by id");
                Console.WriteLine("3.Add note");
                Console.WriteLine("4.Update note");
                Console.WriteLine("5.Delete note");
                Console.WriteLine("6.Exit");

                Console.WriteLine("Please, enter your choise");
                try
                {
                    var choise = int.Parse(Console.ReadLine());
                    switch (choise)
                    {
                        case 1:
                            {
                                GetAll();
                            }
                            break;
                        case 2:
                            {
                                GetById();
                            }
                            break;
                        case 3:
                            {
                                Create();
                            }
                            break;
                        case 4:
                            {
                                Update();
                            }
                            break;
                        case 5:
                            {
                                Delete();
                            }
                            break;
                        case 6:
                            {
                                Environment.Exit(0);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("Something wrong with connection");
                }
            }
        }
        private void GetAll()
        {
            var packet = _client.ProcessRequest("/api/diarynote", HttpMethod.Get, null);
            var diary = JsonSerializer.Deserialize<IEnumerable<DiaryNote>>(packet, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        
            foreach(var note in diary)
            {
                Console.WriteLine(note);
            }
        }

        private void GetById()
        {
            var id = int.Parse(Console.ReadLine());
            var packet = _client.ProcessRequest($"/api/diarynote/{id}", HttpMethod.Get, null);
            var note = JsonSerializer.Deserialize<IEnumerable<DiaryNote>>(packet, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            Console.WriteLine(note);
        }

        private void Create()
        {
            var note = new DiaryNote();

            Console.WriteLine("Input date");
            note.Date = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Input pressure");
            note.Pressure = int.Parse(Console.ReadLine());

            Console.WriteLine("Input cloudness");
            note.Cloudness = (CloudnessEnum)Enum.Parse(typeof(CloudnessEnum), Console.ReadLine());

            Console.WriteLine("Input wind direction");
            note.WindDirection = Console.ReadLine();

            _client.ProcessRequest("/api/diarynote/", HttpMethod.Post, JsonSerializer.Serialize(note));
        }

        private void Update()
        {
            var note = new DiaryNote();

            Console.WriteLine("Input ID");
            note.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Input date");
            note.Date = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Input pressure");
            note.Pressure = int.Parse(Console.ReadLine());

            Console.WriteLine("Input cloudness");
            note.Cloudness = (CloudnessEnum)Enum.Parse(typeof(CloudnessEnum), Console.ReadLine());

            Console.WriteLine("Input wind direction");
            note.WindDirection = Console.ReadLine();

            _client.ProcessRequest("/api/diarynote/", HttpMethod.Put, JsonSerializer.Serialize(note));
        }

        public void Delete()
        {
            Console.WriteLine("Input ID");
            var id = int.Parse(Console.ReadLine());

            _client.ProcessRequest($"/api/diarynote/{id}", HttpMethod.Delete, null);
        }
    }
}
