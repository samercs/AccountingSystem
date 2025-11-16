using AccountingSystem.Core.Service;
using AccountingSystem.Data;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace AccountingSystem
{
    public class AutofacConfig
    {
        public static void ConfigureContainer(ContainerBuilder builder, IConfiguration configuration)
        {
            // Register services
            builder.RegisterType<AppService>().As<IAppService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<CookieService>().As<ICookieService>().InstancePerLifetimeScope();
            builder.RegisterType<DataContextFactory>().As<IDataContextFactory>()
                .WithParameter(new TypedParameter(typeof(IConfiguration), configuration))
                .SingleInstance();
        }
    }
}
