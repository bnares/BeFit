using ArtGuard.Helpers;
using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Models.DTO.DaneWykonanegoCwiczenia;
using ArtGuard.Models.DTO.SesjeTreingowe;
using ArtGuard.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArtGuard.Controllers
{
    
    
    public class SesjaTreningowaController : Controller
    {
        private readonly ITrainingSessionService _trainingSessionService;

        public SesjaTreningowaController(ITrainingSessionService trainingSessionService)
        {
            _trainingSessionService = trainingSessionService;
        }
        [Authorize(Roles = "user")]
        public IActionResult Display()
        {
            return View();
        }

        [Authorize(Roles ="user")]
        public async Task<IActionResult> SeeAllTrainings()
        {
            var allTraings = await _trainingSessionService.PobierzWszystkieSesjeTreningowe();
            return View(allTraings);
        }

        [Authorize(Roles = "user")]
        public IActionResult DodajSesjeTreningowa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DodajSesjeTreningowa(DodajSesjeTreningowaDTO dto)
        {
            var result = await _trainingSessionService.DodajSesjeTreningowa(dto);
            Notification notification;
            if (result.StatusCode > 0)
            {
                notification = new Notification("success", "Created new Training session");
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                return RedirectToAction("Display");
            }
            notification = new Notification("error", "Cant Creat Training session");
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return View(dto);
        }

        [Authorize(Roles ="user")]
        public async Task<IActionResult> UsunSesjeTreningowa(int id)
        {
            var result =await _trainingSessionService.UsunSesjeTreingowa(id);
            Notification notification;
            if (result.StatusCode > 0)
            {
                notification = new Notification("warning", $"Training deleted");
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                return RedirectToAction("Display");
            }
            notification = new Notification("error", "Training cant deleted");
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return RedirectToAction("SeeAllTrainings");
        }

        [HttpGet]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> PokazSzczegółySesjiTrenngowej(int id)
        {
            var result = await _trainingSessionService.PokazSzczegółySesjiTrenngowej(id);
            Notification notification;
            if(result == null)
            {
                notification = new Notification("error", "Cant find such training");
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                
                return RedirectToAction("Display");
            }
            TempData["idSesjiTreningowej"] = id;
            return View(result);
        }

        public async Task<IActionResult> DodajCwiczenieDoSesjiTreningowej()
        {
            var cwiczenia = await _trainingSessionService.PobierzWszystkieCwiczenia();
            TempData["cwiczenia"] = JsonConvert.SerializeObject(cwiczenia);
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> DodajCwiczenieDoSesjiTreningowej(DodajWykonaneCwiczenieDTO dto)
        {
            Notification notification;
            var convertToNumber = int.TryParse((string?)Url.ActionContext.RouteData.Values["id"],out int idSesjiTreningowejZurlId);
            if (!convertToNumber)
            {
                notification = new Notification("success", "Cant find such training session");
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                return View(dto);
            }

            //var idSesjiTreningowejZurlId =(int) (Url.ActionContext.RouteData.Values["id"]!);
            
            var result = await _trainingSessionService.DodajCwiczenieDoSesjiTreningowej(dto, idSesjiTreningowejZurlId);
            
            if (result.StatusCode > 0)
            {
                notification = new Notification("success", result.Message);
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                RedirectToAction("Display");
            }
            notification = new Notification("success", result.Message);
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return View(dto);

        }
        [Authorize(Roles ="user")]
        public async Task<IActionResult> UsunCwiczenieSesjiTreningowej(int id)
        {
            Notification notification;
            var result =await  _trainingSessionService.UsunCwiczenieSesjiTreningowej(id);
            if (result.StatusCode > 0)
            {
                notification = new Notification("warning", result.Message);
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                RedirectToAction("Display");
            }
            notification = new Notification("error", result.Message);
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return RedirectToAction("Display");
        }

    }
}
