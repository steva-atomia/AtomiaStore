/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="atomia.utils.js" />

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

    /**
     * Find domains from a search term.
     * @param {string} searchTerm - The search term used to find domains
     * @param {Function} success - Callback for handling results
     * @param {Function} error - Callback on error.
     */
    function findDomains(searchTerm, success, error) {

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
    

    function _checkStatus(domainSearchId, success, error, interval) {
        var requestData = {
            'domainSearchId': domainSearchId
        },
            request,
            onGoingRequest;
        
        onGoingRequest = statusRequests[domainSearchId];

        // Only run one status check at a time.
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
                            setTimeout(_checkStatus, interval, domainSearchId, success, error, nextInterval);
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

            utils.subscribe('request.complete', function (settings, data) {
                if (data.DomainSearchId !== undefined) {
                    clearStatusRequest(data.DomainSearchId);
                }
            });
        }
    }

    /**
     * Check status for search with id 'domainSearchId'.
     * @param {string} domainSearchId - Id for search to check, received in results of 'findDomains'.
     * @param {Function} success - Callback on successful check.
     * @param {Function} error - Callback on error
     */
    function checkStatus(domainSearchId, success, error) {
        // Seed with a start interval.
        _checkStatus(domainSearchId, success, error, startInterval);
    }


    /**
     * Set status check timing options. Increases interval linearly from 'startInterval' to 'finalInterval'
     * @param {Object} options
     * @param {number} options.startInterval        - Time in ms for interval between checks to start.
     * @param {number} options.startIntervalCount   - How many times to use 'startInterval' before slowing down checks.
     * @param {number} options.finalInterval        - Time in ms for final interval between checks.
     * @param {number} options.statusCheckMaxCount  - How many times to check status before stopping.
     */
    function setStatusCheckTiming(options) {
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
        findDomains: findDomains,
        checkStatus: checkStatus,
        setStatusCheckTiming: setStatusCheckTiming
    });

})(Atomia.Api.Domains, _, Atomia.Utils);
