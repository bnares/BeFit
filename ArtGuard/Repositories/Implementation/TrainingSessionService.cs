using ArtGuard.Helpers;
using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Models.DTO.DaneWykonanegoCwiczenia;
using ArtGuard.Models.DTO.SesjeTreingowe;
using ArtGuard.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;

namespace ArtGuard.Repositories.Implementation
{
    public class TrainingSessionService : ITrainingSessionService
    {
        private readonly ArtGuardDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        public TrainingSessionService(ArtGuardDbContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public async Task<List<SejsaTreningowa>> PobierzWszystkieSesjeTreningowe()
        {
            var userId = _httpContext?.HttpContext?.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return await _dbContext.SesjeTreningowe.Include(x=>x.User).OrderByDescending(x => x.DataCwiczenia).Where(x=>x.UserId==userId).ToListAsync();
        }

        public async Task<Status> DodajSesjeTreningowa(DodajSesjeTreningowaDTO dto)
        {
            var userId = _httpContext?.HttpContext?.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            
            if (userId != null && user != null)
            {
                var sesjaTreingowa = new SejsaTreningowa() { DataCwiczenia = dto.DateTime, User = user, UserId = userId };
                _dbContext.SesjeTreningowe.Add(sesjaTreingowa);
                await _dbContext.SaveChangesAsync();

                return new Status() { StatusCode = 1, Message = "New Trainig Session Created" };
            }
            return new Status() { StatusCode = 0, Message = "Cant Created Training Session" };
        }


        public async Task<Status> UsunSesjeTreingowa(int id)
        {
            var sesja = await _dbContext.SesjeTreningowe.FirstOrDefaultAsync(x => x.Id == id);
            if (sesja == null) return new Status() { StatusCode = 0, Message = "Nie mozna znalezc takiej sesji" };
            _dbContext.SesjeTreningowe.Remove(sesja);
            var daneCwiczenWykonaneWKonkretnejSesji = _dbContext.DaneWykonanychCwiczen.Where(x => x.IdSesjiTreningowej == id).ToList();
            _dbContext.RemoveRange(daneCwiczenWykonaneWKonkretnejSesji);
            await _dbContext.SaveChangesAsync();
            return new Status() { StatusCode = 1, Message = $"Sesja from {sesja.DataCwiczenia.ToString("d", DateTimeFormatInfo.InvariantInfo)}" };

        }

        public async Task<List<DaneWykonanegoCwiczenia>?> PokazSzczegółySesjiTrenngowej(int id)
        {
            var sesja = await _dbContext.DaneWykonanychCwiczen.Include(x=>x.Cwiczenie)
                .Where(x => x.IdSesjiTreningowej==id).ToListAsync();
            if (sesja == null) return null;
            return sesja;
        }

        public async Task<List<Cwiczenie>> PobierzWszystkieCwiczenia()
        {
            var dane = await _dbContext.Cwiczenia.ToListAsync();
            return dane;
        }

        public async Task<Status> DodajCwiczenieDoSesjiTreningowej(DodajWykonaneCwiczenieDTO dto, int idSesjiTreningowej)
        {
            var cwiczenie = await _dbContext.Cwiczenia.FirstOrDefaultAsync(x => x.Id == dto.IdCwiczenia);
            var sesjaTreningowa = await _dbContext.SesjeTreningowe.FirstOrDefaultAsync(x => x.Id == idSesjiTreningowej);
            if (cwiczenie == null || sesjaTreningowa == null) return new Status() { StatusCode = 0, Message = "Nie mozna znalezc takiej sesji treningowej lub cwiczenia" };
            var daneWykonanegoCwiczenia = new DaneWykonanegoCwiczenia()
            {
                IdCwiczenia = dto.IdCwiczenia,
                Cwiczenie = cwiczenie,
                IdSesjiTreningowej = idSesjiTreningowej,
                SesjaTreningowa = sesjaTreningowa,
                LiczbaPowtorzen = dto.LiczbaPowtorzen,
                LiczbaSerii = dto.LiczbaSerii,
                ObciazenieWCwiczeniu = dto.ObciazenieWCwiczeniu,
            };
            _dbContext.DaneWykonanychCwiczen.Add(daneWykonanegoCwiczenia);
            await _dbContext.SaveChangesAsync();
            return new Status() { StatusCode = 1, Message = "Dodane dane do cwiczenia" };
        }

        public async Task<Status> UsunCwiczenieSesjiTreningowej(int id)
        {
            var cwiczenieSesji = _dbContext.DaneWykonanychCwiczen.FirstOrDefault(x => x.Id == id);
            if (cwiczenieSesji == null) return new Status() { StatusCode = 0, Message = "Cant find such training data" };
            _dbContext.Remove(cwiczenieSesji);
            await _dbContext.SaveChangesAsync();
            return new Status() { StatusCode = 1, Message = "Training deleted" };
        }
    }
}
