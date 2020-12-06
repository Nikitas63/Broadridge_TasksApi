using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TasksApi.Handlers
{
    public static class Notification
    {
        public class Command : INotification
        {
            public string Message { get; set; }
        }

        public class Handler : INotificationHandler<Command>
        {
            public Task Handle(Command notification, CancellationToken cancellationToken)
            {
                Console.WriteLine($"- Notification {notification.Message}");
                return Task.CompletedTask;
            }
        }
    }
}
