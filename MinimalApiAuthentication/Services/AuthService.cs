
using Microsoft.AspNetCore.Identity;
using MinimalApiAuthentication.Models;

namespace MinimalApiAuthentication.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthResult()
            {
                Errors = new List<string>() { "Invalid email or password" }
            };
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (isValidPassword == true)
        {
            return new AuthResult()
            {
                Succeeded = true,
                Token = _tokenService.GenerateToken(email)
            };
        }

        return new AuthResult()
        {
            Errors = new List<string>() { "Invalid email or password" }
        };
    }

    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        var user = new ApplicationUser() { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return new AuthResult()
            {
                Succeeded = true,
                Token = _tokenService.GenerateToken(email)
            };
        }

        return new AuthResult()
        {
            Errors = result.Errors.Select(x => x.Description)
        };
    }
}
