using System;
using System.Collections.Generic;
using Domain.Interfaces.Entities;
using PersonsDomain;
using Storage.Interfaces.ReadModels;

namespace PersonsApi
{
    public class PersonsIdentifierFactory : EntityPrefixIdentifierFactory
    {
        public PersonsIdentifierFactory() : base(new Dictionary<Type, string>
        {
            {typeof(Checkpoint), "ckp"},
            {typeof(PersonAggregate), "per"}
        })
        {
        }
    }
}