using Microsoft.Extensions.DependencyInjection;
using VeryBigShoeCompany.Repositories.Orders;
using VeryBigShoeCompany.RepositoryContracts.Orders;
using VeryBigShoeCompany.ServiceContracts.Orders;
using VeryBigShoeCompany.Services.Orders;

namespace VeryBigShoeCompany.API.ExtensionMethods
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrations(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderValidatorService, OrderValidatorService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
