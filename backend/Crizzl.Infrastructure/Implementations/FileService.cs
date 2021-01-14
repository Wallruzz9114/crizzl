using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Crizzl.Application.Interfaces;
using Crizzl.Application.Settings;
using Crizzl.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Crizzl.Infrastructure.Implementations
{
    public class FileService : IFileService
    {
        private readonly Cloudinary _cloudinary;

        public FileService(IOptions<CloudinarySettings> cloudinaryConfiguration)
        {
            var account = new Account(
                cloudinaryConfiguration.Value.CloudName,
                cloudinaryConfiguration.Value.APIKey,
                cloudinaryConfiguration.Value.APISecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public FileUpload UploadImage(IFormFile file)
        {
            var imageUploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
                };

                imageUploadResult = _cloudinary.Upload(uploadParams);
            }

            if (imageUploadResult.Error != null) throw new Exception(imageUploadResult.Error.Message);

            return new FileUpload
            {
                PublicId = imageUploadResult.PublicId,
                URL = imageUploadResult.SecureUrl.AbsoluteUri
            };
        }

        public string DeleteFile(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deletionResult = _cloudinary.Destroy(deleteParams);

            return deletionResult.Result == "ok" ? deletionResult.Result : null;
        }
    }
}