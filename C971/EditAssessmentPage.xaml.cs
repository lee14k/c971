using System.Collections.ObjectModel;

namespace C971;

public partial class EditAssessmentPage : ContentPage
{
    public ObservableCollection<Assessment> Assessments { get; set; }
    public Course _course;
    public Assessment Assessment { get; set; }
    private bool _isInitializing = true;

    public EditAssessmentPage(Course course, Assessment assessment)
    {
        InitializeComponent();
        _course = course;
        Assessment = assessment;

        Assessments = new ObservableCollection<Assessment>();

        BindingContext = this;


        this.Appearing += EditCoursePage_Appearing;
    }

    private void EditCoursePage_Appearing(object sender, EventArgs e)
    {
        _isInitializing = false;

        LoadAssessments();
    }

    private async void LoadAssessments()
    {
        var dbService = new LocalDbService();

        var loadedAssessments = await dbService.GetAssessmentByCourseId(_course.CourseId);

        Assessments.Clear();
        foreach (var assessment in loadedAssessments)
        {
            Assessments.Add(assessment);
        }
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

        if (e.NewDate < Assessment.StartDate)
        {
            DisplayAlert("Error", "End date cannot be before start date.", "OK");
        }
        else
        {
            _course.EndDate = e.NewDate;
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Assessment.AssessmentTitle))
        {
            await DisplayAlert("Validation Error", "Assessment Title cannot be empty.", "OK");
            return;
        }

        if (Assessment.StartDate < DateTime.Now.Date)
        {
            await DisplayAlert("Validation Error", "Start date cannot be in the past.", "OK");
            return;
        }

        if (Assessment.EndDate < Assessment.StartDate)
        {
            await DisplayAlert("Validation Error", "End date cannot be before the start date.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Assessment.AssessmentTitle))
        {
            await DisplayAlert("Validation Error", "Assessment Nmme cannot be empty.", "OK");
            return;
        }

        var dbService = new LocalDbService();
        await dbService.UpdateAssessment(Assessment);

        foreach (var assessment in Assessments)
        {
            await dbService.UpdateAssessment(assessment);
        }

        await DisplayAlert("Success", "Assessment saved successfully.", "OK");

        await Navigation.PopAsync();
    }
}