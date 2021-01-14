using Crizzl.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Crizzl.Application.Interfaces
{
    public interface IFileService
    {
        FileUpload UploadImage(IFormFile file);
        string DeleteFile(string publicId);
    }
}