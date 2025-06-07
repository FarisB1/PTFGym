using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;

namespace MobilnaAplikacija.ViewModels
{
    public partial class NapredakViewModel : ObservableObject
    {
        private readonly INapredakService _napredakService;

        [ObservableProperty]
        private List<Napredak> napredaks;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasError;

        public NapredakViewModel(INapredakService napredakService)
        {
            _napredakService = napredakService;
            napredaks = new List<Napredak>();
        }

        [RelayCommand]
        public async Task LoadNapredakAsync()
        {
            try
            {
                HasError = false;
                ErrorMessage = string.Empty;
                IsLoading = true;

                Napredaks = await _napredakService.GetAllNapredak();
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