using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdentityServer.Data;
using MyIdentityServer.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyIdentityServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _users;

        public UsersController(IUser users)
        {
            _users = users;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            return _users.GetAllUsers();
        }

        // POST api/<UserController>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Registration(UserCreateViewModel user)
        {
            try
            {
                await _users.Registration(user);
                return Ok($"Registrasi user {user.UserName} berhasil");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserViewModel>> Authenticate(UserCreateViewModel createUser)
        {
            try
            {
                var user = await _users.Authenticate(createUser.UserName, createUser.Password);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
