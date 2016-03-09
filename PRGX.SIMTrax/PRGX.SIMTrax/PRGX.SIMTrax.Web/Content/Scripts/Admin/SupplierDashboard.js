$(document).ready(function () {
    CheckPermission("Suppliers", function () {
        GetCompanySummary();
    });
});

$('#companySummaryDropdown .dropdown-menu').on({
    "click": function (e) {
        e.stopPropagation();
    }
});

function GetCompanySummary() {
    CompanySummaryDetails();
    GetSupplierUserDetailsForDashboard(1, "", 1);
    //GetBuyersMappedToSuppliers(1, "", 1);
}

//function GoToPayments() {
//    var url = "/Admin/PaymentReports"
//    // window.location.href = "/Admin/PaymentReports";
//    Navigate('PaymentReports', url, "Payment", true);
//}

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

function CompanySummaryDetails() {
    SelectMenu("AdminSupportTab");
    if (localStorage.fromPaymentPage == "Payment") {
        $("#CompanySummaryBreadCrumb").html("< " + backToPayments);
        $("#CompanySummaryBreadCrumb").attr("onclick", "GoToPayments()");
        localStorage.fromPaymentPage = "";
    }
    else {
        $("#CompanySummaryBreadCrumb").html("< " + backToSupplierOrg);
        $("#CompanySummaryBreadCrumb").attr("onclick", "GoToSupplierOrganisations()");
    }
    $('#supplier-primary-contact-table-rows').html('');
    //$("#buyers-mapped-table-rows").html('');
    $("#supplier-users-table-rows").html('');
    //$("#supplier-questionnaire-table-rows").html('');
    //$("#supplier-verification-list-table-rows").html('');
    //var companyId = companyIdForProfileDetails;
    //var url = window.location.pathname;
    //var arr = url.split('/');
    //companyId = parseInt(arr[arr.length - 1]);
    $('#hdnCompanyId').val(supplierPartyId);
    if (!CanCreateUser) {
        $('#btnShowAddUserForm').hide();
    }
    $.ajax({
        type: "POST",
        url: "/Admin/GetSupplierDetailsForDashboard",
        data: { partyId: supplierPartyId },
        dataType: "json",
        success: function (data) {
            $('#CompanyName').html(data.result.SupplierOrganizationName);
            localStorage.companyName = data.result.SupplierOrganizationName;
            document.title = data.result.SupplierOrganizationName + " " + dashboardText;
            var name = "-";
            var primaryEmail = "-";
            var action = "-";
            if (CanEditUser) {
                action = "<div class=\"btn-group pull-right\" id=\"companySummaryDropdown\"> <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionsButton + "<span class=\"caret\"></span></button><ul class=\"dropdown-menu\" ><li><a class=\"btnEditSupplierPrimaryContactDetailsDashboard\"  data-UserId-Profile=\"" + data.result.SupplierUserId + "\">" + editProfileButton + "</a></li></ul></div>";
            }
            var newRow = "<tr class=\" odd \"  ><td >" + data.result.SupplierId + "</td><td>" + data.result.SupplierOrganizationName +
                    "</td><td>" + data.result.PrimaryContactName + "</td><td>" + data.result.PrimaryContactEmail + "</td>" +
                    "<td  class=\"text-align-center\">" + action + "</td></tr>";
            $('#supplier-primary-contact-table-rows').append(newRow);
            $('#tblPrimaryDetails').footable();
            $('#tblPrimaryDetails').trigger('footable_redraw');
            //if (data.result.SupplierProducts.length > 0) {
            //    GetSupplierVerificationDetails(data.companyDetails);
            //}
        }
    });
}


//function GetBuyersMappedToSuppliers(pageNumber, sortParameter, sortDirection) {
//    var pageSize = 5;
//    var row = "";
//    $.ajax({
//        cache: false,
//        type: 'post',
//        url: '/Admin/GetBuyersMappedToSuppliers',
//        data: { companyId: $('#hdnCompanyId').val(), pageNumber: pageNumber, sortDirection: sortDirection },
//        success: function (response) {
//            if (response.buyerDetails.length > 0) {
//                var trClass = "";
//                for (var i = 0; (i < response.buyerDetails.length) ; i++) {
//                    if (i % 2 == 0) {
//                        trClass = "odd";
//                    }
//                    else {
//                        trClass = "even";
//                    }
//                    var items = response.buyerDetails[i];
//                    var uName = items.BuyerCompany.CompanyId;

//                    var trCls = "even";
//                    if (i % 2 == 0) {
//                        trCls = "odd";
//                    }
//                    var Status = "Not Active";
//                    if (items.BuyerCompany.IsActive) {
//                        Status = "Active";
//                    }
//                    row += "<tr class=\"" + trCls + "\"><td>" + items.BuyerCompany.CompanyName + "</td><td>" + Status + "</td><td style=\"text-align: center\" class=\"removeBuyerSupplierMapping\"><button class=\"btn btn-color btnRemove btnRemoveBuyer-click\" data-RemoveId=\"" + uName + "\" style=\"width:80px\">" + removeButton + "</button>\n";
//                }
//                $("#buyers-mapped-table-rows").html(row);
//            }
//            else {
//                $('#buyers-mapped-table-rows').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
//            }
//            $('.BuyersMappedPaginator').html(displayLinksForSmallContainers($('#hdnBuyersMappedCurrentPage').val(), Math.ceil(response.totalBuyers / pageSize), "CompanyName", sortDirection, "GetBuyersMappedToSuppliers", "#hdnBuyersMappedCurrentPage"));
//            $('#buyersMappedListTable').footable();
//            $('#buyersMappedListTable').trigger('footable_redraw');
//            if (!CanMapSupplierBuyer) {
//                $('.removeBuyerSupplierMapping').hide();
//            }
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            //alert('Failed to retrieve Buyer info.');
//        }
//    });
//}


function GetSupplierUserDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
    var pageSize = 5;
    var row = "";
    $.ajax({
        type: 'post',
        url: '/Admin/GetSupplierUserDetailsForDashboard',
        data: { supplierPartyId: $('#hdnCompanyId').val(), pageNumber: pageNumber, sortDirection: sortDirection },
        success: function (response) {
            if (response.userDetails.length > 0) {
                for (var i = 0; (i < response.userDetails.length) ; i++) {
                    var trClass = "odd";
                    if (i % 2 == 0) {
                        trClass = "even";
                    }
                    var actionDropdown = "-";
                    if (CanEditUser || CanChangePassword) {
                        actionDropdown = "<div class=\"btn-group pull-right\"  id=\"companySummaryDropdown\" > <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionsButton + "<span class=\"caret\"></span></button> <ul class=\"dropdown-menu\" >";
                        if (CanEditUser) {
                            actionDropdown += "<li><a class=\"btnEditSupplierUser\"  data-UserId-Profile=\"" + response.userDetails[i].UserId + "\">" + editUserDetails + "</a></li>";
                        }
                        if (CanChangePassword) {
                            actionDropdown += "<li><a onclick='ChangePassword(" + response.userDetails[i].UserId + ",\"" + response.userDetails[i].LoginId + "\")'>" + editPasswordButton + "</a></li>";
                        }
                        actionDropdown += "</ul></div>";
                    }
                    row += "<tr class=\"" + trClass + "\"><td>" + response.userDetails[i].UserName + "</td><td>" + response.userDetails[i].UserType + "</td><td  class=\"text-align-center\">" + actionDropdown + "</td></tr>";
                }
                $("#supplier-users-table-rows").html(row);
            }
            else {
                $('#supplier-users-table-rows').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
            }
            $('.SupplierUsersPaginator').html(displayLinksForSmallContainers($('#hdnSupplierUsersCurrentPage').val(), Math.ceil(response.totalUsers / pageSize), "FirstName", sortDirection, "GetSupplierUserDetailsForDashboard", "#hdnSupplierUsersCurrentPage"));
            $('#suppliersUsersListTable').footable();
            $('#suppliersUsersListTable').trigger('footable_redraw');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

//function GetSupplierVerificationDetails(data) {
//    $('#supplier-verification-list-table-rows').html('');
//    var output = "";
//    if (data.SupplierProducts.length > 0) {

//        var enablePublishButton = false;
//        var isPaymentDone = false;
//        var btnDetailCls = "";
//        var btnDetailDisabled = "";
//        var btnProfileCls = "";
//        var btnProfileDisabled = "";
//        var btnSanctionCls = "";
//        var btnSanctionDisabled = "";
//        var btnFITCls = "btn-normal";
//        var btnFITDisabled = "-";
//        var btnHSCls = "btn-normal";
//        var btnHSDisabled = "-";
//        var btnDSCls = "btn-normal";
//        var btnDSDisabled = "-";
//        //var btnCLCls = "btn-normal";
//        //var btnCLDisabled = "-";

//        if (isPaymentDone) {
//            if (data.Status == 1) {
//                btnDetailCls = "btn-normal";
//                btnDetailDisabled = "-";
//            }
//        }
//        else {
//            if (data.Status == 1) {
//                btnDetailCls = "btn-color";
//            }
//        }
//        switch (data.Status) {
//            case 1:
//                btnProfileCls = "btn-color";
//                btnSanctionCls = "btn-color";
//                btnSanctionDisabled = "-";
//                break;
//            case 2:
//                btnDetailCls = "btn-color";
//                btnDetailDisabled = checkedText;
//                btnProfileCls = "btn-color";
//                btnSanctionCls = "btn-color";
//                btnSanctionDisabled = "-";
//                break;
//            case 3:
//                if (data.IsPublished) {
//                    btnDetailDisabled = publishedText;
//                    btnProfileDisabled = publishedText;
//                    $("#publish-message").hide();
//                }
//                else {
//                    btnDetailDisabled = checkedText;
//                    btnProfileDisabled = checkedText;
//                }
//                btnDetailCls = "btn-color";
//                btnProfileCls = "btn-color";
//                btnSanctionCls = "btn-color";
//                break;
//            case 4:
//                if (data.IsPublished) {
//                    btnDetailDisabled = publishedText;
//                    btnProfileDisabled = publishedText;
//                    btnSanctionDisabled = publishedText;
//                    $("#publish-message").hide();
//                }
//                else {
//                    btnDetailDisabled = checkedText;
//                    btnProfileDisabled = checkedText;
//                    btnSanctionDisabled = checkedText;
//                }
//                btnDetailCls = "btn-color";
//                btnProfileCls = "btn-color";
//                btnSanctionCls = "btn-color";
//                break;
//        }
//        for (var j = 0; j < data.SupplierProducts.length; j++) {
//            if (j % 2 == 0) {
//                trClass = "odd";
//            }
//            else {
//                trClass = "even";
//            }
//            if (data.SupplierProducts[j].Product.ProductName == "DS") {
//                trClass = "odd";
//            }
//            output += "<tr class=\"" + trClass + "\">";

//            switch (data.SupplierProducts[j].Status) {
//                case 2:
//                    isPaymentDone = true;
//                    switch (data.SupplierProducts[j].Product.ProductName) {
//                        case "FIT":
//                            btnFITCls = "btn-color";
//                            btnFITDisabled = paidText;
//                            break;
//                        case "H&S":
//                            btnHSCls = "btn-color";
//                            btnHSDisabled = paidText;
//                            break;
//                        case "DS":
//                            btnDSCls = "btn-color";
//                            btnDSDisabled = paidText;
//                            break;
//                    }
//                    break;
//                case 3:
//                    isPaymentDone = true;
//                    switch (data.SupplierProducts[j].Product.ProductName) {
//                        case "FIT":
//                            btnFITCls = "btn-color";
//                            btnFITDisabled = evaluationStartedText;
//                            break;
//                        case "H&S":
//                            btnHSCls = "btn-color";
//                            btnHSDisabled = evaluationStartedText;
//                            break;
//                        case "DS":
//                            btnDSCls = "btn-color";
//                            btnDSDisabled = evaluationStartedText;
//                            break;
//                    }
//                    break;
//                case 4:
//                    isPaymentDone = true;
//                    switch (data.SupplierProducts[j].Product.ProductName) {
//                        case "FIT":
//                            btnFITCls = "btn-color";
//                            btnFITDisabled = evaluatedText;
//                            break;
//                        case "H&S":
//                            btnHSCls = "btn-color";
//                            btnHSDisabled = evaluatedText;
//                            break;
//                        case "DS":
//                            btnDSCls = "btn-color";
//                            btnDSDisabled = evaluatedText;
//                            break;
//                    }
//                    break;
//                case 5:
//                    isPaymentDone = true;
//                    switch (data.SupplierProducts[j].Product.ProductName) {
//                        case "FIT":
//                            btnFITCls = "btn-normal";
//                            btnFITDisabled = publishedText;
//                            break;
//                        case "H&S":
//                            btnHSCls = "btn-normal";
//                            btnHSDisabled = publishedText;
//                            break;
//                        case "DS":
//                            btnDSCls = "btn-normal";
//                            btnDSDisabled = publishedText;
//                            break;
//                    }
//                    break;
//            }
//            var productStatus = "";
//            switch (data.SupplierProducts[j].Status) {
//                case 0: productStatus = "Not Submitted"; break;
//                case 1: productStatus = "Awaiting payment"; break;
//                case 2: productStatus = "Submitted"; break;
//                case 3: productStatus = "Submitted"; break;
//                case 4: productStatus = "Checked"; break;
//                case 5: productStatus = "Published"; break;
//            }
//            switch (data.SupplierProducts[j].Product.ProductName) {
//                case "FIT":
//                    if (btnFITDisabled != "") {
//                        switch (productStatus) {
//                            case "Published":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + publishedText + "</td><td style='text-align: center'><a class='SICCodeLink QuesEvaluate FITPublish' style='cursor: pointer' data-pillar='FIT' data-companyId='" + data.CompanyId + "'>" + btnFITDisabled + "</a></td></tr>";
//                                break;
//                            case "Checked":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + checkedText + "</td><td style='text-align: center'><a class='SICCodeLink QuesEvaluate FITCheck' style='cursor: pointer' data-pillar='FIT' data-companyId='" + data.CompanyId + "'>" + productStatus + "</a></td></tr>";
//                                enablePublishButton = true;
//                                break;
//                            case "Submitted":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + productStatus + "</td><td style='text-align: center'><input type='button' style='width:80px' class='btn " + btnFITCls + " QuesEvaluate FITCheck' data-pillar='FIT' data-companyId='" + data.CompanyId + "' value=" + checkButton + " /></td></tr>";
//                                break;
//                            default:
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + productStatus + "</td><td style='text-align: center'>-</td></tr>";
//                                break;
//                        }
//                        break;
//                    }
//                case "DS":
//                    if (btnDSDisabled != "") {
//                        switch (productStatus) {
//                            case "Published":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + publishedText + "</td><td style='text-align: center'><a class='SICCodeLink QuesEvaluate DSPublish' style='cursor: pointer' data-pillar='DS' data-companyId='" + data.CompanyId + "'>" + btnDSDisabled + "</a></td></tr/>";
//                                break;
//                            case "Checked":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + checkedText + "</td><td style='text-align: center'><a class='SICCodeLink QuesEvaluate DSCheck' style='cursor: pointer' data-pillar='DS' data-companyId='" + data.CompanyId + "'>" + productStatus + "</a></td></tr>";
//                                enablePublishButton = true;
//                                break;
//                            case "Submitted":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + productStatus + "</td><td style='text-align: center'><input type='button' style='width:80px' class='btn " + btnDSCls + " QuesEvaluate DSCheck' data-pillar='DS' data-companyId='" + data.CompanyId + "' value=" + checkButton + " /></td></tr>";
//                                break;
//                            default:
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + productStatus + "</td><td style='text-align: center'>-</td></tr>";
//                                break;
//                        }
//                    }
//                    break;
//                case "H&S":
//                    if (btnHSDisabled != "") {
//                        switch (productStatus) {
//                            case "Published":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + publishedText + "</td><td style='text-align: center'><a class='SICCodeLink QuesEvaluate HSPublish' style='cursor: pointer' data-pillar='HS' data-companyId='" + data.CompanyId + "'>" + btnHSDisabled + "</a></td></tr>";
//                                break;
//                            case "Checked":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + checkedText + "</td><td style='text-align: center'><a class='SICCodeLink QuesEvaluate HSCheck' style='cursor: pointer' data-pillar='HS' data-companyId='" + data.CompanyId + "'>" + productStatus + "</a></td></tr>";
//                                enablePublishButton = true;
//                                break;
//                            case "Submitted":
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + productStatus + "</td><td style='text-align: center'><input type='button' style='width:80px' class='btn " + btnHSCls + " QuesEvaluate HSCheck' data-pillar='HS' data-companyId='" + data.CompanyId + "' value=" + checkButton + " /></td></tr>";
//                                break;
//                            default:
//                                output += "<td style='text-align: center'>" + data.SupplierProducts[j].Product.ProductName + "</td><td style='text-align: center'>" + productStatus + "</td><td style='text-align: center'>-</td></tr>";
//                                break;
//                        }
//                    }
//                    break;
//            }
//        }
//        if (btnDetailDisabled != "") {
//            if (btnDetailDisabled == checkedText) {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + detailsText + "</td><td style='text-align: center'>" + checkedText + "</td><td  style='text-align: center'><a class=\"SICCodeLink showReadOnlyDetailsResults\" data-companyId='" + data.CompanyId + "'>" + btnDetailDisabled + "</a></td></tr>";
//            }
//            else if (btnDetailDisabled == publishedText) {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + detailsText + "</td><td style='text-align: center'>" + publishedText + "</td><td  style='text-align: center'><a class=\"SICCodeLink showReadOnlyDetailsResults\" data-companyId='" + data.CompanyId + "'>" + btnDetailDisabled + "</a></td></tr>";
//            }
//            else {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + detailsText + "</td><td  style='text-align: center'>" + btnDetailDisabled + "</td><td style='text-align: center'>-</td></tr>";
//            }
//        }
//        else {
//            if (data.Status == 0) {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + detailsText + "</td><td style='text-align: center'>" + pendingText + "</td><td style='text-align: center'>-</td></tr>";
//            }
//            else {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + detailsText + "</td><td style='text-align: center'>" + pendingText + "</td><td style='text-align: center'><input type='button' style='width:80px' class='btn " + btnDetailCls + " detailVerify' data-companyId='" + data.CompanyId + "' value=" + checkButton + "  /></td></tr>";
//            }
//        }
//        if (btnProfileDisabled != "") {
//            if (btnProfileDisabled == publishedText) {
//                output += "<tr class=\"odd\"><td style='text-align: center'>" + profileText + "</td><td style='text-align: center'>" + publishedText + "</td><td  style='text-align: center'><a class=\"SICCodeLink showReadOnlyProfileResults\" data-companyId='" + data.CompanyId + "'>" + btnProfileDisabled + "</a></td></tr>";
//            }
//            else if (btnProfileDisabled == checkedText) {
//                output += "<tr class=\"odd\"><td style='text-align: center'>" + profileText + "</td><td style='text-align: center'>" + checkedText + "</td><td  style='text-align: center'><a class=\"SICCodeLink profileVerify\" data-companyId='" + data.CompanyId + "'>" + btnProfileDisabled + "</a></td></tr>";
//            }
//            else {
//                output += "<tr class=\"odd\"><td style='text-align: center'>" + profileText + "</td><td style='text-align: center'>" + btnProfileDisabled + "</td><td style='text-align: center'>-</td></tr>";
//            }
//        }
//        else {
//            if (data.Status == 0) {
//                output += "<tr class=\"odd\"><td style='text-align: center'>" + profileText + "</td><td style='text-align: center'>" + pendingText + "</td><td style='text-align: center'>-</td></tr>";
//            }
//            else {
//                output += "<tr class=\"odd\"><td style='text-align: center'>" + profileText + "</td><td style='text-align: center'>" + pendingText + "</td><td style='text-align: center'><input type='button' style='width:80px' class='btn " + btnProfileCls + " profileVerify' data-companyId='" + data.CompanyId + "' value=" + checkButton + " /></td></tr>";
//            }
//        }
//        if (btnSanctionDisabled != "") {
//            output += "<tr class=\"even\"><td style='text-align: center'>" + sanctionText + "</td><td style='text-align: center'>" + btnSanctionDisabled + "</td><td style='text-align: center'>-</td></tr>";
//        }
//        else {
//            if (data.Status == 0) {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + sanctionText + "</td><td style='text-align: center'>" + pendingText + "</td><td style='text-align: center'>-</td></tr>";
//            }
//            else {
//                output += "<tr class=\"even\"><td style='text-align: center'>" + sanctionText + "</td><td style='text-align: center'>" + pendingText + "</td><td style='text-align: center'><input type='button' style='width:80px' class='btn " + btnSanctionCls + " sanctionVerify' data-companyId='" + data.CompanyId + "' value=" + checkButton + " /></td></tr>";
//            }
//        }
//        $("#btnPublish").hide();
//        if (data.Status >= 3 && CanPublishSupplier) {
//            var button = "<button style='width:80px' data-companyId=\"" + data.CompanyId + "\"  class=\"btn btn-color publish-status-mail\">" + publishButton + "</button>";
//            $('#btnPublish').html(button);
//            if (data.IsPublished != true) {
//                $("#btnPublish").show();
//            }
//            if (enablePublishButton == true) {
//                $("#btnPublish").show();
//            }
//        }
//    }
//    else {
//        output = "<tr><td colspan=\"2\">" + noRecordsFound + "</td></tr>";
//        $('.verifySupplierPaginator').remove();
//    }
//    $('#supplier-verification-list-table-rows').html(output);
//    if (!CanSanctionCheck) {
//        $('.sanctionVerify').hide();
//    }
//    if (!CanDetailsCheck) {
//        $('.detailVerify').hide();
//    }
//    if (!CanViewDetailsCheck) {
//        $('.showReadOnlyDetailsResults').hide();
//    }
//    if (!CanProfileCheck) {
//        $('.profileVerify').hide();
//    }
//    if (!CanViewProfileCheck) {
//        $('.showReadOnlyProfileResults').hide();
//    }
//    if (!CanFITCheck) {
//        $('.FITCheck').hide();
//    }
//    if (!CanViewFITCheck) {
//        $('.FITPublish').hide()
//    }
//    if (!CanHSCheck) {
//        $('.HSCheck').hide();
//    }
//    if (!CanViewHSCheck) {
//        $('.HSPublish').hide();
//    }
//    if (!CanDSCheck) {
//        $('.DSCheck').hide();
//    }
//    if (!CanViewDSCheck) {
//        $('.DSPublish').hide();
//    }
//}


////function GetUsers(response) {
////    var row = "";
////    if (response.length > 0) {
////        for (i = 0; i < response.length; i++) {
////            var trCls = "even";
////            if (i % 2 == 0) {
////                trCls = "odd";
////            }

////                if (response[i].IsActive) {
////                    row += "<tr class=\"" + trCls + "\"><td>" + response[i].FirstName + " " + response[i].LastName + "</td><td>Active" +
////                        "</td><td  class=\"text-align-center\"><div class=\"btn-group pull-right\"  id=\"companySummaryDropdown\" > <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">Actions<span class=\"caret\"></span></button> <ul class=\"dropdown-menu\" ><li><a class=\"btnEditSupplierProfile-click\"  data-UserId-Profile=\"" + response[i].UserId + "\">Edit Profile</a></li><li><a class=\"btnEditSupplierPassword-click\"  data-UserId-Password=\"" + response[i].UserId + "\">Edit Password</a></li></ul></div></td></tr>";

////                }
////                else {

////                    row += "<tr class=\"" + trCls + "\"><td>" + response[i].FirstName + " " + response[i].LastName + "</td><td> Not Active" +
////                           "</td><td  class=\"text-align-center\"><div class=\"btn-group pull-right\" id=\"companySummaryDropdown\"> <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">Actions<span class=\"caret\"></span></button> <ul class=\"dropdown-menu\"><li><a class=\"btnEditSupplierProfile-click\"  data-UserId-Profile=\"" + response[i].UserId + "\">Edit Profile</a></li><li><a class=\"btnEditSupplierPassword-click\"  data-UserId-Password=\"" + response[i].UserId + "\">Edit Password</a></li></ul></div></td></tr>";

////                }


////        }
////    }
////    $("#supplier-users-table-rows").html(row);

////}

////function getParameterByName(name) {
////    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
////    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
////        results = regex.exec(location.search);
////    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
////}

//$(document).on('click', '.detailVerify', function () {
//    if (localStorage.CompanyName != "") {
//        $("#detailsCheckBreadCrumb").html("< " + backToText + " " + localStorage.CompanyName + " " + dashboardText);
//    }
//    GenerateDetailsCheckAuditorUIforSupplier();
//    var compnyId = $(this).attr('data-companyId');
//    var start = 1900;
//    var end = new Date().getFullYear();
//    var options = "";
//    options = "<option value=''>" + selectYearText + "</option>"
//    for (var year = end ; year >= start; year--) {
//        options += "<option value=\"" + year + "\">" + year + "</option>";
//    }
//    document.getElementById("CompanyYearDetailCheck").innerHTML = options;
//    GetSupplierDetailsInfoforSupplier(compnyId);
//    return false;
//});

//function GenerateDetailsCheckAuditorUIforSupplier() {
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
//                                     "<input type=\"checkbox\" name=\"details-status-not-checked-" + i + "\" value=\"" + i + "\" /><span style=\"padding-left:6px\"><b>" + checked + "</b></span></div>" +
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

//$(document).on('click', '.profileVerify', function () {
//    localStorage.setItem("BreadCrumb", "SupplierDashboard");
//    Common.IsLoadingNeeded = true;
//    var companyID = $(this).attr('data-companyId');
//    Common.IsNavigateAsynchronous = false;
//    Navigate('GetSupplierInformation', '/Admin/GetSupplierInformation', "Supplier Information", true);
//    GetSupplierInformationById(companyID);
//    return false;
//});



//$(document).on('click', '.sanctionVerify', function () {
//    Common.IsLoadingNeeded = true;
//    var compId = $(this).attr('data-companyId');
//    localStorage.SanctionCompanyId = compId;
//    localStorage.TempCompanyId = compId;
//    window.location.href = "/Sanction";
//    return false;
//});

//$(document).on('click', '.QuesEvaluate', function () {
//    var cmpnyID = $(this).attr('data-companyId');
//    localStorage.QuestnreCompId = cmpnyID;
//    var pillar = $(this).attr('data-pillar');
//    $.ajax({
//        type: "POST",
//        url: "/Admin/Evaluate",
//        data: { companyId: cmpnyID, pillar: pillar },
//        success: function (response) {

//            if (response && typeof (response) != "undefined") {
//                if (response == Common.LogoutAction) {
//                    Logout();
//                }
//                else {
//                    window.location = response;
//                    $("#companySummary").show();

//                }
//            }


//        },
//        error: function (response) {

//        }
//    });
//    return false;
//});

function GoToSupplierOrganisations() {
    localStorage.tempVal = "fromSupplierPage";
    window.location.href = "/Admin/SupplierOrganisations";
}

$(document).on('click', '.btnEditSupplierPrimaryContactDetailsDashboard', function () {
    $('#EditSupplierPrimaryDetailsDashboard').find('form')[0].reset();
    $('.field-validation-valid').html('');

    var userID = $(this).attr('data-UserId-Profile');
    $.ajax({
        type: 'post',
        url: '/Admin/EditUserProfile',
        data: { userId: userID },
        dataType: "json",
        success: function (data) {
            var items = data.users;
            $("#txtloginIDDashboard").val(items.LoginId);
            $("#txtPrimaryFirstNameDashboard").val(items.PrimaryFirstName);
            $("#txtPrimaryEmailDashboard").val(items.PrimaryEmail);
            $("#txtPrimaryLastNameDashboard").val(items.PrimaryLastName);
            $("#txtTelephoneDashboard").val(items.Telephone);
            $("#txtJobTitleDashboard").val(items.JobTitle);
            $("#hdnUserIdDashboard").val(items.UserId);
            $('#hdnCompIDDashboard').val(items.OrganizationPartyId);
            $('#hdnSupplierPrimaryContactPartyIdDashboard').val(items.PrimaryContactPartyId);
            $('#EditSupplierPrimaryDetailsDashboard').modal('show');
        },
        error: function (data) {
        }

    });
    return false;
});


$(document).on('click', '.btnEditSupplierUser', function () {
    var userID = $(this).attr('data-UserId-Profile');
    var BreadCrumb = "DashBoard";
    if (supplierPartyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/CreateUserForSupplierCompany',
            data: { supplierPartyId: supplierPartyId, BreadCrumb: BreadCrumb, UserId: userID },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDashBoardDivs();
                    $("#CreateUser").show();
                    $("#CreateUser").html(response);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    return false;
});

function ClearAllDashBoardDivs() {
    $("#SupplierDashboard").html("");
    $("#SupplierDashboard").hide();
}

function ChangePassword(userId, loginId) {
    $("#txtloginIDEditPassword").val(loginId);
    $("#hdnEditPasswordUserId").val(userId);
    $("#txtSupplierPassword").val("");
    $("#txtConfirmSupplierPassword").val("");
    $("#SupplierEditPassword").modal('show');
};

$(document).on('click', '#btnCancelSupplierPasswrd', function () {
    $('#SupplierEditPassword').find('form')[0].reset();
    $('.field-validation-valid').html('');
});

//$(document).on('click', '#btnCancelSupplierDetails', function () {
//    $('#SupplierEditProfile').find('form')[0].reset();
//    $('.field-validation-valid').html('');
//});

$(document).on('click', '#btnResetSupplierPrimaryDetails', function () {
    var logId = $('#txtloginID').val();
    $('#EditSupplierPrimaryDetailsDashboard').find('form')[0].reset();
    $('.field-validation-valid').html('');
    $('#txtloginID').val(logId);
    $("#txtPrimaryFirstName").val("");
    $("#txtPrimaryEmail").val("");
    $("#txtPrimaryLastName").val("");
    $("#txtTelephone").val("");
    $("#hdnUserId").val("");
    $('#hdnCompID').val("");
});

//$(document).on('click', '#btnResetSupplierDetails', function () {
//    $('#SupplierEditProfile').find('form')[0].reset();
//    $('.field-validation-valid').html('');
//});


$(document).on('click', '#btnCancelSupplierPrimaryDetails', function () {
    $('#EditSupplierPrimaryDetailsDashboard').find('form')[0].reset();
    $('.field-validation-valid').html('');
});



//$(document).on('click', '#btnResetSupplierPasswrd', function () {

//    $('#SupplierEditPassword').find('form')[0].reset();
//    $('.field-validation-valid').html('');
//    var val = $('#hdnUserId').val();
//    openSupplierPasswordPopUp(val);
//});

$(document).on('click', '#ChangeSupplierPassword', function () {
    $.validator.unobtrusive.parse($('#supplierPasswordChange'));
    if (!$('#supplierPasswordChange').valid()) {
        return false;
    }
    $.ajax({
        type: 'post',
        url: '/Admin/ChangeUserPassword',
        data: $('#supplierPasswordChange').serialize(),
        dataType: "json",

        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#SupplierEditPassword').modal('hide');
            }
            else
                showErrorMessage(response.message);
            GetSupplierUserDetailsForDashboard(1, "", 1);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;

});

//$(document).on('click', '#btnSaveSupplierDetails', function () {
//    $.validator.unobtrusive.parse($('#EditSupplierDetails'));
//    if (!$('#EditSupplierDetails').valid()) {
//        return false;
//        showErrorMessage(validDataError);
//    }
//    $.ajax({
//        type: 'post',
//        url: '/Admin/UpdateSupplierDetails',
//        data: $('#EditSupplierDetails').serialize(),
//        dataType: "json",

//        success: function (response) {
//            if (response) {
//                showSuccessMessage(response.message);
//                $('#SupplierEditProfile').modal('hide');

//                GetCompanySummary();
//            }
//            else
//                showErrorMessage(response.message);
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;

//});


$(document).on('click', '#btnSaveSupplierPrimaryDetails', function () {
    $.validator.unobtrusive.parse($('#supplierPrimaryContactDetailsDashboard'));
    if (!$('#supplierPrimaryContactDetailsDashboard').valid()) {
        return false;
        showErrorMessage(validDataError);
    }
    $.ajax({
        type: 'post',
        url: '/Admin/UpdateUserProfile',
        data: $('#supplierPrimaryContactDetailsDashboard').serialize(),
        dataType: "json",
        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#EditSupplierPrimaryDetailsDashboard').modal('hide');
                GetCompanySummary();
            }
            else
                showErrorMessage(response.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
});


//function GetSupplierInformationById(companyId) {
//    localStorage.breadCrumb = companyId;
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

//function GetSupplierDetailsInfoforSupplier(compnyId) {

//    $.ajax({
//        type: 'post',
//        url: '/Admin/SupplierDetailsPrimaryCheck',
//        data: { companyId: compnyId },
//        async: false,
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                var data = response.model;
//                var answers = response.answers;
//                $('#companySummary').hide();
//                $('#tabDetailsVerify').show();

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
//                //FillSupplierData(data);
//                AddRequiredIcons();
//                $('#organisationNameDetailCheck').attr('readonly', true);
//                $('#organisationNameDetailCheck').val(data.OrganisationName);
//                $("#DetailCheckSupplierForm input[name=sector][value='" + data.sector + "']").prop("checked", true);
//                $('#TradingDetailCheck').val(data.Trading);
//                $('#TypeOfCompanyDetailCheck').val(data.TypeOfCompany);
//                $('#CompanyYearDetailCheck').val(data.CompanyYear);
//                //For Not Sure
//                if (data.sector == 485) {
//                    //$("#hdnBusinessSectorDescription").val(data.BusinessSectorDescription)
//                    $("#DetailCheckSupplierForm input[name=sector][value='" + 485 + "']").parent().show();
//                    $("#spnBusinessSectorDescriptionDetailCheck").html(data.BusinessSectorDescription).prop("readonly", true);
//                    $("#divsectorDescriptionDetailCheck").show();
//                }
//                else {
//                    $("#DetailCheckSupplierForm [name=sector][value='" + 485 + "']").parent().hide();
//                    $("#spnBusinessSectorDescriptionDetailCheck").html("");
//                    $("#divsectorDescriptionDetailCheck").hide();
//                }
//                if (data.IsVAT) {
//                    $("#DetailCheckSupplierForm [name=IsVAT][value='True']").prop("checked", true);
//                    $('#HaveVATDetailCheck').show();
//                    $('#vatNumberDetailCheck').val(data.VATNumber);
//                    $('#DetailIsVATAnswerdiv').hide();
//                }
//                else {
//                    $("#DetailCheckSupplierForm input[name=IsVAT][value='False']").prop("checked", true);
//                    $('#vatNumberDetailCheck').val('');
//                    $('#HaveVATDetailCheck').hide();
//                    $('#DetailIsVATAnswerdiv').show();
//                }
//                if (data.HaveDuns) {
//                    $("#DetailCheckSupplierForm input[name=HaveDuns][value='True']").prop("checked", true);
//                    $('#HaveDUNSDetailCheck').show();
//                    $('#DUNSNumberDetailCheck').val(data.DUNSNumber);
//                    $('#DetailCheckHaveDunsAnswerdiv').hide();
//                }
//                else {
//                    $("#DetailCheckSupplierForm input[name=HaveDuns][value='False']").prop("checked", true);
//                    $('#HaveDUNSDetailCheck').hide();
//                    $('#DUNSNumberDetailCheck').val('');
//                    $('#DetailCheckHaveDunsAnswerdiv').show();
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
//                $('#firstAddressLine1DetailCheck').val(data.FirstAddressLine1);
//                $('#firstAddressLine2DetailCheck').val(data.FirstAddressLine2);
//                $('#firstAddressCityDetailCheck').val(data.FirstAddressCity);
//                $('#firstAddressStateDetailCheck').val(data.FirstAddressState);
//                $('#firstAddressPostalCodeDetailCheck').val(data.FirstAddressPostalCode);
//                $('#firstAddressCountryDetailCheck').val(data.FirstAddressCountry);
//                $('#companyIdDetailCheck').val(data.CompanyId);
//                $('#userIdDetailCheck').val(data.UserId);
//                $('#regNumberDetailCheck').val(data.CompanyRegistrationNumber);
//                //if(data)
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//}


$('#btnShowAddUserForm').click(function () {
    var UserId = 0;
    var BreadCrumb = "DashBoard";
    if (supplierPartyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/CreateUserForSupplierCompany',
            data: { supplierPartyId: supplierPartyId, BreadCrumb: BreadCrumb, UserId: UserId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDashBoardDivs();
                    $("#CreateUser").show();
                    $("#CreateUser").html(response);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
})


////$('#btnAddReferrer').click(function () {
////    $('#SearchSupplierReferrer')[0].reset();
////    $("#hdn-add-supplier-referrer-details-page").val(1);
////    PagerLinkClick('1', "SearchSupplierReferrerData", "#hdn-supplier-add-referrer-details-page", "", 1);
////})


////function SearchSupplierReferrerData(pageNo, sortParameter, sortDirection) {
////    $('#SupplierAddReferrer').modal('show');
////    var buyerName = $('#txtBuyerName').val();
////    var campName = $('#txtCampaignName').val();
////    var suppId = $('#hdnCompanyId').val();
////    $.ajax({
////        type: 'post',
////        url: '/Admin/GetBuyerCampaignDetails',
////        data: { pageNo: pageNo, BuyerName: buyerName, CampaignName: campName, SupplierId: suppId },
////        dataType: "json",
////        success: function (response) {
////            var row = "";
////            var count = 0;
////            var total = 0;
////            if (response.users.length > 0) {
////                for (i = 0; i < response.users.length; i++) {
////                    var items = response.users[i];
////                    var uName = items.CampaignId;
////                    var trCls = "odd";
////                    if (i % 2 == 0) {
////                        trCls = "even";
////                    }

////                    if (items.IsSupplierReferred != true) {
////                        row += "<tr class=\"" + trCls + "\"><td>" + items.CompanyName + "</td><td>" + items.CampaignName + "</td><td style=\"text-align: center\"><button class=\"btn btn-color add-supplier-referrer-click\" data-CampaignId=\"" + uName + "\" style=\"width:100px\" >Add</button>\n";
////                        row += "<input class=\"chkBox assign-supplier-primary-referrer\"  type=\"checkbox\"   data-id=\"" + response.users[i].CampaignId + "\" /></tr>";
////                        count++;


////                    }
////                    $("#supplier-referrer-table").html(row);
////                }
////                if (count == 0) {
////                    $("#supplier-referrer-table").html("<tr><td colspan='3'>No records found.</td></tr>");
////                }
////            }
////            else {
////                $('#supplier-referrer-table').html("<tr><td colspan='3'>No records found.</td></tr>");
////            }
////            if (response.hasRefferer) {
////                $('.chkBox').attr('disabled', true);
////            }
////            else {
////                $('.chkBox').removeAttr('disabled');

////            }

////            //$('.addSupplierReferrerPaginator').html(displayLinks($('#hdn-supplier-add-referrer-details-page').val(), Math.ceil(response.total / 2), sortParameter, sortDirection, "SearchSupplierReferrerData", "#hdn-supplier-add-referrer-details-page"));
////            //if (response.total <= 2) {
////            //    $('.addSupplierReferrerPaginator').css('margin-right', '0px');
////            //}
////        },
////        error: function (result) {

////        }
////    })
////    return false;

////}

////$(document).on('click', '.add-supplier-referrer-click', function () {
////    var supplierId = $('#hdnCompanyId').val();
////    var campaignId = $(this).attr('data-CampaignId');
////    $.ajax({
////        async: false,
////        type: 'post',
////        url: '/Admin/UpdateSupplierStatusValues',
////        data: { supplierId: supplierId, campaignId: campaignId },
////        dataType: "json",
////        success: function (response) {
////            if (response) {
////                showSuccessMessage(response.message);
////                $('#SupplierAddReferrer').modal('hide');
////                GetCompanySummary();
////            }
////            else
////                showErrorMessage(response.message);
////        },
////        error: function (jqXHR, textStatus, errorThrown) {
////        }
////    });
////    return false;
////});


////$(document).on('click', '.assign-supplier-primary-referrer', function () {
////    var supId = $('#hdnCompanyId').val();
////    var campaignId = $(this).attr('data-id');
////    $.ajax({
////        async: false,
////        type: 'post',
////        url: '/Admin/AddLandingPageReferrer',
////        data: { supplierId: supId, campaignId: campaignId },
////        dataType: "json",

////        success: function (response) {
////            if (response) {
////                showSuccessMessage(response.message);
////                $('#SupplierAddReferrer').modal('hide');
////                GetCompanySummary();
////            }
////            else
////                showErrorMessage(response.message);
////        },
////        error: function (jqXHR, textStatus, errorThrown) {
////        }
////    });
////    return false;
////});

////$(document).on('click', '#btnSearchReferrer', function () {
////    SearchSupplierReferrerData(1, "", "");
////    return false;
////})

//$('#userForm [name=LoginId]').blur(function () {
//    var logId = $('#LoginId').val();
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetLoginId',
//        data: { loginId: logId },
//        dataType: "json",
//        async: false,
//        success: function (response) {
//            if (response.id == logId) {
//                showErrorMessage(response.message);
//                //  $('#LoginId').focus();
//                $("#btnAddUser").prop('disabled', true);

//            }
//            else {
//                $('#EmailError').removeClass('error-text');
//                $('#EmailError').addClass('available');

//                $("#btnAddUser").prop('disabled', false);
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }

//    });
//    return false;
//});

//$('.addOrEditUser').click(function () {

//    $.validator.unobtrusive.parse($('#userForm'));
//    if (!$('#userForm').valid()) {
//        return false;
//    }

//    info = $('#userForm').serialize();


//    $.ajax({
//        type: 'post',
//        url: '/Supplier/AddUser',
//        data: info,
//        async: true,
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                if (response) {

//                    if (response.success == true || response.success == false) {
//                        (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.messge);
//                    }
//                    if (!(response.isUpdate)) {
//                        $('#userForm')[0].reset();
//                    }


//                    $("#AddUsers").hide();
//                    $("#addUserBreadCrumb").hide();
//                    $('#companySummary').show();
//                    GetSupplierUserDetailsForDashboard(1, "", 1);
//                    $('#tabDetailsVerify').hide();

//                }
//                else {
//                    showErrorMessage(insertionError);
//                }
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//    return false;
//});

//$(document).on('click', '.btnCancelAddUser', function () {
//    $("#AddUsers").hide();
//    $("#addUserBreadCrumb").hide();
//    $('#companySummary').show();
//    GetCompanySummary();
//    $('#tabDetailsVerify').hide();

//});

//function BackToSupplierDetails() {
//    $("#AddUsers").hide();
//    $("#addUserBreadCrumb").hide();
//    $('#companySummary').show();
//    GetCompanySummary();
//    $('#tabDetailsVerify').hide();
//}



////$(document).on('click', '.btnDeleteUser-Click', function () {

////    var userId = $(this).attr('data-UserId-Delete');
////    if (userId != '') {
////        $.ajax({
////            type: "POST",
////            data: { userId: userId },
////            url: "/Supplier/DeleteUser",
////            success: function (response) {

////                if (response) {
////                    (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.messge);
////                    GetCompanySummary();
////                }
////            },
////            error: function (response) { }
////        });
////    }
////});


////$(document).on('click', '.btnRemoveReferrer-click', function () {

////    var supplierId = $('#hdnCompanyId').val();
////    var campaignId = $(this).attr('data-RemoveId');
////    $.ajax({
////        async: false,
////        type: 'post',
////        url: '/Admin/DeleteReferrer',
////        data: { supplierId: supplierId, campaignId: campaignId },
////        dataType: "json",

////        success: function (response) {
////            if (response) {
////                showSuccessMessage(response.message);
////                GetCompanySummary();

////            }
////            else
////                showErrorMessage(response.message);
////        },
////        error: function (jqXHR, textStatus, errorThrown) {
////        }
////    });
////    return false;
////});

//$('#btnCancelDetailCheck').click(function () {
//    $('#tabDetailsVerify').hide();
//    $('#companySummary').show();
//    GetCompanySummary();
//    $('html, body').animate({
//        scrollTop: 0
//    }, 'slow');
//});


//function NavigateBackToSupplier() {
//    $('#companySummary').show();
//    GetCompanySummary();
//    $('#tabDetailsVerify').hide();
//    return false;
//}

//$(document).on('click', '.showReadOnlyDetailsResults', function () {
//    if (localStorage.CompanyName != "") {
//        $("#detailsCheckBreadCrumb").html("< " + backToText + " " + localStorage.CompanyName + " " + dashboardText);
//    }
//    GenerateDetailsCheckAuditorUIforSupplier();
//    var companyId = $(this).attr('data-companyId');
//    GetSupplierDetailsInfoforSupplier(companyId);
//    $('#DetailCheckSupplierForm input,#DetailCheckSupplierForm select,#DetailCheckSupplierForm textarea').prop('disabled', true);
//    $('#btnCancelDetailCheck').hide();
//    $('#btnVerifyDetailCheck').hide();
//    return false;
//});


//$('#btnVerifyDetailCheck').click(function () {

//    var companyId = $('#companyIdDetailCheck').val();
//    var notSure = $("#rdoNotSureDetail").prop('checked');
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
//            data: $('#DetailCheckSupplierForm').serialize(),
//            async: false,
//            success: function (data) {
//                if (typeof (data) != "undefined") {
//                    if (data.result) {
//                        $('#companySummary').show();
//                        $('#tabDetailsVerify').hide();
//                        $('#TradingDetailCheck').val('');
//                        $('#TypeOfCompanyDetailCheck').val('');
//                        $('#CompanyYearDetailCheck').val('');
//                        $('#organisationNameDetailCheck').val("");
//                        $('#firstAddressLine1DetailCheck').val("");
//                        $('#firstAddressLine2DetailCheck').val("");
//                        $('#firstAddressCityDetailCheck').val("");
//                        $('#firstAddressStateDetailCheck').val("");
//                        $('#firstAddressPostalCodeDetailCheck').val("");
//                        $('#firstAddressCountryDetailCheck').val("");
//                        $('#companyIdDetailCheck').val("");
//                        $('#regNumberDetailCheck').val("");
//                        $("#DetailCheckSupplierForm").find("input[name=isVAT][value='False']").prop("checked", true);
//                        $('#vatNumberDetailCheck').val('');
//                        $('#HaveVATDetailCheck').hide();
//                        $('#DetailIsVATAnswerdiv').show();
//                        GetCompanySummary();
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


//function InitialProfileAnswersValidation() {
//    //Details check count
//    var detailsCheckQuestionCount = 6;
//    for (var i = 1; i <= detailsCheckQuestionCount; i++) {
//        var QuestionId = i;
//        var needed = true;
//        switch (QuestionId) {
//            case 3:
//                if ($('#DetailCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "True") {
//                    needed = false;
//                }
//                break;
//            case 4:
//                if ($('#DetailCheckSupplierForm').find('input[name=IsVAT]:Checked').val() != "True") {
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
//        var CompanyId = $('#companyIdDetailCheck').val();
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
//                if ($('#DetailCheckSupplierForm').find('input[name=HaveDuns]:Checked').val() == "True") {
//                    NotNeeded = true;
//                }
//                break;
//            case 14:
//                if ($('#DetailCheckSupplierForm').find('input[name=HaveDuns]:Checked').val() != "True") {
//                    NotNeeded = true;
//                }
//                break;
//            case 3:
//                if ($('#DetailCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "True") {
//                    NotNeeded = true;
//                }
//                break;
//            case 4:
//                if ($('#DetailCheckSupplierForm').find('input[name=IsVAT]:Checked').val() != "True") {
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

//$(document).on('click', '.showReadOnlyProfileResults', function () {
//    localStorage.setItem("BreadCrumb", "SupplierDashboard");
//    Common.IsLoadingNeeded = true;
//    localStorage.IsVerified = true;

//    var companyId = $(this).attr('data-companyId');
//    Common.IsNavigateAsynchronous = false;
//    Navigate('GetSupplierInformation', '/Admin/GetSupplierInformation', "Supplier Information", true);
//    GetSupplierInformationById(companyId);
//    return false;
//});

//$(document).on('click', '.publish-status-mail', function () {
//    var companyId = $(this).attr('data-companyId');
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetSupplierProductStatus',
//        data: { companyId: companyId },
//        async: true,
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                if (response.length > 0) {
//                    var html = "";
//                    for (var i = 0; i < response.length; i++) {
//                        var cls = "odd";
//                        if (i % 2 == 0) {
//                            cls = "even";
//                        }
//                        if (!response[i].IsPublished) {
//                            html += "<tr class=\"" + cls + "\"><td>" + response[i].ProductName + "</td>"
//                            html += "<td style=\"text-align:center\"><input class=\"product-publish-select\" type=\"checkbox\" data-productId=\"" + response[i].ProductId + "\" data-supplierId=\"" + response[i].SupplierId + "\"/></td></tr>";
//                        }
//                    }
//                    if (html == "") {
//                        html = "<tr><td>" + noRecordsFound + "</td></tr>";
//                    }
//                    $('#publish-product-pop-up-body').html(html);
//                    $('#publish-product-pop-up').modal('show');
//                }
//                else {
//                    var html = "<tr><td>" + noRecordsFound + "</td></tr>";
//                    $('#publish-product-pop-up-body').html(html);
//                    $('#publish-product-pop-up').modal('show');
//                }
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//})

//function PublishAndSendReportMails() {
//    var publishList = [];
//    var checkedboxes = $('.product-publish-select:checkbox:checked');
//    var companyId = 0;
//    for (var i = 0 ; i < checkedboxes.length; i++) {
//        var item = $(checkedboxes[i]);
//        var productId = item.attr('data-productId');
//        var supplierId = item.attr('data-supplierId');
//        companyId = supplierId;
//        var item = { SupplierId: supplierId, ProductId: productId, IsPublished: false };
//        publishList.push(item);
//    }
//    if (publishList.length > 0) {
//        publishList = JSON.stringify({ 'publishList': publishList });
//        $.ajax({
//            type: 'post',
//            url: '/Admin/PublishAndSendReportMails',
//            data: publishList,
//            contentType: 'application/json; charset=utf-8',
//            async: true,
//            success: function (response) {
//                if (typeof (response) != "undefined") {
//                    if (response) {
//                        showSuccessMessage(publishedSuccessMessage);
//                        GetCompanySummary();
//                    }
//                    else {
//                        showErrorMessage(errorMessage);
//                    }
//                    $('#publish-product-pop-up').modal('hide');
//                }
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//            }
//        });
//    }
//    else {
//        showErrorMessage(publishProductsValidation);
//    }
//}


//function detailCheckValidation() {

//    var result = true;
//    RemoveBorderColor('#vatNumberDetailCheck');

//    $('.error-text').hide();
//    var setFocus = false;
//    $.validator.unobtrusive.parse($('#DetailCheckSupplierForm'));
//    if (!$('#DetailCheckSupplierForm').valid()) {
//        result = false;
//    }
//    if (!result) {
//        var els = document.querySelector('.input-validation-error');
//        if (els != null) {
//            els.focus();
//        }
//    }
//    if ($('#DetailCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "True") {
//        if ($('#vatNumberDetailCheck').val() == "") {
//            SetBorderColor("#vatNumberDetailCheck", "red");
//            $('#vatNumberDetailCheck').show();
//            if (!setFocus) {
//                $('#vatNumberDetailCheck').focus();
//                setFocus = true;
//            }
//            showErrorMessage(vatNumberValidation);
//            result = false;
//        }

//    }
//    else if ($('#DetailCheckSupplierForm').find('input[name=IsVAT]:Checked').val() == "False") {
//        $('#vatNumberDetailCheck').val('');
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


//$('#DetailCheckSupplierForm').on('click', 'input[name=IsVAT]', function (e) {
//    if ($(this).attr("value") == "True") {
//        $('#HaveVATDetailCheck').show();
//        $('#DetailIsVATAnswerdiv').hide();
//    }
//    if ($(this).attr("value") == "False") {
//        $('#HaveVATDetailCheck').hide();
//        $('#vatNumberDetailCheck').val('');
//        $('#DetailIsVATAnswerdiv').show();
//    }
//    return true;
//});

//$('#DetailCheckSupplierForm').on('click', 'input[name=HaveDuns]', function (e) {
//    if ($(this).attr("value") == "True") {
//        $('#HaveDUNSDetailCheck').show();
//        $('#DetailCheckHaveDunsAnswerdiv').hide();
//    }
//    if ($(this).attr("value") == "False") {
//        $('#HaveDUNSDetailCheck').hide();
//        $('#DetailCheckHaveDunsAnswerdiv').show();
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

//$(document).on('click', '.btnRemoveBuyer-click', function () {
//    var supplierId = $('#hdnCompanyId').val();
//    var buyerId = $(this).attr('data-RemoveId');
//    $.ajax({
//        type: 'post',
//        url: '/Admin/AddRemoveBuyerSupplierMapping',
//        data: { buyerId: buyerId, supplierId: supplierId, isDelete: true },
//        async: true,
//        success: function (response) {
//            if (typeof (response) != "undefined") {

//                if (response.result == true) {
//                    showSuccessMessage(response.message);
//                }
//                else {
//                    showErrorMessage(response.message);
//                }
//                GetBuyersMappedToSuppliers(1, "", 1);
//            }

//        }
//    });
//});


$(document).on('click', '#view-supplier-profile', function () {
    localStorage.setItem("profileParentNavigate", "SupplierDashboard");
    var url = "/Profile/" + $('#hdnCompanyId').val();
    window.location.href = url;
});