using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views
{
    public partial class CreateRezervacijaPage : ContentPage
    {
        public CreateRezervacijaPage(CreateRezervacijaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ((CreateRezervacijaViewModel)BindingContext).LoadData();
        }
    }
}