
using Core.Features.Auth.DTOs;
using Core.Features.Auth.Interfaces;
using Domain.Models;
using Generics.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.Auth.Commands.Login
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IGenericRepository<Usuario> usuarioRepository,
            IGenericRepository<Rol> rolRepository,
            IJwtService jwtService,
            IPasswordService passwordService,
            ILogger<LoginCommandHandler> logger)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _jwtService = jwtService;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var correo = command.Correo.Trim();

            // 1. Buscar usuario
            var usuario = await _usuarioRepository.GetOneByAsync(
                x => x.Correo == correo,
                true,
                cancellationToken
            );

            _logger.LogInformation(
                "Login: usuario encontrado = {Encontrado}",
                usuario != null
            );

            if (usuario == null)
            {
                throw new UnauthorizedAccessException(
                    "Correo o contraseña incorrectos."
                );
            }

            // 2. Verificar estado
            _logger.LogInformation(
                "Login: usuario activo = {Estado}",
                usuario.Estado
            );

            if (!usuario.Estado)
            {
                throw new UnauthorizedAccessException(
                    "El usuario se encuentra inactivo."
                );
            }

            // 3. Verificar contraseña
            var passwordValido = _passwordService.VerifyPassword(
                command.Password,
                usuario.PasswordHash
            );

            _logger.LogInformation(
                "Login: contraseña válida = {Valida}",
                passwordValido
            );

            if (!passwordValido)
            {
                throw new UnauthorizedAccessException(
                    "Correo o contraseña incorrectos."
                );
            }

            // 4. Obtener rol
            var rol = await _rolRepository.GetOneByAsync(
                x => x.IdRol == usuario.IdRol,
                true,
                cancellationToken
            );

            _logger.LogInformation(
                "Login: rol encontrado = {Encontrado}",
                rol != null
            );

            if (rol == null)
            {
                throw new Exception(
                    "El usuario no tiene un rol válido."
                );
            }

            // 5. Generar JWT
            var expiracion = DateTime.UtcNow.AddHours(1);

            var token = _jwtService.GenerarToken(
                usuario,
                rol,
                expiracion
            );

            return new LoginResponseDto
            {
                Token = token,
                Expiracion = expiracion,
                Usuario = new UsuarioLoginDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Correo = usuario.Correo,
                    Rol = rol.Nombre
                }
            };
        }
    }
}
