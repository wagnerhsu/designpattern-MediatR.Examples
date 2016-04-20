﻿namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;

    internal sealed class CharacteristicLocalisedConfiguration : BaseEntityConfiguration<CharacteristicLocalised>
    {
        public CharacteristicLocalisedConfiguration()
        {
            
            this.HasRequired(x => x.Locale)
                .WithMany()
                .HasForeignKey(x => x.LocaleId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Characteristic)
                .WithMany()
                .HasForeignKey(x => x.CharacteristicId)
                .WillCascadeOnDelete(false);
        }
    }
}