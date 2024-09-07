using System.Collections.ObjectModel;

namespace C971
{
    public partial class TermDetailPage : ContentPage
    {
        public Term Term { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public TermDetailPage(Term term)
        {
            InitializeComponent();
            Term = term;
            Courses = new ObservableCollection<Course>();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InsertDummyCoursesForTerm(Term.TermId);
            await LoadCourses(Term.TermId);
        }
        private async Task LoadCourses(int termId)
        {
            var dbService = new LocalDbService();
            var coursesFromDb = await dbService.GetCoursesByTermId(termId);

            if (coursesFromDb != null && coursesFromDb.Count > 0)
            {
                Courses.Clear();
                foreach (var course in coursesFromDb)
                {
                    Courses.Add(course);
                }
            }
            else
            {
                Console.WriteLine("No courses found for this term.");
            }
            Console.WriteLine($"Total courses loaded: {Courses.Count}");
        }
        private async Task InsertDummyCoursesForTerm(int termId)
        {
            var dbService = new LocalDbService();

            // Check if there are already courses for this term
            var existingCourses = await dbService.GetCoursesByTermId(termId);

            if (existingCourses.Count == 0)
            {
                // If no courses exist, insert dummy courses
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
                    }
                };

                // Insert dummy courses into the database
                foreach (var course in dummyCourses)
                {
                    await dbService.CreateCourse(termId, course);
                }

                Console.WriteLine("Dummy courses inserted.");
            }
            else
            {
                Console.WriteLine("Courses already exist for this term.");
            }
        }
        private async Task EditCourse(Course course)
        {
            await Navigation.PushAsync(new EditCoursePage(course));
        }
        private async void AddCourseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage(Term.TermId));
        }
        private async void DetailedCourseButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var course = button?.BindingContext as Course;

            if (course != null)
            {
                await Navigation.PushAsync(new CourseDetailPage(course));
            }
        }
        private async void OnDeleteTermClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the term: {Term.TermTitle}?", "Yes", "No");

            if (confirm)
            {
                var dbService = new LocalDbService();
                await dbService.DeleteTerm(Term);
                await DisplayAlert("Deleted", "Term deleted successfully.", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}