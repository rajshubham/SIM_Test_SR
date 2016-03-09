var to = false;
$(document).ready(function () {
    editProfile();
});
var organisation = "";
var isContactEmailExists = false;
var currTab = "None";
function editProfile() {
    currTab = "None";
    $('#layout-header-container-fluid').addClass("header-pos-questionnaire");
    SelectMenu("SupplierQuestionnarieTab");
    var sectionId = getParameterByName('section');
    if (localStorage.GeneralInformationTab != undefined) {
        switch (localStorage.GeneralInformationTab) {
            case "OrgDetails": GetOrganisationDetails();
                localStorage.removeItem("GeneralInformationTab");
                    break;
                case "Contacts": GetContactDetails();
                    localStorage.removeItem("GeneralInformationTab");
                    break;
                case "Address": GetAddressDetails();
                    localStorage.removeItem("GeneralInformationTab");
                    break;
                default: GetProfileSummary();
                    localStorage.removeItem("GeneralInformationTab");
                    break;
            }
    }
    else {
        GetProfileSummary();
    }
    //else {
    //    var val = $(document).find('.' + sectionId).attr('data-value');
    //    switch (parseInt(val)) {
    //        case 1:
    //            GetProfileSummary();
    //            break;
    //        case 2: GetOrganisationDetails()
    //            break;
    //        case 3: GetContactDetails();
    //            break;
    //        case 4: GetAddressDetails();
    //            break;
    //        case 5: GetMarketingDetails();
    //            break;
    //        case 6: GetCapabilityDetails();
    //            break;
    //        case 7: GetBankDetails();
    //            break;
    //        case 8: GetReferenceDetails();
    //            break;
    //    }
    //}
    $('#txtSearchSector').keyup(function () {
        $('#imgProcessing').show();
        if (to) { clearTimeout(to); }
        to = setTimeout(function () {
            collapseAllUnselectedNodes();
            searchSICCode(function () { hideLoader(); });           
        }, 500);
    });
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.href);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function searchSICCode(callback) {
    $("#demoTree").jstree("search", $('#txtSearchSector').val());
    callback();
}

function hideLoader() {
    $('#imgProcessing').hide();
}


$('#profile-tab-drop-down').bind('change keyup', function () {
    var val = parseInt($('#profile-tab-drop-down').val());
    switch(val)
    {
        case 1:
            GetProfileSummary();
            break;
        case 2: GetOrganisationDetails()
            break;
        case 3: GetContactDetails();
            break;
        case 4: GetAddressDetails();
            break;
        case 5: GetMarketingDetails();
            break;
        case 6: GetCapabilityDetails();
            break;
        case 7: GetBankDetails();
            break;
        case 8: GetReferenceDetails();
            break;
    }
});
function StartProfileLoading() {
    $("#profileSideBar").css({ "margin-top": "-2%" });
    $('#summary-details-div,#company-details-div,#contact-details-div,#address-details-div,#marketing-details-div,#capability-details-div,#bank-details-div,#reference-details-div').hide();
    $('#profileSummaryTab, #profileGeneralInformationTab, #profileContactTab,#profileBankTab,#profileReferenceTab,#profileAddressTab,#profileMarketingTab,#profileCapabilityTab').removeClass('tab-selected');
}
function EndProfileLoading(divId) {
    $('#' + divId).show();
    var value = $('#' + divId).attr('data-value');
    $('#profile-tab-drop-down').val(value);
    ScrollToTop();
}

function SetSelectedTab(divId) {
    $('#' + divId).addClass('tab-selected');
    var event = $('#' + divId);
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

function SetSelectedDiv(divId) {
    $('#' + divId).show();
}
function GetProfileSummary() {
 
    currTab = "None";
    var chartColor = '#709B6B';
  
    StartProfileLoading();
    SetSelectedTab('profileSummaryTab');
    $.ajax({
        type: "POST",
        url: "/Supplier/GetSellerProfilePercentage",
        success: function (response) {
            if (response != null && typeof (response) != "undefined") {
                if (response == Common.Logout) {
                    Logout();
                }
                else {
                    $("#profileSummaryChart").html('');
                    $("#divCompanyProfilePercentage").html(response.CompanyDetailsScore + "%");
                    $("#divCompanyProfilePercentageBar").css("width", response.CompanyDetailsScore + "%");
                    $("#divContactPercentage").html(response.ContactDetailsScore + "%");
                    $("#divContactPercentageBar").css("width", response.ContactDetailsScore + "%");
                    $("#divAddressPercentage").html(response.AddressDetailsScore + "%");
                    $("#divAddressPercentageBar").css("width", response.AddressDetailsScore + "%");
                    $("#divMarketingPercentage").html(response.MarketingDetailsScore + "%");
                    $("#divMarketingPercentageBar").css("width", response.MarketingDetailsScore + "%");
                    $("#divCapabilityPercentage").html(response.CapabilityDetailsScore + "%");
                    $("#divCapabilityPercentageBar").css("width", response.CapabilityDetailsScore + "%");
                    var companyPercentage = parseInt(response.CompanyDetailsScore);
                    if (companyPercentage < 30) {
                        $("#divCompanyProfilePercentageBar").css("background-color", "#8a2529 !important");
                    }
                    else if (companyPercentage > 30 && companyPercentage < 70) {
                        $("#divCompanyProfilePercentageBar").css("background-color", "#e7a614 !important");
                    }
                    else {
                        $("#divCompanyProfilePercentageBar").css("background-color", "#709b6b !important");
                    }
                    var contactPercentage = parseInt(response.ContactDetailsScore);
                    if (contactPercentage < 30) {
                        $("#divContactPercentageBar").css("background-color", "#8a2529 !important");
                    }
                    else if (contactPercentage > 30 && contactPercentage < 70) {
                        $("#divContactPercentageBar").css("background-color", "#e7a614 !important");
                    }
                    else {
                        $("#divContactPercentageBar").css("background-color", "#709b6b !important");
                    }
                    var addressPercentage = parseInt(response.AddressDetailsScore);
                    if (addressPercentage < 30) {
                        $("#divAddressPercentageBar").css("background-color", "#8a2529 !important");
                    }
                    else if (addressPercentage > 30 && addressPercentage < 70) {
                        $("#divAddressPercentageBar").css("background-color", "#e7a614 !important");
                    }
                    else {
                        $("#divAddressPercentageBar").css("background-color", "#709b6b !important");
                    }
                    var marketingPercentage = parseInt(response.MarketingDetailsScore);
                    if (marketingPercentage < 30) {
                        $("#divMarketingPercentageBar").css("background-color", "#8a2529 !important");
                    }
                    else if (marketingPercentage > 30 && marketingPercentage < 70) {
                        $("#divMarketingPercentageBar").css("background-color", "#e7a614 !important");
                    }
                    else {
                        $("#divMarketingPercentageBar").css("background-color", "#709b6b !important");
                    }
                    var capabilityPercentage = parseInt(response.CapabilityDetailsScore);
                    if (capabilityPercentage < 30) {
                        $("#divCapabilityPercentageBar").css("background-color", "#8a2529 !important");
                    }
                    else if (capabilityPercentage > 30 && capabilityPercentage < 70) {
                        $("#divCapabilityPercentageBar").css("background-color", "#e7a614 !important");
                    }
                    else {
                        $("#divCapabilityPercentageBar").css("background-color", "#709b6b !important");
                    }
                    var totalPercentage = parseInt(response.TotalScore);
                    if (totalPercentage < 30) {
                        chartColor = '#8A2529';
                    }
                    else if (totalPercentage > 30 && totalPercentage < 70) {
                        chartColor = '#E7A614';
                    }
                    else {
                        chartColor = '#709B6B';
                    }
                    if (!IsOldBrowser()) {
                        $("#profileSummaryChart").drawDoughnutChart([
                       { title: "Completed", value: totalPercentage, color: chartColor },
                       { title: "Not Completed", value: (100 - totalPercentage), color: "#D2D2D2" }
                        ]);
                        var summaryNumber = "<div style=\"text-align:center;\">" + totalPercentage + "% of </div><div  style=\"text-align:center;\">"+questionnaireText+"</div><div style=\"text-align:center;\">"+answeredText+"</div>";
                        $('#profileSummaryChart .doughnutSummaryNumber').html(summaryNumber);
                    }
                    else {
                        var s1 = [['Completed', totalPercentage], ['Not Completed', (100 - totalPercentage)]];
                        var plot3 = $.jqplot('profileSummaryChart', [s1], {
                            height: 250,
                            grid: { drawBorder: false, shadow: false },
                            seriesDefaults: {
                                // make this a donut chart.
                                seriesColors: [chartColor, '#D2D2D2'],
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
                        $('#profileSummaryChart').append("<div class='doughnutSummaryNumber' style='top:86px !important;left:78px !important;'><div style=\"text-align:center;\">" + totalPercentage + "% of </div><div  style=\"text-align:center;\">"+questionnaireText+"</div><div style=\"text-align:center;\">"+answeredText+"</div>");
                    }
                    var count = 2;
                    if (parseInt(response.CompanyDetailsScore) > 0) { count += 1; }
                    if (parseInt(response.ContactDetailsScore) > 0) { count += 1; }
                    if (parseInt(response.MarketingDetailsScore) > 0) { count += 1; }
                    if (parseInt(response.CapabilityDetailsScore) > 0) { count += 1; }
                    if (parseInt(response.AddressDetailsScore) > 0) { count += 1; }
                    //$('#countOfCompleted').html(count + " of 7 Sections have answers");
                    $('#countOfCompleted').html(requiredSectionMessage);
                }
            }
        },
        error: function (response) {
        }
    });
    EndProfileLoading('summary-details-div');
}

function GetOrganisationDetails() {
    if (currTab != "None") {
        $("." + currTab).click(function (e) {
        }).click();
    }
    currTab = "submitCompanyDetails";
  
    StartProfileLoading();
    AddRequiredIcons();
    var start = 1900;
    var end = new Date().getFullYear();
    var options = "";
    options = "<option value=''>"+selectYearText+" </option>"
    for (var year = end ; year >= start; year--) {
        options += "<option value=\"" + year + "\">" + year + "</option>";
    }
    $('#CompanyYear').append(options);
    SetSelectedTab('profileGeneralInformationTab');
    $.ajax({
        type: 'post',
        url: '/Supplier/GetCompanyDetailsByPartyId',
        success: function (data) {
            if (typeof (data) != "undefined") {
                FillSellerProfileData(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    EndProfileLoading('company-details-div');
    return false;
}
function GetContactDetails() {
    if (currTab != "None") {
        $("." + currTab).click(function (e) {
        }).click();
    }

    currTab = "None";
    
    StartProfileLoading();
    $('.save-cancel-contacts').hide();
    $('.add-contacts-button').show();
    $('.back-to-contacts').hide();
    AddRequiredIcons();
    SetSelectedTab('profileContactTab');
    $.ajax({
        type: 'post',
        url: '/Supplier/GetContactDetailsByPartyId',
        async: true,
        success: function (data) {
            if (typeof (data) != "undefined") {
                FillSupplierContactData(data.generalContacts, data.additionalContacts);
                $('#SellerPartyId').val(data.SellerPartyId);
            }
            $('#contacts-list-table').show();
            $('#assign-contacts-list-table').hide();
            $('#contact-create-edit-form').hide();
            $('#contacts-list-table-data').footable();
            $('#contacts-list-table-data').trigger('footable_redraw');
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    EndProfileLoading('contact-details-div');
    return false;
}


function FillSupplierContactData(general, additional) {

    $('#contacts-list-table-rows').html('');
    var primaryRow = "<tr class=\"odd\"  id='contact-row-1'><td>-</td><td>-</td><td>-</td><td>-</td>";
    primaryRow += "<td>-</td><td>-</td><td id=\"relationship-role-value\" data-role=\"515\">" + salesText + "</td><td style=\"text-align:center;\"><span data-rownumber =\"1\" class=\"AddProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>-</td><td>-</td></tr>";
    var AccountRow = "<tr class=\"even\"  id='contact-row-2'><td>-</td><td>-</td><td>-</td><td>-</td>";
    AccountRow += "<td>-</td><td>-</td><td id=\"relationship-role-value\" data-role=\"516\">" + accFinRemitText + "</td><td style=\"text-align:center;\"><span data-rownumber =\"2\" class=\" AddProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>-</td><td>-</td></tr>";
    var SustainabilityRow = "<tr class=\"odd\"  id='contact-row-3'><td>-</td><td>-</td><td>-</td><td>-</td>";
    SustainabilityRow += "<td>-</td><td>-</td><td id=\"relationship-role-value\" data-role=\"517\">"+sustainabilityText+"</td><td style=\"text-align:center;\"><span data-rownumber =\"3\" class=\"AddProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>-</td><td>-</td></tr>";
    var HSRow = "<tr class=\"even\"  id='contact-row-4'><td>-</td><td>-</td><td>-</td><td>-</td>";
    HSRow += "<td>-</td><td>-</td><td id=\"relationship-role-value\" data-role=\"519\">"+healthSafetyText+"</td><td style=\"text-align:center;\"><span data-rownumber =\"4\" class=\"AddProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>-</td><td>-</td></tr>";
    var ProcurementRow = "<tr class=\"odd\"  id='contact-row-5'><td>-</td><td>-</td><td>-</td><td>-</td>";
    ProcurementRow += "<td>-</td><td>-</td><td id=\"relationship-role-value\" data-role=\"518\">"+procurementText+"</td><td style=\"text-align:center;\"><span data-rownumber =\"5\" class=\"AddProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>-</td><td>-</td></tr>";
    if (general.length > 0) {
        for (var i = 0 ; i < general.length; i++) {
            var contact = general[i];
            switch (contact.ContactType) {
                case 515:
                    primaryRow = "<tr class=\"odd\"  id='contact-row-1'><td>" + contact.FirstName.trim() + " " + contact.LastName.trim() + "</td><td>" + ((contact.JobTitle != null) ? contact.JobTitle : " ") + "</td><td>" + contact.Email + "</td><td id=\"address-value\" data-addressId=\"" + ((contact.RefAddressContactMethod != 0) ? contact.RefAddressContactMethod : "0") + "\">" + ((contact.RefAddressContactMethod != null) ? contact.MailingAddressValue : " ") + "</td>";
                    primaryRow += "<td>" + contact.Telephone + "</td><td>" + ((contact.Fax != null) ? contact.Fax : " ") + "</td><td id=\"relationship-role-value\" data-role=\"515\">" + salesText + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"1\" data-contact-party =\"" + contact.ContactPartyId + "\" data-contact=\"" + contact.Id + "\" class=\"EditProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>" + contact.AssignedCount + " " + ((contact.AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-contact-party =\"" + contact.ContactPartyId + "\" data-name=\"" + contact.FirstName + " " + contact.LastName + "\" class=\"btn btn-normal view-assigned-buyers\">" + assignButton + "</button></td></tr>";
                    break;
                case 516:
                    AccountRow = "<tr class=\"even\"  id='contact-row-2'><td>" + contact.FirstName.trim() + " " + contact.LastName.trim() + "</td><td>" + ((contact.JobTitle != null) ? contact.JobTitle : " ") + "</td><td>" + contact.Email + "</td><td id=\"address-value\" data-addressId=\"" + ((contact.RefAddressContactMethod != 0) ? contact.RefAddressContactMethod : "0") + "\">" + ((contact.RefAddressContactMethod != null) ? contact.MailingAddressValue : " ") + "</td>";
                    AccountRow += "<td>" + contact.Telephone + "</td><td>" + ((contact.Fax != null) ? contact.Fax : " ") + "</td><td id=\"relationship-role-value\" data-role=\"516\">" + accFinRemitText + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"2\" data-contact-party =\"" + contact.ContactPartyId + "\" data-contact=\"" + contact.Id + "\" class=\"EditProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>" + contact.AssignedCount + " " + ((contact.AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-contact-party =\"" + contact.ContactPartyId + "\" data-name=\"" + contact.FirstName + " " + contact.LastName + "\" class=\"btn btn-normal view-assigned-buyers\">" + assignButton + "</button></td></tr>";
                    break;
                case 517:
                    SustainabilityRow = "<tr class=\"odd\"  id='contact-row-3'><td>" + contact.FirstName.trim() + " " + contact.LastName.trim() + "</td><td>" + ((contact.JobTitle != null) ? contact.JobTitle : " ") + "</td><td>" + contact.Email + "</td><td id=\"address-value\" data-addressId=\"" + ((contact.RefAddressContactMethod != 0) ? contact.RefAddressContactMethod : "0") + "\">" + ((contact.RefAddressContactMethod != null) ? contact.MailingAddressValue : " ") + "</td>";
                    SustainabilityRow += "<td>" + contact.Telephone + "</td><td>" + ((contact.Fax != null) ? contact.Fax : " ") + "</td><td id=\"relationship-role-value\" data-role=\"517\">" + sustainabilityText + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"3\" data-contact-party =\"" + contact.ContactPartyId + "\" data-contact=\"" + contact.Id + "\" class=\"EditProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>" + contact.AssignedCount + " " + ((contact.AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-contact-party =\"" + contact.ContactPartyId + "\" data-name=\"" + contact.FirstName + " " + contact.LastName + "\" class=\"btn btn-normal view-assigned-buyers\">" + assignButton + "</button></td></tr>";
                    break;
                case 518:
                    ProcurementRow = "<tr class=\"odd\"  id='contact-row-5'><td>" + contact.FirstName.trim() + " " + contact.LastName.trim() + "</td><td>" + ((contact.JobTitle != null) ? contact.JobTitle : " ") + "</td><td>" + contact.Email + "</td><td id=\"address-value\" data-addressId=\"" + ((contact.RefAddressContactMethod != 0) ? contact.RefAddressContactMethod : "0") + "\">" + ((contact.RefAddressContactMethod != null) ? contact.MailingAddressValue : " ") + "</td>";
                    ProcurementRow += "<td>" + contact.Telephone + "</td><td>" + ((contact.Fax != null) ? contact.Fax : " ") + "</td><td id=\"relationship-role-value\" data-role=\"518\">" + procurementText + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"5\" data-contact-party =\"" + contact.ContactPartyId + "\"  data-contact=\"" + contact.Id + "\" class=\"EditProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>" + contact.AssignedCount + " " + ((contact.AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\"  data-contact-party =\"" + contact.ContactPartyId + "\" data-name=\"" + contact.FirstName + " " + contact.LastName + "\" class=\"btn btn-normal view-assigned-buyers\">" + assignButton + "</button></td></tr>";
                    break;
                case 519:
                    HSRow = "<tr class=\"even\"  id='contact-row-4'><td>" + contact.FirstName.trim() + " " + contact.LastName.trim() + "</td><td>" + ((contact.JobTitle != null) ? contact.JobTitle : " ") + "</td><td>" + contact.Email + "</td><td id=\"address-value\" data-addressId=\"" + ((contact.RefAddressContactMethod != 0) ? contact.RefAddressContactMethod : "0") + "\">" + ((contact.RefAddressContactMethod != null) ? contact.MailingAddressValue : " ") + "</td>";
                    HSRow += "<td>" + contact.Telephone + "</td><td>" + ((contact.Fax != null) ? contact.Fax : " ") + "</td><td id=\"relationship-role-value\" data-role=\"519\">" + healthSafetyText + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"4\" data-contact-party =\"" + contact.ContactPartyId + "\" data-contact=\"" + contact.Id + "\" class=\"EditProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td>" + contact.AssignedCount + " " + ((contact.AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-contact-party =\"" + contact.ContactPartyId + "\" data-name=\"" + contact.FirstName + " " + contact.LastName + "\" class=\"btn btn-normal view-assigned-buyers\">" + assignButton + "</button></td></tr>";
                    break;
            }
        }
        $('#contacts-list-table-rows').append(primaryRow);
        $('#contacts-list-table-rows').append(AccountRow);
        $('#contacts-list-table-rows').append(SustainabilityRow);
        $('#contacts-list-table-rows').append(HSRow);
        $('#contacts-list-table-rows').append(ProcurementRow);
    }
    var rowNumber = 6;
    if (additional.length > 0) {
        for (var i = 0 ; i < additional.length; i++) {
            var contact = additional[i];
            var relationShipRole = "";
            switch (contact.ContactType) {
                case 1:
                    relationShipRole = "Sales";
                    break;
                case 5:
                    relationShipRole = "Procurement";
                    break;
                case 6:
                    relationShipRole = "Health & Safety";
                    break;
                case 2:
                    relationShipRole = "Account/Finance/ Remittance";
                    break;
                case 4:
                    relationShipRole = "Sustainability";
                    break;
            }
            var colorClass = "odd";
            if (rowNumber % 2 == 0) { colorClass = "even" }
            var newRow = "<tr class=\"" + colorClass + "\" id='contact-row-" + rowNumber + "' ><td>" + contact.FirstName + " " + contact.LastName + "</td><td>" + ((contact.JobTitle != null) ? contact.JobTitle : " ") + "</td><td>" + contact.Email + "</td><td id=\"address-value\" data-addressId=\"" + ((contact.RefAddressContactMethod != 0) ? contact.RefAddressContactMethod : "0") + "\">" + ((contact.RefAddressContactMethod != null) ? contact.MailingAddressValue : " ") + "</td>";
            newRow += "<td>" + contact.Telephone + "</td><td>" + ((contact.Fax != null) ? contact.Fax : " ") + "</td><td id=\"relationship-role-value\" data-role=\"0\">-</td><td style=\"text-align:center;\"><span  data-rownumber =\"" + rowNumber + "\" data-contact-party =\"" + contact.ContactPartyId + "\"  data-contact=\"" + contact.Id + "\" class=\"EditProfileIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span><span class=\"delete-contact-icon edit-delete-icons\" data-contact=\"" + contact.Id + "\" data-contact-party=\"" + contact.ContactPartyId + "\" data-contactName =\"" + contact.FirstName + " " + contact.LastName + "\"><i class=\"fa  fa-trash-o\"></i></span></td><td>" + contact.AssignedCount + " " + ((contact.AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-contact-party =\"" + contact.ContactPartyId + "\" data-name=\"" + contact.FirstName + " " + contact.LastName + "\" class=\"btn btn-normal view-assigned-buyers\">" + assignButton + "</button></td></tr>";
            rowNumber += 1;
            $('#contacts-list-table-rows').append(newRow);
        }
    }
    
}

$(document).on('click', '.delete-contact-icon', function () {
    var contactId = parseInt($(this).attr('data-contact'));
    var contactPartyId = parseInt($(this).attr('data-contact-party'));
    var name = $(this).attr('data-contactName');
    $.ajax({
        type: 'post',
        url: '/Supplier/BuyersAssignedToContact',
        async: true,
        data: { contactPartyId: contactPartyId },
        success: function (data) {
            var con = "buyers";
            if (data == 1) { con = "buyer" }
            $('#delete-contact-pop-up-content').html("<b>" + name + "</b> " + isAssignedToText +" " + data + "  " + con + ".<br/>" + deleteContactText)
            $('#delete-contact-pop-up').modal('show');
            $('#delete-contact-map').attr('data-contact', contactId);
            $('#delete-contact-map').attr('data-contact-party', contactPartyId);
            $('#delete-contact-map').attr('data-contactName', name);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });

});

$(document).on('click', '#delete-contact-map', function () {
    var contactId = $(this).attr('data-contact');
    var name = $(this).attr('data-contactName');
    var contactPartyId = parseInt($(this).attr('data-contact-party'));
    $('#delete-contact-pop-up').modal('hide');
    $.ajax({
        type: 'post',
        url: '/Supplier/DeleteContactById',
        async: true,
        data: { contactPartyId: contactPartyId ,contactId :contactId},
        success: function (data) {
            if (data.result) {
                showSuccessMessage(name + " " + data.message);
                GetContactDetails();
            }
            else {
                showErrorMessage(data.message);
            }
          },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});
var contactPartyId = 0;
var buyerContactPageNo = 1;
var contactName = "";
$(document).on('click', '.view-assigned-buyers', function () {
    $('.back-to-contacts').show();
    $('.add-contacts-button').hide();
    $('#buyer-name-for-contacts').val('');
    contactPartyId = $(this).attr('data-contact-party');
    contactName = $(this).attr('data-name');
    PagerLinkClick('1', "GetAssignedBuyerList", "#hdn-contacts-buyer-current-page", '', 1);
});
$(document).on('click', '#btn-assign-contacts-search', function () {
    PagerLinkClick('1', "GetAssignedBuyerList", "#hdn-contacts-buyer-current-page", '', 1);
});
$('#buyer-name-for-contacts').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        PagerLinkClick('1', "GetAssignedBuyerList", "#hdn-contacts-buyer-current-page", '', 1);
        return false;
    }

});
function GetAssignedBuyerList(pageNo, sortParameter, sortDirection) {
    buyerContactPageNo = pageNo;
    $.ajax({
        type: 'post',
        url: '/Supplier/BuyerSupplierContactsList',
        async: true,
        data: { pageNo: pageNo, sortParameter: sortParameter, sortDirection: sortDirection, buyerName: $('#buyer-name-for-contacts').val(), contactPartyId: contactPartyId },
        success: function (response) {
            $('#assign-contacts-list-table-rows').html('');
            var data = response.data;
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var rowClass = "odd";
                    if (i % 2 == 0) {
                        rowClass = "even";
                    }
                    var row = "<tr class=\"" + rowClass + "\"><td>" + data[i].BuyerName + "</td><td id=\"buyer-contact-role-" + (i + 1) + "\">" + GiveContactType(data[i].Role) + "</td>";
                    row += "<td style=\"text-align:center;\" id=\"edit-contact-role-" + (i + 1) + "\"><span class=\" edit-delete-icons edit-buyer-contact\" data-Assigned=\"" + data[i].IsAssigned + "\" data-row=\"" + (i + 1) + "\" data-buyer-party=\"" + data[i].BuyerPartyId + "\" data-contact-party=\"" + data[i].ContactPartyId + "\" data-role=\"" + data[i].Role + "\"><i class=\"fa  fa-pencil-square-o\"></i></span></td></tr>";
                    $('#assign-contacts-list-table-rows').append(row);
                }
                $('#assign-contacts-header').html(contactName + " - " + assignToClientsText);
            }
            else {
                var row = "<tr><td colspan=\"3\">"+noRecordsFound+"</td></tr>";
                $('#assign-contacts-list-table-rows').append(row);
                $('#assign-contacts-header').html(contactName + " - " + assignToClientsText);
            }
                 $('#assign-contacts-table').after(displayLinks($('#hdn-contacts-buyer-current-page').val(), Math.ceil(response.totalRecords / 10), sortParameter, sortDirection, "GetAssignedBuyerList", "#hdn-contacts-buyer-current-page"));
                $('#contacts-list-table').hide();
                $('#assign-contacts-list-table').show();
                $('#contact-create-edit-form').hide();
                ScrollToTop();
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}


$(document).on('click', '.edit-buyer-contact', function () {
    var isAssigned = $(this).attr('data-Assigned');
    var dataRow = $(this).attr('data-row');
    var contactPartyId = $(this).attr('data-contact-party');
    var buyerPartyId = $(this).attr('data-buyer-party');
    var role = $(this).attr('data-role');
    var dropDownValues = "<select class=\"form-control\"  id=\"buyer-supplier-contact-role-" + dataRow + "\"><option value=\"0\">"+noRoleText+"</option><option value=\"515\">"+salesText+"</option><option value=\"516\">"+accFinRemitText+"</option>";
    dropDownValues += "<option value=\"517\">"+sustainabilityText+"</option><option value=\"519\">"+healthSafetyText+"</option><option value=\"518\">"+procurementText+"</option></select>";
    $('#buyer-contact-role-' + dataRow).html(dropDownValues);
    $('#buyer-supplier-contact-role-' + dataRow).val(role);
    var savebutton = "<span class=\"edit-delete-icons save-buyer-contact\" data-Assigned=\"" + isAssigned + "\" data-buyer-party=\"" + buyerPartyId + "\" data-row=\"" + dataRow + "\" data-contact-party=\"" + contactPartyId + "\" data-role=\"" + role + "\"><i class=\"fa fa-floppy-o\"></i></span>";
    savebutton += "<span class=\"edit-delete-icons cancel-buyer-contact\" data-Assigned=\"" + isAssigned + "\" data-buyer-party=\"" + buyerPartyId + "\" data-row=\"" + dataRow + "\" data-contact-party=\"" + contactPartyId + "\" data-role=\"" + role + "\"><i class=\"fa fa-times\"></i></span>";
    $('#edit-contact-role-' + dataRow).html(savebutton);
});

$(document).on('click', '.cancel-buyer-contact', function () {
    var isAssigned = $(this).attr('data-Assigned');
    var dataRow = $(this).attr('data-row');
    var contactPartyId = $(this).attr('data-contact-party');
    var buyerPartyId = $(this).attr('data-buyer-party');
    var role = $(this).attr('data-role');
    var value = GiveContactType(parseInt(role));
    $('#buyer-contact-role-' + dataRow).html(value);
    var editbutton = "<span class=\" edit-delete-icons edit-buyer-contact\" data-Assigned=\"" + isAssigned + "\" data-row=\"" + dataRow + "\" data-buyer-party=\"" + buyerPartyId + "\" data-contact-party=\"" + contactPartyId + "\" data-role=\"" + role + "\"><i class=\"fa  fa-pencil-square-o\"></i></span>";
    $('#edit-contact-role-' + dataRow).html(editbutton);
});

$(document).on('click', '.save-buyer-contact', function () {
    var isAssigned = $(this).attr('data-Assigned');
    var dataRow = $(this).attr('data-row');
    var contactPartyId = $(this).attr('data-contact-party');
    var buyerPartyId = $(this).attr('data-buyer-party');
    var role = $('#buyer-supplier-contact-role-' + dataRow).val();
    $.ajax({
        type: 'post',
        url: '/Supplier/AddOrUpdateBuyerContacts',
        async: true,
        data: { isAssigned: isAssigned, buyerPartyId: buyerPartyId,contactpartyId: contactPartyId, role: role },
        success: function (data) {
            if (data) {
                showSuccessMessage("Saved Successfully");
                contactPartyId = contactPartyId;
                PagerLinkClick(buyerContactPageNo, "GetAssignedBuyerList", "#hdn-contacts-buyer-current-page", '', 1);
            }
            else {
                showErrorMessage(errorMessage);
            }
            ScrollToTop();
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});


function GiveContactType(role) {
    var relationShipRole = "No Role";
    switch (role) {
        case 515:
            relationShipRole = "Sales";
            break;
        case 518:
            relationShipRole = "Procurement";
            break;
        case 519:
            relationShipRole = "Health & Safety";
            break;
        case 516:
            relationShipRole = "Account/Finance/ Remittance";
            break;
        case 517:
            relationShipRole = "Sustainability";
            break;
    }
    return relationShipRole;
}
$(document).on('click', '.EditProfileIcon', function () {
    var rowNumber = $(this).attr('data-rownumber');
    var contactId = $(this).attr('data-contact');
    var contactPartyId = $(this).attr('data-contact-party');

    $('#create-edit-contacts-header').html(editContactText);
    EditProfileIcon(rowNumber, contactId, contactPartyId);
    ScrollToTop();
});
$(document).on('click', '.AddProfileIcon', function () {
    ClearContactForm();
    var rowNumber = $(this).attr('data-rownumber');
    var row = $('#contact-row-' + rowNumber);
    var roleValue = row.find('#relationship-role-value').attr('data-role');
    $('#contact-relationship').val(roleValue);
    $('#contacts-list-table').hide();
    $('#assign-contacts-list-table').hide();
    $('#contact-create-edit-form').show();
    ScrollToTop();

});

var contactEmailToEdit = "";

function EditProfileIcon(rowNumber, contactId, contactPartyId) {
    ClearContactForm();
    var row = $('#contact-row-' + rowNumber);
    var name = row.find("td:nth-child(1)").text();
    var names = name.split(' ');
    var FirstName = names[0];
    var LastName = "";
    if (names.length > 1) { LastName = names[names.length - 1]; }
    var Title = row.find("td:nth-child(2)").text();
    var Email = row.find("td:nth-child(3)").text();
    var Phone = row.find("td:nth-child(5)").text();
    var Fax = row.find("td:nth-child(6)").text();
    var roleValue = row.find('#relationship-role-value').attr('data-role');
    var addressValue = row.find('#address-value').attr('data-addressId');
    if (addressValue == 0) {
        addressValue = '';
    }
    contactEmailToEdit = Email;

    $('#contact-first-name').val(FirstName);
    $('#contact-last-name').val(LastName);
    $('#contact-title').val(Title);
    $('#contact-email').val(Email);
    $('#contact-address').val(addressValue);
    $('#contact-phone').val(Phone);
    $('#contact-fax').val(Fax);
    $('#contact-relationship').val(roleValue);
    $('#contact-row-id').val(rowNumber);
    $('#contact-id').val(contactId);
    $('#contact-party-id').val(contactPartyId);
    $('#contacts-list-table').hide();
    $('#assign-contacts-list-table').hide();
    $('#contact-create-edit-form').show();
}
var nonExistingGeneralAddressTypes = [];
function ClearContactForm() {
    AddAddressToDropDown();
    $('#contact-address-div').hide();
    $('.save-cancel-contacts').show();
    $('.add-contacts-button').hide();
    $('#contact-row-id').val(0);
    $('#contact-id').val(0);
    $('#contact-party-id').val(0);

    $('#contact-relationship').val(0);
    $('#contact-first-name').val("");
    $('#contact-last-name').val("");
    $('#contact-title').val("");
    $('#contact-email').val("");
    $('#contact-phone').val("");
    $('#contact-fax').val("");
    isContactEmailExists = false;
    contactEmailToEdit = "";
    RemoveBorderColor('#contact-first-name');
    RemoveBorderColor('#contact-last-name');
    RemoveBorderColor('#contact-email');
    RemoveBorderColor('#contact-phone');
    $('#contact-first-name-error').hide();
    $('#contact-last-name-error').hide();
    $('#contact-email-error').hide();
    $('#contact-phone-error').hide();

}

function AddAddressToDropDown() {
    var addressDropDown = "<option value=\"\" data-add=\"false\">"+selectAddressText+"</option><option value=\"0\" data-add=\"true\">"+addNewAddressText+"</option>";
    $.ajax({
        type: 'post',
        url: '/Supplier/GetAddressDetailsByPartyId',
        async: false,
        success: function (data) {
            if (typeof (data) != "undefined") {
                nonExistingGeneralAddressTypes = data.nonExistingGeneralAddressTypes;
                if (data.generalAddressList.length > 0) {
                    for (var i = 0; i < data.generalAddressList.length; i++) {
                        addressDropDown += "<option value=\"" + data.generalAddressList[i].RefContactMethod + "\" >" + data.generalAddressList[i].AddressTypeValue + "</option>";
                    }
                }

                if (data.additionalAddressList.length > 0) {
                    for (var i = 0; i < data.additionalAddressList.length; i++) {
                        addressDropDown += "<option value=\"" + data.additionalAddressList[i].RefContactMethod + "\" >" + data.additionalAddressList[i].AddressTypeValue + "-" + data.additionalAddressList[i].Line1 + "-" + data.additionalAddressList[i].City + "</option>";
                    }
                }
            }
            $('#contact-address').html(addressDropDown);
            $('#contact-address').val('');
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
$(document).on('click', ".back-to-contacts-list", function () {
    $('#contacts-list-table').show();
    $('#assign-contacts-list-table').hide();
    $('#contact-create-edit-form').hide();
    $('.save-cancel-contacts').hide();
    $('.back-to-contacts').hide();
    $('.add-contacts-button').show();
    GetContactDetails();
    ScrollToTop();
});

$(document).on('click', '.add-contact-to-list', function () {
    ClearContactForm();
    $('#contacts-list-table').hide();
    $('#assign-contacts-list-table').hide();
    $('#contact-create-edit-form').show();
    $('#create-edit-contacts-header').html(addContactText);
    ScrollToTop();

});

var contactModel = "";
$(document).on('click', '.save-to-contacts-list', function () {
    if (!EditContactFormValidation()) {
        return false;
    }
    var FirstName = $('#contact-first-name').val();
    var LastName = $('#contact-last-name').val();
    var Title = ($('#contact-title').val() != " ") ? $('#contact-title').val() : null;
    var Email = $('#contact-email').val();
    var MailingAddress = $('#contact-address').val();
    var addressModel = null;
    if (MailingAddress == "0") {
        var Address1 = $('#contact-address-line-1').val();
        var Address2 = ($('#contact-address-line-2').val() != "") ? $('#contact-address-line-2').val() : null;
        var City = $('#contact-address-city').val();
        var State = ($('#contact-address-state').val() != "") ? $('#contactaddress-state').val() : null;
        var Postal = $('#contact-address-postal').val();
        var Country = $('#contact-address-country').val();
        var AddressType = $('#contact-address-types').val();
        addressModel = { Id: 0, Line1: Address1, Line2: Address2, City: City, State: State, ZipCode: Postal, RefCountryId: Country, AddressType: AddressType };
    }

    var Phone = $('#contact-phone').val();
    var Fax = ($('#contact-fax').val() != " ") ? $('#contact-fax').val() : null;
    var RelationShipRole = parseInt($('#contact-relationship').val());
    var rowNumber = $('#contact-row-id').val();
    var ContactId = $('#contact-id').val();
    var ContactPartyId = $('#contact-party-id').val();
    var relationShipRole = "";
    switch (RelationShipRole) {
        case 515:
            relationShipRole = "Sales";
            break;
        case 518:
            relationShipRole = "Procurement";
            break;
        case 519:
            relationShipRole = "Health & Safety";
            break;
        case 516:
            relationShipRole = "Account/Finance/ Remittance";
            break;
        case 517:
            relationShipRole = "Sustainability";
            break;
    }
    var model = { Id: ContactId, FirstName: FirstName,ContactPartyId:ContactPartyId, LastName: LastName, JobTitle: Title, Email: Email, RefAddressContactMethod: MailingAddress, Telephone: Phone, Fax: Fax, ContactType: RelationShipRole };
    contactModel = model;
    $.ajax({
        type: 'post',
        url: '/Supplier/AddOrUpdateContactsList',
        async: true,
        data: { model: JSON.stringify(model), addressModel: JSON.stringify(addressModel) },
        success: function (data) {
            if (data.IsRoleAlreadyExists) {
                if (data.refAddressContactMethod > 0) {
                    contactModel.refAddressContactMethod = data.refAddressContactMethod;
                }
                $('#mapped-contact-pop-up-content').html("<b>" + data.ExistingRoleUser + "</b> "+alreadyMappedText+" <b>" + relationShipRole + "</b> "+mappedContactText+" <b>" + FirstName + " " + LastName + "</b> "+mapAndUnmapText+" <b> " + data.ExistingRoleUser + "</b>");
                $('#mapped-contact-pop-up').modal('show');
                AddAddressToDropDown();
                return false;
            }
            if (data.result) {
                    showSuccessMessage(data.message);
                GetContactDetails();
                ScrollToTop();
                $('.save-cancel-contacts').hide();
                $('.add-contacts-button').show();
            }
            else {
                showErrorMessage(data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});

$(document).on('click', '#automatic-contacts-map', function () {
    var ContactId = $('#contact-id').val();
    $('.save-cancel-contacts').hide();
    $('.add-contacts-button').show();
    $.ajax({
        type: 'post',
       url: '/Supplier/UpdateMappingContactTypes',
        async: true,
        data: { model: JSON.stringify(contactModel) },
        success: function (data) {
            if (data.result) {
                showSuccessMessage(data.message);
                $('#mapped-contact-pop-up').modal('hide');
                GetContactDetails();
                ScrollToTop();
            }
            else
            {
                showErrorMessage(data.message);
            }
               
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
});


function EditContactFormValidation() {
    var result = true;
    RemoveBorderColor('#contact-first-name');
    RemoveBorderColor('#contact-last-name');
    RemoveBorderColor('#contact-email');
    RemoveBorderColor('#contact-phone');
    //RemoveBorderColor('#contact-relationship');
    $('.error-text').hide();
    var isFocusSet = false;
    if ($('#contact-first-name').val() == "") {
        SetBorderColor("#contact-first-name", "red");
        $('#contact-first-name-error').show();
        if (!isFocusSet) {
            $('#contact-first-name').focus();
            isFocusSet = true;
        }
        result = false;
    }
    if ($('#contact-last-name').val() == "") {
        SetBorderColor("#contact-last-name", "red");
        $('#contact-last-name-error').show();
        if (!isFocusSet) {
            $('#contact-last-name').focus();
            isFocusSet = true;
        }
        result = false;
    }
    var isEmailThere = true;
    if ($('#contact-email').val() == "") {
        SetBorderColor("#contact-email", "red");
        isEmailThere = false;
        $('#contact-email-error').show();
        $('#contact-email-error').html(emailError);
        if (!isFocusSet) {
            $('#contact-email').focus();
            isFocusSet = true;
        }
        result = false;
    }
     
    if (isEmailThere) {
        if (!ValidateContactEmail($('#contact-email').val())) {
            SetBorderColor("#contact-email", "red");
            $('#contact-email-error').show();
            $('#contact-email-error').html(validEmail);
            result = false;
        }

    }
    //if (isContactEmailExists) {
    //    if (!isFocusSet) {
    //        $('#contact-email').focus();
    //        isFocusSet = true;
    //    }
    //    $('#contact-email-error').show();
    //    $('#contact-email-error').html('Contact Already Exists');
    //    result = false;
    //}
    var isMobileThere = true;
    if ($('#contact-phone').val() == "") {
        isMobileThere = false;
        $('#contact-phone-error').html(telephoneError);
        SetBorderColor("#contact-phone", "red");
        $('#contact-phone-error').show();
        if (!isFocusSet) {
            $('#contact-phone').focus();
            isFocusSet = true;
        }
        result = false;
    }

    if (isMobileThere) {
        if (!ValidateContactPhone($('#contact-phone').val())) {
            SetBorderColor("#contact-phone", "red");
            $('#contact-phone-error').show();
            $('#contact-phone-error').html(validPhoneNumber);
            if (!isFocusSet) {
                $('#contact-phone').focus();
                isFocusSet = true;
            }
            result = false;
        }

    }
    if ($('#contact-address').val() == "0") {
        RemoveBorderColor('#contact-address-line-1');
        RemoveBorderColor('#contact-address-city');
        RemoveBorderColor('#contact-address-postal');
        RemoveBorderColor('#contact-adress-country');
        RemoveBorderColor('#contact-address-types');
        if ($('#contact-address-line-1').val() == "") {
            SetBorderColor("#contact-address-line-1", "red");
            $('#contact-address-line-1-error').show();
            if (!isFocusSet) {
                $('#contact-address-line-1').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#contact-address-city').val() == "") {
            SetBorderColor("#contact-address-city", "red");
            $('#contact-address-city-error').show();
            if (!isFocusSet) {
                $('#contact-address-city').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#contact-address-postal').val() == "") {
            SetBorderColor("#contact-address-postal", "red");
            $('#contact-address-postal-error').show();
            if (!isFocusSet) {
                $('#contact-address-postal').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#contact-address-country').val() == "0") {
            SetBorderColor("#contact-address-country", "red");
            $('#contact-address-country-error').show();
            if (!isFocusSet) {
                $('#contact-address-country').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#contact-address-types').val() == "0") {
            SetBorderColor("#contact-address-types", "red");
            $('#contact-address-types-error').show();
            if (!isFocusSet) {
                $('#contact-address-types').focus();
                isFocusSet = true;
            }
            result = false;
        }
    }

    return result;

}

$('#contact-first-name').keyup(function () {
    if ($('#contact-first-name-error').attr('display') != "none") {
        if ($('#contact-first-name').val() != "") {
            $('#contact-first-name-error').hide();
            RemoveBorderColor('#contact-first-name');
        }
    }
});
$('#contact-last-name').keyup(function () {
    if ($('#contact-last-name-error').attr('display') != "none") {
        if ($('#contact-last-name').val() != "") {
            $('#contact-last-name-error').hide();
            RemoveBorderColor('#contact-last-name');
        }
    }
});
$('#contact-phone').keyup(function () {
    if ($('#contact-phone-error').attr('display') != "none") {
        if ($('#contact-phone').val() != "") {
            $('#contact-phone-error').hide();
            RemoveBorderColor('#contact-phone');
        }
    }
});
$('#contact-email').keyup(function () {
    if ($('#contact-email-error').attr('display') != "none") {
        if ($('#contact-email').val() != "") {
            $('#contact-email-error').hide();
            RemoveBorderColor('#contact-email');
        }
    }
});


$('#contact-email').blur(function () {
    if (!ValidateContactEmail($('#contact-email').val())) {
        $('#contact-email').focus();
        SetBorderColor("#contact-email", "red");
        $('#contact-email-error').show();
        $('#contact-email-error').html(validEmail);
    }
});
$('#contact-phone').blur(function () {
    if (!ValidateContactPhone($('#contact-phone').val())) {
        $('#contact-phone').focus();
        SetBorderColor("#contact-phone", "red");
        $('#contact-phone-error').show();
        $('#contact-phone-error').html(validPhoneNumber);
    }
});
function ValidateContactEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if (email == "" || !emailReg.test(email)) {
         return false;
    } else {
        return true;
    }
}

function ValidateContactPhone(phone) {
    var phoneReg = /^([0-9, ,+-]+)?$/;
    if (phone == "" || !phoneReg.test(phone)) {
        return false;
    } else {
        return true;
    }

}
    function GetCompanyCustomerSectors(companyId,Source) {
        $.ajax({
            type: 'post',
            url: '/Supplier/GetCompanyCustomerSectors',
            data: { companyId: companyId },
            async: true,
            success: function (customerSectors) {
                if (typeof (customerSectors) != "undefined") {
                    for (var i = 0; i < customerSectors.length; i++) {
                        $('#lblSIC' + (i + 1)).text(customerSectors[i].SectorName);
                        $('#SIC' + (i + 1)).val(customerSectors[i].SectorId);
                        if(Source == 1)
                        {
                            $('#lnkTree' + (i + 1)).hide();
                            $('#lnkSIC' + (i + 1)).hide();
                        }
                        else {
                            $('#lnkTree' + (i + 1)).hide();
                            $('#lnkSIC' + (i + 1)).show();
                        }
                    }
               
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function LoadSicCode() {
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
                            url = "/Supplier/GetIndustryCodesofSellerWithValues";
                        }
                        //else {
                        //    parentId = node.attr('id');
                        //    url = "/Supplier/GetChildren/?parentId=" + parentId;
                        //}

                        return url;
                    },
                    "success": function (retval) {
                        // 
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
            plugins: ["themes", "json_data", "ui", "checkbox", "types","search"]

        }).bind("change_state.jstree", function (e, d) {
            // 
            checked_ids = [];
            parent_Ids = [];
            //TODO :: Push selected values into array
            $("#demoTree").jstree("get_checked", null, true).each
                (function () {
                    // 
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
                })
            }

            $('#' + d.rslt.obj[0].id).find('li').each(function () {
                $.jstree._reference('#demoTree').set_type("enabled", "#" + this.id);
                //Remove disable class
                $($('#' + this.id).children('ins').next().children()[0]).removeClass('disabled');
            })

            //on uncheck of any value show select button, hide edit button & clear value in hidden field for selected sector
            $('#lnkSIC' + $('#hdnCurrentSIC').val()).hide();
            $('#lnkTree' + $('#hdnCurrentSIC').val()).show();
            $('#SIC' + $('#hdnCurrentSIC').val()).val('');
            $('#lblSIC' + $('#hdnCurrentSIC').val()).text('');

        }).bind("loaded.jstree", function (e, d) {

            $("#demoTree").jstree("get_checked", null, true).each
            (function () {
                $.jstree._reference('#demoTree').set_type("disabled", "#" + this.id);
                $($('#' + this.id).children('ins').next().children()[0]).addClass('disabled');
                $('#' + this.id).parents('li').each(function () {

                    if ($(this).hasClass('jstree-undetermined') && $(this).hasClass('jstree-closed')) {

                        $(this).removeClass('jstree-closed').addClass('jstree-open');
                    }
                    $.jstree._reference('#demoTree').set_type("disabled", "#" + this.id);
                    //Apply disable class
                    $($('#' + this.id).children('ins').next().children()[0]).addClass('disabled');
                });
            });

            //$("#demoTree").jstree("get_checked", null, true).each
            //   (function () {
            //       
            //       $.jstree._reference('#demoTree').set_type("disabled", "#" + this.id);
            //       $('#' + this.id).parents('li').each(function () {                   
            //           $.jstree._reference('#demoTree').set_type("disabled", "#" + this.id);
            //           //Apply disable class
            //           $($('#' + this.id).children('ins').next().children()[0]).addClass('disabled');
            //       });
            //   });


        });
    }

    function SelectSicCode(id) {
        DisableLastSelected();        
        $('html').addClass('body-no-scroll');
        $('#SicCodeModal').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');
        $('#hdnCurrentSIC').val(id);
        //collapseAllUnselectedNodes();   
        return false;
    }

    function EditSicCode(id) {
        DisableLastSelected();
        $('html').addClass('body-no-scroll');
        $('#SicCodeModal').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');
        $.jstree._reference('#demoTree').set_type("enabled", "#" + $('#SIC' + id).val());
        //Remove disable class
        $($('#' + $('#SIC' + id).val()).children('ins').next().children()[0]).removeClass('disabled');
        $('#hdnCurrentSIC').val(id);
        collapseAllUnselectedNodes();
        return false;
    }

    $(document).on('hide.bs.modal', '#SicCodeModal', function () {
        $('html').removeClass('body-no-scroll');
        $('#txtSearchSector').val('');
        $("#demoTree").jstree("search", $('#txtSearchSector').val());
        collapseAllUnselectedNodes();
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


    function isElementExists(elementId) {
        return $(elementId).length > 0;
    }

   
    $('input[name="IsSubsidaryStatus"]').click(function () {
        if ($(this).attr("value") == "True") {
          
            $('#ifSubsidary').show();
        }
        if ($(this).attr("value") == "False") {
            $('#ifSubsidary').hide();
        }
    });
    $('input[name="HaveDuns"]').click(function () {
        if ($(this).attr("value") == "True") {
            $('#HaveDUNS').show();
            $('label[for="DUNSNumber"]').addClass('required');
        }
        if ($(this).attr("value") == "False") {
            $('#HaveDUNS').hide();
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
   
   


    function FillSellerProfileData(data) {
      
        $('#CompanyDetailsForm #SellerPartyId').val(data.SellerPartyId);
        $('#OrganisationName').attr('readonly', false);
        $('#OrganisationName').val(data.OrganisationName);
        $('#OrganisationName').attr('readonly', true);
        $('#Trading').val(data.Trading);
        $('#CompanyRegistrationNumber').val(data.CompanyRegistrationNumber);
        $('#TypeOfCompany').val(data.TypeOfCompany);
        $('#CompanyYear').val(data.CompanyYear);

        if (data.IsSubsidaryStatus) {
            $("input[name=IsSubsidaryStatus][value='True']").prop("checked", true);
            $('#ifSubsidary').show();
            $('#UltimateParent').val(data.UltimateParent);
            $('#ParentDunsNumber').val(data.ParentDunsNumber);
            $('#IsParentAnswerdiv').hide();
        }
        else {
            $("input[name=IsSubsidaryStatus][value='False']").prop("checked", true);
            $('#UltimateParent').val('');
            $('#ParentDunsNumber').val('');
            $('#ifSubsidary').hide();
            $('#IsParentAnswerdiv').show();
        }

        if (data.IsVAT) {
            $("input[name=IsVAT][value='True']").prop("checked", true);
            $('#HaveVAT').show();
            $('#VATNumber').val(data.VATNumber);
            $('#downloadForm').show();
            $('#IsVATAnswerdiv').hide();
        }
        else {
            $("input[name=IsVAT][value='False']").prop("checked", true);
            $('#VATNumber').val('');
            $('#downloadForm').hide();
            $('#HaveVAT').hide();
            $('#IsVATAnswerdiv').show();
        }
        if (data.HaveDuns) {
            $("input[name=HaveDuns][value='True']").prop("checked", true);
            $('#HaveDUNS').show();
            $('#DUNSNumber').val(data.DUNSNumber);
            $('#HaveDunsAnswerdiv').hide();
        }
        else {
            $("input[name=HaveDuns][value='False']").prop("checked", true);
            $('#DUNSNumber').val('');
            $('#HaveDUNS').hide();
            $('#HaveDunsAnswerdiv').show();
        }
      
    }


    function GetAddressDetails() {
        if (currTab != "None") {
            $("." + currTab).click(function (e) {
            }).click();
        }
        currTab = "None";
        StartProfileLoading();
        SetSelectedTab('profileAddressTab');
        AddRequiredIcons();
        $('.save-cancel-address').hide();
        $('.add-address-button').show();
        $('.back-to-address').hide();
        $.ajax({
            type: 'post',
            url: '/Supplier/GetAddressDetailsByPartyId',
            async: true,
            success: function (data) {
                if (typeof (data) != "undefined") {
                    FillSupplierAddressDetails(data);
                }
                $('#address-list-table').show();
                $('#assign-address-list-table').hide();
                $('#address-create-edit-form').hide();
                $('#address-list-table-data').footable();
                $('#address-list-table-data').trigger('footable_redraw');
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
        EndProfileLoading("address-details-div");
        return false;
    }

function FillSupplierAddressDetails(data) {


        $('#address-list-table-rows').html('');
        var primaryRow = "<tr class=\"odd\"  id='address-row-1'><td  id=\"type-role-value\" data-role=\"509\">"+mailingText+"</td><td>-</td><td>-</td><td>-</td>";
        primaryRow += "<td>-</td><td>-</td><td>-</td><td style=\"text-align:center;\"><span  data-rownumber =\"1\"   class=\"add-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td  style=\"text-align:center\">" + naText + "</td></tr>";
        var RegisteredRow = "<tr class=\"even\"  id='address-row-2'><td  id=\"type-role-value\" data-role=\"510\">"+registeredText+"</td><td>-</td><td>-</td><td>-</td>";
        RegisteredRow += "<td>-</td><td>-</td><td>-</td><td style=\"text-align:center;\"><span  data-rownumber =\"2\"   class=\"add-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td  style=\"text-align:center\">"+naText+"</td></tr>";
        var HeadQuarterRow = "<tr class=\"odd\"  id='address-row-3'><td  id=\"type-role-value\" data-role=\"511\">"+headQuartersText+"</td><td>-</td><td>-</td><td>-</td>";
        HeadQuarterRow += "<td>-</td><td>-</td><td>-</td><td style=\"text-align:center;\"><span  data-rownumber =\"3\"   class=\"add-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td  style=\"text-align:center\">"+naText+"</td></tr>";
        var general = data.generalAddressList;
        if (general.length > 0) {
            for (var i = 0 ; i < general.length; i++) {
                var address = general[i];
                switch (parseInt(address.AddressType)) {
                    case 509:
                        primaryRow = "<tr class=\"odd\" id='address-row-1' ><td id=\"type-role-value\" data-role=\"" + (parseInt(address.AddressType)) + "\">" + address.AddressTypeValue + "</td><td>" + address.Line1 + "</td><td>" + ((address.Line2 != null) ? address.Line2 : " ") + "</td><td>" + address.City + "</td>";
                        primaryRow += "<td>" + ((address.State != null) ? address.State : " ") + "</td><td>" + ((address.ZipCode != null) ? address.ZipCode : " ") + "</td><td id=\"address-country-value\" data-country=\"" + (parseInt(address.RefCountryId)) + "\">" + address.CountryName + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"1\"  data-address=\"" + address.Id + "\" class=\"edit-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td  style=\"text-align:center\">"+naText+"</td></tr>";
                        break;
                    case 510:
                        RegisteredRow = "<tr class=\"even\" id='address-row-2' ><td id=\"type-role-value\" data-role=\"" + (parseInt(address.AddressType)) + "\">" + address.AddressTypeValue + "</td><td>" + address.Line1 + "</td><td>" + ((address.Line2 != null) ? address.Line2 : " ") + "</td><td>" + address.City + "</td>";
                        RegisteredRow += "<td>" + ((address.State != null) ? address.State : " ") + "</td><td>" + ((address.ZipCode != null) ? address.ZipCode : " ") + "</td><td id=\"address-country-value\" data-country=\"" + (parseInt(address.RefCountryId)) + "\">" + address.CountryName + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"2\"  data-address=\"" + address.Id + "\" class=\"edit-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td  style=\"text-align:center\">" + naText + "</td></tr>";
                        break;
                    case 511:
                        HeadQuarterRow = "<tr class=\"odd\" id='address-row-3' ><td id=\"type-role-value\" data-role=\"" + (parseInt(address.AddressType)) + "\">" + address.AddressTypeValue + "</td><td>" + address.Line1 + "</td><td>" + ((address.Line2 != null) ? address.Line2 : " ") + "</td><td>" + address.City + "</td>";
                        HeadQuarterRow += "<td>" + ((address.State != null) ? address.State : " ") + "</td><td>" + ((address.ZipCode != null) ? address.ZipCode : " ") + "</td><td id=\"address-country-value\" data-country=\"" + (parseInt(address.RefCountryId)) + "\">" + address.CountryName + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"3\"  data-address=\"" + address.Id + "\" class=\"edit-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span></td><td  style=\"text-align:center\">" + naText + "</td></tr>";
                        break;
                }
            }
            $('#address-list-table-rows').append(primaryRow);
            $('#address-list-table-rows').append(RegisteredRow);
            $('#address-list-table-rows').append(HeadQuarterRow);
        }
        var rowNumber = 4;
        var additional = data.additionalAddressList;
        if (additional.length > 0) {
            for (var i = 0 ; i < additional.length; i++) {
                var address = additional[i];
                var colorClass = "odd";
                if (rowNumber % 2 == 0) { colorClass = "even" }
                var newRow = "<tr class=\"" + colorClass + "\" id='address-row-" + rowNumber + "' ><td id=\"type-role-value\" data-role=\"" + (parseInt(address.AddressType)) + "\">" + address.AddressTypeValue + "</td><td>" + address.Line1 + "</td><td>" + ((address.Line2 != null) ? address.Line2 : " ") + "</td><td>" + address.City + "</td>";
                newRow += "<td>" + ((address.State != null) ? address.State : " ") + "</td><td>" + ((address.ZipCode != null) ? address.ZipCode : " ") + "</td><td id=\"address-country-value\" data-country=\"" + (parseInt(address.RefCountryId)) + "\">" + address.CountryName + "</td><td style=\"text-align:center;\"><span  data-rownumber =\"" + rowNumber + "\"  data-address=\"" + address.Id + "\" class=\"edit-address edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span><span class=\"delete-address-icon edit-delete-icons\" data-address=\"" + address.Id + "\"  data-role=\"" + (parseInt(address.AddressType)) + "\"><i class=\"fa  fa-trash-o\"></i></span></td><td  style=\"text-align:center\">" + ((parseInt(address.AddressType) == 512) ? "<button title=\"Assign Buyers\" data-addressId=\"" + address.Id + "\" class=\"btn btn-normal view-assigned-buyers-address\">" + assignButton + "</button>" : naText) + "</td></tr>";
                rowNumber += 1;
                $('#address-list-table-rows').append(newRow);
            }
        }
    }

var generalAddressTypeValue = null;

    $(document).on('click', '.edit-address', function () {
        var rowNumber = $(this).attr('data-rownumber');
        var addressId = $(this).attr('data-address');
        $('#create-edit-address-header').html(editAddressText);
        EditAddressIcon(rowNumber, addressId);
        ScrollToTop();
    });

    $(document).on('click', '.add-address', function () {
        ClearAddressForm();
        var rowNumber = $(this).attr('data-rownumber');
        var row = $('#address-row-' + rowNumber);
        var roleValue = row.find('#type-role-value').attr('data-role');
        var addresTypeValue = row.find("td:nth-child(1)").text();
        $('#address-types-div').hide();
        generalAddressTypeValue = roleValue;
        $('#address-list-table').hide();
        $('#create-edit-address-header').html(addText+" " + addresTypeValue + " " + addressText);
        $('#assign-address-list-table').hide();
        $('#address-create-edit-form').show();
        ScrollToTop();

    });
    function AppendCountriesForAddressDetails() {
        $.ajax({
            type: 'post',
            url: '/Supplier/GetCountries',
            async: false,
            success: function (data) {
                if (data.length > 0) {
                    $('#address-country').html = "";
                    var dropDownValues = "<option value=\"0\">"+selectCountryText+"</option>";
                    for (var i = 0; i < data.length; i++) {
                        dropDownValues += "<option value=\"" + data[i].Value + "\">" + data[i].Text + "</option>"
                    }
                    $('#address-country').html(dropDownValues);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    
    function EditAddressIcon(rowNumber, addressId) {
        ClearAddressForm();
        var row = $('#address-row-' + rowNumber);
        var addresTypeValue = row.find("td:nth-child(1)").text();
        var Address1 = row.find("td:nth-child(2)").text();
        var Address2 = row.find("td:nth-child(3)").text();
        var City = row.find("td:nth-child(4)").text();
        var State = row.find("td:nth-child(5)").text();
        var Postal = row.find("td:nth-child(6)").text();
        var roleValue = row.find('#type-role-value').attr('data-role');
        var countryValue = row.find('#address-country-value').attr('data-country');
        generalAddressTypeValue = roleValue;
    if (roleValue == "509" || roleValue == "510" || roleValue == "511") {
            $('#address-types-div').hide();
            $('#create-edit-address-header').html(editText+" " + addresTypeValue + " " + addressText);
        }
        else {
            $('#address-types-div').show();
            $('#create-edit-address-header').html(editAddressText);
        }
        $('#address-line-1').val(Address1);
        $('#address-line-2').val(Address2);
        $('#address-city').val(City);
        $('#address-state').val(State);
        $('#address-postal').val(Postal);
        $('#address-country').val(countryValue);
        $('#address-types').val(roleValue);
        $('#address-row-id').val(rowNumber);
        $('#address-id').val(addressId);
        $('#address-list-table').hide();
        $('#assign-address-list-table').hide();
        $('#address-create-edit-form').show();
    }

    function ClearAddressForm() {
        $('.save-cancel-address').show();
        $('.add-address-button').hide();
        AppendCountriesForAddressDetails();
        $('#address-types-div').show();
        $('#address-line-1').val('');
        $('#address-line-2').val('');
        $('#address-city').val('');
        $('#address-state').val('');
        $('#address-postal').val('');
        $('#address-country').val(0);
        $('#address-types').val(0);
        $('#address-row-id').val(0);
        $('#address-id').val(0);
        RemoveBorderColor('#address-line-1');
        RemoveBorderColor('#address-city');
        RemoveBorderColor('#address-postal');
        RemoveBorderColor('#address-country');
        RemoveBorderColor('#address-types');
        $('.error-text').hide();

    }

    $(document).on('click', ".back-to-address-list", function () {
        $('.save-cancel-address').hide();
        $('.add-address-button').show();
        $('#address-list-table').show();
        $('#assign-address-list-table').hide();
        $('.back-to-address').hide();
        $('#address-create-edit-form').hide();
        GetAddressDetails();
        ScrollToTop();
    });

    $(document).on('click', '.add-address-to-list', function () {
        ClearAddressForm();
        $('#address-list-table').hide();
        $('#assign-address-list-table').hide();
        $('#address-create-edit-form').show();
        $('#create-edit-address-header').html(addAddressText);
        ScrollToTop();

    });

    var addressModel = "";

    $(document).on('click', '.save-to-address-list', function () {
        if (!EditAddressFormValidation()) {
            return false;
        }
        $('#save-cancel-address').hide();
        $('.add-address-button').hide();
    var Address1 = $('#address-line-1').val();
    var Address2 = ($('#address-line-2').val() != "") ? $('#address-line-2').val() : null;
     var City = $('#address-city').val();
    var State = ($('#address-state').val() != "") ? $('#address-state').val() : null;
        var Postal = $('#address-postal').val();
        var Country = $('#address-country').val();
        var AddressType = $('#address-types').val();
        if (AddressType == "0" || AddressType  ==  null)
        {
            AddressType = generalAddressTypeValue;
            generalAddressTypeValue = null;
        }
    var AddressId = $('#address-id').val();
        
        var model = { Id: AddressId, Line1: Address1, Line2: Address2, City: City, State: State, ZipCode: Postal, RefCountryId: Country, AddressType: AddressType };
        addressModel = model;
        $.ajax({
            type: 'post',
            url: '/Supplier/AddOrUpdateAddressList',
            async: false,
            data: { model: JSON.stringify(model) },
            success: function (data) {
               
                if (data.result) {
                    if (AddressId > 0) {
                        showSuccessMessage(data.message);
                    }
                    else {
                        showSuccessMessage(data.message);
                    }
                    GetAddressDetails();
                    ScrollToTop();
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });



function EditAddressFormValidation() {
        RemoveBorderColor('#address-line-1');
        RemoveBorderColor('#address-city');
        RemoveBorderColor('#address-postal');
        RemoveBorderColor('#adress-country');
        RemoveBorderColor('#address-types');
        $('.error-text').hide();
        var result = true;
        var isFocusSet = false;
        if ($('#address-line-1').val() == "") {
            SetBorderColor("#address-line-1", "red");
            $('#address-line-1-error').show();
            if (!isFocusSet) {
                $('#address-line-1').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#address-city').val() == "") {
            SetBorderColor("#address-city", "red");
            $('#address-city-error').show();
            if (!isFocusSet) {
                $('#address-city').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#address-postal').val() == "") {
            SetBorderColor("#address-postal", "red");
            $('#address-postal-error').show();
            if (!isFocusSet) {
                $('#address-postal').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#address-country').val() == "0") {
            SetBorderColor("#address-country", "red");
            $('#address-country-error').show();
            if (!isFocusSet) {
                $('#address-country').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#address-types').val() == "0" && generalAddressTypeValue == null) {
            SetBorderColor("#address-types", "red");
            $('#address-types-error').show();
            if (!isFocusSet) {
                $('#address-types').focus();
                isFocusSet = true;
            }
            result = false;
        }

        return result;
    }

    $('#address-line-1').keyup(function () {
        if ($('#address-line-1-error').attr('display') != "none") {
            if ($('#address-line-1').val() != "") {
                $('#address-line-1-error').hide();
                RemoveBorderColor('#address-line-1');
            }
        }
    });
    $('#address-city').keyup(function () {
        if ($('#address-city-error').attr('display') != "none") {
            if ($('#address-city').val() != "") {
                $('#address-city-error').hide();
                RemoveBorderColor('#address-city');
            }
        }
    });
    $('#address-postal').keyup(function () {
        if ($('#address-postal-error').attr('display') != "none") {
            if ($('#address-postal').val() != "") {
                $('#address-postal-error').hide();
                RemoveBorderColor('#address-postal');
            }
        }
    });
    $('#address-country').change(function () {
        if ($('#address-country-error').attr('display') != "none") {
            if ($('#address-country').val() != "0") {
                $('#address-country-error').hide();
                RemoveBorderColor('#address-country');
            }
        }
    });
    $('#address-types').change(function () {
        if ($('#address-types-error').attr('display') != "none") {
            if ($('#address-types').val() != "0") {
                $('#address-types-error').hide();
                RemoveBorderColor('#address-types');
            }
        }
    });

    $(document).on('click', '.delete-address-icon', function () {
        var addressId = $(this).attr('data-address');
        var type = $(this).attr('data-role');
        var checkRemittance = false;
        //Remittance Addres Value
        if (type == 512) {
            checkRemittance = true;
        }
        $.ajax({
            type: 'post',
            url: '/Supplier/BuyersAssignedToAddress',
            async: true,
            data: { addressId: addressId, checkRemittance: checkRemittance },
            success: function (data) {
                    var count = "buyers";
                    if (data == 1) { con = "buyer" }
                    if (data > 0) {
                        $('#delete-address-pop-up-content').html("<b>" + addressText + "</b> " + isAssignedToText + " " + data + " " + count + ".<br/>" + deleteAddressText);
                    }
            else {
                        $('#delete-address-pop-up-content').html(deleteAddressText);
                    }
                    $('#delete-address-pop-up').modal('show');
                    $('#delete-address-map').attr('data-address', addressId);
               
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });

    });

    $(document).on('click', '#delete-address-map', function () {
        var addressId = $(this).attr('data-address');
        $('#delete-address-pop-up').modal('hide');
        $.ajax({
            type: 'post',
            url: '/Supplier/DeleteAddressById',
            async: true,
        data: { addressId: addressId },
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(data.message);
                    GetAddressDetails();
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });

    var addressId = 0;
    var buyerAddressPageNo = 1;
    $(document).on('click', '.view-assigned-buyers-address', function () {
        $('.back-to-address').show();
        $('.add-address-button').hide();
        $('#buyer-name-for-address').val('');
        addressId = $(this).attr('data-addressId');
        PagerLinkClick('1', "GetAssignedBuyerListForAddress", "#hdn-address-buyer-current-page", '', 1);
    });
    $(document).on('click', '#btn-assign-address-search', function () {
        PagerLinkClick('1', "GetAssignedBuyerListForAddress", "#hdn-address-buyer-current-page", '', 1);
    });
    $('#buyer-name-for-address').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            PagerLinkClick('1', "GetAssignedBuyerListForAddress", "#hdn-address-buyer-current-page", '', 1);
            return false;
        }

    });
    function GetAssignedBuyerListForAddress(pageNo, sortParameter, sortDirection) {
        buyerAddressPageNo = pageNo;
        $.ajax({
            type: 'post',
            url: '/Supplier/BuyerSupplierAddressDetails',
            async: true,
            data: { pageNo: pageNo, sortParameter: sortParameter, sortDirection: sortDirection, buyerName: $('#buyer-name-for-address').val(), addressId: addressId },
            success: function (response) {
                $('#assign-address-list-table-rows').html('');
                var data = response.data;
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var rowClass = "odd";
                        if (i % 2 == 0) {
                            rowClass = "even";
                        }
                        var row = "<tr class=\"" + rowClass + "\"><td>" + data[i].BuyerName + "</td><td style=\"text-align:center;\"><span style='cursor:pointer;' onclick='AddOrRemoveBuyerSupplierAddress(" +
                         !(data[i].IsAssigned) + "," + data[i].RefPartyId + "," + data[i].RefContactMethod + ")'>"
                         + (data[i].IsAssigned ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</span></td></tr>";
                        $('#assign-address-list-table-rows').append(row);
                    }
                }
                else {
                    var row = "<tr><td colspan=\"3\">"+noRecordsFound+"</td></tr>";
                    $('#assign-address-list-table-rows').append(row);
                }
                $('#assign-address-table').after(displayLinks($('#hdn-address-buyer-current-page').val(), Math.ceil(response.totalRecords / 10), sortParameter, sortDirection, "GetAssignedBuyerListForReference", "#hdn-address-buyer-current-page"));
                $('#address-list-table').hide();
                $('#assign-address-list-table').show();
                $('#address-create-edit-form').hide();
                ScrollToTop();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function AddOrRemoveBuyerSupplierAddress(IsAdd, RefPartyId, refContactMethodId) {
        var reqData = { isAdd: IsAdd, buyerPartyId: RefPartyId, refContactMethodId: refContactMethodId }
        $.ajax({
            type: 'post',
            data: JSON.stringify(reqData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: '/Supplier/AddOrRemoveBuyerSupplierAddressDetails',
            success: function (data) {
                if (data.result) {
                      showSuccessMessage(data.message);
                    
                }
                else {
                    showErrorMessage(data.message);
                }
                PagerLinkClick(buyerAddressPageNo, "GetAssignedBuyerListForAddress", "#hdn-address-buyer-current-page", '', 1);
            }
        });
    }

    $(document).on('click', '.submitCompanyDetails', function () {
        if (!CompanyDetailsFormValidation()) {
            var els = document.querySelector('.input-validation-error');
            if (els != null) {
                els.focus();
            }
            return false;
        }
       
            $.ajax({
                type: 'post',
                url: '/Supplier/SaveCompanyDetails',
                data: $('#CompanyDetailsForm').serialize(),
                async: false,
                success: function (response) {
                    if (response.result)
                        showSuccessMessage(response.message);
                    else
                        showErrorMessage(response.message);
                    $('html,body').animate({
                        scrollTop: 0
                    }, 'slow');
                    if (currTab == "submitCompanyDetails") {
                    GetProfileSummary();
                      }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        return false;
    });

    var focusHeight = 0;

    function UploadSellerLogo() {
        if (typeof FormData == "undefined") {
            //setCookie("oldBrowser", "true");
            return false;
        }
        else {
            var data = new FormData();

            var files = $("#logo").get(0).files;
            // Add the uploaded image content to the form data collection
            var isLogo = false;
            if (files.length > 0) {
                data.append("Logo", files[0]);
                isLogo = true;
            }
            data.append("IsLogo", isLogo);
            //var forms = $("#W9W8Form").get(0).files;
            //var isForm = false;
            //if (forms.length > 0) {
            //    data.append("VATForm", forms[0]);
            //    isForm = true;
            //}
            //data.append("IsForm", isForm);

            var ajaxRequest = $.ajax({
                type: "POST",
                url: "/Supplier/UploadSellerLogo",
                async: false,
                contentType: false,
                processData: false,
                data: data,
                success: function (response) {

                    if (!response.result)
                        showErrorMessage(response.message);
                   
                }

            });

            ajaxRequest.done(function (xhr, textStatus) {
                // Do other operation
            });
            return true;
        }
    }

    function CompanyDetailsFormValidation() {

        var result = true;
        RemoveBorderColor('#UltimateParent');
        RemoveBorderColor('#VATNumber');
        RemoveBorderColor('#DUNSNumber');
        $('.error-text').hide();

        $.validator.unobtrusive.parse($('#CompanyDetailsForm'))
        if (!$('#CompanyDetailsForm').valid()) {
            result = false;
        }

        if ($('input[name=IsSubsidaryStatus]:Checked').val() == "True") {
            if ($('#UltimateParent').val() == "") {
                SetBorderColor("#UltimateParent", "red");
                $('#UltimateParentError').show();
                result = false;
            }
            if ($('#ParentDunsNumber').val() == "") {
                SetBorderColor("#ParentDunsNumber", "red");
                $('#ParentDunsNumberError').show();
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
         
        }
        else if ($('input[name=IsVAT]:Checked').val() == "False") {
            $('#VATNumber').val('');
        }
        if ($('input[name=HaveDuns]:Checked').val() == "True") {
            if ($('#DUNSNumber').val() == "") {
                SetBorderColor("#DUNSNumber", "red");
                $('#DUNSNumberError').show();
                result = false;
            }
        }
      
        return result;
    }

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

    function PreviewImage() {

        if (ValidateFileEditProfile()) {
            if (typeof FileReader != "undefined") {
                var oFReader = new FileReader();
                oFReader.readAsDataURL(document.getElementById("logo").files[0]);
                oFReader.onload = function (oFREvent) {
                    document.getElementById("imgLogoPreview").src = oFREvent.target.result;
                };
                $('#imagePreview').show();
            }
            UploadSellerLogo();
            return false;
        }
        else {
            document.getElementById("imgLogoPreview").src = "";
            $('#imagePreview').hide();
            $("#logo").val("");
        }
        return false;
    }

    function ValidateFileEditProfile() {
        var validFilesTypes = ["png", "jpg", "jpeg"];
        var fileValue = document.getElementById('logo').value;
        if (fileValue.length < 1) {
            showWarningMessage(uploadImageValidation);
            return false;
        }
        var file = document.getElementById("logo");
        var path = file.value;
        var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
        var isValidFile = false;
        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile) {
            var filemessage = String.format(fileExtensionValidation, ext);
            showWarningMessage(filemessage);
        }
        return isValidFile;
    }

function GetBankDetails() {
        if (currTab != "None") {
            $("." + currTab).click(function (e) {
            }).click();
        }
        currTab = "None";
      
        StartProfileLoading();
        SetSelectedTab('profileBankTab');

        $.ajax({
            type: 'post',
            url: '/Supplier/GetBankDetailsByOrganisationId',
            async: true,
            success: function (data) {

                if (typeof (data) != "undefined") {
                    $('#bankdetails-list-table-rows').html('');
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                            var rowClass = "odd";
                            if (i % 2 == 0) { rowClass = "even"; }
                            var row = "<tr class=\"" + rowClass + "\" id=\"bank-row-" + (i + 1) + "\"><td>" + data[i].AccountName + "</td><td>" + data[i].AccountNumber + "</td><td>" + data[i].BranchIdentifier + "</td><td>" + data[i].SwiftCode + "</td>";
                            row += "<td>" + data[i].IBAN + "</td><td>" + data[i].BankName + "</td><td>" + data[i].Address + "</td><td>" + data[i].CountryName + "</td>";
                            row += "<td>" + data[i].PreferredMode + "</td><td><span id=\"country-value\" data-rownumber =\"" + (i + 1) + "\"  data-bank=\"" + data[i].Id + "\" data-legal=\"" + data[i].RefLegalEntity + "\" data-country=\"" + data[i].RefCountryId + "\" class=\"EditBankIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span><span class=\"delete-bank-icon edit-delete-icons\" data-name=\"" + data[i].AccountName + "\" data-bank=\"" + data[i].Id + "\"><i class=\"fa  fa-trash-o\"></i></span></td><td>" + data[i].AssignedCount + " " + ((data[i].AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-bankId=\"" + data[i].Id + "\" data-name=\"" + data[i].AccountName + "\" class=\"btn btn-normal view-assigned-buyers-bank\">" + assignButton + "</button></td></tr>"
                            $('#bankdetails-list-table-rows').append(row);
                        }
                    }
                    else {
                        var row = "<tr><td colspan=\"3\">"+noRecordsFound+"</td></tr>";
                        $('#bankdetails-list-table-rows').append(row);
                    }
                    $('#bankdetails-list-table').show();
                    $('#assign-bankdetails-list-table').hide();
                    $('#bankdetails-create-edit-form').hide();
                    $('.save-cancel-bankdetails').hide();
                    $('.add-bankdetails-button').show();
                    $('.back-to-bankdetails').hide();
                    $('#bankdetails-list-table-data').footable();
                    $('#bankdetails-list-table-data').trigger('footable_redraw');
                    ScrollToTop();
                }
             },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
        EndProfileLoading("bank-details-div");
        return false;
    }

    $(document).on('click', ".back-to-bankdetails-list", function () {
        $('.save-cancel-bankdetails').hide();
        $('.add-bankdetails-button').show();
        $('#bankdetails-list-table').show();
        $('#assign-bankdetails-list-table').hide();
        $('.back-to-bankdetails').hide();
        $('#bankdetails-create-edit-form').hide();
        GetBankDetails();
        ScrollToTop();
    });
    $(document).on('click', '.add-bankdetails-to-list', function () {
        ClearBankForm();
        $('#create-edit-bankdetails-header').html(addBankDetailsText);
        AppendCountriesForBankDetails();
        $('#bankdetails-list-table').hide();
        $('#assign-bankdetails-list-table').hide();
        $('#bankdetails-create-edit-form').show();
        ScrollToTop();
    });
    var bankId = 0;
    var accountName = "";
    var buyerBankPageNo = 1;
    $(document).on('click', '.view-assigned-buyers-bank', function () {
        $('.back-to-bankdetails').show();
        $('.add-bankdetails-button').hide();
        bankId = $(this).attr('data-bankId');
        accountName = $(this).attr('data-name')
        $('#buyer-name-for-bank').val('');
        PagerLinkClick('1', "GetAssignedBuyerListForBank", "#hdn-bank-buyer-current-page", '', 1);

    });
    $(document).on('click', '#btn-assign-bank-search', function () {
        PagerLinkClick('1', "GetAssignedBuyerListForBank", "#hdn-bank-buyer-current-page", '', 1);
    });
    $('#buyer-name-for-bank').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            PagerLinkClick('1', "GetAssignedBuyerListForBank", "#hdn-bank-buyer-current-page", '', 1);
            return false;
        }

    });
    function GetAssignedBuyerListForBank(pageNo, sortParameter, sortDirection) {
        buyerBankPageNo = pageNo;
        $.ajax({
            type: 'post',
            url: '/Supplier/BuyerSupplierBankList',
            async: true,
            data: { pageNo: pageNo, sortParameter: sortParameter, sortDirection: sortDirection, buyerName: $('#buyer-name-for-bank').val(), bankId: bankId },
            success: function (response) {
                $('#assign-bankdetails-list-table-rows').html('');
                var data = response.data;
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var rowClass = "odd";
                        if (i % 2 == 0) {
                            rowClass = "even";
                        }
                        var row = "<tr class=\"" + rowClass + "\"><td>" + data[i].BuyerName + "</td><td style=\"text-align:center;\"><span style='cursor:pointer;' onclick='AddOrRemoveBuyerSupplierBankDetails(" +
                         !(data[i].IsAssigned) + "," + data[i].RefPartyId + "," + data[i].BankId + ")'>"
                         + (data[i].IsAssigned ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</span></td></tr>";
                        $('#assign-bankdetails-list-table-rows').append(row);
                    }
                    $('#assign-bankdetails-header').html(accountName + "-"+ assignToClientsText);
                }
                else {
                    var row = "<tr><td colspan=\"3\">No Records Found</td></tr>";
                    $('#assign-bankdetails-list-table-rows').append(row);
                    $('#assign-bankdetails-header').html(accountName + "- "+assignToClientsText);
                }
                    $('#assign-bankdetails-table').after(displayLinks($('#hdn-bank-buyer-current-page').val(), Math.ceil(response.totalRecords / 10), sortParameter, sortDirection, "GetAssignedBuyerListForBank", "#hdn-bank-buyer-current-page"));
                    $('#bankdetails-list-table').hide();
                    $('#assign-bankdetails-list-table').show();
                    $('#bankdetails-create-edit-form').hide();
                    ScrollToTop();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function AddOrRemoveBuyerSupplierBankDetails(IsAdd, RefPartyId, bank_Id) {
        var reqData = { isAdd: IsAdd, buyerPartyId: RefPartyId, bankId: bank_Id }
        $.ajax({
            type: 'post',
            data: JSON.stringify(reqData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: '/Supplier/AddOrRemoveBuyerSupplierBankDetails',
            success: function (data) {
                if (data) {
                        showSuccessMessage(data.message);
                  
                }
                else {
                    showErrorMessage(data.message);
                }
                bankId = bank_Id;
                PagerLinkClick(buyerBankPageNo, "GetAssignedBuyerListForBank", "#hdn-bank-buyer-current-page", '', 1);
            }
        });
    }
    function ClearBankForm() {
        $('.save-cancel-bankdetails').show();
        $('.add-bankdetails-button').hide();
        $('#bankdetails-row-id').val(0);
        $('#bankdetails-id').val(0);
        $('#bankdetails-country').val(0);
        $('#bankdetails-account-name').val("");
        $('#bankdetails-account-no').val("");
        $('#bankdetails-swift-code').val("");
        $('#bankdetails-sort-code').val("");
        $('#bankdetails-bic-code').val("");
        $('#bankdetails-bank-name').val("");
        $('#bankdetails-address').val("");
        $('#bankdetails-preferred').val("");
        RemoveBorderColor('#bankdetails-country');
        RemoveBorderColor('#bankdetails-account-name');
        RemoveBorderColor('#bankdetails-account-no');
        RemoveBorderColor('#bankdetails-sort-code');
        RemoveBorderColor('#bankdetails-swift-code');
        RemoveBorderColor('#bankdetails-bic-code');
        RemoveBorderColor('#bankdetails-bank-name');
        RemoveBorderColor('#bankdetails-address');
        RemoveBorderColor('#bankdetails-preferred');
        $('#bankdetails-country-error').hide();
        $('#bankdetails-account-name-error').hide();
        $('#bankdetails-account-no-error').hide();
        $('#bankdetails-swift-code-error').hide();
        $('#bankdetails-bic-code-error').hide();
        $('#bankdetails-bank-name-error').hide();
        $('#bankdetails-address-error').hide();
        $('#bankdetails-preferred-error').hide();
        $('#bankdetails-sort-code-error').hide();
    }
function AppendCountriesForBankDetails() {
        $.ajax({
            type: 'post',
            url: '/Supplier/GetCountries',
            async: false,
            success: function (data) {
            if (data.length > 0) {
                    $('#bankdetails-country').html = "";
                    var dropDownValues = "<option value=\"0\">"+selectCountryText+"</option>";
                for (var i = 0; i < data.length; i++) {
                    dropDownValues += "<option value=\"" + data[i].Value + "\">" + data[i].Text + "</option>"
                    }
                    $('#bankdetails-country').html(dropDownValues);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    $(document).on('click', '.EditBankIcon', function () {
        var rowNumber = $(this).attr('data-rownumber');
        var bankId = $(this).attr('data-bank');
        AppendCountriesForBankDetails();
        EditBankDetails(rowNumber, bankId);
        ScrollToTop();
    });

function EditBankDetails(rowNumber, bankId) {
    $('#create-edit-bankdetails-header').html(editBankDetailsText);
        ClearBankForm();
        var row = $('#bank-row-' + rowNumber);
        var AccountName = row.find("td:nth-child(1)").text();
        var AccountNo = row.find("td:nth-child(2)").text();
        var SortCode = row.find("td:nth-child(3)").text();
        var SwiftCode = row.find("td:nth-child(4)").text();
        var BicCode = row.find("td:nth-child(5)").text();
        var BankName = row.find("td:nth-child(6)").text();
        var Address = row.find("td:nth-child(7)").text();
        var PreferredMode = row.find("td:nth-child(9)").text();
        var Country = row.find('#country-value').attr('data-country');
        var legal = row.find('#country-value').attr('data-legal');
        $('#bankdetails-row-id').val(rowNumber);
        $('#bankdetails-id').val(bankId);
        $('#bankdetails-country').val(Country);
        $('#bankdetails-legal').val(legal);
        $('#bankdetails-account-name').val(AccountName);
        $('#bankdetails-account-no').val(AccountNo);
        $('#bankdetails-sort-code').val(SortCode);
        $('#bankdetails-swift-code').val(SwiftCode);
        $('#bankdetails-bic-code').val(BicCode);
        $('#bankdetails-bank-name').val(BankName);
        $('#bankdetails-address').val(Address);
        $('#bankdetails-preferred').val(PreferredMode);
        $('#bankdetails-list-table').hide();
        $('#bankdetails-create-edit-form').show();
    }

    $(document).on('click', '.save-to-bankdetails-list', function () {
    if (!EditBankFormValidation()) {
            return false;
        }
        $('#save-cancel-bankdetails').hide();
        $('.add-bankdetails-button').show();
        var AccountName = $('#bankdetails-account-name').val();
        var AccountNo = $('#bankdetails-account-no').val();
        var SwiftCode = $('#bankdetails-swift-code').val();
        var IBAN = $('#bankdetails-bic-code').val();
        var BankName = $('#bankdetails-bank-name').val();
        var Address = $('#bankdetails-address').val();
        var PreferredMode = $('#bankdetails-preferred').val();
        var Country = parseInt($('#bankdetails-country').val());
        var rowNumber = $('#bankdetails-row-id').val();
        var BankDetailsId = $('#bankdetails-id').val();
        var SortCode = $('#bankdetails-sort-code').val();
        var legalEntity = parseInt($('#bankdetails-legal').val());
        var model = { Id: BankDetailsId, AccountName: AccountName, AccountNumber: AccountNo, BranchIdentifier: SortCode, SwiftCode: SwiftCode, IBAN: IBAN, BankName: BankName, Address: Address, RefCountryId: Country, PreferredMode: PreferredMode, RefLegalEntity: legalEntity };
        $.ajax({
            type: 'post',
            url: '/Supplier/AddOrUpdateBankDetails',
            async: true,
            data: { model: JSON.stringify(model) },
            success: function (data) {
            if (data.result) {
                    showSuccessMessage(data.message);
                    GetBankDetails();
                    ScrollToTop();
                }
            else {
                showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });
    function EditBankFormValidation() {
        var result = true;
        RemoveBorderColor('#bankdetails-country');
        RemoveBorderColor('#bankdetails-account-name');
        RemoveBorderColor('#bankdetails-account-no');
        RemoveBorderColor('#bankdetails-swift-code');
        RemoveBorderColor('#bankdetails-bic-code');
        RemoveBorderColor('#bankdetails-bank-name');
        RemoveBorderColor('#bankdetails-address');
        RemoveBorderColor('#bankdetails-preferred');
        $('.error-text').hide();
        var isFocusSet = false;
        if ($('#bankdetails-account-name').val() == "") {
            SetBorderColor("#bankdetails-account-name", "red");
            $('#bankdetails-account-name-error').show();
            if (!isFocusSet) {
                $('#bankdetails-account-name').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#bankdetails-account-no').val() == "") {
            SetBorderColor("#bankdetails-account-no", "red");
            $('#bankdetails-account-no-error').show();
            if (!isFocusSet) {
                $('#bankdetails-account-no').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#bankdetails-sort-code').val() == "") {
            SetBorderColor("#bankdetails-sort-code", "red");
            isEmailThere = false;
            $('#bankdetails-sort-code-error').show();
            if (!isFocusSet) {
                $('#bankdetails-sort-code').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#bankdetails-swift-code').val() == "") {
            SetBorderColor("#bankdetails-swift-code", "red");
            isEmailThere = false;
            $('#bankdetails-swift-code-error').show();
            if (!isFocusSet) {
                $('#bankdetails-swift-code').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#bankdetails-bic-code').val() == "") {
            SetBorderColor("#bankdetails-bic-code", "red");
            isEmailThere = false;
            $('#bankdetails-bic-code-error').show();
            if (!isFocusSet) {
                $('#bankdetails-bic-code').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#bankdetails-bank-name').val() == "") {
            SetBorderColor("#bankdetails-bank-name", "red");
            isEmailThere = false;
            $('#bankdetails-bank-name-error').show();
            if (!isFocusSet) {
                $('#bankdetails-bank-name').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#bankdetails-address').val() == "") {
            SetBorderColor("#bankdetails-address", "red");
            isEmailThere = false;
            $('#bankdetails-address-error').show();
            if (!isFocusSet) {
                $('#bankdetails-address').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#bankdetails-country').val() == "0") {
            SetBorderColor("#bankdetails-country", "red");
            isEmailThere = false;
            $('#bankdetails-country-error').show();
            if (!isFocusSet) {
                $('#bankdetails-country').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if ($('#bankdetails-preferred').val() == "") {
            SetBorderColor("#bankdetails-preferred", "red");
            isEmailThere = false;
            $('#bankdetails-preferred-error').show();
            if (!isFocusSet) {
                $('#bankdetails-preferred').focus();
                isFocusSet = true;
            }
            result = false;
        }

        return result;
    }

    $('#bankdetails-account-name').keyup(function () {
        if ($('#bankdetails-account-name-error').attr('display') != "none") {
            if ($('#bankdetails-account-name').val() != "") {
                $('#bankdetails-account-name-error').hide();
                RemoveBorderColor('#bankdetails-account-name');
            }
        }
    });
    $('#bankdetails-account-no').keyup(function () {
        if ($('#bankdetails-account-no-error').attr('display') != "none") {
            if ($('#bankdetails-account-no').val() != "") {
                $('#bankdetails-account-no-error').hide();
                RemoveBorderColor('#bankdetails-account-no');
            }
        }
    });
    $('#bankdetails-swift-code').keyup(function () {
        if ($('#bankdetails-swift-code-error').attr('display') != "none") {
            if ($('#bankdetails-swift-code').val() != "") {
                $('#bankdetails-swift-code-error').hide();
                RemoveBorderColor('#bankdetails-swift-code');
            }
        }
    });
    $('#bankdetails-sort-code').keyup(function () {
        if ($('#bankdetails-sort-code-error').attr('display') != "none") {
            if ($('#bankdetails-sort-code').val() != "") {
                $('#bankdetails-sort-code-error').hide();
                RemoveBorderColor('#bankdetails-sort-code');
            }
        }
    });
    $('#bankdetails-bic-code').keyup(function () {
        if ($('#bankdetails-bic-code-error').attr('display') != "none") {
            if ($('#bankdetails-bic-code').val() != "") {
                $('#bankdetails-bic-code-error').hide();
                RemoveBorderColor('#bankdetails-bic-code');
            }
        }
    });
    $('#bankdetails-bank-name').keyup(function () {
        if ($('#bankdetails-bank-name-error').attr('display') != "none") {
            if ($('#bankdetails-bank-name').val() != "") {
                $('#bankdetails-bank-name-error').hide();
                RemoveBorderColor('#bankdetails-bank-name');
            }
        }
    });
    $('#bankdetails-address').keyup(function () {
        if ($('#bankdetails-address-error').attr('display') != "none") {
            if ($('#bankdetails-address').val() != "") {
                $('#bankdetails-address-error').hide();
                RemoveBorderColor('#bankdetails-address');
            }
        }
    });
    $('#bankdetails-preferred').keyup(function () {
        if ($('#bankdetails-preferred-error').attr('display') != "none") {
            if ($('#bankdetails-preferred').val() != "") {
                $('#bankdetails-preferred-error').hide();
                RemoveBorderColor('#bankdetails-preferred');
            }
        }
    });
    $('#bankdetails-country').change(function () {
        if ($('#bankdetails-country-error').attr('display') != "none") {
            if ($('#bankdetails-country').val() != "0") {
                $('#bankdetails-country-error').hide();
                RemoveBorderColor('#bankdetails-country');
            }
        }
    });

    $(document).on('click', '.delete-bank-icon', function () {
        var name = $(this).attr('data-name');
        var bankId = $(this).attr('data-bank');
        $('#delete-bank-pop-up-content').html(deleteAccountMessage+' <b>' + name + '</b> ?')
        $('#delete-bank-map').attr('data-bank', bankId);
        $('#delete-bank-map').attr('data-name', name);
        $('#delete-bank-pop-up').modal('show');
    });
    $(document).on('click', '#delete-bank-map', function () {
        var bankId = $(this).attr('data-bank');
        var name = $(this).attr('data-name');
        $('#delete-bank-pop-up').modal('hide');
        $.ajax({
            type: 'post',
            url: '/Supplier/DeleteBankAccountById',
            async: true,
            data: { bankId: bankId },
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(name + data.message);
                    GetBankDetails();
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });

    function GetReferenceDetails() {
        if (currTab != "None") {
            $("." + currTab).click(function (e) {
            }).click();
        }
        currTab = "None";
       
        StartProfileLoading();
        SetSelectedTab('profileReferenceTab');
        $('.save-cancel-reference').hide();
        $('.add-reference-button').show();
        $('.back-to-reference').hide();

        $.ajax({
            type: 'post',
            url: '/Supplier/GetReferenceDetailsBySellerId',
            async: true,
            success: function (data) {
                if (typeof (data) != "undefined") {
                    $('#reference-list-table-rows').html('');
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            var rowClass = "odd";
                            if (i % 2 == 0) { rowClass = "even"; }
                            var canContact = "No";
                        if (data[i].CanWeContact == true) { canContact = "Yes" }
                            var row = "<tr class=\"" + rowClass + "\" id=\"reference-row-" + (i + 1) + "\"><td>" + data[i].ClientName + "</td><td>" + data[i].ContactName + "</td><td>" + data[i].JobTitle + "</td>";
                            row += "<td>" + data[i].Email + "</td><td style=\"text-align:right\">" + data[i].Phone + "</td><td>" + data[i].MailingAddress + "</td><td>" + data[i].Fax + "</td>";
                            row += "<td>" + data[i].ClientRole + "</td><td>" + canContact + "</td><td><span id=\"contact-value\" data-rownumber =\"" + (i + 1) + "\"  data-reference=\"" + data[i].Id + "\" data-canwecontact=\"" + data[i].CanWeContact + "\" class=\"EditReferenceIcon edit-delete-icons\"><i class=\"fa  fa-pencil-square-o\"></i></span><span class=\"delete-reference-icon edit-delete-icons\" data-name=\"" + data[i].ContactName + "\" data-reference=\"" + data[i].Id + "\"><i class=\"fa  fa-trash-o\"></i></span></td><td>" + data[i].AssignedCount + " " + ((data[i].AssignedCount == 1) ? "Client" : "Clients") + "</td><td><button title=\"View Buyers\" data-referenceId=\"" + data[i].Id + "\" data-name=\"" + data[i].ClientName + "\" class=\"btn btn-normal view-assigned-buyers-reference\">" + assignButton + "</button></td></tr>"
                            $('#reference-list-table-rows').append(row);
                        }
                    }
                    else {
                        var row = "<tr><td colspan=\"3\">"+noRecordsFound+"</td></tr>";
                        $('#reference-list-table-rows').append(row);
                    }
                    $('#reference-list-table').show();
                    $('#assign-reference-list-table').hide();
                    $('#reference-create-edit-form').hide();
                   $('#reference-list-table-data').footable();
                   $('#reference-list-table-data').trigger('footable_redraw');
                   ScrollToTop();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
        EndProfileLoading('reference-details-div');
        return false;
    }
    $(document).on('click', ".back-to-reference-list", function () {
        $('.save-cancel-reference').hide();
        $('.add-reference-button').show();
        $('#reference-list-table').show();
        $('#assign-reference-list-table').hide();
        $('.back-to-reference').hide();
        $('#reference-create-edit-form').hide();
        GetReferenceDetails();
        ScrollToTop();
    });
    $(document).on('click', '.add-reference-to-list', function () {
        $('#create-edit-reference-header').html(addReferenceDetailsText);
        ClearReferenceForm();
        AppendCountriesForBankDetails();
        $('#reference-list-table').hide();
        $('#assign-reference-list-table').hide();
        $('#reference-create-edit-form').show();
        ScrollToTop();
    });
    function ClearReferenceForm() {
        $('.save-cancel-reference').show();
        $('.add-reference-button').hide();
        $('#reference-row-id').val(0);
        $('#reference-id').val(0);
        $('#reference-client-name').val("");
        $('#reference-contact-name').val("");
        $('#reference-job-title').val("");
        $('#reference-email').val("");
        $('#reference-phone').val("");
        $('#reference-address').val("");
        $('#reference-fax').val("");
        $('#reference-client-role').val("");
        $("input[name=reference-can-we-contact][value='true']").prop("checked", true);
        RemoveBorderColor('#reference-contact-name');
        RemoveBorderColor('#reference-client-name');
        RemoveBorderColor('#reference-job-title');
        RemoveBorderColor('#reference-email');
        RemoveBorderColor('#reference-phone');
        RemoveBorderColor('#reference-address');
        RemoveBorderColor('#reference-fax');
        RemoveBorderColor('#reference-client-role');
        $('#reference-contact-name-error').hide();
        $('#reference-client-name-error').hide();
        $('#reference-job-title-error').hide();
        $('#reference-email-error').hide();
        $('#reference-phone-error').hide();
        $('#reference-address-error').hide();
        $('#reference-fax-error').hide();
        $('#reference-client-role-error').hide();
    }

    $(document).on('click', '.EditReferenceIcon', function () {
        var rowNumber = $(this).attr('data-rownumber');
        var referenceId = $(this).attr('data-reference');
        EditReferenceDetails(rowNumber, referenceId);
        ScrollToTop();
    });

    function EditReferenceDetails(rowNumber, referenceId) {
        ClearReferenceForm();
        $('#create-edit-reference-header').html(editReferenceDetailsText);
       var row = $('#reference-row-' + rowNumber);
        var ClientName = row.find("td:nth-child(1)").text();
        var ContactName = row.find("td:nth-child(2)").text();
        var JobTitle = row.find("td:nth-child(3)").text();
        var Email = row.find("td:nth-child(4)").text();
        var Phone = row.find("td:nth-child(5)").text();
        var Address = row.find("td:nth-child(6)").text();
        var Fax = row.find("td:nth-child(7)").text();
        var ClientRole = row.find("td:nth-child(8)").text();
        var CanWeContact = row.find('#contact-value').attr('data-canwecontact');
        $('#reference-row-id').val(rowNumber);
        $('#reference-id').val(referenceId);
        $('#reference-client-name').val(ClientName);
        $('#reference-contact-name').val(ContactName);
        $('#reference-job-title').val(JobTitle);
        $('#reference-email').val(Email);
        $('#reference-phone').val(Phone);
        $('#reference-address').val(Address);
        $('#reference-fax').val(Fax);
        $('#reference-client-role').val(ClientRole);
        $("input[name=reference-can-we-contact][value='" + CanWeContact + "']").prop("checked", true);
        $('#reference-list-table').hide();
        $('#assign-reference-list-table').hide();
        $('#reference-create-edit-form').show();
    }

    function EditReferenceFormValidation() {
        var result = true;
        RemoveBorderColor('#reference-contact-name');
        RemoveBorderColor('#reference-client-name');
        RemoveBorderColor('#reference-job-title');
        RemoveBorderColor('#reference-email');
        RemoveBorderColor('#reference-phone');
        RemoveBorderColor('#reference-address');
        RemoveBorderColor('#reference-fax');
        RemoveBorderColor('#reference-client-role');
        $('.error-text').hide();
        var isFocusSet = false;
        if ($('#reference-contact-name').val() == "") {
            SetBorderColor("#reference-contact-name", "red");
            $('#reference-contact-name-error').show();
            if (!isFocusSet) {
                $('#reference-contact-name').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#reference-client-name').val() == "") {
            SetBorderColor("#reference-client-name", "red");
            $('#reference-client-name-error').show();
            if (!isFocusSet) {
                $('#reference-client-name').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#reference-job-title').val() == "") {
            SetBorderColor("#reference-job-title", "red");
            $('#reference-job-title-error').show();
            if (!isFocusSet) {
                $('#reference-job-title').focus();
                isFocusSet = true;
            }
            result = false;
        }
        var isEmailThere = true;
        if ($('#reference-email').val() == "") {
            SetBorderColor("#reference-email", "red");
            isEmailThere = false;
            $('#reference-email-error').show();
            $('#reference-email-error').html(emailError);
            if (!isFocusSet) {
                $('#reference-email').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if (isEmailThere) {
            if (!ValidateContactEmail($('#reference-email').val())) {
                SetBorderColor("#reference-email", "red");
                $('#reference-email-error').show();
                $('#reference-email-error').html(validEmail);
                result = false;
            }

        }
        
        var isMobileThere = true;
        if ($('#reference-phone').val() == "") {
            isMobileThere = false;
            $('#reference-phone-error').html(telephoneError);
            SetBorderColor("#reference-phone", "red");
            $('#reference-phone-error').show();
            if (!isFocusSet) {
                $('#reference-phone').focus();
                isFocusSet = true;
            }
            result = false;
        }

        if (isMobileThere) {
            if (!ValidateContactPhone($('#reference-phone').val())) {
                SetBorderColor("#reference-phone", "red");
                $('#reference-phone-error').show();
                $('#reference-phone-error').html(validPhoneNumber);
                if (!isFocusSet) {
                    $('#reference-phone').focus();
                    isFocusSet = true;
                }
                result = false;
            }

        }
        if ($('#reference-address').val() == "") {
            SetBorderColor("#reference-address", "red");
            $('#reference-address-error').show();
            if (!isFocusSet) {
                $('#reference-address').focus();
                isFocusSet = true;
            }
            result = false;
        } if ($('#reference-fax').val() == "") {
            SetBorderColor("#reference-fax", "red");
            $('#reference-fax-error').show();
            if (!isFocusSet) {
                $('#reference-fax').focus();
                isFocusSet = true;
            }
            result = false;
        }
        if ($('#reference-client-role').val() == "") {
            SetBorderColor("#reference-client-role", "red");
            $('#reference-client-role-error').show();
            if (!isFocusSet) {
                $('#reference-client-role').focus();
                isFocusSet = true;
            }
            result = false;
        }
        return result;

    }

    $('#reference-contact-name').keyup(function () {
        if ($('#reference-contact-name-error').attr('display') != "none") {
            if ($('#reference-contact-name').val() != "") {
                $('#reference-contact-name-error').hide();
                RemoveBorderColor('#reference-contact-name');
            }
        }
    });
    $('#reference-client-name').keyup(function () {
        if ($('#reference-client-name-error').attr('display') != "none") {
            if ($('#reference-client-name').val() != "") {
                $('#reference-client-name-error').hide();
                RemoveBorderColor('#reference-client-name');
            }
        }
    });
    $('#reference-job-title').keyup(function () {
        if ($('#reference-job-title-error').attr('display') != "none") {
            if ($('#reference-job-title').val() != "") {
                $('#reference-job-title-error').hide();
                RemoveBorderColor('#reference-job-title');
            }
        }
    });
    $('#reference-email').keyup(function () {
        if ($('#reference-email-error').attr('display') != "none") {
            if ($('#reference-email').val() != "") {
                $('#reference-email-error').hide();
                RemoveBorderColor('#reference-email');
            }
        }
    });

    $('#reference-phone').keyup(function () {
        if ($('#reference-phone-error').attr('display') != "none") {
            if ($('#reference-phone').val() != "") {
                $('#reference-phone-error').hide();
                RemoveBorderColor('#reference-phone');
            }
        }
    });
    $('#reference-address').keyup(function () {
        if ($('#reference-address-error').attr('display') != "none") {
            if ($('#reference-address').val() != "") {
                $('#reference-address-error').hide();
                RemoveBorderColor('#reference-address');
            }
        }
    });
    $('#reference-fax').keyup(function () {
        if ($('#reference-fax-error').attr('display') != "none") {
            if ($('#reference-fax').val() != "") {
                $('#reference-fax-error').hide();
                RemoveBorderColor('#reference-fax');
            }
        }
    });
    $('#reference-client-role').keyup(function () {
        if ($('#reference-client-role-error').attr('display') != "none") {
            if ($('#reference-client-role').val() != "") {
                $('#reference-client-role-error').hide();
                RemoveBorderColor('#reference-client-role');
            }
        }
    });

    $('#reference-email').blur(function () {
        if (!ValidateContactEmail($('#reference-email').val())) {
            $('#reference-email').focus();
            SetBorderColor("#reference-email", "red");
            $('#reference-email-error').show();
            $('#reference-email-error').html(validEmail);
        }
    });
    $('#reference-phone').blur(function () {
        if (!ValidateContactPhone($('#reference-phone').val())) {
            $('#reference-phone').focus();
            SetBorderColor("#reference-phone", "red");
            $('#reference-phone-error').show();
            $('#reference-phone-error').html(validPhoneNumber);
        }
    });


    $(document).on('click', '.save-to-reference-list', function () {
        if (!EditReferenceFormValidation()) {
            return false;
        }
        $('.save-cancel-reference').hide();
        $('.add-reference-button').show();
        var ClientName = $('#reference-client-name').val();
        var ContactName = $('#reference-contact-name').val();
        var JobTitle = $('#reference-job-title').val();
        var Email = $('#reference-email').val();
        var Phone = $('#reference-phone').val();
        var Address = $('#reference-address').val();
        var Fax = $('#reference-fax').val();
        var CanWeContact = $('input[name=reference-can-we-contact]:checked').val()
        var ClientRole = $('#reference-client-role').val();
        var rowNumber = $('#reference-row-id').val();
        var ReferenceDetailsID = $('#reference-id').val();

        var model = {
            Id: ReferenceDetailsID, ClientName: ClientName, ContactName: ContactName, JobTitle: JobTitle,
            Email: Email, Phone: Phone, MailingAddress: Address, Fax: Fax, CanWeContact: CanWeContact, ClientRole: ClientRole
        };
        $.ajax({
            type: 'post',
            url: '/Supplier/AddOrUpdateReferenceDetails',
            async: true,
            data: { model: JSON.stringify(model) },
            success: function (data) {
                if (data.result) {
                 
                        showSuccessMessage(data.message);
                    
                    GetReferenceDetails();
                    ScrollToTop();
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });

    $(document).on('click', '.delete-reference-icon', function () {
        var name = $(this).attr('data-name');
        var referenceId = $(this).attr('data-reference');
        $('#delete-reference-pop-up-content').html(deleteReferenceMessage+' <b>' + name + '</b> ?')
        $('#delete-reference-map').attr('data-reference', referenceId);
        $('#delete-reference-map').attr('data-name', name);
        $('#delete-reference-pop-up').modal('show');
    });
    $(document).on('click', '#delete-reference-map', function () {
        var referenceId = $(this).attr('data-reference');
        var name = $(this).attr('data-name');
        $('#delete-reference-pop-up').modal('hide');
        $.ajax({
            type: 'post',
            url: '/Supplier/DeleteReferenceById',
            async: true,
            data: { referenceId: referenceId },
            success: function (data) {
                if (data.result) {
                    showSuccessMessage(name + data.message);
                    GetReferenceDetails();
                }
                else {
                    showErrorMessage(data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    });

  
    var referenceId = 0;
    var buyerReferencePageNo = 1;
    var clientName = "";
    $(document).on('click', '.view-assigned-buyers-reference', function () {
        $('.back-to-reference').show();
        $('.add-reference-button').hide();
        $('#buyer-name-for-reference').val('');
        referenceId = $(this).attr('data-referenceId');
        clientName = $(this).attr('data-name');
         PagerLinkClick('1', "GetAssignedBuyerListForReference", "#hdn-reference-buyer-current-page", '', 1);
    });
    $(document).on('click', '#btn-assign-reference-search', function () {
        PagerLinkClick('1', "GetAssignedBuyerListForReference", "#hdn-reference-buyer-current-page", '', 1);
    });
    $('#buyer-name-for-reference').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            PagerLinkClick('1', "GetAssignedBuyerListForReference", "#hdn-reference-buyer-current-page", '', 1);
            return false;
        }

    });
    function GetAssignedBuyerListForReference(pageNo, sortParameter, sortDirection) {
        buyerReferencePageNo = pageNo;
        $.ajax({
            type: 'post',
            url: '/Supplier/BuyerSupplierReferenceList',
            async: true,
            data: { pageNo: pageNo, sortParameter: sortParameter, sortDirection: sortDirection, buyerName: $('#buyer-name-for-reference').val(), referenceId: referenceId },
            success: function (response) {
                $('#assign-reference-list-table-rows').html('');
                var data = response.data;
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var rowClass = "odd";
                        if (i % 2 == 0) {
                            rowClass = "even";
                        }
                        var row = "<tr class=\"" + rowClass + "\"><td>" + data[i].BuyerName + "</td><td style=\"text-align:center;\"><span style='cursor:pointer;' onclick='AddOrRemoveBuyerSupplierReference(" +
                         !(data[i].IsAssigned) + "," + data[i].BuyerId + "," + data[i].ReferenceId + ")'>"
                         + (data[i].IsAssigned ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</span></td></tr>";
                        $('#assign-reference-list-table-rows').append(row);
                    }
                    
                }
                else {
                    var row = "<tr><td colspan=\"3\">"+noRecordsFound+"</td></tr>";
                    $('#assign-reference-list-table-rows').append(row);
                }
                $('#assign-reference-header').html(clientName + "- "+ assignToClientsText);
                $('#assign-reference-table').after(displayLinks($('#hdn-reference-buyer-current-page').val(), Math.ceil(response.totalRecords / 10), sortParameter, sortDirection, "GetAssignedBuyerListForReference", "#hdn-reference-buyer-current-page"));
                    $('#reference-list-table').hide();
                    $('#assign-reference-list-table').show();
                    $('#reference-create-edit-form').hide();
                    
                ScrollToTop();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function AddOrRemoveBuyerSupplierReference(IsAdd, buyerId, reference_Id) {
        var reqData = { isAdd: IsAdd, buyerId: buyerId, referenceId: reference_Id }
        $.ajax({
            type: 'post',
            data: JSON.stringify(reqData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: '/Supplier/AddOrRemoveBuyerSupplierReferenceDetails',
            success: function (data) {
                if (data.result) {
                        showSuccessMessage(data.message);
                    
                }
                else {
                    showErrorMessage(data.message);
                }
                referenceId = reference_Id;
                PagerLinkClick(buyerReferencePageNo, "GetAssignedBuyerListForReference", "#hdn-reference-buyer-current-page", '', 1);
            }
        });
    }



    function GetMarketingDetails() {
        if (currTab != "None") {
            $("." + currTab).click(function (e) {
            }).click();
        }
        currTab = "submitMarketingdetails";
       
        StartProfileLoading();
        AddRequiredIcons();
        SetSelectedTab('profileMarketingTab');
        $.ajax({
            type: 'post',
            url: '/Supplier/GetMarketingDetailsByPartyId',
            async: true,
            success: function (data) {
                if (typeof (data) != "undefined") {
                    FillSupplierMarketingDetails(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
        EndProfileLoading('marketing-details-div');
        return false;
    }

    function FillSupplierMarketingDetails(data) {
        if (data.LogoFilePath != null) {
            document.getElementById("imgLogoPreview").src = data.LogoFilePath;
            $('#imagePreview').show();
        }
        else {
            document.getElementById("imgLogoPreview").src = "";
            $('#imagePreview').hide();
        }
        $('#MarketingDetailsForm #SellerPartyId').val(data.SellerPartyId);
        $("#logo").val("");
        $('#WebsiteLink').val(data.WebsiteLink);
        $('#OrganisationFacebookaccount').val(data.OrganisationFacebookaccount);
        $('#OrganisationTwitteraccount').val(data.OrganisationTwitteraccount);
        $('#OrganisationLinkedInaccount').val(data.OrganisationLinkedInaccount);
        $('#BusinessDescription').val(data.BusinessDescription);
        if ($('#BusinessDescription').val().length != 0) {
            var charRemain = 1200 - ($('#BusinessDescription').val().length);
            $('#lblCharCountBD').html("(" + $('#BusinessDescription').val().length + charLimitText);
        }
    }

    $(document).on('click', '.submitMarketingdetails', function () {

        if (!EditMarketingFormValidation()) {
            var els = document.querySelector('.input-validation-error');
            if (els != null) {
                els.focus();
            }
            return false;
        }
      
            $.ajax({
                type: 'post',
                url: '/Supplier/SaveMarketingDetails',
                data: $('#MarketingDetailsForm').serialize(),
                async: false,
                success: function (response) {
                    if (response.success)
                        showSuccessMessage(response.message);
                    else
                        showErrorMessage(response.message);
                    $('html,body').animate({
                        scrollTop: 0
                    }, 'slow');
                    if (currTab == "submitMarketingdetails") {
                        GetProfileSummary();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                }
            });
       
        return false;
    });


    function EditMarketingFormValidation() {
        var result = true;
        $.validator.unobtrusive.parse($('#MarketingDetailsForm'))
        if (!$('#MarketingDetailsForm').valid()) {
            result = false;
        }
        return result;
    }


    function GetCapabilityDetails() {
        if (currTab != "None") {
            $("." + currTab).click(function (e) {
            }).click();
        }
        currTab = "submitCapabilitydetails";
        StartProfileLoading();

        SetSelectedTab('profileCapabilityTab');
        AddRequiredIcons();
        LoadSicCode();
        var html = $('#TurnOver').html();
        html = html.replace(/&amp;pound;/g, "&pound;");
        html = html.replace(/&amp;euro;/g, "&euro;")
        $('#TurnOver').html(html);
        $.ajax({
            type: 'post',
            url: '/Supplier/GetCapabilityDetailsByPartyId',
            async: true,
            success: function (data) {
                if (typeof (data) != "undefined") {
                    FillSupplierCapabilityDetails(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
        EndProfileLoading('capability-details-div');
        return false;
    }

    function FillSupplierCapabilityDetails(data) {

        $('#NumberOfEmployees').val(data.NumberOfEmployees);
        $('#TurnOver').val(data.TurnOver);
        $("input[name=sector][value='" + data.sector + "']").prop("checked", true);
        $('#CapabilityDetailsForm #SellerPartyId').val(data.SellerPartyId);
    //For Not Sure
    if (data.sector == 485) {
        $("input[name=sector][value='" + 485 + "']").parent().show();
        $("#txtPSupplierSectorDescription").val(data.BusinessSectorDescription);
        $("#divPSupplerSectorDescription").show();
    }
    else {
        $("input[name=sector][value='" + 485 + "']").parent().hide();
        $("#txtPSupplierSectorDescription").val("");
        $("#divPSupplerSectorDescription").hide();
    }

        for (var i = 1; i < geoGraphicSalesCount; i++) {
            $("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]").prop("checked", false);

        }
        for (var i = 1; i < geoGraphicSuppCount; i++) {
            $("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]").prop("checked", false);
        }

        for (var j = 0; j < data.CompanySalesGeoRegions.length; j++) {
            for (var i = 0; i < geoGraphicSalesCount; i++) {
                if ($("input[name=\"GeoGraphicSalesList[" + i + "].Value\"]").val() == data.CompanySalesGeoRegions[j].Value) {
                    $("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]").prop("checked", true);
                }

            }
        }
        for (var j = 0; j < data.CompanyServiceGeoRegions.length; j++) {
            for (var i = 0; i < geoGraphicSuppCount; i++) {
                if ($("input[name=\"GeoGraphicSuppList[" + i + "].Value\"]").val() == data.CompanyServiceGeoRegions[j].Value) {
                    $("input[name=\"GeoGraphicSuppList[" + i + "].Selected\"]").prop("checked", true);
                }

            }
        }
        $('#MaxContractValue').val(data.MaxContractValue);
        $('#MinContractValue').val(data.MinContractValue);
        for (var i = 0; i < data.CompanyIndustryCodes.length; i++) {
            $('#lblSIC' + (i + 1)).text(data.CompanyIndustryCodes[i].Text);
            $('#SIC' + (i + 1)).val(data.CompanyIndustryCodes[i].Value);
                $('#lnkTree' + (i + 1)).hide();
                $('#lnkSIC' + (i + 1)).show();
        }
        

    }

    $(document).on('click', '.submitCapabilitydetails', function () {
    
    var notSure = $("#rdoSupplierPNotSure").prop('checked');
    if (notSure == true) {
        var businessSector = $("#txtPSupplierSectorDescription").val();
        if (businessSector == "") {
            showErrorMessage(industrySectorError);
            return false;
        }
    }

    if (!CapabilityFormValidation()) {
            var els = document.querySelector('.input-validation-error');
            if (els != null) {
                els.focus();
            }
            return false;
        }
            $.ajax({
                type: 'post',
                url: '/Supplier/SaveCapabilityDetails',
                data: $('#CapabilityDetailsForm').serialize(),
                async: false,
                success: function (response) {
                    if (response.result)
                        showSuccessMessage(response.message);
                    else
                        showErrorMessage(response.message);
                    ScrollToTop();
                    if (currTab == "submitCapabilitydetails") {
                        GetProfileSummary();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                }
            });
     
        return false;
    });


    function CapabilityFormValidation() {
        var result = true;
        $.validator.unobtrusive.parse($('#CapabilityDetailsForm'))
        if (!$('#CapabilityDetailsForm').valid()) {
            result = false;
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
            var test = $("input[name=\"GeoGraphicSalesList[" + i + "].Selected\"]:Checked").val();
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

    $(window).scroll(function () {
        var width = $(document).width();
        if (width >= 768) {
            var height = $('#profileSideBar').height();
            var b = document.getElementById('defaultFooter');
            var rect2 = b.getBoundingClientRect();
        if (height > rect2.top) {
                var count = height - rect2.top;
                $("#profileSideBar").css({ "margin-top": -count + "px" });
                }
                else {
                $("#profileSideBar").css({ "margin-top": "-2%" });
                }
           
        }
       
    });

    $("#contact-address").bind('change keyup', function () {
        var value = $("#contact-address").val();
    if (value == "") {
            $('#contact-address-div').hide();
            return false;
        }
    if (value == "0") {
            ClearContactAddressForm();
            $('#contact-address-div').show();
            return false;
        }
        ClearContactAddressForm();
        $.ajax({
            type: 'post',
            url: '/Supplier/GetAddressDetailsByContactMethodId',
            data: { contactMethodId: parseInt(value) },
            success: function (data) {
                $('#contact-address-line-1').val(data.Line1);
                $('#contact-address-line-2').val(data.Line2);
                $('#contact-address-city').val(data.City);
                $('#contact-address-state').val(data.State);
                $('#contact-address-postal').val(data.ZipCode);
                $('#contact-address-country').val(data.RefCountryId);
                $('#contact-address-types').val(parseInt(data.AddressType));
                $('#contact-address-div').show();
                $('.readonly-fields').attr('readonly', true);
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });

    });
    function ClearContactAddressForm() {
        $('.readonly-fields').attr('readonly', false);
        $('#contact-address-types-1,#contact-address-types-2,#contact-address-types-3').hide();
        $.each(nonExistingGeneralAddressTypes, function (index, value) {
            $('#contact-address-types-' + value).show();
        });
         AppendCountriesForContactAddressDetails();
        $('#contact-address-types').show();
        $('#contact-address-line-1').val('');
        $('#contact-address-line-2').val('');
        $('#contact-address-city').val('');
        $('#contact-address-state').val('');
        $('#contact-address-postal').val('');
        $('#contact-address-country').val(0);
        $('#contact-address-types').val(0);
        RemoveBorderColor('#contact-address-line-1');
        RemoveBorderColor('#contact-address-city');
        RemoveBorderColor('#contact-address-postal');
        RemoveBorderColor('#contact-address-country');
        RemoveBorderColor('#contact-address-types');
        $('.error-text').hide();

    }
    function AppendCountriesForContactAddressDetails() {
        $.ajax({
            type: 'post',
            url: '/Supplier/GetCountries',
            async: false,
            success: function (data) {
                if (data.length > 0) {
                    $('#contact-address-country').html = "";
                    var dropDownValues = "<option value=\"0\">-- Select Country --</option>";
                    for (var i = 0; i < data.length; i++) {
                        dropDownValues += "<option value=\"" + data[i].Value + "\">" + data[i].Text + "</option>"
                    }
                    $('#contact-address-country').html(dropDownValues);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    $('#contact-address-line-1').keyup(function () {
        if ($('#contact-address-line-1-error').attr('display') != "none") {
            if ($('#contact-address-line-1').val() != "") {
                $('#contact-address-line-1-error').hide();
                RemoveBorderColor('#contact-address-line-1');
            }
        }
    });
    $('#contact-address-city').keyup(function () {
        if ($('#contact-address-city-error').attr('display') != "none") {
            if ($('#contact-address-city').val() != "") {
                $('#contact-address-city-error').hide();
                RemoveBorderColor('#contact-address-city');
            }
        }
    });
    $('#contact-address-postal').keyup(function () {
        if ($('#contact-address-postal-error').attr('display') != "none") {
            if ($('#contact-address-postal').val() != "") {
                $('#contact-address-postal-error').hide();
                RemoveBorderColor('#contact-address-postal');
            }
        }
    });
    $('#contact-address-country').change(function () {
        if ($('#contact-address-country-error').attr('display') != "none") {
            if ($('#contact-address-country').val() != "0") {
                $('#contact-address-country-error').hide();
                RemoveBorderColor('#contact-address-country');
            }
        }
    });
    $('#contact-address-types').change(function () {
        if ($('#contact-address-types-error').attr('display') != "none") {
            if ($('#contact-address-types').val() != "0") {
                $('#contact-address-types-error').hide();
                RemoveBorderColor('#contact-address-types');
            }
        }
    });
function ShowDescriptionToolTip(e, description) {
    $(".tooltip-remove").tipso('hide');
    $(e).tipso('update', 'content', description);

    $(e).tipso('update', 'background', 'rgb(226, 226, 226)');

    $(e).tipso('update', 'color', 'rgb(0, 0, 0)');

    $(e).tipso('show');
}
function ShowHidePDescription(command) {
    if (command == "show") {
        $("#divPSupplerSectorDescription").show();
    }
    else {
        $("#divPSupplerSectorDescription").hide();
    }
}
function HideToolTip(e) {
    
    $(e).tipso('hide');
    $(e).tipso('remove');
    $(e).tipso('destroy');
}

$(".tooltip-remove").hover(
  function () {
      var desc = $(this).attr("data-description");
      $(this).tipso('update', 'content', desc);
      $(this).tipso('update', 'background', '#FF9933');
      $(this).tipso('show');
  }, function () {

      $(this).tipso('hide');
      $(this).tipso('remove');
      $(this).tipso('destroy');
  }
);
$('.GeoGraphicSuppListValidate input').click(function () {
    $("#geoGraphicSuppListError").hide();
    return true;
});

$('.GeoGraphicSalesListValidate input').click(function () {
    $("#geoGraphicSaleListError").hide();
    return true;
});
$('#BusinessDescription').keyup(function () {
    if ($('#BusinessDescription').val().length <= 1200) {
        var charRemain = 1200 - ($('#BusinessDescription').val().length);
        if ($('#BusinessDescription').val().length != 0) {
            $('#lblCharCountBD').html("(" + $('#BusinessDescription').val().length  + charLimitText);
        }
        else {
            $('#lblCharCountBD').html("("+charLimitText+")");
        }
    }
    else {
        $('#lblCharCountBD').html(businessDescriptionValidation);
    }
    return true;
});