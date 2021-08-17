using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWeb.Models
{
    public class Endereco
    {
        [Key]
        [Column("id_endereco")]
        public long EnderecoId { get; set; }

        [Column("cep")]
        public string Cep { get; set; }

        [Column("pais")]
        public string Pais { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("cidade")]
        public string Cidade { get; set; }

        [Column("bairro")]
        public string Bairro { get; set; }

        [Column("rua")]
        public string Rua { get; set; }

        [Column("numero")]
        public int Numero { get; set; }

        [Column("id_usuario")]
        [ForeignKey("Usuario")]
        public long Usuario_Id { get; set; }

        public Usuario Usuario { get; set; }
    }
}
