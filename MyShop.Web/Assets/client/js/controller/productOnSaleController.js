var productConfig = {
    pageSize: 12,
    pageIndex: 1,
}
var productOnSaleController = {
    init: function () {
        productOnSaleController.loadData();
        productOnSaleController.registerEvent();
    },
    registerEvent: function () {
        $('.btn-popup').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productManange').modal('show');
            $('#hidProductID').val($(this).data('id'));
            var productId = parseInt($(this).data('id'));
            productOnSaleController.loadColor(productId);
            productOnSaleController.loadSize(productId);
            productOnSaleController.loadProductName(productId);
        });

        $('#sortControl').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });
        $('.price').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });
        $('.provider').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });
        $('.color_wrapper').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });
        $('.chatlieu').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });
        $('.heel').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });
        $('.height').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });

        $('.types').off('click').on('click', function () {
            productOnSaleController.loadData(true);
        });

    },
    loadData: function (changePageSize) {
        var provider = '';
        $(".provider:checked").each(function () {
            provider += $(this).val() + ",";
        });
        provider = provider.slice(0, -1);

        var color = '';
        $(".mausac:checked").each(function () {
            color += $(this).val() + ",";
        });
        color = color.slice(0, -1);

        var heel = '';
        $(".heel:checked").each(function () {
            heel += $(this).val() + ",";
        });
        heel = heel.slice(0, -1);

        var height = '';
        $(".height:checked").each(function () {
            height += $(this).val() + ",";
        });
        height = height.slice(0, -1);

        var price = '';
        $(".price:checked").each(function () {
            price += $(this).val() + ",";
        });
        price = price.slice(0, -1);

        var chatlieu = '';
        $(".chatlieu:checked").each(function () {
            chatlieu += $(this).val() + ",";
        });
        chatlieu = chatlieu.slice(0, -1);

        var type = '';
        $(".types:checked").each(function () {
            type += $(this).val() + ",";
        });
        type = type.slice(0, -1);
       
        var sort = $('#sortControl').val();
        $.ajax({
            url: '/Product/LoadDataOnSaleProduct',
            type: 'GET',
            data: {
                sort: sort,
                type: type,
                heel: heel,
                height: height,
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
                        $('.productCount').html('Có <span class="require_symbol">' + response.total + '</span> sản phẩm.');
                        productOnSaleController.paging(response.total, function () {
                            productOnSaleController.loadData();
                        }, changePageSize);
                    }
                    productOnSaleController.registerEvent();
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
productOnSaleController.init();