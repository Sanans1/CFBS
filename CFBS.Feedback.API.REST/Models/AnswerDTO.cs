using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.API.REST.Models
{
    public class AnswerDTO<TAnswerDetails>
    {
        public int? ID { get; set; }
        public int QuestionID { get; set; }
        public AnswerType AnswerType { get; set; }
        public DateTime CreatedAt { get; set; }

        public TAnswerDetails AnswerDetails { get; set; }
    }
}
