using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CFBS.Common.Repository;
using CFBS.Feedback.API.REST.Models;
using CFBS.Feedback.DAL;
using CFBS.Feedback.DAL.Entities;

namespace CFBS.Feedback.API.REST.Services.Implementations
{
    public class ActiveQuestionRepository : GenericRepository<FeedbackContext, ActiveQuestion, QuestionDTO>
    {
        public ActiveQuestionRepository(FeedbackContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
