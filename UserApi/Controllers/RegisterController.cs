using FeatureHubSDK;
using Microsoft.AspNetCore.Mvc;
using SharedModels.User;
using UserApi.Core.Models;
using UserApi.Core.Service;

namespace UserApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly bool _registerFeature;

    public RegisterController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
        var config = new EdgeFeatureHubConfig("http://featurehub:8085",
            "e84d6a9c-9f98-4a3a-bda3-ec8c46b2c5d4/VcLpJlnYsxClpKOQQ7LlTaV3HNWwEAXk3oGNNw6c");
        var fh = config.NewContext().Build().Result;
        
        _registerFeature = fh["RegisterFeature"].IsEnabled;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            if (!_registerFeature)
            {
                return BadRequest("Register feature is disabled");
            }
            
            var user = _userService.GetByUsername(dto.Username);
        
            if (user != null)
            {
                return BadRequest("User with username already exists");
            }

            var customerId = await _userService.CreateCustomerEquivalent(dto);

            var hashPassword = _authService.HashPassword(dto.Password);
        
            var newUser = new User
            {
                Username = dto.Username, 
                CustomerId = customerId,
                Password = hashPassword
            };

            newUser = _userService.Add(newUser);
        
            if (newUser != null)
            {
                return Ok();
            }
        
            return BadRequest("Something went wrong");
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}

