var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {

        $('#frmContact').validate({
            rules: {
                name: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                },
                message: "required"
            },
            messages: {
                name: "Yêu cầu nhập tên",
                message: "Yêu cầu nhập nội dung",
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

        $('#template-contactform-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmContact').valid();
            if (isValid) {
                contact.createContact();
            }
        });

        contact.initMap();
    },

    createContact: function () {
        var contact = {
            Name: $('#template-contactform-name').val(),
            Email: $('#template-contactform-email').val(),
            Phone: $('#template-contactform-phone').val(),
            Message: $('#template-contactform-message').val(),
            Status: false
        }
        $.ajax({
            url: '/Contact/SendFeedback',
            type: 'POST',
            dataType: 'json',
            data: {
                contactViewModel: JSON.stringify(contact)
            },
            success: function (response) {

                if (response.status) {
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {
                        window.location.href = response.urlCheckout;
                    }
                    else {
                        setTimeout(function () {
                            $('#contactResult').show();
                            $('#template-contactform-name').val(''),
                            $('#template-contactform-email').val(''),
                            $('#template-contactform-phone').val(''),
                            $('#template-contactform-message').val('')
                        }, 2000);
                    }

                }
                else {
                    $('#contactResult').show();
                    $('#contactResult').text(response.message);
                }
            }
        });
    },

    initMap: function () {
        var uluru = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: uluru
        });

        var contentString = $('#hidAddress').val();

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            title: $('#hidName').val()
        });
        infowindow.open(map, marker);
    }
}

contact.init();