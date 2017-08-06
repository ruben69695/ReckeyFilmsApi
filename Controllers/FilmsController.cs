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
using System.Linq;

namespace ReckeyFilmsApi.Controllers
{
    [Route("api/films")]
    public class FilmsController : Controller
    {
        private string apiKey = "api_key=094cfb8ef66c5c5522e8dff1f82ed80d";
        private string languageUrl = "&language=";
        private string queryUrl = "&query=";
        private GenresController genresController;

        public FilmsController(GenresContext context)
        {
            genresController = new GenresController(context);
        }

        [HttpGet("top_rated")]
        public IEnumerable<Film> GetFilmsTopRated(string language = "en")
        {
            String searchTopRatedMoviesUrl = "https://api.themoviedb.org/3/movie/top_rated?";
            String responseFromServer = "";
            Film pelicula = new Film();
            List<Film> films = new List<Film>();

            GetWebRequest webRequest = new GetWebRequest(searchTopRatedMoviesUrl + apiKey + languageUrl + language);

            if(webRequest.SendRequest() == false)
            {
                Console.WriteLine("Error web request");
            }
            else
            {
                responseFromServer = webRequest.TextResponseFromServer;

                // Leer json obtenido de la base de datos de las peliculas
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(responseFromServer));
                int numResults = 0;

                // Localizamos si hay resultados
                numResults = GetTotalResults(ref jsonTextReader);

                if(numResults > 0)
                {
                    films = GetFilmsByJSONResults(ref jsonTextReader);
                }

            }

            return(films);
        }

        [HttpGet("search/{title}")]
        public IEnumerable<Film> GetFilms(string title, string language = "en")
        {
            String serchMovieUrl = "https://api.themoviedb.org/3/search/movie?";
            String responseFromServer = "";
            Film pelicula = new Film();
            List<Film> films = new List<Film>();

            GetWebRequest webRequest = new GetWebRequest(serchMovieUrl + apiKey + languageUrl + language + queryUrl + title);

            if (webRequest.SendRequest() == false)
            {
                Console.WriteLine("Error web request");
            }
            else
            {
                responseFromServer = webRequest.TextResponseFromServer;

                // Leer json obtenido de la base de datos de las peliculas
                JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(responseFromServer));
                int numResults = 0;

                // Localizamos si hay resultados
                numResults = GetTotalResults(ref jsonTextReader);
                
                // Si hay resultados seguimos leyendo el JSON
                if (numResults > 0)
                {
                    films = GetFilmsByJSONResults(ref jsonTextReader);
                }

            }

            return films;
        }

        private List<Film> GetFilmsByJSONResults(ref JsonTextReader jsonTextReader)
        {
            List<Film> films = new List<Film>();

            while (jsonTextReader.Read())
            {
                // Localizamos el principio de la Array results
                if (jsonTextReader.TokenType == JsonToken.StartArray)
                {
                    // Leemos hasta que se acabe la Array de results
                    while (jsonTextReader.TokenType != JsonToken.EndArray)
                    {
                        jsonTextReader.Read();

                        // Localizamos el principio de cada objeto que hay dentro de la Array results
                        if (jsonTextReader.TokenType == JsonToken.StartObject)
                        {
                            // Leemos objeto por objeto de la Array de results
                            while (jsonTextReader.TokenType != JsonToken.EndObject)
                            {
                                // Llamamos a la funci�n que nos retornara un objeto pelicula por cada objeto JSON
                                films.Add(GetFilm(ref jsonTextReader));
                            }
                        }
                    }
                }
            }

            return films;
        }

        private int GetTotalResults(ref JsonTextReader jsonTextReader)
        {
            bool finded = false;
            int numResults = 0;

            while (jsonTextReader.Read() && finded == false)
            {
                if (jsonTextReader.Value != null)
                {           
                    while (jsonTextReader.Value.ToString() != "total_results" && finded == false)
                    {
                        jsonTextReader.Read();

                        if(jsonTextReader.Value != null)
                        {
                            if (jsonTextReader.Value.ToString() == "total_results")
                            {
                                numResults = (int)jsonTextReader.ReadAsInt32();
                                finded = true;
                            }
                        }
                    }
                }
            }

            return numResults;
        }

        private Film GetFilm(ref JsonTextReader jsonTextReader)
        {
            Film film = new Film();

            while (jsonTextReader.TokenType != JsonToken.EndObject)
            {
                jsonTextReader.Read();  // Leemos del JSON

                if (jsonTextReader.Value != null)
                {
                    switch (jsonTextReader.Value)
                    {
                        case "id":
                            film.Id = (int)jsonTextReader.ReadAsInt32();
                            break;
                        case "vote_average":
                            film.VoteAverage = (double)jsonTextReader.ReadAsDouble();
                            break;
                        case "title":
                            film.Title = jsonTextReader.ReadAsString();
                            break;
                        case "popularity":
                            film.Popularity = (double)jsonTextReader.ReadAsDouble();
                            break;
                        case "poster_path":
                            film.PosterPath = jsonTextReader.ReadAsString();
                            break;
                        case "original_language":
                            film.OriginalLanguage = jsonTextReader.ReadAsString();
                            break;
                        case "original_title":
                            film.CompleteTitle = jsonTextReader.ReadAsString();
                            break;
                        case "genre_ids":
                            List<Genres> generos = new List<Genres>();
                            generos = GetGenreByJSON(ref jsonTextReader);
                            film.Genres = generos;
                            break;
                        case "adult":
                            film.Adult = (bool)jsonTextReader.ReadAsBoolean();
                            break;
                        case "overview":
                            film.Description = jsonTextReader.ReadAsString();
                            break;
                        case "release_date":
                            jsonTextReader.ReadAsDateTime();
                            if(jsonTextReader.Value != null)
                            {
                                film.ReleaseDate = (DateTime)jsonTextReader.Value;
                            }                                
                            break;
                    }
                    //Console.WriteLine("Token: {0}, Value: {1}", jsonTextReader.TokenType, jsonTextReader.Value);
                }
            }

            return film;
        }

        private List<Genres> GetGenreByJSON(ref JsonTextReader jsonTextReader)
        {
            List<Genres> generos = new List<Genres>();
            Int32 numeroGenero = 0;

            while(jsonTextReader.TokenType != JsonToken.EndArray)
            {
                jsonTextReader.Read();

                if(jsonTextReader.Value != null)
                {
                    numeroGenero = Convert.ToInt32(jsonTextReader.Value.ToString());
                    generos.Add(genresController.GetGenreByTmdbId(numeroGenero));          
                }
            }
            return generos;
        }
    }
}