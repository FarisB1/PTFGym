using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;

namespace MobilnaAplikacija.ViewModels
{
    public partial class RezervacijeViewModel : ObservableObject
    {
        private readonly IRezervacijaService _rezervacijaService;

        [ObservableProperty]
        private List<Rezervacija> rezervacije;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasError;

        public RezervacijeViewModel(IRezervacijaService rezervacijaService)
        {
            _rezervacijaService = rezervacijaService;
            rezervacije = new List<Rezervacija>();
        }

        [RelayCommand]
        public async Task LoadRezervacijeAsync()
        {
            try
            {
                HasError = false;
                ErrorMessage = string.Empty;
                IsLoading = true;

                Rezervacije = await _rezervacijaService.GetAllRezervacije();
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