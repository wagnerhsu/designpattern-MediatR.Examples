/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferViewController = Component.OfferViewController;
	import Dto = Common.Models.Dto;

    describe('Given offer view component is loaded', () =>{
	    var scope: ng.IScope,
	        element: ng.IAugmentedJQuery,
	        controller: OfferViewController,
	        state: ng.ui.IStateService,
	        $http: ng.IHttpBackendService,
	        compile: ng.ICompileService;

        var pageObjectSelectors = {
            vendorSection: '#section-vendor',
            applicantSection: '#section-applicant',
            cardItem: '.card-item',
            pageHeader: '#view-offer-header',
            pageHeaderTitle: '.offer-title',
            sectionBasicInformation: '#section-basic-information',
            applicant: '.applicant',
            panelBody: '.panel-body',
			progressSection: '#offer-progress-section'
        }

		var offerStatuses: Common.Models.Dto.IEnumItem[] = [
            { id: '1', code: 'New' },
            { id: '2', code: 'Closed' },
            { id: '3', code: 'Accepted' }
        ];

		var offerMock = TestHelpers.OfferGenerator.generate();

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $state: ng.ui.IStateService,
            $httpBackend: ng.IHttpBackendService,
			enumService: Mock.EnumServiceMock) => {

            $http = $httpBackend;
            compile = $compile;
			state = $state;
            scope = $rootScope.$new();
            scope["offer"] = offerMock;

            enumService.setEnum(Dto.EnumTypeCode.OfferStatus.toString(), offerStatuses);

            element = compile('<offer-view offer="offer"></offer-view>')(scope);
            scope.$apply();
			
			controller = element.controller('offerView');
        }));

        describe('when data are retrived form the server', () => {
            it('page header displays applicant names seppareted by comma', () => {
			    //Add two contacts
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());

                scope['offer'] = offerMock;
                scope.$apply();

                var expectedHeaderNamesText = offerMock.requirement.getContactNames();
                var currentHeaderNames = element.find(pageObjectSelectors.pageHeader).find(pageObjectSelectors.pageHeaderTitle).text();
                
                expect(currentHeaderNames).toEqual(expectedHeaderNamesText);
            });

            it('vendor address is displayed within vendor card', () => {
                offerMock.activity.property.address.propertyNumber = "test property number";
                offerMock.activity.property.address.propertyName = "test property name";
                offerMock.activity.property.address.line2 = "test line2 address";

                scope['offer'] = offerMock;
				element = compile('<offer-view offer="offer"></offer-view>')(scope);
                scope.$apply();

                var expectedAddressText = offerMock.activity.property.address.getAddressText();
                var currentVendorAddressText = element.find(pageObjectSelectors.vendorSection).find(pageObjectSelectors.cardItem).text();

                expect(currentVendorAddressText).toEqual(expectedAddressText);
            });

            it('applicant names are displayed within applicant card seppareted by comma', () => {
                //Add two contacts
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());

                scope['offer'] = offerMock;
				element = compile('<offer-view offer="offer"></offer-view>')(scope);
                scope.$apply();

                var expectedApplicantNamesText = offerMock.requirement.getContactNames();
                var currentApplicantNamesText = element.find(pageObjectSelectors.applicantSection).find(pageObjectSelectors.cardItem).text();

                expect(currentApplicantNamesText).toEqual(expectedApplicantNamesText);
            });
            
            it('negotiator name is displayed within negotiator card', () => {
                var expectedNegotiatorFullNameText = offerMock.negotiator.getName();
                var currentNegotiatorFullName = element
                    .find(pageObjectSelectors.sectionBasicInformation)
                    .find(pageObjectSelectors.applicant)
                    .find(pageObjectSelectors.panelBody)
                    .text();

                expect(currentNegotiatorFullName).toEqual(expectedNegotiatorFullNameText);
            });
        });        

        describe('when details button is clicked', () => {
            it('on context menu within vendor card then redirect to activity is performed', () => {
                var activityMock = TestHelpers.ActivityGenerator.generate();

                spyOn(state, 'go');

                controller.navigateToActivity(activityMock);
                scope.$apply();

                var vendorDetailsLink = element.find(pageObjectSelectors.vendorSection).children("a");
                vendorDetailsLink.click();
                
                expect(state.go).toHaveBeenCalledWith('app.activity-view', { id: activityMock.id });
            });

            

            it('on context menu within applicant card then redirect to requiremen is performed', () => {
                var requirementMock = TestHelpers.RequirementGenerator.generate();

                spyOn(state, 'go');

                controller.navigateToRequirement(requirementMock);
                scope.$apply();

                var applicantDetailsLink = element.find(pageObjectSelectors.applicantSection).children("a");
                applicantDetailsLink.click();

                expect(state.go).toHaveBeenCalledWith('app.requirement-view', { id: requirementMock.id });
            });
        });

        describe('when activity details is clicked', () => {
            it('then activity should be added to property activity list', () => {
                $http.expectPOST(/\/api\/latestviews/, () => {
                    return true;
                }).respond(200, []);

                var activityMock = TestHelpers.ActivityGenerator.generate();
                
                offerMock.activity = activityMock;
                scope.$apply();

                controller.showActivityPreview(offerMock);

                expect($http.flush).not.toThrow();
            });
        });

		describe('when offer status is other than New', () =>{
            beforeEach(() =>{
				offerMock.statusId = _.find(offerStatuses, (status:  Common.Models.Dto.IEnumItem) => status.code === "Accepted").id;
                scope['offer'] = offerMock;
                scope.$apply();
            });

            it('then progress section is rendered', () => {
                var progressSection = element.find(pageObjectSelectors.progressSection);
                expect(progressSection.length).toBe(1);
            });
		});

		describe('when offer status is New', () =>{
            beforeEach(() =>{
				offerMock.statusId = _.find(offerStatuses, (status:  Common.Models.Dto.IEnumItem) => status.code === "New").id;
                scope['offer'] = offerMock;
                scope.$apply();
            });

            it('then progress section is not rendered', () => {
                var progressSection = element.find(pageObjectSelectors.progressSection);
                expect(progressSection.length).toBe(0);
            });
		});
    });
}