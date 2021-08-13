using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.JWT
{
    public interface ISessionService
    {
        Task<string> CreateAuthTokenAsync(Guid leadID);
    }
}
