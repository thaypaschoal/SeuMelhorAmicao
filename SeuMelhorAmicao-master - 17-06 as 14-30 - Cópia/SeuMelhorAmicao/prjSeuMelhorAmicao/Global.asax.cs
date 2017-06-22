using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace prjSeuMelhorAmicao
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Autenticação por requisição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            //Se a requisição esta autenticada, 
            //efetua o login e cria o ticket esta autenticado
            if (Request.IsAuthenticated)
            {
                //Get no cookie gerado pelo formsAutenticate
                HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                //Se for nulo ou vazio return vazio
                if (authCookie == null || authCookie.Value == "")
                    return;

                //Cria o ticket 
                FormsAuthenticationTicket authTicket;

                try
                {
                    //E vai pegar o valor do
                    //cookie onde contem o ticket que é criptografado na criação
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch
                {
                    return;
                }
                //Todas as permissões que o usuario pode ter
                string[] roles = authTicket.UserData.Split(';');

                //Criar o contextUser
                if (Context.User != null) // Cria o usuario com a identidade e as permissões
                    Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }

        }

        /// <summary>
        /// Verificãção de Autenticidade do Form Authentication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            //Se formAuthentication suporta cookie
            if (FormsAuthentication.CookiesSupported == true)
            {
                //Verifica se cookie diferente de null
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //Pega o cookie
                        HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                        FormsAuthenticationTicket authTicket;

                        try
                        {
                            //Pega o ticket dentro do cookie
                            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        }
                        catch
                        {
                            return;
                        }
                        //Pega as permissoes
                        string[] roles = authTicket.UserData.Split(';');

                        //Cria o Context.User passando a identidade e as permissões
                        if (Context.User != null)
                            Context.User = new GenericPrincipal(Context.User.Identity, roles);

                        //Cria uma identidade passando o nome o tipo e as permissoes
                        e.User = new GenericPrincipal(new GenericIdentity(authTicket.Name, "Forms"), authTicket.UserData.Split(';'));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Autenticação de Post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            //Se formAuthentication suporta cookie
            if (FormsAuthentication.CookiesSupported == true)
            {
                //Verifica se cookie diferente de null
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //Pega o cookie
                        HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                        FormsAuthenticationTicket authTicket;

                        try
                        {
                            //Pega o ticket dentro do cookie
                            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        }
                        catch
                        {
                            return;
                        }
                        //Pega as permissoes
                        string[] roles = authTicket.UserData.Split(';');

                        //Cria o Context.User passando a identidade e as permissões
                        if (Context.User != null)
                            Context.User = new GenericPrincipal(Context.User.Identity, roles);

                        //Cria o usuario no httpContext
                        HttpContext.Current.User = new GenericPrincipal(
                            new GenericIdentity(authTicket.Name, "Forms"), authTicket.UserData.Split(';'));


                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}
 