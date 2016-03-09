var Common = (function () {
    return {
        //LogoutAction: "Account/Logout",   
        IsNavigateAsynchronous: true,
        LoadData: true,
        IsLoadingNeeded: true,
        FlaggedColor: "#FFB84D",
        SelfDeclaredColor: "#D4E2B9",
        VerifiedColor: "#6F9B6B"
    };
}());

$(document).ajaxSuccess(function (event, request, settings) {
    if (request.responseJSON != undefined) {
        if (request.responseJSON.logoutAction == "Account/Logout") {
            Logout();
        }
        if (request.responseJSON.errorUrl == "Common/Error") {
            ErrorMessage(request.responseJSON.errorMessage, request.responseJSON.errorDisplay);
        }
    }
    $('#Loading').hide();
});
$.ajaxSetup({
    beforeSend: function () {
        if (Common.IsLoadingNeeded) {
            $('#Loading').show();
        }
        else {
            Common.IsLoadingNeeded = true;
        }
    },
    complete: function () {
        if ($.active == 1) {
            $('#Loading').hide();
        }
        else {
            $('#Loading').show();
        }
    }
});

function Logout() {
    window.location = "/Account/Logout";
}

function ErrorMessage(errorMessage, errorDisplay) {
    var obj = { errorMessage: errorMessage, displayDescription: errorDisplay };
    $.ajax({
        type: 'post',
        url: '/Common/Error',
        data: JSON.stringify(obj),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (typeof (response) != "undefined") {
                var div = $(response);
                $(div).appendTo('#main_Container');
                $('#main_Container').children().css('display', 'none');
                $('#error').show();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

/*Type of position parameter
top-right
bottom-right
bottom-left
top-left
top-full-
width
bottom-full-width
*/
function showMessage(type, message, fadeOut, position) {
    //To clear existing toastr
    toastr.clear();
    if (type == "success") {
        toastr.options.positionClass = 'toast-' + position;

        toastr.success(message);
    }
    else if (type == "info") {
        toastr.options.positionClass = 'toast-' + position;
        toastr.info(message);
    }
    else if (type == "error") {
        toastr.options.positionClass = 'toast-' + position;
        toastr.error(message);
    }
    else if (type == "warning") {
        toastr.options.positionClass = 'toast-' + position;
        toastr.warning(message);
    }
}
function showSuccessMessage(message, fadeOut) {
    //To clear existing toastr
    toastr.clear();
    //fadeOut = false;
    var position = "bottom-full-width";
    toastr.options.positionClass = 'toast-' + position;
    if (typeof fadeOut !== "undefined" && fadeOut == false) { toastr.options.timeOut = 0; }
    toastr.success(message);
}

function showErrorMessage(message, fadeOut) {
    //To clear existing toastr
    toastr.clear();
    //fadeOut = false;
    var position = "bottom-full-width";
    toastr.options.positionClass = 'toast-' + position;
    if (typeof fadeOut !== "undefined" && fadeOut == false) { toastr.options.timeOut = 0; }
    toastr.error(message);
}

function showWarningMessage(message, fadeOut) {
    //To clear existing toastr
    toastr.clear();
    //fadeOut = false;
    var position = "bottom-full-width";
    toastr.options.positionClass = 'toast-' + position;
    if (typeof fadeOut !== "undefined" && fadeOut == false) { toastr.options.timeOut = 0; }
    toastr.warning(message);
}

function showInfoMessage(message, fadeOut) {
    //To clear existing toastr
    toastr.clear();
    //fadeOut = false;
    var position = "bottom-full-width";
    toastr.options.positionClass = 'toast-' + position;
    if (typeof fadeOut !== "undefined" && fadeOut == false) { toastr.options.timeOut = 0; }
    toastr.info(message);
}


function SetFocus(txtBoxIdWithSelecter) {
    $(txtBoxIdWithSelecter).focus();
    $(txtBoxIdWithSelecter).css("outline", "none");
}

function SetBorderColor(txtBoxIdWithSelecter, color) {
    $(txtBoxIdWithSelecter).css("border", "1px solid " + color + "");
}
function RemoveBorderColor(txtBoxIdWithSelecter) {
    $(txtBoxIdWithSelecter).removeClass('input-validation-error');
    $(txtBoxIdWithSelecter).css("border", "1px solid #ccc");
}
//$(document).on('click', 'body', function () {
//    toastr.clear();
//});
function AddRequiredIcons() {
    var els = document.querySelectorAll('input[data-val-required]');
    for (var i = 0; i < els.length; i++) {
        var id = els[i].id;
        $('label[for="' + id + '"]').addClass('required');
    }
    var selectList = document.querySelectorAll('select[data-val-required]');
    for (var i = 0; i < selectList.length; i++) {
        var id = selectList[i].id;
        $('label[for="' + id + '"]').addClass('required');
    }
    var textAreaList = document.querySelectorAll('textarea[data-val-required]');
    for (var i = 0; i < textAreaList.length; i++) {
        var id = textAreaList[i].id;
        $('label[for="' + id + '"]').addClass('required');
    }
}

function convertToJsonDate(jsonDate) {
    //var dateFormat = dateTimeFormat;
    if (jsonDate != undefined && jsonDate != null) {
        var regex = /-?\d+/;
        var matches = regex.exec(jsonDate);
        var date = new Date(parseInt(matches[0]));
        var d = new Date(date);
        if (d.getFullYear() < 2000) {
            return "";
        }
        if (!isNaN(d.getDate()) && !isNaN(d.getMonth()) && !isNaN(d.getFullYear())) {
            //var dayPos = dateFormat.toLowerCase().lastIndexOf('d');
            //var monthPos = dateFormat.toLowerCase().indexOf('m');
            //var yearPos = dateFormat.toLowerCase().indexOf('y');
            //var dateSeparator = '-';
            //var formattedDate = '';
            //if (dayPos < monthPos) {
            //    if (monthPos < yearPos) {
            //        formattedDate = d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear();
            //    }
            //    else {
            //        formattedDate = d.getDate() + "-" + d.getFullYear() + "-" + (d.getMonth() + 1);
            //    }
            //}
            //else {

            //}
            formattedDate = d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear();
            return formattedDate;
        }
        else {
            return "";
        }
    }
    else {
        return "";
    }
}

//$(document).on('click', '.bottomBorderNone', function (e) {
//    $('.bottomBorder').removeClass('bottomBorder').addClass('bottomBorderNone');
//    $(this).removeClass('bottomBorderNone').addClass('bottomBorder');
//});







function ScrollTo(divId) {
    $('html,body').animate({
        scrollTop: $("#" + divId).position().top
    }, 'slow');

}

function ScrollToTop() {
    $('html,body').animate({
        scrollTop: 0
    }, 'slow');
}

function IsValidEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,10})?$/;
    if (!emailReg.test(email)) {
        return false;
    } else {
        return true;
    }

    var regex = "/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/";
    return regex.test(email);
}

$(".panel-heading span.clickable").off("click").on("click", function (e) {
    e.stopPropagation();

    $('.tipso_bubble').hide();
    if ($(this).hasClass('panel-collapsed')) {
        // expand the panel
        $(this).parents('.panel').find('.panel-body').slideDown();
        $(this).removeClass('panel-collapsed');
        $(this).find('i').removeClass('glyphicon-chevron-right').addClass('glyphicon-chevron-down');
    }
    else {
        // collapse the panel
        $(this).parents('.panel').find('.panel-body').slideUp();
        $(this).addClass('panel-collapsed');
        $(this).find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-right');
    }
});

$(".panel-default .panel-heading").off("click").on("click",  function (e) {
    e.stopPropagation();
    $('.tipso_bubble').hide();
    if ($(this).hasClass('panel-collapsed')) {
        // expand the panel
        $(this).parents('.panel').find('.panel-body').slideDown();
        $(this).removeClass('panel-collapsed');
        $(this).find('i').removeClass('glyphicon-chevron-right').addClass('glyphicon-chevron-down');
    }
    else {
        // collapse the panel
        $(this).parents('.panel').find('.panel-body').slideUp();
        $(this).addClass('panel-collapsed');
        $(this).find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-right');
    }
});

function SelectMenu(className) {
    var browserWidth = $(document).width();
    $('.bottom-border').removeClass('bottom-border');
    if (browserWidth>992){
        $("#" + className).addClass('bottom-border');
    }
}

$(document).on('click', '.tipso_close', function () {

    $('.tipso_bubble').remove();
});

function setCookie(cname, cvalue, exdays) {
    
    if (exdays == undefined || exdays == null)
        exdays = 1;
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function isElementExists(elementId) {
    return $(elementId).length > 0;
}

function createElementNSUpdated(param1, param2) {
    if (!document.createElementNS) {
        return $(document.createElement(param2));
    } else {
        return $(document.createElementNS(param1, param2));
    }

}

function IsOldBrowser() {
    if (!document.createElementNS) {
        return true;
    }
    else {
        return false;
    }
}


$(document).on({
    mouseenter: function () {
        var event = $(this);
        if (!$(this).hasClass('tab-selected')) {
            var belowDiv = event.next();
            var aboveDiv = event.prev();
            if (aboveDiv.hasClass('clear-both-left')) {
                aboveDiv.find('span').css('background-color', 'rgb(242,242,242)');
                aboveDiv.find('span').css('width', '100%');
            }
            if (belowDiv.hasClass('clear-both-left')) {
                belowDiv.find('span').css('background-color', 'rgb(242,242,242))');
                belowDiv.find('span').css('width', '100%');
            }
        }
    },
    mouseleave: function () {
        var event = $(this);
        if (!$(this).hasClass('tab-selected')) {
            var belowDiv = event.next();
            var aboveDiv = event.prev();
            if (aboveDiv.hasClass('clear-both-left') && !aboveDiv.prev().hasClass('tab-selected')) {
                aboveDiv.find('span').css('background-color', 'rgb(229, 229, 229)');
                aboveDiv.find('span').css('width', '90%');
            }
            if (belowDiv.hasClass('clear-both-left') && !belowDiv.next().hasClass('tab-selected')) {
                belowDiv.find('span').css('background-color', 'rgb(229, 229, 229)');
                belowDiv.find('span').css('width', '90%');

            }
        }
    }
}, '.leftpanel-text');

$(document).on('click', '.leftpanel-text', function () {
    $('.sticky-side-nav-bar-divider').css('background-color', 'rgb(229, 229, 229)');
    $('.sticky-side-nav-bar-divider').css('width', '90%');
    var event = $(this);
    var belowDiv = event.next();
    var aboveDiv = event.prev();
    if (aboveDiv.hasClass('clear-both-left')) {
        aboveDiv.find('span').css('background-color', 'rgb(242,242,242)');
        aboveDiv.find('span').css('width', '100%');
    }
    if (belowDiv.hasClass('clear-both-left')) {
        belowDiv.find('span').css('background-color', 'rgb(242,242,242))');
        belowDiv.find('span').css('width', '100%');
    }
});

$(document).on('click', '.navbar-collapse.in', function (e) {
    if ($(e.target).is('a')) {
        $(this).collapse('hide');
    }
});


$(document).on({
    mouseenter: function () {
        var event = $(this);
            var belowDiv = event.next();
            var aboveDiv = event.prev();
            if (aboveDiv.find('div').hasClass('dividerdesktop')) {
                aboveDiv.find('div').css('background-color', 'rgb(242,242,242)');
                aboveDiv.find('div').css('width', '100%');
            }
            if (belowDiv.find('div').hasClass('dividerdesktop')) {
                belowDiv.find('div').css('background-color', 'rgb(242,242,242)');
                belowDiv.find('div').css('width', '100%');
            }
        
    },
    mouseleave: function () {
        var event = $(this);
            var belowDiv = event.next();
            var aboveDiv = event.prev();
            if (aboveDiv.find('div').hasClass('dividerdesktop')) {
                aboveDiv.find('div').css('background-color', 'rgb(229, 229, 229)');
                aboveDiv.find('div').css('width', '95%');
            }
            if (belowDiv.find('div').hasClass('dividerdesktop')) {
                belowDiv.find('div').css('background-color', 'rgb(229, 229, 229)');
                belowDiv.find('div').css('width', '95%');

            }
    }
}, '.dropdown-item');

function CheckPermission(permissionName, callback) {
    var obj = { permission: permissionName };
    var hasPermisison = false;
    $.ajax({
        type: 'post',
        url: '/Admin/CheckPermission',
        data: JSON.stringify(obj),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (typeof (response) != "undefined" && response.result== true) {
                callback();
            }
            else {
                window.location.href = "/common/Error?message=Sorry! You do not have permission to view this page&displayDescription=true";
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

}

function FixedTableHeader(divId) {
    var header = document.getElementById('layout-header');
    var headerBox = header.getBoundingClientRect();
    var $table = document.getElementById(divId);
    if ($table != undefined) {

        var tableBox = $table.getBoundingClientRect();
        $('#' + divId + "-fixed-table-div").remove();
        if (headerBox.bottom >= tableBox.top && headerBox.bottom <= tableBox.bottom) {
            var tableHeaders = $('#' + divId).children('thead').children('tr:first-child').children(':visible');
            var trHtml = "";
            var htmlArray = [];
            tableHeaders.each(function (index) {
                if ($(this).attr('display') != "none") {
                    var thWidth = $(this).innerWidth();
                    var domElement = $($(this).wrap('<p/>').parent().html());
                    domElement.css('width', thWidth + 'px');
                    trHtml += domElement.wrap('<p/>').parent().html();
                    $(this).unwrap();
                }
            });
            var tableWidth = $('#' + divId).width();
            var newTable = "<div id=\"" + divId + "-fixed-table-div\"\" style=\"top:50px;position:fixed;width:" + tableWidth + "px;z-index:999\"><table class=\"table tableGrid\" id=\"" + divId + "-fixed-table\"\"><thead>" + trHtml + "</thead></table></div>";

            $(newTable).insertBefore('#' + divId);
        }
        else {
            $('#' + divId + "-fixed-table-div").remove();

        }
    }
}

function FixedTableHeaderWithPagination(divId) {
    var header = document.getElementById('layout-header');
    var headerBox = header.getBoundingClientRect();
    var $table = document.getElementById(divId);
    if ($table != undefined) {
        var tableBox = $table.getBoundingClientRect();
        var totalBox = $('#' + divId).parent().prev()[0].getBoundingClientRect();
        $('#' + divId + "-fixed-table-div").remove();
        if (headerBox.bottom >= totalBox.top && headerBox.bottom <= (tableBox.bottom - 100)) {
            var tableHeaders = $('#' + divId).children('thead').children('tr:first-child').children(':visible');
            var trHtml = "";
            var htmlArray = [];
            tableHeaders.each(function (index) {
                if ($(this).attr('display') != "none") {
                    var thWidth = $(this).innerWidth();
                    var domElement = $($(this).wrap('<p/>').parent().html());
                    domElement.css('width', thWidth + 'px');
                    trHtml += domElement.wrap('<p/>').parent().html();
                    $(this).unwrap();
                }
            });
            var tableWidth = $('#' + divId).width();
            var pagination = $($('#' + divId).parent().prev());
            paginationHtml = pagination.html();
            var newTable = "<div id=\"" + divId + "-fixed-table-div\"\" style=\"top:50px;position:fixed;background-color:white;width:" + tableWidth + "px;z-index:999\"><div style=\"width:100%;float:left;background-color:white;\">" + paginationHtml + "</div><table class=\"table tableGrid\" id=\"" + divId + "-fixed-table\"\"><thead>" + trHtml + "</thead></table></div>";

            $(newTable).insertBefore('#' + divId);
        }
        else {
            $('#' + divId + "-fixed-table-div").remove();

        }
    }
}
$(document).on({
    mouseenter: function () {
        var desc = $(this).attr("data-description");
        var width = $(this).attr("data-width");
        $(this).tipso('update', 'content',"<div style=\"padding-top:10px;padding-left:5px;text-align:left;\">" +desc+"</dev>");
        $(this).tipso('update', 'background', '#FF9933');
        if (width == undefined) {
            $(this).tipso('update', 'width', '350px');
        }
        else {
            $(this).tipso('update', 'width', width + 'px');
        }
        $(this).tipso('show');
    },
    mouseleave: function () {
        $(this).tipso('hide');
        $(this).tipso('remove');
        $(this).tipso('destroy');
    }
}, '.questions-tooltip-remove');
$(document).on({
    mouseenter: function () {
        var desc = $(this).attr("data-description");
        if (desc != undefined) {
            var width = $(this).attr("data-width");
            $(this).tipso('update', 'content', "<div style=\"padding-left:5px;text-align:left;\">" + desc + "</dev>");
            $(this).tipso('update', 'background', '#FF9933');
            if (width == undefined) {
                $(this).tipso('update', 'width', '350px');
            }
            else {
                $(this).tipso('update', 'width', width + 'px');
            }
            $(this).tipso('show');
        }
    },
    mouseleave: function () {
        $(this).tipso('hide');
        $(this).tipso('remove');
        $(this).tipso('destroy');
    }
}, '.profile-tooltip-remove');


String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
}