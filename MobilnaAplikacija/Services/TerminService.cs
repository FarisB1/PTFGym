using System.Net.Http.Headers;
using System.Text.Json;
using MobilnaAplikacija.Models;

namespace MobilnaAplikacija.Services
{
    public interface ITerminService
    {
        Task<List<Termin>> GetAllTermini();
        Task<bool> JoinTermin(int terminId);
        Task<bool> LeaveTermin(int terminId);
        Task<List<Termin>> GetAllTerminiForTrainer();
        Task<bool> DeleteTermin(int terminId);
        Task<List<Clan>> GetTerminClanovi(int terminId);
    }

    public class TerminService : ITerminService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";
        private readonly string _username = "farisbrkic-001";
        private readonly string _password = "Zenica122!";
        private readonly IAuthService _authService;

        public TerminService(IAuthService authService)
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

        public async Task<List<Termin>> GetAllTermini()
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
                    endpoint = $"{_baseUrl}/Home/GetAllTerminTrener/{clanId}";
                }
                else
                {
                    endpoint = $"{_baseUrl}/Home/GetAllTermini";
                }

                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var termini = JsonSerializer.Deserialize<List<Termin>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return termini ?? new List<Termin>();
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get termini: {ex.Message}");
            }
        }


        public async Task<bool> JoinTermin(int terminId)
        {
            try
            {
                var userId = await _authService.GetCurrentClanId();
                Console.WriteLine($"UserID: {userId}");
                Console.WriteLine($"TerminID: {terminId}");

                var response = await _client.PostAsync($"{_baseUrl}/Termins/JoinApi/{userId}/{terminId}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Termin>> GetAllTerminiForTrainer()
        {
            try
            {
                var trenerId = await _authService.GetCurrentClanId();
                Console.WriteLine($"Fetching termini for trainer ID: {trenerId}");

                string endpoint = $"{_baseUrl}/Home/GetAllTerminTrener/{trenerId}";

                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var termini = JsonSerializer.Deserialize<List<Termin>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return termini ?? new List<Termin>();
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get termini for trainer: {ex.Message}");
            }
        }

        public async Task<bool> LeaveTermin(int terminId)
        {
            try
            {
                var response = await _client.PostAsync($"{_baseUrl}/Home/LeaveTermin/{terminId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTermin(int terminId)
        {
            try
            {
                var response = await _client.PostAsync($"{_baseUrl}/Termins/DeleteApi/{terminId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Clan>> GetTerminClanovi(int terminId)
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/Home/GetTerminClanovi/{terminId}");

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
    }
}