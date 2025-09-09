using AuthApi.DTOs.UsuarioDTOs;
using AuthApi.Entidades;
using AuthApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<List<UsuarioListadoDto>> GetAllUsuariosAsync()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();

            return usuarios.Select(u => new UsuarioListadoDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol?.Nombre ?? "Sin Rol"
            }).ToList();
        }
    }
}
