using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;
using System.Collections.ObjectModel;

namespace MobilnaAplikacija.ViewModels
{
    public partial class CreateRezervacijaViewModel : ObservableObject
    {
        private readonly IRezervacijaCreateService _rezervacijaService;

        [ObservableProperty]
        private ObservableCollection<Trener> treneri;

        [ObservableProperty]
        private Trener selectedTrener;

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        public CreateRezervacijaViewModel(IRezervacijaCreateService rezervacijaService)
        {
            _rezervacijaService = rezervacijaService;
            Treneri = new ObservableCollection<Trener>();
        }

        [RelayCommand]
        public async Task LoadData()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var trenerList = await _rezervacijaService.GetTreneri();
                Treneri.Clear();
                foreach (var trener in trenerList)
                {
                    Treneri.Add(trener);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task CreateRezervacija()
        {
            if (SelectedTrener == null)
            {
                ErrorMessage = "Molimo Vas da izaberete trenera";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                bool success = await _rezervacijaService.CreateRezervacija((int)SelectedTrener.id, SelectedDate);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Uspješno", "Rezervacija uspješno kreirana", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    ErrorMessage = "Greška pri kreiranju rezervacije.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}