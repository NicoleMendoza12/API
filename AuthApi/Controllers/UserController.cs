using AuthApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UserController(IUsuarioRepository pUsuarioRepository)
        {
            _usuarioRepository = pUsuarioRepository;
        }

        [HttpGet("usuarios")]
        [Authorize] // Solo usuarios autenticados
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            return Ok(usuarios);
        }
    }
}
