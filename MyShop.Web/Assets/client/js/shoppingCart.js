﻿var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Số điện thoại được yêu cầu",
                    number: "Số điện thoại phải là số."
                }
            }
        });

        $('#btnUpdate').off('click').on('click', function () {
            var cartList = [];
            $.each($('.txtQuantity'), function (i, item) {
                cartList.push({
                    ProductId: $(item).data('id'),
                    Quantity: $(item).val(),
                    Size: $(item).data('size'),
                    Color: $(item).data('color')
                });
            });
            $.ajax({
                url: '/ShoppingCart/Update',
                type: 'POST',
                data: {
                    cartData: JSON.stringify(cartList)
                },
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        window.location.href = "/gio-hang.html";
                    }
                }
            });
        });

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            var size = $(this).data('size');
            var color = $(this).data('color');
            cart.deleteItem(productId, size, color);
        });

        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });

        $('.cart_checkout_btn').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/thanh-toan.html";
        });
       
        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }

        });

        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'NL') {
                $('.boxContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() == 'ATM_ONLINE') {
                $('.boxContent').hide();
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },
    deleteItem: function (productId, size, color) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId,
                size: size,
                color: color
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    window.location.href = "/gio-hang.html";
                }
            }
        });
    },
    createOrder: function () {
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[groupname="bankcode"]:checked').prop('id'),
            Status: false
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {

                        window.location.href = response.urlCheckout;
                    }
                    else {
                        console.log('create order ok');
                        $('#divCheckout').hide();
                        cart.deleteAll();
                        setTimeout(function () {
                            $('#cartContent').html('Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.');
                        }, 2000);
                    }

                }
                else {
                    $('#divMessage').show();
                    $('#divMessage').text(response.message);
                }
            }
        });
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();

                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var cartTotal = $("#cartTotal").html();
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductCategoryAlias: item.Product.ProductCategory.Alias,
                            ProductAlias: item.Product.Alias,
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: (item.Product.PromotionPrice == null) ? item.Product.Price : item.Product.PromotionPrice,
                            Size: item.Size,
                            Color: item.Color,
                            PriceF: (item.Product.PromotionPrice == null) ? numeral(item.Product.Price).format('0,0') : numeral(item.Product.PromotionPrice).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: (item.Product.PromotionPrice == null) ? numeral(item.Quantity * item.Product.Price).format('0,0') : numeral(item.Quantity * item.Product.PromotionPrice).format('0,0')
                        });
                    });

                    $('#cartBody').html(html + cartTotal);

                    if (html == '') {
                        $('#cartContent').html('<p>Không có sản phẩm nào trong giỏ hàng</p><a href="/san-pham/all.html"><i class="icon-line2-action-undo"></i> Tiếp tục mua hàng</a>');
                        $('#note').hide();
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0') + '₫');
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();