using Newtonsoft.Json;
using PhotosSample.Models.Input;
using PhotosSample.Models.Internal;
using PhotosSample.Models.Response;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosSample
{
    public class GooglePhotosApiAdapter
    {
        private string Oath2Token;
        public GooglePhotosApiAdapter(string Oath2Token)
        {
            this.Oath2Token = Oath2Token;
        }

        public List<Album> FetchAllAlbumsFromCloud()
        {
            var client = new RestClient("https://photoslibrary.googleapis.com/v1/albums");
            client.Timeout = -1;

            List<Album> finalResult = new List<Album>();
            string pageToken = null;

            while (true)
            {
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", string.Format("Bearer {0}", Oath2Token));
                if (pageToken != null) request.AddParameter("pageToken", pageToken);
                IRestResponse response = client.Execute(request);

                if (response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var deserializedResponse = JsonConvert.DeserializeObject<AlbumsResponseModel>(response.Content);
                    if (deserializedResponse.albums != null)
                        finalResult.AddRange(deserializedResponse.albums);
                    pageToken = deserializedResponse.nextPageToken;
                    if (deserializedResponse.nextPageToken == null)
                        break;
                }
                else
                    return null;
            }
            return finalResult;
        }
        public Album FetchAlbumByIdFromCloud(string albumid)
        {
            var client = new RestClient($"https://photoslibrary.googleapis.com/v1/albums/{albumid}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", Oath2Token));
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Album>(response.Content);
            }
            else
                return null;
        }

        public List<MediaItem> FetchAllItemsInAlbumFromCloud(Album album)
        {
            string pageToken = string.Empty;
            var mediaItems = new List<MediaItem>();
            int count = 1;
            while (true)
            {
                int v = count++ * 100;
                Console.WriteLine($"Fetching items {(v > int.Parse(album.mediaItemsCount) ? int.Parse(album.mediaItemsCount) : v)}/{album.mediaItemsCount}");
                IRestResponse response = MediaItemSearchRequestFromCloud(album.id, pageToken);
                if (response.IsSuccessful)
                {
                    var responseBody = JsonConvert.DeserializeObject<PhotosResponseModel>(response.Content);
                    mediaItems.AddRange(responseBody.mediaItems);
                    pageToken = responseBody.nextPageToken;
                    if (string.IsNullOrEmpty(pageToken))
                        break;
                }
                HelperMethods.ClearCurrentConsoleLine();
            }
            return mediaItems;
        }
        public IRestResponse MediaItemSearchRequestFromCloud(string albumid, string pageToken)
        {
            var client = new RestClient("https://photoslibrary.googleapis.com/v1/mediaItems:search");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", string.Format("Bearer {0}", Oath2Token));
            request.AddHeader("Content-Type", "application/json");
            GetPhotoFromAlbumInputModel body = new GetPhotoFromAlbumInputModel
            {
                albumId = albumid,
                pageSize = 100.ToString(),
                pageToken = pageToken
            };
            request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }

        public MediaItem FetchMediaItemFromCloud(string mediaId)
        {
            var client = new RestClient($"https://photoslibrary.googleapis.com/v1/mediaItems/{mediaId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", Oath2Token));
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<MediaItem>(response.Content);
            }
            else
                return null;
        }
    }
}
