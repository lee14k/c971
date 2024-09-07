using System.Collections.ObjectModel;
using Plugin.LocalNotification;

namespace C971.Managers // You can adjust the namespace to your folder structure
{
    public class NotificationManager
    {
        // Singleton instance
        private static NotificationManager _instance;

        // Public access to the instance
        public static NotificationManager Instance => _instance ??= new NotificationManager();

        // The shared notification collection
        public ObservableCollection<NotificationRequest> ScheduledNotifications { get; set; }

        // Private constructor to prevent direct instantiation
        private NotificationManager()
        {
            ScheduledNotifications = new ObservableCollection<NotificationRequest>();
        }
    }
}