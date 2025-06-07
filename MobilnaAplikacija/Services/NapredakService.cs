using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MobilnaAplikacija.Models;

namespace MobilnaAplikacija.Services
{
    public interface INapredakService
    {
        Task<List<Napredak>> GetAllNapredak();
        Task<bool> CreateNapredak(Napredak napredak);
        Task<List<Clan>> GetAllClans();
    }

    public class NapredakService : INapredakService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";
        private readonly string _username = "farisbrkic-001";
        private readonly string _password = "Zenica122!";
        private readonly IAuthService _authService;

        public NapredakService(IAuthService authService)
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

        public async Task<List<Napredak>> GetAllNapredak()
        {
            try
            {
                string role = await _authService.GetUserRole();
                Console.WriteLine($"Role Napredak: {role}");
                string endpoint;

                if (role.Equals("Trener", StringComparison.OrdinalIgnoreCase))
                {
                    var clanId = await _authService.GetCurrentClanId();
                    Console.WriteLine($"Role Napredak: {clanId}");
                    endpoint = $"{_baseUrl}/Home/GetTrenerNapredak/{clanId}";
                }
                else if (role.Equals("Clan", StringComparison.OrdinalIgnoreCase))
                {
                    var clanId = await _authService.GetCurrentClanId();
                    Console.WriteLine($"Role Napredak: {clanId}");
                    endpoint = $"{_baseUrl}/Home/GetClanNapredak/{clanId}";
                }
                else
                {
                    endpoint = $"{_baseUrl}/Home/GetAllNapredaks";
                }

                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var napredaks = JsonSerializer.Deserialize<List<Napredak>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return napredaks ?? new List<Napredak>();
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get napredak: {ex.Message}");
            }
        }

        public async Task<bool> CreateNapredak(Napredak napredak)
        {
            try
            {
                var trenerId = await _authService.GetCurrentClanId();
                if (trenerId == null)
                {
                    throw new Exception("Trainer ID not found");
                }

                var clanId = await GetClanIdByName(napredak.clanIme);

                Debug.Write($"ClanID For Napredak: {clanId}");
                if (clanId <= 0)
                {
                    throw new Exception($"Invalid clan ID for name: {napredak.clanIme}");
                }
                var requestObj = new
                {
                    ClanId = clanId,
                    TrenerId = trenerId,
                    DatumUnosa = napredak.datumUnosa,
                    Tezina = napredak.tezina,
                    Biljeske = napredak.biljeske
                };


                Debug.Write($"ClanID For Napredak: {requestObj.ClanId}");
                Debug.Write($"TrenerId For Napredak: {requestObj.TrenerId}");
                Debug.Write($"DatumUnosa For Napredak: {requestObj.DatumUnosa}");
                Debug.Write($"Tezina For Napredak: {requestObj.Tezina}");
                Debug.Write($"Biljeske For Napredak: {requestObj.Biljeske}");

                var json = JsonSerializer.Serialize(requestObj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Update the endpoint to match the API
                var response = await _client.PostAsync($"{_baseUrl}/Home/CreateNapredakAPI", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create napredak: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create napredak: {ex.Message}");
            }
        }

        public async Task<List<Clan>> GetAllClans()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/Home/GetAllClans");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var clanovi = JsonSerializer.Deserialize<List<Clan>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return clanovi ?? new List<Clan>();
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get clanovi: {ex.Message}");
            }
        }

        private async Task<int> GetClanIdByName(string clanIme)
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/Home/GetClanIdByName/{clanIme}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<int>(content);
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get clan ID: {ex.Message}");
            }
        }
    }
}