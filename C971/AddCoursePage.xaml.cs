namespace C971
{
    public partial class AddCoursePage : ContentPage
    {
        private Course _course;
        private Instructor _instructor;
        private bool _isInitializing = true;
        private DateTime TermStartDate { get; set; }
        private DateTime TermEndDate { get; set; }

        public AddCoursePage(int termId, DateTime termStartDate, DateTime termEndDate)
        {
            InitializeComponent();
            TermStartDate = termStartDate;
            TermEndDate = termEndDate;
            _course = new Course { TermId = termId };
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
            if (_course.EndDate < DateTime.Now.Date)
            {
                await DisplayAlert("Validation Error", "End date cannot be in the past.", "OK");
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
            if (_course.StartDate < TermStartDate || _course.StartDate > TermEndDate)
            {
                await DisplayAlert("Validation Error", $"Course start date must be within the term dates: {TermStartDate.ToShortDateString()} - {TermEndDate.ToShortDateString()}.", "OK");
                return;
            }

            if (_course.EndDate < TermStartDate || _course.EndDate > TermEndDate)
            {
                await DisplayAlert("Validation Error", $"Course end date must be within the term dates: {TermStartDate.ToShortDateString()} - {TermEndDate.ToShortDateString()}.", "OK");
                return;
            }
            var dbService = new LocalDbService();
            _course.InstructorId = _instructor.InstructorId;
            try
            {
                await dbService.CreateInstructor(_instructor);
            }
            catch (Exception ex) {
                await DisplayAlert("Error", $"Failed to add instructor: {ex.Message}", "OK");
                return;
                    }

            try
            {
                await dbService.CreateCourse(_course.TermId, _course);

            }
            catch (Exception ex) {
                await DisplayAlert("Error", $"Failed to add course: {ex.Message}", "OK");
                return;
            }
            await DisplayAlert("Success", "New course and instructor added successfully.", "OK");
            await Navigation.PopAsync();
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) &&
                       System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9-]+$");
        }
    }
}