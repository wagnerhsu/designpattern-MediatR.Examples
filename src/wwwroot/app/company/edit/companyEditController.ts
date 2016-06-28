/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {

    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class CompanyEditController extends Core.WithPanelsBaseController  {
        company: Business.Company;
        private companyResource: Common.Models.Resources.ICompanyResourceClass;
        public enumTypeclientCareStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ClientCareStatus;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);

            this.companyResource = dataAccessService.getCompanyResource();
         
        }
        
       hasCompanyContacts = (): boolean => {
            return this.company.contacts != null && this.company.contacts.length > 0;
        }

        showContactList = () => {            
            var selectedContactIds: string[] = this.hasCompanyContacts() ? this.company.contacts.map((contact: Dto.IContact) => { return contact.id }) : [];

            this.components.contactList()
                .loadContacts()
                .then(() => {                    
                    this.components.contactList().setSelected(selectedContactIds);
                    this.components.sidePanels.contact().show();
                });
        }

        cancelUpdateContacts = () => {
            this.components.sidePanels.contact().hide();
        }

        updateContacts = () => {
            var selectedContacts = this.components.contactList().getSelected();            
            this.company.contacts = selectedContacts.map((contact: Dto.IContact) => { return new Business.Contact(contact) });
            this.components.sidePanels.contact().hide();
        }
     
        formatUrlWithProtocol = (url:string):string=> {
            //regular expression for url with a protocol (case insensitive)
            var r = new RegExp('^(?:[a-z]+:)?//', 'i');
            if (url.length > 0) {
                if (!r.test(url)) {
                    return `http://${url}`;
                }
             }
            return url;
        }

        cancelEditCompany= () =>{
            this.$state.go('app.company-view', this.company);
        }

        updateCompany = () =>{
         
            this.company.websiteUrl = this.formatUrlWithProtocol(this.company.websiteUrl);
            this.company.clientCarePageUrl = this.formatUrlWithProtocol(this.company.clientCarePageUrl);
           
            var updatedCompany: Dto.ICompany = angular.copy(this.company); 
            
            this.companyResource
                .update(updatedCompany)
                .$promise
                .then((company: Dto.ICompany) => {                
                   this.company = new Business.Company();
                    var form = this.$scope["editCompanyForm"];
                    form.$setPristine();
                    this.$state.go('app.company-view', company);
                });
        }

        defineComponentIds() {
            this.componentIds = {
                contactSidePanelId: 'editCompany:contactSidePanelComponent',
                contactListId: 'editCompany:contactListComponent'
            }
        }

        defineComponents() {
            this.components = {
                sidePanels: {
                    contact: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                },
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); }            
            };
        }

    }

    angular.module('app').controller('CompanyEditController', CompanyEditController);
}