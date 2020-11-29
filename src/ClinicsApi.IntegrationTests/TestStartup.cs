using ApplicationServices;
using AppointmentsApplication.Storage;
using AppointmentsDomain;
using AppointmentsStorage;
using ClinicsApplication;
using ClinicsApplication.ReadModels;
using ClinicsApplication.Storage;
using ClinicsDomain;
using ClinicsStorage;
using Domain.Interfaces.Entities;
using InfrastructureServices.Eventing.Notifications;
using InfrastructureServices.Eventing.ReadModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentsApplication;
using PaymentsApplication.Storage;
using PaymentsDomain;
using PaymentsStorage;
using ServiceStack;
using Storage;
using Storage.Interfaces;

namespace ClinicsApi.IntegrationTests
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
                        container.AddSingleton<IPersonsService, StubPersonsService>();
                        container.AddSingleton<IClinicsService, StubClinicsService>();

                        var repository = new InProcessInMemRepository();

                        container.AddSingleton<IQueryStorage<Doctor>>(new GeneralQueryStorage<Doctor>(
                            container.Resolve<ILogger>(),
                            container.Resolve<IDomainFactory>(), repository));
                        container.AddSingleton<IEventStreamStorage<ClinicEntity>>(
                            new GeneralEventStreamStorage<ClinicEntity>(
                                container.Resolve<ILogger>(),
                                container.Resolve<IDomainFactory>(),
                                container.Resolve<IChangeEventMigrator>(), repository));
                        container.AddSingleton<IQueryStorage<Unavailability>>(new GeneralQueryStorage<Unavailability>(
                            container.Resolve<ILogger>(),
                            container.Resolve<IDomainFactory>(), repository));
                        container.AddSingleton<IClinicStorage>(c =>
                            new ClinicStorage(c.Resolve<IEventStreamStorage<ClinicEntity>>(),
                                c.Resolve<IQueryStorage<Doctor>>(), c.Resolve<IQueryStorage<Unavailability>>()));

                        container.AddSingleton<IEventStreamStorage<AppointmentEntity>>(
                            new GeneralEventStreamStorage<AppointmentEntity>(
                                container.Resolve<ILogger>(),
                                container.Resolve<IDomainFactory>(),
                                container.Resolve<IChangeEventMigrator>(), repository));
                        container.AddSingleton<IAppointmentStorage>(c =>
                            new AppointmentStorage(c.Resolve<IEventStreamStorage<AppointmentEntity>>()));

                        container.AddSingleton<IEventStreamStorage<PaymentEntity>>(
                            new GeneralEventStreamStorage<PaymentEntity>(
                                container.Resolve<ILogger>(),
                                container.Resolve<IDomainFactory>(),
                                container.Resolve<IChangeEventMigrator>(), repository));
                        container.AddSingleton<IPaymentStorage>(c =>
                            new PaymentStorage(c.Resolve<IEventStreamStorage<PaymentEntity>>()));

                        container.AddSingleton<IReadModelProjectionSubscription>(c =>
                            new InProcessReadModelProjectionSubscription(
                                c.Resolve<ILogger>(), c.Resolve<IIdentifierFactory>(),
                                c.Resolve<IChangeEventMigrator>(),
                                c.Resolve<IDomainFactory>(), repository,
                                new[]
                                {
                                    new ClinicEntityReadModelProjection(c.Resolve<ILogger>(), repository)
                                },
                                c.Resolve<IEventStreamStorage<ClinicEntity>>()));

                        container.AddSingleton<IChangeEventNotificationSubscription>(c =>
                            new InProcessChangeEventNotificationSubscription(
                                c.Resolve<ILogger>(), c.Resolve<IChangeEventMigrator>(),
                                new[]
                                {
                                    new DomainEventPublisherSubscriberPair(new PersonDomainEventPublisher(),
                                        new ClinicManagerEventSubscriber(c.Resolve<IClinicsApplication>())),
                                    new DomainEventPublisherSubscriberPair(new AppointmentDomainEventPublisher(),
                                        new PaymentManagerEventSubscriber(c.Resolve<IPaymentsApplication>()))
                                },
                                c.Resolve<IEventStreamStorage<ClinicEntity>>(),
                                c.Resolve<IEventStreamStorage<AppointmentEntity>>()));
                    }
                }
            });
        }
    }
}