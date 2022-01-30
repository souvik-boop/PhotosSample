using Newtonsoft.Json;
using PhotosSample.Models.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosSample
{
    public class LocalFileManager
    {
        private string baseDirectoryPath;
        public LocalFileManager(string baseDirectoryPath)
        {
            this.baseDirectoryPath = baseDirectoryPath;
            Directory.CreateDirectory($@"{baseDirectoryPath}\Settings");
        }

        public void SaveAlbums(List<Album> albums)
        {
            using (StreamWriter sw = new StreamWriter($@"{baseDirectoryPath}\Settings\Albums.json"))
            {
                sw.WriteLine(JsonConvert.SerializeObject(albums, Formatting.Indented));
            }
        }
        public void SaveAlbumContent(Album item, List<MediaItem> albumData)
        {
            using (StreamWriter sw = new StreamWriter($@"{baseDirectoryPath}\Settings\Album_{item.id}.json"))
            {
                sw.WriteLine(JsonConvert.SerializeObject(albumData, Formatting.Indented));
            }
        }
        public List<Album> FetchAllAlbumsFromLocal()
        {
            return JsonConvert.DeserializeObject<List<Album>>(File.ReadAllText($@"{baseDirectoryPath}\Settings\Albums.json"));
        }
        public List<MediaItem> FetchAllItemsFromAlbumLocal(string albumid)
        {
            var filepath = Directory.GetFiles($@"{baseDirectoryPath}\Settings").FirstOrDefault(s => s.Contains(albumid));
            return JsonConvert.DeserializeObject<List<MediaItem>>(File.ReadAllText(filepath));
        }
    }
}
