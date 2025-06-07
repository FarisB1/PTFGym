using MobilnaAplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilnaAplikacija.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequest loginRequest);
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
        Task LogoutAsync();
        Task<string> GetCurrentUserId();
        Task<string> GetCurrentClanId();
        Task<string> GetUserRole();
    }
}
