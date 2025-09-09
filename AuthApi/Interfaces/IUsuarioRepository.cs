using AuthApi.DTOs.UsuarioDTOs;
using AuthApi.Entidades;

namespace AuthApi.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario> AddAsync(Usuario usuario);
        Task<List<UsuarioListadoDto>> GetAllUsuariosAsync();
    }
}
