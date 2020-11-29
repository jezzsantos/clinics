using System;
using System.Linq;
using Api.Interfaces.ServiceOperations.Appointments;
using Api.Interfaces.ServiceOperations.Clinics;
using Api.Interfaces.ServiceOperations.Doctors;
using AppointmentsDomain;
using ClinicsApplication.ReadModels;
using ClinicsDomain;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentsDomain;
using ServiceStack;
using Storage.Interfaces;
using Clinic = Application.Resources.Clinic;

namespace ClinicsApi.IntegrationTests
{
    [TestClass, TestCategory("Integration.Web")]
    public class ClinicsApiSpec
    {
        private const string ServiceUrl = "http://localhost:2000/";
        private static IWebHost webHost;
        private static IQueryStorage<Doctor> doctorQueryStorage;
        private static IEventStreamStorage<ClinicEntity> clinicEventingStorage;
        private static IEventStreamStorage<AppointmentEntity> appointmentEventingStorage;
        private static IEventStreamStorage<PaymentEntity> paymentEventingStorage;
        private static IQueryStorage<Unavailability> unavailabilityQueryStorage;
        private static int plateCount;

        [ClassInitialize]
        public static void InitializeAllTests(TestContext context)
        {
            webHost = WebHost.CreateDefaultBuilder(null)
                .UseModularStartup<TestStartup>()
                .UseUrls(ServiceUrl)
                .UseKestrel()
                .ConfigureLogging((ctx, builder) => builder.AddConsole())
                .Build();
            webHost.Start();

            var container = HostContext.Container;
            doctorQueryStorage = container.Resolve<IQueryStorage<Doctor>>();
            unavailabilityQueryStorage = container.Resolve<IQueryStorage<Unavailability>>();
            clinicEventingStorage = container.Resolve<IEventStreamStorage<ClinicEntity>>();
            appointmentEventingStorage = container.Resolve<IEventStreamStorage<AppointmentEntity>>();
            paymentEventingStorage = container.Resolve<IEventStreamStorage<PaymentEntity>>();
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
            unavailabilityQueryStorage.DestroyAll();
            clinicEventingStorage.DestroyAll();
            appointmentEventingStorage.DestroyAll();
            paymentEventingStorage.DestroyAll();
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

            var doctor = client.Post(new RegisterDoctorRequest
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

        [TestMethod]
        public void WhenEndAppointment_ThenAppointmentEnded()
        {
            var client = new JsonServiceClient(ServiceUrl);

            var clinic = RegisterClinic(client);

            var doctor = client.Post(new RegisterDoctorRequest
            {
                ClinicId = clinic.Id,
                FirstName = "afirstname",
                LastName = "alastname"
            }).Doctor;

            var appointment = client.Post(new CreateAppointmentRequest
            {
                DoctorId = doctor.Id,
                StartUtc = DateTime.UtcNow.AddHours(1),
                EndUtc = DateTime.UtcNow.AddHours(2)
            }).Appointment;

            client.Patch(new EndAppointmentRequest
            {
                Id = appointment.Id
            });
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