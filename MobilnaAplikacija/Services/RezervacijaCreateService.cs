using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using MobilnaAplikacija.Models;
using System.Diagnostics;

namespace MobilnaAplikacija.Services
{
    public class RezervacijaCreateService : IRezervacijaCreateService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";
        private readonly string _username = "farisbrkic-001";
        private readonly string _password = "Zenica122!";
        private readonly IAuthService _authService;

        public RezervacijaCreateService(IAuthService authService)
        {
            _client = new HttpClient();
            SetupBasicAuthentication();
            _authService = authService;
        }

        private void SetupBasicAuthentication()
        {
            var authenticationString = $"{_username}:{_password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        }

        public async Task<List<Trener>> GetTreneri()
        {
            var response = await _client.GetAsync($"{_baseUrl}/Home/GetAllTreneri");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Trener>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Trener>();
            }
            throw new Exception("Failed to retrieve trainers");
        }

        public async Task<bool> CreateRezervacija(int trenerId, DateTime datumRezervacije)
        {
            try
            {
                var clanId = await _authService.GetCurrentClanId();
                var rezervacija = new
                {
                    ClanId = clanId,
                    TrenerId = trenerId,
                    DatumRezervacije = datumRezervacije
                };

                Debug.Write($"ClanID: {rezervacija.ClanId}");
                Debug.Write($"TrenerID: {rezervacija.TrenerId}");
                Debug.Write($"DatumRezervacije: {rezervacija.DatumRezervacije}");

                var json = JsonSerializer.Serialize(rezervacija);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{_baseUrl}/Home/CreateRezervacijaAPI", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw new Exception("Failed to create reservation");
            }
        }

    }
}