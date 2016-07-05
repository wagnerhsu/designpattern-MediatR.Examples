declare module Antares.Common.Models.Dto {
    interface IActivity {
        id: string;
        propertyId: string;
        activityStatusId: string;
        activityTypeId: string;
        contacts: IContact[];
        attachments?: IAttachment[];
        property?: IPreviewProperty;
        createdDate?: Date;
        viewings?: IViewing[];
        activityUsers: IActivityUser[];
        activityDepartments: IActivityDepartment[];
        offers?: IOffer[];
		askingPrice?: number;
        shortLetPricePerWeek?: number;
        sourceId: string;
        sellingReasonId: string;
        appraisalMeetingStart: string;
        appraisalMeetingEnd: string;
        appraisalMeetingInvitationText: string;
        keyNumber: string;
        accessArrangements: string;
        appraisalMeetingAttendees: Dto.IActivityAttendee[];
        kfValuationPrice?: number;
        agreedInitialMarketingPrice?: number;
        vendorValuationPrice?: number;
        shortKfValuationPrice?: number;
        shortVendorValuationPrice?: number;
        shortAgreedInitialMarketingPrice?: number;
        longKfValuationPrice?: number;
        longVendorValuationPrice?: number;
        longAgreedInitialMarketingPrice?: number;
        disposalTypeId: string;
        decorationId: string;
        serviceChargeAmount?: number;
        serviceChargeNote: string;
        groundRentAmount?: number;
        groundRentNote: string;
        otherCondition: string;
    }
}