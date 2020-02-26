using System;
using System.Collections.Generic;
using System.Text;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.DAL.Entities
{
    public class Answer
    {
        public int? ID { get; set; }
        public int QuestionID { get; set; }
        public AnswerType AnswerType { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Question Question { get; set; }
    }
}
