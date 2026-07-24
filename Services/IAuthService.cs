using VIRTUAL_CLASSE_FORMATION.Dtos;

namespace VIRTUAL_CLASSE_FORMATION.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
