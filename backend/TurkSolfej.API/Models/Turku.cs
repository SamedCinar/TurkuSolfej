using System;
using System.Collections.Generic;

namespace TurkSolfej.API.Models
{
    public class Turku
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Region { get; set; }
        public string? ImageUrl { get; set; }
        public string? NotaUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string>? Tags { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
    }
} 