using System.Web.Http;
using AutoMapper;
using PhoneBookTask.Managers;
using PhoneBookTask.Managers.Interfaces;
using PhoneBookTask.Mapper;
using Unity;
using Unity.WebApi;

namespace PhoneBookTask
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var mapper = mapperConfig.CreateMapper();

            container.RegisterInstance(mapper);
            container.RegisterType<ICompanyManager, CompanyManager>();
            container.RegisterType<IPersonManager, PersonManager>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}