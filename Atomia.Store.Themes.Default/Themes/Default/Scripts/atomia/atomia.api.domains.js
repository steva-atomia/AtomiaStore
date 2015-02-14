/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Domains = Atomia.Api.Domains || {};
/* jshint +W079 */

(function (exports, _, utils) {
    'use strict';

    var statusRequests = {};

    function clearStatusRequest(domainSearchId) {
        statusRequests[domainSearchId] = undefined;
    }

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

    // Check for domain status. Only one request per domainSearchId at a time is allowed.
    function CheckStatus(domainSearchId, success, error) {
        var requestData = {
                'domainSearchId': domainSearchId
            },
            request,
            onGoingRequest;

        onGoingRequest = statusRequests[domainSearchId];

        if (onGoingRequest === undefined) {
            request = utils.request({
                resourceId: 'Domains.CheckStatus',
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

            statusRequests[domainSearchId] = request;

            utils.subscribe("request.complete", function (settings, data, status) {
                if (data.DomainSearchId !== undefined) {
                    clearStatusRequest(data.DomainSearchId);
                }
            });
        }
    }


    _.extend(exports, {
        FindDomains: FindDomains,
        CheckStatus: CheckStatus
    });

})(Atomia.Api.Domains, _, Atomia.Utils);
