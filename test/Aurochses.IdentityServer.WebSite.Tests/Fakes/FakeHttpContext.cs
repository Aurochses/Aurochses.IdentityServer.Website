using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace Aurochses.IdentityServer.WebSite.Tests.Fakes
{
    public class FakeHttpContext : DefaultHttpContext
    {
        private readonly AuthenticationManager _authenticationManager;

        public FakeHttpContext(AuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        protected override AuthenticationManager InitializeAuthenticationManager()
        {
            return _authenticationManager;
        }
    }
}