using Newtonsoft.Json;
using PicTick.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PicTick.Controllers
{
    public class HomeAPIController : ApiController
    {
        PicTickEntities db = new PicTickEntities();
        StudyField sf = new StudyField();

        #region Dashboard

        [HttpGet]
        public ResultData GetDashboardCount(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<GetDashboardCount_Result> studioList = new List<GetDashboardCount_Result>();
                studioList = db.GetDashboardCount(studioId).ToList();
                if (studioList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = studioList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetDashboardCount";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetDashboardAlbumList(long StudioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<DashboardAlbum> dashboardAlbumList = new List<DashboardAlbum>();

                List<Gallery> galleryList = new List<Gallery>();
                galleryList = db.Galleries.Where(g => g.StudioId == StudioId).ToList();

                if (galleryList != null)
                {

                    foreach (Gallery gallery in galleryList)
                    {
                        List<Album> albumList = new List<Album>();
                        albumList = db.Albums.Where(a => a.GalleryId == gallery.Id).ToList();

                        if (albumList != null && albumList.Count > 0)
                            foreach (Album album in albumList)
                            {
                                DashboardAlbum dashboardAlbum = new DashboardAlbum();
                                dashboardAlbum.Id = album.Id;
                                dashboardAlbum.Name = album.Name;

                                List<GetGalleryCustomer_Result> albumCustomerList = new List<GetGalleryCustomer_Result>();
                                albumCustomerList = db.GetGalleryCustomer(gallery.Id).ToList();
                                if (albumCustomerList != null)
                                {
                                    dashboardAlbum.CustomerList = albumCustomerList;
                                }

                                List<GetAlbumSelectedCount_Result> albumSelectionCounyList = new List<GetAlbumSelectedCount_Result>();
                                albumSelectionCounyList = db.GetAlbumSelectedCount(album.Id).ToList();
                                if (albumSelectionCounyList != null)
                                {
                                    dashboardAlbum.SelectedAlbumPhotoCount = albumSelectionCounyList[0].SelectedAlbumPhotoCount;
                                    dashboardAlbum.AlbumPhotoCount = albumSelectionCounyList[0].AlbumPhotoCount;
                                }

                                dashboardAlbumList.Add(dashboardAlbum);
                            }
                    }

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = dashboardAlbumList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetDashboardAlbumList";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetDashboardGalleryList(long StudioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<DashboardAlbum> dashboardAlbumList = new List<DashboardAlbum>();

                List<Gallery> galleryList = new List<Gallery>();
                galleryList = db.Galleries.Where(g => g.StudioId == StudioId).ToList();

                if (galleryList != null)
                {

                    foreach (Gallery gallery in galleryList)
                    {
                        DashboardAlbum dashboardAlbum = new DashboardAlbum();
                        dashboardAlbum.Id = gallery.Id;
                        dashboardAlbum.Name = gallery.Title;

                        List<GetGalleryCustomer_Result> albumCustomerList = new List<GetGalleryCustomer_Result>();
                        albumCustomerList = db.GetGalleryCustomer(gallery.Id).ToList();
                        if (albumCustomerList != null)
                        {
                            dashboardAlbum.CustomerList = albumCustomerList;
                        }

                        List<GetGallerySelectedCount_Result> gallerySelectionCounyList = new List<GetGallerySelectedCount_Result>();
                        gallerySelectionCounyList = db.GetGallerySelectedCount(gallery.Id).ToList();
                        if (gallerySelectionCounyList != null)
                        {
                            dashboardAlbum.SelectedAlbumPhotoCount = gallerySelectionCounyList[0].SelectedAlbumPhotoCount;
                            dashboardAlbum.AlbumPhotoCount = gallerySelectionCounyList[0].GalleryPhotoCount;
                        }

                        dashboardAlbumList.Add(dashboardAlbum);
                    }

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = dashboardAlbumList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetDashboardGalleryList";

                return resultData;
            }
        }

        #endregion

        #region Studio
        [HttpPost]
        public ResultData SaveStudio(Studio data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.Studios.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    Studio oldStudio = db.Studios.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldStudio != null)
                    {
                        oldStudio.Name = data.Name;
                        oldStudio.Mobile = data.Mobile;
                        oldStudio.UserName = data.UserName;
                        oldStudio.Password = data.Password;

                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveStudio";

                return resultData;
            }
        }

        [HttpPost]
        [ActionName("UpdateStudioWaterMark")]
        public ResultData UpdateStudioWaterMark()
        {
            ResultData resultData = new ResultData();
            try
            {
                StudioWaterMark data;
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeWaterMark, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string WaterMark = Convert.ToString(requestObject.Form["WaterMark"]);
                string Position = Convert.ToString(requestObject.Form["Position"]);
                string Font = Convert.ToString(requestObject.Form["Font"]);
                string FontStyle = Convert.ToString(requestObject.Form["FontStyle"]);
                string Opacity = Convert.ToString(requestObject.Form["Opacity"]);
                string FontSize = Convert.ToString(requestObject.Form["FontSize"]);
                string WaterMarkType = Convert.ToString(requestObject.Form["WaterMarkType"]);

                if (Id != null && Id > 0)
                {
                    Studio oldStudio = db.Studios.Where(n => n.Id == Id).FirstOrDefault();
                    if (oldStudio != null)
                    {
                        oldStudio.WaterMark = WaterMark;
                        oldStudio.Position = Position;
                        oldStudio.Font = Font;
                        oldStudio.FontStyle = FontStyle;
                        oldStudio.Opacity = Opacity;
                        oldStudio.FontSize = FontSize;
                        if (paths != null && paths.Length > 0)
                            oldStudio.WaterMarkImage = paths[0];
                        oldStudio.WaterMarkType = WaterMarkType;

                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveStudio";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteStudio(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Studio studio = db.Studios.Where(t => t.Id == id).FirstOrDefault();
                if (studio != null)
                {
                    db.Studios.Remove(studio);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteStudio";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetStudioList()
        {
            ResultData resultData = new ResultData();
            try
            {

                List<Studio> studioList = new List<Studio>();
                studioList = db.Studios.ToList();
                if (studioList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = studioList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetStudioList";

                return resultData;
            }
        }
        #endregion

        #region StudioAbout
        [HttpPost]
        public ResultData SaveStudioAbout(StudioAbout data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.StudioAbouts.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    StudioAbout oldStudioAbout = db.StudioAbouts.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldStudioAbout != null)
                    {
                        oldStudioAbout.Title = data.Title;
                        oldStudioAbout.Description = data.Description;

                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveStudioAbout";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteStudioAbout(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                StudioAbout studioAbout = db.StudioAbouts.Where(t => t.Id == id).FirstOrDefault();
                if (studioAbout != null)
                {
                    db.StudioAbouts.Remove(studioAbout);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteStudioAbout";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetStudioAboutList(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<StudioAbout> studioAboutList = new List<StudioAbout>();
                studioAboutList = db.StudioAbouts.Where(sa => sa.StudioId == studioId).ToList();
                if (studioAboutList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = studioAboutList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetStudioAboutList";

                return resultData;
            }
        }
        #endregion

        #region Gallery
        [ActionName("SaveGallery")]
        public ResultData SaveGallery()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeGalleryCover, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string Title = Convert.ToString(requestObject.Form["Title"]);
                string GalleryPin = Convert.ToString(requestObject.Form["GalleryPin"]);
                string SelectionPin = Convert.ToString(requestObject.Form["SelectionPin"]);
                Boolean AllowDownload = Convert.ToBoolean(requestObject.Form["AllowDownload"]);
                string WatterMark = Convert.ToString(requestObject.Form["WatterMark"]);
                long StudioId = Convert.ToInt64(requestObject.Form["StudioId"]);

                string GalleryCover = "";
                if (paths != null && paths.Length > 0)
                    GalleryCover = paths[0];
                if (Id != null && Id == 0)
                {
                    Gallery gallery = new Gallery();
                    gallery.Title = Title;
                    gallery.GalleryPin = GalleryPin;
                    gallery.SelectionPin = SelectionPin;
                    gallery.AllowDownload = AllowDownload;
                    gallery.WatterMark = WatterMark;
                    gallery.GalleryCover = GalleryCover;
                    gallery.StudioId = StudioId;

                    db.Galleries.Add(gallery);
                    db.SaveChanges();
                }
                else
                {
                    Gallery oldGallery = db.Galleries.Where(g => g.Id == Id).FirstOrDefault();
                    if (oldGallery != null)
                    {
                        oldGallery.Title = Title;
                        oldGallery.GalleryPin = GalleryPin;
                        oldGallery.SelectionPin = SelectionPin;
                        oldGallery.AllowDownload = AllowDownload;
                        oldGallery.WatterMark = WatterMark;
                        oldGallery.StudioId = StudioId;
                        if (GalleryCover != "")
                        {
                            string filePath = "~/" + oldGallery.GalleryCover;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldGallery.GalleryCover = GalleryCover;
                        }

                        db.SaveChanges();
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveAlbumPhoto";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteGallery(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Gallery gallery = db.Galleries.Where(t => t.Id == id).FirstOrDefault();
                if (gallery != null)
                {
                    DeleteGalleryAlbum(id);
                    if (gallery.GalleryCover != null && gallery.GalleryCover != "")
                    {
                        string filePath = "~/" + gallery.GalleryCover;
                        bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                        if (exists)
                            System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));
                    }

                    db.Galleries.Remove(gallery);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteAlbum";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetGalleryList(long StudioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Gallery> galleryList = new List<Gallery>();
                galleryList = db.Galleries.Where(a => a.StudioId == StudioId).ToList();
                if (galleryList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = galleryList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAlbumList";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetGalleryById(long galleryId)
        {
            ResultData resultData = new ResultData();
            try
            {
                Gallery galleryList = new Gallery();
                galleryList = db.Galleries.Where(a => a.Id == galleryId).FirstOrDefault();
                if (galleryList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = galleryList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetGalleryById";

                return resultData;
            }
        }


        #endregion

        #region Album
        [ActionName("SaveAlbum")]
        public ResultData SaveAlbum()
        {
            ResultData resultData = new ResultData();
            try
            {
                //CookieHeaderValue Coki = Request.Headers.GetCookies("session").FirstOrDefault();
                //if (Coki != null)
                //{
                //    CookieState cookie_State = Coki["session"];
                //    sessionId = cookie_State["sid"];
                //    sessionToken = cookie_State["token"];
                //    theme = cookie_State["theme"];
                //}

                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeAlbumCover, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string Name = Convert.ToString(requestObject.Form["Name"]);
                DateTime Date = Convert.ToDateTime(requestObject.Form["Date"]);
                long GalleryId = Convert.ToInt64(requestObject.Form["GalleryId"]);

                string AlbumCover = "";
                if (paths != null && paths.Length > 0)
                    AlbumCover = paths[0];
                if (Id != null && Id == 0)
                {
                    Album album = new Album();
                    album.Name = Name;
                    album.Date = Date;
                    album.Photo = AlbumCover;
                    album.GalleryId = GalleryId;

                    db.Albums.Add(album);
                    db.SaveChanges();

                    GetCustomerFcmTokenByGalleryId(GalleryId);
                }
                else
                {
                    Album oldAlbum = db.Albums.Where(g => g.Id == Id).FirstOrDefault();
                    if (oldAlbum != null)
                    {
                        oldAlbum.Name = Name;
                        oldAlbum.Date = Date;
                        oldAlbum.GalleryId = GalleryId;
                        if (AlbumCover != "")
                        {
                            string filePath = "~/" + oldAlbum.Photo;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldAlbum.Photo = AlbumCover;
                        }

                        db.SaveChanges();
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveAlbumPhoto";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAlbum(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Album album = db.Albums.Where(t => t.Id == id).FirstOrDefault();
                if (album != null)
                {
                    List<AlbumPhoto> albumPhotoList = db.AlbumPhotoes.Where(t => t.AlbumId == id).ToList();
                    if (albumPhotoList != null && albumPhotoList.Count > 0)
                    {
                        string folderPath = "~/UploadImages/Album/" + album.Name;
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                        if (exists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPath), true);

                        string folderPathThumb = "~/UploadImages/AlbumThumb/" + album.Name;
                        bool thumbExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPathThumb));
                        if (thumbExists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPathThumb), true);

                        db.AlbumPhotoes.RemoveRange(albumPhotoList);
                        db.SaveChanges();
                    }

                    db.Albums.Remove(album);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteAlbum";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteGalleryAlbum(long GalleryId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Album> albumList = db.Albums.Where(t => t.GalleryId == GalleryId).ToList();
                if (albumList != null)
                {
                    foreach (Album album in albumList)
                    {
                        DeleteFullAlbumPhoto(album.Id);
                        if (album.Photo != null && album.Photo != "")
                        {
                            string filePath = "~/" + album.Photo;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));
                        }
                        db.Albums.Remove(album);
                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteAlbum";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAlbumList(long GalleryId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<AlbumData> list = new List<AlbumData>();
                List<Album> albumList = new List<Album>();
                albumList = db.Albums.Where(a => a.GalleryId == GalleryId).ToList();
                if (albumList != null)
                {
                    foreach (Album album in albumList)
                    {
                        AlbumData albumData = new AlbumData();
                        albumData.Id = album.Id;
                        albumData.Name = album.Name;
                        albumData.Date = album.Date;
                        albumData.Photo = album.Photo;
                        albumData.GalleryId = album.GalleryId;
                        albumData.SelectedCount = db.AlbumPhotoes.Where(ap => ap.AlbumId == album.Id && ap.IsSelected == true).Count();
                        albumData.AllCount = db.AlbumPhotoes.Where(ap => ap.AlbumId == album.Id).Count();
                        list.Add(albumData);
                    }

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = list;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAlbumList";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAlbumCustomerList(long albumId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<GetAlbumCustomer_Result> albumList = new List<GetAlbumCustomer_Result>();
                albumList = db.GetAlbumCustomer(albumId).ToList();
                if (albumList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = albumList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAlbumList";

                return resultData;
            }
        }

        [HttpPost]
        public ResultData UpdateAlbumSelection(List<AppAlbumPhotoUpdate> dataList)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (dataList != null && dataList.Count > 0)
                {
                    foreach (AppAlbumPhotoUpdate photo in dataList)
                    {
                        AlbumPhoto albumPhoto = db.AlbumPhotoes.Where(ap => ap.Id == photo.Id).First();
                        if (albumPhoto != null)
                        {
                            albumPhoto.IsSelected = photo.IsSelected;
                            db.SaveChanges();
                        }
                    }
                }

                resultData.Message = "Data Updated Successfully !";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;
                return resultData;
            }
        }

        #endregion

        #region Customer
        [HttpPost]
        public ResultData SaveCustomer(Customer data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.Customers.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    Customer oldCustomer = db.Customers.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldCustomer != null)
                    {
                        oldCustomer.Name = data.Name;
                        oldCustomer.Mobile = data.Mobile;
                        oldCustomer.Email = data.Email;
                        oldCustomer.UserName = data.UserName;
                        oldCustomer.Password = data.Password;
                        oldCustomer.StudioId = data.StudioId;

                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveCustomer";

                return resultData;
            }
        }

        [HttpPost]
        public ResultData SaveCustomerAlbum(GalleryCustomer data)
        {
            ResultData resultData = new ResultData();
            try
            {
                GalleryCustomer customerAlbum = db.GalleryCustomers.Where(ca => ca.GalleryId == data.GalleryId && ca.CustomerId == data.CustomerId).FirstOrDefault();
                if (customerAlbum != null && customerAlbum.Id > 0)
                {
                    db.GalleryCustomers.Remove(customerAlbum);
                    db.SaveChanges();
                }
                else if (data != null)
                {
                    db.GalleryCustomers.Add(data);
                    db.SaveChanges();
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveCustomer";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteCustomer(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Customer customer = db.Customers.Where(t => t.Id == id).FirstOrDefault();
                if (customer != null)
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteCustomer";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetCustomerList(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Customer> customerList = new List<Customer>();
                customerList = db.Customers.Where(c => c.ParentId == null && c.StudioId == studioId).ToList();
                if (customerList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = customerList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetCustomerList";
                return resultData;
            }
        }


        public void GetCustomerFcmTokenByGalleryId(long galleryId)
        {

            try
            {
                List<GetCustomerFcmTokenByGallery_Result> customerList = new List<GetCustomerFcmTokenByGallery_Result>();
                customerList = db.GetCustomerFcmTokenByGallery(galleryId).ToList();
                if (customerList != null)
                {
                    foreach (GetCustomerFcmTokenByGallery_Result data in customerList)
                    {
                        long customerId = Convert.ToInt64(data.Id);
                        string devcieId = data.FcmToken;
                        string message = "New Album added to your Gallery. Please Login to your app for more detail";
                        string title = "New Album Added";
                        SendPushNotification(devcieId, message, title);

                        if (data.Id != null && data.Id == 0)
                        {
                            Notification notification = new Notification();
                            notification.CustomerId = customerId;
                            notification.Description = message;
                            notification.Title = title;

                            db.Notifications.Add(notification);
                            db.SaveChanges();
                        }

                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public ResultData GetCustomerGalleryList(long customerId, long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetCustomerAssignGallery_Result> galleryList = new List<GetCustomerAssignGallery_Result>();
                galleryList = db.GetCustomerAssignGallery(customerId, studioId).ToList();
                if (galleryList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = galleryList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetCustomerList";
                return resultData;
            }
        }
        #endregion

        #region AlbumPhoto
        [HttpPost]
        [ActionName("SaveAlbumPhoto")]
        public ResultData SaveAlbumPhoto()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;

                string albumId = Convert.ToString(requestObject.Form["AlbumId"]);

                if (albumId != null && albumId != "")
                {
                    long AlbumId = Convert.ToInt64(albumId);
                    Album album = db.Albums.Where(a => a.Id == AlbumId).FirstOrDefault();
                    if (album != null)
                    {
                        string[] paths = sf.Upload(requestObject, Constants.FileTypeAlbumPhotos, album.Name);

                        if (paths != null && paths.Length > 0)
                        {
                            List<AlbumPhoto> albumPhotoList = new List<AlbumPhoto>();
                            foreach (string path in paths)
                            {
                                AlbumPhoto albumPhoto = new AlbumPhoto();
                                albumPhoto.AlbumId = Convert.ToInt64(AlbumId);
                                albumPhoto.Photo = "UploadImages/Album/" + path;
                                albumPhoto.PhotoThumb = "UploadImages/AlbumThumb/" + path;
                                albumPhoto.IsSelected = false;
                                albumPhotoList.Add(albumPhoto);
                            }

                            db.AlbumPhotoes.AddRange(albumPhotoList);
                            db.SaveChanges();
                        }
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveAlbumPhoto";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAlbumPhoto(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                AlbumPhoto albumPhoto = db.AlbumPhotoes.Where(t => t.Id == id).FirstOrDefault();
                if (albumPhoto != null)
                {
                    string filePath = "~/" + albumPhoto.Photo;
                    bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                    if (exists)
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                    string filePathThumb = "~/" + albumPhoto.PhotoThumb;
                    bool thumbExists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePathThumb));
                    if (thumbExists)
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePathThumb));

                    db.AlbumPhotoes.Remove(albumPhoto);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteCustomer";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteFullAlbumPhoto(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumPhoto> albumPhotoList = db.AlbumPhotoes.Where(t => t.AlbumId == AlbumId).ToList();
                if (albumPhotoList != null && albumPhotoList.Count > 0)
                {
                    Album album = db.Albums.Where(a => a.Id == AlbumId).FirstOrDefault();
                    if (album != null)
                    {
                        string folderPath = "~/UploadImages/Album/" + album.Name;
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                        if (exists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPath), true);

                        string folderPathThumb = "~/UploadImages/AlbumThumb/" + album.Name;
                        bool thumbExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPathThumb));
                        if (thumbExists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPathThumb), true);
                    }

                    db.AlbumPhotoes.RemoveRange(albumPhotoList);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteCustomer";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAlbumPhotoList(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumPhoto> albumPhotoList = new List<AlbumPhoto>();
                albumPhotoList = db.AlbumPhotoes.Where(a => a.AlbumId == AlbumId).OrderBy(o => o.Photo).ToList();
                if (albumPhotoList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = albumPhotoList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAlbumPhotoList";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetSelectedAlbumPhotoList(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumPhoto> albumPhotoList = new List<AlbumPhoto>();
                albumPhotoList = db.AlbumPhotoes.Where(a => a.AlbumId == AlbumId && a.IsSelected == true).ToList();
                if (albumPhotoList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = albumPhotoList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAlbumPhotoList";
                return resultData;
            }
        }
        #endregion

        #region AlbumVideo
        [HttpPost]
        [ActionName("SaveAlbumVideo")]
        public ResultData SaveAlbumVideo()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;

                string albumId = Convert.ToString(requestObject.Form["AlbumId"]);
                string title = Convert.ToString(requestObject.Form["Title"]);

                if (albumId != null && albumId != "")
                {
                    long AlbumId = Convert.ToInt64(albumId);
                    Album album = db.Albums.Where(a => a.Id == AlbumId).FirstOrDefault();
                    if (album != null)
                    {
                        string[] paths = sf.Upload(requestObject, Constants.FileTypeVideo, album.Name);

                        if (paths != null && paths.Length > 0)
                        {
                            List<AlbumVideo> albumPhotoList = new List<AlbumVideo>();
                            foreach (string path in paths)
                            {
                                AlbumVideo albumVideo = new AlbumVideo();
                                albumVideo.AlbumId = Convert.ToInt64(AlbumId);
                                albumVideo.Video = path;
                                albumVideo.Title = title;
                                albumPhotoList.Add(albumVideo);
                            }

                            db.AlbumVideos.AddRange(albumPhotoList);
                            db.SaveChanges();
                        }
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveAlbumVideo";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAlbumVideo(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                AlbumVideo albumVideo = db.AlbumVideos.Where(t => t.Id == id).FirstOrDefault();
                if (albumVideo != null)
                {
                    string filePath = "~/" + albumVideo.Video;
                    bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                    if (exists)
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                    db.AlbumVideos.Remove(albumVideo);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteAlbumVideo";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteFullAlbumVideo(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumVideo> albumVideoList = db.AlbumVideos.Where(t => t.AlbumId == AlbumId).ToList();
                if (albumVideoList != null && albumVideoList.Count > 0)
                {
                    Album album = db.Albums.Where(a => a.Id == AlbumId).FirstOrDefault();
                    if (album != null)
                    {
                        string folderPath = "~/UploadVideo/Album/" + album.Name;
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                        if (exists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPath), true);
                    }

                    db.AlbumVideos.RemoveRange(albumVideoList);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteCustomer";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAlbumVideoList(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumVideo> albumVideoList = new List<AlbumVideo>();
                albumVideoList = db.AlbumVideos.Where(a => a.AlbumId == AlbumId).ToList();
                if (albumVideoList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = albumVideoList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAlbumVideoList";
                return resultData;
            }
        }
        #endregion

        #region Category

        [ActionName("SaveCategoryData")]
        public ResultData SaveCategoryData()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeCategoryPhotos, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string Title = Convert.ToString(requestObject.Form["Title"]);
                long StudioId = Convert.ToInt64(requestObject.Form["StudioId"]);

                string AlbumCover = "";
                if (paths != null && paths.Length > 0)
                    AlbumCover = paths[0];
                if (Id != null && Id == 0)
                {
                    Category category = new Category();
                    category.Title = Title;
                    category.Image = AlbumCover;
                    category.StudioId = StudioId;

                    db.Categories.Add(category);
                    db.SaveChanges();
                }
                else
                {
                    Category oldCategory = db.Categories.Where(c => c.Id == Id).FirstOrDefault();
                    if (oldCategory != null)
                    {
                        oldCategory.Title = Title;
                        oldCategory.Image = AlbumCover;
                        oldCategory.StudioId = StudioId;
                        if (AlbumCover != "")
                        {
                            string filePath = "~/" + oldCategory.Image;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldCategory.Image = AlbumCover;
                        }

                        db.SaveChanges();
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveCategoryData";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetCategoryList(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<Category> categoryList = new List<Category>();
                categoryList = db.Categories.Where(c => c.StudioId == studioId).ToList();
                if (categoryList != null)
                {

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = categoryList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetCategoryList";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteCategory(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Category category = db.Categories.Where(t => t.Id == id).FirstOrDefault();
                if (category != null)
                {
                    List<Portfolio> portfolioList = db.Portfolios.Where(t => t.CategoryId == id).ToList();
                    if (portfolioList != null && portfolioList.Count > 0)
                    {
                        string folderPath = "~/UploadImages/Portfolio/" + category.Title;
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                        if (exists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPath), true);

                        string folderPathThumb = "~/UploadImages/PortfolioThumb/" + category.Title;
                        bool thumbExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPathThumb));
                        if (thumbExists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPathThumb), true);

                        db.Portfolios.RemoveRange(portfolioList);
                        db.SaveChanges();
                    }

                    db.Categories.Remove(category);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteCategory";

                return resultData;
            }
        }
        #endregion

        #region Portfolio
        [HttpPost]
        [ActionName("SavePortfolio")]
        public ResultData SavePortfolio()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;

                string categoryId = Convert.ToString(requestObject.Form["CategoryId"]);

                if (categoryId != null && categoryId != "")
                {
                    long CategoryId = Convert.ToInt64(categoryId);
                    Category category = db.Categories.Where(a => a.Id == CategoryId).FirstOrDefault();
                    if (category != null)
                    {
                        string[] paths = sf.Upload(requestObject, Constants.FileTypePortfolioPhotos, category.Title);

                        if (paths != null && paths.Length > 0)
                        {
                            List<Portfolio> portfolioList = new List<Portfolio>();
                            foreach (string path in paths)
                            {
                                Portfolio portfolio = new Portfolio();
                                portfolio.CategoryId = Convert.ToInt64(CategoryId);
                                portfolio.Image = "UploadImages/Portfolio/" + path;
                                portfolio.ImageThumb = "UploadImages/PortfolioThumb/" + path;
                                portfolioList.Add(portfolio);
                            }

                            db.Portfolios.AddRange(portfolioList);
                            db.SaveChanges();
                        }
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SavePortfolio";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeletePortfolio(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Portfolio portfolio = db.Portfolios.Where(t => t.Id == id).FirstOrDefault();
                if (portfolio != null)
                {
                    string filePath = "~/" + portfolio.Image;
                    bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                    if (exists)
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                    string filePathThumb = "~/" + portfolio.ImageThumb;
                    bool thumbExists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePathThumb));
                    if (thumbExists)
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePathThumb));

                    db.Portfolios.Remove(portfolio);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeletePortfolio";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteFullPortfolioPhoto(long categoryId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Portfolio> portfolioList = db.Portfolios.Where(t => t.CategoryId == categoryId).ToList();
                if (portfolioList != null && portfolioList.Count > 0)
                {
                    Category category = db.Categories.Where(a => a.Id == categoryId).FirstOrDefault();
                    if (category != null)
                    {
                        string folderPath = "~/UploadImages/Portfolio/" + category.Title;
                        bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                        if (exists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPath), true);

                        string folderPathThumb = "~/UploadImages/PortfolioThumb/" + category.Title;
                        bool thumbExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPathThumb));
                        if (thumbExists)
                            System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(folderPath), true);
                    }

                    db.Portfolios.RemoveRange(portfolioList);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteFullPortfolioPhoto";

                return resultData;
            }
        }


        [HttpGet]
        public ResultData GetPortfolioList(long CategoryId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Portfolio> portfolioList = new List<Portfolio>();
                portfolioList = db.Portfolios.Where(a => a.CategoryId == CategoryId).ToList();
                if (portfolioList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = portfolioList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetPortfolioList";
                return resultData;
            }
        }


        #endregion

        #region State

        [HttpPost]
        public ResultData SaveState(State data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.States.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    State oldState = db.States.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldState != null)
                    {
                        oldState.Name = data.Name;

                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveState";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteState(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                State state = db.States.Where(t => t.Id == id).FirstOrDefault();
                if (state != null)
                {
                    db.States.Remove(state);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully !";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteState";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetStates()
        {
            ResultData resultData = new ResultData();
            try
            {
                List<State> state = new List<State>();
                state = db.States.ToList();
                if (state != null)
                {
                    resultData.Message = "Data Get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = state;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found !";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetStates";

                return resultData;
            }

        }

        //[HttpGet]
        //public ResultData GetState(int pageNumber, int pageSize, string search)
        //{
        //    ResultData resultData = new ResultData();
        //    try
        //    {
        //        if (search == null)
        //            search = "";
        //        GetStateClass getStateClass = new GetStateClass();
        //        List<GetStateData_Result> stateById = new List<GetStateData_Result>();
        //        ObjectParameter recordCount = new ObjectParameter("Count", typeof(int));

        //        stateById = db.GetStateData(pageNumber, pageSize, search, recordCount).ToList();
        //        if (stateById != null)
        //        {
        //            getStateClass.StateData = stateById;
        //            getStateClass.Count = Convert.ToInt32(recordCount.Value);

        //            resultData.Message = "Data Get Successfully";
        //            resultData.IsSuccess = true;
        //            resultData.Data = getStateClass;
        //            return resultData;
        //        }
        //        else
        //        {
        //            resultData.Message = "Invalid Detail";
        //            resultData.IsSuccess = true;
        //            resultData.Data = 0;
        //            return resultData;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultData.Message = ex.Message.ToString();
        //        resultData.IsSuccess = false;
        //        resultData.Data = 0;

        //        ErrorLog errorLog = new ErrorLog();
        //        errorLog.ErrorMessage = ex.Message.ToString();
        //        errorLog.StackTrace = ex.StackTrace.ToString();
        //        errorLog.EventName = "GetState";

        //        return resultData;
        //    }

        //}

        #endregion

        #region City

        [HttpPost]
        public ResultData SaveCity(City data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.Cities.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    City oldCity = db.Cities.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldCity != null)
                    {
                        oldCity.Name = data.Name;
                        oldCity.StateId = data.StateId;

                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveCity";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteCity(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                City city = db.Cities.Where(t => t.Id == id).FirstOrDefault();
                if (city != null)
                {
                    db.Cities.Remove(city);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteCity";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetCity(long stateId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<City> city = new List<City>();
                city = db.Cities.Where(c => c.StateId == stateId).ToList();
                if (city != null)
                {
                    resultData.Message = "Data Get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = city;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found !";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetCity";

                return resultData;
            }

        }

        #endregion

        #region Organization

        [HttpGet]
        public ResultData GetStudioDataById(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                Studio studio = new Studio();
                studio = db.Studios.Where(s => s.Id == studioId).FirstOrDefault();
                if (studio != null)
                {
                    resultData.Message = "Data Get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = studio;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found !";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetStudioDataById";

                return resultData;
            }
        }

        [ActionName("UpdatestudioData")]
        public ResultData UpdatestudioData()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                Studio dbStudio = db.Studios.Where(m => m.Id == Id).FirstOrDefault();
                if (dbStudio != null && dbStudio.Id > 0)
                {
                    string[] paths = sf.Upload(requestObject, Constants.FileTypeOrganizationPhotos, "");

                    string Name = Convert.ToString(requestObject.Form["Name"]);
                    string About = Convert.ToString(requestObject.Form["About"]);
                    string Mobile = Convert.ToString(requestObject.Form["Mobile"]);
                    string Services = Convert.ToString(requestObject.Form["Services"]);
                    string Email = Convert.ToString(requestObject.Form["Email"]);
                    string Website = Convert.ToString(requestObject.Form["Website"]);
                    string StudioOwner = Convert.ToString(requestObject.Form["StudioOwner"]);
                    string InviteMessage = Convert.ToString(requestObject.Form["InviteMessage"]);

                    string StudioLogo = "";
                    if (paths != null && paths.Length > 0)
                        StudioLogo = paths[0];

                    dbStudio.Name = Name;
                    dbStudio.Mobile = Mobile;
                    dbStudio.About = About;
                    dbStudio.Services = Services;
                    dbStudio.Email = Email;
                    dbStudio.Website = Website;
                    dbStudio.StudioOwner = StudioOwner;
                    dbStudio.StudioLogo = StudioLogo;
                    dbStudio.InviteMessage = InviteMessage;


                    db.SaveChanges();
                }

                resultData.Message = "Data Updated Successfully";
                resultData.IsSuccess = true;
                resultData.Data = Id;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;
                return resultData;
            }
        }


        #endregion

        #region AppointmentSlot

        [HttpPost]
        public ResultData SaveAppointmentSlot(AppointmentSlot data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.AppointmentSlots.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    AppointmentSlot oldAppointmentSlot = db.AppointmentSlots.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldAppointmentSlot != null)
                    {
                        oldAppointmentSlot.Title = data.Title;
                        db.SaveChanges();
                    }
                }

                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveAppointmentSlot";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAppointmentSlot(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                AppointmentSlot appointmentSlot = db.AppointmentSlots.Where(t => t.Id == id).FirstOrDefault();
                if (appointmentSlot != null)
                {
                    db.AppointmentSlots.Remove(appointmentSlot);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteAppointmentSlot";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAppointmentSlotList(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<AppointmentSlot> appointmentSlotList = new List<AppointmentSlot>();
                appointmentSlotList = db.AppointmentSlots.Where(s => s.StudioId == studioId).ToList();
                if (appointmentSlotList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = appointmentSlotList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAppointmentSlotList";

                return resultData;
            }
        }

        #endregion

        #region Appointment

        [HttpGet]
        public ResultData GetAppointmentList(int studioId, DateTime fromDate)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetAppointmentData_Result> getAppointmentDataList = new List<GetAppointmentData_Result>();
                getAppointmentDataList = db.GetAppointmentData(studioId, fromDate).OrderByDescending(a => a.Date).ToList();
                if (getAppointmentDataList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = getAppointmentDataList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetAppointmentList";

                return resultData;
            }
        }

        #endregion

        #region StudioSocialLink

        [ActionName("SaveStudioSocialLink")]
        public ResultData SaveStudioSocialLink()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeStudioSocialLinkPhotos, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string Title = Convert.ToString(requestObject.Form["Title"]);
                string Link = Convert.ToString(requestObject.Form["Link"]);
                long StudioId = Convert.ToInt64(requestObject.Form["StudioId"]);

                string Image = "";
                if (paths != null && paths.Length > 0)
                    Image = paths[0];
                if (Id != null && Id == 0)
                {
                    StudioSocialLink studioSocialLink = new StudioSocialLink();
                    studioSocialLink.Title = Title;
                    studioSocialLink.Link = Link;
                    studioSocialLink.Image = Image;
                    studioSocialLink.StudioId = StudioId;

                    db.StudioSocialLinks.Add(studioSocialLink);
                    db.SaveChanges();
                }
                else
                {
                    StudioSocialLink oldStudioSocialLink = db.StudioSocialLinks.Where(g => g.Id == Id).FirstOrDefault();
                    if (oldStudioSocialLink != null)
                    {
                        oldStudioSocialLink.Title = Title;
                        oldStudioSocialLink.Link = Link;
                        oldStudioSocialLink.StudioId = StudioId;
                        if (Image != "")
                        {
                            string filePath = "~/" + oldStudioSocialLink.Image;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldStudioSocialLink.Image = Image;
                        }

                        db.SaveChanges();
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveStudioSocialLink";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetStudioSocialLinkList(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<StudioSocialLink> studioSocialLinkList = new List<StudioSocialLink>();
                studioSocialLinkList = db.StudioSocialLinks.Where(s => s.StudioId == studioId).ToList();
                if (studioSocialLinkList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = studioSocialLinkList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<StudioAbout>();
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetStudioSocialLinkList";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteStudioSocialLink(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                StudioSocialLink studioSocialLink = db.StudioSocialLinks.Where(t => t.Id == id).FirstOrDefault();
                if (studioSocialLink != null)
                {
                    string filePath = "~/" + studioSocialLink.Image;
                    bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                    if (exists)
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                    db.StudioSocialLinks.Remove(studioSocialLink);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteStudioSocialLink";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetFontList()
        {
            ResultData resultData = new ResultData();
            try
            {
                List<string> fonts = new List<string>();

                foreach (FontFamily font in System.Drawing.FontFamily.Families)
                {
                    fonts.Add(font.Name);
                }

                resultData.Message = "Data Get Successfully";
                resultData.IsSuccess = true;
                resultData.Data = fonts;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetStudioSocialLinkList";
                return resultData;
            }
        }

        #endregion

        #region Photographer

        [ActionName("SavePhotographer")]
        public ResultData SavePhotographer()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypePhotographer, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string Name = Convert.ToString(requestObject.Form["Name"]);
                string Email = Convert.ToString(requestObject.Form["Email"]);
                string MobileNo = Convert.ToString(requestObject.Form["MobileNo"]);
                Boolean IsActive = Convert.ToBoolean(requestObject.Form["IsActive"]);
                long StudioId = Convert.ToInt64(requestObject.Form["StudioId"]);
                long BranchId = Convert.ToInt64(requestObject.Form["BranchId"]);

                Random generator = new Random();
                String Uniqno = generator.Next(1000, 9999).ToString();
                string Code = Name.Substring(0, 4);
                string ReferalCode = Code + Uniqno;


                string Image = "";
                if (paths != null && paths.Length > 0)
                    Image = paths[0];
                if (Id != null && Id == 0)
                {
                    Photographer photographer = new Photographer();
                    photographer.Name = Name;
                    photographer.Email = Email;
                    photographer.MobileNo = MobileNo;
                    photographer.Image = Image;
                    photographer.IsActive = IsActive;
                    photographer.StudioId = StudioId;
                    photographer.BranchId = BranchId;
                    photographer.ReferalCode = ReferalCode;

                    db.Photographers.Add(photographer);
                    db.SaveChanges();
                }
                else
                {
                    Photographer oldPhotographer = db.Photographers.Where(g => g.Id == Id).FirstOrDefault();
                    if (oldPhotographer != null)
                    {
                        oldPhotographer.Name = Name;
                        oldPhotographer.Email = Email;
                        oldPhotographer.MobileNo = MobileNo;
                        oldPhotographer.IsActive = IsActive;
                        oldPhotographer.StudioId = StudioId;
                        oldPhotographer.BranchId = BranchId;
                        if (Image != "")
                        {
                            string filePath = "~/" + oldPhotographer.Image;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldPhotographer.Image = Image;
                        }

                        db.SaveChanges();
                    }
                }
                resultData.Message = "Data Saved Successfully";
                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SavePhotographer";

                return resultData;
            }
        }


        [HttpGet]
        public ResultData DeletePhotographer(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Photographer photographer = db.Photographers.Where(t => t.Id == id).FirstOrDefault();
                if (photographer != null)
                {
                    db.Photographers.Remove(photographer);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully !";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeletePhotographer";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetPhotographer()
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Photographer> photographer = new List<Photographer>();
                photographer = db.Photographers.ToList();
                if (photographer != null)
                {
                    resultData.Message = "Data Get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = photographer;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found !";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetContest";

                return resultData;
            }

        }

        #endregion

        #region Contest
        [ActionName("SaveContest")]
        public ResultData SaveContest()
        {
            ResultData resultData = new ResultData();
            try
            {
               
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeContest, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string ContestName = Convert.ToString(requestObject.Form["ContestName"]);
                DateTime StartDate = Convert.ToDateTime(requestObject.Form["StartDate"]);
                DateTime EndDate = Convert.ToDateTime(requestObject.Form["EndDate"]);
                string Fees = Convert.ToString(requestObject.Form["Fees"]);

                string Image = "";
                if (paths != null && paths.Length > 0)
                    Image = paths[0];
                if (Id != null && Id == 0)
                {
                    Contest contest = new Contest();
                    contest.ContestName = ContestName;
                    contest.StartDate = StartDate;
                    contest.EndDate = EndDate;
                    contest.Fees = Fees;

                    db.Contests.Add(contest);
                    db.SaveChanges();
                    
                }
                else
                {
                    Contest oldContest = db.Contests.Where(g => g.Id == Id).FirstOrDefault();
                    if (oldContest != null)
                    {
                        oldContest.ContestName = ContestName;
                        oldContest.StartDate = StartDate;
                        oldContest.EndDate = EndDate;
                        oldContest.Fees = Fees;
                        if (Image != "")
                        {
                            string filePath = "~/" + oldContest.Image;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldContest.Image = Image;
                        }

                        db.SaveChanges();
                    }
                }

                resultData.IsSuccess = true;
                resultData.Data = "1";
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "SaveAlbumPhoto";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteContest(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Contest contest = db.Contests.Where(t => t.Id == id).FirstOrDefault();
                if (contest != null)
                {
                    db.Contests.Remove(contest);
                    db.SaveChanges();
                }

                resultData.Message = "Data Deleted Successfully !";
                resultData.IsSuccess = true;
                resultData.Data = 1;
                return resultData;
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "DeleteContest";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetContest()
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Contest> contest = new List<Contest>();
                contest = db.Contests.ToList();
                if (contest != null)
                {
                    resultData.Message = "Data Get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = contest;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found !";
                    resultData.IsSuccess = true;
                    resultData.Data = 0;
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                resultData.Message = ex.Message.ToString();
                resultData.IsSuccess = false;
                resultData.Data = 0;

                ErrorLog errorLog = new ErrorLog();
                errorLog.ErrorMessage = ex.Message.ToString();
                errorLog.StackTrace = ex.StackTrace.ToString();
                errorLog.EventName = "GetContest";

                return resultData;
            }

        }
        #endregion

        public void SendPushNotification(string deviceId, string message, string title)
        {
            try
            {
                WebRequest trequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                trequest.Method = "post";
                //serverkey - key from firebase cloud messaging server  
                trequest.Headers.Add(string.Format("authorization: key={0}", "AIzaSyDkVQ78XlXSNcQp5HA6x23FcY195JwKZnE"));
                //sender id - from firebase project setting  
                trequest.Headers.Add(string.Format("sender: id={0}", "592253101958"));
                trequest.ContentType = "application/json";
                var payload = new
                {
                    to = deviceId,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = message,
                        title = title,
                        badge = 1
                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                byte[] bytearray = Encoding.UTF8.GetBytes(postbody);
                trequest.ContentLength = bytearray.Length;
                using (Stream datastream = trequest.GetRequestStream())
                {
                    datastream.Write(bytearray, 0, bytearray.Length);
                    using (WebResponse tresponse = trequest.GetResponse())
                    {
                        using (Stream datastreamresponse = tresponse.GetResponseStream())
                        {
                            if (datastreamresponse != null) using (StreamReader treader = new StreamReader(datastreamresponse))
                                {
                                    string sresponsefromserver = treader.ReadToEnd();
                                    //result.response = sresponsefromserver;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}
