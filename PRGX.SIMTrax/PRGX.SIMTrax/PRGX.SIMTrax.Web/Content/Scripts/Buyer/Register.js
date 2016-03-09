$(document).ready(function () {
    AddRequiredIcons();
    for (var i = 1; i <= TurnOverListCount; i++) {
        var html = $('#TurnOver-' + i).html();
        html = html.replace(/&amp;pound;/g, "&pound;");
        html = html.replace(/&amp;euro;/g, "&euro;");
        $('#TurnOver-' + i).html(html);
    }
});

var isBuyerEmailExists = false;
var isBuyerOrganisationExists = false;

function ValidateEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if (email == "" || !emailReg.test(email)) {
        $('#BuyerEmailError').removeClass('available');
        $('#BuyerEmailError').removeClass('error-text');
        $('#BuyerEmailErrortext').html('');
        return false;
    }
    else {
        return true;
    }
}

$('#BuyerEmail').blur(function () {
    Common.IsLoadingNeeded = false;
    var email = $('#BuyerEmail').val();
    if (ValidateEmail(email)) {
        $.ajax({
            type: 'post',
            url: '/Account/IsEmailExists',
            data: { email: email },
            async: true,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.result) {
                        isBuyerEmailExists = true;
                        $('#BuyerEmailError').removeClass('available');
                        $('#BuyerEmailError').addClass('error-text');
                        $('#BuyerEmailErrortext').html(response.message);
                        $('#BuyerEmail').focus();
                    }
                    else {
                        isBuyerEmailExists = false;
                        $('#BuyerEmailError').removeClass('error-text');
                        $('#BuyerEmailError').addClass('available');
                        $('#BuyerEmailErrortext').html('');
                    }
                    $('#BuyerEmailValidationLoading').hide();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    return false;
});

$('#BuyerOrganisationName').blur(function () {
    Common.IsLoadingNeeded = false;
    var organisationName = $('#BuyerOrganisationName').val();
    if (organisationName == "") {
        $('#BuyerOrganisationError').removeClass('available');
        $('#BuyerOrganisationError').removeClass('error-text');
        $('#BuyerOrganisationErrorText').html('');
        return false;
    }
    else {
        $.ajax({
            type: 'post',
            url: '/Account/IsOrganisationExists',
            data: { organisationName: organisationName },
            async: true,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.result) {
                        isBuyerOrganisationExists = true;
                        $('#BuyerOrganisationError').removeClass('available');
                        $('#BuyerOrganisationError').addClass('error-text');
                        $('#BuyerOrganisationErrorText').html(response.message);
                    }
                    else {
                        isBuyerOrganisationExists = false;
                        $('#BuyerOrganisationError').removeClass('error-text');
                        $('#BuyerOrganisationError').addClass('available');
                        $('#BuyerOrganisationErrorText').html('');
                    }
                    $('#BuyerOrganisationValidationLoading').hide();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    return false;
});

$('#BuyerEmail').keyup(function () {
    $('#BuyerEmailError').removeClass('available');
    $('#BuyerEmailError').removeClass('error-text');
    $('#BuyerEmailErrortext').html('');
});

$('#BuyerOrganisationName').keyup(function () {
    $('#BuyerOrganisationError').removeClass('available');
    $('#BuyerOrganisationError').removeClass('error-text');
    $('#BuyerOrganisationErrorText').html('');
});

$(document).on('click', '#submitBuyerDetails', function () {
    $.validator.unobtrusive.parse($('#BuyerRegisterForm'))
    if (!$('#BuyerRegisterForm').valid()) {
        var els = document.querySelector('.input-validation-error');
        if (els != null) {
            els.focus();
            setFocus = true;
        }
        return false;
    }
    if (isBuyerEmailExists) {
        $('#BuyerEmail').focus();
        return false;
    }
    if (isBuyerOrganisationExists) {
        $('#BuyerOrganisationName').focus();
        return false;
    }
    var notSure = $("#rdoBSNotSure").prop('checked');
    if (notSure == true) {
        var businessSector = $("#BusinessSectorDescription").val();
        if (businessSector == "") {
            showErrorMessage(industrySectorError);
            return false;
        }
    }
    $.ajax({
        type: 'post',
        url: '/Buyer/Register',
        data: $('#BuyerRegisterForm').serialize(),
        async: false,
        success: function (response) {
            if (response) {
                $('#buyer-registration-alert').modal('show');
            }
            else {
                showErrorMessage(defaultErrorMessage);
            }
        }
    });
    return false;
});

$(document).on('click', '#buyer-registration-alert-ok', function () {
    $('#buyer-registration-alert').modal('hide');
    window.location.href = "/Account/Logout";
});