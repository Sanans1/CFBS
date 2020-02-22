using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class SubmittedAnswer
    {
        public int? ID;
        public int AnswerID;
        public int LocationID;
        public int FeedbackSessionNumber;
        public DateTime CreatedAt;

        public Answer Answer;
        public Location Location;
    }
}
