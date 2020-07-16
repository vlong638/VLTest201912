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
            //数据格式化
            //Date
            if (item.DisplayType == 3) {
                column.formatter = function (value) {
                    var dateMat = new Date(value);
                    if (dateMat == "Invalid Date") {
                        return "";
                    }
                    var year = dateMat.getFullYear();
                    var month = dateMat.getMonth() + 1;
                    var day = dateMat.getDate();
                    return year + "-" + month + "-" + day;
                }
            }

            columns[0].push(column);
        }
        return columns;
    }
}