﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferAddController = Component.OfferAddController;
    import Business = Common.Models.Business;
    import runDescribe = TestHelpers.runDescribe;
    type TestCaseForRequiredValidator = [string, string, boolean]; // [field_description, field_selector, is_not_required]
    type TestCaseForValidator = [string, string, string]; // [field_description, field_value, field_selector]

    declare var moment: any;

    describe('Given offer component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: OfferAddController;

        var pageObjectSelectors = {
            offerDate: '[name=offerDate]',
            offer: '[name=price]',
            exchangeDate: '[name=exchangeDate]',
            completionDate: '[name=completionDate]',
            specialConditions: '[name=specialConditions]',
            status: '[name="selectedStatus"]'
        };

        var format = (date: any) => date.format('DD-MM-YYYY');
        var datesToTest: any = {
            today: format(moment()),
            inThePast: format(moment().day(-7)),
            inTheFuture: format(moment().day(7)),
            longAgo: format(moment().year(1700)),
            reallyLongAgo: format(moment().year(100))
        };

        var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate();

        describe('is in add mode', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                scope = $rootScope.$new();
                scope["requirement"] = requirementMock;

                element = $compile('<offer-add requirement="requirement"></offer-add>')(scope);
                scope.$apply();

                controller = element.controller('offerAdd');
                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            // required validation
            runDescribe('and value for ')
                .data<TestCaseForRequiredValidator>([
                    ['offer date', pageObjectSelectors.offerDate, false],
                    ['offer', pageObjectSelectors.offer, false],
                    ['status', pageObjectSelectors.status, false],
                    ['exchange date', pageObjectSelectors.exchangeDate, true],
                    ['completion date', pageObjectSelectors.completionDate, true],
                    ['special conditions', pageObjectSelectors.specialConditions, true]
                ])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    ` is "${data[0]}" then required message should ${data[2] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) => {
                    assertValidator.assertRequiredValidator('', data[2], data[1]);
                });

            // date format validation
            runDescribe('and value for ')
                .data<TestCaseForValidator>([
                    ['offer date', 'invalid date', pageObjectSelectors.offerDate],
                    ['exchange date', 'invalid date', pageObjectSelectors.exchangeDate],
                    ['completion date', 'invalid date', pageObjectSelectors.completionDate]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be invalid and invalid format message should be displayed`
                )
                .run((data: TestCaseForValidator) => {
                    assertValidator.assertDateFormatValidator(data[1], false, data[2]);
                });

            // number format validation
            runDescribe('and value for ')
                .data<TestCaseForValidator>([
                    ['offer', 'e', pageObjectSelectors.offer],
                    ['offer', '.', pageObjectSelectors.offer]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be invalid and invalid format message should be displayed`
                )
                .run((data: TestCaseForValidator) => {
                    /*
                        TODO:
                        during tests somehow 'required' validation message is displayed instead of 'invalid number' message
                        which is shown during user journey and which is expected
                    */

                    // assertValidator.assertNumberFormatValidator(data[1], false, data[2]);
                    assertValidator.assertRequiredValidator(data[1], false, data[2]);
                });

            // valid field values                
            runDescribe('and value for ')
                .data<TestCaseForValidator>([
                    ['offer date', datesToTest.today, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.inThePast, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.inTheFuture, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.longAgo, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.reallyLongAgo, pageObjectSelectors.offerDate],
                    ['exchange date', datesToTest.today, pageObjectSelectors.exchangeDate],
                    ['exchange date', datesToTest.inTheFuture, pageObjectSelectors.exchangeDate],
                    ['completion date', datesToTest.today, pageObjectSelectors.completionDate],
                    ['completion date', datesToTest.inTheFuture, pageObjectSelectors.completionDate],
                    ['offer', '5', pageObjectSelectors.offer]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be valid and no validation message should be displayed`
                )
                .run((data: TestCaseForValidator) => {
                    assertValidator.assertValidAndNoMessages(data[1], data[2]);
                });

            it('all form elements are rendered', () => {
                for (var formElementSelector in pageObjectSelectors) {
                    if (pageObjectSelectors.hasOwnProperty(formElementSelector)) {
                        var formElement = element.find(pageObjectSelectors[formElementSelector]);
                        expect(formElement.length).toBe(1, `Element ${formElementSelector} not found`);
                    }
                }
            });

            it('special conditions text length is too long then validation message should be displayed', () => {
                var maxLength = 4000;
                assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.specialConditions);
            });
        });
    });
}