using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace C971
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Term> Terms { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Terms = new ObservableCollection<Term>();

            BindingContext = this;

            LoadTerms();
        }

        private async Task LoadTerms()
        {
            var dbService = new LocalDbService();
            var termsFromDb = await dbService.GetTerms();

            Terms.Clear();
            foreach (var term in termsFromDb)
            {
                Terms.Add(term);
            }
        }

        private void DetailedTermButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            var term = button?.BindingContext as Term;

            if (term != null)
            {

                Navigation.PushModalAsync(new TermDetailPage(term));
            }
        }
    }
}