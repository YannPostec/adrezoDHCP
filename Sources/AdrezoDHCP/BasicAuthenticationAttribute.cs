using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Configuration;

namespace AdrezoDHCP
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        // Get variables from config file
        string myhost = ConfigurationManager.AppSettings["web_host"];
        string myport = ConfigurationManager.AppSettings["web_port"];
        string myuser = ConfigurationManager.AppSettings["web_user"];
        string mypwd = ConfigurationManager.AppSettings["web_pwd"];

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var userName = usernamePasswordArray[0];
                var password = usernamePasswordArray[1];

                var isValid = userName == myuser && password == mypwd;

                if (isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                    Thread.CurrentPrincipal = principal;

                    return;
                }
            }

            HandleUnathorized(actionContext,myhost,myport);
        }

        private static void HandleUnathorized(HttpActionContext actionContext, String host, String port)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='AdrezoDHCP' location = 'http://" + host + ":" + port + "'");
        }
    }
}
