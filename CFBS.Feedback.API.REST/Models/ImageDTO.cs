using System;

namespace CFBS.Feedback.API.REST.Models
{
    public class ImageDTO
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
