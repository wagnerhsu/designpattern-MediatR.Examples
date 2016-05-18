/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
	import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    export class NegotiatorsEditController {
        public activityId: string;
        public leadNegotiator: Business.ActivityUser;
        public secondaryNegotiators: Business.ActivityUser[];

        public isLeadNegotiatorInEditMode: boolean = false;
        public isSecondaryNegotiatorsInEditMode: boolean = false;

        private usersSearchMaxCount: number = 100;

        constructor(
            private dataAccessService: Services.DataAccessService) {
        }

        public editLeadNegotiator = () =>{
            this.isLeadNegotiatorInEditMode = true;
        }

        public changeLeadNegotiator = (user: Dto.IDepartmentUser) => {
            this.leadNegotiator = this.createActivityUser(user, Enums.NegotiatorTypeEnum.LeadNegotiator);

            this.isLeadNegotiatorInEditMode = false;
        }

        public cancelChangeLeadNegotiator = () => {
            this.isLeadNegotiatorInEditMode = false;
        }

        public editSecondaryNegotiators = () => {
            this.isSecondaryNegotiatorsInEditMode = true;
        }

        public addSecondaryNegotiator = (user: Dto.IDepartmentUser) => {
            this.secondaryNegotiators.push(this.createActivityUser(user, Enums.NegotiatorTypeEnum.SecondaryNegotiator));
        }

        public cancelAddSecondaryNegotiator = () => {
            this.isSecondaryNegotiatorsInEditMode = false;
        }

        public getUsers = (searchValue: string) =>{
            var excludedIds: string[] = _.map<Business.ActivityUser, string>(this.secondaryNegotiators, 'userId');
            excludedIds.push(this.leadNegotiator.userId);

            return this.dataAccessService
                .getDepartmentUserResource()
                .query({ partialName: searchValue, take: this.usersSearchMaxCount, 'excludedIds[]': excludedIds })
                .$promise
                .then((users: any) => {
                    return users.map((user: Common.Models.Dto.IDepartmentUser) => { return new Common.Models.Business.DepartmentUser(<Common.Models.Dto.IDepartmentUser>user); });
                });
        }

        private createActivityUser = (user: Dto.IDepartmentUser, negotiatorType: Enums.NegotiatorTypeEnum) => {
            return new Business.ActivityUser({
                id: '',
                activityId: this.activityId,
                userId: user.id,
                user: <Dto.IUser>user,
                userType: negotiatorType
            });
        }
    }

	angular.module('app').controller('NegotiatorsEditController', NegotiatorsEditController);
}