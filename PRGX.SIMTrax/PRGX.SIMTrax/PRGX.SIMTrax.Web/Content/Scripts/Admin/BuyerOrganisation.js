$(document).ready(function () {
    CheckPermission("Buyers", function () {

        $("#buyer-to-date").datepicker({
            changeYear: true,
            changeMonth: true,
            constrainInput: false,
            //minDate: "+0",
            maxDate: "+0",
            defaultDate: "+0",
            dateFormat: "dd-M-yy"
        }).css({ "cursor": "default" }).keydown(function () {
        });

        $("#buyer-from-date").datepicker({
            changeYear: true,
            changeMonth: true,
            constrainInput: false,
            maxDate: "+0",
            defaultDate: "+0",
            dateFormat: "dd-M-yy"
        }).css({ "cursor": "default" }).keydown(function () {
        });

        $("buyerName").intellisense({
            url: '/Common/GetBuyerName',
            actionOnEnter: SearchOnEnter
        });

        if (localStorage.BuyerNameFromBuyerHomeSearch != undefined && localStorage.BuyerNameFromBuyerHomeSearch != "") {
            $("#buyerName").val(localStorage.BuyerNameFromBuyerHomeSearch);
            localStorage.removeItem("BuyerNameFromBuyerHomeSearch");
        }

        setLocation("BuyerOrganisationsTab");

        if (localStorage.BuyerIdFromHomeSearch != undefined && localStorage.BuyerIdFromHomeSearch != "") {
            ShowBuyerDashBoard(localStorage.BuyerIdFromHomeSearch);
            localStorage.removeItem("BuyerIdFromHomeSearch");
        }
        else {
            BuyerOrganisationList(1, "CreatedDate", 3);
            GetBuyerAccessList();
        }
    });
});

function SearchOnEnter() {
    $('#hdnCurrentPageBuyerOrganisation').val(1);
    var sortDirection = $(".buyerNameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    PagerLinkClick('1', "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation", sortParameter, sortDirection);
}


function BuyerOrganisationList(currentPage, sortParameter, sortDirection) {
    var startdate = new Date($('#buyer-from-date').val());
    var endDate = new Date($('#buyer-to-date').val());
    var buyerName = $("#buyerName").val();
    if ($('#searchUsingAccess').val() == null)
        $('#searchUsingAccess').val(1);
    var pageSizeBuyerOrganisation = parseInt($("#pageSizeBuyerOrganisation").val());
    if (startdate > endDate) {
        showErrorMessage(dateValidation);
        return false;
    }
    $.ajax({
        cache: false,
        type: 'post',
        url: '/Admin/GetBuyerOrganisations',
        data: { pageSize: pageSizeBuyerOrganisation, currentPage: currentPage, sortDirection: sortDirection, sortParameter: sortParameter, firstName: buyerName, status: parseInt($('#searchUsingStatus').val()), fromDate: $('#buyer-from-date').val(), toDate: $('#buyer-to-date').val(), buyerAccess: parseInt($('#searchUsingAccess').val()) },
        success: function (data) {
            $('#buyer-organisation-table-body').html("");
            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var options = "";
                    var select = "";
                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    if (data.result[i].BuyerRoleName == "") {
                        select = noneSelectedText;
                    }
                    else {
                        select = data.result[i].BuyerRoleName;
                    }
                    var actiondropdown = "";
                    if (data.result[i].VerifiedDate == null && CanVerifyBuyer) {
                        actiondropdown = actiondropdown + "<li class='CompleteVerification' value='" + data.result[i].BuyerPartyId + "'><a>" + verifyBuyerButton + "</a></li>";
                    }
                    if (data.result[i].ActivatedDate == null && data.result[i].VerifiedDate != null && CanActivateBuyer) {
                        actiondropdown = actiondropdown + "<li class='CompleteActivation' value='" + data.result[i].BuyerPartyId + "' data-companyName='" + data.result[i].BuyerOrganizationName + "'><a>" + activateBuyerButton + "</a></li>";
                    }
                    if (data.result[i].ActivatedDate != null && CanChangeAccessType) {
                        actiondropdown = actiondropdown + "<li class='ChangeAccessType' value='" + data.result[i].BuyerPartyId + "' data-companyName='" + data.result[i].BuyerOrganizationName + "'  data-accessType='" + data.result[i].BuyerRoleId + "'><a>" + changeAccessTypeButton + "</a></li>";
                    }
                    if (CanMapBuyerSupplier || CanModifyCampaign || CanCreateVoucher || CanAssignDefualtProduct || CanBuyerSupplierAssignProduct) {
                        actiondropdown = actiondropdown +
                            //"<li class='liBuyerSupplierMapping' value='" + data.result[i].BuyerPartyId + "'><a>" + buyerSupplierMapping + "</a></li>"+
                            "<li class='CreateCampaign' value='" + data.result[i].BuyerPartyId + "'><a>" + createCampaignButton + "</a></li><li class='CreateVoucher' value='" + data.result[i].BuyerPartyId + "'><a>" + createVoucherButton + "</a></li>";
                        //"<li class='AssignProduct' value='" + data.result[i].BuyerPartyId + "' onclick='GetProducts(" + data.result[i].BuyerPartyId + ")'><a>" + assignProductButton + "</a></li><li class='BuyerSupplierProductMapping' onclick='ExportSupplierList(" + data.result[i].BuyerPartyId + ",\"" + data.result[i].BuyerOrganizationName + "\")'><a>" + exportSuppList + "</a></li>";
                    }
                    var tableRow = "<tr class=" + trClass
                        + "><td class= 'buyer-name-ellipsis'><a class='buyerdashboard hyperlink' value='" + data.result[i].BuyerPartyId + "'>" + data.result[i].BuyerOrganizationName
                        + "</a></td><td>" + data.result[i].BuyerStatusString
                        + "</td><td>" + data.result[i].PrimaryContact
                        + "</td><td>" + data.result[i].PrimaryEmail
                        + "</td><td class=\"text-align-right\">" + data.result[i].CreatedDateString
                        + "</td><td class=\"text-align-right\">" + data.result[i].TermsAcceptedDateString
                        + "</td><td class=\"text-align-right\">" + data.result[i].VerifiedDateString
                        + "</td><td class=\"text-align-right\">" + data.result[i].ActivatedDateString
                        + "</td><td id='accesscellof" + data.result[i].BuyerPartyId + "'>" + select
                        + "</td><td id='actioncellof" + data.result[i].BuyerPartyId + "' class=\"text-align-center\">";
                    if (actiondropdown != "") {
                        tableRow += "<div class='btn-group' style='width: 150px; margin:auto;'><button type='button' class='btn btn-color dropdown-toggle' style='width: 150px;' data-toggle='dropdown' id='buyer-organisation-actions'>" + actionsButton + " <span class='caret'></span></button><ul class='dropdown-menu' style=' margin-left: -43px; min-width:150px !important;'>" + actiondropdown + "</ul></div>";
                    }
                    else {
                        tableRow += "-";
                    }
                    tableRow += "</td></tr>";
                    $('#buyer-organisation-table-body').append(tableRow);
                    $('#buyer-organisation-table').footable();
                    $('#buyer-organisation-table').trigger('footable_redraw');
                    if (!CanCreateVoucher) {
                        $('.CreateVoucher').hide();
                    }
                    if (!CanVerifyBuyer) {
                        $('.CompleteVerification').hide();
                    }
                    if (!CanActivateBuyer) {
                        $('.CompleteActivation').hide();
                    }
                    if (!CanModifyCampaign) {
                        $('.CreateCampaign').hide();
                    }
                    if (!CanChangeAccessType) {
                        $('.ChangeAccessType').hide();
                    }
                    if (!CanMapBuyerSupplier) {
                        $('.liBuyerSupplierMapping').hide();
                    }
                    if (!CanAssignDefualtProduct) {
                        $('.AssignProduct').hide();
                    }
                    if (!CanBuyerSupplierAssignProduct) {
                        $('.BuyerSupplierProductMapping').hide();
                    }
                }
            }
            else {
                $('#buyer-organisation-table-body').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
            }
            $('.buyerOrganisationPaginator').html(displayLinks($('#hdnCurrentPageBuyerOrganisation').val(), Math.ceil(data.total / pageSizeBuyerOrganisation), sortParameter, sortDirection, "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation"));
            var contentHtml = "";
            var currentPage = parseInt($('#hdnCurrentPageBuyerOrganisation').val());
            var lastPage = Math.ceil(data.total / pageSizeBuyerOrganisation);
            if (data.result.length > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeBuyerOrganisation) + 1), (pageSizeBuyerOrganisation * currentPage), data.total);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeBuyerOrganisation) + 1), data.total, data.total);
                }
            }
            ScrollToTop();
            $('#search-page-data-buyerOrganisation').html(contentHtml);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

function GetBuyerAccessList() {
    $.ajax({
        async: false,
        type: 'post',
        url: '/Admin/GetBuyerAccessList',
        success: function (data) {
            var options = "<option value=\"0\">" + allText + "</option>";
            for (var j = 0 ; j < data.accessList.length; j++) {
                options += "<option value=\"" + data.accessList[j].Id + "\">" + data.accessList[j].Name + "</option>";
            }
            $('#searchUsingAccess').html(options);
        }
    })
}
$('#pageSizeBuyerOrganisation').change(function () {
    $("#hdnCurrentPageBuyerOrganisation").val(1);
    var sortDirection = $('.buyerNameSort').attr('data-sortdirection');
    var sortParameter = "CreatedDate";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    PagerLinkClick('1', "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation", sortParameter, sortDirection);
});

$(document).on('click', '#btn-buyer-organisation-search', function () {
    $('#hdnCurrentPageBuyerOrganisation').val(1);
    var sortDirection = $('.buyerNameSort').attr('data-sortdirection');
    var sortParameter = "CreatedDate";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    BuyerOrganisationList(1, sortParameter, sortDirection);
});

$(document).on('click', '.CompleteVerification', function () {
    $("#Loading").show();
    var buyerPartyId = $(this).val();
    $.ajax({
        type: 'post',
        url: '/Admin/GetBuyerInformationForVerification',
        data: { buyerPartyId: buyerPartyId },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                ClearAllDivs();
                $("#Loading").hide();
                $("#VerifyBuyer").html(response);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});

$(document).on('click', '.CreateVoucher', function () {
    $("#Loading").show();
    var buyerPartyId = $(this).val();
    var BreadCrumb = "OrganisationPage";
    $.ajax({
        type: 'post',
        url: '/Admin/CreateOrUpdateVoucherForBuyerCompany',
        data: { buyerPartyId: buyerPartyId, BreadCrumb: BreadCrumb },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                ClearAllDivs();
                $("#CreateVoucher").html(response);
                $("#CreateVoucher").show();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});
$(document).on('click', '.ChangeAccessType', function () {
    $("#AssignOrUpdate").val(1);
    $("#companyNameAccess").html($(this).attr("data-companyName"));
    $("#companyIdAccess").val($(this).val());
    $("#modelHeadingAccess").html(changeAccessText);
    $("#CompleteActivationProcess").html(updateText);
    GetModalPopUpForAccessType();
    $("#modal-buyer-assigned-role").val($(this).attr("data-accessType"));
});

$(document).on('click', '.CompleteActivation', function () {
    $("#AssignOrUpdate").val(0);
    $("#companyNameAccess").html($(this).attr("data-companyName"));
    $("#companyIdAccess").val($(this).val());
    $("#modelHeadingAccess").html(assignAccessText);
    $("#CompleteActivationProcess").html(activateText);
    GetModalPopUpForAccessType();
});


function GetModalPopUpForAccessType() {
    $("#assignAccessToBuyer").modal('show');
    $.ajax({
        async: false,
        type: 'post',
        url: '/Admin/GetBuyerAccessList',
        success: function (data) {
            var options = "<option value=\"0\">--- Select  ---</option>";
            for (var j = 0 ; j < data.accessList.length; j++) {
                options += "<option value=\"" + data.accessList[j].Id + "\">" + data.accessList[j].Name + "</option>";
            }
            $('#modal-buyer-assigned-role').html(options);
        }
    });
}

$(document).on('click', '#CompleteActivationProcess', function () {
    var companyId = $("#companyIdAccess").val();
    var roleId = parseInt($("#modal-buyer-assigned-role").val());
    if (roleId == 0) {
        showErrorMessage(buyerAccessValidation);
        return false;
    }
    if (companyId != "" && $("#AssignOrUpdate").val() == 0) {
        $.ajax({
            type: 'post',
            url: '/Admin/ActivateBuyer',
            data: { buyerPartyId: companyId, roleId: roleId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.success)
                        showSuccessMessage(response.message);
                    else
                        showErrorMessage(response.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    else if (companyId != "" && $("#AssignOrUpdate").val() == 1) {
        $.ajax({
            type: 'post',
            url: '/Admin/ChangeBuyerAccessType',
            data: { buyerPartyId: companyId, roleId: roleId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.success)
                        showSuccessMessage(response.message);
                    else
                        showErrorMessage(response.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    $("#assignAccessToBuyer").modal('toggle');
    var sortDirection = $('.buyerNameSort').attr('data-sortdirection');
    var sortParameter = "CreatedDate";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    BuyerOrganisationList($('#hdnCurrentPageBuyerOrganisation').val(), sortParameter, sortDirection);
});

//$(document).on('click', '.UpdateAccessType', function () {
//    var companyId = parseInt($(this).val());
//    var roleId = ($("#buyer-assigned-role-" + companyId).val() != "") ? parseInt($("#buyer-assigned-role-" + companyId).val()) : 0;
//    if (roleId == 0) {
//        showErrorMessage("please select the buyer access");
//        return false;
//    }
//    if (companyId != "") {
//        $.ajax({
//            type: 'post',
//            url: '/Admin/ChangeBuyerAccessType',
//            data: { companyId: companyId, roleId: roleId },
//            async: false,
//            success: function (response) {
//                if (typeof (response) != "undefined") {
//                    if (response.success)
//                        showSuccessMessage(response.message);
//                    else
//                        showErrorMessage(response.message);
//                }
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//            }
//        });
//    }
//    BuyerOrganisationList(1, "", 1);
//});


//$(document).on('click', '.ChangeAccessType1', function () {
//    var companyId = $(this).val();
//    var select = "";
//    $.ajax({
//        async: false,
//        type: 'post',
//        url: '/Admin/GetBuyerAccessList',
//        success: function (data) {
//            var options = "<option value=\"0\">Select</option>";
//            for (var j = 0 ; j < data.accessList.length; j++) {
//                options += "<option value=\"" + data.accessList[j].RoleId + "\">" + data.accessList[j].RoleName + "</option>";
//            }
//            var accessid = "#accesscellof" + companyId;
//            var select = "<select id='buyer-assigned-role-" + companyId + "' class=\"form-control\">" + options + "</select>";
//            $(accessid).html(select);
//            var actionid = "#actioncellof" + companyId;
//            $(actionid).html("<button type='button' class='btn btn-color UpdateAccessType' style='width: 150px;' id='buyer-organisation-actions-" + companyId + "' value = '"+ companyId +"'>Update Access</button>");
//        }
//    });

//});

$(document).on('click', '.buyerdashboard', function () {
    var partyId = parseInt($(this).attr("value"));
    ShowBuyerDashBoard(partyId);
    return false;
});

function ShowBuyerDashBoard(partyId) {
    if (partyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/GetBuyerDashboard',
            data: { buyerPartyId: partyId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDivs();
                    $("#BuyerDashboard").empty();
                    $("#BuyerDashboard").html(response);
                    $("#BuyerDashboard").show();
                }
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    return false;
}

//$(document).on('click', '#liBuyerSupplierMapping', function () {
//    $("#Loading").show();
//    var companyId = $(this).val();
//    var BreadCrumb = "OrganisationPage"
//    if (companyId != "") {
//        $.ajax({
//            type: 'post',
//            url: '/Admin/GetBuyerSupplierMapping',
//            data: { companyId: companyId, BreadCrumb: BreadCrumb },
//            async: false,
//            success: function (response) {
//                if (typeof (response) != "undefined") {
//                    ClearAllDivs();
//                    $("#BuyerSupplierMapping").html("");
//                    $("#BuyerSupplierMapping").html(response);
//                }
//                return false;
//            },
//            error: function (jqXHR, textStatus, errorThrown) {
//            }
//        });
//    }
//    return false;
//});

//function GetProducts(buyerId) {
//    $("#divDefaultProducts").show();
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetProducts',
//        async: true,
//        success: function (response) {

//            if (typeof (response) != "undefined") {
//                $('#tbodyDefaultProducts').html('');

//                var output = "";

//                if (response.data.length > 0) {
//                    for (var i = 0; i < response.data.length; i++) {
//                        if (i % 2 != 0) {
//                            output += "<tr class=\"odd\">";
//                        }
//                        else {
//                            output += "<tr class=\"even\">";
//                        }

//                        output += "<td>" + response.data[i].ProductName + "</td>";
//                        var checked = "fa fa-square-o";
//                        $.grep(response.data[i].BuyerDefaultProducts, function (obj) {

//                            if (obj.BuyerId === parseInt(buyerId)) {
//                                checked = "fa fa-check-square-o";
//                            }
//                        });


//                        output += "<td style=\"text-align: center;\"><i class=\"" + checked + " cursor-pointer\" Id='product" + (i + 1) + "'onclick=ToggleCheck(this) data-productId='" + response.data[i].ProductId + "' data-buyerId= '" + buyerId + "'></i></td></tr>";
//                    }
//                    //  $('#tblDefaultProducts').after(displayLinks($('#hdnBSMSupplierProductCurrentPage').val(), Math.ceil(response.totalRecords / 10), "", "1", "GetBSMBuyers", "#hdnBSMSupplierProductCurrentPage"));
//                }
//                $('#tbodyDefaultProducts').html(output);
//                $('#tblDefaultProducts').footable();
//                $('#tblDefaultProducts').trigger('footable_redraw');
//            }
//            $("#assignProductToBuyer").modal('toggle');
//            $("#modelHeading").html(defaultProductsText);
//        }

//    });
//}

//function ExportSupplierList(buyerId, buyerName) {
//    window.location.href = "/Admin/ExportToExcel" + "?buyerId=" + buyerId + "&buyerName=" + buyerName;
//}

//function ToggleCheck(i) {
//    if ($(i).hasClass("fa-check-square-o")) {
//        $(i).removeClass("fa-check-square-o");
//        $(i).addClass("fa-square-o");
//    }
//    else if ($(i).hasClass("fa-square-o")) {
//        $(i).removeClass("fa-square-o");
//        $(i).addClass("fa-check-square-o");
//    }
//}



//function AddRemoveDe1faultProducts(productId, buyerId, checkBox) {

//    var isDelete = true;
//    var childElement = checkBox.firstElementChild;
//    if ($(childElement).hasClass('fa-square-o')) {
//        isDelete = false;
//    }
//    $.ajax({
//        type: 'post',
//        url: '/Admin/AddRemoveDefaultProducts',
//        data: { buyerId: buyerId, productId: productId, isDelete: isDelete },
//        async: true,
//        success: function (response) {
//            if (typeof (response) != "undefined") {

//                if (response.result == true) {
//                    if ($(childElement).hasClass('fa-square-o')) {
//                        $(childElement).addClass('fa-check-square-o')
//                        $(childElement).removeClass('fa-square-o')
//                        if (response.id > 0) {
//                            //   $(isChecked).parent().next().html("<button type=\"button\" class=\"btn btn-xs btn-color\" onclick=\"AssignProduct('" + supplierId + "','" + response.id + "');\">Assign Products</button>")
//                        }
//                        //<button type=\"button\" class=\"btn btn-xs btn-color\" onclick=\"AssignProduct('" + response.data[i].CompanyId + "','" + buyerSupplierMappingId + "');\">
//                    } else {
//                        $(childElement).addClass('fa-square-o')
//                        $(childElement).removeClass('fa-check-square-o')
//                    }
//                    showSuccessMessage(response.message);
//                }
//                else {
//                    showErrorMessage(response.message);
//                }
//            }
//        },
//        error: function (error) {

//        }
//    });
//}

//$(document).on('click', '#CompleteAssignDefaultProduct', function () {
//    var fit = $("#product1").hasClass("fa-check-square-o");
//    var hs = $("#product2").hasClass("fa-check-square-o");
//    var ds = $("#product3").hasClass("fa-check-square-o");
//    var fitpId = $("#product1").attr("data-productId");
//    var hspId = $("#product2").attr("data-productId");
//    var dspId = $("#product3").attr("data-productId");
//    var buyerId = $("#product1").attr("data-buyerId");
//    $.ajax({
//        type: 'post',
//        url: '/Admin/UpdateDefaultProducts',
//        data: { buyerId: buyerId, fit: fit, hs: hs, ds: ds, FITpId: fitpId, HSpId: hspId, DSpId: dspId },
//        success: function (response) {
//            if (response.result) {
//                showSuccessMessage(response.message);
//                $("#assignProductToBuyer").modal('hide');
//            }
//            else {
//                showErrorMessage(response.message);
//            }
//        }
//    });

//});


$(document).on('click', '.CreateCampaign', function () {
    $("#Loading").show();
    var companyId = $(this).val();
    var BreadCrumb = "OrganisationPage";
    if (companyId != "") {
        $.ajax({
            type: 'post',
            url: '/Campaign/CreateOrEditCampaign',
            data: { buyerId: companyId, BreadCrumb: BreadCrumb },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDivs();
                    $("#CreateCampaign").html(response);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
});


function ClearAllDivs() {
    $("#searchBuyerOrganisation").html("");
    $("#searchBuyerOrganisation").hide();
    return false;
}

//function FillBuyerDataForVerification(data) {
//    $('#BuyerCompanyId').val(data.BuyerCompanyId);
//    $('#BuyerUserId').val(data.BuyerUserId);
//    $('#BuyerFirstName').attr('readonly', false);
//    $('#BuyerEmail').attr('readonly', false);
//    $('#BuyerOrganisationName').attr('readonly', false);
//    $('#BuyerFirstName').val(data.BuyerFirstName);
//    $('#BuyerLastName').val(data.BuyerLastName);
//    $('#BuyerEmail').val(data.BuyerEmail);
//    $('#BuyerTelephone').val(data.BuyerTelephone);
//    $('#BuyerJobTitle').val(data.BuyerJobTitle);
//    $('#BuyerOrganisationName').val(data.BuyerOrganisationName);
//    $('#BuyerFirstAddressLine1').val(data.BuyerFirstAddressLine1);
//    $('#BuyerFirstAddressLine2').val(data.BuyerFirstAddressLine2);
//    $('#BuyerFirstAddressCity').val(data.BuyerFirstAddressCity);
//    $('#BuyerFirstAddressState').val(data.BuyerFirstAddressState);
//    $('#BuyerFirstAddressCountry').val(data.BuyerFirstAddressCountry);
//    $('#BuyerFirstAddressPostalCode').val(data.BuyerFirstAddressPostalCode);
//    $("input[name=BuyerTurnOver][value='" + data.BuyerTurnOver + "']").prop("checked", true);
//    $("input[name=BuyerNumberOfEmployees][value='" + data.BuyerNumberOfEmployees + "']").prop("checked", true);
//    $("input[name=Buyersector][value='" + data.Buyersector + "']").prop("checked", true);
//    //For Not Sure
//    if (data.Buyersector == 485) {
//        //$("#hdnBusinessSectorDescription").val(data.BusinessSectorDescription)
//        $("#spnBuyerSectorDescription").html(data.BusinessSectorDescription)
//        $("#divBuyerSectorDescription").show();
//    }
//    $('#BuyerFirstName').attr('readonly', true);
//    $('#BuyerEmail').attr('readonly', true);
//    $('#BuyerOrganisationName').attr('readonly', true);
//}

$(document).on('click', '.buyerNameSort', function () {
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
        PagerLinkClick('1', "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation", 'CreatedDate', sortDirection);
    }
    else {
        PagerLinkClick('1', "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation", 'CompanyName', sortDirection);
    }
});

$(window).scroll(function () {
    FixedTableHeaderWithPagination('buyer-organisation-table');
});
$(window).resize(function () {
    FixedTableHeaderWithPagination('buyer-organisation-table');
});

$('#btnBuyerOrgExport').click(function () {
    var buyerAccess = parseInt($('#searchUsingAccess').val());
    var firstName = $("#buyerName").val();
    var status = parseInt($('#searchUsingStatus').val());
    var fromDate = $.trim($('#buyer-from-date').val());
    var toDate = $.trim($('#buyer-to-date').val());
    window.location.href = "/Admin/BuyerOrganizationExport/?buyerName=" + firstName + "&fromDate=" + fromDate + "&toDate=" + toDate + "&status=" + status + "&access=" + buyerAccess;
});


$(".searchFilter").change(function () {
    if (!$('#buyer-organisations-filter-reset').is(':visible')) {
        $('#buyer-organisations-filter-reset').show();

    }
    $("#hdnCurrentPageBuyerOrganisation").val(1);
    var sortDirection = $('.buyerNameSort').attr('data-sortdirection');
    var sortParameter = "CreatedDate";
    if (sortDirection != 3) {
        sortParameter = "CompanyName";
    }
    PagerLinkClick('1', "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation", sortParameter, sortDirection);
});


$('#buyer-organisations-filter-reset').click(function () {
    $('#buyer-organisations-filter-reset').hide();
    $('#buyer-from-date').val("");
    $('#buyer-to-date').val("");
    $('#searchUsingStatus').val("0");
    $("#buyerName").val("");
    $('#searchUsingAccess').val("0");
    $("#hdnCurrentPageBuyerOrganisation").val(1);
    $('.fa-sort-asc').addClass('fa-sort').removeClass('fa-sort-asc');
    $('.fa-sort-desc').addClass('fa-sort').removeClass('fa-sort-desc');
    $(".buyerNameSort").attr('data-sortDirection', '3');
    PagerLinkClick('1', "BuyerOrganisationList", "#hdnCurrentPageBuyerOrganisation", 'CreatedDate', 3);
});