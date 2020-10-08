using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library
{
    public class FileProcessing
    {
        private readonly string _fileName;
        private object _lockObject = new object();

        public FileProcessing(string fileName)
        {
            _fileName = fileName;
        }

        public void Create(DiaryNote note)
        {
            var list = ReadFromFile();
            list.Add(note);

            WriteToFile(list);
        }

        public bool Update(DiaryNote note)
        {
            var list = ReadFromFile();
            var itemToUpdate = list.FirstOrDefault(x => x.Id == note.Id);

            if (itemToUpdate is null)
            {
                return false;
            }

            itemToUpdate.Id = note.Id;
            itemToUpdate.Date = note.Date;
            itemToUpdate.Pressure = note.Pressure;
            itemToUpdate.Cloudness = note.Cloudness;
            itemToUpdate.WindDirection = note.WindDirection;
            WriteToFile(list);

            return true;
        }

        public bool Delete(int id)
        {
            var list = ReadFromFile();
            var itemToDelete = list.FirstOrDefault(x => x.Id == id);

            if (itemToDelete is null)
            {
                return false;
            }

            list.Remove(itemToDelete);
            WriteToFile(list);

            return true;
        }

        public List<DiaryNote> Filter(string direction)
        {
            return ReadFromFile()
            .Where(x => x.WindDirection.Contains(direction)).ToList();
        }

        public List<DiaryNote> ReadFromFile()
        {
            var fileText = File.ReadAllText(_fileName);
            var list = string.IsNullOrWhiteSpace(fileText) ? new List<DiaryNote>()
                : JsonConvert.DeserializeObject<List<DiaryNote>>(fileText);

            return list.OrderBy(x => x.Pressure).ToList();
        }

        private void WriteToFile(List<DiaryNote> list)
        {
            lock (_lockObject)
            {
                File.WriteAllText(_fileName, JsonConvert.SerializeObject(list));
            }
        }
    }
}
