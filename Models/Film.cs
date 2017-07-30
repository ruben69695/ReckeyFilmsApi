using System;
using System.Collections.Generic;

namespace ReckeyFilmsApi.Models
{
    public class Film
    {
        public int Id = 0;
        public double VoteAverage = 0.00;
        public string Title = "";
        public double Popularity = 0.00;
        public string PosterPath = "";
        public string OriginalLanguage = "";
        public string CompleteTitle = "";
        public bool Adult = false;
        public string Description = "";
        public DateTime ReleaseDate = DateTime.Now;
        public IEnumerable<Genres> Genres;

        public Film(int id, double votesAverage, string title, double popularity, string poster, string language, string comTitle, bool adult, string descr, DateTime releaseDate, IEnumerable<Genres> genresList)
        {
            this.Id = id;
            this.VoteAverage = votesAverage;
            this.Title = title;
            this.Popularity = popularity;
            this.PosterPath = poster;
            this.OriginalLanguage = language;
            this.CompleteTitle = comTitle;
            this.Adult = adult;
            this.Description = descr;
            this.ReleaseDate = releaseDate;
            this.Genres = genresList;
        }

        public Film()
        {}

    }
}