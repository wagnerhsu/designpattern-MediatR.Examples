﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import Dto = Common.Models.Dto;
    import RequirementService = Antares.Requirement.RequirementService;
    import KfModalService = Antares.Services.KfModalService;
    import ActivityService = Services.ActivityService;
    import ChainTransaction = Common.Models.Business.ChainTransaction;

    export class OfferChainsControlController {
        // bindings
        isPanelVisible: Enums.SidePanelState;
        chainCommand: Common.Models.Commands.IChainTransactionCommand;
        property: Common.Models.Business.Property;
        config: IOfferChainsControlConfig;
        chainType: Enums.OfferChainsType;

        //fields
        currentChain: ChainTransaction;
        panelInPreviewMode: boolean = false;
        chains: Dto.IChainTransaction[];
        titleCode: string = "OFFER.VIEW.DELETE_CHAIN_TITLE";
        messageCode: string = "OFFER.VIEW.DELETE_CHAIN_MESSAGE";
        confirmCode: string = "OFFER.VIEW.DELETE_CHAIN_CONFIRM";

        constructor(private eventAggregator: Core.EventAggregator,
            private chainTransationsService: Services.ChainTransationsService,
            private kfModalService: KfModalService) { }

        private $onChanges = () => {
            var tempCommand = angular.copy(this.chainCommand);
            this.chains = tempCommand.chainTransactions;
        }

        public addChain = () => {
            this.currentChain = new ChainTransaction();
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        public editChain = (chain: ChainTransaction) => {
            this.currentChain = chain;
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        public previewChain = (chain: ChainTransaction) => {
            this.currentChain = chain
            this.panelInPreviewMode = true;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        public removeChain = (chain: ChainTransaction) => {
            this.currentChain = chain
            var promise = this.kfModalService.showModal(this.titleCode, this.messageCode, this.confirmCode);
            promise.then(this.onRemoveConfirm);
        }
        
        private onRemoveConfirm = () => {
            this.chainTransationsService
                .removeChain(this.currentChain, this.chainCommand, this.chainType)
                .then((model: Dto.IActivity | Dto.IRequirement) =>{
                    if(this.chainType === Enums.OfferChainsType.Activity){
                        var activity = <Dto.IActivity> model;
                        this.eventAggregator.publish(new ActivityUpdatedOfferChainsEvent(activity));
                    } else {
                        var requirement = <Dto.IRequirement> model;
                        this.eventAggregator.publish(new RequirementUpdatedOfferChainsEvent(requirement));
                    }
                })
        }

        isEndOfChainVisibleInPanel = () =>{
            return false;
        }
    }

    angular.module('app').controller('OfferChainsControlController', OfferChainsControlController);
}