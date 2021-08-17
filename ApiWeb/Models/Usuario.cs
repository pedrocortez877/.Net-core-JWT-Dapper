using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWeb.Models
{
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public long UsuarioId { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("permissao")]
        public string Permissao { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        [NotMapped]
        public string ConfirmaSenha { get; set; }

        public Endereco endereco;
    }
}
