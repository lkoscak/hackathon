using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApi.Core.Authentication.Repository.Model;

namespace WebApi.Core.Authentication
{
    public class BearerAuthenticationIdentity: GenericIdentity
    {
        public string SessionKey { get; set; }
        public Session Session { get; set; }

        public BearerAuthenticationIdentity(string sessionKey) : base(sessionKey, "Bearer")
        {
            this.SessionKey = sessionKey;
        }
    }
}