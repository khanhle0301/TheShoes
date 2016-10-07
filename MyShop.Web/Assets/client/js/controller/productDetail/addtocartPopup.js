var cart = {
    init: function () {
        cart.registerEvents();
    },
    registerEvents: function () {

        $('.btnAddToCartDetail').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($('#hidProductIDPopup').val());
            var quantity = parseInt($('#ProductDetailsPopupForm .product-quantity input.qty').val());
            var size = document.getElementById("productPopup-size").value;
            var color = document.getElementById("productPopup-color").value;
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId,
                    quantity: quantity,
                    size: size,
                    color: color
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        alert('Thêm sản phẩm thành công.');
                        document.location.reload();
                    }
                    else {
                        alert(response.message);
                    }
                }
            });
        });
    }
}
cart.init();