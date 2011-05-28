using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CommonServiceLocator.NinjectAdapter;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.ResourceEndpoint;
using SharpOAuth2.Provider.ResourceEndpoint.Processors;
using SharpOAuth2.Provider.Services;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuthProvider.Domain.Service;

namespace RESTSample
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

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IKernel kernel = new StandardKernel();
            // Repos
            kernel.Bind<IClientRepository>().To<InMemoryClientRepository>();
            kernel.Bind<ITokenRepository>().To<ShamTokenRepo>();

            // Services
            kernel.Bind<IClientService>().To<ClientService>();
            kernel.Bind<ITokenService>().To<TokenService>();
            kernel.Bind<IResourceOwnerService>().To<ResourceOwnerService>();
            kernel.Bind<IAuthorizationGrantService>().To<AuthorizationGrantService>();
            kernel.Bind<IServiceFactory>().To<ServiceFactory>();

            //Providers 
            kernel.Bind<IResourceProvider>().To<ResourceProvider>();

            // Resource Endpoint Processors
            //TODO: Build Mac Processor
            kernel.Bind<ContextProcessor<IResourceContext>>().To<BearerProcessor>();

            NinjectServiceLocator adapter = new NinjectServiceLocator(kernel);

            ServiceLocator.SetLocatorProvider(() => adapter);

        }


        //protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        //{
        //    IResourceProvider provider = ServiceLocator.Current.GetInstance<IResourceProvider>();

        //    IResourceContext context = new ResourceContextBuilder().FromHttpRequest(new HttpRequestWrapper(HttpContext.Current.Request));

        //    provider.AccessProtectedResource(context);

        //    TokenPrincipal principal = new TokenPrincipal(new GenericIdentity(context.Token.Token, "OAuth"), context.Token.Scope, context.Token);


        //    HttpContext.Current.User = principal;

        //}
    }
}