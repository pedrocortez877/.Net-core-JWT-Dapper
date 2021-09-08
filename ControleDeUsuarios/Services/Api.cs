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
        private readonly string baseUrl = "https://localhost:44355/";

        public HttpClient HttpUtil(string token)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(baseUrl)
            };
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var contentType = new MediaTypeWithQualityHeaderValue
                ("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            return client;
        }

        public TokenJWT LoginUtil(Usuario usuario)
        {
            HttpClient client = HttpUtil("");
            string stringData = JsonConvert.SerializeObject(usuario);
            var contentData = new StringContent
                (stringData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync
                ("/Login", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            var jwt = JsonConvert.DeserializeObject<TokenJWT>(stringJWT);
            return jwt;
        }
   
        public HttpResponseMessage GetUtil(string token, string route)
        {
            HttpClient client = HttpUtil(token);

            HttpResponseMessage response = client.GetAsync(route).Result;

            return response;
        }

        public HttpResponseMessage CreateUtil(Usuario usuario, Endereco endereco, string token)
        {
            HttpClient client = HttpUtil(token);
            
            string route = "/Usuarios";
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
                string result = responseUser.Content.ReadAsStringAsync().Result;
                long idUsuario = JsonConvert.DeserializeObject<long>(result);
                route = "/Enderecos";

                endereco.UsuarioId = idUsuario;
                stringData = JsonConvert.SerializeObject(endereco);
                contentData = new StringContent
                    (stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(route, contentData).Result;
                return response;
            }
        }

        public async Task<HttpResponseMessage> DeleteUtil(long id, string token)
        {
            HttpClient client = HttpUtil(token);

            string url = "/Usuarios/" + id; 

            HttpResponseMessage response = await client.DeleteAsync(url);
            return response;
        }

        public HttpResponseMessage EditUtil(Usuario usuario, Endereco endereco, string token)
        {
            HttpClient client = HttpUtil(token);

            Task<HttpResponseMessage> responseUser = EditUser(usuario, client);

            if (responseUser.Result.IsSuccessStatusCode)
            {
                Task<HttpResponseMessage> responseAddress = EditAddress(endereco, client);
                return responseAddress.Result;
            }
           
            return responseUser.Result;
        }

        public async Task<HttpResponseMessage> EditUser(Usuario usuario, HttpClient client)
        {
            string stringData = JsonConvert.SerializeObject(usuario);
            var contentData = new StringContent
                (stringData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync("/Usuarios", contentData);
            return response;
        }

        public async Task<HttpResponseMessage> EditAddress(Endereco endereco, HttpClient client)
        {
            string stringData = JsonConvert.SerializeObject(endereco);
            var contentData = new StringContent
                (stringData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync("/Enderecos", contentData);
            return response;
        }
    }
}
