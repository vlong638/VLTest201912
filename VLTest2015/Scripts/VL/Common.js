vl = {
    //转换成：2016-07-11
    getDate: function getDate(date) {
        var d = eval('new ' + date.substr(1, date.length - 2));
        var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
        return ar_date.join('-');
    },
    //转换成：2016-07-11 15:00:28
    getDateTime: function getDateTime(date) {
        var d = eval('new ' + date.substr(1, date.length - 2));
        var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
        var ar_time = [d.getHours(), d.getMinutes(), d.getSeconds()];
        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
        for (var i = 0; i < ar_time.length; i++) ar_time[i] = dFormat(ar_time[i]);
        return ar_date.join('-') + ' ' + ar_time.join(':');
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
    }
}

function dFormat(i) {
    return i < 10 ? "0" + i.toString() : i;
}