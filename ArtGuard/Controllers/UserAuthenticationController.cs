using ArtGuard.Models.DTO;
using ArtGuard.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGuard.Controllers
{
    public class UserAuthentication : Controller
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        public UserAuthentication(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService= userAuthenticationService;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Registration(RegistrationModel model)
        {

            if(!ModelState.IsValid) return View(model);
            model.Role = "user";
            var result = await _userAuthenticationService.RegistatrionAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid) return View(model);
            var result = await _userAuthenticationService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userAuthenticationService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> RegAdmin()
        {
            var model = new RegistrationModel()
            {
                Username = "admin",
                Name = "Piotr",
                Email="admin@o2.pl",
                Password="Pa$$w0rd",
                PasswordConfirm="Pa$$w0rd"
               
            };
            model.Role = "admin";
            var result = await _userAuthenticationService.RegistatrionAsync(model);
            return Ok(result);
        }
    }
}
