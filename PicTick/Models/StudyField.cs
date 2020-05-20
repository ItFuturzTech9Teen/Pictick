using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PicTick.Models
{
    public class StudyField
    {
        SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DB_Connect"].ConnectionString);
        SqlConnection connDigitalCard = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DigitalCardConnection"].ConnectionString);

        public void ExecuteQuery(string qry)
        {
            cnn.Open();
            SqlCommand cmd = new SqlCommand(qry, cnn);
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public void ExecuteQueryWithRollBack(string qry, SqlTransaction transaction)
        {
            cnn.Open();
            SqlCommand cmd = new SqlCommand(qry, cnn);
            cmd.Transaction = transaction;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public int GetIntFromExecuteQuery(string qry)
        {
            int id = 0;
            cnn.Open();
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
                rd.Read(); // read first row
                id = rd.GetInt32(0);
            }
            cnn.Close();
            return id;
        }

        public string ExecuteQueryBySP(SqlCommand scmd)
        {
            cnn.Open();
            SqlCommand cmd = scmd;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            string id = cmd.Parameters["@id"].Value.ToString();
            cnn.Close();
            return id;
        }

        public string ExecuteQueryGetIdBySP(SqlCommand scmd)
        {
            cnn.Open();
            SqlCommand cmd = scmd;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            string id = cmd.Parameters["@id"].Value.ToString();
            cnn.Close();
            return id;
        }

        public void ExecuteUpdateQueryBySP(SqlCommand scmd)
        {
            cnn.Open();
            SqlCommand cmd = scmd;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public DataTable GetData(string qry)
        {
            cnn.Close();
            cnn.Open();
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
            cnn.Close();
            return dt;
        }

        public DataTable GetDataBySP(SqlCommand scmd)
        {
            cnn.Open();
            DataTable dt = new DataTable();

            SqlCommand cmd = scmd;
            cmd.CommandTimeout = 240;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;

            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            using (SqlDataReader myReader = cmd.ExecuteReader())
            {
                dt.Load(myReader);
            }


            //da.Fill(dt);
            cnn.Close();
            return dt;
        }

        public void ExecuteQueryDigitalCard(string SqlQuery)
        {
            try
            {
                SqlCommand cm = new SqlCommand(SqlQuery, connDigitalCard);
                connDigitalCard.Open();
                cm.ExecuteNonQuery();
                connDigitalCard.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }

        public DataTable GetDataTableDigitalCard(string strqry)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cm = new SqlCommand(strqry, connDigitalCard);
                connDigitalCard.Open();
                cm.ExecuteNonQuery();
                connDigitalCard.Close();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            return dt;
        }

        public DataTable GetDataBySPTemp(SqlCommand scmd)
        {
            cnn.Open();
            DataTable dt = new DataTable();

            SqlCommand cmd = scmd;
            cmd.CommandTimeout = 240;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
            cnn.Close();
            return dt;
        }
        // Created By : Ketan Patel
        public DataTable GetDataWithPaging(string cols, string tableName, string orderBy, int pageSize, int pageNumber)
        {

            cnn.Open();
            DataTable dt = new DataTable();
            string qry = String.Format("select {0} from {1} ORDER BY {2} OFFSET {3} * ({4} - 1) ROWS FETCH NEXT {5} ROWS ONLY; ",
                cols, tableName, orderBy, pageSize, pageNumber, pageSize);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cnn.Close();
            return dt;

        }

        public DataTable GetDataWithPagingDesc(string cols, string tableName, string orderBy, int pageSize, int pageNumber)
        {

            cnn.Open();
            DataTable dt = new DataTable();
            string qry = String.Format("select {0} from {1} ORDER BY {2} desc OFFSET {3} * ({4} - 1) ROWS FETCH NEXT {5} ROWS ONLY; ",
                cols, tableName, orderBy, pageSize, pageNumber, pageSize);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cnn.Close();
            return dt;

        }

        public DataTable GetDataWithPaging(string cols, string tableName, string orderBy, int pageSize, int pageNumber, string search)
        {

            cnn.Open();
            DataTable dt = new DataTable();
            string qry = String.Format("select {0} from {1} ORDER BY {2} OFFSET {3} * ({4} - 1) ROWS FETCH NEXT {5} ROWS ONLY; ",
                cols, tableName, orderBy, pageSize, pageNumber, pageSize, search);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cnn.Close();
            return dt;

        }
        // Created By : Ketan Patel
        public string GetScalerValue(string qry)
        {
            string value = "";
            cnn.Open();
            SqlCommand cmd = new SqlCommand(qry, cnn);
            value = Convert.ToString(cmd.ExecuteScalar());
            cnn.Close();
            return value;
        }

        public string GetUniqueCode(string tableName, string codeField, string idField, string fixCode)
        {
            cnn.Open();
            DataTable dt = new DataTable();

            string qry = string.Format("select top 1 {0} from {1} order by {2} desc", codeField, tableName, idField);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
            cnn.Close();

            if (dt.Rows.Count > 0)
            {
                long code = Convert.ToInt32(dt.Rows[0][codeField]);
                code++;
                return code.ToString();
            }
            else
                return fixCode;
        }

        // this is for get specific code for each and every table code :   Kapil 30-12-17
        public string GetDataUniqueCode(string tableName, string codeField, string idField, string fixCode)
        {
            cnn.Open();
            DataTable dt = new DataTable();

            string qry = string.Format("select top 1 {0} from {1} order by {2} desc", codeField, tableName, idField);
            SqlCommand cmd = new SqlCommand(qry, cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
            cnn.Close();

            if (dt.Rows.Count > 0)
            {
                string output = Convert.ToString(dt.Rows[0][codeField]);
                string[] data = output.Split('-');
                long code = Convert.ToInt64(data[2]);
                code++;
                return code.ToString();
            }
            else
                return fixCode;
        }

        //public long GetLastRecordID(string TableName,string PKFieldName)
        //{
        //    string qry = "select top 1 " + PKFieldName + " from " + TableName + " order by " + PKFieldName + " desc";
        //    DataTable dt = new DataTable();
        //    dt = GetData(qry);

        //    return 1;
        //}

        //[HttpPost]
        //public string[] Upload()
        //{
        //    string path = Server.MapPath("~/Uploads/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    foreach (string key in Request.Files)
        //    {
        //        HttpPostedFileBase postedFile = Request.Files[key];
        //        postedFile.SaveAs(path + postedFile.FileName);
        //    }

        //    return Content("Success");
        //}

        public string[] Upload(HttpRequestBase requestObject, string fileType, string FolderName)
        {
            string[] filepaths = null;
            if (requestObject.Files.AllKeys.Any())
            {
                filepaths = new string[requestObject.Files.Count];
                int i = 0;
                foreach (string file in requestObject.Files)
                {
                    var httpPostedFile = requestObject.Files[file];
                    if (httpPostedFile != null)
                    {
                        string FileName = Path.GetFileName(httpPostedFile.FileName);
                        string Extension = Path.GetExtension(FileName).ToLower();
                        string NewFileName = String.Format("{0}{1}", Guid.NewGuid(), Extension);
                        string FilePath = "";

                        if (fileType == Constants.FileTypeAlbumPhotos)
                        {
                            string folderPath = "~/UploadImages/Album/" + FolderName;
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string folderPathThumb = "~/UploadImages/AlbumThumb/" + FolderName;
                            bool thumbExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPathThumb));
                            if (!thumbExists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPathThumb));

                            if (Extension.Contains("jpg") || Extension.Contains("png") || Extension.Contains("jpeg"))
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                string thumbPath = Path.Combine(HttpContext.Current.Server.MapPath(folderPathThumb), FileName);

                                var filePath1 = ResizeImage(httpPostedFile, thumbPath);
                                httpPostedFile.SaveAs(path);

                                FilePath = FolderName + "/" + FileName;
                            }

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeVideo)
                        {
                            string folderPath = "~/UploadVideo/Album/" + FolderName;
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                            httpPostedFile.SaveAs(path);
                            FilePath = "UploadVideo/Album/" + FolderName + "/" + FileName;

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeGalleryCover)
                        {
                            string folderPath = "~/UploadImages/GalleryCover";
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                            httpPostedFile.SaveAs(path);
                            FilePath = "UploadImages/GalleryCover/" + FileName;

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeAlbumCover)
                        {
                            string folderPath = "~/UploadImages/AlbumCover";
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                            httpPostedFile.SaveAs(path);
                            FilePath = "UploadImages/AlbumCover/" + FileName;

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeCategoryPhotos)
                        {
                            string folderPath = "~/UploadImages/Category";
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                            httpPostedFile.SaveAs(path);
                            FilePath = "UploadImages/Category/" + FileName;

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypePortfolioPhotos)
                        {
                            string folderPath = "~/UploadImages/Portfolio/" + FolderName;
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string folderPathThumb = "~/UploadImages/PortfolioThumb/" + FolderName;
                            bool thumbExists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPathThumb));
                            if (!thumbExists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPathThumb));

                            if (Extension.Contains("jpg") || Extension.Contains("png") || Extension.Contains("jpeg"))
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                string thumbPath = Path.Combine(HttpContext.Current.Server.MapPath(folderPathThumb), FileName);
                                var filePath1 = ResizeImage(httpPostedFile, thumbPath);
                                httpPostedFile.SaveAs(path);
                                FilePath = FolderName + "/" + FileName;
                            }

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeOrganizationPhotos)
                        {
                            string folderPath = "~/UploadImages/Studio/" + FolderName;
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            if (Extension.Contains("jpg") || Extension.Contains("png") || Extension.Contains("jpeg"))
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/Studio/" + FolderName + "/" + FileName;
                            }
                            else
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/Studio/" + FolderName + "/" + FileName;
                            }

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeStudioSocialLinkPhotos)
                        {
                            string folderPath = "~/UploadImages/StudioSocialLink/" + FolderName;
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            if (Extension.Contains("jpg") || Extension.Contains("png") || Extension.Contains("jpeg"))
                            {
                                //ResizeStream(500, 500, httpPostedFile.InputStream, Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName));
                                //FilePath = "UploadImages/Album/" + FolderName + "/" + FileName;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/StudioSocialLink/" + FolderName + "/" + FileName;
                            }
                            else
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/StudioSocialLink/" + FolderName + "/" + FileName;
                            }

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeWaterMark)
                        {
                            string folderPath = "~/UploadImages/WaterMark";
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                            httpPostedFile.SaveAs(path);
                            FilePath = "UploadImages/WaterMark/" + FileName;

                            filepaths[i] = FilePath;
                            i++;

                            //string folderPath = "~/UploadImages/WaterMark";
                            //bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            //if (!exists)
                            //    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            //string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);

                            //FilePath = "UploadImages/WaterMark/" + FileName;

                            ////watermarkImage(path, "This is a watermark", path1, ImageFormat.Jpeg);
                            //watermarkImage(httpPostedFile.InputStream, "This is a watermark", path, ImageFormat.Jpeg, "TextWatermark");

                            //filepaths[i] = FilePath;
                            //i++;
                        }
                        else if (fileType == Constants.FileTypePhotographer)
                        {
                            string folderPath = "~/UploadImages/Photographer/";
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            if (Extension.Contains("jpg") || Extension.Contains("png") || Extension.Contains("jpeg"))
                            {
                                //ResizeStream(500, 500, httpPostedFile.InputStream, Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName));
                                //FilePath = "UploadImages/Album/" + FolderName + "/" + FileName;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/Photographer/" + FileName;
                            }
                            else
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/Photographer/" + FileName;
                            }

                            filepaths[i] = FilePath;
                            i++;
                        }
                        else if (fileType == Constants.FileTypeCustomer)
                        {
                            string folderPath = "~/UploadImages/Customer/";
                            bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(folderPath));
                            if (!exists)
                                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(folderPath));

                            if (Extension.Contains("jpg") || Extension.Contains("png") || Extension.Contains("jpeg"))
                            {
                                //ResizeStream(500, 500, httpPostedFile.InputStream, Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName));
                                //FilePath = "UploadImages/Album/" + FolderName + "/" + FileName;
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/Customer/" + FileName;
                            }
                            else
                            {
                                string path = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), FileName);
                                httpPostedFile.SaveAs(path);
                                FilePath = "UploadImages/Customer/" + FileName;
                            }

                            filepaths[i] = FilePath;
                            i++;
                        }
                    }
                }
            }
            return filepaths;
        }

        //public void watermarkImage(string sourceImage, string text, string targetImage, ImageFormat fmt)
        //{
        //    try
        //    {
        //        // open source image as stream and create a memorystream for output
        //        FileStream source = new FileStream(sourceImage, FileMode.Open);
        //        Stream output = new MemoryStream();
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(source);

        //        // choose font for text
        //        Font font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);

        //        int opac = 0;

        //        string sOpacity = "75%";
        //        // Determine the opacity of the watermark

        //        switch (sOpacity)
        //        {
        //            case "100%":
        //                opac = 255; // 1 * 255
        //                break;
        //            case "75%":
        //                opac = 191; // .75 * 255
        //                break;
        //            case "50%":
        //                opac = 127; // .5 * 255
        //                break;
        //            case "25%":
        //                opac = 64;  // .25 * 255
        //                break;
        //            case "10%":
        //                opac = 25;  // .10 * 255
        //                break;
        //            default:
        //                opac = 127; // default at 50%; .5 * 255
        //                break;
        //        }

        //        string type = "TextWatermark";
        //        if (type == "TextWatermark")
        //        {

        //            //choose color and transparency
        //            Color color = Color.FromArgb(opac, 41, 57, 85);   // Opacity, colorvalue

        //            //location of the watermark text in the parent image
        //            //Point pt = new Point(10, 5);

        //            SolidBrush brush = new SolidBrush(color);

        //            int imageWidth = img.Width;
        //            int imageHeight = img.Height;

        //            // Get a graphics context
        //            Graphics graphics = Graphics.FromImage(img);

        //            // Calculate the size of the text
        //            SizeF sz = graphics.MeasureString(text, font);

        //            //Position
        //            Point pt = GetWatermarkPosition("TopMiddle", img, sz);

        //            //draw text on image                
        //            graphics.DrawString(text, font, brush, pt);
        //            graphics.Dispose();

        //            //update image memorystream
        //            img.Save(output, fmt);
        //            System.Drawing.Image imgFinal = System.Drawing.Image.FromStream(output);

        //            //write modified image to file
        //            Bitmap bmp = new System.Drawing.Bitmap(img.Width, img.Height, img.PixelFormat);
        //            Graphics graphics2 = Graphics.FromImage(bmp);
        //            graphics2.DrawImage(imgFinal, new Point(0, 0));
        //            bmp.Save(targetImage, fmt);

        //            imgFinal.Dispose();
        //            img.Dispose();
        //        }
        //        else
        //        {
        //            using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(@"E:\Kapil R Singh\Dot Net Project\InstaAlbum\InstaAlbum\InstaAlbum\Img\Logo.png"))

        //            using (Graphics imageGraphics = Graphics.FromImage(img))
        //            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
        //            {
        //                // Calculate the size of the text
        //                SizeF sz = new SizeF(width: watermarkImage.Width, height: watermarkImage.Height);
        //                //Position
        //                Point pt = GetWatermarkPosition("TopMiddle", img, sz);

        //                watermarkBrush.TranslateTransform(pt.X, pt.Y);
        //                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(pt, new Size(watermarkImage.Width + 1, watermarkImage.Height)));
        //                img.Save(targetImage);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //    }
        //}

        public void watermarkImage(Stream sourceStream, string text, string targetImage, ImageFormat fmt, string type)
        {
            try
            {

                // open source image as stream and create a memorystream for output
                //FileStream source = new FileStream(sourceImage, FileMode.Open);
                Stream output = new MemoryStream();
                System.Drawing.Image img = System.Drawing.Image.FromStream(sourceStream);

                // choose font for text
                Font font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);

                int opac = 0;

                string sOpacity = "75%";
                // Determine the opacity of the watermark

                switch (sOpacity)
                {
                    case "100%":
                        opac = 255; // 1 * 255
                        break;
                    case "75%":
                        opac = 191; // .75 * 255
                        break;
                    case "50%":
                        opac = 127; // .5 * 255
                        break;
                    case "25%":
                        opac = 64;  // .25 * 255
                        break;
                    case "10%":
                        opac = 25;  // .10 * 255
                        break;
                    default:
                        opac = 127; // default at 50%; .5 * 255
                        break;
                }

                if (type == "TextWatermark")
                {
                    //choose color and transparency
                    Color color = Color.FromArgb(opac, 41, 57, 85);   // Opacity, colorvalue

                    //location of the watermark text in the parent image
                    //Point pt = new Point(10, 5);

                    SolidBrush brush = new SolidBrush(color);

                    int imageWidth = img.Width;
                    int imageHeight = img.Height;

                    // Get a graphics context
                    Graphics graphics = Graphics.FromImage(img);

                    // Calculate the size of the text
                    SizeF sz = graphics.MeasureString(text, font);

                    //Position
                    Point pt = GetWatermarkPosition("TopMiddle", img, sz);

                    //draw text on image                
                    graphics.DrawString(text, font, brush, pt);
                    graphics.Dispose();

                    //update image memorystream
                    img.Save(output, fmt);
                    System.Drawing.Image imgFinal = System.Drawing.Image.FromStream(output);

                    //write modified image to file
                    Bitmap bmp = new System.Drawing.Bitmap(img.Width, img.Height, img.PixelFormat);
                    Graphics graphics2 = Graphics.FromImage(bmp);
                    graphics2.DrawImage(imgFinal, new Point(0, 0));
                    bmp.Save(targetImage, fmt);

                    imgFinal.Dispose();
                    img.Dispose();
                }
                else
                {
                    using (System.Drawing.Image watermarkImage = System.Drawing.Image.FromFile(@"E:\Kapil R Singh\Dot Net Project\InstaAlbum\InstaAlbum\InstaAlbum\Img\Logo.png"))

                    using (Graphics imageGraphics = Graphics.FromImage(img))
                    using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                    {
                        // Calculate the size of the text
                        SizeF sz = new SizeF(width: watermarkImage.Width, height: watermarkImage.Height);
                        //Position
                        Point pt = GetWatermarkPosition("TopMiddle", img, sz);

                        watermarkBrush.TranslateTransform(pt.X, pt.Y);
                        imageGraphics.FillRectangle(watermarkBrush, new Rectangle(pt, new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                        img.Save(targetImage);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private Point GetWatermarkPosition(string m_position, System.Drawing.Image m_image, SizeF m_watermark)
        {
            int x = 0;
            int y = 0;

            switch (m_position)
            {
                case "Absolute":
                    x = 10; y = 5;
                    break;
                case "TopLeft":
                    x = 0; y = 0;
                    break;
                case "TopRight":
                    x = Convert.ToInt32(m_image.Width - m_watermark.Width); y = 0;
                    break;
                case "TopMiddle":
                    x = Convert.ToInt32(m_image.Width - m_watermark.Width) / 2; y = 0;
                    break;
                case "BottomLeft":
                    x = 0; y = Convert.ToInt32(m_image.Height - m_watermark.Height);
                    break;
                case "BottomRight":
                    x = Convert.ToInt32(m_image.Width - m_watermark.Width);
                    y = Convert.ToInt32(m_image.Height - m_watermark.Height);
                    break;
                case "BottomMiddle":
                    x = Convert.ToInt32((m_image.Width - m_watermark.Width)) / 2;
                    y = Convert.ToInt32(m_image.Height - m_watermark.Height);
                    break;
                case "MiddleLeft":
                    x = 0; y = Convert.ToInt32((m_image.Height - m_watermark.Height)) / 2;
                    break;
                case "MiddleRight":
                    x = Convert.ToInt32(m_image.Width - m_watermark.Width);
                    y = Convert.ToInt32((m_image.Height - m_watermark.Height)) / 2;
                    break;
                case "Center":
                    x = Convert.ToInt32((m_image.Width - m_watermark.Width)) / 2;
                    y = Convert.ToInt32((m_image.Height - m_watermark.Height)) / 2;
                    break;
                default:
                    break;
            }

            return new Point(x, y);
        }

        public string ResizeImage(HttpPostedFileBase fileToUpload, string path)
        {
            string name = Path.GetFileNameWithoutExtension(fileToUpload.FileName);
            var ext = Path.GetExtension(fileToUpload.FileName);
            string myfile = name + ext;

            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(fileToUpload.InputStream, true, false))
                {
                    //var path = Path.Combine(HttpContext.Current.Server.MapPath("~/resizeImageStore"), myfile);
                    try
                    {
                        //Size can be change according to your requirement 
                        float thumbWidth = 360F;
                        float thumbHeight = 270F;
                        //calculate  image  size
                        if (image.Width > image.Height)
                        {
                            thumbHeight = ((float)image.Height / image.Width) * thumbWidth;
                        }
                        else
                        {
                            thumbWidth = ((float)image.Width / image.Height) * thumbHeight;
                        }

                        int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                        int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                        var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                        var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                        thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                        thumbnailGraph.DrawImage(image, imageRectangle);
                        var ms = new MemoryStream();
                        thumbnailBitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms.Position = 0;
                        GC.Collect();
                        thumbnailGraph.Dispose();
                        thumbnailBitmap.Dispose();
                        image.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myfile;
        }

        public string[] UploadBase64(string file, string fileType)
        {
            string[] filepaths = new string[1];
            if (!string.IsNullOrEmpty(file))
            {
                file = file.Replace(" ", "");
                byte[] imageBytes = Convert.FromBase64String(file);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                string NewFileName = String.Format("{0}{1}", Guid.NewGuid(), ".jpg");
                if (fileType == Constants.FileTypeUser)
                {
                    ResizeBase64(600, 800, image, Path.Combine(HttpContext.Current.Server.MapPath("~/UploadImages/Power/"), NewFileName));
                    filepaths[0] = NewFileName;
                }
            }

            return filepaths;
        }

        public static void ResizeBase64(int imageWidth, int imageHeight, System.Drawing.Image img, string outputPath)
        {
            var image = img;
            int thumbnailHeightSize = imageHeight;
            int thumbnailWidthSize = imageWidth;
            int newWidth, newHeight;
            if (image.Width > image.Height)
            {
                newHeight = thumbnailHeightSize;                    // New Height
                double HeightRatio = Convert.ToDouble(image.Height) / Convert.ToDouble(thumbnailHeightSize);
                var width = image.Width / HeightRatio;
                newWidth = Convert.ToInt16(width);                  // New Width
            }
            else
            {
                double WidthRatio = Convert.ToDouble(image.Width) / Convert.ToDouble(thumbnailWidthSize);
                var height = image.Height / WidthRatio;
                newHeight = Convert.ToInt16(height);                // New Height
                newWidth = thumbnailWidthSize;                      // New Height
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);
            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            thumbnailBitmap.Save(outputPath, image.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }

        public static void ResizeStream(int imageWidth, int imageHeight, Stream filePath, string outputPath)
        {
            var image = System.Drawing.Image.FromStream(filePath);
            int thumbnailHeightSize = imageHeight;
            int thumbnailWidthSize = imageWidth;
            int newWidth, newHeight;
            if (image.Width > image.Height)
            {
                newHeight = thumbnailHeightSize;                    // New Height
                double HeightRatio = Convert.ToDouble(image.Height) / Convert.ToDouble(thumbnailHeightSize);
                var width = image.Width / HeightRatio;
                newWidth = Convert.ToInt16(width);                  // New Width
            }
            else
            {
                double WidthRatio = Convert.ToDouble(image.Width) / Convert.ToDouble(thumbnailWidthSize);
                var height = image.Height / WidthRatio;
                newHeight = Convert.ToInt16(height);                // New Height
                newWidth = thumbnailWidthSize;                      // New Height
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);
            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            thumbnailBitmap.Save(outputPath, image.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }

        //public ResultData GetErrorLog()
        //{
        //    ResultData resultData = new ResultData();
        //    try
        //    {
        //        List<ErrorLog> list = new List<ErrorLog>();
        //        DataTable dt = new DataTable();
        //        string qry = "select * from ErrorLog";
        //        dt = GetData(qry);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                ErrorLog errorLog = new ErrorLog()
        //                {
        //                    LogId = Convert.ToInt64(dr["LogId"]),
        //                    ErrorMessage = dr["ErrorMessage"].ToString(),
        //                    StackTrace = dr["StackTrace"].ToString(),
        //                    EventName = dr["EventName"].ToString(),
        //                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"])
        //                };
        //                list.Add(errorLog);
        //            }
        //            resultData.Data = list;
        //            resultData.IsSuccess = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultData.IsSuccess = false;
        //        resultData.Message = ex.Message.ToString();
        //    }
        //    return resultData;
        //}

        public ResultData SaveErrorLog(JObject data)
        {
            ResultData resultData = new ResultData();
            try
            {
                dynamic subjectData = data;

                string qry = string.Format("insert into ErrorLog values('{0}','{1}','{2}','{3}')", subjectData.ErrorMessage, subjectData.StackTrace, subjectData.EventName, DateTime.Now);
                ExecuteQuery(qry);

                resultData.Data = 1;
                resultData.IsSuccess = true;
            }
            catch (Exception ex)
            {
                resultData.IsSuccess = false;
                resultData.Message = ex.Message.ToString();
            }
            return resultData;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public int GetStockTransferID()
        {
            int id = 1000;

            DataTable dt = new DataTable();
            try
            {
                SqlCommand cm = new SqlCommand("select tid from StockTransfer order by tid desc", cnn);
                cnn.Open();
                cm.ExecuteNonQuery();
                cnn.Close();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    id = 1;
                    id = id + Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                //billid = "BILL" + id.ToString();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            return id;
        }

        public string GetNextSequanceNoOld(string branchid, string type)
        {
            int id = 0;
            string orderid = "";
            string qry = "";

            if (type == "O")
            {
                qry = "select Count(*) from SalesOrder where branchid = " + branchid;
                DataTable dt2 = GetData("select ordseqno from Branch where branchid = " + branchid);
                if (dt2.Rows[0][0].ToString() != "")
                {
                    id = Convert.ToInt32(dt2.Rows[0][0].ToString());
                }
                else
                {
                    return "Error : Please Enter Order Sequance No in Branch!!";
                }
            }
            else if (type == "R")
            {
                qry = "select Count(*) from SalesOrderPayment";
                DataTable dt2 = GetData("select recseqno from Branch where branchid = " + branchid);
                if (dt2.Rows[0][0].ToString() != "")
                {
                    id = Convert.ToInt32(dt2.Rows[0][0].ToString());
                }
                else
                {
                    return "Error : Please Enter Receipt Sequance No in Branch!!";
                }
            }
            else if (type == "V")
            {
                qry = "select Count(*) from SalesOrder";
            }
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cm = new SqlCommand(qry, cnn);
                cnn.Open();
                cm.ExecuteNonQuery();
                cnn.Close();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    id = id + Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                orderid = type + id.ToString();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            return orderid;
        }

        public string GetNextSequanceNo(string branchid, string type)
        {
            int id = 0;
            string orderid = "";
            string qry = "";

            if (type == "O")
            {
                DataTable dt2 = GetData("select cast(year as varchar) + cast(ordseqno as varchar) as seqno from Branch where branchid = " + branchid);
                if (dt2.Rows[0][0].ToString() != "")
                {
                    try
                    {
                        id = Convert.ToInt32(dt2.Rows[0][0].ToString());
                    }
                    catch
                    {
                        return "Error : Order Sequance No Should be Enter Only Numbers in Branch!!";
                    }

                    int seqno = id - 1;
                    qry = "select Count(*) from SalesOrder where branchid = " + branchid + " and right(seqno, len(seqno)-1) > " + seqno;
                }
                else
                {
                    return "Error : Please Enter Order Sequance No in Branch!!";
                }
            }
            else if (type == "R")
            {
                qry = "select Count(*) from SalesOrderPayment";
                DataTable dt2 = GetData("select cast(year as varchar) + cast(recseqno as varchar) as seqno from Branch where branchid = " + branchid);
                if (dt2.Rows[0][0].ToString() != "")
                {
                    id = Convert.ToInt32(dt2.Rows[0][0].ToString());
                }
                else
                {
                    return "Error : Please Enter Receipt Sequance No in Branch!!";
                }
            }
            else if (type == "V")
            {
                qry = "select Count(*) from SalesOrder";
            }
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cm = new SqlCommand(qry, cnn);
                cnn.Open();
                cm.ExecuteNonQuery();
                cnn.Close();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    id = id + Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                orderid = type + id.ToString();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            return orderid;
        }

        public string DecodeSpecialCharForAPI(string str)
        {
            str = str.Replace("spcplus", "+");
            str = str.Replace("spcand", "&");
            str = str.Replace("spcimark", "!");
            str = str.Replace("spcatsign", "@");
            str = str.Replace("spchash", "#");
            str = str.Replace("spcdollar", "$");
            str = str.Replace("spcper", "%");
            str = str.Replace("spccaret", "^");
            str = str.Replace("spcstar", "*");
            str = str.Replace("spcrbs", "(");
            str = str.Replace("spcrbe", ")");
            str = str.Replace("spcunders", "_");
            str = str.Replace("spcpipe", "|");
            return str;
        }

        public string GetDateFormatData(string Date, string Type)
        {

            DateTime dtstarttime = new DateTime();
            DateTime dtEndtime = new DateTime();
            if (Date != "undefined" && Date != null && Date != "")
            {
                string[] s = Date.Split('-');
                string startDates = s[0].Trim();
                string EndDates = s[1].Trim();

                dtstarttime = DateTime.ParseExact(startDates, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dtEndtime = DateTime.ParseExact(EndDates, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
            }
            else
            {
                dtEndtime = DateTime.Now.AddDays(1);
                dtstarttime = DateTime.Now.AddMonths(-1);
            }
            string sdate = dtstarttime.ToString("yyyy-MM-dd");
            string edate = dtEndtime.ToString("yyyy-MM-dd");
            if (Type == "S")
            {
                return sdate;
            }
            else
            {
                return edate;
            }

        }

        public string GetBarcode(string category)
        {
            string barcode = "";
            string qry = "";
            int id = 1000;
            if (category == "F")
            {
                id = 10001;
                qry = "select top 1 prodgroup from product where prodgroup < 30001 and ProductType = 'F' order by prodgroup desc";
            }
            else if (category == "SG")
            {
                id = 30001;
                qry = "select top 1 prodgroup from product where prodgroup < 50001 and ProductType = 'SG' order by prodgroup desc";
            }
            else if (category == "SL")
            {
                id = 50001;
                qry = "select top 1 prodgroup from product where prodgroup < 60001 and ProductType = 'SL' order by prodgroup desc";
            }
            else if (category == "CL")
            {
                id = 60001;
                qry = "select top 1 prodgroup from product where prodgroup < 70001 and ProductType = 'CL' order by prodgroup desc";
            }
            else if (category == "CLS")
            {
                id = 70001;
                qry = "select top 1 prodgroup from product where prodgroup < 80001 and ProductType = 'CLS' order by prodgroup desc";
            }
            else if (category == "RG")
            {
                id = 80001;
                qry = "select top 1 prodgroup from product where prodgroup < 85001 and ProductType = 'RG' order by prodgroup desc";
            }
            else if (category == "A")
            {
                id = 85001;
                qry = "select top 1 prodgroup from product where prodgroup < 90001 and ProductType = 'A' order by prodgroup desc";
            }
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cm = new SqlCommand(qry, cnn);
                cnn.Open();
                cm.ExecuteNonQuery();
                cnn.Close();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    id = Convert.ToInt32(dt.Rows[0][0].ToString());
                    id++;
                }
                barcode = id.ToString();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            return barcode;
        }

        private const string key = "AIzaSyBkK8MOoVRv8pcHPQc_2DtvkcA9fXrWTVg";

        public string urlShorter(string url)
        {
            string finalURL = "";
            string post = "{\"longUrl\": \"" + url + "\"}";
            string shortUrl = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + key);
            try
            {
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");
                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            string json = responseReader.ReadToEnd();
                            finalURL = Regex.Match(json, @"""id"": ?""(?<id>.+)""").Groups["id"].Value;
                            //finalURL = Regex.Match(json, @"""id"": ?""(?.+)""").Groups["id"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // if Google's URL Shortener is down...
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return finalURL;
        }
    }
}