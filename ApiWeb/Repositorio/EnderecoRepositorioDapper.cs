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
            var con = new SqlConnection(this.GetConnection());
            try
            {
                var sqlStatement = @"
                    UPDATE EnderecoS SET
                    cep = @Cep,
                    pais = @Pais,
                    estado = @Estado,
                    cidade = @Cidade,
                    bairro = @Bairro,
                    rua = @Rua,
                    numero = @Numero
                    WHERE id_endereco = @EnderecoId
                ";
                con.Open();
                con.Query(sqlStatement, endereco);
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

        public void Excluir(Endereco endereco)
        {
            var con = new SqlConnection(this.GetConnection());
            try
            {
                var sqlStatement = @"
                    DELETE FROM Enderecos
                    WHERE id_endereco = @EnderecoId
                ";
                con.Open();
                con.Query(sqlStatement, endereco);
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

        public Endereco ListaPorId(long id)
        {
            var con = new SqlConnection(this.GetConnection());
            Endereco endereco = new();
            try
            {
                var sqlStatement = "SELECT * FROM Enderecos WHERE id_endereco =" + id;
                con.Open();
                endereco = con.Query<Endereco>(sqlStatement, endereco).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return endereco;
        }

        public IEnumerable<Endereco> ListarTodos()
        {
            var con = new SqlConnection(this.GetConnection());
            IEnumerable<Endereco> enderecos;
            try
            {
                var sqlStatement = "SELECT * FROM Enderecos";
                con.Open();
                enderecos = con.Query<Endereco>(sqlStatement);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return enderecos;
        }

        public void Salvar(Endereco endereco)
        {
            var con = new SqlConnection(this.GetConnection());
            try
            {
                var sqlStatement = @"
                    INSERT INTO Enderecos (cep, pais, estado, cidade, bairro, rua, numero, id_usuario)
                    VALUES (@Cep, @Pais, @Estado, @Cidade, @Bairro, @Rua, @Numero, @Usuario_Id)
                ";
                con.Open();
                con.Execute(sqlStatement, endereco);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
