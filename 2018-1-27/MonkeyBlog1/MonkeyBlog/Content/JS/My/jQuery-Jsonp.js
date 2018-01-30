(function ($) {
    //序列化和我返序列化
    $.extend({
        //没有复选框时用的方法
        "myJsonp1": function (data) {
            var jsonstr = "{";
            for (var i = 0; i < data.length; i++) {
                jsonstr += "\"" + data[i].name + "\":\"" + data[i].value + "\"";
                if (i != data.length - 1) {
                    jsonstr += ",";
                }
            }
            jsonstr += "}";

            return jsonstr;

        }, "myJsonp2": function (data) {     //有复选框时用的方法
            var jsonarry = new Array();
            for (var i = 0; i < data.length; i++) {
                if (jsonarry[data[i].name] != null) {

                    jsonarry[data[i].name] = jsonarry[data[i].name] + "," + data[i].value;
                } else {

                    jsonarry[data[i].name] = data[i].value;
                }

            }
            var jsonstr = "{";
            for (var key in jsonarry) {
                jsonstr += "\"" + key + "\":\"" + jsonarry[key] + "\"";
                jsonstr += ",";
            }

            jsonstr = jsonstr.substr(0, jsonstr.length - 1);
            jsonstr += "}";


            return jsonstr;
        }, "myJsonp3": function (data) {     //有复选框时和有编辑器时用的方法
            var jsonarry = new Array();
            for (var i = 0; i < data.length; i++) {
                if (jsonarry[data[i].name] != null) {

                    jsonarry[data[i].name] = jsonarry[data[i].name] + "," + data[i].value;
                } else {

                    jsonarry[data[i].name] = data[i].value;
                }

            }
            var jsonstr = "{";
            for (var key in jsonarry) {
                if (key != "editorValue") {
                    jsonstr += "\"" + key + "\":\"" + jsonarry[key] + "\"";
                    jsonstr += ",";
                }
             
            }

            jsonstr = jsonstr.substr(0, jsonstr.length - 1);
            jsonstr += "}";


            return jsonstr;
        }
    })
})(jQuery)