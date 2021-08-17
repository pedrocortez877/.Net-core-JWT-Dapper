using ApiWeb.Interfaces;
using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiWeb.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUsuario _repositorio;

        public UserController(IUsuario usuario)
        {
            _repositorio = usuario;
        }

        //POST Login
        [HttpPost]
        [Route("/Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Usuario user)
        {
            Usuario usuario = ValidateUser(user.Email, user.Senha);
            if (usuario != null)
            {
                var token = TokenService.GenerateToken(usuario);

                return Ok(new { token });
            }
            else
            {
                return Unauthorized();
            }
        }

        private Usuario ValidateUser(string email, string senha)
        {
            Usuario usuario = _repositorio.Logar(email, senha);
            if(usuario == null)
            {
                return null;
            }
            return usuario;
        }

        [HttpGet]
        [Route("/Usuarios")]
        [Authorize]
        public IEnumerable<Usuario> GetUsuarios()
        {
            IEnumerable<Usuario> usuarios = _repositorio.ListarTodos();
            return usuarios;
        }

        [HttpGet]
        [Route("/Funcionario")]
        [Authorize(Roles = "USER,ADM")]
        public string Funcionario() => "Funcionário autenticado"; 

        [HttpGet]
        [Route("/Administrador")]
        [Authorize(Roles = "ADM")]
        public string Administrador() => "ADM autenticado!";

        [HttpPost]
        [Route("/Usuario")]
        [Authorize(Roles = "ADM")]
        public IActionResult NovoUsuario([FromBody]Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                long id_usuario = _repositorio.Salvar(usuario);
                return Ok(new { id_usuario });
            }
            return BadRequest();
        }
    }
}
