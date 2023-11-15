using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Models.DTO.Cwiczenie;
using ArtGuard.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ArtGuard.Repositories.Implementation
{
    public class ExerciseService : IExerciseService
    {
        private readonly ArtGuardDbContext _context;

        public ExerciseService(ArtGuardDbContext context)
        {
            _context = context;
        }

        public async Task<Status> AddExercise(CwiczenieDTO dto)
        {
            Cwiczenie cwiczenie = new Cwiczenie();
            cwiczenie.Nazwa = dto.Name;
            _context.Cwiczenia.Add(cwiczenie);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new Status() { StatusCode = 1, Message = $"Exercise '{cwiczenie.Nazwa}' Added" };
            else return new Status() { StatusCode = 0, Message = $"Cant added Exercise" };
        }

        public async Task<Status> DeleteExercise(int id)
        {
            var cwiczenie = _context.Cwiczenia.FirstOrDefault(x => x.Id == id);
            if (cwiczenie != null)
            {
                _context.Cwiczenia.Remove(cwiczenie);
                await _context.SaveChangesAsync();
                return new Status() { StatusCode = 1, Message = $"Delete '${cwiczenie.Nazwa}'" };
            }
            else
            {
                return new Status() { StatusCode = 0, Message = $"Cant Delete Exercise" };
            }
        }

        public async Task<Status> UpdateExercise(CwiczenieUpdateDTO dto)
        {
            var cwiczenie = _context.Cwiczenia.FirstOrDefault(x => x.Id == dto.Id);
            if (cwiczenie != null)
            {
                cwiczenie.Nazwa = dto.Name;
                await _context.SaveChangesAsync();
                return new Status() { StatusCode = 1, Message = $"Exercise updated" };
            }
            else
            {
                return new Status() { StatusCode = 0, Message = $"Cant Update Exercise to ${dto.Name}" };
            }
        }

        public async Task<List<Cwiczenie>> PobierzCwiczenia()
        {
            var cwiczenia =await _context.Cwiczenia.ToListAsync();
            return cwiczenia;
        }
    }
}
