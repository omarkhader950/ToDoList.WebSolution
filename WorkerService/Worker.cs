using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoList.Core.Enums;
using ToDoList.Infrastructure.Data;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int batchSize = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    int skip = 0;

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var batch = await dbContext.TodoItems
                            .Where(t =>
                                (t.Status == TodoStatus.New || t.Status == TodoStatus.InProgress) &&
                                t.DueDate < DateTime.UtcNow)
                            .OrderBy(t => t.Id)
                            .Skip(skip)
                            .Take(batchSize)
                            .AsNoTracking()
                            .ToListAsync(stoppingToken);

                        if (batch.Count == 0)
                            break;

                        foreach (var todo in batch)
                        {
                            string message = $"TodoItem overdue: '{todo.Title}' (Due: {todo.DueDate})";
                            LogToEventViewer(message, EventLogEntryType.Warning);
                            _logger.LogWarning(message);
                        }

                        skip += batchSize;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking for overdue TodoItems.");
                }

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }

        private void LogToEventViewer(string message, EventLogEntryType type)
        {
            const string source = "TodoMonitorWorker";
            const string logName = "Application";

            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }

                EventLog.WriteEntry(source, message, type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write to Windows Event Viewer.");
            }
        }
    }
}
