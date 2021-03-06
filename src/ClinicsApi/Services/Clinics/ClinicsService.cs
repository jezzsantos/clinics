﻿using System;
using Api.Common;
using Api.Interfaces;
using Api.Interfaces.ServiceOperations.Clinics;
using Api.Interfaces.ServiceOperations.Doctors;
using Application.Resources;
using ClinicsApplication;
using QueryAny.Primitives;
using ServiceStack;

namespace ClinicsApi.Services.Clinics
{
    internal class ClinicsService : Service
    {
        private readonly IClinicsApplication clinicsApplication;

        public ClinicsService(IClinicsApplication clinicsApplication)
        {
            clinicsApplication.GuardAgainstNull(nameof(clinicsApplication));

            this.clinicsApplication = clinicsApplication;
        }

        public SearchAvailableDoctorsResponse Get(SearchAvailableDoctorsRequest request)
        {
            var fromUtc = request.FromUtc.GetValueOrDefault(DateTime.MinValue);
            var toUtc = request.ToUtc.GetValueOrDefault(DateTime.MaxValue);
            var available = this.clinicsApplication.SearchAvailableDoctors(Request.ToCaller(), fromUtc, toUtc,
                request.ToSearchOptions(defaultSort: Reflector<Clinic>.GetPropertyName(c => c.Id)),
                request.ToGetOptions());
            return new SearchAvailableDoctorsResponse
            {
                Doctors = available.Results,
                Metadata = available.Metadata
            };
        }

        public CreateClinicResponse Post(CreateClinicRequest request)
        {
            var clinic =
                this.clinicsApplication.Create(Request.ToCaller(), request.Country, request.City, request.Street);
            Response.SetLocation(clinic);
            return new CreateClinicResponse
            {
                Clinic = clinic
            };
        }

        public RegisterDoctorResponse Post(RegisterDoctorRequest request)
        {
            var doctor = this.clinicsApplication.RegisterDoctor(Request.ToCaller(), request.ClinicId, request.FirstName,
                request.LastName);

            return new RegisterDoctorResponse
            {
                Doctor = doctor
            };
        }

        public RegisterClinicResponse Put(RegisterClinicRequest request)
        {
            return new RegisterClinicResponse
            {
                Clinic = this.clinicsApplication.Register(Request.ToCaller(), request.Id, request.Jurisdiction,
                    request.CertificateNumber)
            };
        }

        public OfflineDoctorResponse Put(OfflineDoctorRequest request)
        {
            return new OfflineDoctorResponse
            {
                Clinic = this.clinicsApplication.OfflineDoctor(Request.ToCaller(), request.Id, request.FromUtc,
                    request.ToUtc)
            };
        }
    }
}