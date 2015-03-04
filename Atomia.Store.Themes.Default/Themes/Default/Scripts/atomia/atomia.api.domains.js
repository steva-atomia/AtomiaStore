/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Domains = Atomia.Api.Domains || {};
/* jshint +W079 */

(function (exports, _, utils) {
    'use strict';

    var statusRequests = {},
        statusCheckCount = {},
        startInterval = 200,
        startIntervalCount = 5,
        finalInterval = 2000,
        statusCheckMaxCount = 40;

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
    function _CheckStatus(domainSearchId, success, error, interval) {
        var requestData = {
            'domainSearchId': domainSearchId
        },
            request,
            onGoingRequest;
        
        onGoingRequest = statusRequests[domainSearchId];

        if (onGoingRequest === undefined) {

            if (_.has(statusCheckCount, domainSearchId)) {
                statusCheckCount[domainSearchId] += 1;
            }
            else {
                statusCheckCount[domainSearchId] = 1;
            }

            request = utils.request({
                resourceId: 'Domains.CheckStatus',
                data: requestData,
                success: function (result) {
                    var nextInterval, checkAgain = true;

                    if (!result.FinishSearch) {
                        if (statusCheckCount[domainSearchId] < startIntervalCount) {
                            // 1. Check quickly at first
                            nextInterval = startInterval;
                        }
                        else if (interval < finalInterval) {
                            // 2. Linearly slow down check interval
                            nextInterval = Math.min(interval + startInterval, finalInterval);
                        }
                        else if (statusCheckCount[domainSearchId] <= statusCheckMaxCount) {
                            // 3. Check regularly until max number of checks
                            nextInterval = finalInterval;
                        }
                        else {
                            // 4. Stop checking.
                            checkAgain = false;
                        }

                        if (checkAgain) {
                            setTimeout(_CheckStatus, interval, domainSearchId, success, error, nextInterval);
                        }
                    }

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

    function CheckStatus(domainSearchId, success, error) {
        // Seed status check interval
        _CheckStatus(domainSearchId, success, error, startInterval);
    }


    // Optionally configure timing of status checks.
    function SetStatusCheckTiming(options) {
        if (options.startInterval !== undefined) {
            startInterval = options.startInterval;
        }

        if (options.startIntervalCount !== undefined) {
            startIntervalCount = options.startIntervalCount;
        }

        if (options.finalInterval !== undefined) {
            finalInterval = options.finalInterval;
        }

        if (options.statusCheckMaxCount !== undefined) {
            statusCheckMaxCount = options.statusCheckMaxCount;
        }
    }

    _.extend(exports, {
        FindDomains: FindDomains,
        CheckStatus: CheckStatus,
        SetStatusCheckTiming: SetStatusCheckTiming
    });

})(Atomia.Api.Domains, _, Atomia.Utils);
