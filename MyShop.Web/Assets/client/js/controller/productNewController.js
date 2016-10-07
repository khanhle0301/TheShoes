var productConfig = {
    pageSize: 12,
    pageIndex: 1,
}
var productNewController = {
    init: function () {
        productNewController.loadData();
        productNewController.registerEvent();
    },
    registerEvent: function () {
        $('.btn-popup').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productManange').modal('show');
            $('#hidProductID').val($(this).data('id'));
            var productId = parseInt($(this).data('id'));
            productNewController.loadColor(productId);
            productNewController.loadSize(productId);
            productNewController.loadProductName(productId);
        });

        $('#sortControl').off('click').on('click', function () {
            productNewController.loadData(true);
        });
        $('.price').off('click').on('click', function () {
            productNewController.loadData(true);
        });
        $('.vendor').off('click').on('click', function () {
            productNewController.loadData(true);
        });
        $('.color_wrapper').off('click').on('click', function () {
            productNewController.loadData(true);
        });
        $('.chatlieu').off('click').on('click', function () {
            productNewController.loadData(true);
        });

    },
    loadData: function (changePageSize) {
        var price = '';
        $(".prices li input[type=checkbox]:checked").each(function () {
            price += $(this).val() + ",";
        });
        price = price.slice(0, -1);
        var provider = '';
        $(".providers li input[type=checkbox]:checked").each(function () {
            provider += $(this).val() + ",";
        });
        provider = provider.slice(0, -1);

        var color = '';
        $(".colors li input[type=checkbox]:checked").each(function () {
            color += $(this).val() + ",";
        });
        color = color.slice(0, -1);

        var chatlieu = '';
        $(".chatlieus li input[type=checkbox]:checked").each(function () {
            chatlieu += $(this).val() + ",";
        });
        chatlieu = chatlieu.slice(0, -1);
      
        var sort = $('#sortControl').val();
        $.ajax({
            url: '/Product/LoadDataHotProduct',
            type: 'GET',
            data: {
                sort: sort,
                price: price,
                color: color,
                chatlieu: chatlieu,
                provider: provider,
                page: productConfig.pageIndex,
                pageSize: productConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductImage: item.Image,
                            ProductImage2: item.Image2,
                            ProductName: item.Name,
                            ProductID: item.ID,
                            ProductPrice: numeral(item.Price).format('0,0'),
                            ProductPromotionPrice: numeral(item.PromotionPrice).format('0,0'),
                            url: '/san-pham/' + item.ProductCategory.Alias + '/' + item.Alias + '-' + item.ID + '.html'
                        });

                    });
                    if (html == '') {
                        $('#notfound_product').show();
                        $('#product_top').hide();
                        $('#grid_pagination').hide();
                    }
                    else {
                        $('#notfound_product').hide();
                        $('#product_top').show();
                        $('#grid_pagination').show();
                        $('#tblData').html(html);
                        $('#productCount').text(response.total);
                        productNewController.paging(response.total, function () {
                            productNewController.loadData();
                        }, changePageSize);
                    }
                    productNewController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / productConfig.pageSize);
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "«",
            next: "›",
            last: "»",
            prev: "‹",
            visiblePages: 10,
            onPageClick: function (event, page) {
                productConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
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
                    var template = $('#tpl-product-color').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductColor: item.Name,
                        });
                    });

                    $('#product-color').html(html);
                }
            }
        })
    },

    loadProductName: function (id) {
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
                }
            }
        })
    }
}
productNewController.init();