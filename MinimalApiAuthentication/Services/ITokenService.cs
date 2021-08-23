
namespace MinimalApiAuthentication.Services;
public interface ITokenService
{
    string GenerateToken(string email);
}
