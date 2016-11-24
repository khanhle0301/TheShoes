var productConfig = {
    pageSize: 12,
    pageIndex: 1,
}
var productViewCountController = {
    init: function () {
        productViewCountController.loadData();
        productViewCountController.registerEvent();
    },
    registerEvent: function () {

        $('#sortControl').off('click').on('click', function () {
            productViewCountController.loadData(true);
        });
        $('.price').off('click').on('click', function () {
            productViewCountController.loadData(true);
        });
        $('.provider').off('click').on('click', function () {
            productViewCountController.loadData(true);
        });
        $('.color_wrapper').off('click').on('click', function () {
            productViewCountController.loadData(true);
        });
        $('.chatlieu').off('click').on('click', function () {
            productViewCountController.loadData(true);
            $("#gotoTop").trigger("click");
        });
        $('.heel').off('click').on('click', function () {
            productViewCountController.loadData(true);
            $("#gotoTop").trigger("click");
        });
        $('.height').off('click').on('click', function () {
            productViewCountController.loadData(true);
            $("#gotoTop").trigger("click");
        });

        $('.types').off('click').on('click', function () {
            productViewCountController.loadData(true);
            $("#gotoTop").trigger("click");
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
            url: '/Product/LoadDataViewCountProduct',
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
                            sale: (item.PromotionPrice != null) ? true : false,
                            ProductImage: item.Image,
                            ProductImage2: item.Image2,
                            ProductName: item.Name,
                            ProductID: item.ID,
                            PromotionPrice: numeral(item.PromotionPrice).format('0,0'),
                            ProductPrice: numeral(item.Price).format('0,0'),                           
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
                        productViewCountController.paging(response.total, function () {
                            productViewCountController.loadData();
                            $("#gotoTop").trigger("click");
                        }, changePageSize);
                    }
                    //$("#gotoTop").trigger("click");
                    productViewCountController.registerEvent();
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
    }
}
productViewCountController.init();