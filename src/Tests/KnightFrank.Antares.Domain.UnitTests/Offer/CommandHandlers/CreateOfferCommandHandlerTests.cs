﻿namespace KnightFrank.Antares.Domain.UnitTests.Offer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.CommandHandlers;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using OfferType = KnightFrank.Antares.Dal.Model.Offer.OfferType;

    [Collection("CreateOfferCommandHandler")]
    [Trait("FeatureTitle", "Offer")]
    public class CreateOfferCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly EnumType acceptedEnumType;

        public CreateOfferCommandHandlerTests()
        {
            IFixture fixture = new Fixture().Customize();

            this.acceptedEnumType = fixture.Create<EnumType>();
            this.acceptedEnumType.EnumTypeItems = fixture.CreateMany<EnumTypeItem>().ToList();

            this.acceptedEnumType.EnumTypeItems.First().Code = OfferStatus.Accepted.ToString();
            this.acceptedEnumType.Code = Domain.Common.Enums.EnumType.OfferStatus.ToString();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateOfferCommand_When_Handle_Then_ShouldCreateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<EnumType>> enumTypeRepository,
            [Frozen] Mock<IGenericRepository<OfferType>> offerTypeRepository,
            OfferType offerType,
            CreateOfferCommand command,
            CreateOfferCommandHandler handler,
            List<User> users)
        {
            // TODO remove userRepository after userRepository is removed from tested method
            userRepository.Setup(u => u.Get()).Returns(users.AsQueryable());
            offerRepository.Setup(r => r.Add(It.IsAny<Offer>())).Returns((Offer a) => a);
            offerTypeRepository.Setup(r => r.FindBy(It.IsAny<Expression<Func<OfferType, bool>>>())).Returns(new[] { offerType });

            enumTypeRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<EnumType, bool>>>(), It.IsAny<Expression<Func<EnumType, object>>>()))
                .Returns(new List<EnumType> { this.acceptedEnumType});

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists<Activity>(command.ActivityId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Requirement>(command.RequirementId), Times.Once);
            enumTypeValidator.Verify(x => x.ItemExists(Domain.Common.Enums.EnumType.OfferStatus, command.StatusId), Times.Once);
            offerRepository.Verify(r => r.Add(It.IsAny<Offer>()), Times.Once());
            offerRepository.Verify(r => r.Save(), Times.Once());
            offerTypeRepository.Verify(r => r.FindBy(It.IsAny<Expression<Func<OfferType, bool>>>()), Times.Once());
        }
    }
}
