using System;
using System.Collections.Generic;
using Domain;
using Domain.Interfaces.Entities;

namespace ClinicsDomain
{
    public static class Events
    {
        public static class Clinic
        {
            public class Created : IChangeEvent
            {
                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static Created Create(Identifier id)
                {
                    return new Created
                    {
                        EntityId = id,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class LocationChanged : IChangeEvent
            {
                public int Country { get; set; }

                public string City { get; set; }

                public string Street { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static LocationChanged Create(Identifier id, Location location)
                {
                    return new LocationChanged
                    {
                        EntityId = id,
                        Country = location.Country,
                        City = location.City,
                        Street = location.Street,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class OwnershipChanged : IChangeEvent
            {
                public string Owner { get; set; }

                public List<string> Managers { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static OwnershipChanged Create(Identifier id, ClinicOwner owner)
                {
                    return new OwnershipChanged
                    {
                        EntityId = id,
                        Owner = owner.OwnerId,
                        Managers = new List<string> {owner.OwnerId},
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class DoctorAddedToClinic : IChangeEvent
            {
                public string DoctorId { get; set; }

                public string FirstName { get; set; }

                public string LastName { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static DoctorAddedToClinic Create(Identifier id, ClinicDoctor doctor)
                {
                    return new DoctorAddedToClinic
                    {
                        EntityId = id,
                        DoctorId = doctor.Id,
                        FirstName = doctor.FirstName,
                        LastName = doctor.LastName,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class RegistrationChanged : IChangeEvent
            {
                public string Jurisdiction { get; set; }

                public string Number { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static RegistrationChanged Create(Identifier id, ClinicLicense license)
                {
                    return new RegistrationChanged
                    {
                        EntityId = id,
                        Jurisdiction = license.Jurisdiction,
                        Number = license.CertificateNumber,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class DoctorUnavailabilitySlotAdded : IChangeEvent
            {
                public string ClinicId { get; set; }

                public DateTime From { get; set; }

                public DateTime To { get; set; }

                public UnavailabilityCausedBy CausedBy { get; set; }

                public string CausedByReference { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static DoctorUnavailabilitySlotAdded Create(Identifier clinicId, TimeSlot slot,
                    UnavailabilityCausedBy causedBy, string causedById)
                {
                    return new DoctorUnavailabilitySlotAdded
                    {
                        ClinicId = clinicId,
                        From = slot.From,
                        To = slot.To,
                        CausedBy = causedBy,
                        CausedByReference = causedById,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }
        }
    }
}