using ApiWeb.Interfaces;
using ApiWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb.Controllers
{
    public class EnderecoController : Controller
    {
        private IEndereco _repositorio;

        [HttpPost]
        [Route("/Endereco")]
        [Authorize(Roles = "ADM")]
        public IActionResult EnderecoUsuario([FromBody] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                _repositorio.Salvar(endereco);
                return Ok();
            }
            return BadRequest();
        }
    }
}
