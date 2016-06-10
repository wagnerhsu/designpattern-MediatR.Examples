﻿namespace KnightFrank.Antares.Api.UnitTests.Services.AzureStorage
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Api.Models;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    using Moq;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class DocumentStorageProviderTests
    {

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        public void Given_ConfigureUploadUrl_When_Called_Then_ProperUploadUrlMethodIsSetForEntity(
            CloudStorageContainerType cloudStorageContainerType,
            [Frozen] Mock<IEntityDocumentStorageProvider> entityDocumentStorageProvider,
            DocumentStorageProvider documentStorageProvider,
            AttachmentUrlParameters parameters)
        {
            // Arrange
            // Act
            documentStorageProvider.ConfigureUploadUrl();

            // Assert
            DocumentStorageProvider.GetUploadSasUri method = documentStorageProvider.GetUploadUrlMethod(CloudStorageContainerType.Activity);
            method(parameters);

            entityDocumentStorageProvider.Verify(x => x.GetUploadSasUri<Activity>(parameters, EnumType.ActivityDocumentType), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        public void Given_ConfigureDownloadUrl_When_Called_Then_ProperUploadUrlMethodsAreSetForEntity(
            CloudStorageContainerType cloudStorageContainerType,
            [Frozen] Mock<IEntityDocumentStorageProvider> entityDocumentStorageProvider,
            DocumentStorageProvider documentStorageProvider,
            AttachmentDownloadUrlParameters parameters)
        {
            // Arrange
            // Act
            documentStorageProvider.ConfigureDownloadUrl();

            // Assert
            DocumentStorageProvider.GetDownloadSasUri method = documentStorageProvider.GetDownloadUrlMethod(cloudStorageContainerType);
            method(parameters);

            entityDocumentStorageProvider.Verify(x => x.GetDownloadSasUri<Activity>(parameters, EnumType.ActivityDocumentType), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        public void Given_GetUploadUrlMethod_When_CalledWithConfiguredType_Then_ProperUploadUrlMethodIsSetForEntity(
            CloudStorageContainerType cloudStorageContainerType,
            DocumentStorageProvider documentStorageProvider,
            AttachmentUrlParameters parameters)
        {
            // Arrange
            documentStorageProvider.ConfigureUploadUrl();

            // Act
            DocumentStorageProvider.GetUploadSasUri method = documentStorageProvider.GetUploadUrlMethod(cloudStorageContainerType);

            // Assert
            method.Should().NotBeNull();
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        public void Given_GetUploadUrlMethod_When_CalledWithNotConfiguredType_Then_ShoulThrowException(
            CloudStorageContainerType cloudStorageContainerType,
            DocumentStorageProvider documentStorageProvider,
            AttachmentUrlParameters parameters)
        {
            // Arrange
            // Act
            Action act = () => documentStorageProvider.GetUploadUrlMethod(cloudStorageContainerType);

            // Asset
            act.ShouldThrow<DomainValidationException>().Which.Errors.Single(x => x.ErrorMessage == "Entity is not supported");
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        public void Given_GetDownloadUrlMethod_When_CalledWithConfiguredType_Then_ProperDownloadUrlMethodIsSetForEntity(
           CloudStorageContainerType cloudStorageContainerType,
           DocumentStorageProvider documentStorageProvider,
           AttachmentDownloadUrlParameters parameters)
        {
            // Arrange
            documentStorageProvider.ConfigureDownloadUrl();

            // Act
            DocumentStorageProvider.GetDownloadSasUri method = documentStorageProvider.GetDownloadUrlMethod(cloudStorageContainerType);

            // Assert
            method.Should().NotBeNull();
        }

        [Theory]
        [InlineAutoMoqData(CloudStorageContainerType.Activity)]
        public void Given_GettDownloadUrlMethod_When_CalledWithNotConfiguredType_Then_ShoulThrowException(
            CloudStorageContainerType cloudStorageContainerType,
            DocumentStorageProvider documentStorageProvider,
            AttachmentDownloadUrlParameters parameters)
        {
            // Arrange
            // Act
            Action act = () => documentStorageProvider.GetDownloadUrlMethod(cloudStorageContainerType);

            // Asset
            act.ShouldThrow<DomainValidationException>().Which.Errors.Single(x => x.ErrorMessage == "Entity is not supported");
        }
    }
}