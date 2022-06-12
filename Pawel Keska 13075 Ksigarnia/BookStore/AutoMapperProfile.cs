using AutoMapper;
using BookStore.DataBaseEntities;
using BookStore.DTOModels;

namespace BookStore
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserLoginDto, User>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<Book, BookDto>();
            CreateMap<Book, CreateBookDto>();
            CreateMap<Book, UpdateBookDto>();
        }
    }
}