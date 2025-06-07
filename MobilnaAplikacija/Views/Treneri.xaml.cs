using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views;

public partial class Treneri : ContentPage
{
    public Treneri()
    {
    }

    public Treneri(TreneriViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((TreneriViewModel)BindingContext).LoadTreneriAsync();
    }
}