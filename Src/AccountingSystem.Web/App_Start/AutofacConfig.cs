using AccountingSystem.Data;
using Autofac;
using Autofac.Integration.Mvc;
using AccountingSystem.Core.Service;
using System.Reflection;
using System.Web.Mvc;

namespace AccountingSystem
{
    public class AutofacConfig
    {
        public static IContainer RegisterAll()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());


            builder.RegisterType<AppService>().As<IAppService>().InstancePerRequest();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerRequest();
            builder.RegisterType<CookieService>().As<ICookieService>().InstancePerRequest();
            builder.RegisterType<DataContextFactory>().As<IDataContextFactory>().SingleInstance();
            return Container(builder);
        }

        private static IContainer Container(ContainerBuilder builder)
        {
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            return container;
        }
    }
}
