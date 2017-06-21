using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.Entidade
{
    public class Ong : Usuario 
    {

        public Ong()
        {
            Animal = new List<Animal>();
        }


        public int OngId { get; set; }

        [Required]
        [StringLength(100)]
        public string Endereco { get; set; }

        [Required]
        [StringLength(15)] 
        public string Telefone { get; set; }

        [Required]
        [StringLength(200)]
        public string Biografia { get; set; }

        public byte[] Foto { get; set; }


        public ICollection<Animal> Animal { get; set; }

    }
}