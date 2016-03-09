var IsPasswordCorrect = true;
var passwordChangeSuccess = false;

$(document).ready(function () {
    Common.IsLoadingNeeded = false;
    resetPassword();
});

function resetPassword() {
    $('#Loading').hide();
    $('#OldPassword').val('');
    $('#NewPassword').val('');
    $('#ConfirmPassword').val('');
    $('#OldPasswordError').removeClass('available');
    $('#OldPasswordError').removeClass('error-text');
    if (passwordChangeSuccess) {
        $('#old-password-div').show();
    }
}

$(document).on('click', '#changePassword', function () {
    $.validator.unobtrusive.parse($('#ResetPasswordForm'))
    if ($('#ResetPasswordForm').valid() && IsPasswordCorrect) {
        $.ajax({
            type: 'post',
            url: '/Account/ChangePassword',
            data: $('#ResetPasswordForm').serialize(),
            async: false,
            success: function (response) {
                if (!response.success) {
                    showErrorMessage(passwordResetFailed);
                }
                else {
                    Common.IsPassWordChanged = true;
                    passwordChangeSuccess = true;
                    window.location.href = response.redirectUrl;
                }
            }
        });
    }
    return false;
});

$('#NewPassword').blur(function () {
    var element = $("span[data-valmsg-for='NewPassword']");
    if (element.hasClass('field-validation-error')) {
        var msg = element.find("span[for='NewPassword']").html();
        if (msg = "Invalid Password Format") {
            showErrorMessage(passwordGuidelines);
        }
    }
});

$('#NewPassword').focus(function () {
    $('#passwordTipso').tipso({
        position: 'top',
        width: 300,
        offsetX: 215
    });
    $('#passwordTipso').tipso('update', 'content', '<div style=\"padding-left:20px;text-align:left;\">' + passwordShouldContainAtleast + ': </div> <div style=\"padding-left:20px;text-align:left;\">' + oneUpperCaseLetter + '<br/>' + oneLowerCaseLetter + '<br/>' + oneNumber + '<br/>' + oneSpecialCharacter + '<br/>' + passwordLengthErrorMessage + '<br/>' + startOrEndWithSpaces + '</div>'); $('#passwordTipso').tipso('show');
});

$('#NewPassword').focusout(function () {
    $('#passwordTipso').tipso('hide');
});

$('#OldPassword').blur(function () {
    var element = $("span[data-valmsg-for='OldPassword']");
    if (element.hasClass('field-validation-error')) {
        var msg = element.find("span[for='OldPassword']").html();
        if (msg = "Invalid Password Format") {
            showErrorMessage(passwordGuidelines);
        }
        return false;
    }
    var oldPassword = $('#OldPassword').val();
    var password = $('#Password').val();
    if (oldPassword != null && oldPassword != "") {
        OldPasswordValidation(oldPassword);
    }
    return false;
});
$('#OldPassword').keyup(function () {
    $('#OldPasswordError').removeClass('available');
    $('#OldPasswordError').removeClass('error-text');
    $('#OldPasswordErrorText').html('');
});

$('#ConfirmPassword').focus(function () {
    $('#cnfrmPasswordTipso').tipso({
        position: 'top',
        width: 300,
        offsetX: 215
    });
    $('#cnfrmPasswordTipso').tipso('update', 'content', '<div style=\"padding-left:20px;text-align:left;\">' + passwordShouldContainAtleast + ': </div> <div style=\"padding-left:20px;text-align:left;\">' + oneUpperCaseLetter + '<br/>' + oneLowerCaseLetter + '<br/>' + oneNumber + '<br/>' + oneSpecialCharacter + '<br/>' + passwordLengthErrorMessage + '<br/>' + startOrEndWithSpaces + '</div>'); $('#passwordTipso').tipso('show');
    $('#cnfrmPasswordTipso').tipso('show');
});

$('#ConfirmPassword').focusout(function () {
    $('#cnfrmPasswordTipso').tipso('hide');
});

$('#ConfirmPassword').blur(function () {
    var element = $("span[data-valmsg-for='ConfirmPassword']");
    if (element.hasClass('field-validation-error')) {
        var msg = element.find("span[for='ConfirmPassword']").html();
        if (msg = "Invalid Password Format") {
            showErrorMessage(passwordGuidelines);
        }
    }
});

function OldPasswordValidation(oldPassword) {
    $.ajax({
        type: "POST",
        url: "/Account/OldPasswordValidationForReset",
        data: { oldPassword: oldPassword },
        success: function (response) {
            if (response != undefined) {
                if (response.result) {
                    $('#OldPasswordError').removeClass('error-text');
                    $('#OldPasswordError').addClass('available');
                    IsPasswordCorrect = true;
                }
                else {
                    $('#OldPasswordError').removeClass('available');
                    $('#OldPasswordError').addClass('error-text');
                    $('#OldPasswordErrorText').html(enterCorrectPassword);
                    IsPasswordCorrect = false;
                }
                return response.result;
            }
            else {
                return false;
            }
        },
        error: function () {

        }
    });
}

$(document).on('click', '#cancel', function () {
    parent.history.back();
    return false;
});