namespace KnightFrank.Antares.Dal.Model.Configuration.Resource
{
    using KnightFrank.Antares.Dal.Model.Configuration;
    using KnightFrank.Antares.Dal.Model.Resource;

    internal sealed class LocaleConfiguration : BaseEntityConfiguration<Locale>
    {
        public LocaleConfiguration()
        {
            this.Property(r => r.IsoCode)
                .HasMaxLength(2)
                .IsRequired()
                .IsUnique();

			this.HasMany(p => p.CountryLocaliseds)
				.WithRequired(p => p.Locale)
				.WillCascadeOnDelete(false);

			this.HasMany(p => p.EnumLocaliseds)
				.WithRequired(p => p.Locale)
				.WillCascadeOnDelete(false);

			this.HasMany(p => p.Users)
				.WithRequired(p => p.Locale)
				.WillCascadeOnDelete(false);
		}
	}
}