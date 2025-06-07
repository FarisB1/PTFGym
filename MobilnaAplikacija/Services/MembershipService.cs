using MobilnaAplikacija.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobilnaAplikacija.Services
{
    public class MembershipService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";

        public MembershipService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClanarinaDto>> GetMemberships(int clanId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Home/GetClanClanarina/{clanId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ClanarinaDto>>();
            }
            return new List<ClanarinaDto>();
        }

        public async Task<bool> RenewMembership(int clanId)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/Home/RenewMembershipApi/{clanId}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
