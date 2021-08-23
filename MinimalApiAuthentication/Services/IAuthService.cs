
using MinimalApiAuthentication.Models;

namespace MinimalApiAuthentication.Services;
public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(string email, string password);
}
