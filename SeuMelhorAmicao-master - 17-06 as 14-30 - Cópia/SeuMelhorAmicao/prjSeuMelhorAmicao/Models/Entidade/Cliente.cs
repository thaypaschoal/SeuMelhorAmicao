using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.Entidade
{
    public class Cliente : Usuario
    {
        public Cliente()
        {
            Animal = new List<Animal>();
        }

        public int ClienteId { get; set; }

        [StringLength(1)]
        public string Genero { get; set; }
        
        public DateTime? DataNascimento { get; set; }

        public int Conta { get; set; }

        public ICollection<Animal> Animal { get; set; }
    } 
}