using SharedModels;
using SharedModels.User;
using UserApi.Core.Models;

namespace UserApi.Core.Service;

public interface IUserService : IService<User>
{
    User GetByUsername(string username);
    Task<int> CreateCustomerEquivalent(RegisterUserDto dto);
}