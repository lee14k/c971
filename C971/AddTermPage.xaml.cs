namespace C971;

public partial class AddTermPage : ContentPage
{
    private Term _term;
    private bool _isInitializing = true;

    public AddTermPage()
    {
        InitializeComponent();
        _term = new Term();  // Initialize the Term object
        BindingContext = _term;  // Set _term as the binding context
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

        _term.StartDate = e.NewDate;

        DisplayAlert("Date Changed", $"New Start Date: {_term.StartDate}", "OK");
    }

    private void OnEndDateSelected(object sender, DateChangedEventArgs e)
    {
        if (_isInitializing) return;

        if (e.NewDate < _term.StartDate)
        {
            DisplayAlert("Error", "End date cannot be before start date.", "OK");
            EndDatePicker.Date = _term.EndDate;
        }
        else
        {
            _term.EndDate = e.NewDate;
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_term.TermTitle))
        {
            await DisplayAlert("Validation Error", "Course Title cannot be empty.", "OK");
            return;
        }

        if (_term.StartDate < DateTime.Now.Date)
        {
            await DisplayAlert("Validation Error", "Start date cannot be in the past.", "OK");
            return;
        }

        if (_term.EndDate < _term.StartDate)
        {
            await DisplayAlert("Validation Error", "End date cannot be before the start date.", "OK");
            return;
        }

        var dbService = new LocalDbService();
        await dbService.CreateTerm(_term);
        await DisplayAlert("Success", "New course and instructor added successfully.", "OK");

        await Navigation.PopAsync();
    }
}