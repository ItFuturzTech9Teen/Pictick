using PicTick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PicTick.Controllers
{
    public class AccountAPIController : ApiController
    {
        PicTickEntities db = new PicTickEntities();
        StudyField sf = new StudyField();

        [HttpGet]
        public ResultData ValidateAdmin(string userName, string password)
        {
            ResultData resultData = new ResultData();
            try
            {
                Admin admin = db.Admins.Where(a => a.UserName == userName && a.Password == password).FirstOrDefault();
                if (admin != null)
                {
                    User user = new User();
                    user.Id = admin.Id;
                    user.Name = admin.Name;
                    user.UserName = admin.UserName;
                    user.Password = admin.Password;
                    user.Role = "Admin";
                    user.StudioId = 0;

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = admin;
                    return resultData;
                }
                else
                {
                    resultData.Message = "Invalid Login Detail";
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
                errorLog.EventName = "GetNotice";

                return resultData;
            }
        }

        [HttpGet]
        public ResultData StudioLogin(string username, string password)
        {
            ResultData resultData = new ResultData();
            try
            {
                Studio studio = db.Studios.Where(a => a.UserName == username && a.Password == password).FirstOrDefault();
                if (studio != null)
                {
                    User user = new User();
                    user.Id = studio.Id;
                    user.Name = studio.Name;
                    user.UserName = studio.UserName;
                    user.Password = studio.Password;
                    user.Role = "Studio";
                    user.StudioId = studio.Id;

                    resultData.Message = "Data Get Successfully";
                    resultData.IsSuccess = true;
                    resultData.Data = user;
                    return resultData;
                }
                else
                {
                    resultData.Message = "Invalid Login Detail";
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
                errorLog.EventName = "GetNotice";

                return resultData;
            }
        }
    }
}
