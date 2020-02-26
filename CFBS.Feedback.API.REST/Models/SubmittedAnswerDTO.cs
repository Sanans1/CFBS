using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFBS.Feedback.API.REST.Models
{
    public class SubmittedAnswerDTO<TAnswerDetails>
    {
        public int? ID { get; set; }
        public int AnswerID { get; set; }
        public int LocationID { get; set; }
        public int FeedbackSessionNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public AnswerDTO<TAnswerDetails> Answer { get; set; }
        public LocationDTO Location { get; set; }
    }
}
