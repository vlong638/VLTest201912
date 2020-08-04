jQuery.prototype.renderTable = function (_data, _layui, _parent) {
    const _this = this;
    const $ = _layui.jquery,
        layer = _layui.layer,
        form = _layui.form,
        table = _layui.table,
        laydate = _layui.laydate,
        index = _parent.index,
        util = _layui.util,
        admin = _layui.admin;
    let html = ``;
    _this.html("")
    html = `
        <div class="layui-card">
            <div class="layui-card-body">
                <form class="layui-form toolbar">
                    <div class="layui-form-item">`
    if (!isBlank(_data.search)) {
        // 处理搜索条件
        $.each(_data.search, function (index, item) {
            if (item.type === 1) {
                html += `
                <div class="layui-inline">
                    <label class="layui-form-label">` + item.text + `:</label>
                    <div class="layui-input-inline">
                        <input name="` + item.name + `" class="layui-input"  value="` + item.value + `"/>
                    </div>
                </div>`
            }
            if (item.type === 2) {
                html += `
                <div class="layui-inline">
                    <label class="layui-form-label">` + item.text + `:</label>
                    <div class="layui-input-inline">
                        <input type="number" name="` + item.name + `" class="layui-input" value="` + item.value + `"/>
                    </div>
                </div>`
            }
            if (item.type === 3) {
                html += `
                <div class="layui-inline">
                    <label class="layui-form-label">` + item.text + `:</label>
                    <div class="layui-input-inline">
                        <select name="` + item.name + `">
                            <option value="">请选择</option>`;
                $.each(item.value, function (_index, _item) {
                    html += `<option value="` + _item.value + `">` + _item.name + `</option>`
                });
                html += `</select></div></div>`
            }
            if (item.type === 5) {
                html += `
                <div class="layui-inline">
                    <label class="layui-form-label">` + item.text + `:</label>
                    <div class="layui-input-inline">
                        <input name="` + item.name + `" class="layui-input icon-date datepicker" value="` + item.value + `"/>
                    </div>
                </div>`
            }
        })
        html += `
        <div class="layui-inline">&emsp;
            <button class="layui-btn icon-btn" lay-filter="tbSearch" lay-submit>
                <i class="layui-icon">&#xe615;</i>搜索
            </button>
        </div>`
    }
    html += `</div></form>`

    if (!isBlank(_data.table)) {
        // 初始化表格元素
        html += `<table id="dataTable" lay-filter="dataTable"></table>`
    }

    html += `</div></div>`

    _this.append(html);

    _layui.form.render();

    let scriptObject = ``;

    if (!isBlank(_data.table.line_toolbar)) {
        // 处理行工具栏
        scriptObject = `<script type="text/html" id="tbBar">`;
        $.each(_data.table.line_toolbar, function (index, item) {
            let params = '';
            $.each(item.params, function (_index, param) {
                params += (param + '|');
            })
            params = params.slice(0, params.length - 1)
            switch (item.type) {
                case 'window': {
                    scriptObject += `<a class="layui-btn layui-btn-normal layui-btn-xs" lay-event="window" data-url="` + item.url + `" data-params="` + params + `">` + item.text + `</a>`;
                    break;
                }
                case 'newPage': {
                    scriptObject += `<a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="newPage" data-url="` + item.url + `" data-params="` + params + `">` + item.text + `</a>`;
                    break;
                }
                case 'confirm': {
                    scriptObject += `<a class="layui-btn layui-btn-warm layui-btn-xs" data-desc="` + item.desc + `"
                        lay-event="confirm" data-url="` + item.url + `" data-params="` + params + `">` + item.text + `</a>`;
                    break;
                }
            }
        })
        scriptObject += `</script>`;

        _this.after(scriptObject);

        _data.table.cols[0].push({title: '操作', toolbar: '#tbBar', align: 'center', minWidth: 200})
    }


    /*---↑页面渲染↑-----------------↓事件绑定↓---*/

    let where = {};
    if (!isBlank(_data.table.where)) {
        // 处理加载表格初始参数
        $.each(_data.table.where, function (index, item) {
            where[item.name] = item.value;
        })
    }

    let THToolbar = ['<p>']
    if (!isBlank(_data.table.add_btn)) {
        THToolbar.push('<button lay-event="add" data-url="' + _data.table.add_btn.url +'" data-type="' + _data.table.add_btn.type + '" class="layui-btn layui-btn-sm icon-btn"><i class="layui-icon">&#xe654;</i>' + _data.table.add_btn.text + '</button>&nbsp;')
    }
    THToolbar.push('</p>')
        /*['<p>',
        '<button lay-event="add" class="layui-btn layui-btn-sm icon-btn"><i class="layui-icon">&#xe654;</i>添加</button>&nbsp;',
        // '<button lay-event="del" class="layui-btn layui-btn-sm layui-btn-danger icon-btn"><i class="layui-icon">&#xe640;</i>删除</button>',
        '</p>']*/
    // 表格渲染
    let dataTable = _layui.table.render({
        elem: '#dataTable',
        url: _data.table.url,
        where: where,
        page: _data.table.page,
        toolbar: THToolbar.join(''),
        cellMinWidth: 100,
        cols: _data.table.cols,
        done: function (res, curr, count) {

        },
    });

    /* 表格搜索 */
    form.on('submit(tbSearch)', function (data) {
        dataTable.reload({where: data.field, page: {curr: 1}});
        return false;
    });

    /* 表格头工具栏点击事件 */
    table.on('toolbar(dataTable)', function (obj) {
        if (obj.event === 'add') { // 添加
            if ($(this).attr('data-type') === 'window') {
                admin.open({
                    type: 1,
                    title: '',
                    content: '',
                    success: function (layero, dIndex) {
                        // 回显表单数据
                        form.val('userEditForm', mData);
                        // 表单提交事件
                        form.on('submit(userEditSubmit)', function (data) {
                            data.field.roleIds = insRoleSel.getValue('valueStr');
                            let loadIndex = layer.load(2);
                            $.get(mData ? '../../json/ok.json' : '../../json/ok.json', data.field, function (res) {  // 实际项目这里url可以是mData?'user/update':'user/add'
                                layer.close(loadIndex);
                                if (res.code === 200) {
                                    layer.close(dIndex);
                                    layer.msg(res.msg, {icon: 1});
                                    insTb.reload({page: {curr: 1}});
                                } else {
                                    layer.msg(res.msg, {icon: 2});
                                }
                            }, 'json');
                            return false;
                        });
                        // 渲染多选下拉框
                        let insRoleSel = xmSelect.render({
                            el: '#userEditRoleSel',
                            name: 'userEditRoleSel',
                            layVerify: 'required',
                            layVerType: 'tips',
                            data: [{
                                name: '管理员',
                                value: 1
                            }, {
                                name: '普通用户',
                                value: 2
                            }, {
                                name: '游客',
                                value: 3
                            }]
                        });
                        // 回显选中角色
                        if (mData && mData.roles) {
                            insRoleSel.setValue(mData.roles.map(function (item) {
                                return item.roleId;
                            }));
                        }
                        // 禁止弹窗出现滚动条
                        $(layero).children('.layui-layer-content').css('overflow', 'visible');
                    }
                });
            }
            if ($(this).attr('data-type') === 'newPage') {
                newTab($(this).attr('data-url'), $(this).text(), function () {
                    dataTable.reload({page: {curr: 1}});
                })
            }
        }
    });

    /* 表格工具条点击事件 */
    table.on('tool(dataTable)', function (obj) {
        let _this = $(this);
        let url = _this.attr('data-url');
        let params = _this.attr('data-params').split("|");
        if (obj.event === 'window') { // 弹窗
            admin.open({
                type: 1,
                title: '',
                content: '',
                success: function (layero, dIndex) {
                    // 回显表单数据
                    form.val('userEditForm', mData);
                    // 表单提交事件
                    form.on('submit(userEditSubmit)', function (data) {
                        data.field.roleIds = insRoleSel.getValue('valueStr');
                        let loadIndex = layer.load(2);
                        $.get(mData ? '../../json/ok.json' : '../../json/ok.json', data.field, function (res) {  // 实际项目这里url可以是mData?'user/update':'user/add'
                            layer.close(loadIndex);
                            if (res.code === 200) {
                                layer.close(dIndex);
                                layer.msg(res.msg, {icon: 1});
                                insTb.reload({page: {curr: 1}});
                            } else {
                                layer.msg(res.msg, {icon: 2});
                            }
                        }, 'json');
                        return false;
                    });
                    // 渲染多选下拉框
                    let insRoleSel = xmSelect.render({
                        el: '#userEditRoleSel',
                        name: 'userEditRoleSel',
                        layVerify: 'required',
                        layVerType: 'tips',
                        data: [{
                            name: '管理员',
                            value: 1
                        }, {
                            name: '普通用户',
                            value: 2
                        }, {
                            name: '游客',
                            value: 3
                        }]
                    });
                    // 回显选中角色
                    if (mData && mData.roles) {
                        insRoleSel.setValue(mData.roles.map(function (item) {
                            return item.roleId;
                        }));
                    }
                    // 禁止弹窗出现滚动条
                    $(layero).children('.layui-layer-content').css('overflow', 'visible');
                }
            });
        } else if (obj.event === 'newPage') { // 新标签页
            newTab(url, _this.text(), function () {
                dataTable.reload({page: {curr: 1}});
            })
        } else if (obj.event === 'confirm') { // 提示框
            layer.confirm(_this.attr('data-desc'), {
                skin: 'layui-layer-admin',
                shade: .1
            }, function (i) {
                layer.close(i);
                let loadIndex = layer.load(2);
                let param = {};
                $.each(params, function (index, item) {
                    param[item] = obj.data[item];
                })
                $.get(url, param, function (res) {
                    layer.close(loadIndex);
                    if (res.code === 200) {
                        layer.msg(res.msg, {icon: 1});
                        dataTable.reload({page: {curr: 1}});
                    } else {
                        layer.msg(res.msg, {icon: 2});
                    }
                }, 'json');
            });
        }
    });

    laydate.render({
        elem: '.datepicker',
        type: 'date',
        range: true,
        trigger: 'click'
    });


    //在新标签页中 打开页面
    function newTab(url, tit, endFn) {
        index.openTab({
            title: tit,
            url: url,
            end: endFn
        })
    }
}

jQuery.prototype.renderList = function (_data, _layui) {
    const _this = this;
    const $ = _layui.jquery,
        index = _layui.index;
    let html = ``;
    _this.html("");

    let list = [];
    $.each(_data, function (index, item) {
        if (isBlank(item.parentId)) {
            let node_1 = item;
            node_1.childrenNode = [];
            $.each(_data, function (_index, _item) {
                if (_item.parentId === item.id) {
                    let node_2 = _item;
                    node_2.childrenNode = [];
                    $.each(_data, function (_index_, _item_) {
                        if (_item_.parentId === _item.id) {
                            node_2.childrenNode.push(_item_);
                        }
                    });
                    node_1.childrenNode.push(node_2);
                }
            });
            list.push(node_1)
        }
    })

    // console.log(list);

    html = ``;
    $.each(list, function (index, item) {
        html += `
            <li class="layui-nav-item">
                <a><i class="layui-icon">` + item.icon + `</i>&emsp;<cite>` + item.text + `</cite></a>`
        $.each(item.childrenNode, function (_index, _item) {
            if (_index === 0) {
                html += `<dl class="layui-nav-child">`;
            }
            if (!isBlank(_item.url)) {
                html += `<dd><a lay-href="` + _item.url + `">` + _item.text + `</a></dd>`;
            } else {
                html += `<dd><a>` + _item.text + `</a>`;
                $.each(_item.childrenNode, function (_index_, _item_) {
                    if (_index_ === 0) {
                        html += `<dl class="layui-nav-child">`;
                    }
                    html += `<dd><a lay-href="` + _item_.url + `">` + _item_.text + `</a></dd>`;
                    if (_index_ === _item.childrenNode.length) {
                        html += `</dl>`;
                    }
                });
                html += `</dd>`;
            }
            if (_index === item.childrenNode.length) {
                html += `</dl>`;
            }
        })
        html += `</li>`;
    })

    _this.append(html);
}

function isBlank(s) {
    return s === null || s === undefined || s === "" || s.length === 0
}