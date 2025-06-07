using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Dto;
using MobilnaAplikacija.Services;
using System.Diagnostics;

namespace MobilnaAplikacija.ViewModels
{
    public class MembershipViewModel : ObservableObject
    {
        private readonly MembershipService _membershipService;
        private readonly IAuthService _authService;
        private ClanarinaDto _activeMembership;
        private List<ClanarinaDto> _membershipHistory;
        private bool _isBusy;

        public MembershipViewModel(MembershipService membershipService, IAuthService authService)
        {
            _membershipService = membershipService;
            _authService = authService;
            RenewCommand = new AsyncRelayCommand(RenewMembershipAsync);
            LoadDataAsync();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool HasActiveMembership => _activeMembership != null;
        public DateTime StartDate => _activeMembership?.DatumPocetka ?? DateTime.MinValue;
        public DateTime EndDate => _activeMembership?.DatumZavrsetka ?? DateTime.MinValue;
        public float Amount => _activeMembership?.Iznos ?? 0;
        public bool CanRenew => _activeMembership?.CanRenew ?? false;

        public List<ClanarinaDto> MembershipHistory
        {
            get => _membershipHistory;
            set => SetProperty(ref _membershipHistory, value);
        }

        public IAsyncRelayCommand RenewCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;
                var clanId = await _authService.GetCurrentClanId();
                if (string.IsNullOrEmpty(clanId))
                {
                    await Shell.Current.DisplayAlert("Error", "Korisnik nije pronađen", "OK");
                    return;
                }
                Debug.WriteLine($"ClanID for membership: {clanId}");
                var memberships = await _membershipService.GetMemberships(int.Parse(clanId));
                _activeMembership = memberships
                                    .Where(m => m.DatumZavrsetka >= DateTime.Now)
                                    .OrderByDescending(m => m.DatumZavrsetka)
                                    .FirstOrDefault();

                MembershipHistory = memberships.OrderByDescending(m => m.DatumZavrsetka).ToList();

                OnPropertyChanged(nameof(HasActiveMembership));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(CanRenew));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Greška pri učitavanju podataka korisnika", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RenewMembershipAsync()
        {
            try
            {
                IsBusy = true;
                var clanId = await _authService.GetCurrentClanId();
                if (string.IsNullOrEmpty(clanId))
                {
                    await Shell.Current.DisplayAlert("Error", "Korisnik nije pronađen", "OK");
                    return;
                }

                if (await _membershipService.RenewMembership(int.Parse(clanId)))
                {
                    await LoadDataAsync();
                    await Shell.Current.DisplayAlert("Success", "Uspješno obnovljena članarina", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Greška pri obnavljanju članarine", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Desila se greška", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}