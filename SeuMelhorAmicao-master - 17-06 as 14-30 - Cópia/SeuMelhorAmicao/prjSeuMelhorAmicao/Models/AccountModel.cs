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
            string roles = string.Join(";", usuario.Perfil.Tipo);

            var authTicket = new FormsAuthenticationTicket
                (1,
                usuario.Nome,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                roles,
                FormsAuthentication.FormsCookiePath);


            var authCrypt = FormsAuthentication.Encrypt(authTicket);

            //Nome e permissoes
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, authCrypt);

            return authCookie;
        }
    }
}