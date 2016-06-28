﻿namespace KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

    public class OfferControlsConfiguration : ControlsConfigurationPerTwoTypes<OfferType, RequirementType>
    {
        public override void DefineControls()
        {
            this.DefineControlsForCreate();
            this.DefineControlsForUpdate();
            this.DefineControlsForDetails();
            this.DefineControlsForPreview();
        }

        private void DefineControlsForCreate()
        {
            this.AddControl(PageType.Create, ControlCode.Offer_Status, Field<CreateOfferCommand>.CreateDictionary(x => x.StatusId, nameof(OfferStatus)).Required());
            this.AddControl(PageType.Create, ControlCode.Offer_Price, Field<CreateOfferCommand>.Create(x => x.Price).Required().GreaterThan(0));
            this.AddControl(PageType.Create, ControlCode.Offer_PricePerWeek, Field<CreateOfferCommand>.Create(x => x.PricePerWeek).Required().GreaterThan(0));
            this.AddControl(PageType.Create, ControlCode.Offer_OfferDate, Field<CreateOfferCommand>.Create(x => x.OfferDate).Required().ExternalValidator(new DateInPastValidator(nameof(CreateOfferCommand.OfferDate))));
            this.AddControl(PageType.Create, ControlCode.Offer_ExchangeDate, Field<CreateOfferCommand>.Create(x => x.ExchangeDate).ExternalValidator(new DateInFutureValidator(nameof(CreateOfferCommand.ExchangeDate))));
            this.AddControl(PageType.Create, ControlCode.Offer_CompletionDate, Field<CreateOfferCommand>.Create(x => x.CompletionDate).ExternalValidator(new DateInFutureValidator(nameof(CreateOfferCommand.CompletionDate))));
            this.AddControl(PageType.Create, ControlCode.Offer_SpecialConditions, Field<CreateOfferCommand>.CreateText(x => x.SpecialConditions, 4000).Required());
        }

        private void DefineControlsForUpdate()
        {
            this.AddControl(PageType.Update, ControlCode.Offer_Status, Field<UpdateOfferCommand>.CreateDictionary(x => x.StatusId, nameof(OfferStatus)).Required());
            this.AddControl(PageType.Update, ControlCode.Offer_Price, Field<UpdateOfferCommand>.Create(x => x.Price).Required().GreaterThan(0));
            this.AddControl(PageType.Update, ControlCode.Offer_PricePerWeek, Field<UpdateOfferCommand>.Create(x => x.PricePerWeek).Required().GreaterThan(0));
            this.AddControl(PageType.Update, ControlCode.Offer_OfferDate, Field<UpdateOfferCommand>.Create(x => x.OfferDate).Required().ExternalValidator(new DateInPastValidator(nameof(CreateOfferCommand.OfferDate))));
            this.AddControl(PageType.Update, ControlCode.Offer_ExchangeDate, Field<UpdateOfferCommand>.Create(x => x.ExchangeDate).ExternalValidator(new DateInFutureValidator(nameof(CreateOfferCommand.ExchangeDate))));
            this.AddControl(PageType.Update, ControlCode.Offer_CompletionDate, Field<UpdateOfferCommand>.Create(x => x.CompletionDate).ExternalValidator(new DateInFutureValidator(nameof(CreateOfferCommand.CompletionDate))));
            this.AddControl(PageType.Update, ControlCode.Offer_SpecialConditions, Field<UpdateOfferCommand>.CreateText(x => x.SpecialConditions, 4000).Required());

            this.AddControl(PageType.Update, ControlCode.Offer_SearchStatus, Field<UpdateOfferCommand>.Create(x => x.SearchStatusId));
            this.AddControl(PageType.Update, ControlCode.Offer_MortgageSurveyStatus, Field<UpdateOfferCommand>.Create(x => x.MortgageSurveyStatusId));
            this.AddControl(PageType.Update, ControlCode.Offer_MortgageStatus, Field<UpdateOfferCommand>.Create(x => x.MortgageStatusId));
            this.AddControl(PageType.Update, ControlCode.Offer_AdditionalSurveyStatus, Field<UpdateOfferCommand>.Create(x => x.AdditionalSurveyStatusId));
            this.AddControl(PageType.Update, ControlCode.Offer_Broker, Field<UpdateOfferCommand>.Create(x => x.BrokerId));
            this.AddControl(PageType.Update, ControlCode.Offer_BrokerCompany, Field<UpdateOfferCommand>.Create(x => x.BrokerCompanyId));
            this.AddControl(PageType.Update, ControlCode.Offer_Lender, Field<UpdateOfferCommand>.Create(x => x.LenderId));
            this.AddControl(PageType.Update, ControlCode.Offer_LenderCompany, Field<UpdateOfferCommand>.Create(x => x.LenderCompanyId));
            this.AddControl(PageType.Update, ControlCode.Offer_Surveyor, Field<UpdateOfferCommand>.Create(x => x.SurveyorId));
            this.AddControl(PageType.Update, ControlCode.Offer_SurveyorCompany, Field<UpdateOfferCommand>.Create(x => x.SurveyorCompanyId));
            this.AddControl(PageType.Update, ControlCode.Offer_AdditionalSurveyor, Field<UpdateOfferCommand>.Create(x => x.AdditionalSurveyorId));
            this.AddControl(PageType.Update, ControlCode.Offer_AdditionalSurveyorCompany, Field<UpdateOfferCommand>.Create(x => x.AdditionalSurveyorCompanyId));
            this.AddControl(PageType.Update, ControlCode.Offer_Enquiries, Field<UpdateOfferCommand>.Create(x => x.EnquiriesId));
            this.AddControl(PageType.Update, ControlCode.Offer_ContractApproved, Field<UpdateOfferCommand>.Create(x => x.ContractApproved));
            //TODO add validator LessThanOrEqualTo
            this.AddControl(PageType.Update, ControlCode.Offer_MortgageLoanToValue, Field<UpdateOfferCommand>.Create(x => x.MortgageLoanToValue).GreaterThanOrEqualTo(0)/*.LessThanOrEqualTo(200)*/);
            this.AddControl(PageType.Update, ControlCode.Offer_MortgageSurveyDate, Field<UpdateOfferCommand>.Create(x => x.MortgageSurveyDate));
            this.AddControl(PageType.Update, ControlCode.Offer_AdditionalSurveyDate, Field<UpdateOfferCommand>.Create(x => x.AdditionalSurveyDate));
            this.AddControl(PageType.Update, ControlCode.Offer_ProgressComment, Field<UpdateOfferCommand>.CreateText(x => x.ProgressComment, 4000).Required());
        }

        private void DefineControlsForDetails()
        {
            this.AddControl(PageType.Details, ControlCode.Offer_Status, Field<Offer>.Create(x => x.StatusId, x => x.Status));
            this.AddControl(PageType.Details, ControlCode.Offer_Price, Field<Offer>.Create(x => x.Price));
            this.AddControl(PageType.Details, ControlCode.Offer_PricePerWeek, Field<Offer>.Create(x => x.PricePerWeek));
            this.AddControl(PageType.Details, ControlCode.Offer_OfferDate, Field<Offer>.Create(x => x.OfferDate));
            this.AddControl(PageType.Details, ControlCode.Offer_ExchangeDate, Field<Offer>.Create(x => x.ExchangeDate));
            this.AddControl(PageType.Details, ControlCode.Offer_CompletionDate, Field<Offer>.Create(x => x.CompletionDate));
            this.AddControl(PageType.Details, ControlCode.Offer_SpecialConditions, Field<Offer>.Create(x => x.SpecialConditions));

            this.AddControl(PageType.Details, ControlCode.Offer_SearchStatus, Field<Offer>.Create(x => x.SearchStatusId, x => x.SearchStatus));
            this.AddControl(PageType.Details, ControlCode.Offer_MortgageStatus, Field<Offer>.Create(x => x.MortgageStatusId, x => x.MortgageStatus));
            this.AddControl(PageType.Details, ControlCode.Offer_MortgageSurveyStatus, Field<Offer>.Create(x => x.MortgageSurveyStatusId, x => x.MortgageSurveyStatus));
            this.AddControl(PageType.Details, ControlCode.Offer_AdditionalSurveyStatus, Field<Offer>.Create(x => x.AdditionalSurveyStatusId, x => x.AdditionalSurveyStatus));
            this.AddControl(PageType.Details, ControlCode.Offer_Broker, Field<Offer>.Create(x => x.BrokerId, x => x.Broker));
            this.AddControl(PageType.Details, ControlCode.Offer_BrokerCompany, Field<Offer>.Create(x => x.BrokerCompanyId, x => x.BrokerCompany));
            this.AddControl(PageType.Details, ControlCode.Offer_Lender, Field<Offer>.Create(x => x.LenderId, x => x.Lender));
            this.AddControl(PageType.Details, ControlCode.Offer_LenderCompany, Field<Offer>.Create(x => x.LenderCompanyId, x => x.LenderCompany));
            this.AddControl(PageType.Details, ControlCode.Offer_Surveyor, Field<Offer>.Create(x => x.SurveyorId, x => x.Surveyor));
            this.AddControl(PageType.Details, ControlCode.Offer_SurveyorCompany, Field<Offer>.Create(x => x.SurveyorCompanyId, x => x.SurveyorCompany));
            this.AddControl(PageType.Details, ControlCode.Offer_AdditionalSurveyor, Field<Offer>.Create(x => x.AdditionalSurveyorId, x => x.AdditionalSurveyor));
            this.AddControl(PageType.Details, ControlCode.Offer_AdditionalSurveyorCompany, Field<Offer>.Create(x => x.AdditionalSurveyorCompanyId, x => x.AdditionalSurveyorCompany));
            this.AddControl(PageType.Details, ControlCode.Offer_Enquiries, Field<Offer>.Create(x => x.EnquiriesId, x => x.Enquiries));
            this.AddControl(PageType.Details, ControlCode.Offer_ContractApproved, Field<Offer>.Create(x => x.ContractApproved));
            this.AddControl(PageType.Details, ControlCode.Offer_MortgageLoanToValue, Field<Offer>.Create(x => x.MortgageLoanToValue));
            this.AddControl(PageType.Details, ControlCode.Offer_MortgageSurveyDate, Field<Offer>.Create(x => x.MortgageSurveyDate));
            this.AddControl(PageType.Details, ControlCode.Offer_AdditionalSurveyDate, Field<Offer>.Create(x => x.AdditionalSurveyDate));
            this.AddControl(PageType.Details, ControlCode.Offer_ProgressComment, Field<Offer>.Create(x => x.ProgressComment));

            this.AddControl(PageType.Details, ControlCode.Offer_Requirement, Field<Offer>.Create(x => x.RequirementId, x => x.Requirement));
            this.AddControl(PageType.Details, ControlCode.Offer_Activity, Field<Offer>.Create(x => x.ActivityId, x => x.Activity));
            this.AddControl(PageType.Details, ControlCode.Offer_Negotiator, Field<Offer>.Create(x => x.NegotiatorId, x => x.Negotiator));
            this.AddControl(PageType.Details, ControlCode.Offer_CreatedDate, Field<Offer>.Create(x => x.CreatedDate));
            this.AddControl(PageType.Details, ControlCode.Offer_LastModifiedDate, Field<Offer>.Create(x => x.LastModifiedDate));
        }


        private void DefineControlsForPreview()
        {
            this.AddControl(PageType.Preview, ControlCode.Offer_Status, Field<Offer>.CreateDictionary(x => x.StatusId, nameof(OfferStatus)));
            this.AddControl(PageType.Preview, ControlCode.Offer_Price, Field<Offer>.Create(x => x.Price));
            this.AddControl(PageType.Preview, ControlCode.Offer_PricePerWeek, Field<Offer>.Create(x => x.PricePerWeek));
            this.AddControl(PageType.Preview, ControlCode.Offer_OfferDate, Field<Offer>.Create(x => x.OfferDate));
            this.AddControl(PageType.Preview, ControlCode.Offer_ExchangeDate, Field<Offer>.Create(x => x.ExchangeDate));
            this.AddControl(PageType.Preview, ControlCode.Offer_CompletionDate, Field<Offer>.Create(x => x.CompletionDate));
            this.AddControl(PageType.Preview, ControlCode.Offer_SpecialConditions, Field<Offer>.Create(x => x.SpecialConditions));
            this.AddControl(PageType.Preview, ControlCode.Offer_Activity, Field<Offer>.Create(x => x.Activity));
            this.AddControl(PageType.Preview, ControlCode.Offer_Negotiator, Field<Offer>.Create(x => x.Negotiator));
        }

        public override void DefineMappings()
        {
            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Status,
                        ControlCode.Offer_Price,
                        ControlCode.Offer_OfferDate,
                        ControlCode.Offer_SpecialConditions,
                        ControlCode.Offer_ExchangeDate,
                        ControlCode.Offer_CompletionDate
                    },
                this.When(OfferType.ResidentialSale, RequirementType.ResidentialSale, PageType.Create, PageType.Update, PageType.Details, PageType.Preview));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Status,
                        ControlCode.Offer_PricePerWeek,
                        ControlCode.Offer_OfferDate,
                        ControlCode.Offer_SpecialConditions,
                        ControlCode.Offer_ExchangeDate,
                        ControlCode.Offer_CompletionDate
                    },
                this.When(OfferType.ResidentialLetting, RequirementType.ResidentialLetting, PageType.Create, PageType.Update, PageType.Details, PageType.Preview));


            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_SearchStatus,
                        ControlCode.Offer_MortgageStatus,
                        ControlCode.Offer_MortgageSurveyStatus,
                        ControlCode.Offer_AdditionalSurveyStatus,
                        ControlCode.Offer_Broker,
                        ControlCode.Offer_BrokerCompany,
                        ControlCode.Offer_Lender,
                        ControlCode.Offer_LenderCompany,
                        ControlCode.Offer_Surveyor,
                        ControlCode.Offer_SurveyorCompany,
                        ControlCode.Offer_AdditionalSurveyor,
                        ControlCode.Offer_AdditionalSurveyorCompany,
                        ControlCode.Offer_Enquiries,
                        ControlCode.Offer_ContractApproved,
                        ControlCode.Offer_MortgageLoanToValue,
                        ControlCode.Offer_MortgageSurveyDate,
                        ControlCode.Offer_AdditionalSurveyDate,
                        ControlCode.Offer_ProgressComment
                    },
                this.When(OfferType.ResidentialSale, RequirementType.ResidentialSale, PageType.Update, PageType.Details));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_SearchStatus,
                        ControlCode.Offer_MortgageStatus,
                        ControlCode.Offer_MortgageSurveyStatus,
                        ControlCode.Offer_AdditionalSurveyStatus,
                        ControlCode.Offer_Broker,
                        ControlCode.Offer_BrokerCompany,
                        ControlCode.Offer_Lender,
                        ControlCode.Offer_LenderCompany,
                        ControlCode.Offer_Surveyor,
                        ControlCode.Offer_SurveyorCompany,
                        ControlCode.Offer_AdditionalSurveyor,
                        ControlCode.Offer_AdditionalSurveyorCompany,
                        ControlCode.Offer_Enquiries,
                        ControlCode.Offer_ContractApproved,
                        ControlCode.Offer_MortgageLoanToValue,
                        ControlCode.Offer_MortgageSurveyDate,
                        ControlCode.Offer_AdditionalSurveyDate,
                        ControlCode.Offer_ProgressComment
                    },
                this.When(OfferType.ResidentialLetting, RequirementType.ResidentialLetting, PageType.Update, PageType.Details));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Requirement,
                        ControlCode.Offer_Activity,
                        ControlCode.Offer_Negotiator,
                        ControlCode.Offer_CreatedDate,
                        ControlCode.Offer_LastModifiedDate
                    },
                this.When(OfferType.ResidentialSale, RequirementType.ResidentialSale, PageType.Details));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Requirement,
                        ControlCode.Offer_Activity,
                        ControlCode.Offer_Negotiator,
                        ControlCode.Offer_CreatedDate,
                        ControlCode.Offer_LastModifiedDate
                    },
                this.When(OfferType.ResidentialLetting, RequirementType.ResidentialLetting, PageType.Details));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Activity,
                        ControlCode.Offer_Negotiator
                    },
                this.When(OfferType.ResidentialSale, RequirementType.ResidentialSale, PageType.Preview));

            this.Use(
                new List<ControlCode>
                    {
                        ControlCode.Offer_Activity,
                        ControlCode.Offer_Negotiator
                    },
                this.When(OfferType.ResidentialLetting, RequirementType.ResidentialLetting, PageType.Preview));
        }
    }
}
