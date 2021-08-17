using ApiWeb.Models;
using ControleDeUsuarios.Services;
using ControleDeUsuarios.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ControleDeUsuarios.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = Api.GetUtil(token, "/Usuarios");
            //PEGAR SOMENTE STRING DO TOKEN
            string exception = Api.ExceptionUtil(response);

            if (!string.IsNullOrEmpty(exception))
            {
                ViewBag.Title = exception;
                return View("Exception", exception);
            }
            else
            {
                string stringData = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Usuario> usuarios = JsonConvert.DeserializeObject
                    <List<Usuario>>(stringData);
                return View("Index", usuarios);
            }
        }

        public IActionResult LoginUser()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Logar(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.Email) && !string.IsNullOrEmpty(usuario.Senha))
            {
                string stringJWT = Api.LoginUtil(usuario);
                HttpContext.Session.SetString("token", stringJWT);

                return RedirectToAction("Index");
            }
            return View("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("token", "");
            return View("Login");
        }

        public IActionResult Create()
        {
            string token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Title = "Erro";
                return View("Exception", "Não autorizado a acessar este recurso");
            }
            return View();
        }

        ////POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Create user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Usuario usuario = InstanceUser(user);
            Endereco endereco = InstanceAddress(user);

            var token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = Api.CreateUtil(usuario, endereco, token);
            //PEGAR SOMENTE STRING DO TOKEN
            string exception = Api.ExceptionUtil(response);

            if (!string.IsNullOrEmpty(exception))
            {
                ViewBag.Title = exception;
                return View("Exception", exception);
            }
            return RedirectToAction("Index");
        }

        private Usuario InstanceUser(Create user)
        {
            Usuario usuario = new()
            {
                Nome = user.Nome,
                Email = user.Email,
                Cpf = user.Cpf,
                Permissao = user.Permissao,
                Senha = user.Senha,
                ConfirmaSenha = user.ConfirmaSenha
            };
            return usuario;
        }

        private Endereco InstanceAddress(Create address)
        {
            Endereco endereco = new()
            {
                Cep = address.Cep,
                Pais = address.Pais,
                Estado = address.Estado,
                Cidade = address.Cidade,
                Bairro = address.Bairro,
                Rua = address.Rua,
                Numero = address.Numero
            };

            return endereco;
        }
    }
}
