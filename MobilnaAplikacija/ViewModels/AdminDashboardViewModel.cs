using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Dto;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using MobilnaAplikacija.Services;
using MobilnaAplikacija.Views;

namespace MobilnaAplikacija.ViewModels
{
    public partial class AdminDashboardViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly MembershipService _membershipService;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";

        [ObservableProperty]
        private ObservableCollection<Clan> users;

        [ObservableProperty]
        private ObservableCollection<ClanarinaDto> memberships;

        [ObservableProperty]
        private Clan selectedUser;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        public AdminDashboardViewModel(
            IAuthService authService,
            MembershipService membershipService,
            HttpClient httpClient,
            IServiceProvider serviceProvider)
        {
            _authService = authService;
            _membershipService = membershipService;
            _httpClient = httpClient;
            _serviceProvider = serviceProvider;
            Users = new ObservableCollection<Clan>();
            Memberships = new ObservableCollection<ClanarinaDto>();
        }

        [RelayCommand]
        private async Task EditUserAsync(Clan user)
        {
            if (user == null) return;  // You already have this check, which is good

            var userEditViewModel = _serviceProvider.GetRequiredService<UserEditViewModel>();
            var userEditPage = new UserEditPage(userEditViewModel, user);
            await Shell.Current.Navigation.PushAsync(userEditPage);
        }

        [RelayCommand]
        public async Task LoadUsersAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var response = await _httpClient.GetAsync($"{_baseUrl}/Home/GetAllClanovi");
                if (response.IsSuccessStatusCode)
                {
                    var userList = await response.Content.ReadFromJsonAsync<List<Clan>>();
                    Users.Clear();
                    foreach (var user in userList)
                    {
                        Users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading users: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        


        [RelayCommand]
        private async Task UpdateUserAsync(Clan user)
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/Home/UpdateClan", user);
                if (response.IsSuccessStatusCode)
                {
                    await LoadUsersAsync();
                    await Shell.Current.DisplayAlert("Uspješno", "Korisnik uspješno promijenjen", "OK");
                }
                else
                {
                    throw new Exception("Greška pri promjneni korisnika");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating user: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", ErrorMessage, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}