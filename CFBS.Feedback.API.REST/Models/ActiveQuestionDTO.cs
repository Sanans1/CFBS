using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFBS.Feedback.DAL.Entities;

namespace CFBS.Feedback.API.REST.Models
{
    public class ActiveQuestionDTO
    {
        public int QuestionID;
        public int LocationID;

        public QuestionDTO Question;
        public LocationDTO Location;
    }
}
