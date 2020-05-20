using PicTick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PicTick.Controllers
{
    public class PhotographerAPIController : ApiController
    {
        PicTickEntities db = new PicTickEntities();
        StudyField sf = new StudyField();

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
                string MobileNo = Convert.ToString(requestObject.Form["MobileNo"]);
                if (Id != null && Id == 0)
                {
                    List<Photographer> PhotographerList = new List<Photographer>();
                    PhotographerList = db.Photographers.Where(n => n.MobileNo == MobileNo).ToList();
                    if (PhotographerList.Count > 0)
                    {
                        resultData.Message = "Mobile Already Exist !";
                        resultData.IsSuccess = true;
                        resultData.Data = 0;
                        return resultData;
                    }
                }

                string Name = Convert.ToString(requestObject.Form["Name"]);
                string Email = Convert.ToString(requestObject.Form["Email"]);
                
                Boolean IsActive = Convert.ToBoolean(requestObject.Form["IsActive"]);
                long StudioId = Convert.ToInt64(requestObject.Form["StudioId"]);
                long BranchId = Convert.ToInt64(requestObject.Form["BranchId"]);
                string ReferalCode = Convert.ToString(requestObject.Form["ReferalCode"]);


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
        public ResultData GetPhotographerListByStudioAndBranch(long studioId,long branchId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<GetPhotographerByStudioAndBranch_Result> photographerList = new List<GetPhotographerByStudioAndBranch_Result>();
                photographerList = db.GetPhotographerByStudioAndBranch(studioId,branchId).ToList();
                if (photographerList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = photographerList;
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
                errorLog.EventName = "GetPhotographerListByStudioAndBranch";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData UpdatePhotgrapherData(long photographerId, string name, string mobile, string email)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdatePhotographerData(photographerId, name, mobile, email);
                resultData.Message = "Successfully !";
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

        [ActionName("UpdatePhotographerPhoto")]
        public ResultData UpdatePhotographerPhoto()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;

                string[] paths = sf.Upload(requestObject, Constants.FileTypePhotographer,"");
                long Id = Convert.ToInt64(requestObject.Form["Id"]);

                string Image = "";
                if (requestObject.Files.AllKeys.Any())
                    Image = paths[0];

                if (Id != null && Id != 0)
                {
                    Photographer photographer = db.Photographers.Where(m => m.Id == Id).FirstOrDefault();
                    if (photographer != null && Image != "")
                    {
                        photographer.Image = Image;
                        db.SaveChanges();
                    }

                    resultData.Data = Image;
                    resultData.Message = "Data Updated Successfully !";
                    resultData.IsSuccess = true;
                }
                else
                {
                    resultData.Data = 0;
                    resultData.Message = "Photographer Data Not Match";
                    resultData.IsSuccess = false;
                }

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
                errorLog.EventName = "UpdatePhotographerPhoto";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData UpdateRefearAndEarn(long photographerId, string ReferalCode)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdaterRefearAndEarn(photographerId, ReferalCode);
                resultData.Message = "Successfully !";
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

        #region Login

        [HttpGet]
        public ResultData PhotographerLogin(string mobileNo)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Photographer> photographerList = new List<Photographer>();
                photographerList = db.Photographers.Where(c => c.MobileNo == mobileNo).ToList();
                if (photographerList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = photographerList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<Photographer>();
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
                errorLog.EventName = "PhotographerLogin";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData SendVerificationCode(string mobileNo, string code)
        {
            ResultData resultData = new ResultData();
            try
            {
                SendSMS sms = new SendSMS();
                string res = sms.sendotp("you Verification Code is " + code, mobileNo);

                if (res.Equals("ok"))
                {
                    resultData.Message = "Data Get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = 1;
                    return resultData;
                }
                else
                {
                    resultData.Message = "Failed !";
                    resultData.IsSuccess = false;
                    resultData.Data = 0;
                    return resultData;
                }
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
                errorLog.EventName = "SendVerificationCode";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData PhotographerOTPVerification(long photographerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdatePhotographerVerification(photographerId);
                resultData.Message = "Successfully !";
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

        [HttpGet]
        public ResultData UpdatePhotographerFCMToken(long photographerId, string fcmToken)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdatePhotographerFCMToken(photographerId, fcmToken);
                resultData.Message = "Successfully !";
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

        [ActionName("SaveCustomer")]
        public ResultData SaveCustomer()
        {
            ResultData resultData = new ResultData();
            try
            {
                var properties = Request.Properties as Dictionary<string, object>;
                var requestObject = ((System.Web.HttpContextWrapper)(properties["MS_HttpContext"])).Request;
                string[] paths = sf.Upload(requestObject, Constants.FileTypeCustomer, "");

                long Id = Convert.ToInt64(requestObject.Form["Id"]);
                string MobileNo = Convert.ToString(requestObject.Form["MobileNo"]);
                if (Id != null && Id == 0)
                {
                    List<Customer> customerList = new List<Customer>();
                    customerList = db.Customers.Where(n => n.Mobile == MobileNo).ToList();
                    if (customerList.Count > 0)
                    {
                        resultData.Message = "Mobile Already Exist !";
                        resultData.IsSuccess = true;
                        resultData.Data = 0;
                        return resultData;
                    }
                }
                string Name = Convert.ToString(requestObject.Form["Name"]);
                string Email = Convert.ToString(requestObject.Form["Email"]);
                string UserName = Convert.ToString(requestObject.Form["UserName"]);
                string Password = Convert.ToString(requestObject.Form["Password"]);
                long StudioId = Convert.ToInt64(requestObject.Form["StudioId"]);
                long PhotographerId = Convert.ToInt64(requestObject.Form["PhotographerId"]);


                string Image = "";
                if (paths != null && paths.Length > 0)
                    Image = paths[0];
                if (Id != null && Id == 0)
                {
                    Customer customer = new Customer();
                    customer.Name = Name;
                    customer.Email = Email;
                    customer.Mobile = MobileNo;
                    customer.Image = Image;
                    customer.StudioId = StudioId;
                    customer.PhotographerId = PhotographerId;

                    db.Customers.Add(customer);
                    db.SaveChanges();
                }
                else
                {
                    Customer oldCustomer = db.Customers.Where(g => g.Id == Id).FirstOrDefault();
                    if (oldCustomer != null)
                    {
                        oldCustomer.Name = Name;
                        oldCustomer.Email = Email;
                        oldCustomer.Mobile = MobileNo;
                        oldCustomer.StudioId = StudioId;
                        oldCustomer.PhotographerId = PhotographerId;
                        if (Image != "")
                        {
                            string filePath = "~/" + oldCustomer.Image;
                            bool exists = System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath));
                            if (exists)
                                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));

                            oldCustomer.Image = Image;
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
                errorLog.EventName = "SaveCustomer";

                return resultData;
            }
        }


        #endregion

        #region AddressBranch

        [HttpPost]
        public ResultData SaveAddressBranch(StudioAddress data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.StudioAddresses.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    StudioAddress oldStudioAddress = db.StudioAddresses.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldStudioAddress != null)
                    {
                        oldStudioAddress.Country = data.Country;
                        oldStudioAddress.StateId = data.StateId;
                        oldStudioAddress.CityId = data.CityId;
                        oldStudioAddress.StudioId = data.StudioId;
                        oldStudioAddress.Address = data.Address;
                        oldStudioAddress.LatLong = data.LatLong;
                        oldStudioAddress.Pincode = data.Pincode;
                        oldStudioAddress.StudioName = data.StudioName;


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
                errorLog.EventName = "SaveAddressBranch";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAddressBranch(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                StudioAddress studioAddress = db.StudioAddresses.Where(t => t.Id == id).FirstOrDefault();
                if (studioAddress != null)
                {
                    db.StudioAddresses.Remove(studioAddress);
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
                errorLog.EventName = "DeleteAddressBranch";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAddressBranch(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {

                List<GetStudioAddressBranch_Result> addressBranchList = new List<GetStudioAddressBranch_Result>();
                addressBranchList = db.GetStudioAddressBranch(studioId).ToList();
                if (addressBranchList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = addressBranchList;
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
                errorLog.EventName = "GetAddressBranch";

                return resultData;
            }
        }

        #endregion

        #region Album

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
        public ResultData GetCustomerGalleryList(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetCustomerGalleryData_Result> galleryList = new List<GetCustomerGalleryData_Result>();
                galleryList = db.GetCustomerGalleryData(customerId).ToList();
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
        public ResultData GetCustomerAlbumByAlbumId(long albumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetCustomerAlbumByAlbumId_Result> aboutList = new List<GetCustomerAlbumByAlbumId_Result>();
                aboutList = db.GetCustomerAlbumByAlbumId(albumId).ToList();
                if (aboutList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = aboutList;
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
                errorLog.EventName = "GetCustomerAlbumByAlbumId";
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

        #region Contest



        #endregion
    }
}
