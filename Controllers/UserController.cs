using anuR.Models.DTO;
using anuR.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// Get: /User
    /// <summary>
    ///     Get all user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin,HR,")]
    public async Task<ActionResult<IEnumerable<Models.User>>> GetUsers()
    {
        return await _userService.ListUsers();
    }


    /// Post: /User
    /// <summary>
    ///     Create a user.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Models.User>> CreateUser(Models.User userDTO)
    {
        var user = await _userService.CreateUser(userDTO);
        return user;
    }

    /// Get: /User/5
    /// <summary>
    ///     Get a specific user.
    /// </summary>
    /// <param Guid="Guid"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<ActionResult<Models.User>> Get(Guid id)
    {
        var user = await _userService.GetUser(id);
        return user;
    }
    
    /// Put: /User/5
    /// <summary>
    /// Update a user.
    /// </summary>
    /// <param Guid="Guid"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,HR,User")]
    public async Task<ActionResult<Models.User>> UpdateUser(Guid id, User userDTO)
    {
        //write in terminal to see the id
        var user = await _userService.UpdateUser(id, userDTO);
        return user;
    }
    
    /// Get: /me
    /// <summary>
    /// Get self user.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("me")]
    [Authorize(Roles = "Admin,HR,User")]
    public async Task<ActionResult<Models.User>> GetMe()
    {
        var user =  _userService.GetMe(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        return user;
    }
    
    /// Delete: /User/5
    /// <summary>
    /// Delete a user.
    /// </summary>
    /// <param Guid="Guid"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUser(id);
        return NoContent();
    }
}