using System.Collections.ObjectModel;
using SQLite;

namespace C971
{
    public partial class CourseDetailPage : ContentPage
    {
        private Course _course;
        private Instructor _instructor;

        public ObservableCollection<Instructor> Instructors { get; set; }

        public CourseDetailPage(Course course)
        {
            _course = course;
            InitializeComponent();
            Instructors = new ObservableCollection<Instructor>();

            // Set the BindingContext initially with just the course
            BindingContext = new
            {
                Course = _course,
                Instructors = Instructors
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await InsertDummyInstructorsForCourse(_course.CourseId);
            await LoadInstructorForCourse(_course.CourseId);
        }

        // Method to load instructors for the current course
        private async Task LoadInstructorForCourse(int courseId)
        {
            var dbService = new LocalDbService();
            var instructorFromDb = await dbService.GetInstructorByCourseId(courseId);

            if (instructorFromDb != null)
            {
                _instructor = instructorFromDb; // Assign the instructor to the private field
                // Update BindingContext with both course and instructor
                BindingContext = new { Course = _course, Instructor = _instructor };
            }
            else
            {
                Console.WriteLine("Instructor not found for the course.");
            }
        }

        // Method to insert dummy instructors
        private async Task InsertDummyInstructorsForCourse(int courseId)
        {
            var dbService = new LocalDbService();
            var existingInstructors = await dbService.GetInstructorByCourseId(courseId);

            if (existingInstructors == null)
            {
                var dummyInstructors = new List<Instructor>
                {
                    new Instructor { InstructorName = "John Doe", InstructorEmail = "johndoe@example.com", InstructorPhone = "555-1234", CourseId=courseId },
                    new Instructor { InstructorName = "Jane Smith", InstructorEmail = "janesmith@example.com", InstructorPhone = "555-5678", CourseId=courseId },
                    new Instructor { InstructorName = "Mark Johnson", InstructorEmail = "markjohnson@example.com", InstructorPhone = "555-8765", CourseId=courseId }
                };

                foreach (var instructor in dummyInstructors)
                {
                    await dbService.CreateInstructor(instructor);
                }

                Console.WriteLine("Dummy instructors inserted.");
            }
        }

        // Edit course info button click event handler
        private async void EditCourseInfoButton_Clicked(object sender, EventArgs e)
        {
            if (_course != null && _instructor != null)
            {
                // Navigate to the EditCoursePage, passing both course and instructor
                await Navigation.PushAsync(new EditCoursePage(_course, _instructor));
            }
            else
            {
                Console.WriteLine("Course or Instructor is null, cannot navigate.");
            }
        }
        private void AssesmentInfoButton_Clicked (object sender, EventArgs e)
        {

        }
    }
}