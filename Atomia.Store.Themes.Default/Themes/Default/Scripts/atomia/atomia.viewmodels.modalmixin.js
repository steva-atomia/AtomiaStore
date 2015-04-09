/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    /** Create notification model. */
    function ModalMixin() {
        var self = this;

        self.modalIsOpen = ko.observable(false);

        self.openModal = function openModal() {
            utils.publish('uiOpeningModal');
            self.modalIsOpen(true);
        };

        self.closeModal = function closeModal() {
            self.modalIsOpen(false);
        };

        self.toggleModal = function toggleModal() {
            self.modalIsOpen() ? self.closeModal() : self.openModal();
        };
       
        /** Close any open modal when another one is opening. */
        utils.subscribe('uiOpeningModal', function () {
            self.modalIsOpen(false);
        });
    }


    _.extend(exports, {
        ModalMixin: ModalMixin
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
