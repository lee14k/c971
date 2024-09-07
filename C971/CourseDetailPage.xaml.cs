using System.Collections.ObjectModel;
using Plugin.LocalNotification;
using SQLite;
using C971.Managers; 

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

        private async Task LoadInstructorForCourse(int courseId)
        {
            var dbService = new LocalDbService();
            var instructorFromDb = await dbService.GetInstructorByCourseId(courseId);

            if (instructorFromDb != null)
            {
                _instructor = instructorFromDb;
                BindingContext = new { Course = _course, Instructor = _instructor };
            }
            else
            {
                Console.WriteLine("Instructor not found for the course.");
            }
        }

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

        private async void EditCourseInfoButton_Clicked(object sender, EventArgs e)
        {
            if (_course != null && _instructor != null)
            {
                await Navigation.PushAsync(new EditCoursePage(_course));
            }
            else
            {
                Console.WriteLine("Course or Instructor is null, cannot navigate.");
            }
        }

        private async void AssessmentInfoButton_Clicked(object sender, EventArgs e)
        {
            if (_course != null)
            {
                await Navigation.PushAsync(new AssessmentDetailPage(_course));
            }
            else
            {
                Console.WriteLine("Course is null, cannot navigate to assessments.");
            }
        }

        private async void OnShareNotesButtonClicked(object sender, EventArgs e)
        {
            // Ensure that the Course object is not null
            if (_course != null && !string.IsNullOrWhiteSpace(_course.Notes))
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = _course.Notes,
                    Title = "Share Course Notes"
                });
            }
            else
            {
                await DisplayAlert("No Notes", "There are no notes to share for this course.", "OK");
            }
        }

        private async void SetStartDateReminder_Clicked(object sender, EventArgs e)
        {
            await SetReminderNotification(_course.StartDate, "Course Start Reminder", $"Your course '{_course.CourseTitle}' starts today!", true);
        }

        private async void SetEndDateReminder_Clicked(object sender, EventArgs e)
        {
            await SetReminderNotification(_course.EndDate, "Course End Reminder", $"Your course '{_course.CourseTitle}' ends today!", false);
        }

        private async Task SetReminderNotification(DateTime dateTime, string title, string message, bool isStartDate)
        {
            // Retrieve the existing notification list
            var scheduledNotifications = NotificationManager.Instance.ScheduledNotifications;

            // Check if a notification for this course's start/end date already exists
            bool notificationExists = scheduledNotifications.Any(n =>
                n.NotificationId == (isStartDate ? _course.CourseId : _course.CourseId + 1000));

            if (notificationExists)
            {
                await DisplayAlert("Reminder Exists", $"A reminder for the {(isStartDate ? "start" : "end")} of this course has already been set.", "OK");
                return; // Exit early, no need to create another reminder
            }

            // Check if the date has already passed
            if (dateTime < DateTime.Now.Date)
            {
                await DisplayAlert("Invalid Date", "This date has already happened!", "OK");
                return; // Exit early, no need to set a reminder for a past date
            }

            // If the date is today, pop the notification immediately
            var notifyTime = dateTime;
            if (notifyTime.Date == DateTime.Now.Date)
            {
                notifyTime = DateTime.Now.AddSeconds(5); // Immediate notification, slight delay for user clarity
                await DisplayAlert("Reminder Set", "Reminder set successfully!", "OK");
            }

            // Proceed to create the notification
            var notificationRequest = new NotificationRequest
            {
                NotificationId = isStartDate ? _course.CourseId : _course.CourseId + 1000, // Unique ID for start/end notifications
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime,
                    NotifyRepeatInterval = null // Notify once
                }
            };

            // Schedule the notification
            await LocalNotificationCenter.Current.Show(notificationRequest);

            // Add the notification to the shared ScheduledNotifications collection
            NotificationManager.Instance.ScheduledNotifications.Add(notificationRequest);

            await DisplayAlert("Reminder Set", "Reminder set successfully!", "OK");
        }
    }
}