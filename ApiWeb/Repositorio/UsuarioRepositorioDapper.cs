using ApiWeb.Interfaces;
using ApiWeb.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Repositorio
{
    public class UsuarioRepositorioDapper : IUsuario
    {
        private readonly IConfiguration _configuration;

        public UsuarioRepositorioDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var connection = _configuration.GetConnectionString("DefaultConnection");
            return connection;
        }

        public void Editar(Usuario usuario)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = @"
                UPDATE USUARIOS SET
                Nome = @Nome,
                Cpf = @Cpf,
                Permissao = @Permissao,
                Email = @Email
                WHERE UsuarioId = @UsuarioId
            ";
            con.Query(sqlStatement, usuario);
        }

        public void Excluir(Usuario usuario)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = @"
                DELETE FROM Usuarios
                WHERE UsuarioId = @UsuarioId
            ";
            con.Query(sqlStatement, usuario);
        }

        public Usuario ListaPorId(long id)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = "SELECT * FROM Usuarios WHERE UsuarioId =" + id;
            Usuario usuario = con.Query<Usuario>(sqlStatement).FirstOrDefault();
            return usuario;
        }

        public IEnumerable<Usuario> ListarTodos()
        {
            using var con = new SqlConnection(this.GetConnection());
            IEnumerable<Usuario> usuarios;
            var sqlStatement = "SELECT * FROM Usuarios";
            usuarios = con.Query<Usuario>(sqlStatement);
            return usuarios;
        }

        public Usuario Logar(string email, string senha)
        {
            using var con = new SqlConnection(this.GetConnection());
            Usuario usuario = new();
            var sqlStatement = @"SELECT * FROM Usuarios WHERE Email = @Email AND Senha = @Senha";
            usuario = con.Query<Usuario>(sqlStatement, new { Senha = senha, Email = email }).FirstOrDefault();
            return usuario;
        }

        public async Task<long> Salvar(Usuario usuario)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = @"
                INSERT INTO Usuarios (Nome, Cpf, Permissao, Email, Senha)
                OUTPUT INSERTED.UsuarioId
                VALUES (@Nome, @Cpf, @Permissao, @Email, @Senha);
                ";

            var id = await con.QueryAsync<int>(sqlStatement, usuario);
           
            return id.Single();
        }
    }
}
