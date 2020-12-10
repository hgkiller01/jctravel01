//=============== 大寫鎖定鍵打開提示 ============================================//
/*
使用：$("#txtPWD").capsLockTip(); //注意不要將些代碼加在$(document).ready()裡面。
*/
(function ($) {
    $.fn.extend({
        capsLockTip: function () {
            return this.each(function () {
                //創建對象實例並保存。
                //獲取實例對象：var api = $("#txtPWD").data("txtPWD");
                var ins = new $.CapsLockTip($(this));
                $(this).data(this.id, ins);
            });
        }
    });


    //創建一個實例。
    //___target jq目標對象。
    //___divTipID   顯示提示文本的div。
    $.CapsLockTip = function (___target) {
        //設置當前實例的配置參數。
        this.target = ___target;
        var _this = this;

        $(document).ready(function () {
            _this.target.bind("keypress", function (_event) {
                var e = _event || window.event;
                var kc = e.keyCode || e.which;
                var isShift = e.shiftKey || (kc == 16) || false;
                $.fn.capsLockTip.capsLockActived = false;
                if ((kc >= 65 && kc <= 90 && !isShift) || (kc >= 97 && kc <= 122 && isShift))
                    $.fn.capsLockTip.capsLockActived = true;
                _this.showTips($.fn.capsLockTip.capsLockActived);
            });

            _this.target.bind("keydown", function (_event) {
                var e = _event || window.event;
                var kc = e.keyCode || e.which;
                if (kc == 20 && null != $.fn.capsLockTip.capsLockActived) {
                    $.fn.capsLockTip.capsLockActived = !$.fn.capsLockTip.capsLockActived;
                    _this.showTips($.fn.capsLockTip.capsLockActived);
                }
            });

            _this.target.bind("focus", function (_event) {
                if (null != $.fn.capsLockTip.capsLockActived)
                    _this.showTips($.fn.capsLockTip.capsLockActived);
            });

            _this.target.bind("blur", function (_event) {
                _this.showTips(false);
            });
        });

        //創建顯示大寫鎖定的div。
        this.createTooltip = function () {
            if (null != $.fn.capsLockTip.divTip)
                return $.fn.capsLockTip.divTip;

            $("body").append("<div id='divTip__985124855558842555' style='width:100px; height:25px; padding-top:3px; display:none; position:absolute; z-index:9999999999999; text-align:center; background-color:#FDF6AA; color:Red; font-size:12px; border:solid 1px #DBC492; border-bottom-color:#B49366; border-right-color:#B49366;'>大寫鎖定已打開</div>");
            $.fn.capsLockTip.divTip = $("#divTip__985124855558842555");

            return $.fn.capsLockTip.divTip;
        };

        //顯示或隱藏大寫鎖定提示。
        this.showTips = function (display) {
            var divTip = _this.createTooltip();
            if (display) {
                var offset = _this.target.offset();
                divTip.css("left", offset.left + "px");
                divTip.css("top", offset.top + _this.target[0].offsetHeight + 3 + "px");
                divTip.show();
            }
            else {
                divTip.hide();
            }
        };

        //jq控件公用靜態對象。

        //提示框。
        $.fn.capsLockTip.divTip = null;
        //大寫鎖定鍵狀態
        $.fn.capsLockTip.capsLockActived = null;
    };
})(jQuery);