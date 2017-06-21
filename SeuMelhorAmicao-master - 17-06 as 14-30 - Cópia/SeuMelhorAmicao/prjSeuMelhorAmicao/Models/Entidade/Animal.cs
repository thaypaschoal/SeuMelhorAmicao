using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models.Entidade
{
    public class Animal
    {
        public Animal()
        {
            Cliente = new List<Cliente>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(1)] 
        public string Genero { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataEntrada { get; set; }

        [Required]
        [StringLength(15)]
        public string Especie { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        public byte[] Foto { get; set; }

        public int? ONGId { get; set; }

        public virtual Ong Ong { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}