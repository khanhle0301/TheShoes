var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($('#hidProductID').val());
            var quantity = parseInt($('#ProductDetailsForm .product-quantity input.qty').val());
            var size = document.getElementById("product-size").value;
            var color = document.getElementById("product-color").value;
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
common.init();