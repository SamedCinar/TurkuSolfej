using System;
using System.Collections.Generic;

namespace TurkSolfej.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
        public string? Instrument { get; set; }
        public string? ExperienceLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public List<int>? FavoriteTurkuler { get; set; }
        public List<int>? Playlists { get; set; }
        public bool IsAdmin { get; set; }
    }
} 