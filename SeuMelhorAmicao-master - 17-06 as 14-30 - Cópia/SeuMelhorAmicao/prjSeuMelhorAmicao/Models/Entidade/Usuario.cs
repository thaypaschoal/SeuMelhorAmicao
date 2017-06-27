using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.Entidade
{
    public class Usuario
    {
        public Usuario()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Nome { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Required]
        public string Senha { get; set; }

        
        public Perfil Perfil { get; set; }
    }
}