﻿// <copyright file="enumTypeItems.tt">
// </copyright>
// ReSharper disable InconsistentNaming
module Antares.Common.Models.Enums
{
	export enum EnumType
	{
	 	EntityType,
	 	OwnershipType,
	 	ActivityStatus,
	 	Division,
	 	ActivityDocumentType,
	 	OfferStatus,
	 	ActivityUserType,
	 	ActivityDepartmentType,
	 	MortgageStatus,
	 	MortgageSurveyStatus,
	 	AdditionalSurveyStatus,
	 	SearchStatus,
	 	Enquiries,
	 }

	
	export enum EntityType
	{
		Property,
		Requirement,
	}

	export enum OwnershipType
	{
		Freeholder,
		Leaseholder,
	}

	export enum ActivityStatus
	{
		PreAppraisal,
		MarketAppraisal,
		NotSelling,
	}

	export enum Division
	{
		Residential,
		Commercial,
	}

	export enum ActivityDocumentType
	{
		TermsOfBusiness,
		MarketingSignOff,
		CDDDocument,
		Photograph,
		FloorPlan,
		Brochure,
		VideoTour,
		EPC,
		GasCertificate,
	}

	export enum OfferStatus
	{
		New,
		Withdrawn,
		Rejected,
		Accepted,
	}

	export enum ActivityUserType
	{
		LeadNegotiator,
		SecondaryNegotiator,
	}

	export enum ActivityDepartmentType
	{
		Managing,
		Standard,
	}

	export enum MortgageStatus
	{
		Unknown,
		NotRequiredCashFromSale,
		NotRequiredCashInBank,
		InProgress,
		Agreed,
	}

	export enum MortgageSurveyStatus
	{
		Unknown,
		NotRequired,
		Complete,
		Outstanding,
	}

	export enum AdditionalSurveyStatus
	{
		Unknown,
		NotRequired,
		InProgress,
		Complete,
	}

	export enum SearchStatus
	{
		NotStarted,
		AppliedFor,
		Complete,
	}

	export enum Enquiries
	{
		NotStarted,
		Sent,
		Complete,
	}
}