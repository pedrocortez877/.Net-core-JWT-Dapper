using ControleDeUsuarios.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ControleDeUsuarios.Controllers
{
    public class HomeController : Controller
    {
        Api api;
        public HomeController()
        {
            api = new Api();
        }
        //GET Funcionario
        public IActionResult FUNC()
        {
            var token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = api.GetUtil(token, "/Funcionario");

            if (!response.IsSuccessStatusCode)
            {
                return ExceptionUtil(response.StatusCode);
            }
            else
            {
                string stringData = response.Content.ReadAsStringAsync().Result;
                return View("../User/Funcionario", stringData);
            }
        }

        //GET Administrador
        public IActionResult ADM()
        {
            var token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = api.GetUtil(token, "/Administrador");

            if (!response.IsSuccessStatusCode)
            {
                return ExceptionUtil(response.StatusCode);
            }
            else
            {
                string stringData = response.Content.ReadAsStringAsync().Result;
                return View("../User/Administrador", stringData);
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
    }
}
