using PhotosSample.Models.Internal;
using System.Collections.Generic;

namespace PhotosSample.Models.Response
{
    public class PhotosResponseModel
    {
        public IList<MediaItem> mediaItems { get; set; }
        public string nextPageToken { get; set; }

    }
}
