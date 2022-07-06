using System.Reflection;
using System.Text;
using BookStore;
using BookStore.DataBase;
using BookStore.DataBaseEntities;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Auth
var authorizationSettings = new AuthorizationSettings();
builder.Configuration.GetSection("Authentication").Bind(authorizationSettings);
builder.Services.AddSingleton(authorizationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authorizationSettings.JwtIssuer,
        ValidAudience = authorizationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationSettings.JwtKey)),
    };
});
// Add Scopes
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookCategoryService, BookCategoryService>();
builder.Services.AddHttpContextAccessor();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var server = builder.Configuration["DbServer"] ?? "postgre";
var port = builder.Configuration["DbPort"] ?? "5432";
var user = builder.Configuration["DBUser"] ?? "postgres";
var password = builder.Configuration["DBPassword"] ?? "postgres123";
var database = builder.Configuration["Database"] ?? "BookStore";

builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseNpgsql($"Server={server}; Port={port};Database={database};Username ={user};Password={password}")
);

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
DatabaseSeed.PrepPopulation(app);
app.MapControllers();

app.Run();