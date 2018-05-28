using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi2.Models
{
    public class JWTFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsUserAuthorized(actionContext))
            {
                var response = new {Code = 401, Message = "Unable to access, Please login again"};
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);
                return;
            }
            base.OnAuthorization(actionContext);
        }

        private bool IsUserAuthorized(HttpActionContext actionContext)
        {
            var requestToken = actionContext.Request.Headers.Authorization;
            string authHeader = null;
            if (requestToken != null)
            {
                authHeader = requestToken.Parameter;
            }
            if (authHeader!=null)
            {
                var auth = new AuthenticationModule();
                var userValidatedToken = auth.GenerateUserClaimFromJWt(authHeader);
                if (userValidatedToken != null)
                {
                    var identity = auth.PopulateUserIdentity(userValidatedToken);
                    string[] roles = {"All"};
                    var generalPrincipal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = generalPrincipal;
                    var authIdentity = Thread.CurrentPrincipal.Identity as JWTIdentity;
                    if (authIdentity != null && !string.IsNullOrEmpty(authIdentity.UserName))
                    {
                        authIdentity.UserId = identity.UserId;
                        authIdentity.UserName = identity.UserName;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}