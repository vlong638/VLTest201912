jQuery.fn.extend({
    renderTreeSelect: function (_layui, treeData, _setting) {
        const $ = _layui.jquery;
        const _this = $(this);
        const class_name = _this.attr('id') + '_select'
        _this.css("display", "none");

        let html = `
            <div class="layui-unselect layui-form-select ` + class_name + `">
                <div class="layui-select-title">
                    <input type="text" placeholder="请选择" value="" readonly="" class="layui-input layui-unselect ` + class_name + `">
                    <i class="layui-edge"></i>
                </div>
                <dl class="layui-anim layui-anim-upbit ` + class_name + `" style="display: none">
                    
                </dl>
            </div>`
        _this.after(html)

        _layui.tree.render({
            elem: 'dl.' + class_name,
            id: class_name,
            data: treeData,
            showCheckbox: false,
            accordion: false,
            click: function (obj) {
                console.log(obj)
                if (obj.data.children !== null && obj.data.children !== undefined && obj.data.children.length !== 0) {
                    //当前节点下是否有子节点
                    return false;
                } else {
                    $(".layui-unselect.layui-form-select." + class_name).removeClass("layui-form-selected");
                    $("dl." + class_name).hide();
                    $("input." + class_name).val(obj.data.title);
                    _this.val(obj.data.id)
                    console.log(_this.val());
                }
                // console.log(obj.data); //得到当前点击的节点数据
                // console.log(obj.state); //得到当前节点的展开状态：open、close、normal
                // console.log(obj.elem); //得到当前节点元素
            }
        });
        $(".layui-anim .layui-anim-upbit").on("click", function () {
            console.log("11")
        })

        $("div." + class_name).on('click', function () {
            // console.log($(".layui-unselect.layui-form-select." + class_name));
            if ($(".layui-unselect.layui-form-select." + class_name).hasClass("layui-form-selected")) {
                $(".layui-unselect.layui-form-select." + class_name).removeClass("layui-form-selected");
                $("dl." + class_name).hide();
            } else {
                $(".layui-unselect.layui-form-select." + class_name).addClass("layui-form-selected");
                $("dl." + class_name).show();
            }
        })

        $("dl." + class_name).on('click', function () {
            return false;
        })


    },
    setTreeVal: function (_layui, value) {
        const $ = _layui.jquery;
        const _this = $(this);
        const class_name = _this.attr('id') + '_select'

        $("input." + class_name).val(value.title);
        _this.val(value.id)

        // _layui.tree.setChecked(class_name, value); //单个勾选 id 为 value 的节点
        // _layui.tree.setChecked('demoId', [2, 3]); //批量勾选 id 为 2、3 的节点
    }
})