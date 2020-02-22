using System;
using System.Collections.Generic;
using System.Text;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.DAL.Entities
{
    public class Answer
    {
        public int? ID;
        public int QuestionID;
        public AnswerType AnswerType;
        public DateTime CreatedAt;

        public Question Question;
    }
}
