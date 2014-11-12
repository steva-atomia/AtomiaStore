(function () {
    amplify.request.define("Domains.FindDomains", "ajax", {
        url: "/Domains/FindDomains/",
        dataType: "json",
        type: "GET",
        decoder: "jsend"
    });


} ());
