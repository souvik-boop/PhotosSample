using System;

namespace PhotosSample.Models.Internal
{
    public class MediaMetadata
    {
        public DateTime creationTime { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public Photo photo { get; set; }

    }
}
