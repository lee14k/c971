using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.LocalNotification;
using C971.Managers;

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

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTerms(); 
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
            NotificationsListView.ItemsSource = NotificationManager.Instance.ScheduledNotifications;

            if (NotificationManager.Instance.ScheduledNotifications.Count == 0)
            {
                DisplayAlert("No Notifications", "There are no scheduled notifications.", "OK");
            }
            else
            {
                NotificationsListView.IsVisible = true;
            }
        }

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
        private async Task InsertDummyCoursesForAllTerms()
        {
            var dbService = new LocalDbService();

            var terms = await dbService.GetTerms();

            foreach (var term in terms)
            {
                await InsertDummyCoursesForTerm(term.TermId);
            }
        }

        private async Task InsertDummyCoursesForTerm(int termId)
        {
            var dbService = new LocalDbService();

            var existingCourses = await dbService.GetCoursesByTermId(termId);

            if (existingCourses == null || existingCourses.Count == 0)
            {
                var dummyInstructor = await dbService.GetInstructorByName("Anika Patel");

                if (dummyInstructor == null)
                {
                    dummyInstructor = new Instructor
                    {
                        InstructorName = "Anika Patel",
                        InstructorEmail = "anika.patel@strimeuniversity.edu",
                        InstructorPhone = "555-1234"
                    };

                    await dbService.CreateInstructor(dummyInstructor);
                    Console.WriteLine("Dummy instructor inserted.");
                }

                var dummyCourses = new List<Course>
        {
            new Course
            {
                CourseTitle = "Scripting and Programming",
                TermId = termId,
                StartDate = DateTime.Parse("2025-07-01"),
                EndDate = DateTime.Parse("2025-12-31"),
                Status = "In Progress",
                InstructorId = dummyInstructor.InstructorId,
                Notifications = true,
                Notes = "Plan to pass - study hard and write lots of code"
            }
        };

                foreach (var course in dummyCourses)
                {
                    await dbService.CreateCourse(termId, course);
                }

                Console.WriteLine("Dummy course inserted for term: " + termId);
            }
            else
            {
                Console.WriteLine("Courses already exist for this term. Skipping dummy course insertion.");
            }
        }
    }
}
