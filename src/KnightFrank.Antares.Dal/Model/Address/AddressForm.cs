﻿namespace KnightFrank.Antares.Dal.Model.Address
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class AddressForm : BaseEntity
    {
        public Guid CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<AddressFieldDefinition> AddressFieldDefinitions { get; set; }

        public virtual ICollection<AddressFormEntityType> AddressFormEntityTypes { get; set; }
    }
}
