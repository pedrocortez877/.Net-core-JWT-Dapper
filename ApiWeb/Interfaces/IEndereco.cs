using ApiWeb.Models;
using System.Collections.Generic;

namespace ApiWeb.Interfaces
{
    public interface IEndereco
    {
        void Salvar(Endereco Endereco);
        void Excluir(Endereco Endereco);
        IEnumerable<Endereco> ListarTodos();
        Endereco ListaPorIdUsuario(long id);
        void Editar(Endereco endereco);
    }
}
