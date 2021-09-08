using ApiWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWeb.Interfaces
{
    public interface IUsuario
    {
        Task<long> Salvar(Usuario usuario);
        void Excluir(Usuario usuario);
        IEnumerable<Usuario> ListarTodos();
        Usuario ListaPorId(long id);
        void Editar(Usuario usuario);
        Usuario Logar(string email, string senha);
    }
}
