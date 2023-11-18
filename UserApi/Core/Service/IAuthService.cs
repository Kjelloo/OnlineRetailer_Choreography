using UserApi.Core.Models;

namespace UserApi.Core.Service;

public interface IAuthService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, User user);
    string GenerateToken (User user);
}