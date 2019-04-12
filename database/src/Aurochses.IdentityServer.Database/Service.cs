using Aurochses.Database.EntityFrameworkCore;
using Aurochses.IdentityServer.Database.Context;

namespace Aurochses.IdentityServer.Database
{
    public class Service : ServiceBase
    {
        public Service(BaseContext baseContext, IdentityContext workflowContext, IdentityServerConfigurationContext identityServerConfigurationContext, IdentityServerPersistedGrantContext identityServerPersistedGrantContext)
            : base(baseContext, workflowContext, identityServerConfigurationContext, identityServerPersistedGrantContext)
        {

        }
    }
}