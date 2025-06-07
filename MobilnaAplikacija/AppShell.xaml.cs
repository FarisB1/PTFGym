using MobilnaAplikacija.Views;
using MobilnaAplikacija.Views.Termini;
using System.Windows.Input;

namespace MobilnaAplikacija
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;

        public AppShell(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(Termini), typeof(Termini));
            Routing.RegisterRoute(nameof(Rezervacije), typeof(Rezervacije));
            Routing.RegisterRoute(nameof(Treneri), typeof(Treneri));
            Routing.RegisterRoute(nameof(Napredak), typeof(Napredak));
            Routing.RegisterRoute(nameof(TerminTreneri), typeof(TerminTreneri));
            Routing.RegisterRoute(nameof(CreateTermin), typeof(CreateTermin));
            Routing.RegisterRoute(nameof(TerminDetails), typeof(TerminDetails));
            Routing.RegisterRoute(nameof(MembershipView), typeof(MembershipView)); 
            Routing.RegisterRoute(nameof(CreateRezervacijaPage), typeof(CreateRezervacijaPage)); 
            Routing.RegisterRoute(nameof(CreateNapredak), typeof(CreateNapredak));
            Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));

        }

        public ICommand CustomBackCommand => new Command(async () =>
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Exit", "Do you want to go back?", "Yes", "No");
            if (answer)
            {
                await Shell.Current.GoToAsync(".."); // Navigate back
            }
        });

    }
}