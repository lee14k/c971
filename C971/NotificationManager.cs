using System.Collections.ObjectModel;
using Plugin.LocalNotification;

namespace C971.Managers
{
    public class NotificationManager
    {
        private static NotificationManager _instance;

        public static NotificationManager Instance => _instance ??= new NotificationManager();

        public ObservableCollection<NotificationRequest> ScheduledNotifications { get; set; }

        private NotificationManager()
        {
            ScheduledNotifications = new ObservableCollection<NotificationRequest>();
        }
    }
}