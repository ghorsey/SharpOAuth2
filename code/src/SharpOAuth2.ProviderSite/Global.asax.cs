using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuthProvider.Domain;
using SharpOAuthProvider.Domain.Service;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using CommonServiceLocator.NinjectAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Services;

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
            kernel.Bind<IClientRepository>().To<InMemoryClientRepository>();
            kernel.Bind<ITokenRepository>().To<InMemoryTokenRepository>();
            kernel.Bind<IClientService>().To<ClientService>();
            kernel.Bind<ITokenService>().To<TokenService>();
            kernel.Bind<IAuthorizationServiceFactory>().To<ServiceFactory>();
            kernel.Bind<IAuthorizationProvider>().To<AuthorizationProvider>();
                
            NinjectServiceLocator adapter = new NinjectServiceLocator(kernel);

            ServiceLocator.SetLocatorProvider(() => adapter);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}