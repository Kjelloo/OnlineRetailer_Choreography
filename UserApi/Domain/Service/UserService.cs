using FeatureHubSDK;
using Newtonsoft.Json;
using RestSharp;
using SharedModels;
using SharedModels.Customer;
using SharedModels.Helpers;
using SharedModels.User;
using UserApi.Core.Models;
using UserApi.Core.Service;
using UserApi.Domain.Repositories;

namespace UserApi.Domain.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly RestClient _restClient;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
        _restClient = new RestClient(RestConnectionHelper.GetCustomerUrl());
    }

    public User Add(User entity)
    {
        return _repository.Add(entity);
    }

    public User Get(int id)
    {
        var user = _repository.Get(id);
        
        if (user is null) throw new Exception("User does not exist");
        
        return user;
    }

    public IEnumerable<User> GetAll()
    {
        return _repository.GetAll();
    }

    public User Edit(User entity)
    {
        return _repository.Edit(entity);
    }

    public User Remove(User entity)
    {
        return _repository.Remove(entity);
    }

    public User GetByUsername(string username)
    {
        return _repository.FindByUsername(username);
    }

    public async Task<int> CreateCustomerEquivalent(RegisterUserDto dto)
    {
        var newCustomer = new CustomerDto
        {
            Email = dto.Email,
            Name = dto.Name,
            Phone = dto.Phone,
            BillingAddress = dto.BillingAddress,
            ShippingAddress = dto.BillingAddress,
            CreditStanding = 500
        };

        // Send customer to customer api
        var jsonBody = JsonConvert.SerializeObject(newCustomer);
        
        var request = new RestRequest();
        
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(jsonBody);
        var response = await _restClient.PostAsync<int>(request);
        
        if (response is 0)
        {
            throw new Exception("Customer could not be created");
        }

        return response;
    }
}