using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.Database.Data.App.AutoMapper
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(Startup).Assembly
            );
        }
    }
}