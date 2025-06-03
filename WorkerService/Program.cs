using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Data;
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
var host = builder.Build();
host.Run();
