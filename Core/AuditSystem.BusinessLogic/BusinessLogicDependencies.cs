using System;
using AuditSystem.BusinessLogic.Services.EntityServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;
using AuditSystem.BusinessLogic.Services.Transaction;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Transaction;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.BusinessLogic
{
    public static class BusinessLogicDependencies
    {
        public static IServiceCollection AddDomain(this IServiceCollection services) =>
            services
                .AddMapper()
                .AddServices();

        private static IServiceCollection AddMapper(this IServiceCollection services) =>
            services
                .AddAutoMapper(typeof(BusinessLogicDependencies).Assembly);

        private static IServiceCollection AddServices(this IServiceCollection services) =>
            services
                .AddScoped<ITransaction, Transaction>()
                .AddScoped<IRiskService, RiskService>();
    }
}