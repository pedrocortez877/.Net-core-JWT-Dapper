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
    public class EnderecoRepositorioDapper : IEndereco
    {
        private readonly IConfiguration _configuration;

        public EnderecoRepositorioDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var connection = _configuration.GetConnectionString("DefaultConnection");
            return connection;
        }

        public void Editar(Endereco endereco)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = @"
                UPDATE EnderecoS SET
                Cep = @Cep,
                Pais = @Pais,
                Estado = @Estado,
                Cidade = @Cidade,
                Bairro = @Bairro,
                Rua = @Rua,
                Numero = @Numero
                WHERE EnderecoId = @EnderecoId
            ";
            con.Query(sqlStatement, endereco);
        }

        public void Excluir(Endereco endereco)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = @"
                DELETE FROM Enderecos
                WHERE EnderecoId = @EnderecoId
            ";
            con.Query(sqlStatement, endereco);
        }

        public Endereco ListaPorIdUsuario(long id)
        {
            using var con = new SqlConnection(this.GetConnection());
            Endereco endereco = new();
            var sqlStatement = "SELECT * FROM Enderecos WHERE UsuarioId =" + id;
            endereco = con.Query<Endereco>(sqlStatement, endereco).FirstOrDefault();
            return endereco;
        }

        public IEnumerable<Endereco> ListarTodos()
        {
            using var con = new SqlConnection(this.GetConnection());
            IEnumerable<Endereco> enderecos;
            var sqlStatement = "SELECT * FROM Enderecos";
            enderecos = con.Query<Endereco>(sqlStatement);
            return enderecos;
        }

        public void Salvar(Endereco endereco)
        {
            using var con = new SqlConnection(this.GetConnection());
            var sqlStatement = @"
                INSERT INTO Enderecos (Cep, Pais, Estado, Cidade, Bairro, Rua, Numero, UsuarioId)
                VALUES (@Cep, @Pais, @Estado, @Cidade, @Bairro, @Rua, @Numero, @UsuarioId)
            ";
            con.Execute(sqlStatement, endereco);
        }
    }
}
