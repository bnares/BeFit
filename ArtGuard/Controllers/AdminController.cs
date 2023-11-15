using ArtGuard.Helpers;
using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Models.DTO.Cwiczenie;
using ArtGuard.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArtGuard.Controllers
{
    public class AdminController : Controller
    {
        private readonly IExerciseService _exerciseService;

        public AdminController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [Authorize(Roles ="admin")]
        public IActionResult Display()
        {
            return View();
        }
        [Authorize(Roles ="admin")]
        public IActionResult DodajCwiczenie() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DodajCwiczenie(CwiczenieDTO dto)
        {
            Notification notification;
            if(dto!=null && ModelState.IsValid)
            {
                var result = await _exerciseService.AddExercise(dto);
                if (result.StatusCode > 0)
                {
                    notification = new Notification("success", $"Created {dto.Name}");
                    TempData["msg"] = JsonConvert.SerializeObject(notification);
                    return RedirectToAction("Display");
                }
                else
                {
                    notification = new Notification("error", $"Cant Add {dto.Name}");
                    TempData["msg"] = JsonConvert.SerializeObject(notification);
                    return View(dto);
                }
            }
            notification = new Notification("error", $"Sth went wrong");
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return View(dto);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> WszystkieCwiczenia()
        {
            var result = await _exerciseService.PobierzCwiczenia();
            return View(result);
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> UsunCwiczenie(int id)
        {
            var result =await _exerciseService.DeleteExercise(id);
            Notification notification;
            if (result.StatusCode > 0) {
                notification = new Notification("success", $"Deleted Exercise");
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                return RedirectToAction("Display");
            }

            notification = new Notification("error", $"Cant Delete");
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return RedirectToAction("WszystkieCwiczeni");
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> EdytujCwiczenie(int id)
        {
            var ListOfexercises = await _exerciseService.PobierzCwiczenia();
            var exerciseToEdit = ListOfexercises.FirstOrDefault(x => x.Id == id);
            Notification notification;
            if(exerciseToEdit != null)
            {
                var updateDto = new CwiczenieUpdateDTO();
                updateDto.Id = exerciseToEdit.Id;
                updateDto.Name = exerciseToEdit.Nazwa;
                return View(updateDto);
            }
            notification = new Notification("error", "Cant Find Such Exercise");
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return RedirectToAction("WszystkieCwiczenia");
            
        }

        [Authorize(Roles ="admin")]
            [HttpPost]
        public async Task<IActionResult> EdytujCwiczenie(CwiczenieUpdateDTO updateDTO)
        {
            var result = await _exerciseService.UpdateExercise(updateDTO);
            Notification notification;
            if (result.StatusCode > 0)
            {
                notification = new Notification("success", $"Updated '{updateDTO.Name}' Exercise");
                TempData["msg"] = JsonConvert.SerializeObject(notification);
                return RedirectToAction("Display");
            }

            notification = new Notification("error", $"Cant Update");
            TempData["msg"] = JsonConvert.SerializeObject(notification);
            return RedirectToAction("WszystkieCwiczeni");
        }


    }
}
