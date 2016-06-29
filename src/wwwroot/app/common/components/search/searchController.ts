﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class SearchController {
        // component attributes (API)
        public loadItems: ($viewValue: string) => void;
        public onSelectItem: <T>($item: T) => void;
        public onChangeValue: <T>($item: T) => void;
        public onCancel: () => void;
        public itemTemplateUrl: string;
        public searchPlaceholder: string;

        // component data
        public options: SearchOptions;
        public selectedItem: any;

        public select = <T>($item: T) => {
            this.onSelectItem($item);

            if (this.options.nullOnSelect)
                this.selectedItem = null;
        }

        public change = () => {
            this.onChangeValue(this.selectedItem);

            if (this.options.nullOnSelect)
                this.selectedItem = null;
        }

        public cancel = () => {
            this.onCancel();

            this.selectedItem = null;
        }
    }

    angular.module('app').controller('SearchController', SearchController);
};