namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    public interface IEnumTypeItemValidator
    {
        void ItemExists(Enums.EnumType enumType, Guid enumTypeItemId);
    }
}