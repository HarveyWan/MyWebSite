using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Web;
using System.Globalization;

namespace KDMSCommon
{
     public  class ImgHelper
    {
        #region 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public static  string UploadImageFile(string fileData)
        {
            if (string.IsNullOrEmpty(fileData))
            {
                return "";
            }

            string newFileName = string.Empty;
            string newDirectory = string.Empty;
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty);


            string fileExtension = "jpg";
            newDirectory = @"\" + DateTime.Now.ToString("yyyyMMdd");
            string path = ConfigManage.ImageUploadFolder + newDirectory;
            DirectoryInfo directory = new DirectoryInfo(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string tempFile = fileData.Replace(" ", "+");
            byte[] bytes = Convert.FromBase64String(tempFile);
            newFileName = guid + "." + fileExtension;
            using (FileStream fs = new FileStream(path + "\\" + newFileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            string url = ConfigManage.ImageUrlHead + newDirectory.Replace("\\", "/") + "/" + newFileName;
            return url;
        }
        #endregion
        private string FilePhysicalPath = string.Empty;
        private string FileRelativePath = string.Empty;
        public UploadImgResult UploadImage(HttpPostedFileBase file)
        {
            UploadImgResult result = new UploadImgResult();
            try
            {
                string fileNameExt = (new FileInfo(file.FileName)).Extension;

                //string saveName = GetImageName() + fileNameExt;

                //获得要保存的文件路径
                String newFileName = NewFileName(fileNameExt);
                String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                FilePhysicalPath += ymd + "/";
                FileRelativePath += ymd + "/";
                String fileFullPath = FilePhysicalPath + newFileName;
                LogHelper.LogMsg(fileFullPath);
                if (!Directory.Exists(FilePhysicalPath))
                    Directory.CreateDirectory(FilePhysicalPath);
                file.SaveAs(fileFullPath);

                string relativeFileFullPath = FileRelativePath + newFileName;
                result.Result = 1;
                result.msg = relativeFileFullPath;
            }
            catch (Exception ex)
            {
                result.Result = 3;
                result.msg = ex.Message;
            }
            return result;
        }

        public class UploadImgResult
        {
            public int Result { set; get; } //1成功，2失败，3异常
            public string msg { set; get; }
        }
        private string NewFileName(string fileNameExt)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileNameExt;
            //return Guid.NewGuid().ToString() + fileNameExt;
        }

    }
}
