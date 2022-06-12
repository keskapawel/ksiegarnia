using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BookStore.DataBase;
using BookStore.DataBaseEntities;
using BookStore.DTOModels;
using BookStore.Exception;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Services
{

    public interface IUserService
    {
        void RegisterUser(UserRegisterDto dto);
        string Login(UserLoginDto dto);
    }

    public class UserService : IUserService
    {
        private readonly BookStoreDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthorizationSettings _authorizationSettings;

        public UserService(BookStoreDbContext context, IPasswordHasher<User> passwordHasher, AuthorizationSettings authorizationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authorizationSettings = authorizationSettings;
        }


        public void RegisterUser(UserRegisterDto dto)
        {
            var user = new User()
            {
                Email = dto.Email,
                RoleId = dto.RoleId,
            };
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public string Login(UserLoginDto dto)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(x => x.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid email or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }
            var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Email} "),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authorizationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authorizationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authorizationSettings.JwtIssuer,
                _authorizationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}