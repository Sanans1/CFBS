using System;
using System.Collections.Generic;
using System.Text;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.DAL.Entities
{
    public class Question
    {
        public int? ID { get; set; }
        public string Text { get; set; }
        public FeedbackType FeedbackType { get; set; }
        public AnswerType AnswerType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
