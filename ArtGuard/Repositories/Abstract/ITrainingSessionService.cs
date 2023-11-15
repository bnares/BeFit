using ArtGuard.Models.Domain;
using ArtGuard.Models.DTO;
using ArtGuard.Models.DTO.DaneWykonanegoCwiczenia;
using ArtGuard.Models.DTO.SesjeTreingowe;

namespace ArtGuard.Repositories.Abstract
{
    public interface ITrainingSessionService
    {
        Task<Status> DodajSesjeTreningowa(DodajSesjeTreningowaDTO dto);
        Task<List<SejsaTreningowa>> PobierzWszystkieSesjeTreningowe();
        Task<Status> UsunSesjeTreingowa(int id);
        Task<List<DaneWykonanegoCwiczenia>?> PokazSzczegółySesjiTrenngowej(int id);
        Task<List<Cwiczenie>> PobierzWszystkieCwiczenia();
        Task<Status> DodajCwiczenieDoSesjiTreningowej(DodajWykonaneCwiczenieDTO dto, int idSesjiTreningowej);
        Task<Status> UsunCwiczenieSesjiTreningowej(int id);
    }
}