vl_easyui = {
    config: function config(configResponse) {
        $("#CustomConfigId").val(configResponse.CustomConfigId);
    },
    wheres: function wheres(viewConfig) {
        for (var i = 0; i < viewConfig.Wheres.length; i++) {
            var where = viewConfig.Wheres[i];
            console.log("wheres");
            var control = $("#where_" + where.ComponentName);
            if (control[0] && control[0].type == "select-one") {
                control.combobox('setValue', where.Value);
            }
            else if (control[0] && control[0].type == "text") {
                if (control[0].className == "") {
                    control.val(where.Value)
                } else {
                    control.textbox('setValue', where.Value);
                }
            } else {
            }
        }
    },
    properties: function properties(viewConfig) {
        var columns = [[]]
        for (var i = 0; i < viewConfig.Properties.length; i++) {
            var column = {}
            var item = viewConfig.Properties[i];
            column.field = item.ColumnName;
            column.title = item.DisplayName;
            column.width = item.DisplayWidth;
            column.sortable = item.IsSortable;
            column.checkbox = item.IsCheckable;
            column.halign = 'center';
            column.align = 'left';

            //数据格式化
            //Hidden
            if (item.DisplayType == 1) {
                column.hidden = true;
            }
            ////Date
            //if (item.DisplayType == 3) {
            //    column.formatter = function (value) {
            //        return vl.getDate(value);

            //        //var date = new Date(value);
            //        //if (date == "Invalid Date") {
            //        //    return "";
            //        //}
            //        //var year = date.getFullYear();
            //        //var month = date.getMonth() + 1;
            //        //var day = date.getDate();
            //        //return year + "-" + month + "-" + day;
            //    }
            //}
            ////DateTime
            //if (item.DisplayType == 31) {
            //    column.formatter = function (value) {
            //        return vl.getDateTime(value);

            //        //var date = new Date(value);
            //        //if (date == "Invalid Date") {
            //        //    return "";
            //        //}
            //        //var y = date.getFullYear();
            //        //var m = date.getMonth() + 1;
            //        //var d = date.getDate();
            //        //var h = date.getHours();
            //        //var m1 = date.getMinutes();
            //        //var s = date.getSeconds();
            //        //m = m < 10 ? ("0" + m) : m;
            //        //d = d < 10 ? ("0" + d) : d;
            //        //return y + "-" + m + "-" + d + " " + h + ":" + m1 + ":" + s;
            //    }
            //}

            columns[0].push(column);
        }
        return columns;
    }
}