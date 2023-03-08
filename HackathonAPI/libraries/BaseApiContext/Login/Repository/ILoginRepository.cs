using FleetWebApi.BusinessLogic.Login.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetWebApi.BusinessLogic.Login.Repository
{
    public interface ILoginRepository
    {
        Task<string> login(LoginData loginData);
    }
}
