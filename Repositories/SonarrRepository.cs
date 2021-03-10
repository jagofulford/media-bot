using System;
using System.Collections.Generic;
using System.Text;
using media_bot.Models;

namespace media_bot.Repositories
{
    class SonarrRepository : IMediaRepository
    {
        public bool AddMedia(string uniqueId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MediaObject> SearchMedia(string searchTerm)
        {
            var media = new MediaObject("movie title", 2020, "poster", "", "", Source.sonarr);
            var mediaLibrary = new List<MediaObject>() { media };
            return mediaLibrary;
        }
    }
}
