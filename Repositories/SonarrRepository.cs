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

        public IDictionary<int,MediaObject> SearchMedia(string searchTerm)
        {
            var media = new MediaObject("movie title", 2020, "poster", "", "", Source.sonarr);
            var mediaLibrary = new Dictionary<int, MediaObject>
            {
                { 1, media }
            };
            return mediaLibrary;
        }
    }
}
