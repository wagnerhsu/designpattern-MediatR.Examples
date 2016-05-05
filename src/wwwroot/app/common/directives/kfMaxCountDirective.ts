/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class KfMaxCount implements ng.IDirective {
        restrict = 'E';
        require = 'ngModel';
        scope = {
            ngModel: '=ngModel'
        };
        link: any;

        constructor() {
            this.link = this.unboundLink.bind(this);
        }

        unboundLink(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {

            ngModel.$validators['kfMaxCount'] = (modelValue: number) => {
                var maxCount: number = parseInt(attrs['maxCount']);
                if (isNaN(maxCount)) {
                    maxCount = 0;
                }

                return (modelValue !== undefined && modelValue <= maxCount);
            }
            scope.$watch('ngModel', (newValue, oldValue) => {
                if ((newValue !== undefined || oldValue !== undefined) && newValue !== oldValue) {
                    ngModel.$setDirty();
                }
            });
        }

        static factory() {
            var directive = () => {
                return new KfMaxCount();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('kfMaxCount', KfMaxCount.factory());
}