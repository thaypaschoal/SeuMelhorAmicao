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


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            
            if (Request.IsAuthenticated)
            {
            
                HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            
                if (authCookie == null || authCookie.Value == "")
                    return;

            
                FormsAuthenticationTicket authTicket;

                try
                {
            
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch
                {
                    return;
                }
            
                string[] roles = authTicket.UserData.Split(';');

            
                if (Context.User != null) 
                    Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }

        }

        
        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
          
            if (FormsAuthentication.CookiesSupported == true)
            {
       
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
             
                        HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                        FormsAuthenticationTicket authTicket;

                        try
                        {
            
                            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        }
                        catch
                        {
                            return;
                        }
          
                        string[] roles = authTicket.UserData.Split(';');


                        if (Context.User != null)
                            Context.User = new GenericPrincipal(Context.User.Identity, roles);


                        e.User = new GenericPrincipal(new GenericIdentity(authTicket.Name, "Forms"), authTicket.UserData.Split(';'));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {

            if (FormsAuthentication.CookiesSupported == true)
            {

                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    { 
                        HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                        FormsAuthenticationTicket authTicket;

                        try
                        {
                            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                        }
                        catch
                        {
                            return;
                        }

                        string[] roles = authTicket.UserData.Split(';');

                        
                        if (Context.User != null)
                            Context.User = new GenericPrincipal(Context.User.Identity, roles);

                        
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
 