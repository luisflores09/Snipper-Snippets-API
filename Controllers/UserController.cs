using Microsoft.AspNetCore.Mvc;
using Snipper_Snippets_API.Models;
using Snipper_Snippets_API.Services;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AuthService _authService;

    public UserController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = _authService.CreateUser(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetUser()
    {
        var user = HttpContext.Items["User"] as UserResponse;
        return user != null ? Ok(user) : Unauthorized();
    }
}