using ApiWeb.Interfaces;
using ApiWeb.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var con = new SqlConnection(this.GetConnection());
            try
            {
                var sqlStatement = @"
                    UPDATE USUARIOS SET
                    nome = @Nome,
                    cpf = @Cpf,
                    permissao = @Permissao,
                    email = @Email,
                    senha = @Senha
                    WHERE id_usuario = @UsuarioId
                ";
                con.Open();
                con.Query(sqlStatement, usuario);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                con.Close();
            }
        }

        public void Excluir(Usuario usuario)
        {
            var con = new SqlConnection(this.GetConnection());
            try
            {
                var sqlStatement = @"
                    DELETE FROM Usuarios
                    WHERE id_usuario = @UsuarioId
                ";
                con.Open();
                con.Query(sqlStatement, usuario);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                con.Close();
            }
        }

        public Usuario ListaPorId(long id)
        {
            var con = new SqlConnection(this.GetConnection());
            Usuario usuario = new();
            try
            {
                var sqlStatement = "SELECT * FROM Usuarios WHERE id_usuario =" + id;
                con.Open();
                usuario = con.Query<Usuario>(sqlStatement, usuario).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return usuario;
        }

        public IEnumerable<Usuario> ListarTodos()
        {
            var con = new SqlConnection(this.GetConnection());
            IEnumerable<Usuario> usuarios;
            try
            {
                var sqlStatement = "SELECT * FROM Usuarios";
                con.Open();
                usuarios = con.Query<Usuario>(sqlStatement);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return usuarios;
        }

        public Usuario Logar(string email, string senha)
        {
            var con = new SqlConnection(this.GetConnection());
            Usuario usuario = new();
            try
            {
                var sqlStatement = @"SELECT * FROM Usuarios WHERE email = @Email AND senha = @Senha";
                con.Open();
                usuario = con.Query<Usuario>(sqlStatement, new { Senha = senha, Email = email }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return usuario;
        }

        public long Salvar(Usuario usuario)
        {
            var con = new SqlConnection(this.GetConnection());
            long id;
            try
            {
                var sqlStatement = @"
                    INSERT INTO Usuarios (nome, cpf, permissao, email, senha)
                    VALUES (@Nome, @Cpf, @Permissao, @Email, @Senha);
                    SELECT CAST(SCOPE_IDENTITY() as INT);
                ";
                con.Open();
                id = con.Execute(sqlStatement, usuario);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return id;
        }
    }
}
