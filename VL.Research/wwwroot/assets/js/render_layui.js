1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83
84
85
86
87
88
89
90
91
92
93
94
95
96
97
98
99
100
101
102
103
104
105
106
107
108
109
110
111
112
113
114
115
116
117
118
119
120
121
122
123
124
125
126
127
128
129
130
131
132
133
134
135
136
137
138
139
140
141
142
143
144
145
146
147
148
149
150
151
152
153
154
155
156
157
158
159
160
161
162
163
164
165
166
167
168
169
170
171
172
173
174
175
176
177
178
179
180
181
182
183
184
185
186
187
188
189
190
191
192
193
194
195
196
197
198
199
200
201
202
203
204
205
206
207
208
209
210
211
212
213
214
215
216
217
218
219
220
221
222
223
224
225
226
227
228
229
230
231
232
233
234
235
236
237
238
239
240
241
242
243
244
245
246
247
248
249
250
251
252
253
254
255
256
257
258
259
260
261
262
263
264
265
266
267
268
269
270
271
272
273
274
275
276
277
278
279
280
281
282
283
284
285
286
287
288
289
290
291
292
293
294
295
296
297
298
299
300
301
302
303
304
305
306
307
308
309
310
311
312
313
314
315
316
317
318
319
320
321
322
323
324
325
326
327
328
329
330
331
332
333
334
335
336
337
338
339
340
341
342
343
344
345
346
347
348
349
350
351
352
353
354
355
356
357
358
359
360
361
362
363
364
365
366
367
368
369
370
371
372
373
374
375
376
377
378
379
380
381
382
383
384
385
386
387
388
389
390
391
392
393
394
395
396
397
398
399
400
401
402
403
404
405
406
407
408
409
410
411
412
413
414
415
416
417
418
419
420
421
422
423
424
425
426
427
428
429
430
431
432
433
434
435
436
437
438
439
440
441
442
443
444
445
446
447
448
449
450
451
452
453
454
455
456
457
458
459
460
461
462
463
464
465
466
467
468
469
470
471
472
473
474
475
476
477
478
479
480
481
482
483
484
485
486
487
488
489
490
491
492
493
494
495
496
497
498
499
500
501
502
503
504
505
506
507
508
509
510
511
512
513
514
515
516
517
518
519
520
521
522
523
524
525
526
527
528
529
530
531
532
533
534
535
536
537
538
539
540
541
542
543
544
545
546
547
548
549
550
551
552
553
554
555
556
557
558
559
560
561
562
563
564
565
566
567
568
569
570
571
572
573
574
575
576
577
578
579
580
581
582
583
584
585
586
587
588
589
590
591
592
593
594
595
596
597
598
599
600
601
602
603
604
605
606
607
608
609
610
611
612
613
614
615
616
617
618
619
620
621
622
623
624
625
626
627
628
629
630
631
632
633
634
635
636
637
638
639
640
641
642
643
644
645
646
647
648
649
650
651
652
653
654
655
656
657
658
659
660
661
662
663
664
665
666
667
668
669
670
671
672
673
674
675
676
677
678
679
680
681
682
683
684
685
686
687
688
689
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

    html = ``;
    $.each(list, function (index, item) {
        html += `
            <li class="layui-nav-item">
                <a style="color: black !important;"><i class="layui-icon" style="color: black !important;">` + item.icon + `</i>&emsp;<cite>` + item.text + `</cite></a>`
        $.each(item.childrenNode, function (_index, _item) {
            if (_index === 0) {
                html += `<dl class="layui-nav-child" style="background-color: #DFE1E7 !important;">`;
            }
            if (!isBlank(_item.url)) {
                html += `<dd><a lay-href="` + _item.url + `" style="color: black;">` + _item.text + `</a></dd>`;
            } else {
                html += `<dd><a style="color: black !important;">` + _item.text + `</a>`;
                $.each(_item.childrenNode, function (_index_, _item_) {
                    if (_index_ === 0) {
                        html += `<dl class="layui-nav-child" style="background-color: #D4D6DB !important;">`;
                    }
                    html += `<dd><a lay-href="` + _item_.url + `" style="color: black;">` + _item_.text + `</a></dd>`;
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
                    <div class="layui-form-item" style="display: flex;flex-wrap: nowrap;">
                        <div style="display: flex;flex-wrap: wrap;/*flex: 1 0 0;*/">`
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
                $.each(item.options, function (_index, _item) {
                    html += `<option value="` + _item.value + `" ` + (_item.checked ? 'selected' : '') + `>` + _item.name + `</option>`
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
            <div class="layui-inline" style="flex: 1 0 0;">&emsp;
                <button class="layui-btn icon-btn" lay-filter="tbSearch" lay-submit>
                    <i class="layui-icon">&#xe615;</i>搜索
                </button>
            </div>`
        html += `</div>`

    }
    html += `
        <div style="flex: 1 0 100px;">
            <div class="layui-inline" style="border: 1px solid #ccc;width: 26px;height: 26px;text-align: center;position: fixed;right: 55px;top: 35px;"
                title="编辑并保存" id="MODEL_SAVE">
                <i class="layui-icon layui-icon-edit"></i>
            </div>`

    if (!isBlank(_data.modelId)) {
        html += `
            <div class="layui-inline" style="border: 1px solid #ccc;width: 26px;height: 26px;text-align: center;position: fixed;right: 20px;top: 35px;"
                title="删除" id="MODEL_DELETE">
                <i class="layui-icon layui-icon-delete"></i>
            </div>`
    }
    html += `</div>`

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
            switch (item.type) {
                case 'window': {
                    scriptObject += `<a class="layui-btn layui-btn-normal layui-btn-xs" lay-event="window"
                        data-default="` + item.defaultParam.join('&') + `" data-url="` + item.url + `" data-params="` + item.params.join('|') + `" data-area='` + JSON.stringify(_data.table.add_btn.area) + `' data-yesFun="` + item.yesFun + `">` + item.text + `</a>`;
                    break;
                }
                case 'newPage': {
                    scriptObject += `<a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="newPage"
                        data-default="` + item.defaultParam.join('&') + `" data-url="` + item.url + `" data-params="` + item.params.join('|') + `">` + item.text + `</a>`;
                    break;
                }
                case 'confirm': {
                    scriptObject += `<a class="layui-btn layui-btn-warm layui-btn-xs" lay-event="confirm"
                        data-desc="` + item.desc + `" data-default="` + item.defaultParam.join('&') + `" data-url="` + item.url + `" data-params="` + item.params.join('|') + `">` + item.text + `</a>`;
                    break;
                }
            }
        })
        scriptObject += `</script>`;

        _this.after(scriptObject);

        _data.table.cols[0].push({ title: '操作', toolbar: '#tbBar', align: 'center', minWidth: 200 })
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
        THToolbar.push('<button lay-event="add" data-default="' + _data.table.add_btn.defaultParam.join('&') + '" data-url="' + _data.table.add_btn.url + '" data-area=\'' + JSON.stringify(_data.table.add_btn.area) + '\' data-type="' + _data.table.add_btn.type + '" class="layui-btn layui-btn-sm icon-btn"><i class="layui-icon">&#xe654;</i>' + _data.table.add_btn.text + '</button>&nbsp;')
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
        method: "post",
        contentType: "application/json",
        page: _data.table.page,
        toolbar: THToolbar.join(''),
        cellMinWidth: 100,
        limit: _data.table.limit,
        initSort: isBlank(_data.table.initSort) ? {} : _data.table.initSort,
        response: {
            statusCode: 200,
            dataName: 'data1', //规定数据列表的字段名称，默认：data
            countName: 'data2'
        },
        cols: _data.table.cols,
        done: function (res, curr, count) {

        },
    });

    //监听排序事件
    table.on('sort(dataTable)', function (obj) { //注：sort 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
        _data.table.initSort = obj
        table.reload('dataTable', {
            initSort: obj, //记录初始排序，如果不设的话，将无法标记表头的排序状态。
            where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                field: obj.field, //排序字段
                order: obj.type //排序方式
            }
        });

        layer.msg('服务端排序。order by ' + obj.field + ' ' + obj.type);
    });

    /* 表格搜索 */
    form.on('submit(tbSearch)', function (data) {
        data.field.search = [];
        for (let i in data.field) {
            if (i !== 'search') {
                $.each(_data.search, function (index, item) {
                    if (item.name === i) {
                        if (item.type === 3 || item.type === 4) {
                            $.each(item.options, function (_index, _item) {
                                _item.checked = _item.value === data.field[i];
                            })
                        }
                    }
                })
                data.field.search.push({ key: i, value: data.field[i] })
            }
        }
        dataTable.reload({ where: data.field, page: { curr: 1 } });
        return false;
    });

    $('#MODEL_SAVE').on('click', function () {
        layer.prompt({
            formType: 0,
            value: isBlank(_data.modelName) ? '' : _data.modelName,
            title: '请输入模版名称',
            area: ['800px', '350px'], //自定义文本域宽高
            btn: ['保存', '取消']
        }, function (value, index, elem) {
            _data.modelId = isBlank(_data.modelId) ? "" : _data.modelId
            _data.modelName = value
            $.post(_data.saveModelUrl, JSON.stringify(_data), function (res) {
                layer.close(loadIndex);
                if (res.code === 200) {
                    layer.msg(res.msg, { icon: 1 });
                } else {
                    layer.msg(res.msg, { icon: 2 });
                }
            }, 'json');
            layer.close(index);
        });
        return false;
    });

    $('#MODEL_DELETE').on('click', function () {
        layer.confirm("确认删除此模版？", {
            skin: 'layui-layer-admin',
            shade: .1
        }, function (i) {
            console.log("_data.deleteModelUrl = " + _data.deleteModelUrl);
            $.get(_data.deleteModelUrl, { modelId: _data.modelId }, function (res) {
                layer.close(loadIndex);
                if (res.code === 200) {
                    layer.msg(res.msg, { icon: 1 });
                    _parent.location.reload();
                    _parent.admin.events.closeThisTabs();
                } else {
                    layer.msg(res.msg, { icon: 2 });
                }
            }, 'json');
        });
        return false;
    });

    /* 表格头工具栏点击事件 */
    table.on('toolbar(dataTable)', function (obj) {
        let _this = $(this);
        let url = _this.attr('data-url');
        if (!isBlank(_this.attr('data-default'))) {
            url += '?' + _this.attr('data-default')
        }
        if (obj.event === 'add') { // 添加
            if (_this.attr('data-type') === 'window') {
                admin.open({
                    type: 2,
                    area: JSON.parse(_this.attr('data-area')),
                    title: '',
                    content: [url],
                    success: function (layero, dIndex) {
                        $(layero).children('.layui-layer-content').css('overflow', 'visible');
                    }
                });
            }
            if (_this.attr('data-type') === 'newPage') {
                newTab(url, _this.text(), function () {
                    dataTable.reload({ page: { curr: 1 } });
                })
            }
        }
    });

    /* 表格工具条点击事件 */
    table.on('tool(dataTable)', function (obj) {
        let _this = $(this);
        let params = _this.attr('data-params').split("|");
        let url = _this.attr('data-url') + buildUrlParam(obj.data, params);
        if (!isBlank(_this.attr('data-default'))) {
            url += '&' + _this.attr('data-default')
        }
        if (obj.event === 'window') { // 弹窗
            let _index_;
            admin.open({
                type: 2,
                area: JSON.parse(_this.attr('data-area')),
                title: 'a',
                content: [url],
                btn: ['确认', '取消'],
                success: function (layero, dIndex) {
                    $(layero).children('.layui-layer-content').css('overflow', 'visible');
                    _index_ = dIndex;
                },
                yes: function () {
                    if (isBlank(_this.attr('data-yesFun'))) {
                        return false;
                    } else {
                        eval("$('#layui-layer-iframe'+_index_)[0].contentWindow." + _this.attr('data-yesFun') + '()')
                    }
                }
            });
        } else if (obj.event === 'newPage') { // 新标签页
            newTab(url, _this.text(), function () {
                dataTable.reload({ page: { curr: 1 } });
            })
        } else if (obj.event === 'confirm') { // 提示框
            url = _this.attr('data-url')
            if (!isBlank(_this.attr('data-default'))) {
                url += '?' + _this.attr('data-default')
            }
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
                        layer.msg(res.msg, { icon: 1 });
                        dataTable.reload({ page: { curr: 1 } });
                    } else {
                        layer.msg(res.msg, { icon: 2 });
                    }
                }, 'json');
            });
        }
    });

    function buildUrlParam(data, params) {
        let urlParam = [];
        $.each(params, function (index, item) {
            urlParam.push(item + '=' + data[item])
        })
        if (urlParam.length !== 0) {
            return '?' + urlParam.join('&');
        }
        return ''
    }

    $('.datepicker').each(function () {
        laydate.render({
            elem: this,
            type: 'date',
            range: true,
            trigger: 'click'
        });
    })


    //在新标签页中 打开页面
    function newTab(url, tit, endFn) {
        index.openTab({
            title: tit,
            url: url,
            end: endFn
        })
    }
}

jQuery.prototype.renderForm = function (_data, _layui, _parent) {
    const _this = this;
    const $ = _layui.jquery,
        layer = _layui.layer,
        form = _layui.form,
        table = _layui.table,
        laydate = _layui.laydate,
        index = _parent.index,
        util = _layui.util,
        admin = _layui.admin,
        xmSelect = _layui.xmSelect;

    _this.html("")
    let html = ``;
    $.each(_data.cards, function (index, card) {
        html += `
            <div class="layui-card">
                <div class="layui-card-header">` + card.text + `</div>
                <div class="layui-card-body">`;
        if (!card.isTextArea) {
            $.each(card.content, function (index, row) {
                html += `<div class="layui-form-item layui-row" style="margin-bottom: 0;">`;
                html = displayRow(html, row)
                html += `</div>`;
            });
        } else {
            console.log(!card.isTextArea);
            html += `
                <div class="layui-form-item layui-row">
                    <div class="layui-inline layui-col-md12">
                        <label class="layui-form-label` + (card.areaRequired ? ' layui-form-required' : '') + `">` + card.areaText + `:</label>
                        <div class="layui-input-block">
                            <textarea rows="2" name="` + card.areaParam + `" class="layui-input"` + (card.areaRequired ? ' lay-verify="required" required' : '') + ` placeholder="请输入"
                                 style="resize: none;padding-top: 10px;height: ` + ((card.lineNum === null || card.lineNum === 0 ? 1 : card.lineNum) * (38 + 25) - 25) + `px"></textarea>
                        </div>
                    </div>
                </div>`
        }
        html += `
                </div>
            </div>`
    })

    console.log(html);

    _this.append(html);

    layui.form.render();

    $("textarea").each(function () {
        if ($(this).attr("data-defaultValue") !== null) {
            $(this).val($(this).attr("data-defaultValue"))
        }
    })

    if (!isBlank(_data.getUrl)) {
        setDefaultValue();
    }

    function setDefaultValue() {
        let url = isBlank(_data.getUrl_param) ? _data.getUrl : _data.getUrl + '?' + _data.getUrl_param.join('&');
        sendAjax('get', url, {}, function (res) {
            form.val('formAdvForm', res.data);
        }, "application/json", "json", function (res) {
            layer.msg("查询失败", { icon: 2 });
        })
    }

    // 表单提交事件
    form.on('submit(formAdvSubmit)', function (data) {
        data.field = getValue(data.field);
        console.log(data.field);
        let loadIndex = layer.load(2);
        $.get(_data.saveUrl, data.field, function (res) {  // 实际项目这里url可以是mData?'user/update':'user/add'
            layer.close(loadIndex);
            if (res.code === 200) {
                layer.msg(res.msg, { icon: 1 });
                _parent.admin.events.closeThisTabs();
            } else {
                layer.msg(res.msg, { icon: 2 });
            }
        }, 'json');
        return false;
    });

    $('.datepicker').each(function () {
        laydate.render({
            elem: this,
            type: 'date',
            trigger: 'click'
        });
    })

    $('.rangeDate').each(function () {
        laydate.render({
            elem: this,
            type: 'date',
            range: true,
            trigger: 'click'
        });
    })

    function getValue(data) {
        $('.rangeDate').each(function () {
            let params = $(this).attr('name')
            let values = $(this).val().split(' - ');
            delete data[params];
            $.each(params.split(','), function (index, item) {
                data[item] = values[index];
            })
        })

        let r = {};
        $('input[data-type="checkbox"]:checked').each(function () {
            let _this = $(this);
            if (isBlank(r[_this.attr('name')]))
                r[_this.attr('name')] = [];
            r[_this.attr('name')].push(_this.val());
        })
        for (let i in r) {
            data[i] = r[i].join(',');
        }

        return data
    }

    function displayRow(html, row) {
        $.each(row, function (index, element) {
            if (isBlank(element.child)) {
                html = getElement(html, element);
            } else {
                html += `<div class="layui-inline layui-col-md` + element.width + `" style="margin-bottom: 0">`
                html = displayRow(html, element.child)
                html += `</div>`
            }
        })

        return html;

        function getElement(html, element) {
            switch (element.attr.type) {
                case 1: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">
                            <input name="` + element.attr.param + `" class="layui-input"` + (element.attr.required ? ' lay-verify="required" required' : '') + `
                                value="` + (isBlank(element.attr.value) ? "" : element.attr.value) + `" placeholder="请输入"/>
                        </div>
                    </div>`;
                    break;
                }
                case 2: {
                    if (element.attr.range) {
                        html += `
                        <div class="layui-inline layui-col-md` + element.width + `">
                            <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                            <div class="layui-input-block" style="text-align: center;line-height: 36px">
                                <input name="` + element.attr.param[0] + `" class="layui-input" type="number" style="width: 45%;display: inline-block;float: left;"
                                    ` + (element.attr.required ? ' lay-verify="required" required' : '') + `value="` + element.attr.value[0] + `" placeholder="请输入"/>
                                    -
                                <input name="` + element.attr.param[1] + `" class="layui-input" type="number" style="width: 45%;display: inline-block;float: right;"
                                ` + (element.attr.required ? ' lay-verify="required" required' : '') + `value="` + element.attr.value[1] + `" placeholder="请输入"/>
                            </div>
                        </div>`;
                    } else {
                        html += `
                        <div class="layui-inline layui-col-md` + element.width + `">
                            <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                            <div class="layui-input-block">
                                <input name="` + element.attr.param + `" class="layui-input" type="number" value="` + (isBlank(element.attr.value) ? "" : element.attr.value) + `"
                                    ` + (element.attr.required ? ' lay-verify="required" required' : '') + ` placeholder="请输入"/>
                            </div>
                        </div>`;
                    }
                    break;
                }
                case 3: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">
                            <select name="` + element.attr.param + `" ` + (element.attr.required ? ' lay-verify="required" required' : '') + `>
                                <option value="">请选择</option>`;
                    $.each(element.attr.options, function (index, option) {
                        html += `<option value="` + option.value + `" ` + (option.checked ? 'selected' : '') + `>` + option.text + `</option>`
                    })
                    html += `
                            </select>
                        </div>
                    </div>`;
                    break;
                }
                case 4: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">`;
                    $.each(element.attr.options, function (index, option) {
                        html += `<input type="radio" name="` + element.attr.param + `" value="` + option.value + `" lay-skin="primary" title="` + option.text + `"` + (option.checked ? 'checked=""' : '') + `>`
                    })
                    html += `</div></div>`;
                    break;
                }
                case 5: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">`;
                    $.each(element.attr.options, function (index, option) {
                        html += `<input data-type="checkbox" type="checkbox" name="` + element.attr.param + `" value="` + option.value + `" lay-skin="primary" title="` + option.text + `"` + (option.checked ? ' checked=""' : '') + `>`
                    })
                    html += `</div></div>`;
                    break;
                }
                case 6: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">
                            <input type="checkbox" name="` + element.attr.param + `" lay-skin="switch" lay-text="是|否"` + (element.attr.value ? ' checked=""' : '') + `>
                        </div>
                    </div>`;
                    break;
                }
                case 7: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">
                            <input name="` + element.attr.param + `" class="layui-input icon-date` + (element.attr.range ? ' rangeDate' : ' datepicker') + `" autocomplete="off"
                                   ` + (element.attr.required ? ' lay-verify="required" required' : '') + ` value="` + (isBlank(element.attr.value) ? "" : element.attr.value) + `" placeholder="请输入"/>
                        </div>
                    </div>`;
                    break;
                }
                case 8: {
                    html += `
                    <div class="layui-inline layui-col-md` + element.width + `">
                        <label class="layui-form-label` + (element.attr.required ? ' layui-form-required' : '') + `">` + element.attr.text + `:</label>
                        <div class="layui-input-block">
                            <textarea rows="2" name="` + element.attr.param + `" class="layui-input"` + (element.attr.required ? ' lay-verify="required" required' : '') + `
                                style="resize: none;padding-top: 10px;height: ` + ((element.attr.lineNum === null || element.attr.lineNum === 0 ? 1 : element.attr.lineNum) * (38 + 25) - 25) + `px"
                                data-defaultValue="` + (isBlank(element.attr.value) ? "" : element.attr.value) + `" placeholder="请输入"></textarea>
                        </div>
                    </div>`;
                    break;
                }
            }
            return html;
        }
    }

}


function isBlank(s) {
    return s === null || s === undefined || s === "" || s.length === 0
}