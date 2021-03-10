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
        readonly string _title;
        readonly int _year;
        readonly string _poster;
        readonly string _overview;
        readonly string _id;
        readonly Source _source;

        public MediaObject(string title, int year, string poster, string overview, string id, Source source)
        {
            _year = year;
            _poster = poster;
            _title = title;
            _overview = overview;
            _id = id;
            _source = source;
        }

        public string GetMediaDisplayTitle()
        {
            var year = (_year == 0) ? "unk" : _year.ToString();
            var mediaTitle = _title + " (" + year + ") " + _overview.Substring(0, Math.Min(100,_overview.Length));
            return mediaTitle;
        }
    }
}
