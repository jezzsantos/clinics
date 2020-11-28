using System;
using Application.Resources;
using Domain.Interfaces;

namespace ClinicsApplication
{
    public interface IClinicsApplication
    {
        Clinic Create(ICurrentCaller caller, int country, string city, string street);

        SearchResults<Doctor> SearchAvailableDoctors(ICurrentCaller caller, DateTime fromUtc, DateTime toUtc,
            SearchOptions searchOptions, GetOptions getOptions);

        Clinic OfflineDoctor(ICurrentCaller caller, string id, DateTime fromUtc, DateTime toUtc);

        Clinic Register(ICurrentCaller caller, string id, string jurisdiction, string certificateNumber);

        void UpdatePracticeManagerEmail(ICurrentCaller caller, string managerId, string email);

        Doctor GetDoctor(ICurrentCaller caller, string doctorId);
    }
}