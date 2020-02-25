using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFBS.Feedback.API.REST.Models
{
    public class SubmittedAnswerDTO<TAnswerDetails>
    {
        public int? ID;
        public int AnswerID;
        public int LocationID;
        public int FeedbackSessionNumber;
        public DateTime CreatedAt;

        public AnswerDTO<TAnswerDetails> Answer;
        public LocationDTO Location;
    }
}
