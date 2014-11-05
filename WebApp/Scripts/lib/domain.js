var Atomia = Atomia || {};

Atomia.Domain = (function (request) {

    function findDomains(data, callback) {
        // TODO: pre-conditions

        request("Domains.FindDomains", data, callback);
    }

    request.define("Domains.FindDomains", "ajax", {
        url: "/Domains/FindDomains/",
        dataType: "json",
        type: "GET"
    });

} (amplify.request));
