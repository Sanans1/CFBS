using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class ActiveQuestion
    {
        public int QuestionID;
        public int LocationID;
        public DateTime CreatedAt;

        public Question Question;
        public Location Location;
    }
}
