using System;
using System.Linq;
using Api.Interfaces.ServiceOperations;
using ApplicationServices;
using ClinicsApplication.ReadModels;
using ClinicsApplication.Storage;
using ClinicsDomain;
using ClinicsStorage;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using FluentAssertions;
using InfrastructureServices.Eventing.ReadModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using Storage;
using Storage.Interfaces;
using Storage.ReadModels;
using Clinic = Application.Resources.Clinic;
using IRepository = Storage.IRepository;

namespace ClinicsApi.IntegrationTests
{
    [TestClass, TestCategory("Integration.Web")]
    public class ClinicsApiSpec
    {
        private const string ServiceUrl = "http://localhost:2000/";
        private static IWebHost webHost;
        private static IQueryStorage<Doctor> doctorQueryStorage;
        private static IEventStreamStorage<ClinicEntity> clinicEventingStorage;
        private static IQueryStorage<Unavailability> unavailabilityQueryStorage;
        private static int plateCount;
        private static IRepository repository;

        [ClassInitialize]
        public static void InitializeAllTests(TestContext context)
        {
            webHost = WebHost.CreateDefaultBuilder(null)
                .UseModularStartup<Startup>()
                .UseUrls(ServiceUrl)
                .UseKestrel()
                .ConfigureLogging((ctx, builder) => builder.AddConsole())
                .Build();
            webHost.Start();

            // Override services for testing
            var container = HostContext.Container;
            container.AddSingleton<IPersonsService, StubPersonsService>();
            repository = new InProcessInMemRepository();

            doctorQueryStorage = new GeneralQueryStorage<Doctor>(container.Resolve<ILogger>(),
                container.Resolve<IDomainFactory>(), repository);
            clinicEventingStorage = new GeneralEventStreamStorage<ClinicEntity>(container.Resolve<ILogger>(),
                container.Resolve<IDomainFactory>(),
                container.Resolve<IChangeEventMigrator>(), repository);
            unavailabilityQueryStorage = new GeneralQueryStorage<Unavailability>(container.Resolve<ILogger>(),
                container.Resolve<IDomainFactory>(), repository);

            container.AddSingleton(clinicEventingStorage);
            container.AddSingleton<IClinicStorage>(c =>
                new ClinicStorage(doctorQueryStorage, clinicEventingStorage, unavailabilityQueryStorage));
            container.AddSingleton<IReadModelProjectionSubscription>(c => new InProcessReadModelProjectionSubscription(
                c.Resolve<ILogger>(),
                new ReadModelProjector(c.Resolve<ILogger>(),
                    new ReadModelCheckpointStore(c.Resolve<ILogger>(), c.Resolve<IIdentifierFactory>(),
                        c.Resolve<IDomainFactory>(), repository),
                    c.Resolve<IChangeEventMigrator>(),
                    new ClinicEntityReadModelProjection(c.Resolve<ILogger>(), repository)),
                c.Resolve<IEventStreamStorage<ClinicEntity>>()));

            //HACK: subscribe again (see: https://forums.servicestack.net/t/integration-testing-and-overriding-registered-services/8875/5)
            HostContext.AppHost.OnAfterInit();
        }

        [ClassCleanup]
        public static void CleanupAllTests()
        {
            webHost?.StopAsync().GetAwaiter().GetResult();
        }

        [TestInitialize]
        public void Initialize()
        {
            doctorQueryStorage.DestroyAll();
            clinicEventingStorage.DestroyAll();
            unavailabilityQueryStorage.DestroyAll();
        }

        [TestMethod]
        public void WhenCreateClinic_ThenReturnsClinic()
        {
            var client = new JsonServiceClient(ServiceUrl);

            var clinic = client.Post(new CreateClinicRequest
            {
                Country = 2010,
                City = Location.Cities[0],
                Street = Location.Streets[0]
            }).Clinic;

            clinic.Address.Country.Should().Be(2010);
            clinic.Address.City.Should().Be(Location.Cities[0]);
            clinic.Address.Street.Should().Be(Location.Streets[0]);
            clinic.Owner.Id.Should().Be(CurrentCallerConstants.AnonymousUserId);
            clinic.PracticeManagers.Single().Id.Should().Be(CurrentCallerConstants.AnonymousUserId);
        }

        [TestMethod]
        public void WhenGetAvailableAndNoClinics_ThenReturnsNone()
        {
            var client = new JsonServiceClient(ServiceUrl);

            var clinics = client.Get(new SearchAvailableDoctorsRequest());

            clinics.Doctors.Count.Should().Be(0);
        }

        [TestMethod]
        public void WhenGetAvailableAndClinics_ThenReturnsAvailable()
        {
            var client = new JsonServiceClient(ServiceUrl);

            var clinic1 = RegisterClinic(client);
            var clinic2 = RegisterClinic(client);

            var datum = DateTime.UtcNow.AddDays(1);
            client.Put(new OfflineDoctorRequest
            {
                Id = clinic1.Id,
                FromUtc = datum,
                ToUtc = datum.AddDays(1)
            });

            var clinics = client.Get(new SearchAvailableDoctorsRequest
            {
                FromUtc = datum,
                ToUtc = datum.AddDays(1)
            });

            clinics.Doctors.Count.Should().Be(1);
            clinics.Doctors[0].Id.Should().Be(clinic2.Id);
        }

        [TestMethod]
        public void WhenGetDoctorAfterAddingToClinic_ThenReturnsDoctor()
        {
            var client = new JsonServiceClient(ServiceUrl);

            var clinic = RegisterClinic(client);

            var doctor = client.Post(new CreateDoctorRequest
            {
                ClinicId = clinic.Id,
                FirstName = "afirstname",
                LastName = "alastname"
            }).Doctor;

            var doctors = client.Get(new SearchAvailableDoctorsRequest()).Doctors;

            doctors.First().Id.Should().Be(doctor.Id);
            doctors.First().Name.FirstName.Should().Be("afirstname");
            doctors.First().Name.LastName.Should().Be("alastname");
        }

        private static Clinic RegisterClinic(IRestClient client)
        {
            var clinic1 = client.Post(new CreateClinicRequest
            {
                Country = 2010,
                City = Location.Cities[0],
                Street = Location.Streets[0]
            }).Clinic;
            client.Put(new RegisterClinicRequest
            {
                Id = clinic1.Id,
                Jurisdiction = "New Zealand",
                CertificateNumber = $"ABC{++plateCount:###}"
            });
            return clinic1;
        }
    }
}