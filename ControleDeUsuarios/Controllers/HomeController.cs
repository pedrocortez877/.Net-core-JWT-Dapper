using ControleDeUsuarios.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ControleDeUsuarios.Controllers
{
    public class HomeController : Controller
    {
        //GET Funcionario
        public IActionResult FUNC()
        {
            var token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = Api.GetUtil(token, "/Funcionario");
            //PEGAR SOMENTE STRING DO TOKEN
            string exception = Api.ExceptionUtil(response);

            if (!string.IsNullOrEmpty(exception))
            {
                ViewBag.Title = exception;
                return View("../User/Exception", exception);
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
            HttpResponseMessage response = Api.GetUtil(token, "/Administrador");
            //PEGAR SOMENTE STRING DO TOKEN
            string exception = Api.ExceptionUtil(response);

            if (!string.IsNullOrEmpty(exception))
            {
                ViewBag.Title = exception;
                return View("../User/Exception", exception);
            }
            else
            {
                string stringData = response.Content.ReadAsStringAsync().Result;
                return View("../User/Administrador", stringData);
            }
        }
    }
}
