(function ($) {
    var _ajax = $.ajax;

    $.getAntiForgeryToken = function () {
        // Finds the <input type="hidden" name={tokenName} value="..." /> from the specified window.
        // var inputElements = tokenWindow.$("input[type='hidden'][name=' + tokenName + "']");
        var tokenName = "__RequestVerificationToken";
        var inputElements = window.document.getElementsByTagName("input");

        for (var i = 0; i < inputElements.length; i++) {
            var inputElement = inputElements[i];
            if (inputElement.type === "hidden" && inputElement.name === tokenName) {
                return {
                    name: tokenName,
                    value: inputElement.value
                };
            }
        }
    };

    $.appendAntiForgeryToken = function (data, token) {
        // Converts data if not already a string.
        if (data && typeof data !== "string") {
            data = $.param(data);
        }

        // Gets token from current window by default.
        token = token ? token : $.getAntiForgeryToken();

        data = data ? data + "&" : "";
        // If token exists, appends {token.name}={token.value} to data.

        return token ? data + encodeURIComponent(token.name) + "=" + encodeURIComponent(token.value) : data;
    };

    // Wraps $.ajax(settings).
    $.ajax = function (settings) {
        if (settings !== undefined &&
            settings !== null &&
            settings.type !== undefined &&
            settings.type.toLowerCase() === "post") {
            var token = settings.token ? settings.token : $.getAntiForgeryToken();
            settings.data = $.appendAntiForgeryToken(settings.data, token);
        }

        return _ajax(settings);
    };
})(jQuery);
