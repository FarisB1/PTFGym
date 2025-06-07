using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;

namespace MobilnaAplikacija.ViewModels
{
    public partial class TreneriViewModel : ObservableObject
    {
        private readonly ITrenerService _trenerService;

        [ObservableProperty]
        private List<Trener> treneri;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasError;

        public TreneriViewModel(ITrenerService trenerService)
        {
            _trenerService = trenerService;
            treneri = new List<Trener>();
        }

        [RelayCommand]
        public async Task LoadTreneriAsync()
        {
            try
            {
                HasError = false;
                ErrorMessage = string.Empty;
                IsLoading = true;

                Treneri = await _trenerService.GetAllTreneri();
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