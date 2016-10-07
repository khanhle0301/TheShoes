var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        $('#recover-form-password').validate({
            rules: {
                email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                }
            }
        });

        $('#recover-password-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#recover-form-password').valid();
            if (isValid) {
                common.recoverPassword();
            }
        });


        $('#customer_login').validate({
            rules: {
                username: "required",
                password: "required"
            },
            messages: {
                username: "Nhập tài khoản",
                password: "Nhập mật khẩu"
            }
        });


        $('#login-form-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#customer_login').valid();
            if (isValid) {
                common.createLogin();
            }
        });

        $('#create_customer').validate({
            rules: {
                name: "required",
                phone: "required",
                email: {
                    required: true,
                    email: true
                },
                address: "required",
                username: "required",
                password: "required",
                repassword: {
                    equalTo: "#register-form-password"
                }
            },
            messages: {
                name: "Nhập họ tên",
                phone: "Nhập số điện thoại",
                username: "Nhập tài khoản",
                password: "Nhập mật khẩu",
                address: "Nhập địa chỉ",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                repassword: "Mật khẩu nhập lại không đúng"
            }
        });

        $('#register-form-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#create_customer').valid();
            if (isValid) {
                common.register();
            }
        });

    },
    recoverPassword: function () {
        var email = $('#recover-email').val();
        $.ajax({
            url: '/Account/RecoverPassword',
            data: {
                email: email
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    alert('Gởi thành công.');
                    window.location.href = "/dang-nhap.html";
                }
                else {
                    setTimeout(function () {
                        alert('Gởi thất bại.');
                    });
                }
            }
        });
    },
    createLogin: function () {
        var userName = $('#login-form-username').val();
        var passWord = $('#login-form-password').val();       
        $.ajax({
            url: '/Account/Login',
            data: {
                userName: userName,
                password: passWord               
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    window.location.href = "/";
                }
                else {
                    setTimeout(function () {
                        $('#loginResult').show();
                        $('#login-form-username').val(''),
                        $('#login-form-password').val('')
                    });
                }
            }
        });
    },
    register: function () {
        var register = {
            FullName: $('#register-form-name').val(),
            UserName: $('#register-form-username').val(),
            Password: $('#register-form-password').val(),
            Address: $('#register-form-address').val(),
            Email: $('#register-form-email').val(),
            PhoneNumber: $('#register-form-phone').val()
        }
        $.ajax({
            url: '/Account/Register',
            type: 'POST',
            dataType: 'json',
            data: {
                accountViewModel: JSON.stringify(register)
            },
            success: function (response) {

                if (response.status) {
                    alert('Đăng ký thành công.');
                    window.location.href = "/dang-nhap.html";
                }
                else {
                    setTimeout(function () {
                        $('#existsResult').text(response.message);
                        $('#registerResult').show();
                    });
                }
            }
        });
    }
}
common.init();