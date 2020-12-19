using Domain.Interfaces.Entities;
using PersonsApplication.ReadModels;
using PersonsDomain;

namespace PersonsApplication.Storage
{
    public interface IPersonsStorage
    {
        PersonAggregate Load(Identifier id);

        PersonAggregate Save(PersonAggregate person);

        Person FindByEmailAddress(string emailAddress);

        Person GetPerson(Identifier id);
    }
}