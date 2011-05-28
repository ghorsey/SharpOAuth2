using System.Web.Mvc;
using System.Web.Routing;
using CommonServiceLocator.NinjectAdapter;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.AuthorizationEndpoint.Processor;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.ResourceEndpoint;
using SharpOAuth2.Provider.ResourceEndpoint.Processors;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Processor;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuthProvider.Domain.Service;

namespace SharpOAuth2.ProviderSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            IKernel kernel = new StandardKernel();
            // Repos
            kernel.Bind<IClientRepository>().To<InMemoryClientRepository>();
            kernel.Bind<ITokenRepository>().To<InMemoryTokenRepository>();

            // Services
            kernel.Bind<IClientService>().To<ClientService>();
            kernel.Bind<ITokenService>().To<TokenService>();
            kernel.Bind<IResourceOwnerService>().To<ResourceOwnerService>();
            kernel.Bind<IAuthorizationGrantService>().To<AuthorizationGrantService>();
            kernel.Bind<IServiceFactory>().To<ServiceFactory>();

            // Providers
            kernel.Bind<IAuthorizationProvider>().To<AuthorizationProvider>();
            kernel.Bind<ITokenProvider>().To<TokenProvider>();
            kernel.Bind<IResourceProvider>().To<ResourceProvider>();

            
            // Token Endpoint Processors
            kernel.Bind<ContextProcessor<ITokenContext>>().To<AuthenticationCodeProcessor>();
            kernel.Bind<ContextProcessor<ITokenContext>>().To<ResourceOwnerPasswordCredentialProcessor>();
            kernel.Bind<ContextProcessor<ITokenContext>>().To<ClientCredentialsProcessor>();
            kernel.Bind<ContextProcessor<ITokenContext>>().To<RefreshTokenProcessor>();

            // Resource Endpoint Processors
            //TODO: Build Mac Processor
            kernel.Bind<ContextProcessor<IResourceContext>>().To<BearerProcessor>();

            // Authorization Endpoint Processors
            kernel.Rebind<ContextProcessor<IAuthorizationContext>>().To<AuthorizationCodeProcessor>();
            kernel.Bind<ContextProcessor<IAuthorizationContext>>().To<ImplicitFlowProcessor>();
    
            NinjectServiceLocator adapter = new NinjectServiceLocator(kernel);

            ServiceLocator.SetLocatorProvider(() => adapter);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}