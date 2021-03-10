using System;
using System.Collections.Generic;
using System.Text;

namespace media_bot.Models
{
    interface IMediaRepository
    {
        IEnumerable<MediaObject> SearchMedia(string searchTerm);
        bool AddMedia(string uniqueId);
    }
}
