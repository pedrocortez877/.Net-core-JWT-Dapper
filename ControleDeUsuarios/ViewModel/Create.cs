using ApiWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeUsuarios.ViewModel
{
    public class Create
    {

        //USUARIO
        public long UsuarioId { get; set; }

        [MaxLength(200, ErrorMessage = "Nome pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Nome { get; set; }

        [MaxLength(11, ErrorMessage = "CPF pode ter no máximo 11 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Cpf { get; set; }

        [MaxLength(50, ErrorMessage = "Permissão pode ter no máximo  carcteres")]
        [Required(ErrorMessage = "*")]
        public string Permissao { get; set; }

        [EmailAddress]
        [MaxLength(100, ErrorMessage = "E-mail pode ter no máximo 100 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        [MaxLength(200, ErrorMessage = "Senha pode ter no máximo 200 carcteres")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string Senha { get; set; }

        public string ConfirmaSenha { get; set; }

        //ENDEREÇO

        [Key]
        public long EnderecoId { get; set; }

        [MaxLength(20, ErrorMessage = "Cep pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Cep { get; set; }

        [MaxLength(200, ErrorMessage = "Pais pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Pais { get; set; }

        [MaxLength(200, ErrorMessage = "Estado pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Estado { get; set; }

        [MaxLength(200, ErrorMessage = "Cidade pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Cidade { get; set; }

        [MaxLength(200, ErrorMessage = "Cidade pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Bairro { get; set; }

        [MaxLength(200, ErrorMessage = "Bairro pode ter no máximo 200 carcteres")]
        [Required(ErrorMessage = "*")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "*")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "*")]
        public long Usuario_Id { get; set; }


    }
}
