using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Data.Model
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Title field is required")]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}
