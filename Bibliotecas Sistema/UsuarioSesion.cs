namespace Bibliotecas_Sistema
{
    // Clase estática que guarda la información de la sesión del usuario
    public static class UsuarioSesion
    {
        // Id del usuario logueado (0 si no hay sesión)
        public static int IdUsuarioLogueado { get; set; } = 0;

        // Rol del usuario (ej. "ADMIN", "USER")
        public static string RolUsuario { get; set; } = string.Empty;

        // Nombre de usuario u otros datos útiles
        public static string NombreUsuario { get; set; } = string.Empty;
    }
}
