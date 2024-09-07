namespace C971
{
    public partial class EditCoursePage : ContentPage
    {
        public Course Course { get; set; }
        public Instructor Instructor { get; set; }

        public EditCoursePage(Course course)
        {
            InitializeComponent();
            Course = course;
            LoadInstructor(Course.InstructorId);
        }
        private async void LoadInstructor(int instructorId)
        {
            var dbService = new LocalDbService();
            Instructor = await dbService.GetInstructorById(instructorId);

            if (Instructor != null)
            {
                BindingContext = this;
            }
            else
            {
                await DisplayAlert("Error", "Instructor not found.", "OK");
                await Navigation.PopAsync();
            }
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Course.CourseTitle))
            {
                await DisplayAlert("Validation Error", "Course Title cannot be empty.", "OK");
                return;
            }
            var dbService = new LocalDbService();
            await dbService.UpdateCourse(Course);
            await dbService.UpdateInstructor(Instructor);
            await DisplayAlert("Success", "Course and Instructor details saved successfully.", "OK");
            await Navigation.PopAsync(); 
        }
        private void OnStartDateSelected(object sender, DateChangedEventArgs e)
        {
            Course.StartDate = e.NewDate;
        }
        private void OnEndDateSelected(object sender, DateChangedEventArgs e)
        {
            Course.EndDate = e.NewDate;
        }
        private async void OnDeleteCourseClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the term: {Course.CourseTitle}?", "Yes", "No");

            if (confirm)
            {
                var dbService = new LocalDbService();
                await dbService.DeleteCourse(Course);
                await DisplayAlert("Deleted", "Term deleted successfully.", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}