using Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts;
using Services;
using System.Reflection;
using System.Text;
using ToDoList.Core.Repositories;
using ToDoList.Core.ServiceContracts;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Mapping;
using ToDoList.Infrastructure.Repositories;
using ToDoList.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);





builder.Services.AddControllers();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IToDoItemsService, ToDoItemsService>();
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

builder.Services.AddScoped<IMappingService, MappingService>();

builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();


builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IUserValidator, UserValidator>();






builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
