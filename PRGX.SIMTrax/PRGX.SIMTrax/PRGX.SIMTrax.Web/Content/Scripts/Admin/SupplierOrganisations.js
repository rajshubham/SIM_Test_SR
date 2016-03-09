$('#ActionDropdown .dropdown-menu').on({
    "click": function (e) {
        e.stopPropagation();
    }
});

$(document).ready(function () {
    CheckPermission("Suppliers", function () {
        SelectMenu("AdminSupportTab");
        verifyAndAuditSuppliers();
    });
    $('input[placeholder]').simplePlaceholder();
});

$(".tooltip-remove").hover(
 function () {
     var desc = $(this).attr("data-description");
     $(this).tipso('update', 'content', desc);
     $(this).tipso('update', 'background', 'rgb(226, 226, 226)');
     $(this).tipso('update', 'color', 'rgb(0, 0, 0)');
     $(this).tipso('show');
 }, function () {
     $(this).tipso('hide');
     $(this).tipso('remove');
     $(this).tipso('destroy');
 }
);

function verifyAndAuditSuppliers() {
    if (localStorage.SupplierNameFromBuyerHomeSearch != undefined && localStorage.SupplierNameFromBuyerHomeSearch != "") {
        $("#txtSupplierVSS").val(localStorage.SupplierNameFromBuyerHomeSearch);
        localStorage.removeItem("SupplierNameFromBuyerHomeSearch");
    }

    $("#txtToDate").datepicker({
        changeYear: true,
        changeMonth: true,
        constrainInput: false,
        //minDate: "+0",
        maxDate: "+0",
        defaultDate: "+0",
        dateFormat: "dd-M-yy"
    }).css({ "cursor": "default" }).keydown(function () {
    });

    $("#txtFromDate").datepicker({
        changeYear: true,
        changeMonth: true,
        constrainInput: false,
        maxDate: "+0",
        defaultDate: "+0",
        dateFormat: "dd-M-yy"
    }).css({ "cursor": "default" }).keydown(function () {
    });

    if (localStorage.tempVal == "fromSupplierPage") {
        $('#txtFromDate').val(localStorage.FromDate);
        $('#txtToDate').val(localStorage.ToDate);
        $('#ddlSupplierStatus').val(localStorage.SupplierStatus);
        $('#txtSupplierVSS').val(localStorage.SupplierName);
    }
    else {
        $('#ddlSupplierStatus').val("503");
    }

    localStorage.tempVal = "";
    $(".fa-sort-asc").addClass('fa-sort').removeClass('fa-sort-asc');
    $(".fa-sort-desc").addClass('fa-sort').removeClass('fa-sort-desc');
    $(".supplierNameSort").attr('data-sortdirection', 3);
    SupplierStatusList(1, "", 3);
}

$('#pageSizeSupplierSearch').change(function () {
    $('#hdn-supplier-status-current-page').val(1);
    var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    PagerLinkClick(1, "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
});

function SupplierStatusList(index, sortParameter, sortDirection) {
    var startdate = new Date($('#txtFromDate').val());
    var endDate = new Date($('#txtToDate').val());
    if (startdate != "Invalid Date") {
        localStorage.FromDate = $('#txtFromDate').val();
    }
    else {
        localStorage.FromDate = "";
    }
    if (endDate != "Invalid Date") {
        localStorage.ToDate = $('#txtToDate').val();
    }
    else {
        localStorage.ToDate = "";
    }
    localStorage.SupplierStatus = $('#ddlSupplierStatus').val();
    localStorage.SupplierName = $('#txtSupplierVSS').val();
    var pageSizeSupplierSearch = parseInt($('#pageSizeSupplierSearch').val());
    var supplierName = ($("#txtSupplierVSS").val() != $('input[id=txtSupplierVSS]').attr('placeholder')) ? $("#txtSupplierVSS").val() : "";
    var supplierId = ($("#txtSupplierIdVSS").val() != $('input[id=txtSupplierIdVSS]').attr('placeholder')) ? $("#txtSupplierIdVSS").val() : "";
    var referrerName = ($('#txtReferrerName').val() != $('input[id=txtReferrerName]').attr('placeholder')) ? $('#txtReferrerName').val() : "";
    if (($('#txtToDate').val() != "") && startdate > endDate) {
        showErrorMessage(dateValidation);
        return false;
    }
    var obj = { status: parseInt($('#ddlSupplierStatus').val()), fromDate: $('#txtFromDate').val(), toDate: $('#txtToDate').val(), index: index, supplierName: supplierName, source: parseInt($('#ddlSource').val()), supplierId: supplierId, referrerName: referrerName, count: pageSizeSupplierSearch, sortDirection: sortDirection }
    $.ajax({
        cache: false,
        type: 'post',
        url: '/Admin/GetSupplierOrganisation',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var tableRow = "";
            $('#tblSuppliers').find("tr:gt(0)").remove();
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var row = "";

                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    var supplierReferrer = "";
                    if (data.result[i].SupplierReferrers.length > 0) {
                        if (data.result[i].SupplierReferrers.length == 1) {
                            supplierReferrer = data.result[i].SupplierReferrers[0].BuyerOrganizationName;
                        }
                        else {
                            supplierReferrer = data.result[i].SupplierReferrers[0].BuyerOrganizationName;
                            for (j = 0; j < data.result[i].SupplierReferrers.length; j++) {
                                if (data.result[i].SupplierReferrers[j].LandingReferrer == true) {
                                    supplierReferrer = data.result[i].SupplierReferrers[j].BuyerOrganizationName;
                                }
                            }
                            supplierReferrer += "(" + data.result[i].SupplierReferrers.length + ")";
                        }
                    }
                    else {
                        supplierReferrer = "";
                    }

                    var action = "";
                    if (CanManageReferrer || CanEditUser) {
                        action += "<li><a class=\"btn-edit-referrer\" data-supplier-id=\"" + data.result[i].SupplierId + "\">" + addOrEditReferrerButton + "</a></li>" +
                            "<li><a class=\"btnEditSupplierPrimaryContactDetails\"  data-UserId-Profile=\"" + data.result[i].SupplierUserId + "\">" + editProfileButton + "</a></li>";
                    }
                    if (data.result[i].SupplierStatus == SubmittedStatus) {
                        //if (CanDetailsCheck || CanProfileCheck) {
                        //    action += "<li><a class=\"detailCheck\"  data-companyId=\"" + data.result[i].SupplierId + "\">" + detailsCheckButton + "</a></li><li><a class=\"profileCheck\"  data-companyId=\"" + data.result[i].SupplierId + "\">" + profileCheckButton + "</a></li>";
                        //}
                    }
                    else if (data.result[i].SupplierStatus == RegisteredStatus) {
                        //if (CanProfileCheck) {
                        //    action += "<li><a class=\"profileCheck\"   data-companyId=\"" + data.result[i].SupplierId + "\">" + profileCheckButton + "</a></li>";
                        //}
                    }
                    if (action != "") {
                        row = "<div class=\"btn-group pull-right\" id=\"ActionDropdown\"> <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionButton + "<span class=\"caret\"></span></button> <ul class=\"dropdown-menu\" >" + action + "</ul></div>";
                    }
                    else {
                        row = "-";
                    }
                    var companyName = "";
                    if (data.result[i].SupplierOrganizationName.length > 26) {
                        companyName = data.result[i].SupplierOrganizationName.substring(0, 25) + "...";
                    }
                    else {
                        companyName = data.result[i].SupplierOrganizationName;
                    }

                    tableRow = "<tr class=" + trClass + "><td><a class='hyperlink fntColorSimTraxGreen' onclick='ShowSupplierCompanyDetails(" + data.result[i].SupplierId + ",\"" + data.result[i].SupplierOrganizationName + "\")'>" + companyName
                  + "</a></td><td class=\"text-align-right\"><span class='supplierId'>" + data.result[i].SupplierId
                  + "</span></td><td class=\"text-align-right\">" + data.result[i].SignUpDateString
                  + "</td><td class=\"text-align-right\">" + data.result[i].RegisteredDateString
                  + "</td><td class=\"text-align-right\">" + data.result[i].DetailsVerifiedDateString
                  + "</td><td class=\"text-align-right\">" + data.result[i].ProfileVerifiedDateString
                  + "</td><td class='thStatusSup'>" + data.result[i].SupplierStatusString
                  + "</td><td class=\"text-align-left\">" + data.result[i].ProjectSource
                  + "</td><td class=\"text-align-left\">" + supplierReferrer
                  + "</td><td  class=\"text-align-center\">" + row + "</td></tr>";
                    $('#spnSupCount-supplierstatus').html(data.total);
                    $('#tblSuppliersBody').append(tableRow);

                    $('#tblSuppliers').footable();
                    $('#tblSuppliers').trigger('footable_redraw');
                    if (!CanManageReferrer) {
                        $('.btn-edit-referrer').hide();
                    }
                    if (!CanEditUser) {
                        $('.btnEditSupplierPrimaryContactDetails').hide();
                    }
                    if (!CanDetailsCheck) {
                        $('.detailCheck').hide();
                    }
                    if (!CanProfileCheck) {
                        $('.profileCheck').hide();
                    }
                    if (parseInt($('#ddlSupplierStatus').val()) != 503) {
                        $('.thStatusSup').hide();
                    }
                    else {
                        $('.thStatusSup').show();
                    }
                }
            }
            else {
                $('#tblSuppliersBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
                $('#spnSupCount-supplierstatus').html('0');
                $('#paginator').remove();
            }
            $('.supplierStatusPaginator').html(displayLinks($('#hdn-supplier-status-current-page').val(), Math.ceil(data.total / pageSizeSupplierSearch), sortParameter, sortDirection, "SupplierStatusList", "#hdn-supplier-status-current-page"));
            if (data.total <= pageSizeSupplierSearch) {
                $('.supplierStatusPaginator').css('margin-right', '0px');
            }
            var contentHtml = "";
            var currentPage = parseInt($('#hdn-supplier-status-current-page').val());
            var lastPage = Math.ceil(data.total / pageSizeSupplierSearch);
            if (data.result.length > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeSupplierSearch) + 1), (pageSizeSupplierSearch * currentPage), data.total);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeSupplierSearch) + 1), data.total, data.total);
                }
            }
            $('#search-page-data-supplierstatus').html(contentHtml);
            $('html, body').animate({
                scrollTop: 0
            }, 'slow');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Supplier info.');
        }
    });
}

$(document).on('click', '.buyerdashboard', function () {
    var partyId = parseInt($(this).attr("value"));
    ShowBuyerDashBoard(partyId);
    return false;
});

function ShowSupplierDashBoard(partyId) {
    if (partyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/GetSupplierDashboard',
            data: { supplierPartyId: partyId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDivs();
                    $("#SupplierDashboard").empty();
                    $("#SupplierDashboard").html(response);
                    $("#SupplierDashboard").show();
                }
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    return false;
}

function ClearAllDivs() {
    $("#verifyAndAuditSuppliers").html("");
    $("#verifyAndAuditSuppliers").hide();
    return false;
}

function ShowSupplierCompanyDetails(partyId, partyName) {
    //companyIdForProfileDetails = companyId;
    //ProfileCompanyNameFromSearch = CompanyName;
    //window.location.href = "/Summary/" + companyId;
    ShowSupplierDashBoard(partyId);
}

$(document).on('click', '.btn-edit-referrer', function () {
    var supplierId = $(this).attr('data-supplier-id');
    //var splitData = data.split(" ");
    $('#hdnSupplierId').attr("value", supplierId);
    $('#SearchReferrer')[0].reset();
    $("#referrerTable-content").html('');
    $("#hdn-supplier-referrer-details-page").val(1);
    SearchReferrerData(1, "", 1);
});

//function AssignSupplierReferrer(supplierId, assignReferrer) {
//    var campaignId = [];

//    campaignId = assignReferrer
//    $.ajax({
//        async: false,
//        type: 'post',
//        url: '/Admin/UpdateSupplierStatusValues',
//        data: JSON.stringify({ 'supplierId': supplierId, 'campaignId': campaignId }),
//        contentType: 'application/json; charset=utf-8',
//        success: function (response) {
//            if (response) {
//                showSuccessMessage(response.message);
//                $('#AddReferrer').modal('hide');
//                var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
//                var sortParameter = "";
//                if (sortDirection != 3) {
//                    sortParameter = "CompanyName";
//                }
//                PagerLinkClick('1', "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
//            }
//            else
//                showErrorMessage(response.message);
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//};

//function RemoveSupplierReferrer(supplierId, removeReferrer) {
//    var campaignId = [];
//    campaignId = removeReferrer;
//    $.ajax({
//        async: false,
//        type: 'post',
//        url: '/Admin/DeleteReferrer',
//        data: JSON.stringify({ 'supplierId': supplierId, 'campaignId': campaignId }),
//        contentType: 'application/json; charset=utf-8',

//        success: function (response) {
//            if (response) {
//                showSuccessMessage(response.message);
//                $('#AddReferrer').modal('hide');
//                var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
//                var sortParameter = "";
//                if (sortDirection != 3) {
//                    sortParameter = "CompanyName";
//                }
//                PagerLinkClick('1', "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
//            }
//            else
//                showErrorMessage(response.message);
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//};

$(document).ready(function () {
    $("txtSupplierVSS").intellisense({
        url: '/Common/GetSuppliersForIntellisense',
        actionOnEnter: SearchOnEnter
    });
});

$(function () {
    $("#txtSupplierIdVSS").keypress(function (e) {
        if (e.which == 13) {
            SearchOnEnter();
        }
    });
});

$(function () {
    $("#txtReferrerName").keypress(function (e) {
        if (e.which == 13) {
            SearchOnEnter();
        }
    });
});

function SearchOnEnter() {
    $('#hdn-supplier-status-current-page').val(1);
    var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    PagerLinkClick(1, "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
    $('#txtSupplierVSSDivIntellisense').hide();
}

function isNumberKey(e) {
    var charCode = (e.which) ? e.which : e.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


function keyPress(e) {
    var charCode = (e.which) ? e.which : e.keyCode
    if (charCode > 31)
        return false;
    return true;
}

$(document).on('click', '#btnSearchReferrers', function () {
    SearchReferrerData(1, "", "");
    return false;
});

function SearchReferrerData(pageNo, sortParameter, sortDirection) {
    var selectionCount = 0;
    var primaryreferrer = 0;
    $('.add-referrer').each(function () {
        if ($(this).is(':checked')) {
            selectionCount++;
        }
    });
    $('.remove-refferer').each(function () {
        if (!$(this).is(':checked')) {
            selectionCount++;
        }
    });
    $('.assign-primary-referrer').each(function () {
        if ($(this).is(':checked')) {
            primaryreferrer++;
        }
    });
    if (primaryreferrer > 1) {
        selectionCount++;
    }
    if (selectionCount > 0) {
        showErrorMessage(referrerNavigateValidation);
        return false;
    }
    $('#AddReferrer').modal('show');
    var buyerName = $('#tbBuyerName').val();
    var campaignName = $('#tbCampaignName').val();

    var suppId = $('#hdnSupplierId').val();
    $.ajax({
        type: 'post',
        url: '/Admin/GetSupplierReferrerBuyerCampaignDetails',
        data: { pageNo: pageNo, buyerName: buyerName, campaignName: campaignName, supplierId: suppId },
        dataType: "json",
        success: function (response) {
            var row = "";
            if (response.referrers.length > 0) {
                for (i = 0; i < response.referrers.length; i++) {
                    var trCls = "odd";
                    if (i % 2 == 0) {
                        trCls = "even";
                    }
                    if (response.referrers[i].IsSupplierReferred != true) {
                        row += "<tr class=\"" + trCls + "\"><td>" + response.referrers[i].BuyerOrganizationName + "</td><td>" + response.referrers[i].CampaignName + "</td><td style=\"text-align: center;\"><input class=\"btn btn-color add-referrer\"  type=\"checkbox\" data-campaign-id=\"" + response.referrers[i].CampaignId + "\"></td>";
                    }
                    else {
                        row += "<tr class=\"" + trCls + "\"><td>" + response.referrers[i].BuyerOrganizationName + "</td><td>" + response.referrers[i].CampaignName + "</td><td style=\"text-align: center;\"><input class=\"btn btn-color remove-refferer\" data-remove-campaign-id=\"" + response.referrers[i].CampaignId + "\"  type=\"checkbox\" checked=\"checked\" ></td>";
                    }
                    if (response.referrers[i].LandingReferrer == true) {
                        row += "<td style=\"text-align: center;\"><input class=\"chkBox assign-primary-referrer\" type=\"radio\" name=\"radioclick\" data-primary-campaign-id=\"" + response.referrers[i].CampaignId + "\" checked=\"checked\" /></td></tr>";
                    }
                    else {
                        row += "<td style=\"text-align: center;\"><input class=\"chkBox assign-primary-referrer\" type=\"radio\" name=\"radioclick\" data-primary-campaign-id=\"" + response.referrers[i].CampaignId + "\" /></td></tr>";
                    }
                }
                $("#referrerTable-content").html(row);
            }
            else {
                $('#referrerTable-content').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
            }
            $('.addReferrerPaginator').html(displayLinks($('#hdn-supplier-referrer-details-page').val(), Math.ceil(response.total / 5), sortParameter, sortDirection, "SearchReferrerData", "#hdn-supplier-referrer-details-page"));
            if (response.total <= 5) {
                $('.addReferrerPaginator').css('margin-right', '0px');
            }
        },
        error: function (result) {
        }
    });
    return false;
}

//function AssignPrimaryReferrer(supplierId, primaryReferrer) {

//    $.ajax({
//        async: false,
//        type: 'post',
//        url: '/Admin/AddLandingPageReferrer',
//        data: JSON.stringify({ 'supplierId': supplierId, 'campaignId': primaryReferrer }),
//        contentType: 'application/json; charset=utf-8',

//        success: function (response) {
//            if (response) {
//                showSuccessMessage(response.message);
//                $('#AddReferrer').modal('hide');
//                var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
//                var sortParameter = "";
//                if (sortDirection != 3) {
//                    sortParameter = "CompanyName";
//                }
//                PagerLinkClick('1', "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
//            }
//            else
//                showErrorMessage(response.message);
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//};

$(document).on('click', '#search-suppliers-filter-header-clear', function () {
    $('#search-suppliers-filter-header-clear').hide();
    $('#txtSupplierIdVSS').val("");
    $('#txtSupplierVSS').val("");
    $('#txtFromDate').val("");
    $('#txtToDate').val("");
    $('#txtReferrerName').val("");
    $("#ddlSupplierStatus").get(0).selectedIndex = 0;
    $("#ddlSource").get(0).selectedIndex = 0;
    verifyAndAuditSuppliers();
});

$(".supplierSearchFilter").change(function () {
    $("#search-suppliers-filter-header-clear").show();
    $('#hdn-supplier-status-current-page').val(1);
    var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    PagerLinkClick(1, "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
});

$(document).on('click', '.supplierNameSort', function () {
    var sortDirection = $(this).attr('data-sortdirection');
    if ($(this).find('i').hasClass('fa-sort')) {
        $(this).find('i').removeClass('fa-sort');
    }
    $('.fa-sort-asc').addClass('fa-sort').removeClass('fa-sort-asc');
    $('.fa-sort-desc').addClass('fa-sort').removeClass('fa-sort-desc');
    if (sortDirection == 1) {
        $(this).find('i').addClass('fa-sort-asc');
        $(this).attr('data-sortDirection', '2');
    }
    else if (sortDirection == 2) {
        $(this).find('i').addClass('fa-sort');
        $(this).attr('data-sortDirection', '3');
    }
    else {
        $(this).find('i').addClass('fa-sort-desc');
        $(this).attr('data-sortDirection', '1');
    }
    sortDirection = $(this).attr('data-sortdirection');
    if (sortDirection == 3) {
        PagerLinkClick('1', "SupplierStatusList", "#hdn-supplier-status-current-page", "", sortDirection);
    }
    else {
        PagerLinkClick('1', "SupplierStatusList", "#hdn-supplier-status-current-page", 'companyName', sortDirection);
    }
});

//$(document).on('click', '.detailCheck', function () {

//    GenerateDetailsCheckAuditorUI();
//    var companyId = $(this).attr('data-companyId');

//    var start = 1900;
//    var end = new Date().getFullYear();
//    var options = "";
//    options = "<option value=''>" + selectYearText + "</option>"
//    for (var year = end ; year >= start; year--) {
//        options += "<option value=\"" + year + "\">" + year + "</option>";
//    }
//    document.getElementById("CompanyYearInitialCheck").innerHTML = options;
//    GetSupplierDetailsInfoToVerify(companyId);
//    $('html, body').animate({
//        scrollTop: 0
//    }, 'slow');
//    return false;
//});

//function GetSupplierDetailsInfoToVerify(companyId) {

//    $.ajax({
//        type: 'post',
//        url: '/Admin/SupplierDetailsPrimaryCheck',
//        data: { companyId: companyId },
//        async: false,
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                var data = response.model;
//                var answers = response.answers;
//                $('#verifyAndAuditSuppliers').hide();
//                $('#tabVerifyDetails').show();

//                for (var i = 1; i <= InitialQuestionCount; i++) {
//                    $('#details-status-comment-' + i).val('');
//                    $('input[ name ="details-status-not-checked-' + i + '"]').prop('checked', false);
//                    $('#details-status-comment-' + i).prop('disabled', true);
//                    $('#details-status-options-' + i).prop('disabled', true);
//                    $('#details-comments-visibility-' + i).prop('disabled', true);
//                    $('#details-status-flagged-reason-' + i).prop('disabled', true);
//                    $('#details-status-verify-reason-' + i).prop('disabled', true);
//                    $('input[ name ="details-status-not-checked-' + i + '"]').parent().parent().parent().addClass('audit-status-div');
//                    $('input[ name ="details-status-not-checked-' + i + '"]').parent().parent().parent().css("border", "1px solid #8DE8F7");
//                }
//                AddRequiredIcons();
//                $('#organisationNameInitialCheck').attr('readonly', true);
//                $('#organisationNameInitialCheck').val(data.OrganisationName);
//                $("#InitialCheckSupplierForm input[name=sector][value='" + data.sector + "']").prop("checked", true);
//                $('#TradingInitialCheck').val(data.Trading);
//                $('#TypeOfCompanyInitialCheck').val(data.TypeOfCompany);
//                $('#CompanyYearInitialCheck').val(data.CompanyYear);
//                //For Not Sure
//                if (data.sector == 485) {
//                    //$("#hdnBusinessSectorDescription").val(data.BusinessSectorDescription)
//                    $("#InitialCheckSupplierForm input[name=sector][value='" + 485 + "']").parent().show();
//                    $("#spnBusinessSectorDescriptionInitialCheck").html(data.BusinessSectorDescription).prop("readonly", true);
//                    $("#divsectorDescriptionInitialCheck").show();
//                }
//                else {
//                    $("#InitialCheckSupplierForm [name=sector][value='" + 485 + "']").parent().hide();
//                    $("#spnBusinessSectorDescriptionInitialCheck").html("");
//                    $("#divsectorDescriptionInitialCheck").hide();
//                }
//                if (data.IsVAT) {
//                    $("#InitialCheckSupplierForm [name=IsVAT][value='True']").prop("checked", true);
//                    $('#HaveVATInitialCheck').show();
//                    $('#vatNumberInitialCheck').val(data.VATNumber);
//                    $('#InitialIsVATAnswerdiv').hide();
//                }
//                else {
//                    $("#InitialCheckSupplierForm input[name=IsVAT][value='False']").prop("checked", true);
//                    $('#vatNumberInitialCheck').val('');
//                    $('#HaveVATInitialCheck').hide();
//                    $('#InitialIsVATAnswerdiv').show();
//                }
//                if (data.HaveDuns) {
//                    $("#InitialCheckSupplierForm input[name=HaveDuns][value='True']").prop("checked", true);
//                    $('#HaveDUNSInitialCheck').show();
//                    $('#DUNSNumberInitialCheck').val(data.DUNSNumber);
//                    $('#InitialCheckHaveDunsAnswerdiv').hide();
//                }
//                else {
//                    $("#InitialCheckSupplierForm input[name=HaveDuns][value='False']").prop("checked", true);
//                    $('#HaveDUNSInitialCheck').hide();
//                    $('#DUNSNumberInitialCheck').val('');
//                    $('#InitialCheckHaveDunsAnswerdiv').show();
//                }

//                for (var i = 1; i <= InitialQuestionCount; i++) {
//                    var answer = $.grep(answers, function (e) {
//                        return e.QuestionId == i;
//                    });
//                    if (answer.length >= 0 && answer[0] != undefined) {
//                        if (answer[0].AnswerId != 1) {
//                            $('#details-status-options-' + i).prop('disabled', false);

//                            $('#details-status-options-' + i).val(answer[0].AnswerId);
//                            $('input[name ="details-status-not-checked-' + i + '"]').prop('checked', true);
//                            switch (answer[0].AnswerId) {
//                                case 3: $('#details-status-verify-reason-' + i).hide();
//                                    $('#details-status-flagged-reason-' + i).show();
//                                    $('#details-status-flagged-reason-' + i).prop('disabled', false);
//                                    $('#details-status-flagged-reason-' + i).val(answer[0].ReasonCode);
//                                    break;
//                                case 4:
//                                    $('#details-status-flagged-reason-' + i).hide();
//                                    $('#details-status-verify-reason-' + i).show();
//                                    $('#details-status-verify-reason-' + i).prop('disabled', false);
//                                    $('#details-status-verify-reason-' + i).val(answer[0].ReasonCode);
//                                    break;
//                            }
//                            $('#details-status-comment-' + i).prop('disabled', false);
//                            $('#details-comments-visibility-' + i).prop('disabled', false);
//                            $('#details-status-comment-' + i).val(answer[0].Comments);
//                            $('#details-comments-visibility-' + i).val(answer[0].CommentsVisibleTo);

//                            var color = '#8DE8F7';
//                            switch (answer[0].AnswerId) {
//                                case 2:
//                                    color = '#DCE9AE';
//                                    break;
//                                case 3:
//                                    color = '#FFC000';
//                                    break;
//                                case 4:
//                                    color = '#439539';
//                                    break;
//                            }
//                            var colorText = "1px solid  " + color;
//                            $('input[ name ="details-status-not-checked-' + i + '"]').parent().parent().parent().css("border", colorText);
//                        }

//                    }
//                }
//                $('#firstAddressLine1InitialCheck').val(data.FirstAddressLine1);
//                $('#firstAddressLine2InitialCheck').val(data.FirstAddressLine2);
//                $('#firstAddressCityInitialCheck').val(data.FirstAddressCity);
//                $('#firstAddressStateInitialCheck').val(data.FirstAddressState);
//                $('#firstAddressPostalCodeInitialCheck').val(data.FirstAddressPostalCode);
//                $('#firstAddressCountryInitialCheck').val(data.FirstAddressCountry);
//                $('#companyIdInitialCheck').val(data.CompanyId);
//                $('#userIdInitialCheck').val(data.UserId);
//                $('#regNumberInitialCheck').val(data.CompanyRegistrationNumber);
//                //if(data)
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//}



//$(document).on('click', '.profileCheck', function () {
//    localStorage.setItem("BreadCrumb", "SupplierHome");
//    Common.IsLoadingNeeded = true;
//    var companyId = $(this).attr('data-companyId');
//    Common.IsNavigateAsynchronous = false;
//    Navigate('GetSupplierInformation', '/Admin/GetSupplierInformation', "Supplier Information", true);
//    GetSupplierInformationByIdInMainPage(companyId);
//    return false;
//});

//function InitialProfileAnswersValidation() {
//    //Details check count
//    var detailsCheckQuestionCount = 6;
//    for (var i = 1; i <= detailsCheckQuestionCount; i++) {
//        var QuestionId = i;
//        var needed = true;
//        switch (QuestionId) {
//            case 3:
//                if ($('#InitialCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "True") {
//                    needed = false;
//                }
//                break;
//            case 4:
//                if ($('#InitialCheckSupplierForm').find('input[name=IsVAT]:Checked').val() != "True") {
//                    needed = false;
//                }
//                break;
//        }
//        if (needed && !$('input[name="details-status-not-checked-' + QuestionId + '"]').prop('checked')) {
//            return false;
//        }
//    }
//    return true;
//}


//function CheckInitialProfileAnswers() {
//    var result = false;
//    var answerList = [];
//    for (var i = 1; i <= InitialQuestionCount; i++) {
//        var CompanyId = $('#companyIdInitialCheck').val();
//        var QuestionId = i;
//        var Answer = 1;
//        var Reason = null;
//        var Comments = null;
//        var NotNeeded = false;
//        var CommentsVisibleTo = 1;
//        switch (i) {
//            case 10:
//            case 11:
//            case 13:
//                if ($('#InitialCheckSupplierForm').find('input[name=HaveDuns]:Checked').val() == "True") {
//                    NotNeeded = true;
//                }
//                break;
//            case 14:
//                if ($('#InitialCheckSupplierForm').find('input[name=HaveDuns]:Checked').val() != "True") {
//                    NotNeeded = true;
//                }
//                break;
//            case 3:
//                if ($('#InitialCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "True") {
//                    NotNeeded = true;
//                }
//                break;
//            case 4:
//                if ($('#InitialCheckSupplierForm').find('input[name=IsVAT]:Checked').val() != "True") {
//                    NotNeeded = true;
//                }
//                break;
//            default:
//                break;
//        }
//        if (!NotNeeded) {
//            Comments = $('#details-status-comment-' + QuestionId).val();
//            CommentsVisibleTo = $('#details-comments-visibility-' + QuestionId).val();
//            if ($('input[name="details-status-not-checked-' + QuestionId + '"]').prop('checked')) {
//                Answer = $('#details-status-options-' + QuestionId).val();
//                if (Answer == 3) {
//                    Reason = $('#details-status-flagged-reason-' + QuestionId).val();
//                }
//                else if (Answer == 4) {
//                    Reason = $('#details-status-verify-reason-' + QuestionId).val();
//                }

//            }
//            var item = { SupplierId: CompanyId, QuestionId: QuestionId, AnswerId: Answer, ReasonCode: Reason, Comments: Comments, CommentsVisibleTo: CommentsVisibleTo };
//            answerList.push(item);
//        }
//        else {
//            NotNeeded = false;
//        }
//    }
//    answerList = JSON.stringify({ 'answerList': answerList });
//    $.ajax({
//        dataType: 'json',
//        type: 'POST',
//        url: " /Admin/PostProfileAnswers",
//        data: answerList,
//        contentType: 'application/json; charset=utf-8',
//        async: false,
//        success: function (response) {
//            result = response;
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return result;
//}


//function GenerateDetailsCheckAuditorUI() {
//    var flaggedReasonCodeHtml = "";
//    var verifyReasonCodeHtml = "";
//    var visibiltyHtml = "";
//    for (var j = 0; j < detailsFlaggedReasonList.length; j++) {

//        flaggedReasonCodeHtml += "<option value=\"" + detailsFlaggedReasonList[j].Value + "\">" + detailsFlaggedReasonList[j].Text + "</option>";
//    }
//    for (var j = 0; j < detailsVisibilityList.length; j++) {

//        visibiltyHtml += "<option value=\"" + detailsVisibilityList[j].Value + "\">" + detailsVisibilityList[j].Text + "</option>";
//    }
//    for (var j = 0; j < detailsVerifyReasonList.length; j++) {

//        verifyReasonCodeHtml += "<option value=\"" + detailsVerifyReasonList[j].Value + "\">" + detailsVerifyReasonList[j].Text + "</option>";
//    }
//    for (var i = 1; i <= InitialQuestionCount; i++) {
//        if ($('#details-answer-' + i).length) {
//            var auditResponsehtml = "  <div class=\"col-md-12 col-sm-12 col-xs-12 details-status-not-checked padding-zero audit-status-checked-div\">" +
//                                     "<input type=\"checkbox\" name=\"details-status-not-checked-" + i + "\" value=\"" + i + "\" /><span style=\"padding-left:6px\"><b>" + checkedText + "</b></span></div>" +
//                                "<div class=\"col-md-6 col-sm-5 col-xs-12 padding-top-10px\"><label>" + verificationStatusText + "</label>" +
//                                     "<select class=\"form-control  details-status-options\" id=\"details-status-options-" + i + "\">" +
//                                         "<option value=\"4\">Verified</option><option value=\"2\">Self Declared</option><option value=\"3\">Flagged</option></select></div>" +
//                               " <div class=\"col-md-6 col-sm-5 col-xs-12 padding-top-10px\"><label>" + reasonCodeText + "</label><select class=\"form-control display-none\" id=\"details-status-flagged-reason-" + i + "\">" +
//                               flaggedReasonCodeHtml + "</select>" +
//                               "<select class=\"form-control \" id=\"details-status-verify-reason-" + i + "\">" +
//                               verifyReasonCodeHtml + "</select></div>" +
//                               "<div class=\"col-md-6 col-sm-4 col-xs-12 padding-top-10px\" id=\"details-status-comment-div-" + i + "\"><label>" + commentsText + "</label><br />" +
//                               "<input type=\"text\" class=\"form-control\" id=\"details-status-comment-" + i + "\" /></div>" +
//                                "<div class=\"col-md-6 col-sm-5 col-xs-12 padding-top-10px\"><label>" + commentsVisibleToText + "</label><br /><select class=\"form-control \" id=\"details-comments-visibility-" + i + "\">" +
//                               visibiltyHtml + "</select><div class=\"clear-both\"></div></div>";
//            $('#details-answer-' + i).html(auditResponsehtml);
//        }
//    }

//}

//function GetSupplierInformationByIdInMainPage(companyId) {
//    localStorage.breadCrumb = 0;
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetSupplierInformationByCompanyId',
//        data: { companyId: companyId },
//        async: false,
//        success: function (data) {
//            if (typeof (data) != "undefined") {
//                FillSupplierData(data.model, data.answers, data.status);
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//}

//$(document).on('click', '.sanctionCheck', function () {
//    Common.IsLoadingNeeded = true;
//    var companyId = $(this).attr('data-companyId');
//    localStorage.SanctionCompanyId = companyId;
//    localStorage.TempCompanyId = 0;
//    var id = localStorage.TempCompanyId;
//    // Navigate('sanctionIndex', "/Sanction", "Sanction Verification", true);
//    window.location.href = "/Sanction";
//    return false;
//});


//$('#btnCancelInitialCheck').click(function () {
//    $('#verifyAndAuditSuppliers').show();
//    $('#tabVerifyDetails').hide();
//    $('html, body').animate({
//        scrollTop: 0
//    }, 'slow');
//});

//$('#BackToSuppliers').click(function () {
//    $('#verifyAndAuditSuppliers').show();
//    $('#tabVerifyDetails').hide();
//    $('html, body').animate({
//        scrollTop: 0
//    }, 'slow');
//});

//$('#btnApproveInitialCheck').click(function () {

//    var companyId = $('#companyIdInitialCheck').val();
//    var notSure = $("#rdoNotSureInitial").prop('checked');
//    if (notSure == true) {
//        showErrorMessage(industrySectorValidation);
//        return false;
//    }
//    if (!InitialProfileAnswersValidation()) {
//        showErrorMessage(auditQuestionsValidation);
//        return false;
//    }
//    if (!CheckInitialProfileAnswers()) {
//        showErrorMessage(errorMessage);
//        return false;
//    }
//    if (detailCheckValidation() && companyId != "") {
//        $.ajax({
//            type: 'post',
//            url: '/Admin/VerifyInitialCheck',
//            data: $('#InitialCheckSupplierForm').serialize(),
//            async: false,
//            success: function (data) {
//                if (typeof (data) != "undefined") {
//                    if (data.result) {
//                        $('#verifyAndAuditSuppliers').show();
//                        $('#tabVerifyDetails').hide();
//                        $('#TypeOfCompanyInitialCheck').val('');
//                        $('#CompanyYearInitialCheck').val('');
//                        $('#organisationNameInitialCheck').val("");
//                        $('#firstAddressLine1InitialCheck').val("");
//                        $('#firstAddressLine2InitialCheck').val("");
//                        $('#firstAddressCityInitialCheck').val("");
//                        $('#firstAddressStateInitialCheck').val("");
//                        $('#firstAddressPostalCodeInitialCheck').val("");
//                        $('#firstAddressCountryInitialCheck').val("");
//                        $('#companyIdInitialCheck').val("");
//                        $('#regNumberInitialCheck').val("");
//                        $("#InitialCheckSupplierForm").find("input[name=isVAT][value='False']").prop("checked", true);
//                        $('#vatNumberInitialCheck').val('');
//                        $('#HaveVATInitialCheck').hide();
//                        $('#InitialIsVATAnswerdiv').show();
//                        verifyAndAuditSuppliers();
//                        $('html, body').animate({
//                            scrollTop: 0
//                        }, 'slow');
//                        showSuccessMessage(data.message);
//                    }
//                    else {
//                        showErrorMessage(data.message);
//                    }
//                }
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//            }
//        });
//    }
//    else {
//        return false;
//    }
//});


//function detailCheckValidation() {
//    var result = true;
//    RemoveBorderColor('#vatNumberInitialCheck');

//    $('.error-text').hide();
//    var setFocus = false;
//    $.validator.unobtrusive.parse($('#InitialCheckSupplierForm'));
//    if (!$('#InitialCheckSupplierForm').valid()) {
//        result = false;
//    }
//    if (!result) {
//        var els = document.querySelector('.input-validation-error');
//        if (els != null) {
//            els.focus();
//        }
//    }
//    if ($('#InitialCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "True") {
//        if ($('#vatNumberInitialCheck').val() == "") {
//            SetBorderColor("#vatNumberInitialCheck", "red");
//            $('#vatNumberInitialCheck').show();
//            if (!setFocus) {
//                $('#vatNumberInitialCheck').focus();
//                setFocus = true;
//            }
//            showErrorMessage(vatNumberValidation);
//            result = false;
//        }

//    }
//    else if ($('#InitialCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "False") {
//        $('#vatNumberInitialCheck').val('');
//        //$('#W9W8Form').val('');
//    }
//    return result;
//}

//$(document).on('click', '.details-status-not-checked :checkbox', function () {
//    var $this = $(this);
//    var value = $(this).parent().parent().attr('data-questionid');
//    var inputId = $(this).parent().parent().attr('data-inputid');
//    var color = '#8DE8F7';
//    if ($this.is(':checked')) {
//        $('#details-status-options-' + value).show();
//        $('#details-status-options-' + value).prop('disabled', false);
//        $('#details-status-comment-' + value).prop('disabled', false);
//        $('#details-comments-visibility-' + value).prop('disabled', false);
//        color = '#439539';
//        switch (inputId) {
//            case "None": $('#details-status-options-' + value).val(4);
//                $('#details-status-verify-reason-' + value).prop('disabled', false);
//                $('#details-status-verify-reason-' + value).show();
//                $('#details-status-flagged-reason-' + value).hide();
//                break;
//            default:
//                var val = $('#' + inputId).val();
//                if (val != "") {
//                    $('#details-status-options-' + value).val(4);
//                    $('#details-status-verify-reason-' + value).prop('disabled', false);
//                    $('#details-status-verify-reason-' + value).show();
//                    $('#details-status-flagged-reason-' + value).hide();
//                }
//                else {
//                    $('#details-status-options-' + value).val(3);
//                    $('#details-status-flagged-reason-' + value).prop('disabled', false);
//                    $('#details-status-flagged-reason-' + value).show();
//                    $('#details-status-verify-reason-' + value).hide();
//                    $('#details-status-flagged-reason-' + value).val(490);
//                    color = '#FFC000';
//                }
//                break;
//        }
//    }
//    else {
//        $('#details-status-comment-' + value).prop('disabled', true);
//        $('#details-status-options-' + value).prop('disabled', true);
//        $('#details-comments-visibility-' + value).prop('disabled', true);
//        $('#details-status-flagged-reason-' + value).prop('disabled', true);
//        $('#details-status-verify-reason-' + value).prop('disabled', true);
//        if ($('#details-status-options-' + value).val() == 3) {
//            $('#details-status-flagged-reason-' + value).show();
//            $('#details-status-verify-reason-' + value).hide();
//        }
//        else {
//            $('#details-status-flagged-reason-' + value).hide();
//            $('#details-status-verify-reason-' + value).show();
//        } color = '#8DE8F7';
//    }
//    var colorText = "1px solid  " + color;
//    $('input[ name ="details-status-not-checked-' + value + '"]').parent().parent().parent().css("border", colorText);
//});

//$('#InitialCheckSupplierForm').on('click', 'input[name=IsVAT]', function (e) {
//    if ($(this).attr("value") == "True") {
//        $('#HaveVATInitialCheck').show();
//        $('#InitialIsVATAnswerdiv').hide();
//    }
//    if ($(this).attr("value") == "False") {
//        $('#HaveVATInitialCheck').hide();
//        $('#vatNumberInitialCheck').val('');
//        $('#InitialIsVATAnswerdiv').show();
//    }
//    return true;
//});

//$('#InitialCheckSupplierForm').on('click', 'input[name=HaveDuns]', function (e) {
//    if ($(this).attr("value") == "True") {
//        $('#HaveDUNSInitialCheck').show();
//        $('#InitialCheckHaveDunsAnswerdiv').hide();
//    }
//    if ($(this).attr("value") == "False") {
//        $('#HaveDUNSInitialCheck').hide();
//        $('#InitialCheckHaveDunsAnswerdiv').show();
//    }
//    return true;
//});

//$(document).on('change', '.details-status-options', function () {
//    var value = $(this).val();
//    var questionId = $(this).parent().parent().attr('data-questionid');
//    var color = '#439539';
//    switch (value) {
//        case "2":
//            color = '#DCE9AE';
//            break;
//        case "3":
//            color = '#FFC000';
//            break;
//    }
//    var colorText = "1px solid  " + color;
//    $('input[ name ="details-status-not-checked-' + questionId + '"]').parent().parent().parent().css("border", colorText);
//    $('#details-status-flagged-reason-' + questionId).prop('disabled', true);
//    $('#details-status-verify-reason-' + questionId).prop('disabled', true);
//    if (value == "3") {
//        $('#details-status-flagged-reason-' + questionId).show();
//        $('#details-status-verify-reason-' + questionId).hide();
//        $('#details-status-flagged-reason-' + questionId).prop('disabled', false);
//        $('#details-status-verify-reason-' + questionId).prop('disabled', true);
//    }
//    else if (value == "4") {
//        $('#details-status-flagged-reason-' + questionId).hide();
//        $('#details-status-verify-reason-' + questionId).show();
//        $('#details-status-flagged-reason-' + questionId).prop('disabled', true);
//        $('#details-status-verify-reason-' + questionId).prop('disabled', false);
//    }
//});


$('#btnSupplierStatusExport').click(function () {
    var fromDate = $.trim($('#txtFromDate').val());
    var toDate = $.trim($('#txtToDate').val());
    var status = parseInt($('#ddlSupplierStatus').val());
    var supplierName = ($("#txtSupplierVSS").val() != $('input[id=txtSupplierVSS]').attr('placeholder')) ? $("#txtSupplierVSS").val() : "";

    var supplierId = ($('#txtSupplierIdVSS').val() != $('input[id=txtSupplierIdVSS]').attr('placeholder')) ? $('#txtSupplierIdVSS').val() : "";
    var referrerName = ($('#txtReferrerName').val() != $('input[id=txtReferrerName]').attr('placeholder')) ? $('#txtReferrerName').val() : "";
    var source = $('#ddlSource').val();
    window.location.href = "/Admin/SupplierOrganizationExport/?fromDate=" + fromDate + "&toDate=" + toDate + "&supplierName=" + supplierName + "&referrerName=" + referrerName + "&Status=" + status + "&supplierId=" + supplierId + "&source=" + source;
});

$(window).scroll(function () {
    FixedTableHeaderWithPagination('tblSuppliers');
});

$(window).resize(function () {
    FixedTableHeaderWithPagination('tblSuppliers');

});

$('#SaveReferrerDetails').click(function () {
    var assignReferrer = [];
    var removeReferrer = [];
    var primaryReferrer = [];
    var supplierId = $('#hdnSupplierId').val();
    var j = 0;
    $('.add-referrer').each(function () {
        if ($(this).is(':checked')) {
            assignReferrer.push($(this).attr("data-campaign-id"));
        }
    });
    //if (assignReferrer.length > 0) {
    //    AssignSupplierReferrer(supplierId, assignReferrer);
    //}
    $('.remove-refferer').each(function () {
        if (!$(this).is(':checked')) {
            removeReferrer.push($(this).attr("data-remove-campaign-id"));
        }
    });
    //if (removeReferrer.length > 0) {
    //    RemoveSupplierReferrer(supplierId, removeReferrer);
    //}
    $('.assign-primary-referrer').each(function () {
        if ($(this).is(':checked')) {
            primaryReferrer = $(this).attr("data-primary-campaign-id");
        }
    });
    if (assignReferrer.length == 0 && removeReferrer.length == 0 && primaryReferrer.length == 0) {
        showErrorMessage(referrerValidation);
    }
    else {
        AddOrUpdateSupplierReferrer(supplierId, assignReferrer, removeReferrer, primaryReferrer);
    }
});

function AddOrUpdateSupplierReferrer(supplierId, assignReferrer, removeReferrer, primaryReferrer) {
    $.ajax({
        async: false,
        type: 'post',
        url: '/Admin/AddOrUpdateSupplierReferrer',
        data: JSON.stringify({ 'supplierId': supplierId, 'assignReferrerCampaign': assignReferrer, 'removeReferrerCampaign': removeReferrer, 'primaryReferrerCampaign': primaryReferrer }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#AddReferrer').modal('hide');
                var sortDirection = $(".supplierNameSort").attr('data-sortdirection');
                var sortParameter = "";
                if (sortDirection != 3) {
                    sortParameter = "CompanyName";
                }
                PagerLinkClick('1', "SupplierStatusList", "#hdn-supplier-status-current-page", sortParameter, sortDirection);
            }
            else
                showErrorMessage(response.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
};

$(document).on('click', '.btnEditSupplierPrimaryContactDetails', function () {
    $('#EditSupplierPrimaryDetails').find('form')[0].reset();
    $('.field-validation-valid').html('');

    $("#txtPrimaryEmail").attr("class", "form-control");
    $("#txtPrimaryFirstName").attr("class", "form-control");
    $("#txtPrimaryLastName").attr("class", "form-control");
    $("#txtTelephone").attr("class", "form-control");
    $("#txtJobTitle").attr("class", "form-control");

    var userID = $(this).attr('data-UserId-Profile');
    $.ajax({
        type: 'post',
        url: '/Admin/EditUserProfile',
        data: { userId: userID },
        dataType: "json",
        success: function (data) {
            var items = data.users;
            $("#txtLoginID").val(items.LoginId);
            $("#txtPrimaryFirstName").val(items.PrimaryFirstName);
            $("#txtPrimaryEmail").val(items.PrimaryEmail);
            $("#txtPrimaryLastName").val(items.PrimaryLastName);
            $("#txtTelephone").val(items.Telephone);
            $("#txtJobTitle").val(items.JobTitle);
            $("#hdnUserId").val(items.UserId);
            $('#hdnCompID').val(items.OrganizationPartyId);
            $('#txtSupplierPrimaryContactPartyId').val(items.PrimaryContactPartyId);
            $('#EditSupplierPrimaryDetails').modal('show');
        },
        error: function (data) {
        }

    });
    return false;
});


$(document).on('click', '#btnResetSupplierPrimaryDetails', function () {
    var logId = $('#txtLoginID').val();
    $('#EditSupplierPrimaryDetails').find('form')[0].reset();
    $("#txtPrimaryEmail").attr("class", "form-control");
    $("#txtPrimaryFirstName").attr("class", "form-control");
    $("#txtPrimaryLastName").attr("class", "form-control");
    $("#txtTelephone").attr("class", "form-control");
    $("#txtJobTitle").attr("class", "form-control");
    $('.field-validation-valid').html('');
    $('#txtLoginID').val(logId);
    $("#txtPrimaryFirstName").val("");
    $("#txtPrimaryEmail").val("");
    $("#txtPrimaryLastName").val("");
    $("#txtTelephone").val("");
});


$(document).on('click', '#btnCancelSupplierPrimaryDetails', function () {
    $('#EditSupplierPrimaryDetails').find('form')[0].reset();
    $('.field-validation-valid').html('');
});

$(document).on('click', '#btnSaveSupplierPrimaryDetails', function () {
    if ($("#txtPrimaryEmail").val() == "" && $("#txtPrimaryFirstName").val() == "" && $("#txtPrimaryLastName").val() == "" && $("#txtTelephone").val() == "" && ($("#txtJobTitle").val() == "")) {
        showErrorMessage(requiredFieldsMessage);
        $("#txtPrimaryEmail").attr("class", "input-validation-error form-control");
        $("#txtPrimaryFirstName").attr("class", "input-validation-error form-control");
        $("#txtPrimaryLastName").attr("class", "input-validation-error form-control");
        $("#txtTelephone").attr("class", "input-validation-error form-control");
        return false;
    }
    if ($("#txtPrimaryEmail").val() == "") {
        showErrorMessage(primaryEmailValidation);
        $("#txtPrimaryEmail").attr("class", "input-validation-error form-control");
        return false;
    }
    if ($("#txtPrimaryEmail").val() != "") {
        var email = $("#txtPrimaryEmail").val();
        if (!IsValidEmail(email)) {
            showErrorMessage(validPrimaryEmail);
            $("#txtPrimaryEmail").attr("class", "input-validation-error form-control");
            return false;
        }
    }
    if ($("#txtPrimaryFirstName").val() == "") {
        showErrorMessage(primaryFirstNameValidation);
        $("#txtPrimaryFirstName").attr("class", "input-validation-error form-control");
        return false;
    }
    if ($("#txtPrimaryLastName").val() == "") {
        showErrorMessage(primaryLastNameValidation);
        $("#txtPrimaryLastName").attr("class", "input-validation-error form-control");
        return false;
    }
    if ($("#txtTelephone").val() == "") {
        showErrorMessage(telephoneNumberValidation);
        $("#txtTelephone").attr("class", "input-validation-error form-control");
        return false;

    }
    if ($("#txtTelephone").val() != "") {
        var phnNumber = $("#txtTelephone").val();
        if (!IsPhoneNumberValid(phnNumber)) {
            showErrorMessage(validTelephoneNumber);
            $("#txtTelephone").attr("class", "input-validation-error form-control");
            return false;
        }
    }
    $.ajax({
        type: 'post',
        url: '/Admin/UpdateUserProfile',
        data: { OrganizationPartyId: $("#hdnCompID").val(), LoginId: $("#txtLoginID").val(), PrimaryFirstName: $("#txtPrimaryFirstName").val(), PrimaryLastName: $("#txtPrimaryLastName").val(), PrimaryEmail: $("#txtPrimaryEmail").val(), Telephone: $("#txtTelephone").val(), JobTitle: $("#txtJobTitle").val(), UserId: $("#hdnUserId").val(), PrimaryContactPartyId: $("#txtSupplierPrimaryContactPartyId").val() },
        dataType: "json",

        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#EditSupplierPrimaryDetails').modal('hide');
                verifyAndAuditSuppliers();
            }
            else
                showErrorMessage(response.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
});

function IsPhoneNumberValid(phnNumber) {
    var phnNum = /^[0-9-+]+$/;
    return phnNum.test(phnNumber);
}

(function ($) {
    $.simplePlaceholder = {
        placeholderClass: null,

        hidePlaceholder: function () {
            var $this = $(this);
            if ($this.val() == $this.attr('placeholder') && $this.data($.simplePlaceholder.placeholderData)) {
                $this
                  .val("")
                  .removeClass($.simplePlaceholder.placeholderClass)
                  .data($.simplePlaceholder.placeholderData, false);
            }
        },

        showPlaceholder: function () {
            var $this = $(this);
            if ($this.val() == "") {
                $this
                  .val($this.attr('placeholder'))
                  .addClass($.simplePlaceholder.placeholderClass)
                  .data($.simplePlaceholder.placeholderData, true);
            }
        },

        preventPlaceholderSubmit: function () {
            $(this).find(".simple-placeholder").each(function (e) {
                var $this = $(this);
                if ($this.val() == $this.attr('placeholder') && $this.data($.simplePlaceholder.placeholderData)) {
                    $this.val('');
                }
            });
            return true;
        }
    };

    $.fn.simplePlaceholder = function (options) {
        if (document.createElement('input').placeholder == undefined) {
            var config = {
                placeholderClass: 'placeholding',
                placeholderData: 'simplePlaceholder.placeholding'
            };

            if (options) $.extend(config, options);
            $.extend($.simplePlaceholder, config);

            this.each(function () {
                var $this = $(this);
                $this.focus($.simplePlaceholder.hidePlaceholder);
                $this.blur($.simplePlaceholder.showPlaceholder);
                $this.data($.simplePlaceholder.placeholderData, false);
                if ($this.val() == '') {
                    $this.val($this.attr("placeholder"));
                    $this.addClass($.simplePlaceholder.placeholderClass);
                    $this.data($.simplePlaceholder.placeholderData, true);
                }
                $this.addClass("simple-placeholder");
                $(this.form).submit($.simplePlaceholder.preventPlaceholderSubmit);
            });
        }

        return this;
    };

})(jQuery);

