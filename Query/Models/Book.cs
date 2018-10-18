namespace Query.Models
{
    public class Book
    {
        public int bookId { get; set; } // int
        public int userId { get; set; } // int
        public string title { get; set; } // nvarchar(255)
        public bool favorite { get; set; } // bit
        public int sort { get; set; } // int
    }
}
