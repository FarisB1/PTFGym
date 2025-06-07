using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views
{
    [QueryProperty(nameof(ReturnUrl), nameof(ReturnUrl))]
    public partial class LoginPage : ContentPage
    {
        public string ReturnUrl { get; set; }

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}