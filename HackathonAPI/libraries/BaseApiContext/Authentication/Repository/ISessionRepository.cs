using WebApi.Core.Authentication.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Core.Authentication.Repository
{
    public interface ISessionRepository
    {
        Session GetActiveByKey(string sessionKey);
        Session GetActiveByB2BKey(string sessionKey);
        Task<LoginDTO> Authenticate(string username, string password, string ipAddress, bool keepActiveSessions = false);
        Task<bool> KeepAlive(string sessionKey);
        bool Logout();
    }
}
