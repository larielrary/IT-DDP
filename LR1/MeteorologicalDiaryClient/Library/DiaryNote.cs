using System;

namespace Library
{
    public class DiaryNote
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Pressure { get; set; }

        public CloudnessEnum Cloudness { get; set; }

        public string WindDirection { get; set; }

        public override string ToString()
        {
            return $"{Id} {Date} {Pressure} {Cloudness} {WindDirection}";
        }
    }
}
