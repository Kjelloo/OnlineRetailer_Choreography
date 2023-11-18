using FeatureHubSDK;
using Microsoft.AspNetCore.Mvc;
using SharedModels.User;
using UserApi.Core.Service;

namespace UserApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly bool _loginFeature;

    public LoginController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
        
        var config = new EdgeFeatureHubConfig("http://featurehub:8888",
            "e84d6a9c-9f98-4a3a-bda3-ec8c46b2c5d4/VcLpJlnYsxClpKOQQ7LlTaV3HNWwEAXk3oGNNw6c");
        var fh = config.NewContext().Build().Result;
        
        _loginFeature = fh["LoginFeature"].IsEnabled;
    }
    
    [HttpPost]
    public ActionResult<TokenDto> Login([FromBody] AuthDto dto)
    {
        try
        {
            if (!_loginFeature)
            {
                return BadRequest("Login feature is disabled");
            }
            
            var user = _userService.GetByUsername(dto.Username);
        
            if (user != null && _authService.VerifyPassword(dto.Password, user))
            {
                return Ok(new TokenDto
                {
                    JWT = _authService.GenerateToken(user), 
                    User = new UserDto { Id = user.Id, Username = user.Username, CustomerId = user.CustomerId}
                });
            }

            return Unauthorized();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}

