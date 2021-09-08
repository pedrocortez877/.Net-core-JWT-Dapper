using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWeb.Models
{
    public class Endereco
    {
        [Key]
        public long EnderecoId { get; set; }

        public string Cep { get; set; }

        public string Pais { get; set; }

        public string Estado { get; set; }

        public string Cidade { get; set; }

        public string Bairro { get; set; }

        public string Rua { get; set; }

        public int Numero { get; set; }

        [ForeignKey("Usuario")]
        public long UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
    }
}
