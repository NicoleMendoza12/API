using AuthApi.DTOs.UsuarioDTOs;

namespace AuthApi.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioRespuestaDto> RegistrarAsync(UsuarioRegistroDTO dto);
        Task<UsuarioRespuestaDto?> LoginAsync(UsuarioLoginDto dto);
    }
}
