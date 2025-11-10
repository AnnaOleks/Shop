using Shop.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShopTARgv24.RealEstateTest.Macros;
using Shop.Core.ServiceInterface;
using Shop.ApplicationServices.Services;
using Microsoft.Extensions.Hosting;
using ShopTARgv24.RealEstateTest.Mock;

namespace ShopTARgv24.RealEstateTest
{
    public abstract class TestBase
    {
        //
        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }


        //
        public virtual void SetupServices(IServiceCollection services)
        {
            //
            services.AddScoped<IRealEstateService, RealEstateServices>();


            //services.AddScoped<IFileServices, FakeFileServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockHostEnvironment>();

            //
            services.AddDbContext<ShopContext>(x =>
            {
                //
                x.UseInMemoryDatabase("TestDb");
                //
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }


        // макрос - 
        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
        }


        protected T Svc<T>() where T : notnull
        {
            return serviceProvider.GetRequiredService<T>();
        }

        // 
        //protected T Svc<T>()
        //{
        //    return serviceProvider.GetService<T>();
        //}

        //
        public void Dispose()
        {

        }

    }
}