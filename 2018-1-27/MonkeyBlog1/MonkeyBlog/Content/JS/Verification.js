(function ($) {
    $.extend({
        //验证手机号格式
        isPoneAvailable: function (str) {
            var myreg = /^[1][3,4,5,7,8][0-9]{9}$/;
            if (!myreg.test(str)) {
                return false;
            } else {
                return true;
            }
        },
        //弹出消息提示
        Message: function (_val) {
            var _la = layer.open({

                title: "信息提示",
                area: ["250px", "160px"],
                content: _val,
                btn: ["确定"],
                yes: function (index) {
                    layer.close(_la);
                },
            });
        }
    });
})(jQuery);