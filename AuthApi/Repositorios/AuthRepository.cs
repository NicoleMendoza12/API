using AuthApi.DTOs.UsuarioDTOs;
using AuthApi.Entidades;
using AuthApi.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Repositorios
{
    public class AuthRepository : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IConfiguration _config;  

        public AuthRepository(IUsuarioRepository usuarioRepo, IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _config = config;
        }

        public async Task<UsuarioRespuestaDto> RegistrarAsync(UsuarioRegistroDTO dto)
        {
            // Crear el usuario con RolId
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = 2 // Usuario por defecto
            };

            // Guardar en la base de datos
            await _usuarioRepo.AddAsync(usuario);

            // Recargar el usuario para incluir el Rol
            usuario = await _usuarioRepo.GetByEmailAsync(usuario.Email);

            // Generar token
            string token = GenerarToken(usuario);

            return new UsuarioRespuestaDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol?.Nombre ?? "Usuario",
                Token = token
            };
        }

        public async Task<UsuarioRespuestaDto?> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _usuarioRepo.GetByEmailAsync(dto.Email);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return null;

            return new UsuarioRespuestaDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.Nombre,
                Token = GenerarToken(usuario)
            };
        }

        private string GenerarToken(Usuario usuario)
        {
            // Validar que el usuario y su rol no sean nulos
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            if (usuario.Rol == null) throw new InvalidOperationException("El usuario no tiene rol asignado.");

            // Crear clave y credenciales para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Definir los Claims del token (email y rol)
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Email ?? ""),
        new Claim("rol", usuario.Rol.Nombre)
    };

            // Crear token JWT con issuer, audience, claims y expiración
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            // Retornar token en formato string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
