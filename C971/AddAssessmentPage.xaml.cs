namespace C971;

public partial class AddAssessmentPage : ContentPage
{
    public Assessment Assessment { get; set; }
    public int CourseId { get; set; }

    public AddAssessmentPage(int courseId)
    {
        InitializeComponent();
        CourseId = courseId;
        Assessment = new Assessment
        {
            CourseId = courseId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(1)
        };
        BindingContext = this;
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var dbService = new LocalDbService();
        var existingAssessments = await dbService.GetAssessmentByCourseId(CourseId);

        if (existingAssessments.Any(a => a.AssessmentType == Assessment.AssessmentType))
        {
            await DisplayAlert("Validation Error", $"There can only be one {Assessment.AssessmentType} assessment per course.", "OK");
            return;
        }
        await dbService.CreateAssessment(CourseId, Assessment);
        await DisplayAlert("Success", "Assessment added successfully.", "OK");
        await Navigation.PopAsync();  
    }
}