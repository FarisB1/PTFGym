using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services; 

namespace MobilnaAplikacija.ViewModels
{
    public partial class TerminTrenerViewModel : ObservableObject
    {
        private readonly ITerminService _terminService;

        [ObservableProperty]
        private List<Termin> termini;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasError;

        public TerminTrenerViewModel(IAuthService authService)
        {
            _terminService = new TerminService(authService);
            termini = new List<Termin>();
        }

        [RelayCommand]
        public async Task LoadTerminiAsync()
        {
            try
            {
                HasError = false;
                ErrorMessage = string.Empty;
                IsLoading = true;

                // This will use the method in HomeController that gets termins for a specific trainer
                Termini = await _terminService.GetAllTerminiForTrainer();
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}