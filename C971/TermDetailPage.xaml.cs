using System.Collections.ObjectModel;

namespace C971
{
    public partial class TermDetailPage : ContentPage
    {
        private Term _term;
        public ObservableCollection<Course> Courses { get; set; } 

        public TermDetailPage(Term term)
        {
            _term = term;
            InitializeComponent();
            Courses = new ObservableCollection<Course>();

            BindingContext = new
            {
                Term = _term,
                Courses = Courses
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await InsertDummyCoursesForTerm(_term.TermId);

            await LoadCourses(_term.TermId);
        }

        private async Task LoadCourses(int termId)
        {
            var dbService = new LocalDbService();
            var coursesFromDb = await dbService.GetCoursesByTermId(termId);

            Courses.Clear();
            foreach (var course in coursesFromDb)
            {
                Courses.Add(course);
            }
        }
        private async Task InsertDummyCoursesForTerm(int termId)
        {
            var dbService = new LocalDbService();

            var existingCourses = await dbService.GetCoursesByTermId(termId);
            if (existingCourses.Count == 0)
            {
                var dummyCourses = new List<Course>
                {
                    new Course
                    {
                        CourseTitle = "Mathematics 101",
                        TermId = termId,
                        StartDate = DateTime.Parse("2024-09-01"),
                        EndDate = DateTime.Parse("2024-12-15"),
                        Status = "In Progress",
                        InstructorId = 1,
                        Notifications = true,
                        Notes = "Mathematics course covering algebra and calculus."
                    },
                    new Course
                    {
                        CourseTitle = "History 101",
                        TermId = termId,
                        StartDate = DateTime.Parse("2024-09-01"),
                        EndDate = DateTime.Parse("2024-12-15"),
                        Status = "Completed",
                        InstructorId = 2,
                        Notifications = true,
                        Notes = "History course covering World War II."
                    },
                    new Course
                    {
                        CourseTitle = "Computer Science 101",
                        TermId = termId,
                        StartDate = DateTime.Parse("2024-09-01"),
                        EndDate = DateTime.Parse("2024-12-15"),
                        Status = "In Progress",
                        InstructorId = 3,
                        Notifications = false,
                        Notes = "Introductory programming course."
                    },
                    new Course
                    {
                        CourseTitle = "Physics 101",
                        TermId = termId,
                        StartDate = DateTime.Parse("2024-09-01"),
                        EndDate = DateTime.Parse("2024-12-15"),
                        Status = "In Progress",
                        InstructorId = 4,
                        Notifications = true,
                        Notes = "Physics course covering mechanics and thermodynamics."
                    },
                    new Course
                    {
                        CourseTitle = "Biology 101",
                        TermId = termId,
                        StartDate = DateTime.Parse("2024-09-01"),
                        EndDate = DateTime.Parse("2024-12-15"),
                        Status = "Completed",
                        InstructorId = 5,
                        Notifications = false,
                        Notes = "Biology course covering cell biology and genetics."
                    },
                    new Course
                    {
                        CourseTitle = "Chemistry 101",
                        TermId = termId,
                        StartDate = DateTime.Parse("2024-09-01"),
                        EndDate = DateTime.Parse("2024-12-15"),
                        Status = "In Progress",
                        InstructorId = 6,
                        Notifications = true,
                        Notes = "Chemistry course covering organic and inorganic chemistry."
                    }
                };

                foreach (var course in dummyCourses)
                {
                    await dbService.CreateCourse(course);
                }
            }
        }
        private async void DetailedCourseButton_Clicked(object sender, EventArgs e)
        {
            
                var button = sender as Button;

            var course = button?.BindingContext as Course;


            if (course != null)
                {
                
                await    Navigation.PushAsync(new CourseDetailPage(course));
                }
            
        }

    }
}