using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.Entidade
{
    public class Perfil
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string Tipo { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
} 