using System.Net.Http.Headers;
using System.Text.Json;
using MobilnaAplikacija.Models;

namespace MobilnaAplikacija.Services
{
    public interface IRezervacijaService
    {
        Task<List<Rezervacija>> GetAllRezervacije();
    }

    public class RezervacijaService : IRezervacijaService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";
        private readonly string _username = "farisbrkic-001";
        private readonly string _password = "Zenica122!";
        private readonly IAuthService _authService;

        public RezervacijaService(IAuthService authService)
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

        public async Task<List<Rezervacija>> GetAllRezervacije()
        {
            try
            {

                string role = await _authService.GetUserRole();
                Console.WriteLine($"Role termini: {role}");
                string endpoint;

                if (role.Equals("Trener", StringComparison.OrdinalIgnoreCase))
                {
                    var clanId = await _authService.GetCurrentClanId();
                    Console.WriteLine($"Role termini: {clanId}");
                    endpoint = $"{_baseUrl}/Home/GetTrenerRezervacije/{clanId}";
                }
                else if (role.Equals("Clan", StringComparison.OrdinalIgnoreCase))
                {
                    var clanId = await _authService.GetCurrentClanId();
                    Console.WriteLine($"Role termini: {clanId}");
                    endpoint = $"{_baseUrl}/Home/GetClanRezervacije/{clanId}";
                }
                else
                {
                    endpoint = $"{_baseUrl}/Home/GetAllRezervacije";
                }

                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rezervacije = JsonSerializer.Deserialize<List<Rezervacija>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return rezervacije ?? new List<Rezervacija>();
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get rezervacije: {ex.Message}");
            }
        }
    }
}