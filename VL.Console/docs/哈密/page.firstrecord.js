define(function () {
    var globalwrapper = "#mod_wrapper";
    var globalmod = null;
    var mydia = null;
    var dia_class = null;

    var myhighrisk = null;
    var highrisk_class = null;

    var share = null;
    var base = null;
    var ctrl = {
        btn_save: "#btn_save",
        btn_print: "#btn_print",
        btn_chufang: "#btn_chufang",
        btn_examorder: "#btn_examorder",
        btn_laborder: "#btn_laborder",
        btn_deletevr: "#btn_deletevr",
        form_td_widthflag: ".form_td_widthflag",
        btn_diff: "#btn_diff",
        btn_refresh: "#btn_refresh"
    };

    function getCtrl(id) {
        return $(globalwrapper).find(id);
    }
    function getID() {
        var t = getCtrl("[data-id='tmpl_id']");
        if (t != null && t != undefined) {
            return parseInt(t.val(), 10);
        }
    }
    function setID(id) {
        var t = getCtrl("[data-id='tmpl_id']");
        if (t != null && t != undefined) {
            t.val(id);
        }
    }

    //初始化界面
    function initstate() {
        autowidth($(globalwrapper));
        $(".panel-htop").remove();
        reload();

        //诊断
        dia_class.init(mydia, base, share);
        //高危
        highrisk_class.myinit(myhighrisk, base, share);
        //处方
        $(ctrl.btn_chufang).unbind("click").on("click", function () {
            require(["base_usercontrol", "jquery", "easyui"], function (a) {
                a.showwindow($("#win_chufang"));
            });
            return false;
        });
        //检查
        $(ctrl.btn_examorder).unbind("click").on("click", function () {
            require(["base_usercontrol", "jquery", "easyui"], function (a) {
                a.showwindow($("#win_examorder"));
            });
            return false;
        });
        //检验
        $(ctrl.btn_laborder).unbind("click").on("click", function () {
            require(["base_usercontrol", "easyui"], function (a) {
                a.showwindow($("#win_laborder"));
            });
            return false;
        });
    }
    function reload() {
        var hasfile = getID() > 0;
        var _req = myrequest.getrequest();
        //保存
        var btn_save = getCtrl(ctrl.btn_save);
        EventUtil.showOrHide(btn_save, globalmod != webglobal.enum_mod.view);
        EventUtil.bindclick(btn_save, save);
        //打印
        var btn_print = getCtrl(ctrl.btn_print);
        EventUtil.showOrHide(btn_print, hasfile);
        EventUtil.bindclick(btn_print, print);
        //删除
        var btn_deletevr = getCtrl(ctrl.btn_deletevr);
        EventUtil.showOrHide(btn_deletevr, hasfile && globalmod != webglobal.enum_mod.view && _req.isCanEdit());
        EventUtil.bindclick(btn_deletevr, deletevr);
        //刷新
        var btn_refresh = getCtrl(ctrl.btn_refresh);
        EventUtil.showOrHide(btn_refresh, hasfile && globalmod != webglobal.enum_mod.view && _req.isCanEdit());
        EventUtil.bindclick(btn_refresh, refresh);

        //痕迹
        var btn_diff = getCtrl(ctrl.btn_diff);
        EventUtil.showOrHide(btn_diff, hasfile);
        if (!myextend.isNull(btn_diff)) {
            var baseurl = webglobal.pages.Page_DiffList;
            var _req = myrequest.getrequest();
            baseurl = myextend.UrlUpdateParams(baseurl, "attachedid", getID());
            baseurl = myextend.UrlUpdateParams(baseurl, "attachedtype", webglobal.enum_difftype.firstrecord);
            baseurl = myextend.UrlUpdateParams(baseurl, "jigoudm", _req.jigoudm);
            btn_diff.attr("href", baseurl);
        }
    }
    //数据载入
    function loaddata(data) {
        if (globalwrapper == null || globalwrapper == undefined) return;
        $.get(webglobal.templates.mod_firstrecord, { stamp: Math.random() + 1 }, function (template) {
            var arr = [];
            arr.push(data);
            //template
            $.template("myTemplate", template);
            var newrow = $(globalwrapper).html($.tmpl("myTemplate", arr));
            initstate();
            require(["base_usercontrol"], function (a) {
                a.drawcontrol($(globalwrapper), globalmod, true, prerender, afterrender);
            });
        });

    }
    //新增
    function loadadd(data) {
        globalmod = webglobal.enum_mod.add;
        loaddata(data);
    }
    //编辑
    function loadedit(data) {
        globalmod = webglobal.enum_mod.edit;
        loaddata(data);
    }
    //只读
    function loadview(data) {
        globalmod = webglobal.enum_mod.view;
        loaddata(data);
    }
    //渲染前
    function prerender() {
        MaskUtil.mask($(globalwrapper), "正在加载，请稍候...");
    }
    //渲染后
    function afterrender(data) {
        MaskUtil.unmask($(globalwrapper));
        bindkeydown();
        autowidth();

        if (globalmod == webglobal.enum_mod.add) {
            getPredignosis(globalwrapper);
        }
        if (globalmod != webglobal.enum_mod.view) {
            LeaveCheckUtil.leave("form_wrapper", "离开前是否保存？", save, null, clearCache);
            saveCache();
        }
        if (globalmod == webglobal.enum_mod.edit) {
            dia_class.refresh(globalwrapper, save);
        }
        EventUtil.HideOnView(globalwrapper, ".btn_diagnosis_detail", globalmod != webglobal.enum_mod.view);
        EventUtil.HideOnView(globalwrapper, ".btn_highrisk_detail", globalmod != webglobal.enum_mod.view);

    }

    function bindkeydown() {
        require(["common", "jquery"], function () {
            PressUtil.bindkeydown($(globalwrapper), true, { btn_save: $(globalwrapper).find(ctrl.btn_save) });
        });
    }
    //保存
    function save(sender, success_callback, e) {
        var web_list_callback = function (data) {
            if (data == null || data == undefined) return;
            if (data.result == webglobal.enum_const.service_result_success) {
                TipUtil.ShowSuccess("保存成功");
                setID(data.info.id);
                //initstate();
                reload();
                base.reload(globalwrapper);

                save_highrisk(data.info.id);


                if (success_callback != null) {
                    success_callback(sender, e);
                }
            }
            else {
                $.messager.alert('错误', data.msg);
            }
        };
        var web_list_bef_callback = function (data) {
            EventUtil.setVisible(sender, false);
        };
        var web_list_com_callback = function (data) {
            EventUtil.setVisible(sender, true);
        };
        var web_list_err_callback = function (data) { };

        var d = getDiagnosis();
        base.setVal("tmpl_diagnosisdic", JSON.stringify(d), globalwrapper);

        var data = guiddata();
        if (data == null || data == undefined) return;
        var dataparam = "input=" + myextend.HtmlEncode(JSON.stringify(data));
        if (dataparam == "" || dataparam == undefined)
            return;
        myextend.ajaxPost(webglobal.services.SaveFirstRecord, dataparam, web_list_callback, web_list_bef_callback, web_list_com_callback, web_list_err_callback, true);

    }
    function setDiagnosis(data) {
        dia_class.set(data, globalwrapper);
    }
    function getDiagnosis() {
        return dia_class.get(globalwrapper);
    }
    function deletevr() {
        var id = getID();
        if (id <= 0 || isNaN(id)) {
            $.messager.alert('错误', "初诊记录不存在");
            return;
        }

        //提示
        $.messager.confirm('提示', "确认删除 所选 初诊记录？", function (r) {
            if (r) {
                do_deletevr(id);
            }
        });
    }
    function do_deletevr(id) {
        var web_list_callback = function (data) {
            if (data == null || data == undefined) return;
            if (data.result == webglobal.enum_const.service_result_success) {
                setID(0);
                reload();
            }
            else {
                $.messager.alert('提示', data.msg);
            }
        };
        var web_list_bef_callback = function (data) {
            MaskUtil.mask($(globalwrapper), "正在删除...");
        };
        var web_list_com_callback = function (data) {
            MaskUtil.unmask($(globalwrapper));
        };
        var web_list_err_callback = function (data) { };

        var _req = myrequest.getrequest();
        var dictionary = new myextend.Dictionary();
        dictionary.set("id", id);
        dictionary.set("jobnumber", _req.userid);

        var dataparam = "input=" + myextend.HtmlEncode(JSON.stringify(dictionary.getItems()));
        if (dataparam == "" || dataparam == undefined)
            return;
        myextend.ajaxPost(webglobal.services.DelFirstRecord, dataparam, web_list_callback, web_list_bef_callback, web_list_com_callback, web_list_err_callback, true);
    }


    function autowidth() {
        require(["jextend"], function () {
            $(document).ready(function () {
                //自适应宽度
                var w = $(globalwrapper).width() * 0.96;
                var td_w = $(globalwrapper).find(ctrl.form_td_widthflag).find("span").width();
                myextend.setCustomTable(parseInt(w, 10), parseInt(td_w, 10) <= 0 ? 74 : parseInt(td_w, 10), '.customtable');
            });
        });
    }

    function show() {
        require(["share_diagnosis", "share_highrisk", "share_services", "base_usercontrol", "tmpl", "jquery", "common", "jextend", "web_global", "easyui"], function (dia, high, s, b) {
            dia_class = dia;
            highrisk_class = high;
            base = b;
            share = s;
            mydia = {
                tmpl_maindiagnosis: "tmpl_maindiagnosis",
                tmpl_diagnosisinfo: "tmpl_diagnosisinfo",
                tmpl_secondarydiagnosis: "tmpl_secondarydiagnosis",
                tmpl_diagnosisdic: "tmpl_diagnosisdic",
                wrapper: globalwrapper,
                btn: ".btn_diagnosis_detail",
                win: "#win_diagnosis"
            };
            myhighrisk = {
                tmpl_highrisklevel: "tmpl_highrisklevel",
                tmpl_highriskreason: "tmpl_highriskreason",
                tmpl_highrisklevel_text: "tmpl_highrisklevel_text",
                tmpl_highriskdic: "tmpl_highriskdic",
                wrapper: globalwrapper,
                btn: ".btn_highrisk_detail",
                win: "#win_highrisk"
            };
            var web_list_callback = function (data) {
                if (data == null || data == undefined) return;
                if (data.result == webglobal.enum_const.service_result_success) {
                    var mod = data.info.mod;

                    //新增
                    if (mod == webglobal.enum_mod.add) {
                        loadadd(data.info);
                    }
                    //编辑
                    else if (mod == webglobal.enum_mod.edit) {
                        loadedit(data.info);
                    }
                    //只读
                    else if (mod == webglobal.enum_mod.view) {
                        loadview(data.info);
                    }
                }
            };
            //获取参数
            var _req = myrequest.getrequest();

            var dictionary = new myextend.Dictionary();
            //            dictionary.set("bingrenid", _req.bingrenid);
            //            dictionary.set("shenfenzh", _req.shenfenzh);
            //            dictionary.set("userid", _req.userid);
            //            dictionary.set("username", _req.username);
            //            dictionary.set("jigoudm", _req.jigoudm);
            //            dictionary.set("pid", _req.pid);
            //            dictionary.set("departmentcode", _req.departmentcode);
            //            dictionary.set("departmentname", _req.departmentname);
            //            dictionary.set("jiuzhenid", _req.jiuzhenid);
            _req.todic(dictionary);
            myextend.ajaxPost_simple(webglobal.services.GetOneFirstRecord, dictionary, web_list_callback, true);

        });
    }
    //预诊
    function getPredignosis(ctrl_wrapper) {
        share.getPreDiagnosis(ctrl_wrapper, function (ctrl_wrapper, data) {
            if (data == null || data == undefined) return;
            var t = 0;
            t = parseFloat(data.sbp);
            if (t > 0 && !isNaN(t)) {
                base.setVal("tmpl_sbp", t, ctrl_wrapper);
            }
            t = parseFloat(data.dbp);
            if (t > 0 && !isNaN(t)) {
                base.setVal("tmpl_dbp", t, ctrl_wrapper);
            }
            t = parseFloat(data.weight);
            if (t > 0 && !isNaN(t)) {
                base.setVal("tmpl_weight", t, ctrl_wrapper);
            }
            t = parseFloat(data.heartrate);
            if (t > 0 && !isNaN(t)) {
                base.setVal("tmpl_pulse", t, ctrl_wrapper);
            }
            t = parseFloat(data.height);
            if (t > 0 && !isNaN(t)) {
                base.setVal("tmpl_height", t, ctrl_wrapper);
            }
            setBMI();
        });
    }

    function setBMI() {
        var height = base.getVal("tmpl_height");
        var weight = base.getVal("tmpl_weight");

        var _intheight = parseInt(height, 10);
        if (_intheight <= 0 || isNaN(_intheight))
            return;
        var _intweight = parseInt(weight, 10);
        if (_intweight <= 0 || isNaN(_intweight))
            return;

        var _bmi = parseInt(_intweight * 10000 / (_intheight * _intheight), 10);
        if (_bmi <= 0 || isNaN(_bmi))
            return;
        base.setVal("tmpl_bmi", _bmi);
    }
    function print() {
        var web_list_callback = function (data) {
            if (data == null || data == undefined) return;
            if (data.result == webglobal.enum_const.service_result_success) {
                share.print(getID(), data.info.type);
            }
        };
        //获取参数
        var _req = myrequest.getrequest();

        var dictionary = new myextend.Dictionary();
        dictionary.set("q", webglobal.enum_printtype.firstrecord);
        dictionary.set("jigoudm", _req.jigoudm);

        myextend.ajaxPost_simple(webglobal.services.GetPrintType, dictionary, web_list_callback, true);
    }

    function height_change(sender) {
        setBMI();
    }
    function weight_change(sender) {
        setBMI();
    }
    function visitdate_selected(date) {
        var web_list_callback = function (data) {
            if (data == null || data == undefined) return;
            if (data.result == webglobal.enum_const.service_result_success) {
                base.setVal("tmpl_checkweek", data.info.checkweek, globalwrapper);
                base.setVal("tmpl_checkday", data.info.checkday, globalwrapper);
                base.setVal("tmpl_followupappointment", myextend.myformatter(data.info.followupappointment), globalwrapper);
            }
        };
        //获取参数
        var _req = myrequest.getrequest();

        var dictionary = new myextend.Dictionary();
        dictionary.set("bingrenid", _req.bingrenid);
        dictionary.set("shenfenzh", _req.shenfenzh);
        dictionary.set("jigoudm", _req.jigoudm);
        dictionary.set("visitdate", myextend.myformatter(date));
        

        myextend.ajaxPost_simple(webglobal.services.GetCheckWeekDay, dictionary, web_list_callback, true);
    }
    function setHighRisk(data) {
        highrisk_class.set(data);
    }

    function save_highrisk(id) {
        var web_list_callback = function (data) {
            if (data == null || data == undefined) return;
            if (data.result == webglobal.enum_const.service_result_success) {
                reloadHighRisk(data.info);
            }
        };
        var _req = myrequest.getrequest();

        var dictionary = new myextend.Dictionary();
        dictionary.set("bingrenid", _req.bingrenid);
        dictionary.set("shenfenzh", _req.shenfenzh);
        dictionary.set("jigoudm", _req.jigoudm);
        dictionary.set("id", id);

        myextend.ajaxPost_simple(webglobal.services.UpdateFirstHighRisk, dictionary, web_list_callback, true);
    }
    function reloadHighRisk(data) {
        var t = data.highriskdic == "" || data.highriskdic == undefined || data.highriskdic == null ? [] : JSON.parse(data.highriskdic);
        setHighRisk(t);
        require(["header"], function (a) { a.show(); });
    }
    //实时缓存 
    function ischange() {
        return base.isChange($(globalwrapper));
    }
    function guiddata() {
        var data = base.getFormDic(globalwrapper, globalmod == webglobal.enum_mod.add);
        if (data == null || data == undefined) return;
        //获取参数
        var _req = myrequest.getrequest();
        //        data.jigoudm = _req.jigoudm;
        //        data.shenfenzh = _req.shenfenzh;
        //        data.bingrenid = _req.bingrenid;
        //        data.doctorname = _req.username;
        _req.todata(data);
        return data;
    }
    function saveCache() {
        var enum_cachetype = webglobal.enum_cachetype.firstrecord;

        if (enum_cachetype == null || enum_cachetype == undefined) return;
        require(["thread"], function (a) {
            Concurrent.Thread.create(share.threadCacheRecord, enum_cachetype, ischange, guiddata, share.addCacheRecord);
        });
    }
    function clearCache(callback) {
        share.clearCacheRecord(webglobal.enum_cachetype.firstrecord, callback);
    }
    function linkage_Dateofprenatal(dateofprenatal) {
        if (dateofprenatal == null || dateofprenatal == undefined) return;
        var lastmenstrualperiod = null;
        var visitdate = myextend.myparser(base.getVal("tmpl_visitdate", globalwrapper));
        var weeks = myextend.getpregweekorday(lastmenstrualperiod, dateofprenatal, visitdate, 2, 1);
        var days = myextend.getpregweekorday(lastmenstrualperiod, dateofprenatal, visitdate, 2, 2);

        if (!isNaN(weeks) && !isNaN(days) && weeks > 0) {
            var data = { "checkweek": weeks, "checkday": days };

            base.setVal("tmpl_checkweek", weeks, globalwrapper);
            base.setVal("tmpl_checkday", days, globalwrapper);

            base.setVal("tmpl_chiefcomplaint", refresh_weekday(data, base.getVal("tmpl_chiefcomplaint", globalwrapper)), globalwrapper);
            base.setVal("tmpl_presenthistory", refresh_weekday(data, base.getVal("tmpl_presenthistory", globalwrapper)), globalwrapper);

            var t = getDiagnosis()
            $.map(t, function (item) {
                if (item.t + "" == "1") {
                    item.i = refresh_weekday(data, item.i);
                }
            });
            setDiagnosis(t);
        }
    }
    function refresh_weekday(data, info) {
        var weeks = data.checkweek;
        var days = data.checkday;
        var _info = info + "";
        _info = _info.replace(/孕(\w+)\+(\w+)周/, "孕" + weeks + "+" + days + "周");
        _info = _info.replace(/孕(\w+)周(\w+)天/, "孕" + weeks + "周" + days + "天");
        return _info;
    }

    //刷新实时信息
    function refresh() {
        require(["jquery", "common", "jextend", "web_global"], function () {
            var web_list_callback = function (data) {
                if (data == null || data == undefined) return;
                if (data.result == webglobal.enum_const.service_result_success) {
                    base.reload_reverse(globalwrapper, data.info);
                    TipUtil.ShowMsg("完成刷新");
                }
            };

            //获取参数
            var _req = myrequest.getrequest();

            var dictionary = new myextend.Dictionary();
            _req.todic(dictionary);

            myextend.ajaxPost_simple(webglobal.services.GetFirstRecord_RealTimeData, dictionary, web_list_callback, true);
        });
    }
    return {
        show: show,
        setBMI: setBMI,
        height_change: height_change,
        weight_change: weight_change,
        setDiagnosis: setDiagnosis,
        setHighRisk: setHighRisk,
        visitdate_selected: visitdate_selected,
        linkage_Dateofprenatal: linkage_Dateofprenatal
    }
});
