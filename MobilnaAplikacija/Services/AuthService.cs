using MobilnaAplikacija.Models;
using MobilnaAplikacija.Services;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ISecureStorage _secureStorage;
    private readonly string _baseUrl = "http://farisbrkic-001-site1.anytempurl.com";
    private readonly string _username = "farisbrkic-001";
    private readonly string _password = "Zenica122!";

    public AuthService(HttpClient httpClient, ISecureStorage secureStorage)
    {
        _httpClient = httpClient;
        _secureStorage = secureStorage;
        // Configure basic authentication
        var authString = $"{_username}:{_password}";
        var base64Auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);

        // Set base address
        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public async Task<bool> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Account/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if(result.clanId == 0)
                {

                    await _secureStorage.SetAsync("clanId", result.clanId.ToString());
                    await _secureStorage.SetAsync("userName", "Admin");
                    await _secureStorage.SetAsync("userRole", "Admin");
                }
                else if (result != null)
                {
                    await _secureStorage.SetAsync("userId", result.userId);
                    await _secureStorage.SetAsync("clanId", result.clanId.ToString());
                    await _secureStorage.SetAsync("userEmail", result.email);
                    await _secureStorage.SetAsync("userName", result.ime); 
                    await _secureStorage.SetAsync("userRole", result.role);

                    Console.WriteLine($"User {result.ime} logged in with role {result.role}");
                }

                return true;
            }
            Debug.WriteLine($"Login failed with status: {response.StatusCode}");
            Debug.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");
            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }
    public async Task<string> GetUserRole()
    {
        return await _secureStorage.GetAsync("userRole") ?? "User";
    }


    public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Account/register", registerRequest);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Registration failed with status: {response.StatusCode}");
                Debug.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Registration error: {ex.Message}");
            return false;
        }
    }


    public async Task LogoutAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("/Account/Logout", null);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Logout failed with status: {response.StatusCode}");
                Debug.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Logout error: {ex.Message}");
            throw; // Rethrow to handle in UI
        }
    }
    public async Task<string> GetCurrentUserId()
    {
        return await _secureStorage.GetAsync("userId");
    }

    public async Task<string> GetCurrentClanId()
    {
        return await _secureStorage.GetAsync("clanId");
    }
}