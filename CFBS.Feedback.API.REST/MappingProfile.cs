using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CFBS.Feedback.API.REST.Models;
using CFBS.Feedback.DAL.Entities;

namespace CFBS.Feedback.API.REST
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ImageDTO, Image>().ReverseMap();
            CreateMap<LocationDTO, Location>().ReverseMap();
            CreateMap<QuestionDTO, Question>().ReverseMap();

            CreateMap<SubmittedAnswerDTO<AnswerDetailsDTO>, SubmittedAnswer>().ReverseMap();
            CreateMap<SubmittedAnswerDTO<ImageAnswerDetailsDTO>, SubmittedAnswer>().ReverseMap();

            CreateMap<AnswerDTO<AnswerDetailsDTO>, Answer>().ReverseMap();
            CreateMap<AnswerDTO<ImageAnswerDetailsDTO>, Answer>().ReverseMap();

            CreateMap<AnswerDetailsDTO, TextAnswer>().ReverseMap();

            CreateMap<ImageAnswerDetailsDTO, ImageAnswer>().ReverseMap();

            CreateMap<ActiveQuestionDTO, ActiveQuestion>().ReverseMap();
        }
    }
}
