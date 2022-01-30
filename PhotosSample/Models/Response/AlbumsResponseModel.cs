using PhotosSample.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosSample.Models.Response
{
    public class AlbumsResponseModel
    {
        public List<Album> albums { get; set; }
        public string nextPageToken { get; set; }

    }
}
