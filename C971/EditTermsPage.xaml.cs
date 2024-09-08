namespace C971
{
    public partial class EditTermsPage : ContentPage
    {
        public Term Term { get; set; }
        public EditTermsPage(Term term)
        {
            InitializeComponent();
            Term = term;

            BindingContext = this;
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Term.TermTitle))
            {
                await DisplayAlert("Validation Error", "Term Title cannot be empty.", "OK");
                return;
            }
            var dbService = new LocalDbService();
            var allTerms = await dbService.GetTerms();
            foreach (var existingTerm in allTerms)
            {
                if (existingTerm.TermId != Term.TermId)
                {
                    if ((Term.StartDate >= existingTerm.StartDate && Term.StartDate <= existingTerm.EndDate) ||
                        (Term.EndDate >= existingTerm.StartDate && Term.EndDate <= existingTerm.EndDate) ||
                        (Term.StartDate <= existingTerm.StartDate && Term.EndDate >= existingTerm.EndDate))
                    {
                        await DisplayAlert("Validation Error", "The term dates overlap with another term.", "OK");
                        return;
                    }
                }
            }
            await dbService.UpdateTerm(Term);
            await DisplayAlert("Success", "Term details saved successfully.", "OK");

            await Navigation.PopAsync();
        }
        private async void OnDeleteTermClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the term: {Term.TermTitle}?", "Yes", "No");

            if (confirm)
            {
                var dbService = new LocalDbService();
                await dbService.DeleteTerm(Term); 
                await DisplayAlert("Deleted", "Term deleted successfully.", "OK");
                await Navigation.PopAsync(); 
            }
        }
    }
}