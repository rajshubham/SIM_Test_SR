
var to = false;

$(document).ready(function () {
    $("DummyOrganisationName").intellisense({
        url: '/Account/GetSuppliersListForRegistration',
        width: '93.8',
        actionOnEnter: CheckwhetherDummySupplierExists,
        actionOnSelect: 'CheckwhetherDummySupplierExists',
        IsBothActionsSame: true
    });
    supplierRegister();
});
function supplierRegister() {
    AddRequiredIcons();
    LoadIndustryCodes();
    var html = $('#TurnOver').html();
    html = html.replace(/&amp;pound;/g, "&pound;");
    html = html.replace(/&amp;euro;/g, "&euro;");
    $('#TurnOver').html(html);
    var start = 1900;
    var end = new Date().getFullYear();
    var options = "";
    options = "<option value=''>--- Select the year you started ---</option>"
    for (var year = end ; year >= start; year--) {
        options += "<option value=" + year + ">" + year + "</option>";
    }
    //document.getElementById("CompanyYear").innerHTML = options;
    $('#CompanyYear').append(options);
    if (isLoggedInUser) {
        var SellerPartyId = $('#SellerPartyId').val();
        if (SellerPartyId > 0) {
            if (CompanyServiceGeoRegionList != null && typeof CompanyServiceGeoRegionList != 'undefined'
                && CompanySalesGeoRegionList != null && typeof CompanySalesGeoRegionList != 'undefined'
                && CompanyIndustryCodeList != null && typeof CompanyIndustryCodeList != 'undefined') {
                FillGeoRegionsAndIndustryCode(CompanyServiceGeoRegionList, CompanySalesGeoRegionList, CompanyIndustryCodeList);
                FillAddressDivs(showDifferentAddress, showHeadQuarterAddress, showRemittanceAddress);
                $('#CompanyYear').val(establishedYear);
            }
        }
        if (isCompanyDetailsSubmitted) {
            OpenContactDetailsPanel();
        }
        else {
            //if companydetails are incomplete then show company details section
            OpeningCompanyDetailsPanel();
        }
        $('#Email').blur();
        $('#IsAgreeOnTerms').attr('checked', true);
        $('#IsAgreeOnTerms').attr('checked', true);
    }
    else {
    }
    $('#txtSearchSector').keyup(function () {
        $('#imgProcessing').show();
        if (to) { clearTimeout(to); }
        to = setTimeout(function () {           
            collapseAllUnselectedNodes();
            searchSICCode(function () { hideLoader(); });
            //collapseAllUnselectedNodes();            
            //$("#demoTree").jstree("search", $('#txtSearchSector').val());
        }, 500);        
    });
    $('#SicCodeModal').on('hidden.bs.modal', function () {
        $('#txtSearchSector').val('');
        $("#demoTree").jstree("search", $('#txtSearchSector').val());        
        collapseAllUnselectedNodes();
    })
    $('#FirstName').focus();

    //To add asterik for primary contacts
    $('label[for="ContactDetails_0__FirstName"]').addClass('required');

    $('label[for="ContactDetails_0__LastName"]').addClass('required');

    $('label[for="ContactDetails_0__Email"]').addClass('required');

    $('label[for="ContactDetails_0__Telephone"]').addClass('required');

}

function FillGeoRegionsAndIndustryCode(CompanyServiceGeoRegionList, CompanySalesGeoRegionList, CompanyIndustryCodeList) {
    for (var j = 0; j < CompanySalesGeoRegionList.length; j++) {
        for (var i = 0; i < geoGraphicSalesCount; i++) {
            if ($("input[name=\"GeoGraphicSalesList[" + i + "].Value\"]").val() == CompanySalesGeoRegionList[j].Value) {
                $("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]").prop("checked", true);
            }

        }
    }
    for (var j = 0; j < CompanyServiceGeoRegionList.length; j++) {
        for (var i = 0; i < geoGraphicSuppCount; i++) {
            if ($("input[name=\"GeoGraphicSuppList[" + i + "].Value\"]").val() == CompanyServiceGeoRegionList[j].Value) {
                $("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]").prop("checked", true);
            }

        }
    }
    for (var i = 0; i < CompanyIndustryCodeList.length; i++) {
        $('#lblSIC' + (i + 1)).text(CompanyIndustryCodeList[i].Text);
        $('#lnkTree' + (i + 1)).hide();
        $('#lnkSIC' + (i + 1)).show();
        $('#SIC' + (i + 1)).val(CompanyIndustryCodeList[i].Value);
    }
}

function FillAddressDivs(showDifferentAddress, showHeadQuarterAddress, showRemittanceAddress) {
    if (showDifferentAddress) {
        $('input[name = "IsAddressDifferent"][value = "True"]').prop('checked', true);
        $('#ifDifferentAddress').show();
        $('label[for="SecondAddressLine1"]').addClass('required');
        $('label[for="SecondAddressCity"]').addClass('required');
        $('label[for="SecondAddressPostalCode"]').addClass('required');
        $('label[for="SecondAddressCountry"]').addClass('required');
    }
    if (showHeadQuarterAddress) {
        $('input[name = "IsHeadQuartersAddressDifferent"][value = "True"]').prop('checked', true);
        $('#ifHeadQuarterDifferentAddress').show();
        $('label[for="HeadQuartersAddressLine1"]').addClass('required');
        $('label[for="HeadQuartersAddressCity"]').addClass('required');
        $('label[for="HeadQuartersAddressPostalCode"]').addClass('required');
        $('label[for="HeadQuartersAddressCountry"]').addClass('required');
    }
    if (showRemittanceAddress) {
        $('input[name = "IsRemittanceAddressDifferent"][value = "True"]').prop('checked', true);
        $('#ifRemittanceDifferentAddress').show();
        $('label[for="RemittanceAddressLine1"]').addClass('required');
        $('label[for="RemittanceAddressCity"]').addClass('required');
        $('label[for="RemittanceAddressPostalCode"]').addClass('required');
        $('label[for="RemittanceAddressCountry"]').addClass('required');
    }
}

function searchSICCode(callback)
{
    $("#demoTree").jstree("search", $('#txtSearchSector').val());
    callback();
}

function hideLoader() {
    $('#imgProcessing').hide();
}


function panelSelection(divId) {
    $('panel-item-enabled').removeClass('panel-item-enabled');
    $('#userPanel,#companyPanel,#contactPanel,#verificationPanel').addClass('panel-item-disabled').addClass('hidden-xs');
    $('#' + divId).removeClass('panel-item-disabled').addClass('panel-item-enabled').removeClass('hidden-xs');
    var breadCrumb = $('#' + divId).attr('data-breadcrumb');
    $('#registerBreadCrumb').html(breadCrumb);
}

function LoadIndustryCodes() {
    $("#demoTree").jstree({
        json_data: {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": function (node) {
                    var parentId = "";
                    var url = ""
                    if (node == -1) {
                        url = "/Account/GetIndustryCodes";
                    }
                    //else {
                    //    parentId = node.attr('id');
                    //    url = "/Home/GetChildren/?parentId=" + parentId;
                    //}

                    return url;
                },
                "success": function (retval) {
                    return retval;
                }
            }
        },
        checkbox: {
            real_checkboxes: true,
            real_checkboxes_names: function (n) {
                return [("check_" + (n[0].id || Math.ceil(Math.random() * 10000))), n[0].id]
            }
        },
        "themes": { "icons": false },
        "types": {
            "types": {
                "disabled": {
                    "check_node": false,
                    "uncheck_node": false,
                    "open_node": false,
                    "close_node": false,
                    "set_icon": "../../Content/Images/arrow-left.png"
                },
                "enabled": {
                    "check_node": true,
                    "uncheck_node": true,
                    "open_node": true,
                    "close_node": true
                }
            }
        },
        "search": {
            "case_insensitive": true,
            "ajax": false,
            "close_opened_onclear": true,            
            "show_only_matches": true,
        },
        plugins: ["themes", "json_data", "ui", "checkbox", "types", "search","massload"]

    }).bind("change_state.jstree", function (e, d) {
        //// 
        //checked_ids = [];
        //parent_Ids = [];
        ////TODO :: Push selected values into array
        //$("#demoTree").jstree("get_checked", null, true).each
        //    (function () {
        //        // 
        //        checked_ids.push(this.id);
        //    });

        ////TODO :: Remove children whose parent is already selected
        //$("#demoTree").jstree("get_checked", null, true).each
        //   (function () {
        //       var parents_node = $("#" + this.id + "").parents('li');
        //       if (parents_node.length > 0) {
        //           var length = parents_node.length;
        //           for (var i = 0; i < length; i++) {
        //               var parentId = parents_node[i].id;
        //               if (this.id != parentId) {
        //                   var exist = $.inArray(parentId, checked_ids);
        //                   if (exist != -1) {
        //                       checked_ids.splice($.inArray(this.id, checked_ids), 1);
        //                   }
        //               }
        //           }
        //       }
        //   });
    }).bind("check_node.jstree", function (e, d) {
        //
        //var a = this.id;
        var checked_ids = [], parent_Ids = [];
        //TODO :: Push selected values into array
        $("#demoTree").jstree("get_checked", null, true).each
            (function () {
                checked_ids.push(this.id);
            });

        //TODO :: Remove children whose parent is already selected
        $("#demoTree").jstree("get_checked", null, true).each
           (function () {
               var parents_node = $("#" + this.id + "").parents('li');
               if (parents_node.length > 0) {
                   var length = parents_node.length;
                   for (var i = 0; i < length; i++) {
                       var parentId = parents_node[i].id;
                       if (this.id != parentId) {
                           var exist = $.inArray(parentId, checked_ids);
                           if (exist != -1) {
                               checked_ids.splice($.inArray(this.id, checked_ids), 1);
                           }
                       }
                   }
               }
           });

        //Disable selected checkbox
        $.jstree._reference('#demoTree').set_type("disabled", "#" + d.rslt.obj[0].id);

        //Apply disable class
        $($('#' + d.rslt.obj[0].id).children('ins').next().children()[0]).addClass('disabled')


        $('#' + d.rslt.obj[0].id).parents('li').each(function () {
            $.jstree._reference('#demoTree').set_type("disabled", "#" + this.id);
            //Apply disable class
            $($('#' + this.id).children('ins').next().children()[0]).addClass('disabled')
        });

        $('#' + d.rslt.obj[0].id).find('li').each(function () {
            $.jstree._reference('#demoTree').set_type("disabled", "#" + this.id);
            //Apply disable class
            $($('#' + this.id).children('ins').next().children()[0]).addClass('disabled')
        });

        //incase of edit deselect previously selected node
        if ($('#SIC' + $('#hdnCurrentSIC').val()).val() != '') {
            //checking whether user has selected same node or not
            if (d.rslt.obj[0].id != $('#SIC' + $('#hdnCurrentSIC').val()).val()) {
                $("#demoTree").jstree("uncheck_node", '#' + $('#SIC' + $('#hdnCurrentSIC').val()).val());
            }
        }

        $('#lnkSIC' + $('#hdnCurrentSIC').val()).show();
        $('#lnkTree' + $('#hdnCurrentSIC').val()).hide(); 


        //update text in Main page
            $('#lblSIC' + $('#hdnCurrentSIC').val()).text($('#' + d.rslt.obj[0].id).find('a')[0].text);
        $('#SIC' + $('#hdnCurrentSIC').val()).val(d.rslt.obj[0].id);

        $('#SicCodeModal').modal('hide');

    }).bind("uncheck_node.jstree", function (e, d) {
        $.jstree._reference('#demoTree').set_type("enabled", "#" + d.rslt.obj[0].id);
        //Remove disable class
        $($('#' + d.rslt.obj[0].id).children('ins').next().children()[0]).removeClass('disabled');

        var isSiblingSelected = false;
        $('#' + d.rslt.obj[0].id).siblings('li').each(function () {
            if ($("#demoTree").jstree("is_checked", $('#' + this.id))) {
                isSiblingSelected = true;
                return false;
            }
        });

        if (!isSiblingSelected) {
            $('#' + d.rslt.obj[0].id).parents('li').each(function () {
                $.jstree._reference('#demoTree').set_type("enabled", "#" + this.id);
                //Remove disable class
                $($('#' + this.id).children('ins').next().children()[0]).removeClass('disabled');
            });
        }

        $('#' + d.rslt.obj[0].id).find('li').each(function () {
            $.jstree._reference('#demoTree').set_type("enabled", "#" + this.id);
            //Remove disable class
            $($('#' + this.id).children('ins').next().children()[0]).removeClass('disabled');
        });

        //on uncheck of any value show select button, hide edit button & clear value in hidden field for selected sector
        $('#lnkSIC' + $('#hdnCurrentSIC').val()).hide();
        $('#lnkTree' + $('#hdnCurrentSIC').val()).show();
        $('#SIC' + $('#hdnCurrentSIC').val()).val('');
        $('#lblSIC' + $('#hdnCurrentSIC').val()).text('');

    }).bind("clear_search.jstree", function () {
        //$('#imgProcessing').hide();
    });
}

function SelectSicCode(id) {
    $('#CustomerSectorListError').hide();
    DisableLastSelected();
    $('html').addClass('body-no-scroll');
    $('#SicCodeModal').modal({
        backdrop: 'static',
        keyboard: true
    }, 'show');
    $('#hdnCurrentSIC').val(id);
    collapseAllUnselectedNodes();
    return false;
}

function EditSicCode(id) {
    DisableLastSelected();
    $('html').addClass('body-no-scroll');
    $('#SicCodeModal').modal('show');
    $.jstree._reference('#demoTree').set_type("enabled", "#" + $('#SIC' + id).val());
    //Remove disable class
    $($('#' + $('#SIC' + id).val()).children('ins').next().children()[0]).removeClass('disabled');
    $('#hdnCurrentSIC').val(id);
    collapseAllUnselectedNodes();
    return false;
}

$(document).on('hide.bs.modal', '#SicCodeModal', function () {
    $('html').removeClass('body-no-scroll');
});

function DisableLastSelected() {
    if ($('#hdnCurrentSIC').val() != '' && $('#SIC' + $('#hdnCurrentSIC').val()).val() != '') {
        $.jstree._reference('#demoTree').set_type("disabled", "#" + $('#SIC' + $('#hdnCurrentSIC').val()).val());
        //Remove disable class
        $($('#' + $('#SIC' + $('#hdnCurrentSIC').val()).val()).children('ins').next().children()[0]).addClass('disabled');
    }
}
function collapseAllUnselectedNodes() {
    $('#demoTree').find('li').each(function () {
        if ($(this).attr('class') == 'jstree-unchecked jstree-open') {
            $(this).attr('class', 'jstree-unchecked jstree-closed');
        }
    });
}

$('input[name="CompanyFirmStatusVM[0].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        showErrorMessage('Cannot select firm diversity status when Not appicable is selected');
        $('input[name="CompanyFirmStatusVM[0].IsSelected"]').prop('checked', false);
    }
    else {
        if ($('input[name="CompanyFirmStatusVM[0].IsSelected"]').prop('checked'))
            $('#statesDiv-0').show();
        else
            $('#statesDiv-0').hide();
    }
});
$('input[name="CompanyFirmStatusVM[1].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        showErrorMessage('Cannot select firm diversity status when Not appicable is selected');
        $('input[name="CompanyFirmStatusVM[1].IsSelected"]').prop('checked', false);
    }
    else {
        if ($('input[name="CompanyFirmStatusVM[1].IsSelected"]').prop('checked'))
            $('#statesDiv-1').show();
        else
            $('#statesDiv-1').hide();
    }
});
$('input[name="CompanyFirmStatusVM[2].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        showErrorMessage('Cannot select firm diversity status when Not appicable is selected');
        $('input[name="CompanyFirmStatusVM[2].IsSelected"]').prop('checked', false);
    }
    else {
        if ($('input[name="CompanyFirmStatusVM[2].IsSelected"]').prop('checked'))
            $('#statesDiv-2').show();
        else
            $('#statesDiv-2').hide();
    }
});
$('input[name="CompanyFirmStatusVM[3].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        showErrorMessage('Cannot select firm diversity status when Not appicable is selected');
        $('input[name="CompanyFirmStatusVM[3].IsSelected"]').prop('checked', false);
    }
    else {
        if ($('input[name="CompanyFirmStatusVM[3].IsSelected"]').prop('checked'))
            $('#statesDiv-3').show();
        else
            $('#statesDiv-3').hide();
    }
});
$('input[name="CompanyFirmStatusVM[4].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        showErrorMessage('Cannot select firm diversity status when Not appicable is selected');
        $('input[name="CompanyFirmStatusVM[4].IsSelected"]').prop('checked', false);
    }
    else {
        if ($('input[name="CompanyFirmStatusVM[4].IsSelected"]').prop('checked'))
            $('#statesDiv-4').show();
        else
            $('#statesDiv-4').hide();
    }
});
$('input[name="CompanyFirmStatusVM[5].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        showErrorMessage('Cannot select firm diversity status when Not appicable is selected');
        $('input[name="CompanyFirmStatusVM[5].IsSelected"]').prop('checked', false);
    }
    else {
        if ($('input[name="CompanyFirmStatusVM[5].IsSelected"]').prop('checked'))
            $('#statesDiv-5').show();
        else
            $('#statesDiv-5').hide();
    }
});

$('input[name="CompanyFirmStatusVM[6].IsSelected"]').click(function () {
    if ($('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked')) {
        for (var i = 1; i < firmStatusCount; i++) {
            $("input[name=\"CompanyFirmStatusVM[" + i + "].IsSelected\"]").prop("checked", false);
            $('#statesDiv-' + i).hide();
            $("#ddlStates-" + i + "> option").attr("selected", false);
            $('#ddlStates-' + i).multiselect('refresh');
        }
        $('input[name="CompanyFirmStatusVM[6].IsSelected"]').prop('checked', true);
    }

});


var isEmailExists = false;
var isOrganisationExists = false;
var isDummyOrganisationExists = false;
var IsDummyOrganisationSelectedFromSuggest = false;
var IsOrganisationSelectedFromSuggest = false;

$(document).on('click', '#submitUserDetails', function (e) {
    if ($('#signUpForm').valid()) {
        var userInfo = $('#signUpForm').serialize();
        if (isEmailExists || isDummyOrganisationExists) {
            return false;
        }
        if (!$('#IsAgreeOnTerms').prop('checked')) {
            showErrorMessage(readAndAgreeToTermsAndCondition);
            return false;
        }
        var firstName = $('#FirstName').val();
        var lastName = $('#LastName').val();
        var loginId = $('#Email').val();
        var password = $('#Password').val();
        var companyName = $('#DummyOrganisationName').val();
        var IsEmployed = $('#IsEmplyeeOfCompany').prop('checked');

        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');        
        var encryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(password), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
        $('#Password').val(encryptedPassword);
        $('#ConfirmPassword').val(encryptedPassword);

        $.ajax({
            type: 'POST',
            url: '/Account/AddSellerUserDetails',
            data: $('#signUpForm').serialize()  + "&CampaignId=" + parseInt($('#CampaignId').val()) + "&CampaignSupplierId=" + parseInt($('#CampaignSupplierId').val()),
            success: function (response) {
                if (response.result) {
                    $('#SellerPartyId').val(response.SellerPartyId);
                    $('#UserPartyId').val(response.UserPartyId);

                    //if ($('#IsPreRegistered').val() == 'True') {
                    //    SavePreRegCompanyDetails();
                    //}

                    showSuccessMessage(response.message);
                    OpeningCompanyDetailsPanel();
                    $('#loggedIn').show();
                    $('#login-user-name').html(firstName + " " + lastName + "<b class=\"caret\"></b>");
                    $('#loggedOut').hide();
                    $('html,body').animate({
                        scrollTop: 0
                    }, 'fast');
                }
                else {
                    showErrorMessage(response.message);
                }
            }
        });
        return false;
    }
    return false;
});

function OpeningCompanyDetailsPanel() {
    $('.tipso_bubble').remove();
    $('#userDetails').hide();
    $('#CompanyDetails').show();
    panelSelection('companyPanel');
    $('#OrganisationName').removeAttr('disabled');
    $('#ContactDetails_0__FirstName').val($('#FirstName').val());
    $('#ContactDetails_0__LastName').val($('#LastName').val());
    $('#ContactDetails_0__Email').val($('#Email').val());
    $('#OrganisationName').val($('#DummyOrganisationName').val());
    $('#OrganisationName').blur();
     $("#FirmStatus").addClass('required');
}

function OpenContactDetailsPanel() {
    $('.tipso_bubble').remove();
    $('#userDetails').hide();
    //if companydetails are completed then show contact details section
    $('#ContactDetails').show();
    $('#CompanyDetails').hide();

    panelSelection('contactPanel');
}

//function SavePreRegCompanyDetails() {
//    $.ajax({
//        type: 'POST',
//        url: '/Supplier/RegisterDetails',
//        data: $('#signUpForm').serialize(),
//        async: false,
//        success: function (response) {
//            //Do nothing
//        }
//    });
//}

$(document).on('click', '#submitCompanyDetails', function (e) {

    var notSure = $("#rdoBSNotSure").prop('checked');
    if (notSure == true) {
        var businessSector = $("#BusinessSectorDescription").val();
        if (businessSector == "") {
            showErrorMessage(industryErrorText);
            return false;
        }
    }
    if (!formValidation()) {
        return false;
    }
    if (isOrganisationExists) {
        return false;
    }

    $.ajax({
        type: 'POST',
        url: '/Supplier/AddOrUpdateSellerOrganisationDetails',
        data: $('#signUpForm').serialize(),
        async: false,
        success: function (response) {
            if (response.result) {

                $('#ContactDetails').show();
                $('#CompanyDetails').hide();
                panelSelection('contactPanel');
                $('#ContactSellerPartyId').val(response.SellerPartyId);
                ScrollToTop();
                showSuccessMessage(response.message);
            }
        }
    });
    return false;
});


$(document).on('click', '#contactDetailsSubmit', function (e) {
    if (!contactFormValidation()) {
       
        return false;
    }
    var partyId = parseInt($('#ContactSellerPartyId').val());
    if (partyId > 0) {
        $.ajax({
            type: 'post',
            url: '/Supplier/AddSellerContactDetails',
            data: $('#contactForm').serialize(),
            success: function (response) {
                if (response.result) {
                    showSuccessMessage(response.message);
                    window.location.href = response.redirectUrl;
                }
            }
        });
    }
    return false;
});

function contactFormValidation() {
    var result = true;
    var inputDivs = $('.contact-details-form input');
    for (var i = 0; i < $('.contact-details-form input').length; i++)
    {
        var id = $($('.contact-details-form input')[i]).attr("id");
        RemoveBorderColor('#' + id);
    }
    //RemoveBorderColor('#ProcurementFirstName');
    //RemoveBorderColor('#ProcurementLastName');
    //RemoveBorderColor('#ProcurementTelephone');
    //RemoveBorderColor('#ProcurementJobTitle');
    //RemoveBorderColor('#HSFirstName');
    //RemoveBorderColor('#HSLastName');
    //RemoveBorderColor('#HSTelephone');
    //RemoveBorderColor('#HSJobTitle');
    //RemoveBorderColor('#AccountsFirstName');
    //RemoveBorderColor('#AccountsLastName');
    //RemoveBorderColor('#AccountsTelephone');
    //RemoveBorderColor('#AccountsJobTitle');
    //RemoveBorderColor('#SustainabilityFirstName');
    //RemoveBorderColor('#SustainabilityLastName');
    //RemoveBorderColor('#SustainabilityTelephone');
    //RemoveBorderColor('#SustainabilityJobTitle');

    $('.error-text').hide();
    var setFocus = false;
  
    var count = $('.contact-details-form').length;
    for (var i = 0; i < count; i++)
    {
        if( i == 0)
        {
            if ($('#ContactDetails_' + i + '__FirstName').val() == "") {
                SetBorderColor('#ContactDetails_' + i + '__FirstName', "red");
                if (!setFocus) {
                    $('#ContactDetails_' + i + '__FirstName').focus();
                    setFocus = true;
                }
                result = false;
            }
            if ($('#ContactDetails_' + i + '__LastName').val() == "") {
                SetBorderColor('#ContactDetails_' + i + '__LastName', "red");
                if (!setFocus) {
                    $('#ContactDetails_' + i + '__LastName').focus();
                    setFocus = true;
                }
                result = false;

            }
            if ($('#ContactDetails_' + i + '__Email').val() == "") {
                SetBorderColor('#ContactDetails_' + i + '__Email', "red");
                $('#ContactsEmailError-' + i).show();
                if (!setFocus) {
                    $('#ContactDetails_' + i + '__Email').focus();
                    setFocus = true;
                }
                result = false;
            }
            if ($('#ContactDetails_' + i + '__Telephone').val() == "") {
                SetBorderColor('#ContactDetails_' + i + '__Telephone', "red");
                $('#ContactsTelephoneError-' + i).show();
                if (!setFocus) {
                    $('#ContactDetails_' + i + '__Telephone').focus();
                    setFocus = true;
                }
                result = false;
            }
        }
        else
        {
            if ($('#ContactDetails_' + i + '__FirstName').val() != "" || $('#ContactDetails_' + i + '__Email').val() != "" || $('#ContactDetails_' + i + '__Telephone').val() != "" || $('#ContactDetails_' + i + '__LastName').val() != "") {
                if ($('#ContactDetails_' + i + '__FirstName').val() == "") {
                    SetBorderColor('#ContactDetails_' + i + '__FirstName', "red");
                    if (!setFocus) {
                        $('#ContactDetails_' + i + '__FirstName').focus();
                        setFocus = true;
                    }
                    result = false;
                }
                if ($('#ContactDetails_' + i + '__LastName').val() == "") {
                    SetBorderColor('#ContactDetails_' + i + '__LastName', "red");
                    if (!setFocus) {
                        $('#ContactDetails_' + i + '__LastName').focus();
                        setFocus = true;
                    }
                    result = false;

                }
                if ($('#ContactDetails_' + i + '__Email').val() == "") {
                    SetBorderColor('#ContactDetails_' + i + '__Email', "red");
                    $('#ContactsEmailError-'+i).show();
                    if (!setFocus) {
                        $('#ContactDetails_' + i + '__Email').focus();
                        setFocus = true;
                    }
                    result = false;
                }
                if ($('#ContactDetails_' + i + '__Telephone').val() == "") {
                    SetBorderColor('#ContactDetails_' + i + '__Telephone', "red");
                    $('#ContactsTelephoneError-'+i).show();
                    if (!setFocus) {
                        $('#ContactDetails_' + i + '__Telephone').focus();
                        setFocus = true;
                    }
                    result = false;
                }
              
            }
        }
    }
  

    return result;
}



$('.contacts-telephone').keyup(function () {
    var errorId = $(this).attr("data-rowId");
    if ($(this).attr('display') != "none") {
        if ($(this).val() != "") {
            $('#ContactsTelephoneError-' + errorId).hide();
            RemoveBorderColor('#' + $(this).attr("id"));
        }
    }
});


$('.contacts-first-name').keyup(function () {
    if ($(this).val() != "") {
        RemoveBorderColor('#' + $(this).attr("id"));
    }
});
$('.contacts-last-name').keyup(function () {
    if ($(this).val() != "") {
        RemoveBorderColor('#' + $(this).attr("id"));
    }
});
$('.contacts-email').keyup(function () {
    var errorId = $(this).attr("data-rowId");
    if ($('#ContactsEmailError-' + errorId).attr('display') != "none") {
        if ($(this).val() != "") {
            $('#ContactsEmailError-' + errorId).hide();
            RemoveBorderColor('#' + $(this).attr("id"));
        }
    }
});

//function UploadSupplierFiles() {
//    if (typeof FormData == "undefined") {
//        //setCookie("oldBrowser", "true");
//        return false;
//    }
//    else {
//        //  var data = new FormData();
//        //  var files = $("#logo").get(0).files;
//        //// Add the uploaded image content to the form data collection
//        //var isLogo = false;
//        //if (files.length > 0) {
//        //    data.append("Logo", files[0]);
//        //    isLogo = true;
//        //}
//        //data.append("IsLogo", isLogo);
//        //var forms = $("#W9W8Form").get(0).files;
//        //var isForm = false;
//        //if (forms.length > 0) {
//        //    data.append("VATForm", forms[0]);
//        //    isForm = true;
//        //}
//        //data.append("IsForm", isForm);
//        //var ajaxRequest = $.ajax({
//        //    type: "POST",
//        //    url: "/Account/UploadSupplierFiles",
//        //    async: false,
//        //    contentType: false,
//        //    processData: false,
//        //    data: data
//        //});
//        //ajaxRequest.done(function (xhr, textStatus) {
//        //    // Do other operation
//        //});
//        return true;
//    }
//}

$(document).on('click', '#backToUserDetails', function (e) {
    $('#userDetails').show();
    $('#CompanyDetails').hide();
    panelSelection('userPanel');
    $('#DummyOrganisationName').val($('#OrganisationName').val());
    $('#DummyOrganisationName').blur();
    return false;
});

$(document).on('click', '#backToCompanyDetails', function (e) {
    $('#CompanyDetails').show();
    $('#ContactDetails').hide();
    panelSelection('companyPanel');
    $('html,body').animate({
        scrollTop: 0
    }, 'fast');
    return false;
});

function PreviewImage() {
    if (ValidateFileRegister()) {
        if (typeof FileReader != "undefined") {
            var oFReader = new FileReader();
            oFReader.readAsDataURL(document.getElementById("logo").files[0]);
            oFReader.onload = function (oFREvent) {
                document.getElementById("imgLogo_Preview").src = oFREvent.target.result;
            };
            $('#imagePreview').show();
        }
        return false;
    }
    else {
        document.getElementById("imgLogo_Preview").src = "";
        $('#imagePreview').hide();
        $("#logo").val("");
    }
}

var validFilesTypesRegister = ["png", "jpg", "jpeg"];

function ValidateFileRegister() {
    var fileValue = document.getElementById('logo').value;
    if (fileValue.length < 1) {
        showWarningMessage("Please select the image to upload.");
        return false;
    }
    var file = document.getElementById("logo");
    var path = file.value;
    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
    var isValidFile = false;
    for (var i = 0; i < validFilesTypesRegister.length; i++) {
        if (ext == validFilesTypesRegister[i]) {
            isValidFile = true;
            break;
        }
    }
    if (!isValidFile) {
        showWarningMessage("The file extension ." + ext + "  is not allowed!<br/>Allowed file extensions are jpg,jpeg,bmp,png,gif.");
    }
    return isValidFile;
}


var validFormFilesTypes = ["bmp", "gif", "png", "jpg", "jpeg", "doc", "xls", "tif", "eps", "txt", "docx", "rtf", "xlsx", "pdf", "ppt", "pptx"];

//function ValidateForm() {
//    var fileValue = document.getElementById('W9W8Form').value;
//    if (fileValue.length < 1) {
//        showWarningMessage("Please select the Form to upload.");
//        return false;
//    }
//    var file = document.getElementById("W9W8Form");
//    var path = file.value;
//    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
//    var isValidFile = false;
//    for (var i = 0; i < validFormFilesTypes.length; i++) {
//        if (ext == validFormFilesTypes[i]) {
//            isValidFile = true;
//            break;
//        }
//    }
//    if (!isValidFile) {
//        showWarningMessage("The file extension ." + ext + "  is not allowed!<br/>Allowed file extensions are bmp,gif,png,jpg,jpeg,doc,xls,tif,eps,txt,docx,rtf,xlsx,pdf,ppt,pptx.");
//        document.getElementById("W9W8Form").src = "";
//        $("#W9W8Form").val("");
//    }
//    else {
//        $('#W9W8FormError').hide();
//    }
//}

$('input[name="IsSubsidaryStatus"]').click(function () {
    if ($(this).attr("value") == "True") {
        $('label[for="UltimateParent"]').addClass('required');//
        $('#ifSubsidary').show();
    }
    if ($(this).attr("value") == "False") {
        $('#ifSubsidary').hide();
    }
});
$('input[name="HaveDuns"]').click(function () {
    if ($(this).attr("value") == "True") {
        $('#HaveDUNSNumber').show();
        $('label[for="DUNSNumber"]').addClass('required');
    }
    if ($(this).attr("value") == "False") {
        $('#HaveDUNSNumber').hide();
    }
});
$('input[name="IsVAT"]').click(function () {
    if ($(this).attr("value") == "True") {
        $('#HaveVAT').show();
        $('label[for="VATNumber"]').addClass('required');
        //$('label[for="W9W8Form"]').addClass('required');
    }
    if ($(this).attr("value") == "False") {
        $('#HaveVAT').hide();
    }
});
$('.Other-geo-sales').on("click", "input[type='checkbox']", function () {
    for (var i = 0; i < geoGraphicSalesCount; i++) {            
        if ($("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]").prop("checked")) {
            if ($('.No-geo-sales').find("input[type='checkbox']").prop("checked")) {
                $('.No-geo-sales').find("input[type='checkbox']").prop("checked", false)
            }
        }
    }    
    return true;
});
$('.No-geo-sales').click(function () {
    if ($('.No-geo-sales').find("input[type='checkbox']").prop("checked")) {
        for (var i = 0; i < geoGraphicSalesCount; i++) {
            if ($('.Other-geo-sales').find("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]").prop("checked")) {
                $('.Other-geo-sales').find("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]").prop("checked", false)
            }
        }
    }
    return true;
});
$('.Other-geo-supp').on("click", "input[type='checkbox']", function () {
    for (var i = 0; i < geoGraphicSuppCount; i++) {
        if ($("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]").prop("checked")) {
            if ($('.No-geo-supp').find("input[type='checkbox']").prop("checked")) {
                $('.No-geo-supp').find("input[type='checkbox']").prop("checked", false)
            }
        }
    }
    return true;
});
$('.No-geo-supp').click(function () {
    if ($('.No-geo-supp').find("input[type='checkbox']").prop("checked")) {
        for (var i = 0; i < geoGraphicSuppCount; i++) {
            if ($('.Other-geo-supp').find("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]").prop("checked")) {
                $('.Other-geo-supp').find("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]").prop("checked", false)
            }
        }
    }
    return true;
});
$('input[name="IsAddressDifferent"]').click(function () {
    if ($(this).attr("value") == "True") {
        $('#ifDifferentAddress').show();
        $('label[for="SecondAddressLine1"]').addClass('required');
        $('label[for="SecondAddressCity"]').addClass('required');
        $('label[for="SecondAddressPostalCode"]').addClass('required');
        $('label[for="SecondAddressCountry"]').addClass('required');
    }
    if ($(this).attr("value") == "False") {
        $('#ifDifferentAddress').hide();
    }
});
$('input[name="IsHeadQuartersAddressDifferent"]').click(function () {
    if ($(this).attr("value") == "True") {
        $('#ifHeadQuarterDifferentAddress').show();
        $('label[for="HeadQuartersAddressLine1"]').addClass('required');
        $('label[for="HeadQuartersAddressCity"]').addClass('required');
        $('label[for="HeadQuartersAddressPostalCode"]').addClass('required');
        $('label[for="HeadQuartersAddressCountry"]').addClass('required');
    }
    if ($(this).attr("value") == "False") {
        $('#ifHeadQuarterDifferentAddress').hide();
    }
});
$('input[name="IsRemittanceAddressDifferent"]').click(function () {
    if ($(this).attr("value") == "True") {
        $('#ifRemittanceDifferentAddress').show();
        $('label[for="RemittanceAddressLine1"]').addClass('required');
        $('label[for="RemittanceAddressCity"]').addClass('required');
        $('label[for="RemittanceAddressPostalCode"]').addClass('required');
        $('label[for="RemittanceAddressCountry"]').addClass('required');
    }
    if ($(this).attr("value") == "False") {
        $('#ifRemittanceDifferentAddress').hide();
    }
});
function formValidation() {

    var result = true;
    RemoveBorderColor('#UltimateParent');
    RemoveBorderColor('#VATNumber');
    RemoveBorderColor('#DUNSNumber');
    RemoveBorderColor('#SecondAddressLine1');
    RemoveBorderColor('#SecondAddressCity');
    RemoveBorderColor('#SecondAddressState');
    RemoveBorderColor('#SecondAddressPostalCode');
    RemoveBorderColor('#SecondAddressCountry');
    var setFocus = false;
    $('.error-text').hide();
    if (!$('#signUpForm').valid()) {
        result = false;
    }
    if (!result) {
        var els = document.querySelector('.input-validation-error');
        if (els != null) {
            els.focus();
            setFocus = true;
        }
    }
    if ($('input[name=IsSubsidaryStatus]:Checked').val() == "True") {
        if ($('#UltimateParent').val() == "") {
            SetBorderColor("#UltimateParent", "red");
            $('#UltimateParentError').show();
            if (!setFocus)
            {
                $('#UltimateParent').focus();
                setFocus = true;
            }
            result = false;
        }
    }
    if ($('input[name=IsVAT]:Checked').val() == "True") {
        if ($('#VATNumber').val() == "") {
            SetBorderColor("#VATNumber", "red");
            $('#VATNumberError').show();
            if (!setFocus) {
                $('#VATNumber').focus();
                setFocus = true;
            }
            result = false;
        }
        //var fileValue = document.getElementById('W9W8Form').value;
        //if (fileValue.length < 1) {
        //    $('#W9W8FormError').show();
        //    result = false;
        //}
    }
    else if ($('input[name=IsVAT]:Checked').val() == "False") {
        $('#VATNumber').val('');
    }
    if ($('input[name=HaveDuns]:Checked').val() == "True") {
        if ($('#DUNSNumber').val() == "") {
            SetBorderColor("#DUNSNumber", "red");
            $('#DUNSNumberError').show();
            if (!setFocus) {
                $('#DUNSNumber').focus();
                setFocus = true;
            }
            result = false;
        }
    }

    if ($('input[name=IsAddressDifferent]:Checked').val() == "True") {
        if ($('#SecondAddressLine1').val() == "") {
            SetBorderColor("#SecondAddressLine1", "red");
            $('#SecondAddressLine1Error').show();
            if (!setFocus) {
                $('#SecondAddressLine1').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#SecondAddressCity').val() == "") {
            SetBorderColor("#SecondAddressCity", "red");
            $('#SecondAddressCityError').show();
            if (!setFocus) {
                $('#SecondAddressCity').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#SecondAddressPostalCode').val() == "") {
            SetBorderColor("#SecondAddressPostalCode", "red");
            $('#SecondAddressPostalCodeError').show();
            if (!setFocus) {
                $('#SecondAddressPostalCode').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#SecondAddressCountry').val() == "") {
            SetBorderColor("#SecondAddressCountry", "red");
            $('#SecondAddressCountryError').show();
            if (!setFocus) {
                $('#SecondAddressCountry').focus();
                setFocus = true;
            }
            result = false;
        }
    }

    if ($('input[name=IsHeadQuartersAddressDifferent]:Checked').val() == "True") {
        if ($('#HeadQuartersAddressLine1').val() == "") {
            SetBorderColor("#HeadQuartersAddressLine1", "red");
            $('#HeadQuarterAddressLine1Error').show();
            if (!setFocus) {
                $('#HeadQuartersAddressLine1').focus();
                setFocus = true;
            }

            result = false;
        }
        if ($('#HeadQuartersAddressCity').val() == "") {
            SetBorderColor("#HeadQuartersAddressCity", "red");
            $('#HeadQuarterAddressCityError').show();
            if (!setFocus) {
                $('#HeadQuartersAddressCity').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#HeadQuartersAddressPostalCode').val() == "") {
            SetBorderColor("#HeadQuartersAddressPostalCode", "red");
            $('#HeadQuarterAddressPostalCodeError').show();
            if (!setFocus) {
                $('#HeadQuartersAddressPostalCode').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#HeadQuartersAddressCountry').val() == "") {
            SetBorderColor("#HeadQuartersAddressCountry", "red");
            $('#HeadQuarterAddressCountryError').show();
            if (!setFocus) {
                $('#HeadQuartersAddressCountry').focus();
                setFocus = true;
            }
            result = false;
        }
    }

    if ($('input[name=IsRemittanceAddressDifferent]:Checked').val() == "True") {
        if ($('#RemittanceAddressLine1').val() == "") {
            SetBorderColor("#RemittanceAddressLine1", "red");
            $('#RemittanceAddressLine1Error').show();
            if (!setFocus) {
                $('#RemittanceAddressLine1').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#RemittanceAddressCity').val() == "") {
            SetBorderColor("#RemittanceAddressCity", "red");
            $('#RemittanceAddressCityError').show();
            if (!setFocus) {
                $('#RemittanceAddressCity').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#RemittanceAddressPostalCode').val() == "") {
            SetBorderColor("#RemittanceAddressPostalCode", "red");
            $('#RemittanceAddressPostalCodeError').show();
            if (!setFocus) {
                $('#RemittanceAddressPostalCode').focus();
                setFocus = true;
            }
            result = false;
        }
        if ($('#RemittanceAddressCountry').val() == "") {
            SetBorderColor("#RemittanceAddressCountry", "red");
            $('#RemittanceAddressCountryError').show();
            if (!setFocus) {
                $('#RemittanceAddressCountry').focus();
                setFocus = true;
            }
            result = false;
        }
    }

    var SICCode = false;
    for (i = 1; i <= 5; i++) {
        if ($("#SIC" + i).val() != "") {
            SICCode = true;
            break;
        }
    }
    if (!SICCode) {
        $("#CustomerSectorListError").show();
       
        result = false;
    }

    var geoGraphicSale = false;
    for (var i = 0; i < geoGraphicSalesCount; i++) {
        if ($("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]:Checked").val() == "true") {
            geoGraphicSale = true;
            break;
        }
    }
    if (!geoGraphicSale) {
        $("#geoGraphicSaleListError").show();       
        result = false;
    }
    else {
        $("#geoGraphicSaleListError").hide();
    }

    var geoGraphicSupp = false;
    for (var i = 0; i < geoGraphicSuppCount; i++) {
        if ($("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]:Checked").val() == "true") {
            geoGraphicSupp = true;
            break;
        }
    }
    if (!geoGraphicSupp) {
        $("#geoGraphicSuppListError").show();
        result = false;
    }
    else {
        $("#geoGraphicSuppListError").hide();
    }

    
    return result;
}


$('#UltimateParent').blur(function () {
    if ($('#UltimateParentError').attr('display') != "none") {
        if ($('#UltimateParent').val() != "") {
            $('#UltimateParentError').hide();
            RemoveBorderColor('#UltimateParent');
        }
    }
});

$('#VATNumber').blur(function () {
    if ($('#VATNumberError').attr('display') != "none") {
        if ($('#VATNumber').val() != "") {
            $('#VATNumberError').hide();
            RemoveBorderColor('#VATNumber');
        }
    }
});
$('#DUNSNumber').blur(function () {
    if ($('#DUNSNumberError').attr('display') != "none") {
        if ($('#DUNSNumber').val() != "") {
            $('#DUNSNumberError').hide();
            RemoveBorderColor('#DUNSNumber');
        }
    }
});
$('#SecondAddressLine1').blur(function () {
    if ($('#SecondAddressLine1Error').attr('display') != "none") {
        if ($('#SecondAddressLine1').val() != "") {
            $('#SecondAddressLine1Error').hide();
            RemoveBorderColor('#SecondAddressLine1');
        }
    }
});
$('#SecondAddressCity').blur(function () {
    if ($('#SecondAddressCityError').attr('display') != "none") {
        if ($('#SecondAddressCity').val() != "") {
            $('#SecondAddressCityError').hide();
            RemoveBorderColor('#SecondAddressCity');
        }
    }
});

$('#SecondAddressPostalCode').blur(function () {
    if ($('#SecondAddressPostalCodeError').attr('display') != "none") {
        if ($('#SecondAddressPostalCode').val() != "") {
            $('#SecondAddressPostalCodeError').hide();
            RemoveBorderColor('#SecondAddressPostalCode');
        }
    }
});
$('#SecondAddressCountry').change(function () {
    if ($('#SecondAddressCountryError').attr('display') != "none") {
        if ($('#SecondAddressCountry').val() != "") {
            $('#SecondAddressCountryError').hide();
            RemoveBorderColor('#SecondAddressCountry');
        }
    }
});

$('#HeadQuartersAddressLine1').blur(function () {
    if ($('#HeadQuarterAddressLine1Error').attr('display') != "none") {
        if ($('HeadQuartersAddressLine1').val() != "") {
            $('#HeadQuarterAddressLine1Error').hide();
            RemoveBorderColor('#HeadQuartersAddressLine1');
        }
    }
});
$('#HeadQuartersAddressCity').blur(function () {
    if ($('#HeadQuarterAddressCityError').attr('display') != "none") {
        if ($('#HeadQuartersAddressCity').val() != "") {
            $('#HeadQuarterAddressCityError').hide();
            RemoveBorderColor('#HeadQuartersAddressCity');
        }
    }
});

$('#HeadQuartersAddressPostalCode').blur(function () {
    if ($('#HeadQuarterAddressPostalCodeError').attr('display') != "none") {
        if ($('#HeadQuartersAddressPostalCode').val() != "") {
            $('#HeadQuarterAddressPostalCodeError').hide();
            RemoveBorderColor('#HeadQuartersAddressPostalCode');
        }
    }
});
$('#HeadQuartersAddressCountry').change(function () {
    if ($('#HeadQuarterAddressCountryError').attr('display') != "none") {
        if ($('#HeadQuartersAddressCountry').val() != "") {
            $('#HeadQuarterAddressCountryError').hide();
            RemoveBorderColor('#HeadQuartersAddressCountry');
        }
    }
});


$('#RemittanceAddressLine1').blur(function () {
    if ($('#RemittanceAddressLine1Error').attr('display') != "none") {
        if ($('#RemittanceAddressLine1').val() != "") {
            $('#RemittanceAddressLine1Error').hide();
            RemoveBorderColor('#RemittanceAddressLine1');
        }
    }
});
$('#RemittanceAddressCity').blur(function () {
    if ($('#RemittanceAddressCityError').attr('display') != "none") {
        if ($('#RemittanceAddressCity').val() != "") {
            $('#RemittanceAddressCityError').hide();
            RemoveBorderColor('#RemittanceAddressCity');
        }
    }
});

$('#RemittanceAddressPostalCode').blur(function () {
    if ($('#RemittanceAddressPostalCodeError').attr('display') != "none") {
        if ($('#RemittanceAddressPostalCode').val() != "") {
            $('#RemittanceAddressPostalCodeError').hide();
            RemoveBorderColor('#RemittanceAddressPostalCode');
        }
    }
});
$('#RemittanceAddressCountry').change(function () {
    if ($('#RemittanceAddressCountryError').attr('display') != "none") {
        if ($('#RemittanceAddressCountry').val() != "") {
            $('#RemittanceAddressCountryError').hide();
            RemoveBorderColor('#RemittanceAddressCountry');
        }
    }
});

function ValidateEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if (email == "" || !emailReg.test(email)) {
        $('#EmailError').removeClass('available');
        $('#EmailError').removeClass('error-text');
        $('#EmailErrortext').html('');
        return false;
    } else {
        return true;
    }
}

$('#Email').blur(function () {
    Common.IsLoadingNeeded = false;
    var email = $('#Email').val();

    if (ValidateEmail(email)) {
        $('#EmailValidationLoading').show();
        $.ajax({
            type: 'post',
            url: '/Account/IsEmailExists',
            data: { email: email },
            async: true,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.result) {
                        isEmailExists = true;
                        $('#EmailError').removeClass('available');
                        $('#EmailError').addClass('error-text');
                        $('#EmailErrortext').html(response.message);
                        $('#Email').focus();
                    }
                    else {
                        isEmailExists = false;
                        $('#EmailError').removeClass('error-text');
                        $('#EmailError').addClass('available');
                        $('#EmailErrortext').html('');
                    }
                    $('#EmailValidationLoading').hide();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    return false;
})
$('#Email').keyup(function () {
    $('#EmailError').removeClass('available');
    $('#EmailError').removeClass('error-text');
    $('#EmailErrortext').html('');
});
$('#OrganisationName').keyup(function () {
    IsOrganisationSelectedFromSuggest = false;

    $('#OrganisationError').removeClass('available');
    $('#OrganisationError').removeClass('error-text');
    $('#OrganisationErrorText').html('');
});


//$('#DummyOrganisationName').keyup(function () {
//    $('#CampaignId').val("0");
//    $('#CampaignSupplierId').val("0");
//    $('#IsFITMappedToBuyer').val("0");
//    $('#IsHSMappedToBuyer').val("0");
//    $('#IsDSMappedToBuyer').val("0");        
//    $('#FirstAddressLine1').val("");
//    $('#FirstAddressLine2').val("");
//    $('#FirstAddressCity').val("");
//    $('#FirstAddressState').val("");
//    $('#FirstAddressCountry').val("");
//    $('#FirstAddressPostalCode').val("");
//    $('#CompanyRegistrationNumber').val("");
//    $('#VATNumber').val("");
//    $("input[name=IsVAT][value='False']").prop("checked", true);
//    $('#HaveVAT').hide();
//    $('#DUNSNumber').val("");    
//    $("input[name=HaveDuns][value='False']").prop("checked", true);
//    $('#HaveDUNSNumber').hide();
    

//    IsDummyOrganisationSelectedFromSuggest = false;
//    $('#DummyOrganisationError').removeClass('available');
//    $('#DummyOrganisationError').removeClass('error-text');
//    $('#DummyOrganisationErrorText').html('');
//});

//$('#Password').blur(function () {
//    var element = $("span[data-valmsg-for='Password']");
//    if (element.hasClass('field-validation-error')) {
//        var msg = element.find("span[for='Password']").html();
//        if (msg = "Invalid Password Format") {
//            showErrorMessage("Password should contain at least one upper case letter, one lower case letter, one number, one special character (e.g. @#$*%_!&) and length must be within 6-15 characters,Should not start or end with spaces");
//        }
//    }

//});

$('#Password').focus(function () {
    $('#passwordTipso').tipso({
        position: 'top',
        width: 300,
        offsetX: 215
    });
    $('#passwordTipso').tipso('update', 'content', passwordTipsoText);
    $('#passwordTipso').tipso('show');
});

$('#Password').focusout(function () {
    $('#passwordTipso').tipso('hide');
});

$('#ConfirmPassword').focus(function () {
    $('#cnfrmPasswordTipso').tipso({
        position: 'top',
        width: 300,
        offsetX: 215
    });
    $('#cnfrmPasswordTipso').tipso('update', 'content', passwordTipsoText);
    $('#cnfrmPasswordTipso').tipso('show');
});

$('#ConfirmPassword').focusout(function () {
    $('#cnfrmPasswordTipso').tipso('hide');
});

$(document).on('hide.bs.modal', '#modalTC', function () {
    $('html').removeClass('body-no-scroll');
});

$('.GeoGraphicSuppListValidate input').click(function () {
    $("#geoGraphicSuppListError").hide();
    return true;
});

$('.GeoGraphicSalesListValidate input').click(function () {
    $("#geoGraphicSaleListError").hide();
    return true;
});

function CheckwhetherDummySupplierExists(key)
{
    Common.IsLoadingNeeded = false;
    var companyName = $('#DummyOrganisationNameUlIntellisense #DummyOrganisationName-' + key).html();

    if(companyName == undefined)
    {
        companyName = $('#DummyOrganisationName').val();
    }
    IsDummyOrganisationSelectedFromSuggest = true;
    $('#DummyOrganisationValidationLoading').show();
    $.ajax({
        type: 'post',
        url: '/Account/CheckwhetherSupplierNameExists',
        data: { organisationName: companyName },
        async: true,
        success: function (response) {
            if (typeof (response) != "undefined") {
                if (response.publicDataId > 0) {
                    GetPublicDataRecord(response.publicDataId);
                }
                if (response.IsAlreadyRegistered) {
                    isDummyOrganisationExists = true;
                    $('#DummyOrganisationError').removeClass('available');
                    $('#DummyOrganisationError').addClass('error-text');
                    $('#DummyOrganisationErrorText').html(response.message);
                    $('#DummyOrganisationName').focus();
                }
                else if (response.IsNotRegistered || response.NoRecord) {
                    isDummyOrganisationExists = false;
                    $('#DummyOrganisationError').removeClass('error-text');
                    $('#DummyOrganisationError').addClass('available');
                    $('#DummyOrganisationErrorText').html('');
                }
                $('#DummyOrganisationValidationLoading').hide();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
}

$('#DummyOrganisationName').blur(function () {
    if (IsDummyOrganisationSelectedFromSuggest)
    {
        return false;
    }
    Common.IsLoadingNeeded = false;
    var organisationName = $('#DummyOrganisationName').val();
    if (organisationName == "") {
        $('#DummyOrganisationError').removeClass('available');
        $('#DummyOrganisationError').removeClass('error-text');
        $('#DummyOrganisationErrorText').html('');
        return false;
    }
    $('#DummyOrganisationValidationLoading').show();

    $.ajax({
        type: 'post',
        url: '/Account/CheckwhetherSupplierNameExists',
        data: { organisationName: organisationName },
        async: true,
        success: function (response) {
            if (typeof (response) != "undefined") {
                if (response.publicDataId > 0) {
                    GetPublicDataRecord(response.publicDataId);
                }
                if (response.result) {
                    isDummyOrganisationExists = true;
                    $('#DummyOrganisationError').removeClass('available');
                    $('#DummyOrganisationError').addClass('error-text');
                    $('#DummyOrganisationErrorText').html(response.message);
                    $('#DummyOrganisationName').focus();
                }
                else  {
                    isDummyOrganisationExists = false;
                    $('#DummyOrganisationError').removeClass('error-text');
                    $('#DummyOrganisationError').addClass('available');
                    $('#DummyOrganisationErrorText').html('');
                }
                $('#DummyOrganisationValidationLoading').hide();
                $("#DummyOrganisationNameDivIntellisense").remove();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

    return false;
})
//function CheckwhetherSupplierExists(key) {
//    Common.IsLoadingNeeded = false;
//    var companyName = $('#OrganisationNameUlIntellisense #OrganisationName-' + key).html();

//    if (companyName == undefined) {
//        companyName = $('#OrganisationName').val();
//    }
//    IsOrganisationSelectedFromSuggest = true;
//    var oldOrganisationName = $('#DummyOrganisationName').val();
//    if (companyName == "") {
//        $('#OrganisationError').removeClass('available');
//        $('#OrganisationError').removeClass('error-text');
//        $('#OrganisationErrorText').html('');
//        return false;
//    }
//    if (oldOrganisationName != companyName) {
//        $('#OrganisationValidationLoading').show();
//        $.ajax({
//            type: 'post',
//            url: '/Account/IsOrganisationExists',
//            data: { organisationName: companyName },
//            async: true,
//            success: function (response) {
//                if (typeof (response) != "undefined") {
//                    if (response.publicDataId > 0) {
//                        GetPublicDataRecord(response.publicDataId);
//                    }
//                    if (response.IsAlreadyRegistered) {
//                        isOrganisationExists = true;
//                        $('#OrganisationError').removeClass('available');
//                        $('#OrganisationError').addClass('error-text');
//                        $('#OrganisationErrorText').html(response.message);
//                        $('#OrganisationName').focus();
//                    }
//                    else if (response.IsNotRegistered || response.NoRecord) {
//                        isOrganisationExists = false;
//                        $('#OrganisationError').removeClass('error-text');
//                        $('#OrganisationError').addClass('available');
//                        $('#OrganisationErrorText').html('');
//                    }
//                    $('#OrganisationValidationLoading').hide();

//                }
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//            }
//        });
//    }
//    else {
//        isOrganisationExists = false;
//        $('#OrganisationError').removeClass('error-text');
//        $('#OrganisationError').addClass('available');
//        $('#OrganisationErrorText').html('');
//    }
//    return false;
//}

function GetPublicDataRecord(publicDataId) {
    $.ajax({
        type: 'post',
        url: '/Account/GetPublicDataRecord',
        data: { publicDataId: publicDataId },
        async: true,
        success: function (response) {
            if (typeof (response) != "undefined") {
                if (response.publicDataRecord != null) {
                    $('#CampaignId').val(response.publicDataRecord.CampaignId);
                    $('#CampaignSupplierId').val(response.publicDataRecord.PreRegSupplierId);
                    //$('#IsFITMappedToBuyer').val(response.publicDataRecord.IsFITMappedToBuyer);
                    //$('#IsHSMappedToBuyer').val(response.publicDataRecord.IsHSMappedToBuyer);
                    //$('#IsDSMappedToBuyer').val(response.publicDataRecord.IsDSMappedToBuyer);

                    $('#OrganisationName').val(response.publicDataRecord.SupplierName);
                    //$('#Email').val(response.publicDataRecord.LoginId);
                    //$('#FirstName').val(response.publicDataRecord.FirstName);
                    //$('#LastName').val(response.publicDataRecord.LastName);
                    //$('#JobTitle').val(response.publicDataRecord.JobTitle);
                    $('#FirstAddressLine1').val(response.publicDataRecord.AddressLine1);
                    $('#FirstAddressLine2').val(response.publicDataRecord.AddressLine2);
                    $('#FirstAddressCity').val(response.publicDataRecord.City);
                    $('#FirstAddressState').val(response.publicDataRecord.State);
                    $('#FirstAddressCountry').val(response.publicDataRecord.Country.toString());
                    $('#FirstAddressPostalCode').val(response.publicDataRecord.ZipCode);
                    $('#CompanyRegistrationNumber').val(response.publicDataRecord.RegistrationNumber);
                    $('#VATNumber').val(response.publicDataRecord.VatNumber);
                    if ($('#VATNumber').val() != null && $('#VATNumber').val() != "") {
                        $("input[name=IsVAT][value='True']").prop("checked", true);
                        //$('#IsVAT').val(true);
                        $('#HaveVAT').show();
                    }
                    else {
                        $("input[name=IsVAT][value='False']").prop("checked", true);
                        //$('#IsVAT').val(false);
                        $('#HaveVAT').hide();
                    }
                    $('#DUNSNumber').val(response.publicDataRecord.DunsNumber);
                    if ($('#DUNSNumber').val() != null && $('#DUNSNumber').val() != "") {
                        $("input[name=HaveDuns][value='True']").prop("checked", true);
                        $('#HaveDUNSNumber').show();
                    }
                    else {
                        $("input[name=HaveDuns][value='False']").prop("checked", true);
                        $('#HaveDUNSNumber').hide();
                    }
                    $('#DummyOrganisationName').val(response.publicDataRecord.SupplierName);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

$('#OrganisationName').blur(function () {
    Common.IsLoadingNeeded = false;

    if (IsOrganisationSelectedFromSuggest) {
        return false;
    }

    var oldOrganisationName = $('#DummyOrganisationName').val();
    var organisationName = $('#OrganisationName').val();
    if (organisationName == "") {
        $('#OrganisationError').removeClass('available');
        $('#OrganisationError').removeClass('error-text');
        $('#OrganisationErrorText').html('');
        return false;
    }
    if (oldOrganisationName != organisationName) {
        $('#OrganisationValidationLoading').show();
        $.ajax({
            type: 'post',
            url: '/Account/IsOrganisationExists',
            data: { organisationName: organisationName },
            async: true,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.result) {
                        isOrganisationExists = true;
                        $('#OrganisationError').removeClass('available');
                        $('#OrganisationError').addClass('error-text');
                        $('#OrganisationErrorText').html(response.message);
                        $('#OrganisationName').focus();

                    }
                    else {
                        isOrganisationExists = false;
                        $('#OrganisationError').removeClass('error-text');
                        $('#OrganisationError').addClass('available');
                        $('#OrganisationErrorText').html('');
                    }
                    $('#OrganisationValidationLoading').hide();
                    $("#OrganisationNameDivIntellisense").remove();

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    else {
        isOrganisationExists = false;
        $('#OrganisationError').removeClass('error-text');
        $('#OrganisationError').addClass('available');
        $('#OrganisationErrorText').html('');
    }
    return false;
})
