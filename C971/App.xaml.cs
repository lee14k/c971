namespace C971
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            // Insert terms on app start
            InsertTermExample();

            MainPage = mainPage;
        }

        private async void InsertTermExample()
        {
            var dbService = new LocalDbService();

            // First term to insert
            var newTerm = new Term
            {
                TermTitle = "Fall 2024",
                StartDate = DateTime.Parse("2024-09-01"),
                EndDate = DateTime.Parse("2025-02-28")
            };

            // Second term to insert
            var newTermTwo = new Term
            {
                TermTitle = "Winter 2024",
                StartDate = DateTime.Parse("2025-03-01"),
                EndDate = DateTime.Parse("2025-09-30")
            };

            // Check for existing terms by TermTitle instead of TermId
            var existingTerm1 = await dbService.GetByTermTitle(newTerm.TermTitle);
            if (existingTerm1 == null)
            {
                await dbService.CreateTerm(newTerm);
                Console.WriteLine("First term successfully inserted.");
            }
            else
            {
                Console.WriteLine("First term already exists.");
            }

            // Insert second term
            var existingTerm2 = await dbService.GetByTermTitle(newTermTwo.TermTitle);
            if (existingTerm2 == null)
            {
                await dbService.CreateTerm(newTermTwo);
                Console.WriteLine("Second term successfully inserted.");
            }
            else
            {
                Console.WriteLine("Second term already exists.");
            }
        }
    }
}