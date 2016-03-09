var numberOfCampaigns = 0;
var campaignStageCount = 0;

function VerifyCampaigns() {
    GetVerifyCampaignDetails(1, "", 3);
    SetSelectedTab("tabVerify");
    $("#campaignapprovediv").hide();
    $("#campaignreleasediv").hide();
    $("#campaignverifydiv").show();
}

function ApproveCampaigns() {
    GetSubmittedCampaigns('1', "", 3);
    SetSelectedTab("tabApprove");
    $("#campaignverifydiv").hide();
    $("#campaignreleasediv").hide();
    $("#campaignapprovediv").show();
}

function ReleaseCampaigns() {
    GetApprovedCampaigns('1', "", 3);
    SetSelectedTab("tabRelease");
    $("#campaignverifydiv").hide();
    $("#campaignapprovediv").hide();
    $("#campaignreleasediv").show();
}

function SetSelectedTab(tabId) {
    $(".tab-div").removeClass('tab-selected');
    $("#" + tabId + "").addClass('tab-selected');
}

$(document).ready(function () {
    setLocation('AdminHomeTab');
    FetchAllData();
    ScrollToTop();
    localStorage.setItem('Home', 0);
    localStorage.setItem('BreadCrumb', "");
    //$("search-from-auditor-home").multiintellisense({
    //    url: '/Admin/GetResultsForMultiSearch',
    //    width: '93',
    //    IsBothActionsSame: true,
    //    actionOnEnter: TesterInSearch,
    //    actionOnSelect: 'TesterInSearch'
    //});
});

$("#search-from-auditor-home").blur(function () {
    if (!$("#search-from-auditor-home-multi-div-Intellisense").is(":hover")) {
        $("#search-from-auditor-home-multi-div-Intellisense").remove();
    }
});

function TesterInSearch(IsHeader, HeaderId, Value, ChildId) {
    var Name = decodeURIComponent(Value);
    if (IsHeader) {
        switch (HeaderId) {
            case "1":
                localStorage.setItem("BuyerNameFromBuyerHomeSearch", $("#search-from-auditor-home").val());
                location.href = '/Admin/BuyerOrganisation';
                break;
            case "2":
                localStorage.setItem("SupplierNameFromBuyerHomeSearch", $("#search-from-auditor-home").val());
                location.href = '/Admin/SupplierOrganisations';
                break;
            case "3":
                localStorage.setItem("UserNameFromBuyerHomeSearch", $("#search-from-auditor-home").val());
                location.href = '/Admin/ManageUsers';
                break;
            case "4":
                localStorage.setItem("LoginIdFromBuyerHomeSearch", $("#search-from-auditor-home").val());
                location.href = '/Admin/ManageUsers';
                break;
        }
    }
    else {
        switch (HeaderId) {
            case "1":
                localStorage.setItem("BuyerIdFromHomeSearch", ChildId);
                location.href = '/Admin/BuyerOrganisation';
                break;
            case "2":
                if (ChildId > 0) {
                    location.href = '/Summary/' + ChildId;
                }
                break;
            case "3":
                localStorage.setItem("UserNameFromBuyerHomeSearch", Name);
                location.href = '/Admin/ManageUsers';
                break;
            case "4":
                localStorage.setItem("LoginIdFromBuyerHomeSearch", Name);
                location.href = '/Admin/ManageUsers';
                break;
        }
    }
    return false;
}

function FetchAllData() {
    $("#Home").show();
    if (!CanViewBuyers && !CanViewSuppliers && !CanViewUsers) {
        $('#search-from-auditor-home').hide();
    }
    if (!CanViewBuyers && !CanViewSuppliers) {
        $('#adminHomeBanner').hide();
    }
    if (CanViewBuyers) {
        $('#hdnverifyBuyersCurrentPage').val(1);
        $('#hdncampaignVerifyCurrentPage').val(1);
        $('#hdncampaignAwaitingCurrentPage').val(1);
        $('#hdncampaignApproveCurrentPage').val(1);
        $('#hdncampaignReleaseCurrentPage').val(1);
        GetVerifyBuyerDetails(1, '', 3);
        GetVerifyCampaignDetails(1, "", 3);
        if (CanReleaseCampaign) {
            $('#tabRelease').show();
            GetApprovedCampaigns(1, "", 3);
        }
        if (CanApproveCampaign) {
            $('#tabApprove').show();
            GetSubmittedCampaigns(1, "", 3);
        }
        GetAwaitingActionCampaignDetails(1, "", 3);
    }
    if (CanViewSuppliers) {
        $('#hdnVerifySupplierCurrentPage').val(1);
        GetSupplierDetails(1, "CompanyName", 3);
        GetSuppliersCountBasedOnStage();
    }
}

function GetSuppliersCountBasedOnStage() {
    $.ajax({
        type: 'post',
        url: '/Admin/GetSuppliersCountBasedOnStage',
        data: { sourceCheck: $('#sourceCheck').val(), viewOptions: $('#viewOptions').val(), referrerName: $('#referrerName').val() },
        success: function (response) {
            var outputHtml = "";
            if (response.data.length > 0) {
                for (var i = 0; i < response.data.length; i++) {
                    var trClass = "odd";
                    if (i % 2 == 0) {
                        trClass = "even";
                    }
                    outputHtml += "<tr class=\"" + trClass + "\"><td>" + response.data[i].Stage + "</td><td class=\"text-align-right\">" + (response.data[i].DetailsScore > 0 ? response.data[i].DetailsScore : "-") + "</td><td class=\"text-align-right\">" + (response.data[i].ProfileScore > 0 ? response.data[i].ProfileScore : "-") + "</td><td class=\"text-align-right\">" + (response.data[i].SanctionScore > 0 ? response.data[i].SanctionScore : "-") + "</td></tr>"
                        //+"<td class=\"text-align-right\">" + (response.data[i].FITScore > 0 ? response.data[i].FITScore : "-") + "</td><td class=\"text-align-right\">" + (response.data[i].HSScore > 0 ? response.data[i].HSScore : "-") + "</td><td class=\"text-align-right\">" + (response.data[i].DSScore > 0 ? response.data[i].DSScore : "-") + "</td>"
                }
                $('#supplier-count-based-on-stage-table-body').html(outputHtml);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}
function GetVerifyBuyerDetails(pageNo, sortParameter, sortDirection) {
    var pageSize = 5;
    $.ajax({
        cache: false,
        type: 'post',
        url: '/Admin/GetBuyersForVerification',
        data: { pageNo: pageNo, sortParameter: sortParameter, sortDirection: sortDirection, buyerName: "", pageSize: pageSize },
        success: function (response) {
            var output = "";
            if (response.result.length > 0) {
                for (var i = 0; i < response.result.length; i++) {
                    if (i % 2 != 0) {
                        output += "<tr class=\"odd\">";
                    }
                    else {
                        output += "<tr class=\"even\">";
                    }
                    output += "<td class= \"text-align-left\">" + response.result[i].BuyerOrganizationName + "</td><td class= \"text-align-left\">" + response.result[i].BuyerStatusString + "</td><td class= \"text-align-left\">" + response.result[i].PrimaryContact + "</td>";
                    output += "<td class= \"text-align-left\">" + response.result[i].PrimaryEmail + "</td>";
                    output += "<td class= \"text-align-left\">" + response.result[i].VerifiedDateString + "</td>";
                    if (response.result[i].ActivatedDate == null && response.result[i].VerifiedDate != null && CanActivateBuyer) {
                        output += "<td class= 'text-align-left'>None selected</td><td class= 'text-align-center'><div class=\"btn btn-color activateBuyer\"  id=\"" + response.result[i].BuyerPartyId + "\" style = 'width: 120px'data-companyName='" + response.result[i].BuyerOrganizationName + "'>" + activateBuyerButton + "</div>";
                    }
                    else if (response.result[i].VerifiedDate == null && CanVerifyBuyer) {
                        output += "<td>-</td><td style=\"text-align: center;\"><div class=\"btn btn-color CompleteVerification\"  style = 'width: 120px' id=\"" + response.result[i].BuyerPartyId + "\">" + verifyBuyerButton + "</div>"
                    }
                    else {
                        output += "<td>-</td><td style=\"text-align: center;\">-</td>";
                    }
                    output += "</td></tr>";
                }
            }
            else {
                output = "<tr><td colspan=\"2\">" + noRecordsFound + "</td></tr>";
                $('.verifyBuyersPaginator').remove();
            }
            $('.verifyBuyersPaginator').html(displayLinks($('#hdnverifyBuyersCurrentPage').val(), Math.ceil(response.totalRecords / pageSize), sortParameter, sortDirection, "GetVerifyBuyerDetails", "#hdnverifyBuyersCurrentPage"));
            $('#home-verify-buyer-table-body').html(output);
            $('#home-verify-buyer-table').footable();
            $('#home-verify-buyer-table').trigger('footable_redraw');
            if (!CanActivateBuyer) {
                $('.activateBuyer').hide();
            }
            if (!CanVerifyBuyer) {
                $('.CompleteVerification').hide();
            }
            $("#verify-buyer-banner").html(response.totalRecords);
            var contentHtml = "";
            var currentPage = parseInt($('#hdnverifyBuyersCurrentPage').val());
            var lastPage = Math.ceil(response.totalRecords / pageSize);
            if (response.result.length > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), (pageSize * currentPage), response.totalRecords);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), response.totalRecords, response.totalRecords);
                }
            }
            ScrollToTop();
            $('#search-page-data-verify-buyer').html(contentHtml);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

function GetVerifyCampaignDetails(currentPage, sortParameter, sortDirection) {
    if ($('#hdnAuditorId').val() != null && $('#hdnAuditorId').val() != "") {
        var obj = { currPage: currentPage, auditorId: $('#hdnAuditorId').val() };
        var pageSize = 5;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Admin/GetAssignedCampaigns",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                if (data.data.length == 0) {
                    $('#home-campaigns-verify-table-body').html("<tr><td colspan='5'>" + noRecordsFound + "</td></tr>")
                }
                else {
                    var tableRow = "";
                    for (var i = 0; i < data.data.length; i++) {
                        var trClass = "";
                        if (i % 2 != 0) {
                            trClass = "odd";
                        }
                        else {
                            trClass = "even";
                        }
                        var actiondropdown = "";
                        if (data.data[i].CampaignType > 522 && data.data[i].AuditorId == 0) {
                            if (CanAssignCampaign) {
                                actiondropdown = "<li class='AssignAuditor' onclick= 'AssignToAuditor(" + data.data[i].CampaignId + ",\"" + data.data[i].CampaignName + "\"," + data.data[i].AuditorId + "," + data.data[i].CampaignType + ")'><a>" + assignAuditorButton + "</a></li>";
                            }
                            if (CanModifyCampaign) {
                                actiondropdown += "<li class='EditCampaign' value='" + data.data[i].CampaignId + "' onclick = 'EditCampaign(" + data.data[i].CampaignId + ")'><a>" + editCampaignButton + "</a></li>";
                            }
                        }
                        else {
                            if (CanVerifyCampaign) {
                                actiondropdown = "<li><a class='verifyCampaign' href='/Campaign/Verify/" + data.data[i].CampaignId + "'>" + verifyListButton + "</a></li>";
                            }
                            if (CanModifyCampaign) {
                                actiondropdown += "<li class='EditCampaign' value='" + data.data[i].CampaignId + "' onclick = 'EditCampaign(" + data.data[i].CampaignId + ")'><a>" + editCampaignButton + "</a></li>";
                            }
                        }
                        tableRow += "<tr class='" + trClass + "'><td>" + data.data[i].BuyerOrganisation
                            + "</td><td>" + data.data[i].CampaignName
                            + "</td><td>" + data.data[i].CampaignTypeString
                            + "</td><td class='text-align-right'>" + data.data[i].CreatedDateString
                            + "</td><td class='text-align-right'>" + data.data[i].SupplierCount
                            + "</td><td style='text-align: center'>";
                        if (actiondropdown != "") {
                            tableRow += "<div class='btn-group' style='width:82px; margin:auto;' id='verifyCampaignActionDiv'><button type='button' class='btn btn-color dropdown-toggle' style='width:82px;' data-toggle='dropdown'>" + actionsButton + "<span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -77px;'>" + actiondropdown + "</ul></div>";
                        }
                        else {
                            tableRow += "-";
                        }
                        tableRow += "</td></tr>";
                    }
                    $('.campaignVerifyPaginator').html(displayLinks($('#hdncampaignVerifyCurrentPage').val(), Math.ceil(data.total / 5), "", sortDirection, "GetVerifyCampaignDetails", "#hdncampaignVerifyCurrentPage"));
                    $("#home-campaigns-verify-table-body").html(tableRow);
                    $('#home-campaigns-verify-table').footable();
                    $('#verifyCampaignActionDiv').show();
                }
                numberOfCampaigns += data.total;
                campaignStageCount++;
                if (campaignStageCount == 3)
                    Banner();
                var contentHtml = "";
                var currentPage = parseInt($('#hdncampaignVerifyCurrentPage').val());
                var lastPage = Math.ceil(data.total / pageSize);
                if (data.data.length > 0) {
                    if (currentPage < lastPage) {
                        contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), (pageSize * currentPage), data.total);
                    }
                    else {
                        contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), data.total, data.total);
                    }
                }
                $('#search-page-data-verify-campaign').html(contentHtml);
            },
            error: function (result) {
                //alert("Error");
            }
        });
    }
}

function GetAwaitingActionCampaignDetails(currentPage, sortParameter, sortDirection) {
    if ($('#hdnAuditorId').val() != null && $('#hdnAuditorId').val() != "") {
        var obj = { PageNo: currentPage, auditorId: $('#hdnAuditorId').val() };
        var pageSize = 5;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Admin/GetCampaignsAwaitingAction",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                if (data.data.length == 0) {
                    $('#home-campaigns-awaiting-table-body').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>")
                }
                else {
                    var tableRow = "";
                    for (var i = 0; i < data.data.length; i++) {
                        var trClass = "";
                        if (i % 2 != 0) {
                            trClass = "odd";
                        }
                        else {
                            trClass = "even";
                        }
                        var status = "";
                        tableRow += "<tr class='" + trClass + "'><td>" + data.data[i].BuyerOrganisation
                            + "</td><td>" + data.data[i].CampaignName
                            + "</td><td class='text-align-left'>" + data.data[i].CampaignStatusString
                            + "</td><td class='text-align-right'>" + data.data[i].SupplierCount
                            + "</td><td class='text-align-left'>" + data.data[i].AssignedToAuditor
                            + "</td><td style='text-align: center'>";
                        if (CanModifyCampaign) {
                            tableRow += "<button type='button' class='btn btn-color' onclick = 'EditCampaign(" + data.data[i].CampaignId + ")'>" + editCampaignButton + "</button>";
                        }
                        else {
                            tableRow += "-"
                        }
                        tableRow += "</td></tr>";
                    }
                    $('.campaignAwaitingPaginator').html(displayLinks($('#hdncampaignAwaitingCurrentPage').val(), Math.ceil(data.total / 5), "", sortDirection, "GetAwaitingActionCampaignDetails", "#hdncampaignAwaitingCurrentPage"));
                    $("#home-campaigns-awaiting-table-body").html(tableRow);
                    $('#home-campaigns-awaiting-table').footable();
                    $('#home-campaigns-awaiting-table').trigger('footable_redraw');
                    $("#campaigns-awaiting-banner").html(data.total);
                    var contentHtml = "";
                    var currentPage = parseInt($('#hdncampaignAwaitingCurrentPage').val());
                    var lastPage = Math.ceil(data.total / pageSize);
                    if (data.data.length > 0) {
                        if (currentPage < lastPage) {
                            contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), (pageSize * currentPage), data.total);
                        }
                        else {
                            contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), data.total, data.total);
                        }
                    }
                    $('#search-page-data-awaiting-campaign').html(contentHtml);
                }
            },
            error: function (result) {
                //alert("Error");
            }
        });
    }
}

function GetSubmittedCampaigns(currentPage, sortParameter, sortDirection) {
    var obj = { currPage: currentPage };
    var pageSize = 5;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Admin/GetSubmittedCampaigns",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.campaigns.length == 0) {
                $('#home-campaigns-approve-table-body').html("<tr><td colspan='7'>" + noRecordsFound + "</td></tr>");
                $('.campaignApprovePaginator').hide();
            }
            else {
                $('#home-campaigns-approve-table-body').show();
                $('#home-campaigns-approve-table-body').html("");
                for (var i = 0; i < data.campaigns.length; i++) {
                    var trClass = "";
                    if (i % 2 != 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    $("#home-campaigns-approve-table-body").append("<tr class='" + trClass + "'><td>" + data.campaigns[i].CampaignName
                        + "</td><td>" + data.campaigns[i].BuyerOrganisation
                        + "</td><td>" + data.campaigns[i].CreatedDateString
                        + "</td><td>" + data.campaigns[i].SupplierCount
                        + "</td><td style='text-align: center'>"
                        + "<button type='button' class='btn btn-color' onclick='DownloadWorkSheet(" + data.campaigns[i].CampaignId + ")'>" + downloadButton + "</button>"
                        + "</td><td style='text-align: center'>"
                        + "<button type='button' class='btn btn-color' onclick='ShowApproveMessage(" + data.campaigns[i].CampaignId + "," + data.campaigns[i].SupplierCount + "," + data.campaigns[i].MasterVendor + ")'>" + approveButton + "</button></td><td style='text-align: center'><button type='button' class='btn btn-color' onclick='RevertToAssign(" + data.campaigns[i].CampaignId + ")'>" + revertButton + "</button></td></tr>");
                }
            }
            $('.campaignApprovePaginator').html(displayLinks($('#hdncampaignApproveCurrentPage').val(), Math.ceil(data.totalRecords / 5), "", sortDirection, "GetSubmittedCampaigns", "#hdncampaignApproveCurrentPage"));
            $('#home-campaigns-approve-table-body').footable();
            $('#home-campaigns-approve-table-body').trigger('footable_redraw');
            numberOfCampaigns += data.totalRecords;
            campaignStageCount++;
            if (campaignStageCount == 3)
                Banner();
            var contentHtml = "";
            var currentPage = parseInt($('#hdncampaignApproveCurrentPage').val());
            var lastPage = Math.ceil(data.totalRecords / pageSize);
            if (data.campaigns.length > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), (pageSize * currentPage), data.totalRecords);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), data.totalRecords, data.totalRecords);
                }
            }
            $('#search-page-data-approve-campaign').html(contentHtml);
        },
        error: function (result) {
            //alert("Error");
        }
    });
}
function GetApprovedCampaigns(currentPage, sortParameter, sortDirection) {
    var pageSize = 5;
    var obj = { currPage: currentPage };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Admin/GetApprovedCampaigns",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.campaigns.length == 0) {
                $('#home-campaigns-release-table-body').html("<tr><td colspan='9'>" + noRecordsFound + "</td></tr>");
                $('#paginator').hide();
            }
            else {
                $('#home-campaigns-release-table-body').show();
                $('#home-campaigns-release-table-body').html("");
                for (var i = 0; i < data.campaigns.length; i++) {
                    var trClass = "";
                    if (i % 2 != 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    var disabled = "";
                    if (data.campaigns[i].IsDownloaded) {
                        disabled = "disabled= disabled";
                    }
                    $("#home-campaigns-release-table-body").append("<tr class='" + trClass + "'><td>" + data.campaigns[i].CampaignName
                        + "</td><td>" + data.campaigns[i].BuyerOrganisation
                        + "</td><td>" + data.campaigns[i].CreatedDateString
                        + "</td><td>" + data.campaigns[i].SupplierCount
                        + "</td><td style='text-align: center'><button type='button' class='btn btn-color' onclick='DownloadWorkSheet(" + data.campaigns[i].CampaignId
                        + ")'>" + downloadButton + "</button></td><td><input type='checkbox' class='mailViaSilverPop' checked='checked' id='chk" + i + "' " + disabled
                        + " /></td><td style='text-align: center'><button type='button' class='btn btn-color' id='btnDwldSupList" + i + "' " + disabled + " onclick='DownloadSuppliersList(" + data.campaigns[i].CampaignId + "," + i
                        + ")'>" + downloadButton + "</button></td><td style='text-align: center'><button type='button' class='btn btn-color' onclick='ShowReleaseMessage(" + data.campaigns[i].CampaignId
                        + "," + data.campaigns[i].IsDownloaded + "," + i
                        + ")'>" + releaseButton + "</button></td><td style='text-align: center'><button type='button' class='btn btn-color' onclick='RevertToSubmitted(" + data.campaigns[i].CampaignId
                        + ")'>" + revertButton + "</button></td></tr>");
                }
            }
            $('.campaignReleasePaginator').html(displayLinks($('#hdncampaignReleaseCurrentPage').val(), Math.ceil(data.totalRecords / 5), "", sortDirection, "GetApprovedCampaigns", "#hdncampaignReleaseCurrentPage"));
            $('#home-campaigns-release-table-body').footable();
            $('#home-campaigns-release-table-body').trigger('footable_redraw');
            numberOfCampaigns += data.totalRecords;
            campaignStageCount++;
            if (campaignStageCount == 3)
                Banner();
            var contentHtml = "";
            var currentPage = parseInt($('#hdncampaignReleaseCurrentPage').val());
            var lastPage = Math.ceil(data.totalRecords / pageSize);
            if (data.campaigns.length > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), (pageSize * currentPage), data.totalRecords);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSize) + 1), data.totalRecords, data.totalRecords);
                }
            }
            $('#search-page-data-release-campaign').html(contentHtml);
        },
        error: function (result) {
        }
    });
}



$(document).on('click', '.CompleteVerification', function () {
    var buyerPartyId = $(this).attr('id');
    var breadcrumb = "Home";
    $.ajax({
        type: 'post',
        url: '/Admin/GetBuyerInformationForVerification',
        data: { buyerPartyId: buyerPartyId, breadCrumb: breadcrumb },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                $("#Home").hide();
                $("#VerifyBuyer").show();
                $("#VerifyBuyer").html(response);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});
$(document).on('click', '.activateBuyer', function () {
    $("#companyNameAccess").html($(this).attr("data-companyName"));
    $("#companyIdAccess").val($(this).attr("id"));
    $("#modelHeadingAccess").html(assignAccess);
    $("#CompleteActivationProcess").html(activate);
    GetModalPopUpForAccesstype();
});
function GetModalPopUpForAccesstype() {
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
        showErrorMessage(pleaseSelectBuyerAccess);
        return false;
    }
    if (companyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/ActivateBuyer',
            data: { buyerPartyId: companyId, roleId: roleId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.success) {
                        showSuccessMessage(response.message);
                        $("#assignAccessToBuyer").modal('hide');
                    }
                    else
                        showErrorMessage(response.message);

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    $("#assignAccessToBuyer").modal('toggle');
    FetchAllData();
});

function ShowApproveMessage(campaignId, SupplierCount, MasterVendor) {
    if (SupplierCount <= MasterVendor) {
        $('#btnApproveCampaign').attr('data-id', campaignId);
        $('#dialogMessageApprove').modal('show');
    }
    else {
        showErrorMessage(supplierCountExceedsMasterVendorvalue)
    }
}

function DownloadWorkSheet(campaignId) {
    window.location.href = '/Admin/WorkSheet/?campaignId=' + campaignId;
}

$("#btnApproveCampaign").click(function () {
    var campaignId = $(this).attr('data-id');
    var obj = { campaignId: campaignId };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Admin/ApproveCampaign",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.result) {
                $('#dialogMessageApprove').modal('hide');
                showSuccessMessage(data.message);
                ApproveCampaigns();
            }
            else {
                $('#dialogMessageApprove').modal('hide');
                showErrorMessage(data.message);
            }

        },
        error: function (result) {
        }
    });
});

function RevertToAssign(campaignId) {
    var obj = { campaignId: campaignId };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Admin/RevertCampaignToAssign",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.result) {
                showSuccessMessage(data.message);
                ApproveCampaigns();
            }
            else {
                showErrorMessage(data.message);
            }
        },
        error: function (result) {
        }
    });
}
function RevertToSubmitted(campaignId) {
    var obj = { campaignId: campaignId };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Admin/RevertCampaignToSubmitted",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.result) {
                showSuccessMessage(data.message);
                ReleaseCampaigns();
            }
            else {
                showErrorMessage(data.message);
            }
        },
        error: function (result) {
        }
    });
}

function DownloadSuppliersList(campaignId, trRowIndex) {
    $("#btnDwldSupList" + trRowIndex).prop("disabled", true);
    $("#chk" + trRowIndex).prop("disabled", true);
    window.location.href = '/Admin/GetCampaignReleaseSuppliers/?campaignId=' + campaignId;
}

function ShowReleaseMessage(campaignId, isDownloaded, trRowIndex) {
    $('#btnReleaseCampaign').attr('data-id', campaignId);
    var isChecked = $("#chk" + trRowIndex).is(":checked");
    isDownloaded = $("#btnDwldSupList" + trRowIndex).is(":disabled");
    $('#btnReleaseCampaign').attr('data-is-downloaded', isDownloaded);
    if (isChecked) {
        if (isDownloaded) {
            confirmMessage = releaseCampaignWithoutMail;
            $('#dialogMessageRelease').find('.modal-body').html("<p>" + confirmMessage + "</p>");
            $('#dialogMessageRelease').modal('show');
        }
        else {
            showErrorMessage(releaseCampaignSuggestion);
        }
    }
    else {
        if (!isDownloaded) {
            confirmMessage = confirmReleaseCampaign;
            $('#dialogMessageRelease').find('.modal-body').html("<p>" + confirmMessage + "</p>");
            $('#dialogMessageRelease').modal('show');
        }
    }
}

$("#btnReleaseCampaign").click(function () {
    var campaignId = $(this).attr('data-id');
    var isDownloaded = $(this).attr('data-is-downloaded');
    var obj = { campaignId: campaignId, isDownloaded: isDownloaded };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Admin/ReleaseCampaign",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.result) {
                $('#dialogMessageRelease').modal('hide');
                showSuccessMessage(data.message);
                updatePage($('#hdnCurrentPage').val(), "GetApprovedCampaigns", "hdnCurrentPage", "", "");
            }
            else {
                showErrorMessage(data.message);
            }
        },
        error: function (result) {
        }
    });
});

$(document).on('click', '.verifySupplierSort', function () {
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
        PagerLinkClick('1', "GetSupplierDetails", "#hdnVerifySupplierCurrentPage", 'CreatedDate', sortDirection);
    }
    else {
        PagerLinkClick('1', "GetSupplierDetails", "#hdnVerifySupplierCurrentPage", 'CompanyName', sortDirection);
    }
});
$("#sourceCheck, #viewOptions").change(function () {
    $(".verifySupplierSort").find('i').removeClass('fa-sort-asc').removeClass('fa-sort-desc').addClass('fa-sort');
    $("#hdnVerifySupplierCurrentPage").val(1);
    GetSupplierDetails(1, 'CreatedDate', 3);
    GetSuppliersCountBasedOnStage();
});

function GetSupplierDetails(pageNo, sortParameter, sortDirection) {
    var pageSizeVerifySupplier = $("#pageSizeVerifySupplier").val();
    $.ajax({
        type: 'post',
        url: '/Admin/GetSuppliersForVerification',
        data: JSON.stringify({ 'pageNo': pageNo, 'sortParameter': sortParameter, 'sortDirection': sortDirection, 'sourceCheck': $('#sourceCheck').val(), 'viewOptions': $('#viewOptions').val(), 'referrerName': $('#referrerName').val(), 'pageSize': pageSizeVerifySupplier }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (typeof (response) != "undefined") {
                $('#home-verify-supplier-table-body').html('');
                var output = "";
                if (response.data.length > 0) {
                    for (var i = 0; i < response.data.length; i++) {
                        if (i % 2 != 0) {
                            output += "<tr class=\"odd\">";
                        }
                        else {
                            output += "<tr class=\"even\">";
                        }
                        var isPaymentDone = false;
                        var btnDetailCls = "";
                        var btnDetailDisabled = "";
                        var btnProfileCls = "";
                        var btnProfileDisabled = "";
                        var btnSanctionCls = "";
                        var btnSanctionDisabled = "";
                        var btnFITCls = "btn-color";
                        var btnFITDisabled = "-";
                        var btnHSCls = "btn-color";
                        var btnHSDisabled = "-";
                        var btnDSCls = "btn-color";
                        var btnDSDisabled = "-";
                        var buttonPublish = "";
                        //for (var j = 0; j < response.data[i].SupplierProducts.length; j++) {
                        //    if (response.data[i].SupplierProducts[j].Status >= 2 && response.data[i].SupplierProducts[j].Status < 3) {
                        //        isPaymentDone = true;
                        //        switch (response.data[i].SupplierProducts[j].Product.ProductName) {
                        //            case "FIT":
                        //                btnFITCls = "btn-color";
                        //                btnFITDisabled = "";
                        //                break;
                        //            case "H&S":
                        //                btnHSCls = "btn-color";
                        //                btnHSDisabled = "";
                        //                break;
                        //            case "DS":
                        //                btnDSCls = "btn-color";
                        //                btnDSDisabled = "";
                        //                break;
                        //        }
                        //    }
                        //    else if (response.data[i].SupplierProducts[j].Status == 3) {
                        //        isPaymentDone = true;
                        //        switch (response.data[i].SupplierProducts[j].Product.ProductName) {
                        //            case "FIT":
                        //                btnFITCls = "btn-color";
                        //                btnFITDisabled = "";
                        //                break;
                        //            case "H&S":
                        //                btnHSCls = "btn-color";
                        //                btnHSDisabled = "";
                        //                break;
                        //            case "DS":
                        //                btnDSCls = "btn-color";
                        //                btnDSDisabled = "";
                        //                break;
                        //        }
                        //    }
                        //    else if (response.data[i].SupplierProducts[j].Status == 4) {
                        //        isPaymentDone = true;
                        //        switch (response.data[i].SupplierProducts[j].Product.ProductName) {
                        //            case "FIT":
                        //                btnFITCls = "btn-color";
                        //                btnFITDisabled = checked;
                        //                break;
                        //            case "H&S":
                        //                btnHSCls = "btn-color";
                        //                btnHSDisabled = checked;
                        //                break;
                        //            case "DS":
                        //                btnDSCls = "btn-color";
                        //                btnDSDisabled = checked;
                        //                break;
                        //        }
                        //        buttonPublish = "btn-color";
                        //    }
                        //    else if (response.data[i].SupplierProducts[j].Status == 5) {
                        //        isPaymentDone = true;
                        //        switch (response.data[i].SupplierProducts[j].Product.ProductName) {
                        //            case "FIT":
                        //                btnFITCls = "btn-color";
                        //                btnFITDisabled = published;
                        //                break;
                        //            case "H&S":
                        //                btnHSCls = "btn-color";
                        //                btnHSDisabled = published;
                        //                break;
                        //            case "DS":
                        //                btnDSCls = "btn-color";
                        //                btnDSDisabled = published;
                        //                break;
                        //        }
                        //    }
                        //}
                        //if (response.data[i].SupplierStatus == 504) {
                        //    btnProfileCls = "btn-color";
                        //    btnSanctionCls = "btn-color";
                        //    btnSanctionDisabled = "-";
                        //}
                        //else if (response.data[i].SupplierStatus == 505) {
                        //    btnDetailCls = "btn-color";
                        //    btnDetailDisabled = checked;
                        //    btnProfileCls = "btn-color";
                        //    btnSanctionCls = "btn-color";
                        //    btnSanctionDisabled = "-";
                        //    buttonPublish = "btn-color";
                        //}
                        //else if (response.data[i].SupplierStatus == 506) {
                        //    btnDetailCls = "btn-color";
                        //    //if (response.data[i].IsPublished == true) {
                        //    //    btnDetailDisabled = published;
                        //    //    btnProfileDisabled = published;
                        //    //} else {
                        //        btnDetailDisabled = checked;
                        //        btnProfileDisabled = checked;
                        //        buttonPublish = "btn-color";
                        //    //}
                        //    btnProfileCls = "btn-color";
                        //    btnSanctionCls = "btn-color";
                        //}
                        //else if (response.data[i].SupplierStatus == 507) {
                        //    btnDetailCls = "btn-color";
                        //    //if (response.data[i].IsPublished == true) {
                        //    //    btnDetailDisabled = published;
                        //    //    btnProfileDisabled = published;
                        //    //    btnSanctionDisabled = published;
                        //    //} else {
                        //        btnDetailDisabled = checked;
                        //        btnProfileDisabled = checked;
                        //        btnSanctionDisabled = checked;
                        //        buttonPublish = "btn-color";
                        //    //}
                        //    btnProfileCls = "btn-color";
                        //    btnSanctionCls = "btn-color";
                        //}
                        //if (isPaymentDone) {
                        //    if (response.data[i].Status == 504) {
                        //        btnDetailCls = "btn-color";
                        //        btnDetailDisabled = "-";
                        //    }
                        //}
                        //else {
                        //    if (response.data[i].Status == 504) {
                        //        btnDetailCls = "btn-color";
                        //    }
                        //}
                        var companyName = "";
                        if (response.data[i].SupplierOrganizationName.length > 26) {
                            companyName = response.data[i].SupplierOrganizationName.substring(0, 25) + "...";
                        }
                        else {
                            companyName = response.data[i].SupplierOrganizationName;
                        }
                        output += "<td class='text-align-left'>" + response.data[i].SupplierId + "</td><td class='text-align-left'><span title='" + response.data[i].SupplierOrganizationName + "'>" + companyName + "</span></td><td class='text-align-left'>" + response.data[i].ProjectSource + "</td><td>-</td><td>-</td><td>-</td>";

                        //if (btnDetailDisabled == "Published" || btnDetailDisabled == "Checked") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink showReadOnlyDetailsResults'  data-companyId='" + response.data[i].SupplierId + "'>" + btnDetailDisabled + "</a></td>";
                        //}
                        //else if (btnDetailDisabled == "-") {
                        //    output += "<td style='text-align: center'><a data-companyId='" + response.data[i].SupplierId + "'>" + btnDetailDisabled + "</a></td>";
                        //}
                        //else {
                        //    output += "<td style='text-align: center'><input type='button' class='btn " + btnDetailCls + " detailVerify' data-companyId='" + response.data[i].SupplierId + "' value='Check' /></td>";
                        //}
                        //if (btnProfileDisabled == "Published") {
                        //    output += "<td  style='text-align: center'><a class=\"SICCodeLink showReadOnlyProfileResults\" data-companyId='" + response.data[i].SupplierId + "'>" + btnProfileDisabled + "</a></td>";
                        //}
                        //else if (btnProfileDisabled == "Checked") {
                        //    output += "<td  style='text-align: center'><a class=\"SICCodeLink profileVerify\" data-companyId='" + response.data[i].SupplierId + "'>" + btnProfileDisabled + "</a></td>";
                        //}
                        //else if (btnProfileDisabled == "") {
                        //    output += "<td style='text-align: center'><input type='button' class='btn " + btnProfileCls + " profileVerify' data-companyId='" + response.data[i].SupplierId + "' value='Check' /></td>";
                        //}
                        //if (btnSanctionDisabled != "") {
                        //    output += "<td style='text-align: center'>" + btnSanctionDisabled + "</td>";
                        //}
                        //else {
                        //    output += "<td style='text-align: center'><input type='button' class='btn " + btnSanctionCls + " sanctionVerify' data-companyId='" + response.data[i].SupplierId + "' value='Check' /></td>";
                        //}
                        //if (btnFITDisabled == "Published") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink QuesEvaluate FITCheckPublish' style='cursor: pointer' data-pillar='FIT' data-companyId='" + response.data[i].SupplierId + "'>" + btnFITDisabled + "</a></td>";
                        //}
                        //else if (btnFITDisabled == "Checked") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink QuesEvaluate FITChecked' style='cursor: pointer' data-pillar='FIT' data-companyId='" + response.data[i].SupplierId + "'>" + btnFITDisabled + "</a></td>";
                        //}
                        //else {
                        //    output += "<td style='text-align: center'><input type='button' class='btn " + btnFITCls + " QuesEvaluate FITCheck' data-pillar='FIT' data-companyId='" + response.data[i].SupplierId + "' value='Check' /></td>";
                        //}
                        //if (btnHSDisabled == "Published") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink QuesEvaluate HSCheckPublish' style='cursor: pointer' data-pillar='HS' data-companyId='" + response.data[i].SupplierId + "'>" + btnHSDisabled + "</a></td>";
                        //}
                        //else if (btnHSDisabled == "Checked") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink QuesEvaluate HSChecked' style='cursor: pointer' data-pillar='HS' data-companyId='" + response.data[i].SupplierId + "'>" + btnHSDisabled + "</a></td>";
                        //}
                        //else {
                        //    output += "<td style='text-align: center'><input type='button' class='btn " + btnHSCls + " QuesEvaluate HSCheck' data-pillar='HS' data-companyId='" + response.data[i].SupplierId + "' value='Check' /></td>";
                        //}
                        //if (btnDSDisabled == "Published") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink QuesEvaluate DSCheckPublish' style='cursor: pointer' data-pillar='DS' data-companyId='" + response.data[i].SupplierId + "'>" + btnDSDisabled + "</a></td>";
                        //}
                        //else if (btnDSDisabled == "Checked") {
                        //    output += "<td style='text-align: center'><a class='SICCodeLink QuesEvaluate DSChecked' style='cursor: pointer' data-pillar='DS' data-companyId='" + response.data[i].SupplierId + "'>" + btnDSDisabled + "</a></td>";
                        //}
                        //else {
                        //    output += "<td style='text-align: center'><input type='button' class='btn " + btnDSCls + " QuesEvaluate DSCheck' data-pillar='DS' data-companyId='" + response.data[i].SupplierId + "' value='" + check + "' /></td>";
                        //}
                        if (response.data[i].Status >= 506 && buttonPublish == "btn-color") {
                            output += "<td style=\"text-align:center\" class=\"publish\"><button data-companyId=\"" + response.data[i].SupplierId + "\"  class=\"btn btn-color publish-status-mail\">" + publish + "</button></td>";
                        }
                        else {
                            output += "<td style=\"text-align:center\" class=\"publish\">-</td>";
                        }
                    }
                }
                else {
                    output = "<tr><td colspan=\"2\">" + noRecordsFound + "</td></tr>";
                }
                $('.verifySupplierPaginator').html(displayLinks($('#hdnVerifySupplierCurrentPage').val(), Math.ceil(response.totalRecords / pageSizeVerifySupplier), sortParameter, sortDirection, "GetSupplierDetails", "#hdnVerifySupplierCurrentPage"));
                $('#home-verify-supplier-table-body').html(output);
                var selectedViewOptions = [];
                $(".filterViewOptions input[type=checkbox]").each(function () {
                    if (!$(this).is(":checked")) {
                        var columnIndex = $(this).attr("data-column-index");
                        $("#home-verify-supplier-table").find("." + $(this).attr("data-verify-id")).hide();
                        $("#home-verify-supplier-table-body td:nth-child(" + columnIndex + ")").hide();
                    }
                    else {
                        var columnIndex = $(this).attr("data-column-index");
                        $("#home-verify-supplier-table").find("." + $(this).attr("data-verify-id")).show();
                        $("#home-verify-supplier-table-body td:nth-child(" + columnIndex + ")").show();
                    }
                });

                $('#home-verify-supplier-table').footable();
                $('#home-verify-supplier-table').trigger('footable_redraw');
                if (!CanPublishSupplier) {
                    $('.publish').hide();
                }
                if (!CanSanctionCheck) {
                    $('.sanctionVerify').hide();
                }
                if (!CanDetailsCheck) {
                    $('.detailVerify').hide();
                }
                if (!CanViewDetailsCheck) {
                    //$('.showReadOnlyDetailsResults').hide();
                    $('.showReadOnlyDetailsResults').removeClass('showReadOnlyDetailsResults').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                }
                if (!CanProfileCheck) {
                    //$('.profileVerify').hide();
                    $('.SICCodeLink, .profileVerify').removeClass('profileVerify').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                    $('.btn, .profileVerify').hide();
                }
                if (!CanViewProfileCheck) {
                    //$('.showReadOnlyProfileResults').hide();
                    $('.showReadOnlyProfileResults').removeClass('showReadOnlyProfileResults').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                }
                if (!CanFITCheck) {
                    $('.FITChecked').removeClass('QuesEvaluate').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                    $('.FITCheck').hide();
                }
                if (!CanViewFITCheck) {
                    $('.FITCheckPublish').removeClass('QuesEvaluate').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                }
                if (!CanHSCheck) {
                    $('.HSChecked').removeClass('QuesEvaluate').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                    $('.HSCheck').hide();
                }
                if (!CanViewHSCheck) {
                    $('.HSCheckPublish').removeClass('QuesEvaluate').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                }
                if (!CanDSCheck) {
                    $('.DSChecked').removeClass('QuesEvaluate').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                    $('.DSCheck').hide();
                }
                if (!CanViewDSCheck) {
                    $('.DSCheckPublish').removeClass('QuesEvaluate').removeClass('SICCodeLink').css({ 'color': 'rgb(51, 51, 51)', 'cursor': 'auto' });
                }
                var contentHtml = "";
                var currentPage = parseInt($('#hdnVerifySupplierCurrentPage').val());
                var lastPage = Math.ceil(response.totalRecords / pageSizeVerifySupplier);
                if (response.data.length > 0) {
                    if (currentPage < lastPage) {
                        contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeVerifySupplier) + 1), (pageSizeVerifySupplier * currentPage), response.totalRecords);
                    }
                    else {
                        contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeVerifySupplier) + 1), response.totalRecords, response.totalRecords);
                    }
                }
                $('#search-page-data-verify-suppliers').html(contentHtml);

            }
            if (response.totalRecords <= pageSizeVerifySupplier) {
                $('.verifySupplierPaginator').css('margin-right', '0px');
            }
            $("#verify-supplier-banner").html(response.totalRecords);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
}

$(document).on('click', '.detailVerify', function () {
    var companyId = $(this).attr('data-companyId');
    GetSupplierDetailsInfoToVerifyHome(companyId);
    return false;
});
$(document).on('click', '#btn-referrer-Name-search', function () {
    //if ($('#referrerName').val() != "") {
        $(".verifySupplierSort").find('i').removeClass('fa-sort-asc').removeClass('fa-sort-desc').addClass('fa-sort');
        GetSupplierDetails(1, 'CreatedDate', 3);
        GetSuppliersCountBasedOnStage();
    //}
    return false;
});
function GetSupplierDetailsInfoToVerifyHome(companyId) {
    $.ajax({
        type: 'post',
        url: '/Admin/SupplierDetails',
        data: { companyId: companyId },
        async: false,
        success: function (response) {
            $("#Home").hide();
            $("#supplierInformationDiv").show();
            $("#supplierInformationDiv").html(response);
            //$('#InitialCheckSupplierForm input,#InitialCheckSupplierForm select,#InitialCheckSupplierForm textarea').prop('disabled', false);
            $('#btnCancelInitialCheck').show();
            $('#btnVerifyInitialCheck').show();
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
}

$(document).on('click', '.showReadOnlyDetailsResults', function () {
    localStorage.setItem("BreadCrumb", "Home");
    var companyId = $(this).attr('data-companyId');
    GetSupplierDetailsInfoToVerifyHome(companyId);
    $('#InitialCheckSupplierForm input,#InitialCheckSupplierForm select,#InitialCheckSupplierForm textarea').prop('disabled', true);
    $('#btnCancelInitialCheck').hide();
    $('#btnVerifyInitialCheck').hide();
    return false;
});


$(document).on('click', '.profileVerify', function () {
    localStorage.setItem("BreadCrumb", "Home");
    Common.IsLoadingNeeded = true;
    var companyId = $(this).attr('data-companyId');
    Common.IsNavigateAsynchronous = false;
    Navigate('GetSupplierInformation', '/Admin/GetSupplierInformation', "Supplier Information", true);
    GetSupplierInformationByIdInMainPage(companyId);
    return false;
});



$(document).on('click', '.showReadOnlyProfileResults', function () {
    localStorage.setItem("BreadCrumb", "Home");
    Common.IsLoadingNeeded = true;
    localStorage.IsVerified = true;
    var companyId = $(this).attr('data-companyId');
    Common.IsNavigateAsynchronous = false;
    Navigate('GetSupplierInformation', '/Admin/GetSupplierInformation', "Supplier Information", true);
    GetSupplierInformationByIdInMainPage(companyId);
    return false;
});

function GetSupplierInformationByIdInMainPage(companyId) {
    //localStorage.setItem('Home', 1);
    localStorage.setItem("BreadCrumb", "Home");
    $.ajax({
        type: 'post',
        url: '/Admin/GetSupplierInformationByCompanyId',
        data: { companyId: companyId },
        async: false,
        success: function (data) {
            if (typeof (data) != "undefined") {
                FillSupplierData(data.model, data.answers, data.status);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
}

$(document).on('click', '.sanctionVerify', function () {
    localStorage.setItem("BreadCrumb", "Home");
    var companyId = $(this).attr('data-companyId');
    $.ajax({
        type: 'post',
        url: '/Admin/GetSupplierSanctioned',
        data: { companyId: companyId },
        async: false,
        success: function (response) {
            $("#Home").hide();
            $("#supplierInformationDiv").show();
            $("#supplierInformationDiv").html(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;

});

$(document).on('click', '.QuesEvaluate', function () {
    var companyId = $(this).attr('data-companyId');
    var pillar = $(this).attr('data-pillar');
    $.ajax({
        type: "POST",
        url: "/Admin/Evaluate",
        data: { companyId: companyId, pillar: pillar, redirectedFromHome: true },
        success: function (response) {

            if (response && typeof (response) != "undefined") {
                if (response == Common.LogoutAction) {
                    Logout();
                }
                else {
                    window.location = response;
                    localStorage.BreadCrumbForQuestionnaire = "Home";
                    $("#tblAuditors").show();
                    $("#tblAuditors").html("No Records found");
                }
            }


        },
        error: function (response) {

        }
    });
    return false;
});

$(document).on('click', '.publish-status-mail', function () {
    var companyId = $(this).attr('data-companyId');
    $.ajax({
        type: 'post',
        url: '/Admin/GetSupplierProductStatus',
        data: { companyId: companyId },
        async: true,
        success: function (response) {
            if (typeof (response) != "undefined") {
                if (response.length > 0) {
                    var html = "";
                    for (var i = 0; i < response.length; i++) {
                        var cls = "odd";
                        if (i % 2 == 0) {
                            cls = "even";
                        }
                        if (!response[i].IsPublished) {
                            html += "<tr class=\"" + cls + "\"><td>" + response[i].ProductName + "</td>"
                            html += "<td style=\"text-align:center\"><input class=\"product-publish-select\" type=\"checkbox\" data-productId=\"" + response[i].ProductId + "\" data-supplierId=\"" + response[i].SupplierId + "\"/></td></tr>";
                        }
                    }
                    if (html == "") {
                        html = "<tr><td>No Records Found</td></tr>";
                    }
                    $('#publish-product-pop-up-body').html(html);
                    $('#publish-product-pop-up').modal('show');
                }
                else {
                    var html = "<tr><td>No Records Found</td></tr>";
                    $('#publish-product-pop-up-body').html(html);
                    $('#publish-product-pop-up').modal('show');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
})

function PublishAndSendReportMails() {
    var publishList = [];
    var checkedboxes = $('.product-publish-select:checkbox:checked');
    var companyId = 0;
    for (var i = 0 ; i < checkedboxes.length; i++) {
        var item = $(checkedboxes[i]);
        var productId = item.attr('data-productId');
        var supplierId = item.attr('data-supplierId');
        companyId = supplierId;
        var item = { SupplierId: supplierId, ProductId: productId, IsPublished: false };
        publishList.push(item);
    }
    if (publishList.length > 0) {
        publishList = JSON.stringify({ 'publishList': publishList });
        $.ajax({
            type: 'post',
            url: '/Admin/PublishAndSendReportMails',
            data: publishList,
            contentType: 'application/json; charset=utf-8',
            async: true,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    if (response.result) {
                        showSuccessMessage(response.message);
                    }
                    else {
                        showErrorMessage(response.message);
                    }
                    $('#publish-product-pop-up').modal('hide');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    else {
        showErrorMessage(selectProductToPublishError);
    }
}

function EditCampaign(campaignId) {
    var companyId = 0;
    var BreadCrumb = "Home";
    $.ajax({
        type: 'post',
        url: '/Campaign/CreateOrEditCampaign',
        data: { buyerId: companyId, BreadCrumb: BreadCrumb, campaignId: campaignId },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                $("#Home").hide();
                $("#CreateOrEditCampaign").html(response);
                $("#CreateOrEditCampaign").show();
                ScrollToTop();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

}

function AssignToAuditor(campaignId, campaignName, auditorId, campaignType) {
    $('#lblCampaignNamePopup').html(campaignName);
    $('#lblCampaignIdPopUp').html(campaignId);
    $('#lblCampaignTypePopUp').html(campaignType);
    $.ajax({
        type: 'post',
        url: '/Admin/GetAssignCampaignAuditorList',
        async: true,
        success: function (response) {
            var optionList = "<option value=''>---select---</option>";
            for (var i = 0; i < response.result.length; i++) {
                optionList += "<option value='" + response.result[i].Value + "'>" + response.result[i].Text + "</option>";
            }
            $("#ddlCampaignAuditor").html(optionList);
            if (auditorId != 0) {
                $("#ddlCampaignAuditor").val(auditorId);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    $('#dialogAssignCampaignAuditor').modal('show');
}

$('#btnAssignAuditor').click(function () {
    var auditorId = $('#ddlCampaignAuditor').val();
    var campaignId = $('#lblCampaignIdPopUp').html();
    var campaignType = $('#lblCampaignTypePopUp').html();
    if (auditorId != "") {
        var obj = { auditorId: auditorId, campaignId: campaignId };
        $.ajax({
            type: 'post',
            url: '/Admin/AssignCampaignToAuditor',
            data: JSON.stringify(obj),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(data.message);
                    $('#dialogAssignCampaignAuditor').modal('hide');
                    //window.location.href = "/Admin/Home";
                    GetVerifyCampaignDetails();
                    GetSubmittedCampaigns();
                    GetApprovedCampaigns();
                    GetAwaitingActionCampaignDetails();
                    ScrollToTop();
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
});


function Banner() {
    $("#campaigns-attention-banner").html(numberOfCampaigns);
}

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
        PagerLinkClick('1', "GetVerifyBuyerDetails", "#hdnverifyBuyersCurrentPage", '', sortDirection);
    }
    else {
        PagerLinkClick('1', "GetVerifyBuyerDetails", "#hdnverifyBuyersCurrentPage", 'CompanyName', sortDirection);

    }
});

$(".verifyBuyerBanner").click(function () {
    var scroll = $(".buyerDiv").position();
    $('html,body').animate({ 'scrollTop': scroll.top - 50 }, 200);
});

$(".campaignAttentionBanner").click(function () {
    var scroll = $(".campaignDiv").position();
    $('html,body').animate({ 'scrollTop': scroll.top - 50 }, 200);
});

$(".campaignAwaitingBanner").click(function () {
    var scroll = $(".campaignAwaitingDiv").position();
    $('html,body').animate({ 'scrollTop': scroll.top - 50 }, 200);
});

$(".verifySupplierBanner").click(function () {
    var scroll = $("#verify-suppliers-div").position();
    $('html,body').animate({ 'scrollTop': scroll.top - 50 }, 200);
});
$(window).scroll(function () {
    FixedTableHeaderWithPagination('home-verify-buyer-table');
    //FixedTableHeaderWithPagination('home-campaigns-verify-table');
    //FixedTableHeaderWithPagination('home-campaigns-approve-table');
    //FixedTableHeaderWithPagination('home-campaigns-release-table');
    //FixedTableHeaderWithPagination('home-campaigns-awaiting-table');
    //FixedTableHeaderWithPagination('home-verify-supplier-table');
});
$(window).resize(function () {
    FixedTableHeaderWithPagination('home-verify-buyer-table');
    //FixedTableHeaderWithPagination('home-campaigns-verify-table');
    //FixedTableHeaderWithPagination('home-campaigns-approve-table');
    //FixedTableHeaderWithPagination('home-campaigns-release-table');
    //FixedTableHeaderWithPagination('home-campaigns-awaiting-table');
    //FixedTableHeaderWithPagination('home-verify-supplier-table');
});

$("#pageSizeVerifySupplier").change(function () {
    $('#hdnVerifySupplierCurrentPage').val(1);
    GetSupplierDetails(1, "CompanyName", 3);
});


$('#btnSupplierOrgExport').click(function () {
    var sourceCheck = $('#sourceCheck').val();
    var viewOptions = $('#viewOptions').val();
    var referrerName = $('#referrerName').val();
    window.location.href = "/Admin/SupplierInfoDownload/?sourceCheck=" + sourceCheck + "&viewOptions=" + viewOptions + "&referrerName=" + referrerName;
});
