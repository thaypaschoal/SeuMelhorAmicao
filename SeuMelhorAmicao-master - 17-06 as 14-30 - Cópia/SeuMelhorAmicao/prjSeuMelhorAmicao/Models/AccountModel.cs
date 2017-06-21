using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace prjSeuMelhorAmicao.Models
{
    public class AccountModel
    {
        
        public HttpCookie Credenciar(Usuario usuario)
        {
            //Adiciona a permissao que o usuario tem
            string roles = usuario.Perfil.Tipo.ToString()  + ";";

            //Cria o ticket de autenticacao
            var authTicket = new FormsAuthenticationTicket
                (1,
                usuario.Nome,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                roles,
                FormsAuthentication.FormsCookiePath);

            //Encriptografa
            var authCrypt = FormsAuthentication.Encrypt(authTicket);

            //Nome e permissoes
            //Cria o cookie
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, authCrypt);
            //e retorna
            return authCookie;
        }
    }
}