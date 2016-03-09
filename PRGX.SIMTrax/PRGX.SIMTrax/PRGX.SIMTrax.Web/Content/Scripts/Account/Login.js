$(document).on('click', '#submitLogin', function (e) {
    $('#Loading').show();

    var password = $('#Password').val() //.trim();
    $('#Password').val(password);
    if (!$('#signinForm').valid()) {
        $('#Loading').hide();
        return false;
    }

    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

    var encryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(password), key, {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    $('#Password').val(encryptedPassword);
    var test = $('#signinForm').serialize();

    $.ajax({
        type: 'post',
        url: '/Account/Login',
        data: $('#signinForm').serialize(),
        async: true,
        success: function (response) {
            if (typeof (response) != "undefined") {
                if (!response.valid) {
                    $('#Loading').hide();
                    showErrorMessage(response.message);
                    $('#Email').val('');
                    $('#Password').val('');
                }
                else {
                    $('#Loading').hide();
                    window.location.href = response.redirectUrl;
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
});

function Register() {
    if ($('#registerType').val() == 1) {
        window.location.href = '/Supplier/Register'
    }
    else if ($('#registerType').val() == 2) {
        window.location.href = '/Buyer/Register'
    }
    else {
        showErrorMessage(selectBuyerOrSupplierText);
        SetBorderColor("#registerType", "red");
    }
    return;
}

$('#registerType').change(function () {
    if ($('#registerType').val() != "") {
        RemoveBorderColor('#registerType');
    }
});

function ForgotPassword() {
    $('#forgotPasswordEmail').val('');
    $('#modalForgotPassword').modal('show');
    RemoveBorderColor('#forgotPasswordEmail');
}

function TriggerNewPassword() {
    var email = $('#forgotPasswordEmail').val();
    if (!ValidateEmail(email)) {
        showErrorMessage('Enter Valid Email');
        SetBorderColor('#forgotPasswordEmail', 'red');
        return false;
    }
    RemoveBorderColor('#forgotPasswordEmail');
    $.ajax({
        type: 'post',
        url: '/Account/TriggerNewPassword',
        data: { email: email },
        async: true,
        success: function (response) {
            if (typeof (response) != "undefined") {
                if (!response.EmailExists) {
                    showErrorMessage(emailNotExitsText);
                    $('#modalForgotPassword').modal('hide');
                }
                if (response.IsSent) {
                    showSuccessMessage(passwordUrlSentText);
                    $('#modalForgotPassword').modal('hide');
                }
                $('#modalForgotPassword').modal('hide');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
}

function ValidateEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if (email == "" || !emailReg.test(email)) {
        return false;
    }
    else {
        return true;
    }
}
