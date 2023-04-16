using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace anuR.Services.User;

public interface IUserService
{
    Task<ActionResult<IEnumerable<Models.User>>> ListUsers();
    Task<ActionResult<Models.User>> CreateUser(Models.User user);
    
    Task<ActionResult<Models.User>> GetUser(Guid id);
    
    Task<ActionResult<Models.User>> UpdateUser(Guid id, Models.DTO.User user);
    
    Task<ActionResult<Models.User>> DeleteUser(Guid id);
    
    Models.User GetMe(string token);
}