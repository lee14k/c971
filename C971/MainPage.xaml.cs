using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.LocalNotification;
using C971.Managers;  // Import the namespace

namespace C971
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Term> Terms { get; set; }
        public ObservableCollection<NotificationRequest> _scheduledNotifications = new ObservableCollection<NotificationRequest>();

        public ObservableCollection<NotificationRequest> ScheduledNotifications
        {
            get => _scheduledNotifications;
            set
            {
                _scheduledNotifications = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();

            Terms = new ObservableCollection<Term>();

            BindingContext = this;

            LoadTerms();  // Load terms initially when the page is created
        }

        // Override the OnAppearing method to reload terms when the page appears
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTerms();  // Reload terms when the page reappears
        }

        // Load terms from the database
        public async Task LoadTerms()
        {
            var dbService = new LocalDbService();
            var termsFromDb = await dbService.GetTerms();

            Terms.Clear();
            foreach (var term in termsFromDb)
            {
                Terms.Add(term);
            }
        }

        private void OnToggleNotificationsClicked(object sender, EventArgs e)
        {
            NotificationsListView.IsVisible = !NotificationsListView.IsVisible;

            if (NotificationsListView.IsVisible)
            {
                ToggleNotificationsButton.Text = "Hide Notifications";
                LoadScheduledNotifications();
            }
            else
            {
                ToggleNotificationsButton.Text = "Show Notifications";
            }
        }

        private void LoadScheduledNotifications()
        {
            // Bind the singleton's ScheduledNotifications collection to the ListView
            NotificationsListView.ItemsSource = NotificationManager.Instance.ScheduledNotifications;

            if (NotificationManager.Instance.ScheduledNotifications.Count == 0)
            {
                DisplayAlert("No Notifications", "There are no scheduled notifications.", "OK");
            }
            else
            {
                NotificationsListView.IsVisible = true;  // Show the ListView with notifications
            }
        }



        private async void DetailedTermButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var term = button?.BindingContext as Term;

            if (term != null)
            {
                await Navigation.PushAsync(new TermDetailPage(term));
            }
        }

        private async void EditTermButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var term = button?.BindingContext as Term;

            if (term != null)
            {
                await Navigation.PushAsync(new EditTermsPage(term));
            }
        }

        private async void OnAddTermClicked(object sender, EventArgs e)
        {

                await Navigation.PushAsync(new AddTermPage());
        }
    }
}