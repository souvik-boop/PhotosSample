namespace PhotosSample.Models.Input
{
    class GetPhotoFromAlbumInputModel
    {
        public string pageSize { get; set; }
        public string albumId { get; set; }
        public string pageToken { get; set; }
    }
}
