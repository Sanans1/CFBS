using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class ActiveQuestion
    {
        public int QuestionID { get; set; }
        public int LocationID { get; set; }

        public virtual Question Question { get; set; }
        public virtual Location Location { get; set; }
    }
}
