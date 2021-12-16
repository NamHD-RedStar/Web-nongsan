using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ShopNongsan.Areas.Admin.Convert_image
{
    public class ImageToBase64
    {
        public static string Convert_base64(string path_img)
        {
            string Pic_Path = Path.Combine(HttpRuntime.AppDomainAppPath, path_img);
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(Pic_Path))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    string base64String;
                    image.Save(ms, image.RawFormat);
                    byte[] imageBytes = ms.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
    }
}