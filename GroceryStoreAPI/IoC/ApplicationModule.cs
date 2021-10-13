using Autofac;
using Microsoft.Extensions.Hosting;

using GroceryStoreAPI.Configuration;
using GroceryStoreAPI.Service;
using GroceryStoreAPI.DataAccess;
using GroceryStoreAPI.BusinessLogic;

namespace GroceryStoreAPI.IoC
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Long running services
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces()
                .Except<ApiService>(service => service.As<IHostedService>().SingleInstance());

            //Registration
            builder.RegisterType<BusinessLogic.BusinessLogic>().As<IBusinessLogic>().SingleInstance();
            builder.RegisterType<DataAccess.DataAccess>().As<IDataAccess>().SingleInstance();
            builder.RegisterType<DataBaseConnectionStringConfiguration>().SingleInstance();
        }
    }
}