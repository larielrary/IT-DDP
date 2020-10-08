using System.Collections.Generic;

namespace Library
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public List<DiaryNote> Diary { get; set; }
    }
}
