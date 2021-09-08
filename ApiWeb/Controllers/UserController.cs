using ApiWeb.Interfaces;
using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWeb.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUsuario _repoUsuario;
        private readonly IEndereco _repoEndereco;
        public UserController(IUsuario usuario, IEndereco endereco)
        {
            _repoUsuario = usuario;
            _repoEndereco = endereco;
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
            Usuario usuario = _repoUsuario.Logar(email, senha);
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
            IEnumerable<Usuario> usuarios = _repoUsuario.ListarTodos();
            return usuarios;
        }

        [HttpGet]
        [Route("/Usuarios/{id}")]
        [Authorize]
        public Usuario GetUsuarioPorId([FromRoute] long id)
        {
            Usuario usuario = _repoUsuario.ListaPorId(id);
            return usuario;
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
        [Route("/Usuarios")]
        [Authorize(Roles = "ADM")]
        public Task<long> NovoUsuario([FromBody]Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                Task<long> idUsuario = _repoUsuario.Salvar(usuario);
                return idUsuario;
            }
            return null;
        }

        [HttpDelete]
        [Route("/Usuarios/{id}")]
        [Authorize(Roles = "ADM")]
        public IActionResult ExcluiUsuario([FromRoute] long id)
        {
            Usuario usuario = _repoUsuario.ListaPorId(id);
            if(usuario == null)
            {
                return BadRequest();
            }
            else
            {
                //DELETA PRIMEIRO O ENDEREÇO PARA NÃO HAVER RESTRIÇÃO DE INTEGRIDADE
                Endereco endereco = _repoEndereco.ListaPorIdUsuario(usuario.UsuarioId);
                if(endereco == null)
                {
                    return BadRequest();
                }
                else
                {
                    _repoEndereco.Excluir(endereco);

                    _repoUsuario.Excluir(usuario);
                    return Ok();
                }
            }
        }

        [HttpPut]
        [Route("/Usuarios")]
        [Authorize(Roles = "ADM")]
        public IActionResult EditaUsuario([FromBody] Usuario usuario)
        {
            if(usuario == null)
            {
                return BadRequest();
            }
            else
            {
                _repoUsuario.Editar(usuario);
                return Ok();
            }
        }
    }
}
