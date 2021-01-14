using System;
using Microsoft.AspNetCore.Http;

namespace Crizzl.Domain.ViewModels
{
    public class FileParameters
    {
        public string URL { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string PublicId { get; set; }
    }
}