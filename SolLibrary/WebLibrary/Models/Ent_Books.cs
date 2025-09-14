using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLibrary.Models
{
    public class Ent_Books
    {
        public int IdBook { get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public string Genre { get; set; }
        public int Copies { get; set; }

    }
}