using Library.Application;
using Library.Domain.Repositories;
using Library.Domain.Services;
using Library.Infrastructure;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Library
{
    public static class UnityConfig
    {
        private static IUnityContainer _container;

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IClientRepository, ClientRepository>();
            container.RegisterType<IBookRepository, BookRepository>();
            container.RegisterType<IIssuanceRepository, IssuanceRepository>();

            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<ILibraryService, LibraryService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            _container = container;
        }

        public static IUnityContainer GetConfiguredContainer()
        {
            return _container;
        }
    }
}