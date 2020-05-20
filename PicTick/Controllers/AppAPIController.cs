using PicTick.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PicTick.Controllers
{
    public class AppAPIController : ApiController
    {
        PicTickEntities db = new PicTickEntities();
        StudyField sf = new StudyField();

        [HttpGet]
        public ResultData CustomerLogin(string mobileNo)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Customer> customerList = new List<Customer>();
                customerList = db.Customers.Where(c => c.Mobile == mobileNo).ToList();
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
                    resultData.Data = new List<Customer>();
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
        public ResultData CustomerOTPVerification(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdateCustomerVerification(customerId);
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
        public ResultData GetDashboardAlbumList(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetCustomerAlbumByCustomerId_Result> dashboardAlbumList = new List<GetCustomerAlbumByCustomerId_Result>();
                dashboardAlbumList = db.GetCustomerAlbumByCustomerId(customerId).ToList();
                if (dashboardAlbumList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = dashboardAlbumList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<GetCustomerAlbumByCustomerId_Result>();
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
        public ResultData GetAlbumPhotoList(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumPhotoAll> dataList = new List<AlbumPhotoAll>();
                List<AlbumPhoto> albumPhotoList = new List<AlbumPhoto>();
                albumPhotoList = db.AlbumPhotoes.Where(a => a.AlbumId == AlbumId).ToList();

                if (albumPhotoList != null)
                {
                    AlbumPhotoAll data = new AlbumPhotoAll();
                    data.PhotoList = albumPhotoList;
                    data.SelectedCount = db.AlbumPhotoes.Where(a => a.AlbumId == AlbumId && a.IsSelected == true).ToList().Count; ;

                    dataList.Add(data);

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = dataList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<AlbumPhoto>();
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
                    resultData.Data = new List<AlbumPhoto>();
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
        public ResultData GetPendingAlbumPhotoList(long AlbumId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AlbumPhoto> albumPhotoList = new List<AlbumPhoto>();
                albumPhotoList = db.AlbumPhotoes.Where(a => a.AlbumId == AlbumId && a.IsSelected == false).ToList();
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
                    resultData.Data = new List<AlbumPhoto>();
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
        public ResultData GetStudioAboutList(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<StudioAbout> studioAboutList = new List<StudioAbout>();
                studioAboutList = db.StudioAbouts.Where(s => s.StudioId == studioId).ToList();
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
                errorLog.EventName = "GetStudioAboutList";
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

        [HttpGet]
        public ResultData UpdateCustomerFCMToken(long customerId, string fcmToken)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdateFCMToken(customerId, fcmToken);
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
        public ResultData GetCustomerNotificationList(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Notification> studioAboutList = new List<Notification>();
                studioAboutList = db.Notifications.Where(n => n.CustomerId == customerId).ToList();
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
                errorLog.EventName = "GetStudioAboutList";
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
        public ResultData GetCustomerGalleryList(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetCustomerGallery_Result> galleryList = new List<GetCustomerGallery_Result>();
                galleryList = db.GetCustomerGallery(customerId).ToList();
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
                errorLog.EventName = "GetCustomerGalleryList";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetCustomerAlbumList(long galleryId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetCustomerAlbumByGalleryId_Result> dashboardAlbumList = new List<GetCustomerAlbumByGalleryId_Result>();
                dashboardAlbumList = db.GetCustomerAlbumByGalleryId(galleryId).ToList();
                if (dashboardAlbumList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = dashboardAlbumList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<GetCustomerAlbumByCustomerId_Result>();
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
                errorLog.EventName = "GetCustomerAlbumList";

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
                    resultData.Data = new List<Category>();
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
                    resultData.Data = new List<Portfolio>();
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

        [HttpPost]
        public ResultData SaveCustomerList(List<Customer> data)
        {
            ResultData responseData = new ResultData();
            try
            {
                foreach (Customer customerData in data)
                {
                    Customer customer = db.Customers.Where(c => c.Mobile == customerData.Mobile).FirstOrDefault();
                    if (customer == null || customer.Id == 0)
                    {
                        db.Customers.Add(customerData);
                        db.SaveChanges();
                    }
                    else
                    {
                        responseData.Message = "Mobile Already Exists !";
                        responseData.IsSuccess = true;
                        responseData.Data = 0;
                        return responseData;
                    }
                }

                responseData.Message = "Data Saved Successfully !";
                responseData.IsSuccess = true;
                responseData.Data = 1;
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.Message = ex.Message.ToString();
                responseData.IsSuccess = false;
                responseData.Data = 0;
                return responseData;
            }
        }

        [HttpGet]
        public ResultData GetAppointmentSlotList(long studioId, DateTime date)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetAppointmentSlot_Result> appointmentSlotList = new List<GetAppointmentSlot_Result>();
                appointmentSlotList = db.GetAppointmentSlot(studioId, date).ToList();
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
                    resultData.Data = new List<GetAppointmentSlot_Result>();
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

        [HttpPost]
        public ResultData SaveAppointment(Appointment data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.Appointments.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    Appointment oldAppointment = db.Appointments.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldAppointment != null)
                    {
                        oldAppointment.AppointmentSlotId = data.AppointmentSlotId;
                        oldAppointment.StudioId = data.StudioId;
                        oldAppointment.Date = data.Date;
                        oldAppointment.CustomerId = data.CustomerId;
                        oldAppointment.Description = data.Description;
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
                errorLog.EventName = "SaveAppointment";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAppointment(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Appointment appointment = db.Appointments.Where(t => t.Id == id).FirstOrDefault();
                if (appointment != null)
                {
                    db.Appointments.Remove(appointment);
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
                errorLog.EventName = "DeleteAppointment";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAppointmentList()
        {
            ResultData resultData = new ResultData();
            try
            {

                List<Appointment> appointmentList = new List<Appointment>();
                appointmentList = db.Appointments.ToList();
                if (appointmentList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = appointmentList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<Appointment>();
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

        [HttpGet]
        public ResultData GetAppointmentByCustomerId(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Appointment> appointmentList = new List<Appointment>();
                appointmentList = db.Appointments.Where(a => a.CustomerId == customerId).OrderByDescending(a => a.Date).ToList();
                if (appointmentList != null)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = appointmentList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<Appointment>();
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
                errorLog.EventName = "GetAppointmentByCustomerId";

                return resultData;
            }
        }

        [HttpPost]
        public ResultData GuestSignUp(Customer data)
        {
            ResultData responseData = new ResultData();
            try
            {
                Customer customer = db.Customers.Where(c => c.Mobile == data.Mobile).FirstOrDefault();
                if (customer != null && customer.Id > 0)
                {
                    responseData.Message = "Mobile No Already Exists!";
                    responseData.IsSuccess = false;
                    responseData.Data = 0;
                }
                else
                {
                    db.Customers.Add(data);
                    db.SaveChanges();
                    responseData.Message = "Data Saved Successfully !";
                    responseData.IsSuccess = true;
                    responseData.Data = 1;
                }
                return responseData;
            }
            catch (Exception ex)
            {
                responseData.Message = ex.Message.ToString();
                responseData.IsSuccess = false;
                responseData.Data = 0;
                return responseData;
            }
        }

        [HttpGet]
        public ResultData GetCustomerChildList(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<Customer> customerList = new List<Customer>();
                customerList = db.Customers.Where(a => a.ParentId == customerId).ToList();
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
                    resultData.Data = new List<Customer>();
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
                errorLog.EventName = "GetCustomerChildList";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteCustomerWithChild(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                Customer customer = db.Customers.Where(t => t.Id == id).FirstOrDefault();
                if (customer != null)
                {
                    List<Customer> customerList = db.Customers.Where(c => c.ParentId == id).ToList();
                    if (customerList != null)
                    {
                        db.Customers.RemoveRange(customerList);
                        db.SaveChanges();
                    }
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
                errorLog.EventName = "DeleteCustomerWithChild";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetStudioInviteMessage(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                Studio studio = new Studio();
                studio = db.Studios.Where(s => s.Id == studioId).FirstOrDefault();
                if (studio != null)
                {
                    resultData.Message = studio.InviteMessage;
                    resultData.IsSuccess = true;
                    resultData.Data = 1;
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
                errorLog.EventName = "GetStudioInviteMessage";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData UpdateCustomerStatus(long customerId)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdateCustomerInviteStatus(customerId, "Invited");
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
        public ResultData UpdateGallerySelectionStatus(long galleryId, Boolean status)
        {
            ResultData resultData = new ResultData();
            try
            {
                Gallery gallery = db.Galleries.Where(g => g.Id == galleryId).FirstOrDefault();
                if (gallery != null)
                {
                    gallery.IsSelectionDone = status;
                    db.SaveChanges();
                }
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

        [HttpPost]
        public ResultData SaveAlbumPhotoComment(AlbumPhotoComment data)
        {
            ResultData resultData = new ResultData();
            try
            {
                if (data != null && data.Id == 0)
                {
                    db.AlbumPhotoComments.Add(data);
                    db.SaveChanges();
                }
                else if (data.Id > 0)
                {
                    AlbumPhotoComment oldAlbumPhotoComment = db.AlbumPhotoComments.Where(n => n.Id == data.Id).FirstOrDefault();
                    if (oldAlbumPhotoComment != null)
                    {
                        oldAlbumPhotoComment.CustomerId = data.CustomerId;
                        oldAlbumPhotoComment.AlbumPhotoId = data.AlbumPhotoId;
                        oldAlbumPhotoComment.Comment = data.Comment;

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
                errorLog.EventName = "SaveAppointment";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAlbumPhotoComment(long albumPhotoId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetAlbumPhotoComment_Result> albumPhotoComment = new List<GetAlbumPhotoComment_Result>();
                albumPhotoComment = db.GetAlbumPhotoComment(albumPhotoId).ToList();
                if (albumPhotoComment != null)
                {
                    resultData.Message = "Datat get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = albumPhotoComment;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<AlbumPhotoComment>();
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
                errorLog.EventName = "GetStudioInviteMessage";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData DeleteAlbumPhotoComment(long id)
        {
            ResultData resultData = new ResultData();
            try
            {
                AlbumPhotoComment albumPhotoComment = db.AlbumPhotoComments.Where(t => t.Id == id).FirstOrDefault();
                if (albumPhotoComment != null)
                {
                    db.AlbumPhotoComments.Remove(albumPhotoComment);
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
                errorLog.EventName = "DeleteCustomerWithChild";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetBackgroundMusic()
        {
            ResultData resultData = new ResultData();
            try
            {
                List<SlideShowSound> slideShowSound = new List<SlideShowSound>();
                slideShowSound = db.SlideShowSounds.ToList();
                if (slideShowSound != null)
                {
                    resultData.Message = "Datat get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = slideShowSound;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<SlideShowSound>();
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
                errorLog.EventName = "GetStudioInviteMessage";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetStudioAdvertisement(long studioId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<GetStudioAdvertisement_Result> studioAdvertisement = new List<GetStudioAdvertisement_Result>();
                studioAdvertisement = db.GetStudioAdvertisement(studioId).ToList();
                if (studioAdvertisement != null)
                {
                    resultData.Message = "Datat get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = studioAdvertisement;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<GetStudioAdvertisement_Result>();
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
                errorLog.EventName = "GetStudioInviteMessage";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData GetAdvertisementDetail(long advertisementId)
        {
            ResultData resultData = new ResultData();
            try
            {
                List<AdvertisementData> AdvertisementDataList = new List<AdvertisementData>();

                List<Advertisement> advertisementList = new List<Advertisement>();
                advertisementList = db.Advertisements.Where(a => a.Id == advertisementId).ToList();
                if (advertisementList.Count > 0)
                {
                    foreach (Advertisement advertisement in advertisementList)
                    {
                        AdvertisementData advertisementData = new AdvertisementData();
                        advertisementData.Advertisement = advertisement;

                        List<AdvertisementPhoto> advertisementPhotoList = new List<AdvertisementPhoto>();
                        advertisementPhotoList = db.AdvertisementPhotoes.Where(ap => ap.AdvertisementId == advertisementId).ToList();
                        if (advertisementPhotoList != null)
                        {
                            advertisementData.PhotoList = advertisementPhotoList;
                        }

                        AdvertisementDataList.Add(advertisementData);
                    }

                    resultData.Message = "Datat get Successfully !";
                    resultData.IsSuccess = true;
                    resultData.Data = AdvertisementDataList;
                    return resultData;
                }
                else
                {
                    resultData.Message = "No Data Found";
                    resultData.IsSuccess = true;
                    resultData.Data = new List<AdvertisementData>();
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
                errorLog.EventName = "GetStudioInviteMessage";
                return resultData;
            }
        }

        [HttpGet]
        public ResultData CheckDigitalCardMember(string mobileNo)
        {
            ResultData resultData = new ResultData();
            try
            {
                DataTable dt = sf.GetDataTableDigitalCard("select * from Member where Mobile = '" + mobileNo + "'");
                if (dt.Rows.Count > 0)
                {
                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = dt.Rows[0]["Id"].ToString();
                    return resultData;
                }
                else
                {
                    Customer customerList = new Customer();
                    customerList = db.Customers.Where(c => c.Mobile == mobileNo).FirstOrDefault();
                    if (customerList != null)
                    {
                        string id = "0";
                        string qryDigitalCard = "insert into Member (Name,Mobile,Email) values('" + customerList.Name + "','" + customerList.Mobile + "','" + customerList.Email + "')";
                        sf.ExecuteQueryDigitalCard(qryDigitalCard);

                        DataTable dt3 = sf.GetDataTableDigitalCard("select top 1 * from Member order by id desc");
                        if (dt3.Rows.Count > 0)
                        {
                            id = dt3.Rows[0]["Id"].ToString();
                        }
                        resultData.Message = "Data Get Successfully";
                        resultData.IsSuccess = true;
                        resultData.Data = customerList;
                        return resultData;
                    }
                    else
                    {
                        resultData.Message = "No Data Found";
                        resultData.IsSuccess = true;
                        resultData.Data = new List<Customer>();
                        return resultData;
                    }
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
                errorLog.EventName = "CheckDigitalCardMember";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData UpdateCustomerData(long customerId, string name, string mobile, string email)
        {
            ResultData resultData = new ResultData();
            try
            {
                db.UpdateCustomerData(customerId, name, mobile, email);
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

    }
}
