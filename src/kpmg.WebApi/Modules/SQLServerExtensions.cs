#region

using kpmg.Core.AirplaneCore;
using kpmg.Core.BaCargoCore;
using kpmg.Core.BaFilialCore;
using kpmg.Core.BaLogradouroCore;
using kpmg.Core.BaParamCore;
using kpmg.Core.BaTipoCargoCore;
using kpmg.Core.BaUsuCore;
using kpmg.Core.BaUsuFilialCore;
using kpmg.Core.BaUsuPermissaoCore;
using kpmg.Core.Helpers.Interfaces;
using kpmg.Core.Views.VBaUsuPermissaoCore;
using kpmg.Infrastructure.DataAccess;
using kpmg.Infrastructure.Repositories;
using kpmg.Infrastructure.Repositories.Views;
using kpmg.WebApi.Modules.Common.FeatureFlags;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

#endregion

namespace kpmg.WebApi.Modules
{
    /// <summary>
    ///     Persistence Extensions.
    /// </summary>
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SqlServerExtensions
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        ///     Add Persistence dependencies varying on configuration.
        /// </summary>
        public static IServiceCollection AddSqlServer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            IFeatureManager featureManager = services
                .BuildServiceProvider()
                .GetRequiredService<IFeatureManager>();

            var isEnabled = featureManager
                .IsEnabledAsync(nameof(CustomFeature.SqlServer))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();


            services.AddDbContext<KpmgContext>(
                options => options.UseSqlServer(
                    configuration.GetValue<string>("PersistenceModule:DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAirplaneRepository, AirplaneRepository>();
            services.AddScoped<IBaCargoRepository, BaCargoRepository>();
            services.AddScoped<IBaFilialRepository, BaFilialRepository>();
            services.AddScoped<IBaLogradouroRepository, BaLogradouroRepository>();
            services.AddScoped<IBaParamRepository, BaParamRepository>();
            services.AddScoped<IBaTipoCargoRepository, BaTipoCargoRepository>();
            services.AddScoped<IBaUsuFilialRepository, BaUsuFilialRepository>();
            services.AddScoped<IBaUsuRepository, BaUsuRepository>();
            services.AddScoped<IBaUsuPermissaoRepository, BaUsuPermissaoRepository>();
            services.AddScoped<IVBaUsuPermissaoRepository, VBaUsuPermissaoRepository>();

            return services;
        }
    }
}