using System.Collections.ObjectModel;
using SQLite;
using Plugin.LocalNotification;


namespace C971
{
    public partial class AssessmentDetailPage : ContentPage
    {
        public Course _course;
        public Assessment _assessment;

        public ObservableCollection<Assessment> Assessments { get; set; }

        public AssessmentDetailPage(Course course)
        {
            _course = course;
            InitializeComponent();
            Assessments = new ObservableCollection<Assessment>();

            BindingContext = new
            {
                Course = _course,
                Assessments = Assessments
            };
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await InsertDummyAssessmentsForCourse(_course.CourseId);
            await LoadAssessmentsForCourse(_course.CourseId);
        }
        private async Task LoadAssessmentsForCourse(int courseId)
        {
            var dbService = new LocalDbService();
            var assessmentsFromDb = await dbService.GetAssessmentByCourseId(courseId);

            if (assessmentsFromDb != null && assessmentsFromDb.Any())
            {
                Assessments.Clear();
                foreach (var assessment in assessmentsFromDb)
                {
                    Assessments.Add(assessment);
                }

                Console.WriteLine($"{assessmentsFromDb.Count} assessments loaded for course: {courseId}");
            }
            else
            {
                Console.WriteLine("No assessments found for the course.");
            }
        }
        private async Task InsertDummyAssessmentsForCourse(int courseId)
        {
            var dbService = new LocalDbService();

            var existingAssessments = await dbService.GetAssessmentByCourseId(courseId);

            if (existingAssessments.Count >= 2)
            {
                Console.WriteLine("This course already has the maximum of two assessments.");
                return;
            }
            var remainingSlots = 2 - existingAssessments.Count;
            var dummyAssessments = new List<Assessment>
    {
        new Assessment
        {
            AssessmentTitle = "Objective Assessment 101",
            AssessmentType = "Objective",
            CourseId = courseId,
            StartDate = DateTime.Parse("2025-07-08"),
            EndDate=DateTime.Parse("2025-07-15"),
        },
        new Assessment
        {
            AssessmentTitle = "Performance Assessment 101",
            AssessmentType = "Performance",
            CourseId = courseId,
            StartDate = DateTime.Parse("2025-07-16"),
            EndDate=DateTime.Parse("2025-07-23"),

        }
    };
            foreach (var assessment in dummyAssessments.Take(remainingSlots))
            {
                await dbService.CreateAssessment(courseId, assessment);
            }
        }
        private async void AssessmentInfoButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedAssessment = button?.CommandParameter as Assessment;

            if (_course != null && selectedAssessment != null)
            {
                await Navigation.PushAsync(new EditAssessmentPage(_course, selectedAssessment));
            }
            else
            {
                await DisplayAlert("Error", "Unable to load the assessment or course is null.", "OK");
            }
        }
        private async void AddNewAssessment_Clicked(object sender, EventArgs e)
        {
            var courseId = _course.CourseId;
            await Navigation.PushAsync(new AddAssessmentPage(courseId));

        }
        private async void SetStartReminderButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var assessment = button?.CommandParameter as Assessment;

            if (assessment != null)
            {
                await SetReminderNotification(
                    assessment.StartDate,
                    "Assessment Start Reminder",
                    $"Your assessment '{assessment.AssessmentTitle}' starts today!",
                    assessment.AssessmentId
                );
            }
        }

        private async void SetEndReminderButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var assessment = button?.CommandParameter as Assessment;

            if (assessment != null)
            {
                await SetReminderNotification(
                    assessment.EndDate,
                    "Assessment End Reminder",
                    $"Your assessment '{assessment.AssessmentTitle}' is due today!",
                    assessment.AssessmentId + 1000 // Ensure unique notification ID for end reminders
                );
            }
        }

        private async Task SetReminderNotification(DateTime notifyTime, string title, string message, int notificationId)
        {
            var notifyExactTime = notifyTime.Date.Add(TimeSpan.Zero);

            if (notifyExactTime < DateTime.Now)
            {
                notifyExactTime = DateTime.Now.AddMinutes(1); // Set notification 1 minute in the future if time has passed
            }

            var notificationRequest = new NotificationRequest
            {
                NotificationId = notificationId,
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyExactTime,
                    NotifyRepeatInterval = null // Notify once
                }
            };

            // Schedule the notification
            await LocalNotificationCenter.Current.Show(notificationRequest);
            await DisplayAlert("Reminder Set", $"A reminder has been set for your assessment! {notifyExactTime:MMMM dd, yyyy HH:mm:ss}", "OK");
        }
    }
}