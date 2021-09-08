using ApiWeb.Models;
using ControleDeUsuarios.Services;
using ControleDeUsuarios.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ControleDeUsuarios.Controllers
{
    public class UserController : Controller
    {
        Api api;
        public UserController()
        {
            api = new Api();
        }
        public IActionResult Index()
        {
            //Verificar se token e nulo!!!
            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = api.GetUtil(token, "/Usuarios");

            if (!response.IsSuccessStatusCode)
            {
                return ExceptionUtil(response.StatusCode);
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
                var stringJWT = api.LoginUtil(usuario);
                HttpContext.Session.SetString("token", stringJWT.Token);

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
            HttpResponseMessage response = api.CreateUtil(usuario, endereco, token);
            if (!response.IsSuccessStatusCode)
            {
                return ExceptionUtil(response.StatusCode);
            }
            return RedirectToAction("Index");
        }

        private static Usuario InstanceUser(Create user)
        {
            Usuario usuario = new()
            {
                UsuarioId = user.UsuarioId,
                Nome = user.Nome,
                Email = user.Email,
                Cpf = user.Cpf,
                Permissao = user.Permissao,
                Senha = user.Senha,
                ConfirmaSenha = user.ConfirmaSenha
            };
            return usuario;
        }

        private static Endereco InstanceAddress(Create address)
        {
            Endereco endereco = new()
            {
                EnderecoId = address.EnderecoId,
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

        private static Create InstanceCreate(Usuario user, Endereco address)
        {
            Create create = new()
            {
                UsuarioId = user.UsuarioId,
                Nome = user.Nome,
                Email = user.Email,
                Cpf = user.Cpf,
                Permissao = user.Permissao,
                Senha = user.Senha,
                ConfirmaSenha = user.ConfirmaSenha,
                EnderecoId = address.EnderecoId,
                Cep = address.Cep,
                Pais = address.Pais,
                Estado = address.Estado,
                Cidade = address.Cidade,
                Bairro = address.Bairro,
                Rua = address.Rua,
                Numero = address.Numero
            };

            return create;
        }

        public IActionResult Delete(long id)
        {
            var token = HttpContext.Session.GetString("token");
            Task<HttpResponseMessage> response = api.DeleteUtil(id, token);
            if (!response.Result.IsSuccessStatusCode)
            {
                return ExceptionUtil(response.Result.StatusCode);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(long id)
        {
            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage responseGetUser = api.GetUtil(token, "/Usuarios/" + id);
            if (!responseGetUser.IsSuccessStatusCode)
            {
                return ExceptionUtil(responseGetUser.StatusCode);
            }
            HttpResponseMessage responseGetAddress = api.GetUtil(token, "/Enderecos/" + id);
            if (!responseGetAddress.IsSuccessStatusCode)
            {
                return ExceptionUtil(responseGetUser.StatusCode);
            }

            string stringUser = responseGetUser.Content.ReadAsStringAsync().Result;
            Usuario usuario = JsonConvert.DeserializeObject
                <Usuario>(stringUser);

            string stringAddress = responseGetAddress.Content.ReadAsStringAsync().Result;
            Endereco endereco = JsonConvert.DeserializeObject
                <Endereco>(stringAddress);

            Create create = InstanceCreate(usuario, endereco);
            return View(create);
            
        }

        [HttpPost]
        public IActionResult EditUserAndAddress(Create create)
        {
            var token = HttpContext.Session.GetString("token");
            Usuario usuario = InstanceUser(create);
            Endereco endereco = InstanceAddress(create);
            
            HttpResponseMessage response = api.EditUtil(usuario, endereco, token);
            if (!response.IsSuccessStatusCode)
            {
                return ExceptionUtil(response.StatusCode);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult ExceptionUtil(HttpStatusCode response)
        {
            if (response == HttpStatusCode.Unauthorized)
            {
                return View("Não autorizado a acessar este recurso");
            }
            if (response == HttpStatusCode.Forbidden)
            {
                return View("Não tem permissões de acesso suficientes");
            }
            if (response == HttpStatusCode.BadRequest)
            {
                return View("Erro desconhecido no servidor");
            }
            return null;
        }

        public IActionResult Details(long id)
        {
            string token = HttpContext.Session.GetString("token");

            HttpResponseMessage responseGetUser = api.GetUtil(token, "/Usuarios/" + id);
            if (!responseGetUser.IsSuccessStatusCode)
            {
                return ExceptionUtil(responseGetUser.StatusCode);
            }
            HttpResponseMessage responseGetAddress = api.GetUtil(token, "/Enderecos/" + id);
            if (!responseGetAddress.IsSuccessStatusCode)
            {
                return ExceptionUtil(responseGetUser.StatusCode);
            }

            string stringUser = responseGetUser.Content.ReadAsStringAsync().Result;
            Usuario usuario = JsonConvert.DeserializeObject
                <Usuario>(stringUser);

            string stringAddress = responseGetAddress.Content.ReadAsStringAsync().Result;
            Endereco endereco = JsonConvert.DeserializeObject
                <Endereco>(stringAddress);

            Create create = InstanceCreate(usuario, endereco);
            return View(create);
        }
    }
}
