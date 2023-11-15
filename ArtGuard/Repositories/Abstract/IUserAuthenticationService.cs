using ArtGuard.Models.DTO;

namespace ArtGuard.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegistatrionAsync(RegistrationModel model);
    }
}