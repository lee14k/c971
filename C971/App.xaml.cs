using Plugin.LocalNotification;

namespace C971
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            var navigationPage = new NavigationPage(mainPage);
            MainPage = navigationPage;
            InitializeAppData(mainPage);  // Initialize after UI is shown
        }

        private async void InitializeAppData(MainPage mainPage)
        {
            // Defer the term insertion and notifications to run in the background
            await InsertTermExample();  // Load terms after UI is displayed
            await mainPage.LoadTerms();  // Reload terms if necessary
            await ScheduleUpcomingCourseNotificationsAsync();  // Schedule notifications asynchronously
        }

        private async Task InsertTermExample()
        {
            var dbService = new LocalDbService();
            await ClearAllTerms(dbService);

            var newTerm = new Term
            {
                TermTitle = "Fall 2024",
                StartDate = DateTime.Parse("2024-09-01"),
                EndDate = DateTime.Parse("2025-02-28")
            };
            await dbService.CreateTerm(newTerm);
            Console.WriteLine("Term successfully inserted.");
        }

        private async Task ClearAllTerms(LocalDbService dbService)
        {
            var existingTerms = await dbService.GetTerms();
            foreach (var term in existingTerms)
            {
                await dbService.DeleteTerm(term);
                Console.WriteLine($"Term '{term.TermTitle}' has been deleted.");
            }
            Console.WriteLine("All existing terms have been cleared.");
        }

        // 1. Offload scheduling to a background thread with batching to avoid UI freezes
        private async Task ScheduleUpcomingCourseNotificationsAsync()
        {
            await Task.Run(async () =>
            {
                var dbService = new LocalDbService();
                var courses = await dbService.GetCourses();

                // 2. Batch scheduling to avoid overloading the system
                foreach (var course in courses)
                {
                    if (course.StartDate > DateTime.Now)
                    {
                        ScheduleNotification("Course Start", $"The course '{course.CourseTitle}' starts today.", course.StartDate);
                    }

                    if (course.EndDate > DateTime.Now)
                    {
                        ScheduleNotification("Course End", $"The course '{course.CourseTitle}' ends today.", course.EndDate);
                    }

                    // Add a delay between each notification to avoid overloading
                    await Task.Delay(500);  // Adjust the delay as needed
                }
            });
        }

        private void ScheduleNotification(string title, string message, DateTime notifyDate)
        {
            if (notifyDate > DateTime.Now)
            {
                var notification = new NotificationRequest
                {
                    NotificationId = new Random().Next(1000, 9999),
                    Title = title,
                    Description = message,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = notifyDate
                    }
                };

                // 3. Show notification
                LocalNotificationCenter.Current.Show(notification);
            }
        }
    }
}