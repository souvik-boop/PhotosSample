namespace PhotosSample.Models.Internal
{
    public class MediaItem
    {
        public string id { get; set; }
        public string productUrl { get; set; }
        public string baseUrl { get; set; }
        public string mimeType { get; set; }
        public MediaMetadata mediaMetadata { get; set; }
        public string filename { get; set; }

    }
}
