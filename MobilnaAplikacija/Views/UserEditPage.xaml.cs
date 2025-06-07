using MobilnaAplikacija.Models;
using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views;

public partial class UserEditPage : ContentPage
{
    private readonly UserEditViewModel _viewModel;

    public UserEditPage(UserEditViewModel viewModel, Clan user)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        // Initialize the view model when the page loads
        Loaded += async (s, e) =>
        {
            await _viewModel.Initialize(user);
        };
    }
}