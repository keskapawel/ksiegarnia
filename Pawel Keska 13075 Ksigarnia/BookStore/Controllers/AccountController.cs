using BookStore.DTOModels;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;


    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody]UserRegisterDto dto)
    {
        _userService.RegisterUser(dto);
        return Ok();
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody]UserLoginDto dto)
    {
        string result = _userService.Login(dto);
        return Ok(result);
    }
}