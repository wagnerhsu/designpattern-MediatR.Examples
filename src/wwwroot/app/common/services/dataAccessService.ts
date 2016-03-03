﻿/// <reference path="../../typings/_all.d.ts" />

module Antares.Services {
    import Resources = Common.Models.Resources;

    export class DataAccessService {
        constructor(private $resource: ng.resource.IResourceService) {
        }

        getContactResource(): Resources.IBaseResourceClass<Resources.IContactResource> {
            //TODO move url to config
            return <Resources.IBaseResourceClass<Resources.IContactResource>>
                this.$resource('http://dev.api.antares.knightfrank.com/api/contacts/:id');
        }
    }

    angular.module('app').service('dataAccessService', DataAccessService);
}