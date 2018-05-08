'use strict';

angular.module('cmsAngular').factory('notificationService', [function () {

    return {
        push: function (message, status) {
            if (status) {
                PNotify.desktop.permission();
                (new PNotify({
                    title: 'Neo CMS',
                    text: message,
                    type: 'success',
                    desktop: {
                        desktop: true,
                        icon: "/Areas/admin/Statics/neocms/admin/images/success-icon.png"
                    }
                })).get().click(function (e) {
                    if ($('.ui-pnotify-closer, .ui-pnotify-sticker, .ui-pnotify-closer *, .ui-pnotify-sticker *').is(e.target)) return;
                });
            } else {
                PNotify.desktop.permission();
                (new PNotify({
                    title: 'Neo CMS',
                    text: message,
                    type: 'error',
                    desktop: {
                        desktop: true,
                        icon: "/Areas/admin/Statics/neocms/admin/images/warning.png"
                    }
                })).get().click(function (e) {
                    if ($('.ui-pnotify-closer, .ui-pnotify-sticker, .ui-pnotify-closer *, .ui-pnotify-sticker *').is(e.target)) return;
                });
            }
        }
    };
}]);