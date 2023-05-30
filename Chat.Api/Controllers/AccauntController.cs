using Identity.Core.Managers;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Identity.Core.Providers;

namespace Chat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccauntController : ControllerBase
{
    private readonly UserManager _userManager;
    private ILogger<AccauntController> _logger;
    private readonly UserProvider _userProvider;

   
    public AccauntController(UserManager userManager, ILogger<AccauntController> logger, UserProvider userProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _userProvider = userProvider;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody]CreateUserModel createUserModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.Register(createUserModel);
        return Ok(new UserModel(user));
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]LoginUserModel loginUserModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _userManager.Login(loginUserModel);
        return Ok(new {Token = token});
    }


    [HttpGet("Profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await _userManager.GetUser(userId);
        if (user == null)
        {
            return Unauthorized();
        }
        return Ok(new UserModel(user));
    }

    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetUser(string username)
    {
       var user = await _userManager.GetUser(username);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(new UserModel(user));
    }

}
