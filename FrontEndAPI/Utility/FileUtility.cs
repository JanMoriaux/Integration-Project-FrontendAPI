using FrontEndAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndAPI.Utility
{
    //Credited to Ben Vanhoutvinck 
    //Modified copy from http://dt4.ehb.be/Bonobo.Git.Server/Repository/DijleZonenApi/develop/Blob/DijleZonenApi/utilities/FileUtility.cs
    public class FileUtility
    {
        public static async Task ProcessFile(List<Microsoft.AspNetCore.Http.IFormFile> files, Event e)
        {
            //------- File upload -------

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            // string filePath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            //string filePath = "C:\\" + Guid.NewGuid().ToString() + ".png";
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        stream.Position = 0;
                        await formFile.CopyToAsync(stream);
                    }
                    // tmp file has been written to disk
                    // get byte[]
                    byte[] fileByteArray = File.ReadAllBytes(filePath);

                    // check if file is valid image
                    ImageFormat format = GetImageFormat(fileByteArray);

                    if (format != ImageFormat.unknown)
                    {
                        // name and transfer the file to permanent location and set imageUrl
                        string permFilePath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + "." + format;
                        File.WriteAllBytes(permFilePath, fileByteArray);
                        e.ImageURL = permFilePath;
                    }
                    else
                    {
                        e.ImageURL = "";
                    }
                }
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }
    }
}

