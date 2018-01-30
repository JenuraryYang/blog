(function ($) {
    $.extend({
        "myJsonp": function (data) {
            var str = "[";
            for (var i = 0; i < data.length; i++) {
                str += "{" + data[i].name + ":\"" + data[i].value + "\"}";
                if (i < data.length - 1) {
                    str += ",";
                }

            }
            str += "]";
            return str;

        }
    })
})(jQuery)