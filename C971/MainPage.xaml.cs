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

        private async void DetailedTermButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            var term = button?.BindingContext as Term;

            if (term != null)
            {

                await Navigation.PushAsync(new TermDetailPage(term));
            }
        }

        private async void EditTermButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var term = button?.BindingContext as Term;
            if (term != null)
            {
            }
        }
    }
}