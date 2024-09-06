using System;
using Microsoft.Maui.Controls;

namespace C971 {
public partial class EditCoursePage : ContentPage
{
    private Course _course;
    private Instructor _instructor;
        private bool _isInitializing = true;

        public EditCoursePage(Course course, Instructor instructor)
    {

            InitializeComponent();

            _course = course;
            _instructor = instructor;



            BindingContext = new
        {
            Course = _course,
            Instructor = _instructor
        };
            this.Appearing += EditCoursePage_Appearing;

        }
        private void EditCoursePage_Appearing(object sender, EventArgs e)
        {
            _isInitializing = false;
            StartDatePicker.Date = _course.StartDate;
            EndDatePicker.Date = _course.EndDate;
        }

        private void OnStartDateSelected(object sender, DateChangedEventArgs e)
        {
            if (_isInitializing) return;

            _course.StartDate = e.NewDate;
            
            DisplayAlert("Date Changed", $"New Start Date: {_course.StartDate}", "OK");
        }

        // Event handler for the end date picker
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

            if (_course.EndDate < _course.StartDate) 
            {
                await DisplayAlert("Validation Error", "End date cannot be before the start date.", "OK");
                return;
            }

            if (_course.EndDate < DateTime.Now.Date)
            {
                await DisplayAlert("Validation Error", "End date cannot be in the past.", "OK");
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

            var dbService = new LocalDbService();
            await dbService.UpdateCourse(_course);
            await dbService.UpdateInstructor(_instructor);
            await DisplayAlert("Success", "Course details saved successfully.", "OK");

            await Navigation.PopAsync();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.All(char.IsDigit);
        }
    }
    }