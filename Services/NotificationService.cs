// Services/NotificationService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorTienda.Services
{
    public class NotificationService
    {
        public class Notification
        {
            public int Id { get; set; }
            public string Message { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.Now;
        }

        private List<Notification> _notifications = new();
        public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();

        public event Action? OnNotificationsChanged;
        private int _nextId = 1;

        public void AddNotification(string message)
        {
            _notifications.Insert(0, new Notification
            {
                Id = _nextId++,
                Message = message
            });

            // Mantener un máximo de 5 notificaciones
            if (_notifications.Count > 5)
            {
                _notifications.RemoveAt(_notifications.Count - 1);
            }

            OnNotificationsChanged?.Invoke();
        }

        public void RemoveNotification(int id)
        {
            _notifications.RemoveAll(n => n.Id == id);
            OnNotificationsChanged?.Invoke();
        }

        public void ClearNotifications()
        {
            _notifications.Clear();
            OnNotificationsChanged?.Invoke();
        }
    }
}