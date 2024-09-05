namespace C971
{
    public partial class CourseDetailPage : ContentPage
    {
        private Course _course;

        public CourseDetailPage(Course course)
        {
            _course = course;
            InitializeComponent();

            BindingContext = _course;
        }
    }
}