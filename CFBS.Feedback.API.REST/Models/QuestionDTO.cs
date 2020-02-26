using System;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.API.REST.Models
{
    public class QuestionDTO
    {
        public int? ID { get; set; }
        public string Text { get; set; }
        public FeedbackType FeedbackType { get; set; }
        public AnswerType AnswerType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
