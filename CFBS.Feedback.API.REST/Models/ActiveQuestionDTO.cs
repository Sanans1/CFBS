using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFBS.Feedback.DAL.Entities;

namespace CFBS.Feedback.API.REST.Models
{
    public class ActiveQuestionDTO
    {
        public int? ID { get; set; }
        public int QuestionID { get; set; }
        public int LocationID { get; set; }

        public QuestionDTO Question { get; set; }
        public LocationDTO Location { get; set; }
    }
}
