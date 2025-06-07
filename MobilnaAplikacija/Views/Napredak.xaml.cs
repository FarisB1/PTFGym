using MobilnaAplikacija.Models;
using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views;

public partial class Napredak : ContentPage
{
    public Napredak()
    {
    }

    public Napredak(NapredakViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((NapredakViewModel)BindingContext).LoadNapredakAsync();
    }
}