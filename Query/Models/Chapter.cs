namespace Query.Models
{
    public class Chapter
    {
        public int bookId { get; set; } // int
        public int chapter { get; set; } // int
        public string title { get; set; } // nvarchar(255)
        public string summary { get; set; } // nvarchar(255)
    }
}
