using Domain.Interfaces.Entities;
using InfrastructureServices.Eventing.ReadModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonsApplication.ReadModels;
using PersonsApplication.Storage;
using PersonsDomain;
using PersonsStorage;
using ServiceStack;
using Storage;
using Storage.Interfaces;

namespace PersonsApi.IntegrationTests
{
    public class TestStartup : ModularStartup
    {
        public new void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new ServiceHost
            {
                AppSettings = new NetCoreAppSettings(Configuration),
                AfterConfigure =
                {
                    host =>
                    {
                        // Override services for testing
                        var container = host.GetContainer();

                        var repository = new InProcessInMemRepository();

                        container.AddSingleton<IQueryStorage<Person>>(new GeneralQueryStorage<Person>(
                            container.Resolve<ILogger>(),
                            container.Resolve<IDomainFactory>(), repository));
                        container.AddSingleton<IEventStreamStorage<PersonEntity>>(
                            new GeneralEventStreamStorage<PersonEntity>(
                                container.Resolve<ILogger>(),
                                container.Resolve<IDomainFactory>(),
                                container.Resolve<IChangeEventMigrator>(), repository));
                        container.AddSingleton<IPersonStorage>(c =>
                            new PersonStorage(c.Resolve<IEventStreamStorage<PersonEntity>>(),
                                c.Resolve<IQueryStorage<Person>>()));

                        container.AddSingleton<IReadModelProjectionSubscription>(c =>
                            new InProcessReadModelProjectionSubscription(
                                c.Resolve<ILogger>(), c.Resolve<IIdentifierFactory>(),
                                c.Resolve<IChangeEventMigrator>(),
                                c.Resolve<IDomainFactory>(), repository,
                                new[]
                                {
                                    new PersonEntityReadModelProjection(c.Resolve<ILogger>(), repository)
                                },
                                c.Resolve<IEventStreamStorage<PersonEntity>>()));
                    }
                }
            });
        }
    }
}