using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIdentityServer.Helpers;
using MyIdentityServer.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyIdentityServer.Data
{
    public class UserRepository : IUser
    {
        private readonly UserManager<IdentityUser> _userManager;
        private AppSettings _appSettings;

        public UserRepository(UserManager<IdentityUser> userManager,IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        public async Task<UserViewModel> Authenticate(string username, string password)
        {
            var currUser = await _userManager.FindByNameAsync(username);
            var userResult = await _userManager.CheckPasswordAsync(currUser, password);
            if (!userResult)
                throw new Exception($"Authentication Failed!");
            var user = new UserViewModel
            {
                UserName = username,
            };

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var users = new List<UserViewModel>();
            foreach(var user in _userManager.Users)
            {
                users.Add(new UserViewModel { UserName = user.UserName});
            }
            return users;
        }

        public async Task Registration(UserCreateViewModel createUser)
        {
            try
            {
                var newUser = new IdentityUser
                {
                    UserName = createUser.UserName,
                    Email = createUser.UserName
                };
                var result = await _userManager.CreateAsync(newUser, createUser.Password);
                if (!result.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    var errors = result.Errors;
                    foreach(var error in errors){
                        sb.Append($"{error.Code}-{error.Description}");
                    }
                    throw new Exception($"Error: {sb.ToString()}");
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error : {ex.Message}");
            }
        }
    }
}
