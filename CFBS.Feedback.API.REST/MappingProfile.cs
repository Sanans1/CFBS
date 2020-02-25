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
            /*
            CreateMap<Product, ProductDTO>().ReverseMap().ForMember(destination => destination.Category, options => options.Ignore())
                .ForMember(destination => destination.Brand, options => options.Ignore())
                .ForMember(destination => destination.PriceHistories, options => options.Ignore());

            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Brand, BrandDTO>().ReverseMap();

            CreateMap<PriceHistory, PriceHistoryDTO>().ReverseMap();

            CreateMap<ProductDTO, CategoryDTO>().ForMember(destination => destination.ID, options => options.Ignore())
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.CategoryName));

            CreateMap<ProductDTO, BrandDTO>().ForMember(destination => destination.ID, options => options.Ignore())
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.BrandName));

            CreateMap<ProductDTO, PriceHistoryDTO>().ForMember(destination => destination.ID, options => options.Ignore())
                .ForMember(destination => destination.DateArchived, options => options.MapFrom(source => DateTime.Now))
                .ForMember(destination => destination.ProductID, options => options.MapFrom(source => source.ID));
                */

            CreateMap<ImageDTO, Image>().ReverseMap();
            CreateMap<LocationDTO, Location>().ReverseMap();
            CreateMap<QuestionDTO, Question>().ReverseMap();

            CreateMap<SubmittedAnswerDTO<AnswerDetailsDTO>, SubmittedAnswer>().ReverseMap();
            CreateMap<SubmittedAnswerDTO<ImageAnswerDetailsDTO>, SubmittedAnswer>().ReverseMap();

            CreateMap<AnswerDTO<AnswerDetailsDTO>, Answer>().ReverseMap();
            CreateMap<AnswerDTO<ImageAnswerDetailsDTO>, Answer>().ReverseMap();

            CreateMap<AnswerDTO<AnswerDetailsDTO>, TextAnswer>().ReverseMap();

            CreateMap<AnswerDTO<ImageAnswerDetailsDTO>, ImageAnswer>().ReverseMap();

            CreateMap<ActiveQuestionDTO, ActiveQuestion>().ReverseMap();
        }
    }
}
