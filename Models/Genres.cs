using System;
using System.ComponentModel.DataAnnotations;

namespace ReckeyFilmsApi.Models
{
    public class Genres
    {
        public int numgenre { get; set; }
        public int tmdbId { get; set; }
        public string name { get; set; }
        public bool master { get; set; }
    }
}