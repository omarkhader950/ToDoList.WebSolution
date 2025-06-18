using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices; 
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Data;
using WorkerService;

Host.CreateDefaultBuilder(args)
    .UseWindowsService() 
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>(); 

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("Default"))); 
    })
    .Build()
    .Run();
