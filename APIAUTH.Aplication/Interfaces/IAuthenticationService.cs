using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(string idToken, string accessToken)> AuthenticateUserAsync(string username, string password);
        Task<(string idToken, string accessToken)> AuthenticateWithGoogleAsync(string authorizationCode);
    }
}
