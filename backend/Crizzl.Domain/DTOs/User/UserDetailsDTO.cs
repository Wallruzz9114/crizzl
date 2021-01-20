using System;
using System.Collections.Generic;
using Crizzl.Domain.ViewModels;

namespace Crizzl.Domain.DTOs.User
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Bio { get; set; }
        public string DatingTarget { get; set; }
        public string Interests { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Alias { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string MainPhotoURL { get; set; }
        public ICollection<PhotoDetails> Photos { get; set; }
    }
}