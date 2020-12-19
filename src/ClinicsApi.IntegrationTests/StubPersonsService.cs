using System;
using Application.Resources;
using ApplicationServices;
using DomainServices;
using Microsoft.Extensions.Logging.Abstractions;
using PersonsApplication;
using PersonsDomain;
using PersonName = Application.Resources.PersonName;

namespace ClinicsApi.IntegrationTests
{
    public class StubPersonsService : IPersonsService
    {
        public Person Get(string id)
        {
            return new Person
            {
                Id = id,
                Name = new PersonName
                {
                    FirstName = "afirstname",
                    LastName = "alastname"
                }
            };
        }

        public Person Create(string firstName, string lastName)
        {
            var idFactory = new PersonIdentifierFactory();
            return new Person
            {
                Id = idFactory.Create(new PersonAggregate(NullLogger.Instance, idFactory, new FakeEmailService())),
                Name = new PersonName
                {
                    FirstName = firstName,
                    LastName = lastName
                }
            };
        }
    }

    public class FakeEmailService : IEmailService
    {
        public bool EnsureEmailIsUnique(string emailAddress, string personId)
        {
            throw new NotImplementedException();
        }
    }
}