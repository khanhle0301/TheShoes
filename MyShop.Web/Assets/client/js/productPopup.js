var popup = {
    init: function () {       
        popup.registerEvent();
    },
    registerEvent: function () {

        $('.btn-popup').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productManange').modal('show');
            $('#hidProductID').val($(this).data('id'));
            var productId = parseInt($(this).data('id'));
            popup.loadData(productId);
            popup.loadSize(productId);
           
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
                    var template = $('#tpl-product-size').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductSize: item.ID,
                        });
                    });

                    $('#product-size').html(html);
                    popup.registerEvent();
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
                    $('#productName').html(productName);
                    popup.registerEvent();
                }
            }
        })
    }
}
popup.init();