namespace C971
{
    public partial class AddCoursePage : ContentPage
    {
        private Course _course;
        private Instructor _instructor;
        private bool _isInitializing = true;

        public AddCoursePage(int termId)
        {
            InitializeComponent();


            // Initialize new course and instructor objects
            _course = new Course { TermId = termId }; // Assign termId to the new course
            _instructor = new Instructor();

            BindingContext = new
            {
                Course = _course,
                Instructor = _instructor
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
                            StartDatePicker.Date = DateTime.Today;
            EndDatePicker.Date = DateTime.Today;
            _isInitializing = false;
        }

        private void OnStartDateSelected(object sender, DateChangedEventArgs e)
        {
            if (_isInitializing) return;

            _course.StartDate = e.NewDate;

            DisplayAlert("Date Changed", $"New Start Date: {_course.StartDate}", "OK");
        }

        private void OnEndDateSelected(object sender, DateChangedEventArgs e)
        {
            if (_isInitializing) return;

            if (e.NewDate < _course.StartDate)
            {
                DisplayAlert("Error", "End date cannot be before start date.", "OK");
                EndDatePicker.Date = _course.EndDate;
            }
            else
            {
                _course.EndDate = e.NewDate;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Validation checks
            if (string.IsNullOrWhiteSpace(_course.CourseTitle))
            {
                await DisplayAlert("Validation Error", "Course Title cannot be empty.", "OK");
                return;
            }

            if (_course.StartDate < DateTime.Now.Date)
            {
                await DisplayAlert("Validation Error", "Start date cannot be in the past.", "OK");
                return;
            }

            if (_course.EndDate < _course.StartDate)
            {
                await DisplayAlert("Validation Error", "End date cannot be before the start date.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(_instructor.InstructorName))
            {
                await DisplayAlert("Validation Error", "Instructor Name cannot be empty.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(_instructor.InstructorEmail))
            {
                await DisplayAlert("Validation Error", "Instructor Email cannot be empty.", "OK");
                return;
            }

            if (!IsValidPhoneNumber(_instructor.InstructorPhone))
            {
                await DisplayAlert("Validation Error", "Instructor Phone number is invalid. It should contain only digits.", "OK");
                return;
            }
            var termId = _course.TermId;
            // Save new course and instructor to the database
            var dbService = new LocalDbService();
            await dbService.CreateCourse(termId, _course);
            await dbService.CreateInstructor(_instructor);
            await DisplayAlert("Success", "New course and instructor added successfully.", "OK");

            await Navigation.PopAsync();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.All(char.IsDigit);
        }
    }
}