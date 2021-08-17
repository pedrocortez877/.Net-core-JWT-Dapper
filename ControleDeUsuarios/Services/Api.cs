using ApiWeb.Models;
using ControleDeUsuarios.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ControleDeUsuarios.Services
{
    public class Api
    {
        public static string baseUrl = "https://localhost:44355/";

        public static HttpClient HttpUtil()
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(baseUrl)
            };
            var contentType = new MediaTypeWithQualityHeaderValue
                ("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            return client;
        }

        public static string LoginUtil(Usuario usuario)
        {
            HttpClient client = HttpUtil();
            string stringData = JsonConvert.SerializeObject(usuario);
            var contentData = new StringContent
                (stringData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync
                ("/Login", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            string jwt = stringJWT[10..^2];
            return jwt;
        }
   
        public static HttpResponseMessage GetUtil(string token, string route)
        {
            HttpClient client = HttpUtil();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = client.GetAsync(route).Result;
            return response;
        }

        public static HttpResponseMessage CreateUtil(Usuario usuario, Endereco endereco, string token)
        {
            HttpClient client = HttpUtil();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            string route = "/Usuario";
            string stringData = JsonConvert.SerializeObject(usuario);
            var contentData = new StringContent
                (stringData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage responseUser = client.PostAsync(route, contentData).Result;

            if (!responseUser.IsSuccessStatusCode)
            {
                return responseUser;
            }
            else
            {
                string idUsuario = responseUser.Content.ReadAsStringAsync().Result.Substring(14, 5);
                route = "/Endereco";

                if (int.TryParse(idUsuario, out int idInt))
                {
                    endereco.Usuario_Id = idInt;
                    stringData = JsonConvert.SerializeObject(endereco);
                    contentData = new StringContent
                        (stringData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(route, contentData).Result;
                    return response;
                }
                else
                {
                    responseUser.StatusCode = HttpStatusCode.BadRequest;
                    return responseUser;
                }
            }
        }

        public static string ExceptionUtil(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "Não autorizado a acessar este recurso";
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return "Não tem permissões de acesso suficientes";
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return "Erro desconhecido no servidor";
            }
            return "";
        }
    }
}
