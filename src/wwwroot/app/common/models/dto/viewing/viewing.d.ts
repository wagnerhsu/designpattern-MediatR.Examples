﻿declare module Antares.Common.Models.Dto {
    interface IViewing {
        id: string;
        activityId: string;
        requirementId: string;
        negotiatorId: string;
        startDate: Date | string;
        endDate: Date | string;
        invitationText: string;
        postviewingComment: string;
        attendeesIds: string[];
    }
}