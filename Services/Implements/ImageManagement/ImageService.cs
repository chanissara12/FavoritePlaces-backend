using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Interfaces.ImageManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Services.Implements.ImageManagement
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        //เปิดใช้งาน Cloudinary โดยใช้ค่าการตั้งค่าจาก appsettings.json
        public ImageService(IConfiguration config)
        {
            var cloudName = config["CloudinarySettings:CloudName"];
            var apiKey = config["CloudinarySettings:ApiKey"];
            var apiSecret = config["CloudinarySettings:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        // function สำหรับอัปโหลดรูปภาพ
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); //ผลการอัปโหลดรูปภาพ

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream(); //อ่านไฟล์ในรูปแบบ Stream
                //สร้าง parameter สำหรับการอัปโหลดรูปภาพ
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = file.FileName + DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UseFilename = true,
                    UniqueFilename = true
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams); //อัปโหลดรูปภาพไปยัง Cloudinary
            }
            return uploadResult;
        }
    }
}
