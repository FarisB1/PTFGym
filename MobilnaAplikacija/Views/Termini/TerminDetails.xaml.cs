using MobilnaAplikacija.Services;
using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views.Termini;

[QueryProperty(nameof(TerminId), "TerminId")]
public partial class TerminDetails : ContentPage
{
    private long _terminId;
    private readonly ITerminService _terminService;
    private readonly IAuthService _authService;

    public long TerminId
    {
        get => _terminId;
        set
        {
            _terminId = value;
            LoadViewModel();
        }
    }

    public TerminDetails(ITerminService terminService, IAuthService authService)
    {
        InitializeComponent();
        _terminService = terminService;
        _authService = authService;
    }

    private void LoadViewModel()
    {
        BindingContext = new TerminDetailsViewModel(_terminService, _authService, _terminId);
    }

    public async Task NavigateBack()
    {
        try
        {
            await Shell.Current.GoToAsync("//Termini");
        }
        catch
        {
            await Shell.Current.GoToAsync("..");
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TerminDetailsViewModel viewModel)
        {
            await viewModel.LoadTerminDetails();
        }
    }
}