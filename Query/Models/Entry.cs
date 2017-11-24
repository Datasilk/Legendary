using System;

namespace Legendary.Query.Models
{
    class Entry
    {
        public int entryId { get; set; } // int
        public int userId { get; set; } // int
        public int bookId { get; set; } // int
        public string chapter { get; set; } // nchar(10)
        public DateTime datecreated { get; set; } // datetime
        public DateTime datemodified { get; set; } // datetime
        public string title { get; set; } // nvarchar(255)
        public string summary { get; set; } // nvarchar(255)
        public string book_title { get; set; } // nvarchar(255)
        public bool book_favorite { get; set; } // bit
        public string author { get; set; } // nvarchar(64)
        public string auther_email { get; set; } // nvarchar(64)
        public bool author_photo { get; set; } // bit
    }
}
