using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Drawing.Imaging;

namespace InsightAcademy.Helper
{
    public class TeacherHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeacherHelper( IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string ConvertAndSave(byte[] byteArray)
        {
            string fileName = "Profileimage";
            if (byteArray == null || byteArray.Length == 0)
            {
                return "";
            }

            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UserFile");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string filePath = Path.Combine(uploadFolder, fileName);

            // Determine the file format based on the file extension
            ImageFormat imageFormat;
            string extension = ".png"; // Default to PNG if extension is not recognized
            if (Path.GetExtension(filePath).Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                Path.GetExtension(filePath).Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                imageFormat = ImageFormat.Jpeg;
                extension = ".jpg";
            }
            else if (Path.GetExtension(filePath).Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                imageFormat = ImageFormat.Png;
            }
            else
            {
                // Unsupported format, default to PNG
                imageFormat = ImageFormat.Png;
            }

            // Save the image with the determined format
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                using (Image image = Image.FromStream(ms))
                {
                    filePath += extension; // Append the determined extension to the file path
                    image.Save(filePath, imageFormat);
                }
            }

            return filePath;
        }
    }
}
