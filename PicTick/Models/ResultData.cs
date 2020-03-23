using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicTick.Models
{
    public class ResultData
    {
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class GeneralDataDigitalCard
    {
        public string Message { get; set; }
        public Boolean IsSuccess { get; set; }
        public string Data { get; set; }

    }
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public long? StudioId { get; set; }
    }

    public class DashboardAlbum
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> AlbumPhotoCount { get; set; }
        public Nullable<int> SelectedAlbumPhotoCount { get; set; }
        public List<GetGalleryCustomer_Result> CustomerList { get; set; }
    }

    public class AppDashboardAlbum
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> AlbumPhotoCount { get; set; }
        public Nullable<int> SelectedAlbumPhotoCount { get; set; }
    }

    public class AppAlbumPhotoUpdate
    {
        public long Id { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    }

    public class AlbumPhotoAll
    {
        public int SelectedCount { get; set; }
        public List<AlbumPhoto> PhotoList { get; set; }
    }

    public partial class AlbumData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Photo { get; set; }
        public long GalleryId { get; set; }
        public int SelectedCount { get; set; }
        public int AllCount { get; set; }
    }
    public partial class StudioWaterMark
    {
        public long Id { get; set; }
        public string WaterMark { get; set; }
        public string Position { get; set; }
        public string Font { get; set; }
        public string FontStyle { get; set; }
        public string Opacity { get; set; }
        public string FontSize { get; set; }
        public string WaterMarkImage { get; set; }
        public string WaterMarkType { get; set; }
    }

    public class AdvertisementData
    {
        public Advertisement Advertisement { get; set; }
        public List<AdvertisementPhoto> PhotoList { get; set; }
    }
}