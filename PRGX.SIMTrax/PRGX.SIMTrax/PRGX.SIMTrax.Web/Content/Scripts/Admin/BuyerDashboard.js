$(document).ready(function () {
    setLocation("BuyerOrganisationsTab");
    ScrollToTop();
    FetchAllData();
});

function FetchAllData() {
    if (!CanCreateVoucher) {
        $('#btnCreateVoucher').hide();
    }
    if (!CanModifyCampaign) {
        $('#btnCreateCampaign').hide();
    }
    if (!CanCreateUser) {
        $('#btnCreateUsers').hide();
    }
    if (CanMapSectionToBuyer || CanCreateSection || CanPublishSection || CanAddQuestion) {
        $('#btnManageQuestionSets').show();
    }
    else {
        $('#btnManageQuestionSets').hide();
    }
    $("#CreateQuestion").hide();
    GetBuyerDashboardDetails();
    GetBuyerCampaignDetailsForDashboard(1, "", 1);
    GetBuyerUserDetailsForDashboard(1, "", 1);
    //GetBuyerQuestionSetDetailsForDashboard(1, "", 1);
    GetVoucherDetailsForDashboard(1, "", 1);
    //GetSupplierDetailsForDashboard(1, "", 1);
}

function GetBuyerDashboardDetails() {
    $.ajax({
        cache: false,
        type: 'post',
        url: '/Admin/GetBuyerDetailsForDashboard',
        data: { partyId: buyerPartyId },
        success: function (response) {
            localStorage.setItem('companyName', response.result.BuyerOrganizationName);
            var heading = response.result.BuyerOrganizationName + "'s " + dashboard;
            document.title = heading;
            $("#headingBuyerDashboard").html(heading);
            var options = "";
            var select = "";

            if (response.result.ActivatedDate == null && response.result.IsVerified == true) {
                select = noneselected;
            }
            else {
                select = response.result.BuyerRoleName;
            }

            var actiondropdown = "";
            if (response.result.VerifiedDate == null && CanVerifyBuyer) {
                actiondropdown = actiondropdown + "<li class='CompleteVerification' value='" + response.result.BuyerPartyId + "'><a>" + verifyBuyer + "</a></li>";
            }
            if (response.result.ActivatedDate == null && response.result.VerifiedDate != null && CanActivateBuyer) {
                actiondropdown = actiondropdown + "<li class='CompleteActivation' value='" + response.result.BuyerPartyId + "' data-companyName='" + response.result.BuyerOrganizationName + "'><a>" + activateBuyer + "</a></li>";
            }
            if (response.result.ActivatedDate != null && CanChangeAccessType) {
                actiondropdown = actiondropdown + "<li class='ChangeAccessType' value='" + response.result.BuyerPartyId + "' data-companyName='" + response.result.BuyerOrganizationName + "' data-accessType='" + response.result.BuyerRoleId + "'><a>" + changeAccessType + "</a></li>";
            }
            if (CanMapBuyerSupplier || CanModifyCampaign || CanCreateVoucher || CanAssignDefualtProduct || CanBuyerSupplierAssignProduct) {
                actiondropdown = actiondropdown +
                    //"<li class='BuyerSupplierMapping' value='" + response.result.BuyerPartyId + "'><a>" + buyerSupplierMapping + "</a></li>" +
                    "<li class='CreateCampaignDashboard' value='" + response.result.BuyerPartyId + "'><a>" + createCampaign + "</a></li>" +
                    "<li class='CreateVoucher' value='" + response.result.BuyerPartyId + "'><a>" + createVoucher + "</a></li>";
                    //+ "<li class='AssignProduct' value='" + response.result.BuyerPartyId + "' onclick='GetProducts(" + response.result.BuyerPartyId + ")'><a>" + assignProduct + "</a></li>"+
                    //"<li class='BuyerSupplierProductMapping' onclick='ExportSupplierListDashBoard(" + response.result.BuyerPartyId + ",\"" + response.result.BuyerOrganizationName + "\")'><a>" + exportSupplierList + "</a></li>";
            }
            var tableRow = "<tr class='odd'><td class= 'buyer-name-ellipsis'>" + response.result.BuyerOrganizationName
                + "</td><td>" + response.result.BuyerStatusString
                + "</td><td>" + response.result.PrimaryContact
                + "</td><td>" + response.result.PrimaryEmail
                + "</td><td class=\"text-align-right\">" + response.result.CreatedDateString
                + "</td><td class=\"text-align-right\">" + response.result.TermsAcceptedDateString
                + "</td><td class=\"text-align-right\">" + response.result.VerifiedDateString
                + "</td><td class=\"text-align-right\">" + response.result.ActivatedDateString
                + "</td><td id='accesscell'>" + select
                + "</td><td id='actioncell' class=\"text-align-center\">";
            if (actiondropdown != "") {
                tableRow += "<div class='btn-group' style='width:82px;'><button type='button' class='btn btn-color dropdown-toggle'' data-toggle='dropdown' id='buyer-organisation-actions' style='width: 82px;'>" + actionsmessage + " <span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -110px;'>" + actiondropdown + "</ul></div>";
            }
            else {
                tableRow += "-";
            }
            tableRow += "</td></tr>";
            $('#buyer-details-table-body').html(tableRow);
            $('#buyer-details-table').footable();
            $('#buyer-details-table').trigger('footable_redraw');
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
                $('.CreateCampaignDashboard').hide();
            }
            if (!CanChangeAccessType) {
                $('.ChangeAccessType').hide();
            }
            if (!CanMapBuyerSupplier) {
                $('.BuyerSupplierMapping').hide();
            }
            if (!CanAssignDefualtProduct) {
                $('.AssignProduct').hide();
            }
            if (!CanBuyerSupplierAssignProduct) {
                $('.BuyerSupplierProductMapping').hide();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

function GetBuyerCampaignDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
    var campaigntablebody = "";
    $.ajax({
        cache: false,
        type: 'post',
        url: '/Admin/GetBuyerCampaignDetailsForDashboard',
        data: { partyId: buyerPartyId, pageNumber: pageNumber, sortDirection: sortDirection },
        success: function (response) {
            var pageSize = 5;
            if (response.campaignDetails.length > 0) {
                var trClass = "";
                for (var i = 0; (i < response.campaignDetails.length) ; i++) {
                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    var actions = "";
                    if ((response.campaignDetails[i].CampaignTypeInt == 523 || response.campaignDetails[i].CampaignTypeInt == 524) && response.campaignDetails[i].AuditorId == 0) {
                        if (CanAssignCampaign) {
                            actions = "<li class='assignToAuditorDashboard' onclick = 'AssignToAuditor(" + response.campaignDetails[i].CampaignId + ",\"" + response.campaignDetails[i].CampaignName + "\",\"" + response.campaignDetails[i].AuditorId + "\"," + response.campaignDetails[i].CampaignTypeInt + ")'><a>" + assignToAuditor + "</a></li>";
                        }
                        if (CanModifyCampaign) {
                            actions += "<li class='editCampaignDashboard' onclick= 'EditCampaign(" + response.campaignDetails[i].CampaignId + ")'><a>" + editCampaign + "</a></li>";
                        }
                    }
                    else if ((response.campaignDetails[i].CampaignTypeInt == 523 || response.campaignDetails[i].CampaignTypeInt == 524) && response.campaignDetails[i].AuditorId != 0 && CanModifyCampaign) {
                        if (response.campaignDetails[i].CampaignStatusInt == 529) {
                            actions = "<li class='editCampaignDashboard' onclick= 'EditCampaign(" + response.campaignDetails[i].CampaignId + ")'><a>" + viewCampaign + "</a></li>";
                        }
                        else {
                            actions = "<li class='editCampaignDashboard' onclick= 'EditCampaign(" + response.campaignDetails[i].CampaignId + ")'><a>" + editCampaign + "</a></li>";
                        }
                    }
                    else if (CanModifyCampaign) {
                        actions = "<li class='editCampaignDashboard' onclick= 'EditCampaign(" + response.campaignDetails[i].CampaignId + ")'><a>" + editCampaign + "</a></li>";
                    }
                    var tablerow = "<tr class=" + trClass
									+ "><td><div class= 'ellipsis-large'>" + response.campaignDetails[i].CampaignName
									+ "</div></td><td><div class= 'ellipsis-small'>" + response.campaignDetails[i].CampaignTypeString
									+ "</div></td><td class=\"text-align-center\">";
                    if (actions != "") {
                        tablerow += "<div class='btn-group' style='width: 82px; margin:auto;'><button type='button' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='campaign-actions'  style='width: 82px;'>" + actionsmessage + " <span class='caret'></span></button><ul class='dropdown-menu'  style='margin-left: -77px;'>" + actions + "</ul></div>";
                    }
                    else {
                        tablerow += "-";
                    }
                    tablerow += "</td></tr>";
                    campaigntablebody += tablerow;
                }
                $("#CampaignTableBody").html(campaigntablebody);
            }
            else {
                $('#CampaignTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
            }
            $('.CampaignPaginator').html(displayLinksForSmallContainers($('#hdnCampaignCurrentPage').val(), Math.ceil(response.totalCampaigns / pageSize), "CampaignName", sortDirection, "GetBuyerCampaignDetailsForDashboard", "#hdnCampaignCurrentPage"));
            $('#CampaignTable').footable();
            $('#CampaignTable').trigger('footable_redraw');
            if (!CanAssignCampaign) {
                $('.assignToAuditorDashboard').hide();
            }
            if (!CanModifyCampaign) {
                $('.editCampaignDashboard').hide();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

//function GetSupplierDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
//    //$('#hdnSupplierCurrentPage').val(1);
//    var pageSize = 5;
//    var supplierstablebody = "";
//    $.ajax({
//        cache: false,
//        type: 'post',
//        url: '/Admin/GetSupplierDetailsForDashboard',
//        data: { companyId: companyId, pageNumber: pageNumber, sortDirection: sortDirection },
//        success: function (response) {
//            if (response.supplierDetails.length > 0) {
//                var trClass = "";
//                for (var i = 0; (i < response.supplierDetails.length) ; i++) {
//                    if (i % 2 == 0) {
//                        trClass = "odd";
//                    }
//                    else {
//                        trClass = "even";
//                    }

//                    var mapped = notMappedMessage;
//                    var buyerSupplierMappingId = 0;
//                    var actions = "";
//                    for (var j = 0; j < response.supplierDetails[i].BuyerMapped.length; j++) {
//                        if (response.supplierDetails[i].BuyerMapped[j].BuyerId == companyId) {
//                            mapped = mappedMessage;
//                            buyerSupplierMappingId = response.supplierDetails[i].BuyerMapped[j].Id;
//                        }
//                    }
//                    if (mapped == "Not mapped") {
//                        if (CanBuyerSupplierAssignProduct) {
//                            actions = "<li class='assignProductsToSupplier' data-companyId='" + response.supplierDetails[i].CompanyId + "' onclick = 'AssignProductDashBoard(" + response.supplierDetails[i].CompanyId + "," + buyerSupplierMappingId + "," + response.supplierDetails[i].CompanyId + ")'><a>" + assignProducts + "</a></li>";
//                        }
//                        if (CanMapBuyerSupplier) {
//                            actions += "<li class='addOrRemoveMapping' onclick = 'ToggleMapping(" + response.supplierDetails[i].CompanyId + "," + companyId + "," + false + ")'><a>" + mapToBuyer + "</a></li>";
//                        }
//                    }
//                    else {
//                        if (CanBuyerSupplierAssignProduct) {
//                            actions = "<li class='assignProductsToSupplier' data-companyId='" + response.supplierDetails[i].CompanyId + "' onclick = 'AssignProductDashBoard(" + response.supplierDetails[i].CompanyId + "," + buyerSupplierMappingId + "," + response.supplierDetails[i].CompanyId + ")'><a>" + assignProducts + "</a></li>";
//                        }
//                        if (CanMapBuyerSupplier) {
//                            actions += "<li class='addOrRemoveMapping' onclick = 'ToggleMapping(" + response.supplierDetails[i].CompanyId + "," + companyId + "," + true + ")'><a>" + unmapFromBuyer + "</a></li>";
//                        }
//                    }
//                    var tablerow = "<tr class=" + trClass
//									+ "><td><div class= 'ellipsis-large'>" + response.supplierDetails[i].CompanyName
//									+ "</div></td><td><div class= 'ellipsis-small'>" + mapped
//									+ "</div></td><td class=\"text-align-center\">";
//                    if (actions != "") {
//                        tablerow += "<div class='btn-group' style='width: 82px;  margin:auto;'><button type='button' style='width: 82px;' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='supplier-actions'>" + actionsmessage + "<span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -84px;'>" + actions + "</ul></div>"
//                    }
//                    else {
//                        tablerow += "-"
//                    }
//                    tablerow += "</td></tr>";
//                    supplierstablebody += tablerow;
//                }
//                $("#SupplierTableBody").html(supplierstablebody);
//            }
//            else {
//                $('#SupplierTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
//            }
//            $('.SuppliersPaginator').html(displayLinksForSmallContainers($('#hdnSupplierCurrentPage').val(), Math.ceil(response.totalSuppliers / pageSize), "CompanyName", sortDirection, "GetSupplierDetailsForDashboard", "#hdnSupplierCurrentPage"));
//            $('#SupplierTable').footable();
//            $('#SupplierTable').trigger('footable_redraw');
//            if (!CanBuyerSupplierAssignProduct) {
//                $('.assignProductsToSupplier').hide();
//            }
//            if (!CanMapBuyerSupplier) {
//                $('.addOrRemoveMapping').hide();
//            }
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            //alert('Failed to retrieve Buyer info.');
//        }
//    });
//}

function GetBuyerUserDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
    var pageSize = 5;
    var userstablebody = "";
    $.ajax({
        type: 'post',
        url: '/Admin/GetBuyerUserDetailsForDashboard',
        data: { buyerPartyId: buyerPartyId, pageNumber: pageNumber, sortDirection: sortDirection },
        success: function (response) {
            if (response.userDetails.length > 0) {
                var trClass = "";
                for (var i = 0; (i < response.userDetails.length) ; i++) {
                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    var tablerow = "<tr class=" + trClass
									+ "><td><div class='ellipsis-large'>" + response.userDetails[i].UserName
									+ "</div></td><td><div class='ellipsis-small'>" + response.userDetails[i].UserType
									+ "</div></td><td class=\"text-align-center\">";
                    if (CanEditUser || CanChangePassword) {
                        tablerow += "<div class='btn-group' style='width: 82px; margin:auto;'><button type='button' style='width: 82px;' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='users-actions'>" + actionsmessage + "<span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -77px;'>";
                        if (CanEditUser) {
                            tablerow += "<li><a onclick='EditUser(" + response.userDetails[i].UserId + ")'>" + editUserDetails + "</a></li>";
                        }
                        if (CanChangePassword) {
                            tablerow += "<li><a onclick='ChangePassword(" + response.userDetails[i].UserId + ",\"" + response.userDetails[i].LoginId + "\")'>" + changePassword + "</a></li>";
                        }
                        tablerow += "</ul></div>";
                    }
                    else {
                        tablerow += "-";
                    }
                    tablerow += "</td></tr>";
                    userstablebody += tablerow;
                }
                $("#UsersTableBody").html(userstablebody);
            }
            else {
                $('#UsersTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
            }
            $('.UsersPaginator').html(displayLinksForSmallContainers($('#hdnUsersCurrentPage').val(), Math.ceil(response.totalUsers / pageSize), "FirstName", sortDirection, "GetBuyerUserDetailsForDashboard", "#hdnUsersCurrentPage"));
            $('#UsersTable').footable();
            $('#UsersTable').trigger('footable_redraw');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

//function GetBuyerQuestionSetDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
//    //$('#hdnQuestionSetsCurrentPage').val(1);
//    var pageSize = 5;
//    var questionsetstablebody = "";
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetBuyerQuestionSetDetailsForDashboard',
//        data: { companyId: companyId, pageNumber: pageNumber, sortDirection: sortDirection },
//        success: function (response) {
//            if (response.questionSetDetails.length > 0) {
//                var trClass = "";
//                for (var i = 0; (i < response.questionSetDetails.length) ; i++) {
//                    if (i % 2 == 0) {
//                        trClass = "odd";
//                    }
//                    else {
//                        trClass = "even";
//                    }
//                    var flag = 0;
//                    var actions = "";
//                    if (response.questionSetDetails[i].Publish == true && CanMapSectionToBuyer) {
//                        if (response.questionSetDetails[i].BuyerQuestionSetMappings.length > 0) {
//                            for (var j = 0; j < response.questionSetDetails[i].BuyerQuestionSetMappings.length; j++) {
//                                if (response.questionSetDetails[i].BuyerQuestionSetMappings[j].BuyerId == companyId) {
//                                    flag = 1;
//                                }
//                            }
//                        }
//                        if (flag == 0) {
//                            actions = "<li class='mapQuestionSet' onclick = 'MapQuestionSetToBuyers(" + true + "," + companyId + "," + response.questionSetDetails[i].Id + ")'><a>" + mapQuestionnaireSection + "</a></li>";
//                        }
//                        else {
//                            actions = "<li class='mapQuestionSet' onclick = 'MapQuestionSetToBuyers(" + false + "," + companyId + "," + response.questionSetDetails[i].Id + ")'><a>" + unmapQuestionnaireSection + "</a></li>";
//                        }
//                    }

//                    var tablerow = "<tr class=" + trClass
//									+ "><td><div class='ellipsis-large'>" + response.questionSetDetails[i].Name
//									+ "</div></td><td><div class='ellipsis-small'>" + ((response.questionSetDetails[i].Publish == true) ? published : notPublished)
//									+ "</div></td><td class=\"text-align-center\">";
//                    if (actions != "") {
//                        tablerow += "<div class='btn-group' style='width: 82px; margin:auto;'><button type='button' style='width: 82px;' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='questionset-actions'>" + actionsmessage + "<span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -149px;'>" + actions + "</ul></div>"
//                    }
//                    else {
//                        tablerow += "-";
//                    }
//                    tablerow += "</td></tr>";
//                    questionsetstablebody += tablerow;
//                }
//                $("#QuestionSetsTableBody").html(questionsetstablebody);
//            }
//            else {
//                $('#QuestionSetsTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
//            }
//            $('.QuestionSetsPaginator').html(displayLinksForSmallContainers($('#hdnQuestionSetsCurrentPage').val(), Math.ceil(response.totalQuestionSets / pageSize), "Name", sortDirection, "GetBuyerQuestionSetDetailsForDashboard", "#hdnQuestionSetsCurrentPage"));
//            $('#QuestionSetsTable').footable();
//            $('#QuestionSetsTable').trigger('footable_redraw');
//            if (!CanMapSectionToBuyer) {
//                $('.mapQuestionSet').hide();
//            }
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            //alert('Failed to retrieve Buyer info.');
//        }
//    });
//}

//function GetBuyerQuestionDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
//    //$('#hdnQuestionsCurrentPage').val(1);
//    var pageSize = 5;
//    var questionstablebody = "";
//    $.ajax({
//        cache: false,
//        type: 'post',
//        url: '/Admin/GetBuyerQuestionDetailsForDashboard',
//        data: { companyId: companyId, pageNumber: pageNumber, sortDirection: sortDirection },
//        success: function (response) {
//            if (response.questionDetails.length > 0) {
//                var trClass = "";
//                for (var i = 0; (i < response.questionDetails.length) ; i++) {
//                    if (i % 2 == 0) {
//                        trClass = "odd";
//                    }
//                    else {
//                        trClass = "even";
//                    }
//                    var actions = "";
//                    if (response.questionDetails[i].IsQuestionSetPublished == false) {
//                        actions = "<div class='btn-group' style='width: 82px;'><button type='button' style='width: 82px; margin:auto;' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='campaign-actions'>" + actionsmessage + "<span class='caret'></span></button><ul class='dropdown-menu'  style='margin-left: -77px;'><li class = 'EditQuestion' onclick = 'EditQuestion(" + response.questionDetails[i].Id + ")'><a>Edit Question</a></li></ul></div>";
//                    }
//                    else {
//                        actions = na;
//                    }
//                    var tablerow = "<tr class=" + trClass
//									+ "><td><div class='ellipsis-large'>" + response.questionDetails[i].Number
//									+ "</div></td><td><div class='ellipsis-small'>" + response.questionDetails[i].Question
//									+ "</div></td><td class=\"text-align-center\">" + actions
//									+ "</td></tr>";
//                    questionstablebody += tablerow;
//                }
//                $("#QuestionsTableBody").html(questionstablebody);
//            }
//            else {
//                $('#QuestionsTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
//            }
//            $('.QuestionsPaginator').html(displayLinksForSmallContainers($('#hdnQuestionsCurrentPage').val(), Math.ceil(response.totalQuestions / pageSize), "Number", sortDirection, "GetBuyerQuestionDetailsForDashboard", "#hdnQuestionsCurrentPage"));
//            $('#QuestionsTable').footable();
//            $('#QuestionsTable').trigger('footable_redraw');
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            //alert('Failed to retrieve Buyer info.');
//        }
//    });
//}

//function GetBuyerVoucherDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
//    //    $('#hdnVouchersCurrentPage').val(1);
//    var pageSize = 5;
//    var voucherstablebody = "";
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetBuyerVoucherDetailsForDashboard',
//        data: { companyId: companyId, pageNumber: pageNumber, sortDirection: sortDirection },
//        success: function (response) {
//            if (response.voucherDetails.length > 0) {
//                var trClass = "";
//                for (var i = 0; (i < response.voucherDetails.length) ; i++) {
//                    if (i % 2 == 0) {
//                        trClass = "odd";
//                    }
//                    else {
//                        trClass = "even";
//                    }
//                    var tablerow = "<tr class=" + trClass
//									+ "><td><div class='ellipsis-large'>" + response.voucherDetails[i].PromotionalCode
//									+ "</div></td><td>" + ((response.voucherDetails[i].BuyerId == null) ? notMappedMessage : mappedMessage)
//									+ "</td><td class=\"text-align-center editVoucherAction\"><div class='btn-group' style='width: 82px; margin:auto;'><button type='button' style='width: 82px;' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='vouchers-actions'>" + actionsmessage + "<span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -77px;'><li onclick='EditVoucher(\"" + response.voucherDetails[i].PromotionalCode + "\")'><a>" + editVoucher + "</a></li></ul></div>"
//									+ "</td></tr>";
//                    voucherstablebody += tablerow;
//                }
//                $("#VouchersTableBody").html(voucherstablebody);

//                if (!CanCreateVoucher) {
//                    $('.editVoucherAction').hide();
//                }
//            }
//            else {
//                $('#VouchersTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
//            }
//            $('.VouchersPaginator').html(displayLinksForSmallContainers($('#hdnVouchersCurrentPage').val(), Math.ceil(response.totalVouchers / pageSize), "PromotionalCode", sortDirection, "GetBuyerVoucherDetailsForDashboard", "#hdnVouchersCurrentPage"));
//            $('#VouchersTable').footable();
//            $('#VouchersTable').trigger('footable_redraw');
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            //alert('Failed to retrieve Buyer info.');
//        }
//    });
//}

$(document).on('click', '.CreateCampaignDashboard', function () {
    var BreadCrumb = "Dashboard";
    if (buyerPartyId != "") {
        $.ajax({
            type: 'post',
            url: '/Campaign/CreateOrEditCampaign',
            data: { buyerId: buyerPartyId, BreadCrumb: BreadCrumb },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDashBoardDivs();
                    $("#CreateCampaign").html(response);
                    $("#CreateCampaign").show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
});

function ClearAllDashBoardDivs() {
    $("#BuyerDashboard").html("");
    $("#BuyerDashboard").hide();
}

//function GetBuyerAccessList() {
//    $.ajax({
//        //async: false,
//        type: 'post',
//        url: '/Admin/GetBuyerAccessList',
//        success: function (data) {
//            var options = "<option value=\"0\">" + all + "</option>";
//            for (var j = 0 ; j < data.accessList.length; j++) {
//                options += "<option value=\"" + data.accessList[j].RoleId + "\">" + data.accessList[j].RoleName + "</option>";
//            }
//            $('#searchUsingAccess').html(options);
//        }
//    })
//}

$(document).on('click', '.CompleteVerification', function () {
    $("#Loading").show();
    var buyerPartyId = $(this).val();
    var breadcrumb = "DashBoard";
    $.ajax({
        type: 'post',
        url: '/Admin/GetBuyerInformationForVerification',
        data: { buyerPartyId: buyerPartyId, breadCrumb: breadcrumb },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                ClearAllDashBoardDivs();
                $("#Loading").hide();
                $("#VerifyBuyer").html(response);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});

//$('#btnManageQuestionSets').off("click").on("click", function () {
//    $('#Loading').show();
//    var BreadCrumb = "DashBoard";
//    var questionSetId = 0;
//    $.ajax({
//        type: 'post',
//        url: '/Admin/ManageQuestionSet',
//        data: { companyId: companyId, BreadCrumb: BreadCrumb },
//        async: false,
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                ClearAllDashBoardDivs();
//                $("#ManageQuestionSet").html(response);
//                $("#ManageQuestionSet").show();
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//});

$(document).on('click', '.ChangeAccessType', function () {
    $("#AssignOrUpdateDashboard").val(1);
    $("#companyNameAccessDashboard").html($(this).attr("data-companyName"));
    $("#companyIdAccessDashboard").val($(this).val());
    $("#modelHeadingAccessDashboard").html(changeAccess);
    $("#CompleteActivationProcessDashboard").html(update);
    GetModalPopUpForAccessType();
    $("#modal-buyer-assigned-roleDashboard").val($(".ChangeAccessType").attr("data-accessType"));
});

$(document).on('click', '.CompleteActivation', function () {
    $("#AssignOrUpdateDashboard").val(0);
    $("#companyNameAccessDashboard").html($(this).attr("data-companyName"));
    $("#companyIdAccessDashboard").val($(this).val());
    $("#modelHeadingAccessDashboard").html(assignAccess);
    $("#CompleteActivationProcessDashboard").html(activate);
    GetModalPopUpForAccessType();
});


function GetModalPopUpForAccessType() {
    $("#assignAccessToBuyerDashboard").modal('show');
    $.ajax({
        async: false,
        type: 'post',
        url: '/Admin/GetBuyerAccessList',
        success: function (data) {
            var options = "<option value=\"0\">--- Select  ---</option>";
            for (var j = 0 ; j < data.accessList.length; j++) {
                options += "<option value=\"" + data.accessList[j].Id + "\">" + data.accessList[j].Name + "</option>";
            }
            $('#modal-buyer-assigned-roleDashboard').html(options);
        }
    });
}

$(document).on('click', '#CompleteActivationProcessDashboard', function () {
    var buyerPartyId = $("#companyIdAccessDashboard").val();
    var roleId = parseInt($("#modal-buyer-assigned-roleDashboard").val());
    if (roleId == 0) {
        showErrorMessage(pleaseSelectTheBuyerAccess);
        return false;
    }
    if (buyerPartyId != "" && $("#AssignOrUpdateDashboard").val() == 0) {
        $.ajax({
            type: 'post',
            url: '/Admin/ActivateBuyer',
            data: { buyerPartyId: buyerPartyId, roleId: roleId },
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
    else if (buyerPartyId != "" && $("#AssignOrUpdateDashboard").val() == 1) {
        $.ajax({
            type: 'post',
            url: '/Admin/ChangeBuyerAccessType',
            data: { buyerPartyId: buyerPartyId, roleId: roleId },
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
    $("#assignAccessToBuyerDashboard").modal('toggle');
    FetchAllData();
});






////$(document).on('click', '.CompleteActivation', function () {
////    var companyId = $(this).val();
////    var roleId = ($("#buyer-assigned-role-" + companyId).val() != "") ? parseInt($("#buyer-assigned-role-" + companyId).val()) : 0;
////    if (roleId == 0) {
////        showErrorMessage("please select the buyer access");
////        return false;
////    }
////    if (companyId != "") {
////        $.ajax({
////            type: 'post',
////            url: '/Admin/ActivateBuyer',
////            data: { companyId: companyId, roleId: roleId },
////            async: false,
////            success: function (response) {
////                if (typeof (response) != "undefined") {
////                    if (response.success)
////                        showSuccessMessage(response.message);
////                    else
////                        showErrorMessage(response.message);
////                }
////            },
////            error: function (jqXHR, textStatus, errorThrown) {
////            }
////        });
////    }
////});

////$(document).on('click', '.UpdateAccessType', function () {
////    var companyId = parseInt($(this).val());
////    var roleId = ($("#buyer-role-" + companyId).val() != "") ? parseInt($("#buyer-role-" + companyId).val()) : 0;
////    if (roleId == 0) {
////        showErrorMessage("please select the buyer access");
////        return false;
////    }
////    if (companyId != "") {
////        $.ajax({
////            type: 'post',
////            url: '/Admin/ChangeBuyerAccessType',
////            data: { companyId: companyId, roleId: roleId },
////            async: false,
////            success: function (response) {
////                if (typeof (response) != "undefined") {
////                    if (response.success)
////                        showSuccessMessage(response.message);
////                    else
////                        showErrorMessage(response.message);
////                }
////            },
////            error: function (jqXHR, textStatus, errorThrown) {
////            }
////        });
////    }
////    FetchAllData();
////});


////$(document).on('click', '.ChangeAccessType', function () {
////    var companyId = $(this).val();
////    var select = "";
////    $.ajax({
////        async: false,
////        type: 'post',
////        url: '/Admin/GetBuyerAccessList',
////        success: function (data) {
////            var options = "<option value=\"0\">Select</option>";
////            for (var j = 0 ; j < data.accessList.length; j++) {
////                options += "<option value=\"" + data.accessList[j].RoleId + "\">" + data.accessList[j].RoleName + "</option>";
////            }
////            var accessid = "#accesscell";
////            var select = "<select id='buyer-role-" + companyId + "' class=\"form-control\">" + options + "</select>";
////            $(accessid).html(select);
////            var actionid = "#actioncell";
////            $(actionid).html("<button type='button' class='btn btn-color UpdateAccessType' style='width: 150px;' id='buyer-actions-" + companyId + "' value = '" + companyId + "'>Update Access</button>");
////        }
////    });

////});
//$(document).off("click", '.BuyerSupplierMapping').on("click", '.BuyerSupplierMapping', function () {
//    $('#Loading').show();
//    var BreadCrumb = "DashBoard";
//    if (companyId != "") {
//        $.ajax({
//            type: 'post',
//            url: '/Admin/GetBuyerSupplierMapping',
//            data: { companyId: companyId, BreadCrumb: BreadCrumb },
//            async: false,
//            success: function (response) {
//                if (typeof (response) != "undefined") {
//                    ClearAllDashBoardDivs();
//                    $("#BuyerSupplierMapping").empty();
//                    $("#BuyerSupplierMapping").show();
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

$(document).on('click', '#btnCreateUsers', function () {
    var BreadCrumb = "DashBoard";
    var UserId = 0;
    if (buyerPartyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/CreateUserForBuyerCompany',
            data: { buyerPartyId: buyerPartyId, BreadCrumb: BreadCrumb, UserId: UserId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDashBoardDivs();
                    $("#CreateUser").show();
                    $("#CreateUser").html("");
                    $("#CreateUser").html(response);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
});

function EditUser(UserId) {
    var BreadCrumb = "DashBoard";
    if (buyerPartyId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/CreateUserForBuyerCompany',
            data: { buyerPartyId: buyerPartyId, BreadCrumb: BreadCrumb, UserId: UserId },
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
}

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
//                $('#tbodyProductsForBuyer').html(output);
//                $('#tbodyProductsForBuyer').footable();
//                $('#tbodyProductsForBuyer').trigger('footable_redraw');
//            }
//            $("#assignProductToBuyer").modal('toggle');
//            $("#HeadingForModal").html(defaultProducts);
//        }

//    });
//}

//function ExportSupplierListDashBoard(buyerId, buyerName) {
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

//$(document).on('click', '#CompleteAssignDefaultProductDashBoard', function () {
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

//function AddRemove1DefaultProducts(productId, buyerId, checkBox) {

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

function AssignToAuditor(campaignId, campaignName, auditorId, campaignType) {
    $('#lblCampaignName').html(campaignName);
    $('#lblCampaignId').html(campaignId);
    $('#lblCampaignType').html(campaignType);
    $.ajax({
        type: 'post',
        url: '/Admin/GetAssignCampaignAuditorList',
        async: true,
        success: function (response) {
            var optionList = "<option value=''>---" + select + "---</option>";
            for (var i = 0; i < response.result.length; i++) {
                optionList += "<option value='" + response.result[i].Value + "'>" + response.result[i].Text + "</option>";
            }
            $("#campaignAuditor").html(optionList);
            if (auditorId != 0) {
                $("#campaignAuditor").val(auditorId);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    $('#assignCampaignToAuditor').modal('show');
}

$('#btnAssignAuditorToCampaign').click(function () {
    var auditorId = $('#campaignAuditor').val();
    var campaignId = $('#lblCampaignId').html();
    var campaignType = $('#lblCampaignType').html();
    if (auditorId != "") {
        $.ajax({
            type: 'post',
            url: '/Admin/AssignCampaignToAuditor',
            data: { auditorId: auditorId, campaignId: campaignId },
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(campaignAssignedToAuditorSuccessfully);
                    $('#assignCampaignToAuditor').modal('hide');
                    GetBuyerCampaignDetailsForDashboard(1, "", 1);
                }
                else {
                    showErrorMessage(campaignCouldNotBeAssignedToAuditor);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
});

function EditCampaign(campaignId) {
    var BreadCrumb = "Dashboard";
    if (buyerPartyId != "") {
        $.ajax({
            type: 'post',
            url: '/Campaign/CreateOrEditCampaign',
            data: { buyerId: buyerPartyId, BreadCrumb: BreadCrumb, campaignId: campaignId },
            async: false,
            success: function (response) {
                if (typeof (response) != "undefined") {
                    ClearAllDashBoardDivs();
                    $("#CreateCampaign").html(response);
                    $("#CreateCampaign").show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
}

//function AssignProductDashBoard(supplierId, buyerSupplierMappingId, companyId) {
//    var buyerId = companyId;
//    $('#assignProductToSupplierDashBoard').modal('show');
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetSupplierProduct',
//        data: { supplierId: supplierId },
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                $('#supplierProductTableBodyDashBoard').html('');
//                var output = "";
//                if (response.data.length > 0) {
//                    for (var i = 0; i < response.data.length; i++) {
//                        if (i % 2 != 0) {
//                            output += "<tr class=\"odd\">";
//                        }
//                        else {
//                            output += "<tr class=\"even\">";
//                        }

//                        output += "<td>" + response.data[i].Product.ProductName + "</td>";
//                        var checked = "fa fa-square-o";
//                        $.grep(response.data[i].BuyerSupplierProductsMapping, function (obj) {
//                            if (obj.BuyerSupplierMappingId === parseInt(buyerSupplierMappingId)) {
//                                checked = "fa fa-check-square-o";
//                            }
//                        });

//                        //$.grep(response.data[i].Product.BuyerDefaultProducts, function (obj) {

//                        //	if (obj.BuyerId === parseInt(buyerId)) {
//                        //		checked = "fa fa-check-square-o";
//                        //	}
//                        //});

//                        //$.grep(response.data[i].BuyerSupplierMapping, function (obj) {
//                        //	if (obj.BuyerId === parseInt(buyerId)) {
//                        //		$.grep(obj.ExcludedProducts, function (ep) {
//                        //			if (ep.ProductId === parseInt((response.data[i].ProductId))) {
//                        //				checked = "fa fa-square-o";
//                        //			}
//                        //		});
//                        //	}
//                        //});
//                        output += "<td style=\"text-align: center;\"><i class=\"" + checked + " cursor-pointer\" Id='product" + (i + 1) + "' onclick=ToggleCheck(this) data-spId='" + response.data[i].SupplierProductId + "' data-bsmId='" + buyerSupplierMappingId + "' data-pId='" + response.data[i].ProductId + "'></i></td></tr>";
//                    }
//                }
//                else {
//                    output = "<tr><td colspan=\"2\">" + noRecordsFound + "</td></tr>";
//                    $('#paginator').remove();
//                }
//                $('#supplierProductTableBodyDashBoard').html(output);
//                $('#supplierProductTableDashBoard').footable();
//                $('#supplierProductTableDashBoard').trigger('footable_redraw');
//            }
//        }
//    });
//}
//$(document).on('click', '#CompleteAssignProduct', function () {
//    var fit = $("#product1").hasClass("fa-check-square-o");
//    var hs = $("#product2").hasClass("fa-check-square-o");
//    var ds = $("#product3").hasClass("fa-check-square-o");
//    var fitSpId = $("#product1").attr("data-spId");
//    var hsSpId = $("#product2").attr("data-spId");
//    var dsSpId = $("#product3").attr("data-spId");
//    var buyerSupplierMappingId = $("#product1").attr("data-bsmId");
//    $.ajax({
//        type: 'post',
//        url: '/Admin/UpdateBuyerSupplierProductMapping',
//        data: { buyerSupplierMappingId: buyerSupplierMappingId, fit: fit, hs: hs, ds: ds, FITspId: fitSpId, HSspId: hsSpId, DSspId: dsSpId },
//        success: function (response) {
//            if (response.result) {
//                showSuccessMessage(response.message);
//                $("#assignProductToSupplierDashBoard").modal('hide');
//            }
//            else {
//                showErrorMessage(response.message);
//            }
//        }
//    });

//});



//function ToggleMapping(supplierId, buyerId, isDelete) {
//    $.ajax({
//        type: 'post',
//        url: '/Admin/AddRemoveBuyerSupplierMapping',
//        data: { buyerId: buyerId, supplierId: supplierId, isDelete: isDelete },
//        async: true,
//        success: function (response) {
//            if (typeof (response) != "undefined") {

//                if (response.result == true) {
//                    showSuccessMessage(response.message);
//                }
//                else {
//                    showErrorMessage(response.message);
//                }
//                GetSupplierDetailsForDashboard(1, "", 1);
//            }

//        }
//    });
//}
////function AddRemoveFromMyBSPM(supplierProductId, buyerSupplierMappingId, checkbox, productId) {
////    var isDelete = true;
////    var childElement = checkbox.firstElementChild;
////    var buyerId = "@ViewBag.CompanyId";
////    if ($(childElement).hasClass('fa-square-o')) {
////        isDelete = false;
////    }

////    $.ajax({
////        type: 'post',
////        url: '/Admin/AddBuyerSupplierProductMapping',
////        data: { supplierProductId: supplierProductId, buyerSupplierMappingId: buyerSupplierMappingId, isDelete: isDelete, productId: productId },
////        success: function (response) {

////            if (typeof (response) != "undefined") {
////                if (response.result) {
////                    if (isDelete) {
////                        $(childElement).removeClass('fa-check-square-o');
////                        $(childElement).addClass('fa-square-o');
////                    }
////                    else {
////                        $(childElement).removeClass('fa-square-o');
////                        $(childElement).addClass('fa-check-square-o');
////                    }
////                    showSuccessMessage(response.message);
////                }
////                else {
////                    showErrorMessage(response.message);
////                }

////            }
////        },
////        error: function (response) {
////            //showErrorMessage(response.message);
////        }
////    });
////}
//function EditVoucher(voucherCode) {
//    var BreadCrumb = "DashBoard";
//    $.ajax({
//        type: 'post',
//        url: '/Admin/CreateVoucher',
//        data: { companyId: companyId, BreadCrumb: BreadCrumb, voucherCode: voucherCode },
//        async: false,
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                ClearAllDashBoardDivs();
//                $("#CreateVoucher").html(response);
//                $("#CreateVoucher").show();
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//}
//function EditQuestionSet(questionSetId) {
//    var BreadCrumb = "DashBoard";
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetCreateQuestionSetTemplate',
//        data: { companyId: companyId, BreadCrumb: BreadCrumb, questionSetId: questionSetId },
//        success: function (response) {
//            if (typeof (response) != "undefined") {
//                ClearAllDashBoardDivs();
//                $("#CreateQuestionSet").html(response);
//                $("#CreateQuestionSet").show();
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//}
//function PublishQuestionSet(questionSetId, Name) {
//    $.ajax({
//        type: "POST",
//        url: "/Admin/PublishQuestionSet",
//        data: { questionSetId: questionSetId, Name: Name },
//        success: function (response) {
//            if (response && typeof (response) != "undefined") {
//                if (response == Common.LogoutAction) {
//                    Logout();
//                }
//                else {
//                    if (response.success) {
//                        showSuccessMessage(response.message);
//                        GetBuyerQuestionSetDetailsForDashboard(1, "", 1);
//                    }
//                    else {
//                        showErrorMessage(response.message);
//                    }
//                }
//            }
//        },
//        error: function (response) {

//        }
//    });
//}
//function MapQuestionSetToBuyers(IsAdd, BuyerId, questionSetId) {
//    $.ajax({
//        type: 'post',
//        data: { isAdd: IsAdd, buyerId: BuyerId, questionSetId: questionSetId },
//        url: '/Admin/AddOrRemoveBuyerMappingForQuestionSet',
//        success: function (data) {
//            if (data.result) {
//                showSuccessMessage(data.message);
//            }
//            else {
//                showErrorMessage(data.message);
//            }
//            GetBuyerQuestionSetDetailsForDashboard(1, "", 1);
//        },
//        error: function (response) {

//        }
//    });
//}
//function Create() {
//    var BreadCrumb = "DashBoard";
//    $.ajax({
//        type: "POST",
//        url: "/Admin/CreateQuestion",
//        data: { companyId: companyId, BreadCrumb: BreadCrumb },
//        success: function (response) {
//            if (response && typeof (response) != "undefined") {
//                if (response == Common.LogoutAction) {
//                    Logout();
//                }
//                else {
//                    ClearAllDashBoardDivs();
//                    ScrollToTop();
//                    $("#CreateQuestion").html(response);
//                    $("#CreateQuestion").show();
//                }
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//}
//function EditQuestion(id) {
//    var BreadCrumb = "DashBoard";
//    $.ajax({
//        type: "POST",
//        url: "/Admin/CreateQuestion",
//        data: { companyId: companyId, BreadCrumb: BreadCrumb, questionId: id },
//        success: function (response) {
//            if (response && typeof (response) != "undefined") {
//                if (response == Common.LogoutAction) {
//                    Logout();
//                }
//                else {
//                    ClearAllDashBoardDivs();
//                    ScrollToTop();
//                    $("#CreateQuestion").html(response);
//                    $("#CreateQuestion").show();
//                }
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }
//    });
//}

function ChangePassword(userId, loginId) {
    $("#txtloginIDEditPassword").val(loginId);
    $("#hdnUserId").val(userId);
    $("#txtBuyerPassword").val("");
    $("#txtConfirmBuyerPassword").val("");
    $("#BuyerEditPassword").modal('show');
}

$(document).on('click', '#ChangeBuyerPassword', function () {
    $.validator.unobtrusive.parse($('#buyerPasswordChange'));
    if (!$('#buyerPasswordChange').valid()) {
        return false;
    }
    $.ajax({
        type: 'post',
        url: '/Admin/ChangeUserPassword',
        data: $('#buyerPasswordChange').serialize(),
        dataType: "json",

        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#BuyerEditPassword').modal('hide');
            }
            else
                showErrorMessage(response.message);
            $('#hdnUsersCurrentPage').val(1);
            GetBuyerUserDetailsForDashboard(1, "", 3);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
});


$(document).on('click', '.CreateVoucher', function () {
    var BreadCrumb = "DashBoard";
    $.ajax({
        type: 'post',
        url: '/Admin/CreateOrUpdateVoucherForBuyerCompany',
        data: { buyerPartyId: buyerPartyId, BreadCrumb: BreadCrumb },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                ClearAllDashBoardDivs();
                $("#CreateVoucher").html(response);
                $("#CreateVoucher").show();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});


function GetVoucherDetailsForDashboard(pageNumber, sortParameter, sortDirection) {
    var pageSize = 5;
    var voucherstablebody = "";
    $.ajax({
        type: 'post',
        url: '/Admin/GetVoucherDetailsForDashboard',
        data: { BuyerPartyId: buyerPartyId, pageNumber: pageNumber, sortDirection: sortDirection },
        success: function (response) {
            if (response.voucherDetails.length > 0) {
                var trClass = "";
                for (var i = 0; (i < response.voucherDetails.length) ; i++) {
                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }
                    var tablerow = "<tr class=" + trClass
									+ "><td><div class='ellipsis-large'>" + response.voucherDetails[i].PromotionalCode
									+ "</div></td><td>" + ((response.voucherDetails[i].BuyerPartyId == null) ? notMappedMessage : mappedMessage)
									+ "</td><td class=\"text-align-center editVoucherAction\"><div class='btn-group' style='width: 82px; margin:auto;'><button type='button' style='width: 82px;' class='btn btn-color dropdown-toggle' data-toggle='dropdown' id='vouchers-actions'>" + actionsmessage + "<span class='caret'></span></button><ul class='dropdown-menu' style='margin-left: -77px;'><li onclick='EditVoucher(\"" + response.voucherDetails[i].PromotionalCode + "\")'><a>" + editVoucher + "</a></li></ul></div>"
									+ "</td></tr>";
                    voucherstablebody += tablerow;
                }
                $("#VouchersTableBody").html(voucherstablebody);

                if (!CanCreateVoucher) {
                    $('.editVoucherAction').hide();
                }
            }
            else {
                $('#VouchersTableBody').html("<tr><td colspan='3'>" + noRecordsFound + "</td></tr>");
            }
            $('.VouchersPaginator').html(displayLinksForSmallContainers($('#hdnVouchersCurrentPage').val(), Math.ceil(response.total / pageSize), "PromotionalCode", sortDirection, "GetVoucherDetailsForDashboard", "#hdnVouchersCurrentPage"));
            $('#VouchersTable').footable();
            $('#VouchersTable').trigger('footable_redraw');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Buyer info.');
        }
    });
}

function EditVoucher(voucherCode) {
    var BreadCrumb = "DashBoard";
    $.ajax({
        type: 'post',
        url: '/Admin/CreateOrUpdateVoucherForBuyerCompany',
        data: { BreadCrumb: BreadCrumb, voucherCode: voucherCode },
        async: false,
        success: function (response) {
            if (typeof (response) != "undefined") {
                ClearAllDashBoardDivs();
                $("#CreateVoucher").html(response);
                $("#CreateVoucher").show();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}