﻿namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Configuration.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Activity : BaseAuditableEntity
    {
        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public Guid ActivityStatusId { get; set; }

        public EnumTypeItem ActivityStatus { get; set; }

        public Guid ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

        public decimal? MarketAppraisalPrice { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public decimal? VendorEstimatedPrice { get; set; }

        public decimal? ShortLetPricePerWeek { get; set; }

        public decimal? AskingPrice { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();

        public virtual ICollection<ActivityUser> ActivityUsers { get; set; } = new List<ActivityUser>();

        public virtual ICollection<ActivityDepartment> ActivityDepartments { get; set; } = new List<ActivityDepartment>();

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public Guid? SourceId { get; set; }

        public virtual EnumTypeItem Source { get; set; }

        public string SourceDescription { get; set; }

        public Guid? SellingReasonId { get; set; }

        public virtual EnumTypeItem SellingReason { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArrangements { get; set; }

        public DateTimeOffset? AppraisalMeetingStart { get; set; }

        public DateTimeOffset? AppraisalMeetingEnd { get; set; }

        public string AppraisalMeetingInvitationText { get; set; }

        public virtual ICollection<ActivityAttendee> AppraisalMeetingAttendees { get; set; } = new List<ActivityAttendee>();
    }
}
