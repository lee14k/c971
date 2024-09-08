using System.Collections.ObjectModel;

namespace C971
{
    public partial class TermDetailPage : ContentPage
    {
        public Term Term { get; set; }
        public ObservableCollection<Course> Courses { get; set; }

        private Dictionary<int, Instructor> _instructorMap;

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
            await LoadCoursesAndInstructors(Term.TermId);
        }

        private async Task LoadCoursesAndInstructors(int termId)
        {
            var dbService = new LocalDbService();

            var coursesFromDb = await dbService.GetCoursesByTermId(termId);

            if (coursesFromDb != null && coursesFromDb.Count > 0)
            {
                var instructorIds = coursesFromDb.Select(c => c.InstructorId).Distinct().ToList();

                var instructorsFromDb = await dbService.GetInstructorsByIds(instructorIds);

                _instructorMap = instructorsFromDb.ToDictionary(i => i.InstructorId);

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

            var existingCourses = await dbService.GetCoursesByTermId(termId);

            if (existingCourses.Count == 0)
            {
                var dummyInstructor = await dbService.GetInstructorByName("Anika Patel");


                    dummyInstructor = new Instructor
                    {
                        InstructorName = "Anika Patel",
                        InstructorEmail = "anika.patel@strimeuniversity.edu",
                        InstructorPhone = "555-1234"
                    };

                    await dbService.CreateInstructor(dummyInstructor);
                    Console.WriteLine("Dummy instructor inserted.");

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
            },
        };

                foreach (var course in dummyCourses)
                {
                    await dbService.CreateCourse(termId, course);
                }

                Console.WriteLine("Dummy course inserted.");
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
                Instructor instructor = null;
                if (_instructorMap != null && _instructorMap.ContainsKey(course.InstructorId))
                {
                    instructor = _instructorMap[course.InstructorId];
                }

                await Navigation.PushAsync(new CourseDetailPage(course, instructor));
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