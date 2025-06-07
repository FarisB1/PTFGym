using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;

namespace MobilnaAplikacija.ViewModels
{
    public class TerminDetailsViewModel : INotifyPropertyChanged
    {
        private readonly ITerminService _terminService;
        private readonly IAuthService _authService;
        private long _terminId;
        private Termin _termin;
        private string _userRole;
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public Termin Termin
        {
            get => _termin;
            set => SetProperty(ref _termin, value);
        }

        public bool CanJoinTermin =>
            _termin != null &&
            _termin.trenutniBrojClanova < _termin.maksimalniBrojClanova &&
            !_userRole.Equals("Trener", StringComparison.OrdinalIgnoreCase);

        public bool CanLeaveTermin =>
            _termin != null &&
            !_userRole.Equals("Trener", StringComparison.OrdinalIgnoreCase);

        public bool CanDeleteTermin =>
            _termin != null &&
            _userRole.Equals("Trener", StringComparison.OrdinalIgnoreCase);

        public ICommand JoinTerminCommand { get; }
        public ICommand LeaveTerminCommand { get; }
        public ICommand DeleteTerminCommand { get; }

        public TerminDetailsViewModel(ITerminService terminService, IAuthService authService, long terminId)
        {
            _terminService = terminService;
            _authService = authService;
            _terminId = terminId;

            JoinTerminCommand = new Command(async () => await JoinTermin());
            LeaveTerminCommand = new Command(async () => await LeaveTermin());
            DeleteTerminCommand = new Command(async () => await DeleteTermin());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public async Task LoadTerminDetails()
        {
            try
            {
                IsBusy = true;
                _userRole = await _authService.GetUserRole();
                var termini = await _terminService.GetAllTermini();
                Termin = termini.FirstOrDefault(t => t.id == _terminId);

                if (Termin == null)
                {
                    await Shell.Current.DisplayAlert("Greška", "Termin nije pronađen", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Greška", $"Greška pri učitavanju detalja: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task JoinTermin()
        {
            try
            {
                bool success = await _terminService.JoinTermin((int)_terminId);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Uspjeh", "Pridružili ste se terminu", "OK");
                    await LoadTerminDetails();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Greška", "Nije moguće pridružiti se terminu", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Greška", $"Greška prilikom pridruživanja: {ex.Message}", "OK");
            }
        }

        private async Task LeaveTermin()
        {
            try
            {
                bool success = await _terminService.LeaveTermin((int)_terminId);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Uspjeh", "Napustili ste termin", "OK");
                    await LoadTerminDetails();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Greška", "Nije moguće napustiti termin", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Greška", $"Greška prilikom napuštanja: {ex.Message}", "OK");
            }
        }

        private async Task DeleteTermin()
        {
            try
            {
                bool confirm = await Shell.Current.DisplayAlert(
                    "Potvrda",
                    "Jeste li sigurni da želite obrisati ovaj termin?",
                    "Da",
                    "Ne");

                if (confirm)
                {
                    bool success = await _terminService.DeleteTermin((int)_terminId);
                    if (success)
                    {
                        await Shell.Current.DisplayAlert("Uspjeh", "Termin je obrisan", "OK");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Greška", "Nije moguće obrisati termin", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Greška", $"Greška prilikom brisanja: {ex.Message}", "OK");
            }
        }
    }
}