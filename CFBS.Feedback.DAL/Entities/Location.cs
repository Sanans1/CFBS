using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class Location
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
