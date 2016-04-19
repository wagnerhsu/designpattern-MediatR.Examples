/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    
    export class RequirementAddController {
        componentIds: any = {
            contactListId: 'addRequirement:contactListComponent',
            contactSidePanelId: 'addRequirement:contactSidePanelComponent'
        }
        components: any = {
            contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
            contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); }
        }
        requirementResource: any;
        requirement: Business.Requirement = new Business.Requirement();
        loadingContacts: boolean = false;
        entityTypeCode: string = 'Requirement';
        isSaving: boolean = false;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {
            
            this.requirementResource = dataAccessService.getRequirementResource();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
            });
        }

        updateContacts() {
            this.requirement.contacts = this.components.contactList().getSelected();
            this.components.contactSidePanel().hide();
        }

        cancelUpdateContacts() {
            this.components.contactSidePanel().hide();
        }

        showContactList = () => {
            this.loadingContacts = true;
            this.components.contactList()
                .loadContacts()
                .then(() => {
                    var selectedIds: string[] = [];
                    if (this.requirement.contacts !== undefined && this.requirement.contacts !== null) {
                        selectedIds = <string[]>this.requirement.contacts.map((c: any) => c.id);
                    }
                    this.components.contactList().setSelected(selectedIds);
                })
                .finally(() => { this.loadingContacts = false; });

            this.components.contactSidePanel().show();
        }

        save() {
            this.isSaving = true;
            this.requirementResource
                .save(new Business.CreateRequirementResource(this.requirement))
                .$promise
                .then(
                (requirement: any) => {
                    this.$state.go('app.requirement-view', requirement);
                })
                .finally(() => {
                    this.isSaving = false;
                });
        }
    }
    angular.module('app').controller('requirementAddController', RequirementAddController);
}