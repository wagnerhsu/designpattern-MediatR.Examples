/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CharacteristicSelect {
        characteristicId: string = null;
        text: string = null;
        isSelected: boolean = false;

        constructor(characteristic?: Dto.IPropertyCharacteristic) {
            if (characteristic) {
                this.characteristicId = characteristic.id;
                this.text = characteristic.text;
                this.isSelected = true;
            }
        }
    }
}