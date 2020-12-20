using System;
using System.Collections.Generic;
using AppointmentsDomain;
using ClinicsDomain;
using Domain.Interfaces.Entities;
using PaymentsDomain;
using Storage.Interfaces.ReadModels;

namespace ClinicsApi
{
    public class ClinicsIdentifierFactory : EntityPrefixIdentifierFactory
    {
        public ClinicsIdentifierFactory() : base(new Dictionary<Type, string>
        {
            {typeof(Checkpoint), "ckp"},
            {typeof(ClinicEntity), "cin"},
            {typeof(UnavailabilityEntity), "una"},
            {typeof(ClinicDoctor), "per"},
            {typeof(AppointmentEntity), "apt"},
            {typeof(PaymentEntity), "pay"}
        })
        {
        }
    }
}