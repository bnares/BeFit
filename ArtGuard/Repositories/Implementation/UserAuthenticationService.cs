using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ArtGuard.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthenticationService(SignInManager<ApplicationUser> signInManager,
                                         UserManager<ApplicationUser> userManager,
                                         RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        public UserAuthenticationService()
        {

        }
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var statuss = new Status();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                statuss.StatusCode = 0;
                statuss.Message = "User does not exist";
                return statuss;
            }
            //we will match password
            if(!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                statuss.StatusCode = 0;
                statuss.Message = "Invalid Password";
                return statuss;
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                //add roles
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                statuss.StatusCode = 1;
                statuss.Message = "Login successfull";
                return statuss;
            }else if(signInResult.IsLockedOut){
                statuss.StatusCode = 0;
                statuss.Message = "User locked out";
                return statuss;
            }
            else
            {
                statuss.StatusCode = 0;
                statuss.Message = "Error on logged in";
                return statuss;
            }
        }
        public async Task<Status> RegistatrionAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExist = await _userManager.FindByNameAsync(model.Username);
            if (userExist != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }
            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.Username,
                EmailConfirmed = true,

            };

            var result = await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }
            //role managment
            if(!await _roleManager.RoleExistsAsync(model.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            }
            if(await _roleManager.RoleExistsAsync(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
