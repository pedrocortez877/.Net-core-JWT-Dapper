using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWeb.Models
{
    public class Usuario
    {
        [Key]
        public long UsuarioId { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string Permissao { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        [NotMapped]
        public string ConfirmaSenha { get; set; }

        public Endereco Endereco;
    }
}
