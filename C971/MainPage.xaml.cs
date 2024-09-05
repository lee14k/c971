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

            // Initialize the observable collection
            Terms = new ObservableCollection<Term>();

            // Set BindingContext for data binding
            BindingContext = this;

            // Load the terms from the database
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
    }
}