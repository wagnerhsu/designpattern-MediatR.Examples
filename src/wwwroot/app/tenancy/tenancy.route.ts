/// <reference path="../typings/_all.d.ts" />

module Antares.Tenancy {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.tenancy-view', {
                url: '/tenancy/view/:id',
                template: "<tenancy-view></tenancy-view>"
            })
            .state('app.tenancy-edit', {
                url: '/activity/:activityId/requirement/:requirementId/tenancy/edit',
                template: "<tenancy-edit tenancy='tenancy'></tenancy-edit>",
                controller: ($scope: ng.IScope, requirement: Dto.IRequirement, activity: Dto.IActivity) => {
                    var activityPreview = new Tenancy.ActivityPreviewEditModel(activity);
                    var requirementPreview = new Tenancy.RequirementPreviewEditModel(requirement);
                    
                    var tenancy = new Tenancy.TenancyEditModel();
                    tenancy.activity = activityPreview;
                    tenancy.landlords = activityPreview.landlords;
                    
                    tenancy.requirement = requirementPreview;
                    tenancy.tenants = requirementPreview.contacts;
                    
                    $scope['tenancy'] = tenancy;
                },
                resolve: {       
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => { 
                        var activityId: string = $stateParams['activityId'];

                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    },
                    requirement: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var requirementId = $stateParams['requirementId'];
                        return dataAccessService.getRequirementResource().get({ id: requirementId }).$promise;
                    }
                }
            });
    }
}