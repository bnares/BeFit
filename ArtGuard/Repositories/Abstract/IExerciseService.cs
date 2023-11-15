using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Models.DTO.Cwiczenie;

namespace ArtGuard.Repositories.Abstract
{
    public interface IExerciseService
    {
        Task<Status> AddExercise(CwiczenieDTO dto);
        Task<Status> DeleteExercise(int id);
        Task<Status> UpdateExercise(CwiczenieUpdateDTO dto);
        Task<List<Cwiczenie>> PobierzCwiczenia();
    }
}