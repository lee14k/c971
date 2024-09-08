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
            InitializeAppData(mainPage);
        }

        private async void InitializeAppData(MainPage mainPage)
        {
            await InsertTermExample();  
            await mainPage.LoadTerms();
            await ScheduleUpcomingCourseNotificationsAsync();  
        }

        private async Task InsertTermExample()
        {
            var dbService = new LocalDbService();
            await ClearAllTerms(dbService);

            var newTerm = new Term
            {
                TermTitle = "Term One",
                StartDate = DateTime.Parse("2025-07-01"),
                EndDate = DateTime.Parse("2025-12-31")
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

        private async Task ScheduleUpcomingCourseNotificationsAsync()
        {
            await Task.Run(async () =>
            {
                var dbService = new LocalDbService();
                var courses = await dbService.GetCourses();

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

                    await Task.Delay(500);  
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

                LocalNotificationCenter.Current.Show(notification);
            }
        }
    }
}