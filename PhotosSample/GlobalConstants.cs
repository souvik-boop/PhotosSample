using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosSample
{
    public class GlobalConstants
    {
        public enum FeaturesEnum
        {
            [Description("Clear the screen")]
            Clear,
            [Description("Sync from Cloud")]
            Sync,
            [Description("List all the albums in your account")]
            ListAllAlbums,
            [Description("Download items from Album")]
            DownloadAlbum,
            [Description("View items in Album")]
            ListItemsInAlbum,
            [Description("Exit the program")]
            Stop
        }
        public enum ListMediaItemFeaturesEnum
        {
            [Description("Basic details")]
            Basic,
            [Description("Arrange by MM/YYYY")]
            Advanced1,
            [Description("Arrange by DD/MM/YYYY")]
            Advanced2,
        }
        public enum DownloadMediaItemFeaturesEnum
        {
            [Description("Download based on id")]
            Option1,
            [Description("Download from specific MM/YYYY")]
            Option2,
            [Description("Download from specific DD/MM/YYYY")]
            Option3,
            [Description("Download all")]
            Option4,
        }

        public const string Oath2Token = "";
        public const string BOUNDARY = "==================================================================";
        public const string baseDirectory = @"E:\GooglePhotos";
        public const string INTRODUCTORY_QUESTION = "What operation would you like to do?";
    }
}
