using System;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.API.REST.Models
{
    public class QuestionDTO
    {
        public int? ID;
        public string Text;
        public FeedbackType FeedbackType;
        public AnswerType AnswerType;
        public DateTime CreatedAt;
    }
}
