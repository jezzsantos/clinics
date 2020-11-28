using System;
using System.Collections.Generic;
using ClinicsApplication.ReadModels;
using ClinicsDomain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;

namespace ClinicsApplication.Storage
{
    public interface IClinicStorage
    {
        ClinicEntity Load(Identifier id);

        ClinicEntity Save(ClinicEntity clinic);

        List<Doctor> SearchAvailableDoctors(DateTime fromUtc, DateTime toUtc, SearchOptions options);

        Doctor GetDoctor(Identifier doctorId);
    }
}