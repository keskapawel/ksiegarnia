using AutoMapper;
using BookStore.DataBase;
using BookStore.DTOModels;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Services
{

    public class BookService
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public BookService(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}