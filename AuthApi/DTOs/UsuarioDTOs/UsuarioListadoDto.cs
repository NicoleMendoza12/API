namespace AuthApi.DTOs.UsuarioDTOs
{
    public class UsuarioListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
