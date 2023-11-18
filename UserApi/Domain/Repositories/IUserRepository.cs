using SharedModels;
using UserApi.Core.Models;

namespace UserApi.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    User FindByUsername(string username);
}