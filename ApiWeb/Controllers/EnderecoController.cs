using ApiWeb.Interfaces;
using ApiWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiWeb.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly IEndereco _repositorio;

        public EnderecoController(IEndereco endereco)
        {
            _repositorio = endereco;
        }

        [HttpPost]
        [Route("/Enderecos")]
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

        [HttpGet]
        [Route("/Enderecos/{id}")]
        [Authorize]
        public Endereco GetEnderecoPorCliente([FromRoute] long id)
        {
            Endereco endereco = _repositorio.ListaPorIdUsuario(id);
            return endereco;
        }

        [HttpPut]
        [Route("/Enderecos")]
        [Authorize(Roles = "ADM")]
        public IActionResult EditaEndereco([FromBody] Endereco endereco)
        {
            if (endereco == null)
            {
                return BadRequest();
            }
            else
            {
                _repositorio.Editar(endereco);
                return Ok();
            }
        }
    }
}
