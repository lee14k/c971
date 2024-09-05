namespace C971
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            InsertTermExample();
        

            MainPage = new NavigationPage(mainPage);
        }

        private async void InsertTermExample()
        {
            var dbService = new LocalDbService();

            // First term to insert
            var newTerm = new Term
            {
                TermTitle = "Fall 2024",
                StartDate = DateTime.Parse("2024-09-01"),
                EndDate = DateTime.Parse("2025-02-28")
            };

            // Second term to insert
            var newTermTwo = new Term
            {
                TermTitle = "Winter 2024",
                StartDate = DateTime.Parse("2025-03-01"),
                EndDate = DateTime.Parse("2025-09-30")
            };

            // Check for existing terms by TermTitle instead of TermId
            var existingTerm1 = await dbService.GetByTermTitle(newTerm.TermTitle);
            if (existingTerm1 == null)
            {
                await dbService.CreateTerm(newTerm);
                Console.WriteLine("First term successfully inserted.");
                //await InsertDummyCoursesForTerm(newTerm.TermId);
            }
            else
            {
                Console.WriteLine("First term already exists.");
            }

            // Insert second term
            var existingTerm2 = await dbService.GetByTermTitle(newTermTwo.TermTitle);
            if (existingTerm2 == null)
            {
                await dbService.CreateTerm(newTermTwo);
                Console.WriteLine("Second term successfully inserted.");
            }
            else
            {
                Console.WriteLine("Second term already exists.");
            }
        }
        //private async Task InsertDummyCoursesForTerm(int termId)
        //{
        //    var dbService = new LocalDbService();

        //    // Check if there are existing courses for the term
        //    var existingCourses = await dbService.GetCoursesByTermId(termId);
        //    if (existingCourses.Count == 0)
        //    {
        //        // Create 6 dummy courses for the specific term
        //        var dummyCourses = new List<Course>
        //        {
        //            new Course
        //            {
        //                CourseTitle = "Mathematics 101",
        //                TermId = termId,
        //                StartDate = DateTime.Parse("2024-09-01"),
        //                EndDate = DateTime.Parse("2024-12-15"),
        //                Status = "In Progress",
        //                InstructorId = 1,
        //                Notifications = true,
        //                Notes = "Mathematics course covering algebra and calculus."
        //            },
        //            new Course
        //            {
        //                CourseTitle = "History 101",
        //                TermId = termId,
        //                StartDate = DateTime.Parse("2024-09-01"),
        //                EndDate = DateTime.Parse("2024-12-15"),
        //                Status = "Completed",
        //                InstructorId = 2,
        //                Notifications = true,
        //                Notes = "History course covering World War II."
        //            },
        //            new Course
        //            {
        //                CourseTitle = "Computer Science 101",
        //                TermId = termId,
        //                StartDate = DateTime.Parse("2024-09-01"),
        //                EndDate = DateTime.Parse("2024-12-15"),
        //                Status = "In Progress",
        //                InstructorId = 3,
        //                Notifications = false,
        //                Notes = "Introductory programming course."
        //            },
        //            new Course
        //            {
        //                CourseTitle = "Physics 101",
        //                TermId = termId,
        //                StartDate = DateTime.Parse("2024-09-01"),
        //                EndDate = DateTime.Parse("2024-12-15"),
        //                Status = "In Progress",
        //                InstructorId = 4,
        //                Notifications = true,
        //                Notes = "Physics course covering mechanics and thermodynamics."
        //            },
        //            new Course
        //            {
        //                CourseTitle = "Biology 101",
        //                TermId = termId,
        //                StartDate = DateTime.Parse("2024-09-01"),
        //                EndDate = DateTime.Parse("2024-12-15"),
        //                Status = "Completed",
        //                InstructorId = 5,
        //                Notifications = false,
        //                Notes = "Biology course covering cell biology and genetics."
        //            },
        //            new Course
        //            {
        //                CourseTitle = "Chemistry 101",
        //                TermId = termId,
        //                StartDate = DateTime.Parse("2024-09-01"),
        //                EndDate = DateTime.Parse("2024-12-15"),
        //                Status = "In Progress",
        //                InstructorId = 6,
        //                Notifications = true,
        //                Notes = "Chemistry course covering organic and inorganic chemistry."
        //            }
        //        };

        //        // Insert each course into the database
        //        foreach (var course in dummyCourses)
        //        {
        //            await dbService.CreateCourse(course);
        //        }
        //    }
        //}


    }
}