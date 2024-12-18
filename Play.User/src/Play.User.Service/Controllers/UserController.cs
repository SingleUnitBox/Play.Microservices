using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Context;
using Play.User.Core.Auth;
using Play.User.Core.DTO;

namespace Play.User.Service.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IContext _context;

    public UserController(IUserService userService, 
        IContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpPost("signUp")]
    public async Task<ActionResult> Post(SignUpDto dto)
    {
        await _userService.SignUp(dto);
        return Ok();
    }

    [HttpPost("signIn")]
    public async Task<ActionResult<JwtToken>> Post(SignInDto dto)
    {
        return Ok(await _userService.SignIn(dto));
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<AccountDto>> Get()
    {
        var accountDto = await _userService.GetAccount(_context.IdentityContext.UserId);
        if (accountDto == null)
        {
            return NotFound();
        }
        
        return Ok(accountDto);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangeUsername(ChangeUsernameDto dto)
    {
        await _userService.ChangeUsername(_context.IdentityContext.UserId, dto);
        return NoContent();
    }
}