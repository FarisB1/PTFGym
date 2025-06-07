using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Dto;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using MobilnaAplikacija.Services;
using System.Diagnostics;

namespace MobilnaAplikacija.ViewModels
{
    public partial class UserEditViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly MembershipService _membershipService;
        private readonly ITrenerService _trenerService;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";

        [ObservableProperty]
        private Clan currentUser;

        [ObservableProperty]
        private ObservableCollection<ClanarinaDto> memberships;

        [ObservableProperty]
        private ObservableCollection<Trener> availableTreners;

        [ObservableProperty]
        private Trener selectedTrener;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private List<string> availableRoles;

        [ObservableProperty]
        private string selectedRole;


        public UserEditViewModel(HttpClient httpClient, MembershipService membershipService, ITrenerService trenerService)
        {
            _httpClient = httpClient;
            _membershipService = membershipService;
            _trenerService = trenerService;
            Memberships = new ObservableCollection<ClanarinaDto>();
            AvailableTreners = new ObservableCollection<Trener>();

        }

        public async Task Initialize(Clan user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            CurrentUser = user;
            await LoadUserData();
        }

        private async Task LoadUserData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var membershipTask = _membershipService.GetMemberships(CurrentUser.id);
                var trenersTask = _trenerService.GetAllTreneri();

                await Task.WhenAll(membershipTask, trenersTask);

                Memberships.Clear();
                foreach (var membership in await membershipTask)
                {
                    Memberships.Add(membership);
                }

                AvailableTreners.Clear();
                foreach (var trener in await trenersTask)
                {
                    AvailableTreners.Add(trener);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Greška pri učitavanju korisnika: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        private async Task SaveUserAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                Debug.Write($"CurrentUser ID: {CurrentUser?.id}");
                Debug.Write($"CurrentUser Name: {CurrentUser?.ime}");

                

                // Update user details
                Debug.Write($"Ime USER: {CurrentUser.ime}");
                Debug.Write($"id USER: {CurrentUser.id}");
                Debug.Write($"email USER: {CurrentUser.email}");

                var updateResponse = await _httpClient.PutAsJsonAsync($"{_baseUrl}/Home/UpdateClan", CurrentUser);
                var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
                Debug.Write($"Update User Response: {updateResponseContent}");

                if (updateResponse.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Success", "Korisnik uspješno promijenjen", "OK");
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    throw new Exception($"Failed to update user: {updateResponseContent}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Debug.Write($"Exception: {ex}");
                await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task RenewMembershipAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var success = await _membershipService.RenewMembership(CurrentUser.id);
                if (success)
                {
                    await LoadUserData(); // Refresh memberships
                    await Shell.Current.DisplayAlert("Success", "Uspješno obnovljena članarina", "OK");
                }
                else
                {
                    throw new Exception("Greška pri obnovi članarine");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Greška pri obnovi članarine: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}