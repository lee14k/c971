namespace C971
{
    public partial class EditCoursePage : ContentPage
    {
        public Course Course { get; set; }
        public Instructor Instructor { get; set; }
        private DateTime TermStartDate { get; set; }
        private DateTime TermEndDate { get; set; }
        public EditCoursePage(Course course, DateTime termStartDate, DateTime termEndDate)
        {
            InitializeComponent();
            TermStartDate = termStartDate;
            TermEndDate = termEndDate;
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
            if (string.IsNullOrWhiteSpace(Course.Status))
            {
                await DisplayAlert("Validation Error", "Status cannot be empty.", "OK");
                return;
            }
            if (Course.StartDate < DateTime.Now.Date)
            {
                await DisplayAlert("Validation Error", "Start date cannot be in the past.", "OK");
                return;
            }

            if (Course.EndDate < Course.StartDate)
            {
                await DisplayAlert("Validation Error", "End date cannot be before the start date.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Instructor.InstructorName))
            {
                await DisplayAlert("Validation Error", "Instructor Name cannot be empty.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Instructor.InstructorEmail))
            {
                await DisplayAlert("Validation Error", "Instructor Email cannot be empty.", "OK");
                return;
            }

            if (!IsValidPhoneNumber(Instructor.InstructorPhone))
            {
                await DisplayAlert("Validation Error", "Instructor Phone number is invalid. It should contain only digits.", "OK");
                return;
            }
            if (Course.StartDate < TermStartDate || Course.StartDate > TermEndDate)
            {
                await DisplayAlert("Validation Error", $"Course start date must be within the term dates: {TermStartDate.ToShortDateString()} - {TermEndDate.ToShortDateString()}.", "OK");
                return;
            }

            if (Course.EndDate < TermStartDate || Course.EndDate > TermEndDate)
            {
                await DisplayAlert("Validation Error", $"Course end date must be within the term dates: {TermStartDate.ToShortDateString()} - {TermEndDate.ToShortDateString()}.", "OK");
                return;
            }


            var dbService = new LocalDbService();
            await dbService.UpdateCourse(Course);  
            await dbService.UpdateInstructor(Instructor);

            await DisplayAlert("Success", "Course and Instructor details saved successfully.", "OK");
            await Navigation.PopAsync();  
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) &&
                       System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9-]+$");
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
            bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the course: {Course.CourseTitle}?", "Yes", "No");

            if (confirm)
            {
                var dbService = new LocalDbService();
                await dbService.DeleteCourse(Course); 
                await DisplayAlert("Deleted", "Course deleted successfully.", "OK");
                await Navigation.PopToRootAsync();

            }
        }
    }
}