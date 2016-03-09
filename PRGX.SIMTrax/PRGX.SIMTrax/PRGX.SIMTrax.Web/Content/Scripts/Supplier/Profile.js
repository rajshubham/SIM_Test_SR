$(document).ready(function () {
    companyProfile();

});

function companyProfile() {
    SelectMenu("SupplierProfileTab");

    if (localStorage.profileParentNavigate == "SupplierDashboard") {
        SelectMenu("AdminSupportTab");
        $('#supplier-profile-bread-crumb').html("<a onclick=\"GoToSuppliersPage()\"> < " + backTo + " " + localStorage.companyName + " " + dashboard + "</a>");
    }
    GetCompanyDetails();
    //if (userTypeValue != undefined && (userTypeValue == 2 || userTypeValue == 4)) {
    //    IsTradingWithSupplier();
    //    IsFavouriteSupplier();
    //    IsAnswerRequested();
    //}
    //if (Common.IsFromBuyerReport) {
    //    $('#buyer-supplier-profile-bread-crumb').html("<a class=\"Navigate\" data-viewname=\"buyerHome\" data-action=\"/Buyer/Index\" data-title=\"Home\">" + home + "</a> > <a class=\"Navigate\" data-viewname=\"BuyerReports\" data-action=\"/Buyer/Reports\" data-title=\"Report\">" + reports + "</a> ><span class=\"breadCrumbCompanyName\" style=\"padding-left:6px;\">" + profile + "</span>");
    //    Common.IsFromBuyerReport = false;
    //    SelectMenu("buyerReportsTab");

    //}
    //else if (Common.IsFromBuyerHome) {
    //    $('#buyer-supplier-profile-bread-crumb').html("<a class=\"Navigate\" data-viewname=\"buyerHome\" data-action=\"/Buyer/Index\" data-title=\"Home\">" + home + "</a> ><span class=\"breadCrumbCompanyName\" style=\"padding-left:6px;\">" + profile + "</span>");
    //    Common.IsFromBuyerHome = false;
    //    SelectMenu("buyerHomeTab");
    //}
    //else if (Common.IsFromBuyerInbox) {
    //    $('#buyer-supplier-profile-bread-crumb').html("<a class=\"Navigate\" data-viewname=\"buyerHome\" data-action=\"/Buyer/Index\" data-title=\"Home\">" + home + "</a> > <a class=\"Navigate\" data-viewname=\"buyerInbox\" data-action=\"/Buyer/Inbox\" data-title=\"Inbox\">" + inbox + "</a> ><span class=\"breadCrumbCompanyName\" style=\"padding-left:6px;\">" + profile + "</span>");
    //    Common.IsFromBuyerInbox = false;
    //    SelectMenu("buyer-inbox-tab");
    //}
    //else if (Common.IsFromSupplierDashboard) {
    //    $('#supplier-profile-bread-crumb').html("<a onclick=\"GoToSuppliersPage()\"> <" + backTo + localStorage.CompanyName + dashboard + "</a>");
    //    //   $('#buyer-supplier-profile-bread-crumb').attr("onclick", "GoToSuppliersPage()");
    //    Common.IsFromSupplierDashboard = false;

    //    SelectMenu("buyer-inbox-tab");
    //}
    //else {
    //    $('#buyer-supplier-profile-bread-crumb').html("<a class=\"Navigate\" data-viewname=\"buyerHome\" data-action=\"/Buyer/Index\" data-title=\"Home\">" + home + "</a> > <a class=\"Navigate\" data-viewname=\"search\" data-action=\"/Buyer/SupplierSearch\" data-title=\"Search\">" + search + "</a> ><span class=\"breadCrumbCompanyName\" style=\"padding-left:6px;\">" + profile + "</span>");
    //}
    $('html,body').animate({
        scrollTop: 0
    }, 'fast');
}

function GoToProfileQuestionnaire(tab) {
    localStorage.GeneralInformationTab = tab;
    window.location.href = '/supplier/questionnaire/general-info';
}


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function GetCompanyDetails() {
    var url = window.location.pathname;
    var arr = url.split('/');
    var sellerPartyId = parseInt(arr[arr.length - 1]);
    companyPartyIdForProfileDetails = sellerPartyId;
    cleanProfileData();
    $.ajax({
        type: "POST",
        url: "/Common/ProfileDetails",
        data: { sellerPartyId: sellerPartyId },
        dataType: "json",
        success: function (data) {
            if (data.companyExists) {
                fillProfileDetails(data.companyDetails);
            }
            else {
                if (data.redirectUrl == '/Buyer/SupplierSearch') {
                    Navigate("search", '/Buyer/SupplierSearch');
                }
                else (data.redirectUrl == '/Common/Error')
                {
                    Navigate("error", '/Common/Error');
                }
            }
        },
        error: function (result) {
            //alert("GetCompanyDetails");
        }
    });

  
        $.ajax({
            type: "POST",
            url: "/Common/GetVerificationStatus",
            data: { sellerPartyId: sellerPartyId },
            dataType: "json",
            success: function (data) {
                FillProfileVerificationStatus(data);
            },
            error: function (result) {
                //alert("GetCompanyDetails");
            }
        });
}

function FillProfileVerificationStatus(data) {
    $('#divLastSubmittedDate').show();
    if (data.lastSubmittedDate != null && data.lastSubmittedDate.indexOf(',') != -1) {
        $('#lstSubDateMonth').html(data.lastSubmittedDate.split(',')[0]);
        $('#lstSubDateYear').html(data.lastSubmittedDate.split(',')[1]);
    }
    else if (data.lastSubmittedDate != null && data.lastSubmittedDate != "") {
        $('#lstSubDateMonth').html(data.lastSubmittedDate);
    }
    else if (data.lastSubmittedDate == "") {
        $('#lstSubDateMonth').html("-");
    }
    if (data.scoreCount != null) {
        $('.chart-date-summary').removeClass('chart-date-summary');
        $('.not-started-summary').removeClass('not-started-summary');


        for (var i = 0; i < data.scoreCount.length; i++) {
            var model = data.scoreCount[i];
            var id = model.CategoryId;
            $("#profile-verification-status-" + id).html('');
            $("#profile-verification-count-" + id).html('');
            if (model.IsDataPresent) {
                var divHtml = "<div class=\"showComplianceReportProfile\" data-compliance-type=\"" + id + "\" data-audit-ans-type=\"3\" style=\"background-color:" + Common.FlaggedColor + ";text-align:center; cursor: pointer; text-decoration: underline;\">" + model.NotVerifiedCount + ((model.NotVerifiedCount == 1) ? answer : answers) + flagged + "</div>";
                divHtml += "<div class=\"showComplianceReportProfile\" data-compliance-type=\"" + id + "\" data-audit-ans-type=\"2\" style=\"background-color:" + Common.SelfDeclaredColor + ";text-align:center; cursor: pointer; text-decoration: underline;\">" + model.MissingCount + ((model.MissingCount == 1) ? answer : answers) + selfDeclared + "</div>";
                divHtml += "<div class=\"showComplianceReportProfile\" data-compliance-type=\"" + id + "\" data-audit-ans-type=\"4\" style=\"background-color:" + Common.VerifiedColor + ";text-align:center;color:white; cursor: pointer; text-decoration: underline;\">" + model.VerifiedCount + ((model.VerifiedCount == 1) ? answer : answers) + verify + "</div>";
                $("#profile-verification-count-" + id).html(divHtml);
            }
            else {
                var divHtml = "<div class=\"not-done-box\"><div class=\"not-done-box-text\">" + answerSubmitted + "</div></div>";
                $("#profile-verification-count-" + id).html(divHtml);
            }

            if (!IsOldBrowser()) {
                if (model.IsDataPresent) {
                    $("#profile-verification-status-" + id).drawDoughnutChart([
                     { title: Flagged, value: model.NotVerifiedPercentage, color: Common.FlaggedColor },
                     { title: SelfDeclare, value: model.MissingPercentage, color: Common.SelfDeclaredColor },
                    { title: verified, value: model.VerifiedPercentage, color: Common.VerifiedColor }
                    ]);
                    $("#profile-verification-status-" + id).find('.doughnutSummaryNumber').addClass("chart-date-summary");
                    $("#profile-verification-status-" + id).find('.doughnutSummaryNumber').html(auditedOn + model.FormattedVerified);
                    $("#profile-verification-status-" + id).css('pointer-events', 'all');
                    $("#profile-verification-header-" + id).css('color', 'black !important');
                }
                else {
                    $("#profile-verification-status-" + id).drawDoughnutChart([
                   { title: "None", value: 100, color: "#E6E6E6" }
                    ]);
                    $("#profile-verification-status-" + id).find('.doughnutSummaryNumber').addClass("not-started-summary");
                    var content = model.StatusValue;
                    $("#profile-verification-status-" + id).find('.doughnutSummaryNumber').html(content);
                    $("#profile-verification-status-" + id).css('pointer-events', 'none');
                    $("#profile-verification-header-" + id).prev().css('color', '#D2D2D2  !important');

                }
            }
            else {
                var s1 = [[Flagged, model.NotVerifiedPercentage], [SelfDeclare, model.MissingPercentage], [verified, model.VerifiedPercentage]];
                var colors = [Common.FlaggedColor, Common.SelfDeclaredColor, Common.VerifiedColor];
                if (!model.IsDataPresent) {
                    s1 = [['None', 100]];
                    colors = ['#D2D2D2'];
                    $("#profile-verification-status-" + id).css('pointer-events', 'none');
                    $("#profile-verification-header-" + id).css('color', '#D2D2D2  !important');
                    var content = model.StatusValue;

                    $("#profile-verification-status-" + id).append("<div class='doughnutSummaryNumber' class=\"not-started-summary\"><div style=\"text-align:center;\">" + content + "</div>");
                }
                else {
                    $("#profile-verification-status-" + id).css('pointer-events', 'all');
                    $("#profile-verification-header-" + id).css('color', 'black  !important');
                    $("#profile-verification-status-" + id).append("<div class='doughnutSummaryNumber' class=\"chart-date-summary\"><div style=\"text-align:center;\">" + auditedOn + model.FormattedVerified + "</div>");

                }
                plot1 = $.jqplot("profile-verification-status-" + id, [s1], {
                    height: 250,
                    grid: { drawBorder: false, shadow: false },
                    seriesDefaults: {
                        // make this a donut chart.
                        seriesColors: colors,
                        renderer: $.jqplot.DonutRenderer,
                        rendererOptions: {
                            // Donut's can be cut into slices like pies.
                            sliceMargin: 0,
                            // Pies and donuts can start at any arbitrary angle.
                            startAngle: 0,
                            showDataLabels: false,
                            // By default, data labels show the percentage of the donut/pie.
                            // You can show the data 'value' or data 'label' instead.
                            dataLabels: 'value',
                            diameter: 174,
                            shadowAlpha: 0, thickness: 22
                        }
                    }
                });
            }
        }
    }
}

$(document).on('click', '.showComplianceReportProfile', function () {
    var auditAnsType = $(this).attr("data-audit-ans-type");
    var complianceType = $(this).attr("data-compliance-type");
    if (auditAnsType != "" && complianceType != "") {
        $("#auditAnsSupplierProfile").val(auditAnsType);
        $("#complianceTypeSupplierProfile").val(complianceType);
        ShowComplianceReportSupplierProfile(1, "", 1);
    }
});

$(document).on('click', '#backhomeSupplierProfile', function () {
    $('#complianceReportSupplierProfile').hide();
    $('#companyProfile').show();
});

$(document).on('change', '#auditAnsSupplierProfile', function () {
    ShowComplianceReportSupplierProfile(1, "", 1);
});

$(document).on('click', '#exportComplianceReportSupplierProfile', function () {
    var auditAnsOption = $("#auditAnsSupplierProfile").val();
    var complianceTypeOption = $("#complianceTypeSupplierProfile").val();
    var supplierId = $('#profileCompanyDetails').val();
    window.location.href = "/Common/SupplierComplianceReportDownload/?supplierId=" + supplierId + "&auditAnsOption=" + auditAnsOption + "&complianceType=" + complianceTypeOption;
});

function ShowComplianceReportSupplierProfile(pageNo, sortParameter, sortDirection) {
    var companyName = $('#CompanyName').html();
    var companyId = $('#profileCompanyDetails').val();
    var auditAnsOption = $("#auditAnsSupplierProfile").val();
    var complianceTypeOption = $("#complianceTypeSupplierProfile").val();
    var complianceTypeText = $("#complianceTypeSupplierProfile option:selected").text();
    $('#supplierProfileComplianceReportHeading').html(complianceReport + ":" + complianceTypeText + " - " + companyName);
    if (companyId != "" && auditAnsOption != "") {
        $('#complianceReportSupplierProfile').show();
        $('#companyProfile').hide();
        $('#complianceReportSupplierProfileTableBody').html('');
        $.ajax({
            type: "POST",
            url: "/Common/GetAnswersBasedOnComplianceType",
            data: { supplierId: companyId, auditAnsOption: auditAnsOption, complianceType: complianceTypeOption, pageNo: pageNo },
            success: function (response) {
                if (response != undefined) {
                    if (response.total > 0) {
                        var tableHtml = "";
                        for (var i = 0 ; i < response.flaggedList.length; i++) {
                            var item = response.flaggedList[i];
                            var cls = "odd";
                            if (i % 2 == 0) {
                                cls = "even";
                            }
                            tableHtml += "<tr class=\"" + cls + "\"><td>" + item.Question + "</td></tr>";
                        }
                        $('#complianceReportSupplierProfileTableBody').html(tableHtml);
                    }
                    else {
                        var tableRow = "<tr><td>" + noRecordFound + "</td></tr>";
                        $('#complianceReportSupplierProfileTableBody').append(tableRow);
                    }
                    $('#complianceReportSupplierProfileTable').after(displayLinks($('#hdnComplianceReportSupplierProfilePageNo').val(), Math.ceil(response.total / 10), "", 1, "ShowComplianceReportSupplierProfile", "#hdnComplianceReportSupplierProfilePageNo"));
                }
            }
        });
    }
    $('html,body').animate({
        scrollTop: 0
    }, 'fast');
}

function fillProfileDetails(data) {

    $('.breadCrumbCompanyName').html(data.CompanyName + "'s " + profile);
    $('#profileHeader').html(data.CompanyName + "'s " + profilelower);
    //if (data.Status == 4) {
    //    $('#companyComplainceScoreBoard').show();
    //    GetComplianceScores(data.CompanyId);
    //    $('#divOrganisationDetails').hide();
    //    if (userTypeValue == 2) 
    //        $('#divOrganisationDetailsHeading').html('<div class=\"col-md-9 col-sm-8 col-xs-7 panel-heading page-sub-heading-container  give-full-width \"><span class="pull-left clickable panel-collapsed"><i class="glyphicon glyphicon-chevron-right"></i></span><h3 class="float-left" style="font-size: 20px;">General Information</h3></div>');
    //    else
    //        $('#divOrganisationDetailsHeading').html('<div class=\"col-md-9 col-sm-8 col-xs-7 panel-heading page-sub-heading-container  give-full-width \"><span class="pull-left clickable panel-collapsed"><i class="glyphicon glyphicon-chevron-right"></i></span><h3 class="float-left" style="font-size: 20px;">General Information</h3></div><div class="col-md-3 col-sm-4 col-xs-5 float-right padding-responsive  give-full-width" align="center"><button type="button" class="btn btn-color col-md-9 col-sm-9 col-xs-12 give-mobile-width" onclick="GoToProfileQuestionnaire(\'OrgDetails\')"> Go to Questionnaire</button></div>');
    //}
    //else {
    $('#companyComplainceScoreBoard').hide();
    $('#divOrganisationDetails').show();
    if (userTypeValue != 1)
        $('#divOrganisationDetailsHeading').html('<div class=\"col-md-9 col-sm-8 col-xs-7 panel-heading page-sub-heading-container  give-full-width \"> <span class="pull-left clickable"><i class="glyphicon glyphicon-chevron-down"></i></span><h3 class="float-left" style="font-size: 20px;">' + generalInformation + '</h3></div>');
    else
        $('#divOrganisationDetailsHeading').html('<div class=\"col-md-9 col-sm-8 col-xs-7 panel-heading page-sub-heading-container  give-full-width \"> <span class="pull-left clickable"><i class="glyphicon glyphicon-chevron-down"></i></span><h3 class="float-left" style="font-size: 20px;">' + generalInformation + '</h3></div><div class="col-md-3 col-sm-4 col-xs-5 float-right padding-responsive  give-full-width" align="center"><button type="button" class="btn btn-color col-md-9 col-sm-9 col-xs-12 give-mobile-width" onclick="GoToProfileQuestionnaire(\'OrgDetails\')">' + goToQuestionaire + '</button></div>');
    // }
    $('#CompanyName').html(data.CompanyName);
    $('#SupplierId').html(data.SupplierId);
    $('#SellerPartyId').val(data.SellerPartyId);
    $('#profileCompanyStatus').val(data.Status);
    $('#BusinessDescription').html(data.BusinessDescription);
    if (data.CompanyLogoString != null) {
        $('#profileImgLogo').attr('src', data.CompanyLogoString)
    }
    else {
        $('#profileImgLogo').attr('src', '/Content/Images/no_logo.png')
    }
    $('#BusinessSector').html(data.BusinessSector);
    $('#CompanySize').html(data.CompanySize);
    $('#TurnOverSize').html(data.TurnOver);
    $('#CustomerSectors').html(data.CustomerSectors);
    $('#CompanyService').html(data.CompanyService);
    $('#CompanySubsidiaries').html(data.CompanySubsidiaries);
    $('#TradingName').html(data.TradingName);
    $('#FacebookAccount').html(data.FacebookAccount);
    if (data.FacebookAccount != "" && data.FacebookAccount != null && data.FacebookAccount.toLowerCase().indexOf("facebook.com") > -1) {
        if (data.FacebookAccount.indexOf("https://") > -1) {
            $('#lnkFacebookProfile').attr('href', data.FacebookAccount);
        }
        else {
            $('#lnkFacebookProfile').attr('href', "https://" + data.FacebookAccount);
        }
        $('#lnkFacebookProfile').show();
    }
    else {
        $('#lnkFacebookProfile').hide();
    }
    $('#TwitterAccount').html(data.TwitterAccount);
    if (data.TwitterAccount != "" && data.TwitterAccount != null && data.TwitterAccount.toLowerCase().indexOf("twitter.com") > -1) {
        if (data.TwitterAccount.indexOf("https://") > -1) {
            $('#lnkTwitterProfile').attr('href', data.TwitterAccount);
        }
        else {
            $('#lnkTwitterProfile').attr('href', "https://" + data.TwitterAccount);
        }
        $('#lnkTwitterProfile').show();
    }
    else {
        $('#lnkTwitterProfile').hide();
    }
    $('#LinkeldInAccount').html(data.LinkeldInAccount);
    if (data.LinkeldInAccount != "" && data.LinkeldInAccount != null && data.LinkeldInAccount.toLowerCase().indexOf("linkedin.com") > -1) {
        if (data.LinkeldInAccount.indexOf("https://") > -1) {
            $('#lnkLinkedInProfile').attr('href', data.LinkeldInAccount);
        }
        else {
            $('#lnkLinkedInProfile').attr('href', "https://" + data.LinkeldInAccount);
        }
        $('#lnkLinkedInProfile').show();
    }
    else {
        $('#lnkLinkedInProfile').hide();
    }
    $('#ProfileWebsiteLink').html(data.WebsiteLink);
    $('#EstablishedYear').html(data.EstablishedYear);
    if (data.MaxContractValue != null && data.MaxContractValue > 0) {
        $('#ProfileMaxContractValue').html(data.CurrencyCodeHtml + ' ' + Number(data.MaxContractValue).toLocaleString(cultureTwoLetterCode));
    }
    else {
        $('#ProfileMaxContractValue').html('');

    }
    if (data.MinContractValue != null && data.MinContractValue > 0) {
        $('#ProfileMinContractValue').html(data.CurrencyCodeHtml + ' ' + Number(data.MinContractValue).toLocaleString(cultureTwoLetterCode));
    }
    else {
        $('#ProfileMinContractValue').html('');

    }
    if (data.ContactDetails.length > 0) {
        for (var i = 0 ; i < data.ContactDetails.length; i++) {
            switch (data.ContactDetails[i].ContactType) {
                case 515:
                    fillPrimaryContactDetails(data.ContactDetails[i]);
                    break;
                case 518:
                    fillProcurementContactDetails(data.ContactDetails[i]);
                    break;
                case 519:
                    fillHSContactDetails(data.ContactDetails[i]);
                    break;
                case 516:
                    fillAccountsContactDetails(data.ContactDetails[i]);
                    break;
                case 517:
                    fillSustainabilityContactDetails(data.ContactDetails[i]);
                    break;
            }
        }
    }
    $('#buyer-address-list-table-rows').html('');
    if (data.AddressDetails.length > 0) {
        for (var i = 0; i < data.AddressDetails.length; i++) {
            var address = data.AddressDetails[i];
            var colorClass = "odd";
            if (i % 2 == 0) { colorClass = "even" }
            var newRow = "<tr class=\"" + colorClass + "\"  ><td >" + address.AddressTypeValue + "</td><td>" + address.Line1 + "</td><td>" + ((address.Line2 != null) ? address.Line2 : " ") + "</td><td>" + address.City + "</td>";
            newRow += "<td>" + ((address.State != null) ? address.State : " ") + "</td><td>" + ((address.ZipCode != null) ? address.ZipCode : " ") + "</td><td>" + address.CountryName + "</td></tr>";
            $('#buyer-address-list-table-rows').append(newRow);
        }
    }
    else {
        var row = "<tr><td colspan=\"3\">" + noRecordFound + "</td></tr>";
        $('#buyer-address-list-table-rows').append(row);
    }
    $('#buyer-address-list-table-data').footable();
    $('#buyer-address-list-table-data').trigger('footable_redraw');

    if (data.IsBuyer) {
        //$('#lnkViewDiscussion').attr('data-action', "/Buyer/Inbox/" + data.CompanyId);
        if (data.IsTradingSupplier) {
            $('#markAsTradSup').prop('checked', true);
        }
        else {

            $('#markAsTradSup').prop('checked', false);
        }
        if (data.IsFavouriteSupplier) {
              $('#markAsFavSup').prop('checked', true);
        }
        else {
             $('#markAsFavSup').prop('checked', false);
        }
        Common.SupplierIdToBuyerInbox = data.CompanyId;
        Common.SupplierNameToBuyerInbox = data.CompanyName;
        $('#buyer-bankdetails-list-table-rows').html('');
        if (data.BankDetails.length > 0) {
            for (var i = 0; i < data.BankDetails.length; i++) {
                var rowClass = "odd";
                if (i % 2 == 0) { rowClass = "even"; }
                var bank = data.BankDetails[i];
                var row = "<tr class=\"" + rowClass + "\"><td>" + bank.AccountName + "</td><td>" + bank.AccountNumber + "</td><td>" + bank.SwiftCode + "</td><td>" + bank.BranchIdentifier + "</td>";
                row += "<td>" + bank.BankName + "</td><td>" + bank.Address + "</td><td>" + bank.CountryName + "</td><td>" + bank.PreferredMode + "</td></tr>";
                $('#buyer-bankdetails-list-table-rows').append(row);
            }
        }
        else {
            var row = "<tr><td colspan=\"3\">" + noRecordFound + "</td></tr>";
            $('#buyer-bankdetails-list-table-rows').append(row);
        }
        $('#BuyerBankDetails').show();
        $('#buyer-bankdetails-list-table-data').footable();
        $('#buyer-bankdetails-list-table-data').trigger('footable_redraw');

        $('#buyer-reference-list-table-rows').html('');
        if (data.ReferenceDetails.length > 0) {
            for (var i = 0; i < data.ReferenceDetails.length; i++) {
                var reference = data.ReferenceDetails[i];
                var rowClass = "odd";
                if (i % 2 == 0) { rowClass = "even"; }
                var canContact = "No";
                if (reference.CanWeContact == true) { canContact = "Yes" }
                var row = "<tr class=\"" + rowClass + "\"><td>" + reference.ClientName + "</td><td>" + reference.ContactName + "</td><td>" + reference.JobTitle + "</td><td>" + reference.Email + "</td>";
                row += "<td>" + reference.Phone + "</td><td>" + reference.MailingAddress + "</td><td>" + reference.Fax + "</td><td>" + reference.ClientRole + "</td><td>" + canContact + "</td></tr>";
                $('#buyer-reference-list-table-rows').append(row);
            }
        }
        else {
            var row = "<tr><td colspan=\"3\">" + noRecordFound + "</td></tr>";
            $('#buyer-reference-list-table-rows').append(row);
        }
        $('#BuyerReferenceDetails').show();
        $('#buyer-reference-list-table-data').footable();
        $('#buyer-reference-list-table-data').trigger('footable_redraw');
    }
    else {
        $('#BuyerBankDetails').hide();
        $('#BuyerReferenceDetails').hide();
    }
    if ($('#BusinessDescription').height() > 0) {
        $('#lastSubmittedDateProfile').css("height", $('#BusinessDescription').height() + "px");
        $('#lastSubmittedDateProfile').css("line-height", $('#BusinessDescription').height() + "px");
    }
    else {
        $('#lastSubmittedDateProfile').css("height", "inherit");
        $('#lastSubmittedDateProfile').css("line-height", "inherit");
    }
}

function fillPrimaryContactDetails(Contact) {
    $('#primaryName').html(Contact.Name);
    $('#primaryEmail').html(Contact.Email);
    $('#primaryTelephone').html(Contact.Telephone);
    $('#primaryJob').html(Contact.JobTitle);
}

function fillProcurementContactDetails(Contact) {
    $('#procurementName').html(Contact.Name);
    $('#procurementEmail').html(Contact.Email);
    $('#procurementTelephone').html(Contact.Telephone);
    $('#procurementJob').html(Contact.JobTitle);
}

function fillHSContactDetails(Contact) {
    $('#HSName').html(Contact.Name);
    $('#HSEmail').html(Contact.Email);
    $('#HSTelephone').html(Contact.Telephone);
    $('#HSJob').html(Contact.JobTitle);
}

function fillAccountsContactDetails(Contact) {
    $('#accountsName').html(Contact.Name);
    $('#accountsEmail').html(Contact.Email);
    $('#accountsTelephone').html(Contact.Telephone);
    $('#accountsJob').html(Contact.JobTitle);
}

function fillSustainabilityContactDetails(Contact) {
    $('#sustainabilityName').html(Contact.Name);
    $('#sustainabilityEmail').html(Contact.Email);
    $('#sustainabilityTelephone').html(Contact.Telephone);
    $('#sustainabilityJob').html(Contact.JobTitle);
}

function cleanProfileData() {
    $('#CompanyName').html('');
    $('#BusinessDescription').html('');
    $('#BusinessSector').html('');
    $('#CompanySizeString').html('');
    $('#PollutionLicenseString').html('');
    $('#HazardousWasteRegulationString').html('');
    $('#ControlledMineralsString').html('');
    $('#CustomerSectors').html('');
    $('#CompanyService').html('');
    $('#CompanySubsidiaries').html('');
    $('#TradingName').html('');
    $('#FacebookAccount').html('');
    $('#TwitterAccount').html('');
    $('#LinkeldInAccount').html('');
    $('#WebsiteLink').html('');
    $('#RegisteredCountryString').html('');
    $('#EstablishedYear').html('');
    $('#MaxContractValue').html('');
    $('#MinContractValue').html('');
    $('#primaryName').html("");
    $('#primaryEmail').html('');
    $('#primaryTelephone').html('');
    $('#primaryJob').html('');
    $('#procurementName').html('');
    $('#procurementEmail').html('');
    $('#procurementTelephone').html('');
    $('#procurementJob').html('');
    $('#HSName').html('');
    $('#HSEmail').html('');
    $('#HSTelephone').html('');
    $('#HSJob').html('');
    $('#accountsName').html('');
    $('#accountsEmail').html('');
    $('#accountsTelephone').html('');
    $('#accountsJob').html('');
    $('#sustainabilityName').html('');
    $('#sustainabilityEmail').html('');
    $('#sustainabilityTelephone').html('');
    $('#sustainabilityJob').html('');
}



function IsAnswerRequested() {
    var obj = { supplierId: companyIdForProfileDetails };
    $.ajax({
        cache: false,
        type: "POST",
        url: "/Common/IsAnswerRequested",
        data: JSON.stringify(obj),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response != null) {
                $('.required-data-summary').html(notRequired);
                $('#view-approved-answer-1').children(':first').removeClass('viewAnswerBuyer-Active').addClass('viewAnswerBuyer-InActive');
                $('#view-approved-answer-2').children(':first').removeClass('viewAnswerBuyer-Active').addClass('viewAnswerBuyer-InActive');
                $('#view-approved-answer-3').children(':first').removeClass('viewAnswerBuyer-Active').addClass('viewAnswerBuyer-InActive');
                $('#view-approved-answer-5').children(':first').removeClass('viewAnswerBuyer-Active').addClass('viewAnswerBuyer-InActive');
                if (response.ansPermissionList.length > 0) {
                    $('#btnRequestAnsFromProfile').hide();
                    $('#txtRequestAnswers').show();
                    $('#txtViewDiscussionProfile').html(discussion);
                    $('#viewDiscussionProfile').parent().css('width', '138px');
                    var sectionsHavingPermission = 0;
                    var sectionsRequested = 0;
                    for (var i = 0; i < response.ansPermissionList.length; i++) {
                        var requestPermission = response.ansPermissionList[i];
                        if (requestPermission.IsApproved) {
                            if (requestPermission.IsPublished) {
                                $('#view-approved-answer-' + requestPermission.ComplianceType).children(':first').addClass('viewAnswerBuyer-Active').removeClass('viewAnswerBuyer-InActive');
                            }
                            sectionsHavingPermission++;
                        }

                        sectionsRequested++;
                    }
                    if (sectionsHavingPermission != sectionsRequested) {
                        $('#txtRequestAnswers').html(access);
                    }
                    else {
                        $('#txtRequestAnswers').html(allAnswer);
                    }
                    $('#viewDiscussionProfile').addClass("lnkViewDiscussion");
                    $('#viewDiscussionProfile').css("cursor", "pointer");
                    $('#viewDiscussionProfile').prop("disabled", false);
                }
                else {
                    var status = parseInt($('#profileCompanyStatus').val());
                    $('#txtViewDiscussionProfile').html(requestAnswer);
                    $('#btnRequestAnsFromProfile').show();
                    $('#txtRequestAnswers').hide();
                    $('#viewDiscussionProfile').parent().css('width', '146px');
                    if (status <= 2) {
                        $('#viewDiscussionProfile').css("cursor", "not-allowed");
                        $('#btnRequestAnsFromProfile').prop("disabled", true);
                        $('#viewDiscussionProfile').prop("disabled", true);
                    }
                    else {
                        $('#viewDiscussionProfile').addClass("lnkViewDiscussion");
                        $('#viewDiscussionProfile').css("cursor", "pointer");
                        $('#btnRequestAnsFromProfile').prop("disabled", false);
                        $('#viewDiscussionProfile').prop("disabled", false);
                    }
                    //$('#txtRequestAnswers').html(' Request access to answers');
                }
                if (response.RequiredIds.length > 0) {
                    for (var i = 0; i < response.RequiredIds.length; i++) {
                        var id = response.RequiredIds[i];
                        $('#view-require-' + id).html(required);
                    }
                }
            }
        }
    });
}

$("#markAsFavSup").click(function () {
    var companyPartyIdForProfileDetails = $('#SellerPartyId').val();
    if (companyPartyIdForProfileDetails > 0) {
        var isAdd = $(this).prop('checked');
        var obj = { isAdd: isAdd, supplierPartyId: companyPartyIdForProfileDetails };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/AddOrRemoveFavouriteSupplier",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(data.message);
                    if (data.isAdd) {

                        $('#markAsFavSup').prop('checked', true);
                    }
                    else {
                          $('#markAsFavSup').prop('checked', false);
                    }

                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (result) {
                // alert("Error");
            }
        });
    }
});

$("#markAsTradSup").click(function () {
    var companyPartyIdForProfileDetails = $('#SellerPartyId').val();
    if (companyPartyIdForProfileDetails > 0) {
        var isAdd = $(this).prop('checked');
        var obj = { isAdd: isAdd, supplierPartyId: companyPartyIdForProfileDetails };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Common/AddOrRemoveTradingSupplier",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(data.message);
                    if (data.isAdd) {
                        $('#markAsTradSup').prop('checked', true);
                    }
                    else {
                        $('#markAsTradSup').prop('checked', false);
                    }
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (result) {
                // alert("Error");
            }
        });
    }
});

$(document).on('click', '.lnkViewDiscussion', function () {
    $.ajax({
        type: 'post',
        url: '/Common/GetRequestCount',
        success: function (count) {
            if (count != undefined) {
                if (count == buyerProfileMaxRequestCount) {
                    showErrorMessage(alreadyRequest + buyerProfileMaxRequestCount + supplier);
                    return false;
                }
                else {
                    Common.IsFromSupplierSearchProfile = true;
                    Navigate('buyerInbox', '/Buyer/Inbox', 'Inbox')
                }
            }
        }
    });;
});

$(document).on('click', '.viewAnswerBuyer-Active', function () {
    var supplierId = $('#profileCompanyDetails').val();
    var complianceType = $(this).attr('data-compliance-type');
    $.ajax({
        type: "POST",
        url: "/Buyer/SetSupplierCompanyId",
        data: { supplierId: supplierId },
        success: function (response) {
            if (response != null) {
                if (response) {
                    var url = "/Buyer/ViewSupplierAnswer#" + complianceType;
                    Navigate("viewSupplierAnswer", url, browseAnswer);
                }
            }
        },
        error: function () {

        }
    });
});


function GoToSuppliersPage() {
    window.location.href = "/Admin/SupplierOrganisations";
}