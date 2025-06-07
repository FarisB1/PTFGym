using Microsoft.Extensions.DependencyInjection;
using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views;

public partial class CreateNapredak : ContentPage
{
    private readonly CreateNapredakViewModel _viewModel;

    public CreateNapredak()
    {
        InitializeComponent();
        _viewModel = IPlatformApplication.Current.Services.GetRequiredService<CreateNapredakViewModel>();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadClanovi();
    }
}