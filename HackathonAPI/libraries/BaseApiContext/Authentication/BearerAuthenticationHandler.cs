using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApi.Core.Authentication
{
    public class BearerAuthenticationHandler: DelegatingHandler
    {
        private const string WWWAuthenticateHeader = "WWW-Authenticate";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            BearerAuthenticationIdentity credentials = ParseAuthorizationHeader(request);

            if (credentials != null)
            {
                var identity = new BearerAuthenticationIdentity(credentials.SessionKey);
                var principal = new GenericPrincipal(identity, null);

                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;
            }

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (credentials == null && response.StatusCode == HttpStatusCode.Unauthorized)
                        Challenge(request, response);

                    return response;
                });
        }

        /// <summary>
        /// Parses the Authorization header and creates user credentials
        /// </summary>
        /// <param name="request"></param>
        protected virtual BearerAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage request)
        {
            if (request.Headers.Authorization == null)
                return null;
            if (request.Headers.Authorization.Scheme != "Bearer" || String.IsNullOrEmpty(request.Headers.Authorization.Parameter))
                return null;

            return new BearerAuthenticationIdentity(request.Headers.Authorization.Parameter);
        }


        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void Challenge(HttpRequestMessage request, HttpResponseMessage response)
        {
            var host = request.RequestUri.DnsSafeHost;
            //response.Headers.Add(WWWAuthenticateHeader, string.Format("Basic realm=\"{0}\"", host));
        }
    }
}