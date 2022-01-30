using EnumsNET;
using Newtonsoft.Json;
using PhotosSample.Models;
using PhotosSample.Models.Input;
using PhotosSample.Models.Internal;
using PhotosSample.Models.Response;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static PhotosSample.GlobalConstants;

namespace PhotosSample
{
    class Program
    {
        static GooglePhotosApiAdapter adapter = new GooglePhotosApiAdapter(Oath2Token);
        static LocalFileManager manager = new LocalFileManager(baseDirectory);
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    // View the choices
                    Console.WriteLine(BOUNDARY);
                    FeaturesEnum choice = ViewFeaturesChoice();
                    Console.WriteLine(BOUNDARY);

                    switch (choice)
                    {
                        case FeaturesEnum.Clear:
                            Console.Clear();
                            break;
                        case FeaturesEnum.ListAllAlbums:
                            ProcessListAllAlbumsFeature();
                            break;
                        case FeaturesEnum.DownloadAlbum:
                            ProcessDownloadAlbumFeature();
                            break;
                        case FeaturesEnum.ListItemsInAlbum:
                            ProcessListItemsInAlbumFeature();
                            break;
                        case FeaturesEnum.Stop:
                            Environment.Exit(0);
                            break;
                        case FeaturesEnum.Sync:
                            ProcessSyncFeature();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        Console.WriteLine("Exception Message : " + ex.Message);
                        Console.WriteLine("Exception StackTrace : " + ex.StackTrace);
                        ex = ex.InnerException;
                    }
                }
            }
        }

        #region Process Cases
        private static void ProcessDownloadAlbumFeature()
        {
            bool flag = true;
            while (flag)
            {
                // View the choices
                Console.WriteLine(BOUNDARY);
                DownloadMediaItemFeaturesEnum choice2 = ViewDownloadMediaItemFeaturesChoice();
                Console.WriteLine(BOUNDARY);
                switch (choice2)
                {
                    case DownloadMediaItemFeaturesEnum.Option1:
                        {
                            Console.Write("Enter media id : ");
                            var mediaId = Console.ReadLine();

                            Console.Write("Enter download directory : ");
                            string baseDirectory = Console.ReadLine();

                            var mediaItem = adapter.FetchMediaItemFromCloud(mediaId);
                            using (WebClient client = new WebClient())
                            {
                                string directoryPath = $@"{baseDirectory}";
                                if (!Directory.Exists(directoryPath))
                                    Directory.CreateDirectory(directoryPath);
                                if (!File.Exists($@"{directoryPath}\\{mediaItem.filename}"))
                                    client.DownloadFile(new Uri(mediaItem.baseUrl), $@"{directoryPath}\\{mediaItem.filename}");
                                else
                                    Console.WriteLine($"File {mediaItem.filename} already downloaded!");
                            }
                        }
                        break;
                    case DownloadMediaItemFeaturesEnum.Option2:
                        {
                            Console.Write("Enter album id : ");
                            string albumid = Console.ReadLine();
                            List<MediaItem> list = manager.FetchAllItemsFromAlbumLocal(albumid);
                            Console.Write("Enter MM/YYYY : ");
                            string date = Console.ReadLine();

                            Console.Write("Enter download directory : ");
                            string baseDirectory = Console.ReadLine();

                            string directoryPath = $@"{baseDirectory}";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);

                            List<MediaItem> enumerable = list.Where(item => item.mediaMetadata.creationTime.Month == int.Parse(date.Split('/')[0]) &&
                                                                        item.mediaMetadata.creationTime.Year == int.Parse(date.Split('/')[1])).ToList();
                            foreach (var item in enumerable)
                            {
                                Console.WriteLine($"Downloading {enumerable.IndexOf(item)}/{enumerable.Count} ...");
                                using (WebClient client = new WebClient())
                                {
                                    if (!File.Exists($@"{directoryPath}\\{item.filename}"))
                                        client.DownloadFile(new Uri(item.baseUrl), $@"{directoryPath}\\{item.filename}");
                                    else
                                        Console.WriteLine($"File {item.filename} already downloaded!");
                                }
                                HelperMethods.ClearCurrentConsoleLine();
                            }
                        }
                        break;
                    case DownloadMediaItemFeaturesEnum.Option3:
                        {
                            Console.Write("Enter album id : ");
                            string albumid = Console.ReadLine();
                            List<MediaItem> list = manager.FetchAllItemsFromAlbumLocal(albumid);
                            Console.Write("Enter DD/MM/YYYY : ");
                            string date = Console.ReadLine();

                            Console.Write("Enter download directory : ");
                            string baseDirectory = Console.ReadLine();

                            string directoryPath = $@"{baseDirectory}";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);

                            List<MediaItem> enumerable = list.Where(item => item.mediaMetadata.creationTime.Day == int.Parse(date.Split('/')[0]) &&
                                                                       item.mediaMetadata.creationTime.Month == int.Parse(date.Split('/')[1]) &&
                                                                       item.mediaMetadata.creationTime.Year == int.Parse(date.Split('/')[2])).ToList();
                            foreach (var item in enumerable)
                            {
                                Console.WriteLine($"Downloading {enumerable.IndexOf(item)}/{enumerable.Count} ...");
                                using (WebClient client = new WebClient())
                                {
                                    if (!File.Exists($@"{directoryPath}\\{item.filename}"))
                                        client.DownloadFile(new Uri(item.baseUrl), $@"{directoryPath}\\{item.filename}");
                                    else
                                        Console.WriteLine($"File {item.filename} already downloaded!");
                                }
                                HelperMethods.ClearCurrentConsoleLine();
                            }
                        }
                        break;
                    case DownloadMediaItemFeaturesEnum.Option4:
                        {
                            Console.Write("Enter album id : ");
                            string albumid = Console.ReadLine();
                            List<MediaItem> list = manager.FetchAllItemsFromAlbumLocal(albumid);
                            string albumName = manager.FetchAllAlbumsFromLocal().FirstOrDefault(x => x.id == albumid).title;

                            Console.Write("Enter download directory : ");
                            string baseDirectory = Console.ReadLine();

                            foreach (var item in list)
                            {
                                string tfn = item.filename.Split('.')[0];
                                string extension = item.filename.Split('.')[1];
                                Console.WriteLine($"Downloading ({list.IndexOf(item) + 1}/{list.Count}) | File : {tfn.Substring(0, Math.Min(tfn.Length, 10))}...");

                                string directoryPath = $@"{baseDirectory}/{albumName}/{item.mediaMetadata.creationTime.Year}/{item.mediaMetadata.creationTime.Month}/{item.mediaMetadata.creationTime.Day}";
                                if (!Directory.Exists(directoryPath))
                                    Directory.CreateDirectory(directoryPath);

                                var fileCount = Directory.GetFiles(directoryPath).Length;

                                using (WebClient client = new WebClient())
                                {
                                    if (!File.Exists($@"{directoryPath}\\{item.filename}"))
                                    {
                                        client.DownloadFile(new Uri(item.baseUrl), $@"{directoryPath}\\{item.filename}");
                                        File.Move($@"{directoryPath}\\{item.filename}", $@"{directoryPath}\\File-{fileCount + 1}.{extension}");
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        flag = false;
                        break;
                }
            }
        }
        private static void ProcessListItemsInAlbumFeature()
        {
            Console.Write("Enter album id : ");
            string albumid = Console.ReadLine();
            List<MediaItem> list = manager.FetchAllItemsFromAlbumLocal(albumid);

            bool flag = true;
            while (flag)
            {
                // View the choices
                Console.WriteLine(BOUNDARY);
                ListMediaItemFeaturesEnum choice2 = ViewListMediaItemFeaturesChoice();
                Console.WriteLine(BOUNDARY);
                switch (choice2)
                {
                    case ListMediaItemFeaturesEnum.Basic:
                        foreach (var item in list)
                        {
                            Console.WriteLine(item.id + " : " + item.filename);
                        }
                        break;
                    case ListMediaItemFeaturesEnum.Advanced1:
                        {
                            Dictionary<string, List<MediaItem>> dateArrangement = new Dictionary<string, List<MediaItem>>();
                            foreach (var item in list)
                            {
                                var key = string.Format("{0}/{1}", item.mediaMetadata.creationTime.Month, item.mediaMetadata.creationTime.Year);
                                if (!dateArrangement.ContainsKey(key))
                                    dateArrangement.Add(key, new List<MediaItem>() { item });
                                else
                                    dateArrangement[key].Add(item);
                            }
                            foreach (var item in dateArrangement)
                            {
                                Console.WriteLine(item.Key + " : " + item.Value.Count);
                            }
                        }
                        break;
                    case ListMediaItemFeaturesEnum.Advanced2:
                        {
                            Dictionary<string, List<MediaItem>> dateArrangement = new Dictionary<string, List<MediaItem>>();
                            foreach (var item in list)
                            {
                                var key = item.mediaMetadata.creationTime.Date.ToString();
                                if (!dateArrangement.ContainsKey(key))
                                    dateArrangement.Add(key, new List<MediaItem>() { item });
                                else
                                    dateArrangement[key].Add(item);
                            }
                            foreach (var item in dateArrangement)
                            {
                                Console.WriteLine(item.Key + " : " + item.Value.Count);
                            }
                        }
                        break;
                    default:
                        flag = false;
                        break;
                }
            }
        }
        private static void ProcessListAllAlbumsFeature()
        {
            var albums = manager.FetchAllAlbumsFromLocal();
            foreach (var item in albums)
            {
                Console.WriteLine(item.id + " : " + item.title);
            }
        }
        private static void ProcessSyncFeature()
        {
            if (!Directory.Exists(@"Data"))
                Directory.CreateDirectory(@"Data");
            var cloudAlbums = adapter.FetchAllAlbumsFromCloud();
            Console.WriteLine("Syncing Albums...");
            manager.SaveAlbums(cloudAlbums);
            Console.WriteLine("Synced Albums!");
            foreach (var item in cloudAlbums)
            {
                Console.WriteLine($"Syncing Album {item.title} content...");
                var albumData = adapter.FetchAllItemsInAlbumFromCloud(item);
                manager.SaveAlbumContent(item, albumData);
                Console.WriteLine($"Synced Album {item.title} content!");
            }
        }
        #endregion

        #region Helpers
        private static FeaturesEnum ViewFeaturesChoice()
        {
            Console.WriteLine(INTRODUCTORY_QUESTION);
            var options = Enum.GetValues(typeof(FeaturesEnum));
            for (int i = 0; i < options.Length; i++)
            {
                var optionNum = (int)options.GetValue(i);
                var optionDesc = ((FeaturesEnum)optionNum).AsString(EnumFormat.Description);
                Console.WriteLine($"{optionNum}. {optionDesc}");
            }
            var choice = (FeaturesEnum)Enum.Parse(typeof(FeaturesEnum), Console.ReadLine());
            return choice;
        }
        private static ListMediaItemFeaturesEnum ViewListMediaItemFeaturesChoice()
        {
            Console.WriteLine(INTRODUCTORY_QUESTION);
            var options = Enum.GetValues(typeof(ListMediaItemFeaturesEnum));
            for (int i = 0; i < options.Length; i++)
            {
                var optionNum = (int)options.GetValue(i);
                var optionDesc = ((ListMediaItemFeaturesEnum)optionNum).AsString(EnumFormat.Description);
                Console.WriteLine($"{optionNum}. {optionDesc}");
            }
            var choice = (ListMediaItemFeaturesEnum)Enum.Parse(typeof(ListMediaItemFeaturesEnum), Console.ReadLine());
            return choice;
        }
        private static DownloadMediaItemFeaturesEnum ViewDownloadMediaItemFeaturesChoice()
        {
            Console.WriteLine(INTRODUCTORY_QUESTION);
            var options = Enum.GetValues(typeof(DownloadMediaItemFeaturesEnum));
            for (int i = 0; i < options.Length; i++)
            {
                var optionNum = (int)options.GetValue(i);
                var optionDesc = ((DownloadMediaItemFeaturesEnum)optionNum).AsString(EnumFormat.Description);
                Console.WriteLine($"{optionNum}. {optionDesc}");
            }
            var choice = (DownloadMediaItemFeaturesEnum)Enum.Parse(typeof(DownloadMediaItemFeaturesEnum), Console.ReadLine());
            return choice;
        }
        #endregion
    }
}