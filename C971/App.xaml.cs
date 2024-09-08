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
            await InsertDummyAssessmentsForCourses();
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

            await InsertDummyCoursesForTerm(newTerm.TermId);
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

                Console.WriteLine("Dummy courses successfully inserted.");
            }
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
        private async Task InsertDummyAssessmentsForCourses()
        {
            var dbService = new LocalDbService();
            var courses = await dbService.GetCourses();

            foreach (var course in courses)
            {
                await InsertDummyAssessmentsForCourse(course.CourseId);
            }
        }

        private async Task InsertDummyAssessmentsForCourse(int courseId)
        {
            var dbService = new LocalDbService();

            var existingAssessments = await dbService.GetAssessmentByCourseId(courseId);

            bool hasObjective = existingAssessments.Any(a => a.AssessmentType == "Objective");
            bool hasPerformance = existingAssessments.Any(a => a.AssessmentType == "Performance");

            if (!hasObjective)
            {
                var objectiveAssessment = new Assessment
                {
                    AssessmentTitle = "Objective Assessment 101",
                    AssessmentType = "Objective",
                    CourseId = courseId,
                    StartDate = DateTime.Parse("2025-07-08"),
                    EndDate = DateTime.Parse("2025-07-15"),
                };

                await dbService.CreateAssessment(courseId, objectiveAssessment);
                Console.WriteLine("Objective assessment inserted.");
            }

            if (!hasPerformance)
            {
                var performanceAssessment = new Assessment
                {
                    AssessmentTitle = "Performance Assessment 101",
                    AssessmentType = "Performance",
                    CourseId = courseId,
                    StartDate = DateTime.Parse("2025-07-16"),
                    EndDate = DateTime.Parse("2025-07-23"),
                };

                await dbService.CreateAssessment(courseId, performanceAssessment);
                Console.WriteLine("Performance assessment inserted.");
            }

            Console.WriteLine("Dummy assessments insertion complete.");
        }
    }
}