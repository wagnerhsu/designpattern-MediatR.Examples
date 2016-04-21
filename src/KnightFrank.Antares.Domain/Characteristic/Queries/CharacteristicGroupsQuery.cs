﻿namespace KnightFrank.Antares.Domain.Characteristic.Queries
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;

    using MediatR;

    public class CharacteristicGroupsQuery : IRequest<IEnumerable<CharacteristicGroupUsage>>
    {
        public string CountryCode { get; set; }

        public Guid PropertyTypeId { get; set; }
    }
}