using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views;

public partial class Rezervacije : ContentPage
{
    public Rezervacije()
    {
    }

    public Rezervacije(RezervacijeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((RezervacijeViewModel)BindingContext).LoadRezervacijeAsync();
    }
}