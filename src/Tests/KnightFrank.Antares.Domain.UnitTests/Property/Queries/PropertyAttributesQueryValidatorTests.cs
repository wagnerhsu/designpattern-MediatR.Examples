﻿namespace KnightFrank.Antares.Domain.UnitTests.Property.Queries
{
    using System;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Property.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("PropertyAttributesQueryValidator")]
    [Trait("FeatureTitle", "Property")]
    public class PropertyAttributesQueryValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly PropertyAttributesQuery query;

        public PropertyAttributesQueryValidatorTests()
        {
            IFixture fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            this.query = fixture.Build<PropertyAttributesQuery>()
                                                   .With(x => x.CountryId, Guid.NewGuid())
                                                   .With(x => x.PropertyTypeId, Guid.NewGuid())
                                                   .Create();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectPropertyAttributesQuery_When_Validating_Then_NoValidationErrors(
            PropertyAttributesQueryValidator validator)
        {
            // Act
            ValidationResult validationResult = validator.Validate(this.query);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_InCorrectPropertyAttributesQueryWithNoCountryCode_When_Validating_Then_ValidationErrors(
           PropertyAttributesQueryValidator validator)
        {
            this.query.CountryId = Guid.Empty;
            
            TestIncorrectCommand(validator, this.query, nameof(this.query.CountryId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InCorrectPropertyAttributesQueryWithNoPropertyTypeId_When_Validating_Then_ValidationErrors(
           PropertyAttributesQueryValidator validator)
        {
            this.query.PropertyTypeId = Guid.Empty;

            TestIncorrectCommand(validator, this.query, nameof(this.query.PropertyTypeId));
        }

        private static void TestIncorrectCommand(PropertyAttributesQueryValidator validator, PropertyAttributesQuery query,
            string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(query);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
