vl_easyui = {
    config: function config(configResponse) {
        $("#CustomConfigId").val(configResponse.CustomConfigId);
    },
    wheres: function wheres(viewConfig) {
        for (var i = 0; i < viewConfig.Wheres.length; i++) {
            var where = viewConfig.Wheres[i];
            $("#where_" + where.ComponentName).val(where.Value)
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
            column.halign = 'center';
            column.align = 'left';

            ////数据格式化
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