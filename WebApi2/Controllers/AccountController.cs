using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class AccountController : ApiController
    {
        private readonly MyRepository _repository = new MyRepository();
        public HttpResponseMessage Login(Login login)
        {
            var user = _repository.GetAllUsers()
                .FirstOrDefault(u => u.UserName == login.UserName && u.Password == login.Password);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid User",
                    Configuration.Formatters.JsonFormatter);
            }
            else
            {
                AuthenticationModule authentication = new AuthenticationModule();
                string token = authentication.GenerateTokenForUser(user.UserName, user.Id);
                return Request.CreateResponse(HttpStatusCode.OK, token, Configuration.Formatters.JsonFormatter);
            }
        }
    }
}
