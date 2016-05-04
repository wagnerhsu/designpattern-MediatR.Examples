﻿namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Specifications;

    public class EnumTypeItemValidator : IEnumTypeItemValidator
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemGenericRepository;

        public EnumTypeItemValidator(IGenericRepository<EnumTypeItem> enumTypeItemGenericRepository)
        {
            this.enumTypeItemGenericRepository = enumTypeItemGenericRepository;
        }

        public void ItemExists(Enums.EnumType enumType, Guid enumTypeItemId)
        {
            bool enumTypeItemExists =
                this.enumTypeItemGenericRepository.Any((new HasId<EnumTypeItem>(enumTypeItemId) & new EnumTypeItemWithEnumType(enumType)).SatisfiedBy());

            if (!enumTypeItemExists)
            {
                throw new BusinessValidationException(BusinessValidationMessage.CreateEnumTypeItemNotExistMessage(enumType.ToString(), enumTypeItemId));
            }
        }
    }
}