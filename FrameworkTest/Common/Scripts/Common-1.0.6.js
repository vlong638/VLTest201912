vl = {
    //转换成：2016-07-11
    getDate: function getDate(value) {
        if (value == null) {
            return "";
        }
        var date = new Date(value);
        if (date == "Invalid Date") {
            date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
            if (date == "Invalid Date") {
                return "";
            }
        }
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        m = m < 10 ? ("0" + m) : m;
        d = d < 10 ? ("0" + d) : d;
        return y + "-" + m + "-" + d;
    },
    //转换成：2016-07-11 15:00:28
    getDateTime: function getDateTime(value) {
        if (value == null) {
            return "";
        }
        var date = new Date(value);
        if (date == "Invalid Date") {
            date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
            if (date == "Invalid Date") {
                return "";
            }
        }
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        var h = date.getHours();
        var m1 = date.getMinutes();
        var s = date.getSeconds();
        m = m < 10 ? ("0" + m) : m;
        d = d < 10 ? ("0" + d) : d;
        h = h < 10 ? ("0" + h) : h;
        m1 = m1 < 10 ? ("0" + m1) : m1;
        s = s < 10 ? ("0" + s) : s;
        return y + "-" + m + "-" + d + " " + h + ":" + m1 + ":" + s;
    },
    /**
    *  返回url的参数值
    * name 参数名称
    */
    getQueryString: function getQueryString(name) {
        //获取到Url并且解析Url编码
        var url = decodeURI(location.search);
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {

            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest[name];
    },
    //下拉项处理,data需为[{value,text},{value,text}]
    select: function select(type, resultType, controlId) {
        $.ajax({
            url: '/Append/GetDropDowns?type=' + type + '&resultType=' + resultType,
            type: 'get',
            dataType: 'json',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    $(controlId).append("<option value='" + data[i].value + "'>" + data[i].text + "</option>");
                }
            }
        });
    },
    //下拉项处理,data需为[text,text]
    multiSelect: function multiSelect(type, resultType, controlId, isEdit) {
        $.ajax({
            url: '/Append/GetDropDowns?type=' + type + '&resultType=' + resultType,
            type: 'get',
            dataType: 'json',
            success: function (data) {
                $(controlId).multiSelect({
                    type: "multi",
                    isEdit: isEdit,
                    allCheck: true,
                    opts: data
                });
            }
        });
    }
}

function dFormat(i) {
    return i < 10 ? "0" + i.toString() : i;
}