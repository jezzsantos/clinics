using System;
using System.Collections.Generic;
using AppointmentsDomain;
using ClinicsDomain;
using Domain.Interfaces.Entities;
using PaymentsDomain;
using Storage.Interfaces.ReadModels;

namespace ClinicsApplication
{
    public class ClinicIdentifierFactory : EntityPrefixIdentifierFactory
    {
        //HACK: Can only have one instance of the IIdentifierFactory in the DI container at a time.
        //HACK: need a better way of providing a shared IIdentifierFactory across domains that share the same Infrastructure
        public ClinicIdentifierFactory() : base(new Dictionary<Type, string>
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