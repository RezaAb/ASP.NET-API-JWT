using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebApi2.Models
{
    public class JWTIdentity : GenericIdentity
    {
        public string UserName { get; set; }
        public int UserId { get; set; }

        public JWTIdentity(string userName) : base(userName)
        {
            UserName = userName;
        }
    }
}