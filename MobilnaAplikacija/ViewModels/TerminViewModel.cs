using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;
using System.Windows.Input;

namespace MobilnaAplikacija.ViewModels
{
    public partial class TerminViewModel : ObservableObject
    {
        public ICommand NavigateToTerminDetailsCommand { get; }

        private readonly ITerminService _terminService;

        [ObservableProperty]
        private List<Termin> termini;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasError;

        public TerminViewModel(ITerminService terminService)
        {
            _terminService = terminService;
            termini = new List<Termin>();
            NavigateToTerminDetailsCommand = new Command<long>(async (terminId) =>
        await Shell.Current.GoToAsync($"TerminDetails?TerminId={terminId}"));
        }

        [RelayCommand]
        public async Task LoadTerminiAsync()
        {
            try
            {
                HasError = false;
                ErrorMessage = string.Empty;
                IsLoading = true;

                Termini = await _terminService.GetAllTermini();
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

        [RelayCommand]
        public async Task ToggleEnrollment(long terminId)
        {
            try
            {
                IsLoading = true;
                var termin = Termini.FirstOrDefault(t => t.id == terminId);
                if (termin == null) return;

                bool success = termin.IsUserEnrolled
                    ? await _terminService.LeaveTermin((int)terminId)
                    : await _terminService.JoinTermin((int)terminId);

                if (success)
                {
                    await LoadTerminiAsync();
                }
                else
                {
                    HasError = true;
                    ErrorMessage = "Greška pri pridruživanju termina, moguće je da ste već zapisani u terminu.";
                }
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
