using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            services.AddDbContext<ShopContext>(X =>
            {
                X.UseInMemoryDatabase("TestDb");
                X.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        }

        private void RegisterMacros(IServiceCollection services)
        {
        }

        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();
        }
    }
}