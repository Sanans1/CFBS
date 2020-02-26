using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class SubmittedAnswer
    {
        public int? ID { get; set; }
        public int AnswerID { get; set; }
        public int LocationID { get; set; }
        public int FeedbackSessionNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Location Location { get; set; }
    }
}
