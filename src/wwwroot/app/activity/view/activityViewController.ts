/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Common.Models.Business;
    import CartListOrder = Common.Component.ListOrder;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ActivityViewController extends Core.WithPanelsBaseController {
        isAttachmentsPanelVisible: boolean;

        activity: Business.Activity;
        attachmentsCartListOrder: CartListOrder = new CartListOrder('createdDate', true);
        enumTypeActivityDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityDocumentType;
        entityType: Enums.EntityTypeEnum = Enums.EntityTypeEnum.Activity;

        activityAttachmentResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityAttachmentSaveCommand>;
        saveActivityAttachmentBusy: boolean = false;
        selectedOffer: Dto.IOffer;
        selectedViewing: Dto.IViewing;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private dataAccessService: Services.DataAccessService,
            private latestViewsProvider: LatestViewsProvider,
            private eventAggregator: Antares.Core.EventAggregator) {

            super(componentRegistry, $scope);

            this.activityAttachmentResource = dataAccessService.getAttachmentResource();

            eventAggregator
                .with(this)
                .subscribe(Common.Component.CloseSidePanelEvent, () => {
                    this.isAttachmentsPanelVisible = false;
                });

            eventAggregator
                .with(this)
                .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                    this.addSavedAttachmentToList(event.attachmentSaved);
                });
        }

        showPropertyPreview = (property: Business.PreviewProperty) => {
            this.components.propertyPreview().setProperty(property);
            this.showPanel(this.components.panels.propertyPreview);

            this.latestViewsProvider.addView({
                entityId: property.id,
                entityType: EntityType.Property
            });
        }

        showActivityAttachmentAdd = () => {
            this.isAttachmentsPanelVisible = true;
        }

        showActivityAttachmentPreview = (attachment: Common.Models.Business.Attachment) => {
            this.components.activityAttachmentPreview().setAttachment(attachment, this.activity.id);
            this.showPanel(this.components.panels.activityAttachmentPreview);
        }

        addSavedAttachmentToList = (result: Dto.IAttachment) => {
            var savedAttachment = new Business.Attachment(result);
            this.activity.attachments.push(savedAttachment);

            this.isAttachmentsPanelVisible = false;
        }

        saveAttachment = (attachment: Antares.Common.Component.Attachment.AttachmentUploadCardModel) => {
            return this.activityAttachmentResource.save({ id: this.activity.id }, new Antares.Common.Component.Attachment.ActivityAttachmentSaveCommand(this.activity.id, attachment))
                .$promise;

            // TODO KC: handle saveActivityAttachmentBusy flag
        }

        showViewingPreview = (viewing: Common.Models.Dto.IViewing) =>{
            this.selectedViewing = viewing;
            this.showPanel(this.components.panels.previewViewingsSidePanel);
        }

        showOfferPreview = (offer: Common.Models.Dto.IOffer) => {
            this.selectedOffer = offer;
            this.showPanel(this.components.panels.offerPreview);
        }

        cancelViewingPreview() {
            this.hidePanels();
        }

        goToEdit = () => {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        }

        navigateToOfferView = (offer: Common.Models.Dto.IOffer) =>{
            this.$state.go('app.offer-view', { id: offer.id });
        }

        defineComponentIds() {
            this.componentIds = {
                propertyPreviewId: 'viewActivity:propertyPreviewComponent',
                propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent',
                activityAttachmentAddSidePanelId: 'viewActivity:activityAttachmentAddSidePanelComponent',
                activityAttachmentAddId: 'viewActivity:activityAttachmentAddComponent',
                activityAttachmentPreviewId: 'viewActivity:activityyAttachmentPreviewComponent',
                activityAttachmentPreviewSidePanelId: 'viewActivity:activityyAttachmentPreviewSidePanelComponent',
                previewViewingSidePanelId: 'viewActivity:previewViewingSidePanelComponent',
                viewingPreviewId: 'viewActivity:viewingPreviewComponent',
                offerPreviewId: 'viewActivity:offerPreviewComponent',
                offerPreviewSidePanelId: 'viewActivity:offerPreviewSidePanelComponent'
            };
        }

        defineComponents() {
            this.components = {
                propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewId); },
                activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddId); },
                activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewId); },
                viewingPreview: () => { return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                offerPreview: () => { return this.componentRegistry.get(this.componentIds.offerPreviewId);  },
                panels: {
                    propertyPreview: () => { return this.componentRegistry.get(this.componentIds.propertyPreviewSidePanelId); },
                    activityAttachmentAdd: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentAddSidePanelId) },
                    activityAttachmentPreview: () => { return this.componentRegistry.get(this.componentIds.activityAttachmentPreviewSidePanelId); },
                    previewViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); },
                    offerPreview: () =>{ return this.componentRegistry.get(this.componentIds.offerPreviewSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('activityViewController', ActivityViewController);
}
