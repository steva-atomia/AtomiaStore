/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Domains = Atomia.Api.Domains || {};
/* jshint +W079 */

(function (exports, _, utils) {
    'use strict';

    function FindDomains(searchTerm, success, error) {
        var requestData = {
            'searchQuery.Query': searchTerm
        };

        utils.request({
            resourceId: 'Domains.FindDomains',
            data: requestData,
            success: function (result) {
                if (success !== undefined) {
                    success(result);
                }
            },
            error: function (result) {
                if (error !== undefined) {
                    error(result);
                }
            }
        });
    }


    _.extend(exports, {
        FindDomains: FindDomains
    });

})(Atomia.Api.Domains, _, Atomia.Utils);
