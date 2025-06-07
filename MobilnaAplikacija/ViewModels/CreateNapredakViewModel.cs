using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;
using System.Collections.ObjectModel;

namespace MobilnaAplikacija.ViewModels
{
    public partial class CreateNapredakViewModel : ObservableObject
    {
        private readonly INapredakService _napredakService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<Clan> clanovi;

        [ObservableProperty]
        private Clan selectedClan;

        [ObservableProperty]
        private string tezina;

        [ObservableProperty]
        private string biljeske;

        [ObservableProperty]
        private DateTime datumUnosa;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private string errorMessage;


        public CreateNapredakViewModel(INapredakService napredakService, IAuthService authService)
        {
            _napredakService = napredakService;
            _authService = authService;
            DatumUnosa = DateTime.Now;
            Clanovi = new ObservableCollection<Clan>();
        }

        [RelayCommand]
        public async Task LoadClanovi()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                HasError = false;
                ErrorMessage = string.Empty;

                // Here you would need to implement GetClanoviForTrener in your service
                var clanoviList = await _napredakService.GetAllClans();
                Clanovi.Clear();
                foreach (var clan in clanoviList)
                {
                    Clanovi.Add(clan);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Save()
        {
            if (IsBusy)
                return;

            if (SelectedClan == null)
            {
                HasError = true;
                ErrorMessage = "Molimo Vas da izaberite korisnika";
                return;
            }

            if (string.IsNullOrWhiteSpace(Tezina) || !double.TryParse(Tezina, out double weight))
            {
                HasError = true;
                ErrorMessage = "Molimo Vas da uneseste validnu težinu";
                return;
            }

            try
            {
                IsBusy = true;
                HasError = false;
                ErrorMessage = string.Empty;

                var napredak = new Napredak
                {
                    datumUnosa = DatumUnosa,
                    tezina = weight,
                    biljeske = Biljeske ?? string.Empty,
                    clanIme = SelectedClan.ime
                };

                // Here you would need to implement CreateNapredak in your service
                await _napredakService.CreateNapredak(napredak);

                // Navigate back or show success message
                await Application.Current.MainPage.DisplayAlert("Napredak", "Uspješno dodan napredak.", "Ok");
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}