using System;
using System.Collections.Generic;
using System.Text;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.DAL.Entities
{
    public class Question
    {
        public int? ID;
        public string Text;
        public FeedbackType FeedbackType;
        public AnswerType AnswerType;
        public DateTime CreatedAt;
    }
}
