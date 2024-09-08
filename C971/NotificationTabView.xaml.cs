using System.Collections.ObjectModel;
using Plugin.LocalNotification;
using Microsoft.Maui.Controls;
using C971.Managers;

namespace C971.Views
{
    public partial class NotificationTabView : ContentView
    {
        public ObservableCollection<NotificationRequest> ScheduledNotifications { get; set; } = new ObservableCollection<NotificationRequest>();

        public NotificationTabView()
        {
            InitializeComponent();
            BindingContext = this;

            LoadScheduledNotifications();
        }

        private void OnToggleNotificationsClicked(object sender, EventArgs e)
        {
            NotificationsListView.IsVisible = !NotificationsListView.IsVisible;

            if (NotificationsListView.IsVisible)
            {
                ToggleNotificationsButton.Text = "Hide Notifications";
            }
            else
            {
                ToggleNotificationsButton.Text = "Show Notifications";
            }
        }

        private void LoadScheduledNotifications()
        {
            ScheduledNotifications.Clear();
            foreach (var notification in NotificationManager.Instance.ScheduledNotifications)
            {
                ScheduledNotifications.Add(notification);
            }

            if (ScheduledNotifications.Count == 0)
            {
                Application.Current.MainPage.DisplayAlert("No Notifications", "There are no scheduled notifications.", "OK");
            }
        }
    }
}