using System;
using System.Collections.Generic;
using System.Text;

namespace media_bot.Models
{
    enum Source
    {
        sonarr,
        radarr
    }
    class MediaObject
    {
        readonly string title;
        readonly int year;
        readonly string poster;
        readonly string overview;
        readonly string id;
        readonly Source source;

        MediaObject(string title, int year, string poster, string overview, string id, Source source)
        {
            this.title = title;
            this.year = year;
            this.poster = poster;
            this.overview = overview;
            this.id = id;
            this.source = source;
        }

        public string GetMediaDisplayTitle()
        {
            var year = (this.year == 0) ? "unk" : this.year.ToString();
            return this.title + " (" + year + ") " + this.overview.Substring(0,100);
        }
    }
}
