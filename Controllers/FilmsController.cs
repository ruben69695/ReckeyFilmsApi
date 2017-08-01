using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReckeyFilmsApi.Models;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using ReckeyFilmsApi.Classes;

namespace ReckeyFilmsApi.Controllers
{
    [Route("api/films")]
    public class FilmsController : Controller
    {
        private string webURL = "https://api.themoviedb.org/3/search/movie?api_key=094cfb8ef66c5c5522e8dff1f82ed80d&language=es&query=";

        public FilmsController()
        {

        }

        [HttpGet("search/{title}")]
        public IActionResult GetFilms(string title)
        {
            String responseFromServer = "";
            Film pelicula = new Film();
            GetWebRequest webRequest = new GetWebRequest(webURL + title);
            
            if(webRequest.SendRequest() == false)
            {
                Console.WriteLine("Error web request");
            }
            else
            {
                responseFromServer = webRequest.TextResponseFromServer;

                // Leer json obtenido de la base de datos de las peliculas
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(responseFromServer));
                while(jsonTextReader.Read())
                {
                    if (jsonTextReader.Value != null)
                    {
                        switch (jsonTextReader.Value)
                        {
                            case "id":
                                pelicula.Id = (int)jsonTextReader.ReadAsInt32();
                                break;
                            case "vote_average":
                                pelicula.VoteAverage = (double)jsonTextReader.ReadAsDouble();
                                break;
                            case "title":
                                pelicula.Title = jsonTextReader.ReadAsString();
                                break;
                            case "popularity":
                                pelicula.Popularity = (double)jsonTextReader.ReadAsDouble();
                                break;
                            case "poster_path":
                                pelicula.PosterPath = jsonTextReader.ReadAsString();
                                break;
                            case "original_language":
                                pelicula.OriginalLanguage = jsonTextReader.ReadAsString();
                                break;
                            case "original_title":
                                pelicula.CompleteTitle = jsonTextReader.ReadAsString();
                                break;
                            case "genre_ids":
                                List<Genres> generos = new List<Genres>();
                                Genres genero = new Genres();
                                genero.numgenre = 35;
                                genero.master = true;
                                genero.name = "Action";
                                generos.Add(genero);
                                pelicula.Genres = generos;
                                break;
                            case "adult":
                                pelicula.Adult =(bool)jsonTextReader.ReadAsBoolean();
                                break;
                            case "overview":
                                pelicula.Description = jsonTextReader.ReadAsString();
                                break;
                            case "release_date":
                                pelicula.ReleaseDate = (DateTime)jsonTextReader.ReadAsDateTime();
                                break;
                        }
                        Console.WriteLine("Token: {0}, Value: {1}", jsonTextReader.TokenType, jsonTextReader.Value);
                    }
                }
            }

            return new ObjectResult(pelicula);
        }
    }
}