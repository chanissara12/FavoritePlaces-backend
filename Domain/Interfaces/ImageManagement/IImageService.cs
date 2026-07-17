using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.ImageManagement
{
    public interface IImageService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
    }
}
