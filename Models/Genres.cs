using System;
using System.ComponentModel.DataAnnotations;

namespace ReckeyFilmsApi.Models
{
    public class Genres
    {
        [Key]
        public int numgenre { get; set; }
        public string name { get; set; }
        public bool master { get; set; }
    }
}