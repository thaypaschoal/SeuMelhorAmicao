using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjSeuMelhorAmicao.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo Obrigatório!")]
        public string  Email{ get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        public string Senha { get; set; }



    }
}