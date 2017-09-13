using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Aurochses.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Aurochses.IdentityServer.WebSite.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        public TestServerFixture()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(
                    ProjectHelpers.GetFolderPath("Aurochses.IdentityServer.WebSite", "src", "Aurochses.IdentityServer.WebSite")
                )
                .UseEnvironment("Production")
                .UseStartup<Startup>()
                // todo: this is temp solution: https://github.com/aspnet/Hosting/issues/954#issuecomment-287482546
                .ConfigureServices(services =>
                {
                    services.Configure((RazorViewEngineOptions options) =>
                    {
                        var previous = options.CompilationCallback;
                        options.CompilationCallback = context =>
                        {
                            previous?.Invoke(context);

                            var assembly = typeof(Startup).GetTypeInfo().Assembly;
                            var assemblies =
                                assembly.GetReferencedAssemblies()
                                    .Select(x => MetadataReference.CreateFromFile(Assembly.Load(x).Location))
                                    .ToList();
                            assemblies.Add(
                                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("System.Private.Corelib")).Location));
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Razor")).Location));
                            // Microsoft.AspNetCore.Razor.Runtime
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Razor.Runtime")).Location));
                            // Microsoft.AspNetCore.Html.Abstractions
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Html.Abstractions")).Location));
                            // Microsoft.AspNetCore.Mvc
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc")).Location));
                            // Microsoft.AspNetCore.Mvc.Abstractions
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Abstractions")).Location));
                            // Microsoft.AspNetCore.Mvc.Core
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Core")).Location));
                            // Microsoft.AspNetCore.Mvc.Localization
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Localization")).Location));
                            // Microsoft.AspNetCore.Mvc.ViewFeatures
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.ViewFeatures")).Location));
                            // Aurochses.Html
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("Aurochses.Html")).Location));
                            // System.Dynamic.Runtime
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("System.Dynamic.Runtime")).Location));
                            // System.IO
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("System.IO")).Location));
                            // System.Linq.Expressions
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("System.Linq.Expressions")).Location));
                            // System.Security.Claims
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("System.Security.Claims")).Location));
                            // System.Text.Encodings.Web
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("System.Text.Encodings.Web")).Location));
                            // IdentityServer4
                            assemblies.Add(
                                MetadataReference.CreateFromFile(
                                    Assembly.Load(new AssemblyName("IdentityServer4")).Location));

                            context.Compilation = context.Compilation.AddReferences(assemblies);
                        };
                    });
                });

            Server = new TestServer(webHostBuilder);

            Client = Server.CreateClient();
        }

        protected TestServer Server { get; }

        public HttpClient Client { get; }

        public virtual void Dispose()
        {
            Client.Dispose();

            Server.Dispose();
        }
    }
}