using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views
{
    public partial class AdminDashboardPage : ContentPage
    {
        private readonly AdminDashboardViewModel _viewModel;

        public AdminDashboardPage(AdminDashboardViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUsersAsync();
        }
    }
}