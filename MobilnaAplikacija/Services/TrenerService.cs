using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using MobilnaAplikacija.Models;

namespace MobilnaAplikacija.Services
{
    public interface ITrenerService
    {
        Task<List<Trener>> GetAllTreneri();
        Task<Trener> GetTrenerById(int id);
    }

    public class TrenerService : ITrenerService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";
        private readonly string _username = "farisbrkic-001";
        private readonly string _password = "Zenica122!";

        public TrenerService()
        {
            _client = new HttpClient();
            SetupBasicAuthentication();
        }

        private void SetupBasicAuthentication()
        {
            var authenticationString = $"{_username}:{_password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        }

        public async Task<List<Trener>> GetAllTreneri()
        {
            try
            {
                var endpoint = $"{_baseUrl}/Home/GetAllTreneri";
                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var treneri = JsonSerializer.Deserialize<List<Trener>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return treneri ?? new List<Trener>();
                }

                throw new Exception($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get treneri: {ex.Message}");
            }
        }

        public async Task<Trener> GetTrenerById(int trenerId)
        {
            try
            {
                var endpoint = $"{_baseUrl}/Home/GetTrainerDetails/{trenerId}";
                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var trener = JsonSerializer.Deserialize<Trener>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return trener ?? throw new Exception("Trener nije pronađen.");
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new Exception("Trener nije pronađen.");

                throw new Exception($"Greška: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Neuspješno dobijanje trenera: {ex.Message}");
            }
        }

    }
}