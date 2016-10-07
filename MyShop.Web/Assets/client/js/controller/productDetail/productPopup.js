var productPopup = {
    init: function () {
        productPopup.registerEvent();
    },
    registerEvent: function () {

        $('.btn-productPopup').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productPopupManange').modal('show');
            $('#hidProductIDPopup').val($(this).data('id'));
            var productId = parseInt($('#hidProductIDPopup').val());
            productPopup.loadColor(productId);
            productPopup.loadSize(productId);
            productPopup.loadData(productId);
        });

    },

    loadSize: function (id) {
        $.ajax({
            url: '/Product/GetSize',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tpl-productPopup-size').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductSize: item.ID,
                        });
                    });
                    $('#productPopup-size').html(html);
                }
            }
        })
    },

    loadColor: function (id) {
        $.ajax({
            url: '/Product/GetColor',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tpl-productPopup-color').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductColor: item.Name,
                        });
                    });

                    $('#productPopup-color').html(html);
                }
            }
        })
    },

    loadData: function (id) {
        $.ajax({
            url: '/Product/GetAll',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var productName = res.data.Name;
                    $('#productPopupName').html(productName);
                }
            }
        })
    }
}
productPopup.init();