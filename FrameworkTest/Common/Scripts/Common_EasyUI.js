vl_easyui = {
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