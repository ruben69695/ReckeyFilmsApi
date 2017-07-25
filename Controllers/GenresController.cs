using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReckeyFilmsApi.Models;

namespace ReckeyFilmsApi.Controllers
{
    [Route("api/genres")]
    public class GenresController : Controller
    {
        private readonly GenresContext _context;

        public GenresController(GenresContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Genres> GetAll()
        {
            return _context.Genres.ToList();
        }

        [HttpGet("{numero}", Name = "GetGenres")]
        public IActionResult GetByNum(int numero)
        {
            var item = _context.Genres.FirstOrDefault(t => t.numgenre == numero);
            if(item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);

        }

        [HttpGet("masters")]
        public IEnumerable<Genres> GetMasters()
        {
            var masterGenres = from m in _context.Genres
                    select m;

            masterGenres = masterGenres.Where(t => t.master == true);            

            return masterGenres.ToList();
        }

        [HttpGet("nomasters")]
        public IEnumerable<Genres> GetNoMasters()
        {
            var noMasters = from m in _context.Genres
                select m;
            
            noMasters = noMasters.Where(t => t.master == false);

            return noMasters.ToList();
        }

    }
}