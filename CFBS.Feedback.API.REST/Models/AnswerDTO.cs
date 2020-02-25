using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.API.REST.Models
{
    public class AnswerDTO<TAnswerDetails>
    {
        public int? ID;
        public int QuestionID;
        public AnswerType AnswerType;
        public DateTime CreatedAt;

        public TAnswerDetails AnswerDetails;
    }
}
