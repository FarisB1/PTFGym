using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;
using MobilnaAplikacija.ViewModels;
using MobilnaAplikacija.Views;
using MobilnaAplikacija.Views.Termini;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobilnaAplikacija
{
    public partial class MainPage : ContentPage
    {
        public TerminViewModel _terminViewmodel;
        private readonly IAuthService _authService;

        private bool _isAdminPanelVisible;
        public bool IsAdminPanelVisible
        {
            get => _isAdminPanelVisible;
            set
            {
                if (_isAdminPanelVisible != value)
                {
                    _isAdminPanelVisible = value;
                    OnPropertyChanged(nameof(IsAdminPanelVisible));
                }
            }
        }

        private bool _isCreateRezervacijaVisible;
        public bool IsCreateRezervacijaVisible
        {
            get => _isCreateRezervacijaVisible;
            set
            {
                if (_isCreateRezervacijaVisible != value)
                {
                    _isCreateRezervacijaVisible = value;
                    OnPropertyChanged(nameof(IsCreateRezervacijaVisible));
                }
            }
        }


        private bool _isCreateNapredakVisible;
        public bool IsCreateNapredakVisible
        {
            get => _isCreateNapredakVisible;
            set
            {
                if (_isCreateNapredakVisible != value)
                {
                    _isCreateNapredakVisible = value;
                    OnPropertyChanged(nameof(IsCreateNapredakVisible));
                }
            }
        }

        private bool _isRezervacijaVisible;
        public bool IsRezervacijaVisible
        {
            get => _isRezervacijaVisible;
            set
            {
                if (_isRezervacijaVisible != value)
                {
                    _isRezervacijaVisible = value;
                    OnPropertyChanged(nameof(IsRezervacijaVisible));
                }
            }
        }

        private bool _isTreneriVisible;
        public bool IsTreneriVisible
        {
            get => _isTreneriVisible;
            set
            {
                if (_isTreneriVisible != value)
                {
                    _isTreneriVisible = value;
                    OnPropertyChanged(nameof(IsTreneriVisible));
                }
            }
        }

        private bool _isTerminiVisible;
        public bool IsTerminiVisible
        {
            get => _isTerminiVisible;
            set
            {
                if (_isTerminiVisible != value)
                {
                    _isTerminiVisible = value;
                    OnPropertyChanged(nameof(IsTerminiVisible));
                }
            }
        }

        private bool _isNapredakVisible;
        public bool IsNapredakVisible
        {
            get => _isNapredakVisible;
            set
            {
                if (_isNapredakVisible != value)
                {
                    _isNapredakVisible = value;
                    OnPropertyChanged(nameof(IsNapredakVisible));
                }
            }
        }

        private bool _isClanarineVisible;
        public bool IsClanarineVisible
        {
            get => _isClanarineVisible;
            set
            {
                if (_isClanarineVisible != value)
                {
                    _isClanarineVisible = value;
                    OnPropertyChanged(nameof(IsClanarineVisible));
                }
            }
        }

        public MainPage(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _terminViewmodel = new TerminViewModel(new TerminService(_authService));
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SetVisibilityBasedOnRole();
        }
        private async void SetVisibilityBasedOnRole()
        {
            try
            {
                var role = await _authService.GetUserRole();
                IsAdminPanelVisible = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
                IsTreneriVisible = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
                IsCreateRezervacijaVisible = string.Equals(role, "Clan", StringComparison.OrdinalIgnoreCase);
                IsTerminiVisible = ((string.Equals(role, "Trener", StringComparison.OrdinalIgnoreCase) || string.Equals(role, "Clan", StringComparison.OrdinalIgnoreCase)));
                IsCreateNapredakVisible = string.Equals(role, "Trener", StringComparison.OrdinalIgnoreCase);
                IsRezervacijaVisible = ((string.Equals(role, "Trener", StringComparison.OrdinalIgnoreCase) || string.Equals(role, "Clan", StringComparison.OrdinalIgnoreCase)));
                IsNapredakVisible = ((string.Equals(role, "Trener", StringComparison.OrdinalIgnoreCase) || string.Equals(role, "Clan", StringComparison.OrdinalIgnoreCase)));
                IsClanarineVisible= ((string.Equals(role, "Trener", StringComparison.OrdinalIgnoreCase) || string.Equals(role, "Clan", StringComparison.OrdinalIgnoreCase)));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }



        private async void OnTerminiButtonClicked(object sender, EventArgs e)
        {
            var role = await _authService.GetUserRole();
            if(string.Equals(role,"Clan", StringComparison.OrdinalIgnoreCase)) { 
                await Shell.Current.GoToAsync(nameof(Termini), true);
            }
            if (string.Equals(role, "Trener", StringComparison.OrdinalIgnoreCase))
            {
                await Shell.Current.GoToAsync(nameof(TerminTreneri), true);
            }
        }

        private async void OnTreneriButtonClicked(object sender, EventArgs e)
        {

            var role = await _authService.GetUserRole();
            if(string.Equals(role,"Admin", StringComparison.OrdinalIgnoreCase)) await Shell.Current.GoToAsync(nameof(Treneri));
        }

        private async void OnRezervacijeButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Rezervacije), true);
        }

        private async void OnNapredakButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.Napredak));
        }
        
        private async void OnMembershipViewButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.MembershipView));
        }
        
        private async void OnCreateRezervacijaButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CreateRezervacijaPage));
        }

        private async void OnCreateNapredakButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CreateNapredak));
        }

        private async void OnAdminDashboardPageButtonClicked(object sender, EventArgs e)
        {
            var role = await _authService.GetUserRole();
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)) await Shell.Current.GoToAsync(nameof(AdminDashboardPage));
        }


        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to logout", "OK");
            }
        }
    }
}
