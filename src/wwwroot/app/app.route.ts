﻿/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('app', {
                url: '/app',
                abstract: true,
                templateUrl: 'app/app.html'
            });

        $stateProvider
            .state('app.contact-add', {
                url: '/contact/add',
                params: {},
                template: '<contact-add></contact-add>',
                resolve: {
                    'rootUrl': (configService: Antares.Services.ConfigService) => {
                        return configService.promise;
                    }
                }
            });

        $urlRouterProvider.otherwise('/app/contact/add');
    }
}