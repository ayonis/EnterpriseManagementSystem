using EnterpriseManagementSystem.Domain.ValueObjects;

namespace EnterpriseManagementSystem.Domain.Interfaces.Notification;


public interface INotificationStrategy
{

    Task SendAsync(NotificationMessage message);
}

