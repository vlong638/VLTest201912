/** EasyWeb iframe v3.1.8 date:2020-05-04 License By http://easyweb.vip */
layui.config({  // common.js是配置layui扩展模块的目录，每个页面都需要引入
    version: '318',   // 更新组件缓存，设为true不缓存，也可以设一个固定值
    base: getProjectUrl() + 'assets/module/'
}).extend({
    steps: 'steps/steps',
    notice: 'notice/notice',
    cascader: 'cascader/cascader',
    dropdown: 'dropdown/dropdown',
    fileChoose: 'fileChoose/fileChoose',
    Split: 'Split/Split',
    Cropper: 'Cropper/Cropper',
    tagsInput: 'tagsInput/tagsInput',
    citypicker: 'city-picker/city-picker',
    introJs: 'introJs/introJs',
    zTree: 'zTree/zTree',
    xmSelect: 'xm-select'
}).use(['layer', 'admin'], function () {
    var $ = layui.jquery;
    var layer = layui.layer;
    var admin = layui.admin;

});

/** 获取当前项目的根路径，通过获取layui.js全路径截取assets之前的地址 */
function getProjectUrl() {
    var layuiDir = layui.cache.dir;
    if (!layuiDir) {
        var js = document.scripts, last = js.length - 1, src;
        for (var i = last; i > 0; i--) {
            if (js[i].readyState === 'interactive') {
                src = js[i].src;
                break;
            }
        }
        var jsPath = src || js[last].src;
        layuiDir = jsPath.substring(0, jsPath.lastIndexOf('/') + 1);
    }
    return layuiDir.substring(0, layuiDir.indexOf('assets'));
}

// 封装ajax
function sendAjax(type, url, data, successCallback, contentType, dataType, failCallback) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        contentType: contentType || "application/json",
        dataType: dataType || "json",
        success: successCallback === null || typeof successCallback !== 'function' ? function (res) {
            console.log(res);
        } : successCallback,
        error: failCallback === null || typeof failCallback !== 'function' ? function (res) {
            console.log(res);
        } : failCallback
    })
}

function matchYMD(str) {
    let reg = /^\d{4}[/-]\d{1,2}[/-]\d{1,2}$/;
    return str.match(reg)
}

function matchNum(str) {
    let reg = /^[0-9]*$/;
    return str.match(reg)
}

function cut(str, target) {
    return str.replace(target === ' ' ? /\s+/g : target,"");
}

// 日期格式化
Date.prototype.format = function(format) {
    let o = {
        "M+" : this.getMonth()+1, //月份
        "d+" : this.getDate(),    //日
        "h+" : this.getHours(),   //小时
        "m+" : this.getMinutes(), //分
        "s+" : this.getSeconds(), //秒
        "q+" : Math.floor((this.getMonth()+3)/3),  //季度
        "S" : this.getMilliseconds() //毫秒
    };
    if(/(y+)/.test(format)) format=format.replace(RegExp.$1,
        (this.getFullYear()+"").substr(4 - RegExp.$1.length));
    for(let k in o)if(new RegExp("("+ k +")").test(format))
        format = format.replace(RegExp.$1, RegExp.$1.length === 1 ? o[k] : ("00"+ o[k]).substr((""+ o[k]).length));
    return format;
};