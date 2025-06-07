using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;
using MobilnaAplikacija.ViewModels;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MobilnaAplikacija.Views.Termini
{
    public partial class TerminTreneri : ContentPage
    {
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;

        public TerminTreneri(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _httpClient = new HttpClient();
            BindingContext = new TerminTrenerViewModel(authService);
            LoadTreneri();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ((TerminTrenerViewModel)BindingContext).LoadTerminiAsync();
        }

        private async void LoadTreneri()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Trener>>("http://farisbrkic-001-site1.anytempurl.com/Home/GetAllTreneri");
                TrenerPicker.ItemsSource = response;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Greška", $"Greška pri učitavanju trenera {ex}", "OK");
            }
        }

        private async void OnCreateTerminClicked(object sender, EventArgs e)
        {
            // Create the Termin object with lowercase property names
            var termin = new Termin
            {
                datumVrijeme = DatumPicker.Date.Add(VrijemePicker.Time),
                vrstaTreninga = VrstaTreningaEntry.Text,
                maksimalniBrojClanova = int.Parse(MaksimalniBrojClanovaEntry.Text),
                trenerId = ((Trener)TrenerPicker.SelectedItem).id
            };

            try
            {
                // Configure JsonSerializerOptions to use uppercase property names
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new UpperCaseNamingPolicy(), // Custom naming policy
                    WriteIndented = true // Optional: Makes the JSON more readable
                };

                // Serialize the Termin object to JSON with uppercase property names
                var json = JsonSerializer.Serialize(termin, jsonOptions);
                Console.WriteLine($"JSON Payload: {json}");

                // Send the JSON payload to the server
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://farisbrkic-001-site1.anytempurl.com/TestCreate", content);

                // Log the response for debugging
                Console.WriteLine($"RESPONSE: {response}");

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Uspjeh", "Termin je uspješno kreiran", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    // Log the error response for debugging
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Response: {errorResponse}");

                    await DisplayAlert("Greška", "Greška pri kreiranju termina", "OK");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception: {ex}");

                await DisplayAlert("Greška", "Greška pri slanju zahtjeva", "OK");
            }
        }

        private async void OnTerminDetailsClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is long terminId)
            {
                await Shell.Current.GoToAsync($"TerminDetails?TerminId={terminId}");
            }
        }
    }
}