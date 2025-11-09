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

        public virtual void SetupServices(IServiceCollection services)
        {

            services.AddScoped<IRealEstateService, RealEstateServices>();

            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockHostEnvironment>();

            services.AddSingleton<IHostEnvironment>(new TestHostEnvironment());

            services.AddDbContext<ShopContext>(x =>
            {
                //
                x.UseInMemoryDatabase("TestDb");
                //
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            RegisterMacros(services);
        }

        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);
            var macroTypes = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && t.IsInterface && !t.IsAbstract);
        }

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public void Dispose()
        {
        }
    }
}