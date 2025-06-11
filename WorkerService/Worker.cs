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
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var overdueTodos = await dbContext.TodoItems
                        .Where(t => (t.Status == TodoStatus.New || t.Status == TodoStatus.InProgress)
                                && t.DueDate < DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                    foreach (var todo in overdueTodos)
                    {
                        string message = $"TodoItem overdue: '{todo.Title}' (Due: {todo.DueDate})";

                        // Log to Windows Event Viewer
                        LogToEventViewer(message, EventLogEntryType.Warning);

                        // Also log to console/log file
                        _logger.LogWarning(message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking for overdue TodoItems.");
                }

                // Wait 1 Seconds before next check
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private void LogToEventViewer(string message, EventLogEntryType type)
        {
            string source = "TodoMonitorWorker";
            string logName = "Application";

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
