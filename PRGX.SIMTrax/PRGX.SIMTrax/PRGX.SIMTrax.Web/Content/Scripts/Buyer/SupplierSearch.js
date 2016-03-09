var pageNo = 1;
var sortParameter = "CompanyName";
var sortDirection = 1;
var TotalCountOfSuppliers = 0;

$(document).ready(function () {

    $("supplierName").intellisense({
        url: '/Buyer/GetSuppliersList',
        width: '95',
        actionOnEnter: SearchSupplierFromIntellisense,
        actionOnSelect: 'RedirectToSupplierProfileFromSearch'
    });
    search();
    if ($(window).width() < 992) {
        $("#button-toggle-filter").hide();
        $("#container-for-filter").removeClass("filter-panel-button");
    }

});

function ExpandFilterOnLoad() {
    if ($("#supplier-search-filter-panel").attr("data-toggle") == 0) {
        $("#supplier-search-filter-panel").animate({ width: "25%" }, 500);
        $("#container").animate({ width: "75%" }, 500, function () {
            $("#supplier-search-filter-panel").attr("data-toggle", 1);
            $("#container-for-filter").css("height", "");
            $("#Filter-container").show();
            $("#vertical-panel-heading").hide();
            $("#button-toggle-filter").css("left", "");
            $('#suppSearchTable').footable();
            $('#suppSearchTable').trigger('footable_redraw');
        });
    }
}


$(document).on('click', '.search-filter-header label,.search-filter-icons', function () {
    var child = $(this).parent().attr('data-child');
    var parent = $('#search-filter-child-' + child).attr('data-parent')
    if (!$('#search-filter-child-' + child).is(':visible')) {
        $('#search-filter-child-' + child).show();
        $('#search-filter-header-' + parent).find('span:first').removeClass('search-filter-close').addClass('search-filter-open');
    }
    else {
        $('#search-filter-child-' + child).hide();
        $('#search-filter-header-' + parent).find('span:first').addClass('search-filter-close').removeClass('search-filter-open');


    }
    ResetNavigationFromBuyerHomeToFalse();
});
function OpeningFilterPanel(child, parent) {
    if (!$('#search-filter-child-' + child).is(':visible')) {
        $('#search-filter-child-' + child).show();
        $('#search-filter-header-' + parent).find('span:first').removeClass('search-filter-close').addClass('search-filter-open');
    }
    else {
        $('#search-filter-child-' + child).hide();
        $('#search-filter-header-' + parent).find('span:first').addClass('search-filter-close').removeClass('search-filter-open');


    }
}
$(document).on('click', '.search-filter-child :checkbox', function () {
    var parent = $(this).parent().parent().parent().attr('data-parent');

    if ($(this).is(':checked')) {
        if (!$('#search-filter-header-' + parent).find('span:last').is(':visible')) {
            $('#search-filter-header-' + parent).find('span:last').show();
            if (!$('#search-filter-header-clear').is(':visible')) {
                $('#search-filter-header-clear').show();
            }
        }
        if (parent == "1") {
            $('#search-filter-child-1  input:checkbox').each(function () {
                var $this = $(this);
                if ($this.is(":checked") && parseInt($this.val()) == 5) {
                    $('#search-filter-header-clear').click();
                    $('.search-filter-child input').prop('disabled', true);
                    $('#search-filter-child-1 input[value="5"]').removeAttr('disabled').prop("checked", true);
                    $('#search-filter-header-clear').show();
                    $('#search-filter-header-1').find('span:last').show();


                }
            });
        }
        else {
            $('.search-filter-child input').removeAttr('disabled');

        }
    }
    else {
        $('.search-filter-child input').removeAttr('disabled');
        var child = $('#search-filter-header-' + parent).attr('data-child');
        var checkedCount = 0;
        $('#search-filter-child-' + child + '  input:checkbox').each(function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                checkedCount++;
            }
        });
        if (checkedCount == 0) {
            $('#search-filter-header-' + parent).find('span:last').hide();
            CheckMainClearAll();
        }

    }
    ResetNavigationFromBuyerHomeToFalse();
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", "CompanyName", 1);
});

function FilterPanelsEnablingClears(divId) {
    var parent = $('#' + divId).attr('data-parent');
    if (!$('#search-filter-header-' + parent).find('span:last').is(':visible')) {
        $('#search-filter-header-' + parent).find('span:last').show();
        if (!$('#search-filter-header-clear').is(':visible')) {
            $('#search-filter-header-clear').show();
        }
    }
}
function CheckMainClearAll() {
    var checkedCount = 0;
    for (var i = 1; i <= 9; i++) {
        $('#search-filter-child-' + i + '  input:checkbox').each(function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                checkedCount++;
            }
        });

    }
    if (checkedCount == 0) {
        $('#search-filter-header-clear').hide();
    }
    else {
        $('#search-filter-header-clear').show();

    }
    ResetNavigationFromBuyerHomeToFalse();
}
$(document).on('click', '.search-filter-child-clear', function () {
    $('.search-filter-child input').removeAttr('disabled');
    var child = $(this).parent().attr('data-child');
    $(this).hide();
    $('#search-filter-child-' + child + '  input:checkbox').each(function () {
        var $this = $(this);
        $this.prop("checked", false);
    });
    CheckMainClearAll();
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", "CompanyName", 1);
});
$(document).on('click', '#search-filter-header-clear', function () {
    $('.search-filter-child input').removeAttr('disabled');
    var checkedCount = 0;
    $(this).hide();
    for (var i = 1; i <= 9; i++) {
        $('#search-filter-child-' + i + '  input:checkbox').each(function () {
            var $this = $(this);
            $this.prop("checked", false);

        });
        $('#search-filter-header-' + i).find('span:last').hide();
    }
    ResetNavigationFromBuyerHomeToFalse();
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", "CompanyName", 1);

});

function ResetNavigationFromBuyerHomeToFalse() {
    Common.NavigationFromBuyerHome = false;
}

function GetFilterModel(currPg, size, sortParameter, sortDirection) {
    var SupplierStatus = [];
    $('#search-filter-child-1  input:checkbox').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            SupplierStatus.push(parseInt($(this).val()));
        }
    });
    var SupplierType = [];
    $('#search-filter-child-2  input:checkbox').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            SupplierType.push(parseInt($(this).val()));
        }
    });
    var Sector = [];
    $('#search-filter-child-3  input:checkbox').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            Sector.push(parseInt($(this).val()));
        }
    });
    var EmployeeSize = [];
    $('#search-filter-child-4  input:checkbox').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            EmployeeSize.push(parseInt($(this).val()));
        }
    });
    var TurnOver = [];
    $('#search-filter-child-5  input:checkbox').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            TurnOver.push(parseInt($(this).val()));
        }
    });
    var TypeOfCompany = [];
    $('#search-filter-child-9  input:checkbox').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            TypeOfCompany.push(parseInt($(this).val()));
        }
    });
    var ComplianceFilters = [];
    //for (var i = 6; i <= 8; i++) {
    //    var required = [];
    //    $('#search-filter-child-' + i + ' .buyer-search-required  input:checkbox').each(function () {
    //        var $this = $(this);
    //        if ($this.is(":checked")) {
    //            required.push(parseInt($(this).val()));
    //        }
    //    });
    //    var status = [];
    //    $('#search-filter-child-' + i + ' .buyer-search-status  input:checkbox').each(function () {
    //        var $this = $(this);
    //        if ($this.is(":checked")) {
    //            status.push(parseInt($(this).val()));
    //        }
    //    });
    //    var shared = [];
    //    $('#search-filter-child-' + i + ' .buyer-search-shared  input:checkbox').each(function () {
    //        var $this = $(this);
    //        if ($this.is(":checked")) {
    //            shared.push(parseInt($(this).val()));
    //        }
    //    });
    //    var pillar = 1;
    //    switch (i) {
    //        case 6: pillar = 1;
    //            break;
    //        case 7: pillar = 2;
    //            break;
    //        case 8: pillar = 4;
    //            break;
    //    }
    //    var model = { Pillar: pillar, RequiredStatus: required, SharedStatus: shared, Status: status };
    //    ComplianceFilters.push(model);
    //}
    var returnModel = { SupplierStatus: SupplierStatus, SupplierType: SupplierType, Sector: Sector, EmployeeSize: EmployeeSize, TurnOver: TurnOver, TypeOfCompany: TypeOfCompany, ComplianceFilters: ComplianceFilters, PageNo: currPg, PageSize: size, SortParameter: sortParameter, SortDirection: sortDirection, SupplierName: $('#supplierName').val(), FromBuyerHome: Common.NavigationFromBuyerHome }

    return returnModel;
}
function search() {
    ExpandFilterOnLoad();
    $("#supplierNameDivIntellisense").remove();
    $('.search-filter-child-clear').hide();
    $('#search-filter-header-clear').hide();
    SearchFilterCollpasablePanel();
    SelectMenu("searchTab");
    $('#search-request-answer-all').removeAttr('checked');
    $('.header-font').removeClass('bottomBorder');
    $('.headerSearch').addClass('bottomBorder');
    $('#search').show();
    $('#search-view-discussion').hide();
    $('#buyer-search-shared-count').val(0);
    GetFilterDropDowns();
    //if (localStorage.OpenVerifiedSuppliers != undefined && localStorage.OpenVerifiedSuppliers == "true") {
    //    OpeningFilterPanel(1, 1);
    //    $('#search-filter-child-1 input[value="3"]').prop("checked", true);
    //    $('#search-filter-child-1 input[value="2"]').prop("checked", true);
    //    FilterPanelsEnablingClears("search-filter-child-1");
    //    OpeningFilterPanel(2, 2);
    //    $('#search-filter-child-2 input[value="2"]').prop("checked", true);
    //    FilterPanelsEnablingClears("search-filter-child-2");

    //    localStorage.removeItem("OpenVerifiedSuppliers");

    //}
    //if (Common.SupplierStatusBuyerHome >= 0) {
    //    OpeningFilterPanel(1, 1);
    //    $('#search-filter-child-1 input[value=\"' + Common.SupplierStatusBuyerHome + '\"]').prop("checked", true);
    //    FilterPanelsEnablingClears("search-filter-child-1");
    //    if (Common.SupplierStatusBuyerHome == 5) {
    //        $('.search-filter-child input').prop('disabled', true);
    //        $('#search-filter-child-1 input[value="5"]').removeAttr('disabled').prop("checked", true);
    //    }
    //    Common.SupplierStatusBuyerHome = -1;
    //    Common.NavigationFromBuyerHome = true;
    //}
    //else if (Common.SupplierTypeFromBuyerHome > 0 && Common.ComplianceTypeFromBuyerHome == 0) {
    //    OpeningFilterPanel(2, 2);
    //    $('#search-filter-child-2 input[value=\"' + Common.SupplierTypeFromBuyerHome + '\"]').prop("checked", true);
    //    FilterPanelsEnablingClears("search-filter-child-2");
    //    Common.SupplierTypeFromBuyerHome = 0;
    //}
    //else if (Common.SupplierTypeFromBuyerHome > 0 && Common.ComplianceTypeFromBuyerHome > 0) {
    //    OpeningFilterPanel(2, 2);
    //    $('#search-filter-child-2 input[value=\"' + Common.SupplierTypeFromBuyerHome + '\"]').prop("checked", true);
    //    FilterPanelsEnablingClears("search-filter-child-2");
    //    if (Common.EmployeeSizeFromBuyerHome > 0) {
    //        OpeningFilterPanel(4, 4);
    //        $('#search-filter-child-4 input[value=\"' + Common.EmployeeSizeFromBuyerHome + '\"]').prop("checked", true);
    //        FilterPanelsEnablingClears("search-filter-child-4");
    //    }
    //    if (Common.TurnoverFromBuyerHome > 0) {
    //        OpeningFilterPanel(5, 5);
    //        $('#search-filter-child-5 input[value=\"' + Common.TurnoverFromBuyerHome + '\"]').prop("checked", true);
    //        FilterPanelsEnablingClears("search-filter-child-5");
    //    }
    //    if (Common.SectorFromBuyerHome > 0) {
    //        OpeningFilterPanel(3, 3);
    //        $('#search-filter-child-3 input[value=\"' + Common.SectorFromBuyerHome + '\"]').prop("checked", true);
    //        FilterPanelsEnablingClears("search-filter-child-3");
    //    }
    //    switch (Common.ComplianceTypeFromBuyerHome) {
    //        case 1:
    //            OpeningFilterPanel(6, 6);
    //            $('#search-filter-child-6 .buyer-search-required input[value=\"1\"]').prop("checked", true);
    //            if (Common.SharedTypeFromBuyerHome > 0) {
    //                $('#search-filter-child-6 .buyer-search-shared input[value=\"' + Common.SharedTypeFromBuyerHome + '\"]').prop("checked", true);
    //            }
    //            if (Common.ProductStatusFromBuyerHome >= 0) {
    //                $('#search-filter-child-6 .buyer-search-status input[value=\"' + Common.ProductStatusFromBuyerHome + '\"]').prop("checked", true);
    //            }
    //            FilterPanelsEnablingClears("search-filter-child-6");

    //            break;
    //        case 2: OpeningFilterPanel(7, 7);
    //            $('#search-filter-child-7 .buyer-search-required input[value=\"1\"]').prop("checked", true);
    //            if (Common.SharedTypeFromBuyerHome > 0) {
    //                $('#search-filter-child-7 .buyer-search-shared input[value=\"' + Common.SharedTypeFromBuyerHome + '\"]').prop("checked", true);
    //            }
    //            if (Common.ProductStatusFromBuyerHome >= 0) {
    //                $('#search-filter-child-7 .buyer-search-status input[value=\"' + Common.ProductStatusFromBuyerHome + '\"]').prop("checked", true);
    //            }
    //            FilterPanelsEnablingClears("search-filter-child-7");
    //            break;
    //        case 4: OpeningFilterPanel(8, 8);

    //            $('#search-filter-child-8 .buyer-search-required input[value=\"1\"]').prop("checked", true);
    //            if (Common.SharedTypeFromBuyerHome > 0) {
    //                $('#search-filter-child-8 .buyer-search-shared input[value=\"' + Common.SharedTypeFromBuyerHome + '\"]').prop("checked", true);
    //            }
    //            if (Common.ProductStatusFromBuyerHome >= 0) {
    //                $('#search-filter-child-8 .buyer-search-status input[value=\"' + Common.ProductStatusFromBuyerHome + '\"]').prop("checked", true);
    //            }
    //            FilterPanelsEnablingClears("search-filter-child-8");
    //            break;
    //    }
    //    Common.SupplierTypeFromBuyerHome = 0;
    //    Common.ComplianceTypeFromBuyerHome = 0;
    //    Common.SharedTypeFromBuyerHome = 0;
    //    Common.ProductStatusFromBuyerHome = -1;
    //    Common.EmployeeSizeFromBuyerHome = 0;
    //    Common.TurnoverFromBuyerHome = 0;
    //    Common.SectorFromBuyerHome = 0;
    //}
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", "CompanyName", 1);

}
function SearchSupplierFromIntellisense() {
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", "CompanyName", 1);
}
$(document).on('click', '#buyer-search-filter-reset', function () {
    $('#buyer-search-shared-count').val(0);
    $('#ddlSearchSupplierType').val(0);
    $('#ddlSearchStatus').val('');
    $('#ddlSearchBusinessSector').val(0);
    $('#ddlSearchNoOfEmployees').val(0);
    $('#ddlSearchTurnOver').val(0);
    //$('#supplierName').val("");
    $('#buyer-search-filter-reset').hide();
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", "CompanyName", 1);
});

function GetFilterDropDowns() {

    $.ajax({
        type: 'post',
        url: '/Buyer/GetSearchFilterDropDowns',
        async: false,
        success: function (data) {
            for (var i = 1; i <= 8; i++) {
                $('#search-filter-child-' + i).hide();
                $('#search-filter-header-' + i).find('span:first').addClass('search-filter-close').removeClass('search-filter-open');

            }
            if (data.CompanyStatusList.length > 0) {
                var options = "";
                for (var i = 0; i < data.CompanyStatusList.length; i++) {
                    options += "<div class=\"col-md-12 col-sm-12 col-xs-12\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.CompanyStatusList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.CompanyStatusList[i].Text + "</div></div>";
                }
                $('#search-filter-child-1').html(options);
            }
            if (data.supplierTypeList.length > 0) {
                var options = "";
                for (var i = 0; i < data.supplierTypeList.length; i++) {
                    options += "<div class=\"col-md-12 col-sm-12 col-xs-12\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.supplierTypeList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.supplierTypeList[i].Text + "</div></div>";
                }
                $('#search-filter-child-2').html(options);
            }
            if (data.BusinessSectorList.length > 0) {
                var options = "";
                for (var i = 0; i < data.BusinessSectorList.length; i++) {
                    options += "<div class=\"col-md-12 col-sm-12 col-xs-12\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.BusinessSectorList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.BusinessSectorList[i].Text + "</div></div>";
                }
                $('#search-filter-child-3').html(options);
            }
            if (data.NoOfEmployeesList.length > 0) {
                var options = "";
                for (var i = 0; i < data.NoOfEmployeesList.length; i++) {
                    options += "<div class=\"col-md-12 col-sm-12 col-xs-12\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.NoOfEmployeesList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.NoOfEmployeesList[i].Text + "</div></div>";
                }
                $('#search-filter-child-4').html(options);
            }
            if (data.TurnOverList.length > 0) {
                var options = "";
                for (var i = 0; i < data.TurnOverList.length; i++) {
                    options += "<div class=\"col-md-12 col-sm-12 col-xs-12\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.TurnOverList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.TurnOverList[i].Text + "</div></div>";
                }
                $('#search-filter-child-5').html(options);
            }
            if (data.CompanyTypeList.length > 0) {
                var options = "";
                for (var i = 0; i < data.CompanyTypeList.length; i++) {
                    options += "<div class=\"col-md-12 col-sm-12 col-xs-12\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.CompanyTypeList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.CompanyTypeList[i].Text + "</div></div>";
                }
                $('#search-filter-child-9').html(options);
            }

            //
            //var complainceChildStartsAt = 6;
            //var complainceChildEndsAt = 8;
            //for (var j = complainceChildStartsAt; j <= complainceChildEndsAt; j++) {
            //    var output = "<div class=\"col-md-12 col-sm-12 col-xs-12 \" style=\"font-weight:bold;\">" + required + "</div>";
            //    if (data.RequiredTypeList.length > 0) {
            //        for (var i = 0; i < data.RequiredTypeList.length; i++) {
            //            output += "<div class=\"col-md-12 col-sm-12 col-xs-12 buyer-search-required\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.RequiredTypeList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.RequiredTypeList[i].Text + "</div></div>";
            //        }
            //    }
            //    output += "<div class=\"col-md-12 col-sm-12 col-xs-12  padding-top-10px\" style=\"font-weight:bold;\">" + status + "</div>";
            //    if (data.ProductStatusTypeList.length > 0) {
            //        for (var i = 0; i < data.ProductStatusTypeList.length; i++) {
            //            output += "<div class=\"col-md-12 col-sm-12 col-xs-12 buyer-search-status\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.ProductStatusTypeList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.ProductStatusTypeList[i].Text + "</div></div>";
            //        }
            //    }
            //    output += "<div class=\"col-md-12 col-sm-12 col-xs-12  padding-top-10px\" style=\"font-weight:bold;\">" + shared + "</div>";
            //    if (data.SharedTypeList.length > 0) {
            //        for (var i = 0; i < data.SharedTypeList.length; i++) {
            //            output += "<div class=\"col-md-12 col-sm-12 col-xs-12 buyer-search-shared\"><div style=\"width:20px;float:left\"> <input type=\"checkbox\" value='" + data.SharedTypeList[i].Value + "'/></div><div style=\"display:table-cell\">" + data.SharedTypeList[i].Text + "</div></div>";
            //        }
            //    }
            //    $('#search-filter-child-' + j).html(output);

            //}

        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to get company status');
        }
    });
}

$('#pageSizeSupplierSearch').change(function () {
    GetSuppliers(1, "CompanyName", 1);
});

var suppliersList = null;

function GetSuppliers(currPg, sortParameter, sortDirection) {
    $('html,body').animate({
        scrollTop: 0
    }, 'fast');
    var size = parseInt($('#pageSizeSupplierSearch').val());
    var model = GetFilterModel(currPg, size, sortParameter, sortDirection);
    $('#search-bulk-button').hide();
    $('#search').show();
    $('#search-view-discussion').hide();
    pageNo = currPg;
    if ($('#search-request-answer-all').is(":checked")) {
        $('#modal-search-bulk-request-leave-page').modal('show');
        return false;
    }
    else {
        $('#modal-search-bulk-request-leave-page').modal('hide');
    }
    if ($('#supplierName').val() != "")
        $('#buyer-search-heading').html(searchResultsFor + "\"" + $('#supplierName').val() + "\"");
    else
        $('#buyer-search-heading').html(searchResults);

    $('#search-page-data').hide();

    $('#trLoading').show();
    $.ajax({
        type: 'post',
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/Buyer/GetSuppliers',
        success: function (data) {
            suppliersList = data.suppliers;
            $('#trLoading').hide();
            if (data.suppliers.length > 0) {
                $('#divNoRecordMsg').hide();
                $('#suppSearchTable').show();
                $('#suppSearchTable').find("tr:gt(0)").remove();
                var trClass = "";
                for (var i = 0; i < data.suppliers.length; i++) {
                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }

                    var profile = ($.inArray(1, data.suppliers[i].AprrovedTypes) < 0) ? notShared : shared;
                    var FIT = ($.inArray(2, data.suppliers[i].AprrovedTypes) < 0) ? notShared : shared;
                    var HS = ($.inArray(3, data.suppliers[i].AprrovedTypes) < 0) ? notShared : shared;
                    var Ds = ($.inArray(5, data.suppliers[i].AprrovedTypes) < 0) ? notShared : shared;
                    var sharedString = "None";
                    if (data.suppliers[i].SectionSharedCount == 4) {
                        sharedString = "All";
                    }
                    else if (data.suppliers[i].SectionSharedCount > 0 && data.suppliers[i].SectionSharedCount < 4) {
                        sharedString = "Some";
                    }
                    var btnCls = "btn btn-color ";
                    var btnText = requestAnswers;
                    var isRequested = false;
                    if (data.suppliers[i].IsDiscussionStarted) {
                        btnCls = "hyperlink";
                        btnText = viewDiscussion;
                        isRequested = true;
                    }
                    var disabledButton = "disabled";
                    //if (data.suppliers[i].StatusId <= 2) {
                    //    disabledButton = "disabled";
                    //}
                    var hyperlinkCls = "hyperlink show-company-details";
                    var colorstyle = "";
                    if (data.suppliers[i].StatusId < 504 || data.suppliers[i].StatusId == 5) {
                        hyperlinkCls = "";
                        colorstyle = "color:black"
                    }
                    var tableRow = "";
                    var ellipsisclass = "";
                    if (data.suppliers[i].CompanyName.length > 20) {
                        ellipsisclass = "supplier-name-ellipsis";
                    }
                    //if (!data.suppliers[i].IsNotRegisteredSupplier) {

                    //    tableRow = "<tr class=" + trClass + "><td><input type=\"checkbox\" class=\"check-color\"  value=\"" + data.suppliers[i].CompanyId + "\" id=\"search-request-answer-all-" + i + "\"" + disabledButton + "/></td><td><div class=\" float-left\" value='" + data.suppliers[i].CompanyId + "'><div class='" + ellipsisclass + " " + hyperlinkCls
                    //        + "' title=\"" + data.suppliers[i].CompanyName + "\" data-companyId=\"" + data.suppliers[i].CompanyId + "\" data-companyName=\"" + data.suppliers[i].CompanyName + "\" style=\"" + colorstyle + "\">" + data.suppliers[i].CompanyName
                    //        + "</div></div><div class=' supplierInformationToolTip float-left' value = '" + i + "'onmouseover='ShowToolTip(this)' onmouseout='HideToolTip(this)'></div></td><td >" + data.suppliers[i].Status + "</td><td style=\"text-align:center;\"><span onclick='AddOrRemoveFavouriteSupplier(" +
                    //        !(data.suppliers[i].IsFavourite) + "," + data.suppliers[i].CompanyId + ")'>" + (data.suppliers[i].IsFavourite ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</td><td style=\"text-align:center;\"><span onclick='AddOrRemoveTradingSupplier(" +
                    //        !(data.suppliers[i].IsTradingWith) + "," + data.suppliers[i].CompanyId + ")'>"
                    //        + (data.suppliers[i].IsTradingWith ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</span></td>";
                    //    var productRows = "<td>" + data.suppliers[i].RequiredFIT + "</td><td>" + data.suppliers[i].StatusFIT + "</td><td>" + data.suppliers[i].SharedFIT + "</td><td>" + data.suppliers[i].RequiredHS + "</td><td>" + data.suppliers[i].StatusHS + "</td><td>" + data.suppliers[i].SharedHS + "</td><td>" + data.suppliers[i].RequiredDS + "</td><td>" + data.suppliers[i].StatusDS + "</td><td>" + data.suppliers[i].SharedDS + "</td><td>" + data.suppliers[i].StartedDate + "</td><td>" + data.suppliers[i].ProfileCreatedDate + "</td><td>" + data.suppliers[i].FITSubmittedDate + "</td><td>" + data.suppliers[i].FITPaymentDate + "</td><td>" + data.suppliers[i].FITVerifiedDate + "</td><td>" + data.suppliers[i].HSSubmittedDate + "</td><td>" + data.suppliers[i].HSPaymentDate + "</td><td>" + data.suppliers[i].HSVerifiedDate + "</td><td>" + data.suppliers[i].DSSubmittedDate + "</td><td>" + data.suppliers[i].DSPaymentDate + "</td><td>" + data.suppliers[i].DSVerifiedDate + "</td>";
                    //    if (HasPermissionForAnswers == "True") {
                    //        tableRow += "<td style=\"text-align:left;\">" + sharedString + "</td>" + productRows + "<td style=\"text-align:center;\"><div class=\"" + btnCls + "  view-discussion\" id=\"request-discussion-" + data.suppliers[i].CompanyId + "\" data-isRequested=\"" + isRequested + "\" data-supplierName=\"" + data.suppliers[i].CompanyName + "\" data-buyerId=\"" + data.suppliers[i].BuyerId + "\" data-supplierId=\"" + data.suppliers[i].CompanyId + "\"" + disabledButton + ">" + btnText + "</div></td></tr>";
                    //        $('#buyer-search-shared-count').removeAttr('disabled');
                    //    }
                    //    else {
                    //        tableRow += "<td style=\"text-align:left;\">N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td style=\"text-align:center;\">N/A</td></tr>"
                    //        $('#buyer-search-shared-count').attr('disabled', true);

                    //    }
                    //}
                    //else {
                    //    tableRow = "<tr class=" + trClass + "><td>-</td><td><div class=\"float-left\"><div class='" + ellipsisclass + " " + hyperlinkCls + "' title=\"" + data.suppliers[i].CompanyName + "\" style=\"color:black;\" >" + data.suppliers[i].CompanyName
                    //        + "</div></div><div class='supplierInformationToolTip float-left' value = '" + i + "'onmouseover='ShowToolTip(this)' onmouseout='HideToolTip(this)'></div></td><td style=\"text-align:center;\" >" + data.suppliers[i].Status + "</td><td style=\"text-align:center;\">N/A</td><td style=\"text-align:center;\">N/A</td>" +
                    //    "<td style=\"text-align:left;\">N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td>N/A</td><td style=\"text-align:center;\">N/A</td></tr>";
                    //}
                    if (!data.suppliers[i].IsNotRegisteredSupplier) {

                        tableRow = "<tr class=" + trClass + "><td><input type=\"checkbox\" class=\"check-color\"  value=\"" + data.suppliers[i].CompanyId + "\" id=\"search-request-answer-all-" + i + "\"" + disabledButton + "/></td><td><div class=\" float-left\" value='" + data.suppliers[i].CompanyId + "'><div class='" + ellipsisclass + " " + hyperlinkCls
                            + "' title=\"" + data.suppliers[i].CompanyName + "\" data-companyId=\"" + data.suppliers[i].CompanyId + "\" data-companyName=\"" + data.suppliers[i].CompanyName + "\" style=\"" + colorstyle + "\">" + data.suppliers[i].CompanyName
                            + "</div></div><div class=' supplierInformationToolTip float-left' value = '" + i + "'onmouseover='ShowToolTip(this)' onmouseout='HideToolTip(this)'></div></td><td >" + data.suppliers[i].Status + "</td><td style=\"text-align:center;\"><span onclick='AddOrRemoveFavouriteSupplier(" +
                            !(data.suppliers[i].IsFavourite) + "," + data.suppliers[i].CompanyId + ")'>" + (data.suppliers[i].IsFavourite ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</td><td style=\"text-align:center;\"><span onclick='AddOrRemoveTradingSupplier(" +
                            !(data.suppliers[i].IsTradingWith) + "," + data.suppliers[i].CompanyId + ")'>"
                            + (data.suppliers[i].IsTradingWith ? "<i class=\"fa fa-check-square-o\"></i>" : "<i class=\"fa fa-square-o\"></i>") + "</span></td><td style=\"text-align:center;\">N/A</td><td>" + data.suppliers[i].StartedDate + "</td><td>" + data.suppliers[i].ProfileCreatedDate + "</td></tr>";
                       
                    }
                    else {
                        tableRow = "<tr class=" + trClass + "><td>-</td><td><div class=\"float-left\"><div class='" + ellipsisclass + " " + hyperlinkCls + "' title=\"" + data.suppliers[i].CompanyName + "\" style=\"color:black;\" >" + data.suppliers[i].CompanyName
                            + "</div></div><div class='supplierInformationToolTip float-left' value = '" + i + "'onmouseover='ShowToolTip(this)' onmouseout='HideToolTip(this)'></div></td><td style=\"text-align:center;\" >" + data.suppliers[i].Status + "</td><td style=\"text-align:center;\">N/A</td><td style=\"text-align:center;\">N/A</td><td>NA</td><td>-</td><td>-</td>" +
                        "</tr>";
                    }
                    $('#suppSearchTable').append(tableRow);

                }


                if ($('#supplierName').val() != '') {
                    $('#lblResultCount').html(data.total + " " + resultsFor + " : " + $('#supplierName').val() + "&nbsp;&nbsp;&nbsp;&nbsp;<a>" + clear + "</a> | <a>" + save + "</a>");
                }
                else {
                    $('#lblResultCount').html("");
                }

            }
            else {
                $('#suppSearchTable').find("tr:gt(0)").remove();
                var tableRow = "<tr><td colspan=\"3\">" + noRecordsFound + "</td></tr>";
                $('#suppSearchTable').append(tableRow);
                $('#supplierSearchPaginator').hide();
            }
            var pageSizeSupplierSearch = parseInt($('#pageSizeSupplierSearch').val());
            $('.supplierSearchPaginator').html(displayLinks($('#hdnCurrentPage').val(), Math.ceil(data.total / pageSizeSupplierSearch), sortParameter, sortDirection, "GetSuppliers", "#hdnCurrentPage"));

            if (data.total <= pageSizeSupplierSearch) {
                $('.supplierSearchPaginator').css('margin-right', '0px');
            }
            var contentHtml = "";
            var pageSizeSupplierSearch = parseInt($('#pageSizeSupplierSearch').val());
            var currentPage = parseInt($('#hdnCurrentPage').val());
            var lastPage = Math.ceil(data.total / pageSizeSupplierSearch);
            if (data.total > 0) {
                if (currentPage < lastPage) {
                    //contentHtml =  (((currentPage - 1) * pageSizeSupplierSearch) + 1) + "-" + (pageSizeSupplierSearch * currentPage) + " of " + data.total +" results";
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeSupplierSearch) + 1), (pageSizeSupplierSearch * currentPage), data.total);
                }
                else {
                    //contentHtml = (((currentPage - 1) * pageSizeSupplierSearch) + 1) + "-" + data.total + " of " + data.total + " results";
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeSupplierSearch) + 1), data.total, data.total);
                }
            }
            else {
                //contentHtml = "0-0 of  0 results";
                contentHtml = String.format(searchPageData, 0, 0, 0);
            }
            $('#search-page-data').html(contentHtml);
            $('#search-page-data').show();

            $('#suppSearchTable').footable();
            $('#suppSearchTable').trigger('footable_redraw');
            TotalCountOfSuppliers = data.total;
            //$('#suppSearchTable').floatThead({
            //    position: 'absolute'
            //});
            //$('#suppSearchTable').floatThead('reflow');

        }


    });
}


$('#btnSearch').click(function () {
    $("#supplierNameDivIntellisense").remove();
    $('#hdnCurrentPage').val(1);
    GetSuppliers(1, "CompanyName", 1);
    return false;
});

$(document).on("click", ".sort-True", function (e) {
    sortParameter = $(this).attr('data-SortParameter');
    sortDirection = parseInt($(this).attr('data-sortDirection'));
    if ($(this).find('i').hasClass('fa-sort')) {
        $(this).find('i').removeClass('fa-sort');
    }
    $('.fa-sort-asc').addClass('fa-sort').removeClass('fa-sort-asc');
    $('.fa-sort-desc').addClass('fa-sort').removeClass('fa-sort-desc');
    if (sortDirection == 1) {
        $(this).find('i').addClass('fa-sort-desc');
        $(this).attr('data-sortDirection', '2');
    }
    else {
        $(this).find('i').addClass('fa-sort-asc');
        $(this).attr('data-sortDirection', '1');
    }
    PagerLinkClick(1, "GetSuppliers", "#hdnCurrentPage", sortParameter, sortDirection);

});

$(document).on('click', '.show-company-details', function () {
    var companyId = parseInt($(this).attr('data-companyId'));
    if (companyId > 0) {
       
        var url = "/Profile/" + companyId;
        window.location.href = url;
    }
})


function AddOrRemoveTradingSupplier(IsAdd, supplierPartyId) {
    var reqData = { isAdd: IsAdd, supplierPartyId: supplierPartyId }
    var test = pageNo;
    $.ajax({
        type: 'post',
        data: JSON.stringify(reqData),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        url: '/Common/AddOrRemoveTradingSupplier',
        success: function (data) {
            if (data.result) {
                showSuccessMessage(data.message);
            }
            else {
                showErrorMessage(data.message);
            }
            PagerLinkClick(pageNo, "GetSuppliers", "#hdnCurrentPage", sortParameter, sortDirection);
        }
    });
}

function AddOrRemoveFavouriteSupplier(IsAdd, supplierPartyId) {
    var reqData = { isAdd: IsAdd, supplierPartyId: supplierPartyId }
    var test = pageNo;
    $.ajax({
        type: 'post',
        data: JSON.stringify(reqData),
        dataType: "json",
        url: '/Common/AddOrRemoveFavouriteSupplier',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result) {
                showSuccessMessage(data.message);
            }
            else {
                showErrorMessage(data.message);
            }
            PagerLinkClick(pageNo, "GetSuppliers", "#hdnCurrentPage", sortParameter, sortDirection);
        }
    });
}

//$('.searchSupplier').change(function () {
//    $('#hdnCurrentPage').val(1);
//    GetSuppliers(1, "", 1);
//})
$(".searchSupplier").bind('change keyup', function () {
    $('#hdnCurrentPage').val(1);
    var filterCount = 0;
    var noFilterCount = 6;
    if ($('#buyer-search-shared-count').val() != "0") {
        filterCount++;
        noFilterCount--;
    }
    if ($('#ddlSearchTurnOver').val() != "0") {
        filterCount++;
        noFilterCount--;
    }
    if ($('#ddlSearchNoOfEmployees').val() != "0") {
        filterCount++;
        noFilterCount--;
    }
    if ($('#ddlSearchBusinessSector').val() != "0") {
        filterCount++;
        noFilterCount--;
    }
    if ($('#ddlSearchStatus').val() != "") {
        filterCount++;
        noFilterCount--;
    }
    if ($('#ddlSearchSupplierType').val() != "0") {
        filterCount++;
        noFilterCount--;
    }
    if (filterCount > 0)
        $('#buyer-search-filter-reset').show();
    else
        $('#buyer-search-filter-reset').hide();
    GetSuppliers(1, "CompanyName", 1);
});

$(document).on('click', '.view-discussion', function () {

    var isRequested = $(this).attr('data-isRequested');
    var supplierId = parseInt($(this).attr('data-supplierId'));
    var buyerId = parseInt($(this).attr('data-buyerId'));
    var supplierName = $(this).attr('data-supplierName');
    if (isRequested == "true") {
        $('#search').hide();
        $('#search-view-discussion').show();
        $('#view-discussion-heading').html(supplierName + " discussion");
        GetDiscussionListInSearch(supplierId, buyerId);
    }
    else {
        RequestAnswerBySingleSupplier(supplierId);
    }

});

function GetDiscussionListInSearch(supplierId, buyerId) {
    $('#buyer-discussion-table').css('height', '250px');
    $('#search-discussion-show-all').show();
    $('#search-discussion-message').val('');
    var buyerClasses = " buyer-chat-color tri-right left-in";
    var supplierClasses = "supplier-chat-color tri-right  right-in";
    var chatHtml = "";
    $('#buyer-discussion-table').html("");
    $("#search-request-access-container  input:checkbox").removeAttr('checked');
    $("#search-request-access-container  input:checkbox").removeAttr('disabled');
    $("#search-request-access-container  label").removeClass("approved-color").removeClass("requested-icon");
    $('#send-search-discussion').attr('data-buyerId', buyerId);
    $('#send-search-discussion').attr('data-supplierId', supplierId);
    $('#view-search-supplier-profile').attr('data-companyId', supplierId);
    ScrollToTop();
    $.ajax({
        type: 'post',
        data: { buyerId: buyerId, supplierId: supplierId },
        url: '/Common/GetDiscussionList',
        success: function (response) {
            if (response != undefined) {
                var data = response.DiscussionList;
                if (data.Discussions.length > 0) {
                    for (var i = 0; i < data.Discussions.length; i++) {
                        var item = data.Discussions[i];
                        var cls = buyerClasses;
                        if (item.CompanyType == 1) {
                            cls = supplierClasses;
                        }
                        chatHtml += "<div class=\"talk-bubble " + cls + "\"><div class=\"talktext\"><p><b>" + item.CompanyTypeString + " on " + item.FormattedDate + "</b></p><p>" + item.Discussion + "</p></div></div>"
                    }
                    $('#buyer-discussion-table').html(chatHtml);
                }
                else {
                    chatHtml = "<div class=\"col-md-12 col-sm-12 col-xs-12 start-a-discussion \"><i>" + startADiscussion + "</i></div>";
                    $('#buyer-discussion-table').html(chatHtml);
                }
                if (data.Discussions.length <= 2) {
                    $('#search-discussion-show-all').hide();
                }
                else {
                    $('#search-discussion-show-all').show();
                }

                if (data.ApprovedComplianceTypes != null && data.ApprovedComplianceTypes.length > 0) {
                    for (var i = 0 ; i < data.ApprovedComplianceTypes.length ; i++) {
                        var item = data.ApprovedComplianceTypes[i];
                        //$("#search-access-answer-type-" + item).hide();
                        $("#search-access-answer-type-" + item).prop("checked", true);
                        $('label[for=search-access-answer-type-' + item + ']').addClass('approved-color');
                        $("#search-access-answer-type-" + item).prop("disabled", true);
                    }
                }
                // if (data.ComplianceTypes != null && data.ComplianceTypes.length > 0) {
                //     for (var i = 0 ; i < data.ComplianceTypes.length ; i++) {
                //         var item = data.ComplianceTypes[i];
                ////         $('label[for=search-access-answer-type-' + item + ']').addClass('requested-icon');
                //         $("#search-access-answer-type-" + item).prop("disabled", true);

                //     }
                // }
                //var divx = document.getElementById("buyer-discussion-table");
                //divx.scrollTop = divx.scrollHeight;
                $("#search-request-access-container  input:checkbox").prop("disabled", true);
                GetInboxCount();
            }
        }
    });
}
$(document).on('click', '#send-search-discussion', function () {
    var buyerId = parseInt($(this).attr('data-buyerId'));
    var supplierId = parseInt($(this).attr('data-supplierId'));
    var message = $('#search-discussion-message').val();
    if (message != "") {
        var messageBy = buyerId;
        var type = [];
        //$("#search-request-access-container  input:checkbox").each(function () {
        //    var $this = $(this);
        //    if ($this.is(":checked")) {
        //        type.push(parseInt($(this).val()));
        //    }
        //});
        //var types = JSON.stringify(type);
        $.ajax({
            type: 'post',
            data: { buyerId: buyerId, supplierId: supplierId, message: message, messageBy: messageBy, types: null },
            url: '/Common/AddDiscussion',
            traditional: true,
            success: function (data) {
                if (data.result) {
                    PagerLinkClick(pageNo, "GetSuppliers", "#hdnCurrentPage", sortParameter, sortDirection);
                    showSuccessMessage(data.resultMessage);

                }
                else {
                    showErrorMessage(data.resultMessage);
                }
            }
        });
    }
    else {
        showErrorMessage(pleaseEnterComments);
    }
});
$(document).on('click', '#back-to-search-results', function () {
    search();
});
$(document).on('click', '#search-discussion-show-all', function () {
    $('#buyer-discussion-table').css('height', 'auto');
    $('#search-discussion-show-all').hide();
});
$(document).on('click', '#search-request-answer-all', function () {
    if ($(this).is(":checked")) {
        //$('#supplier-search-table-body .check-color').prop("checked", true);
        $("#supplier-search-table-body  input:checkbox").each(function () {
            if (!$(this).attr("disabled")) {
                $(this).prop("checked", true);
            }
        });
        var count = 0;
        var newRequestCount = 0;
        var alreadyRequestedCount = 0;
        $("#supplier-search-table-body  input:checkbox").each(function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                count++;
            }
            if ($('#request-discussion-' + parseInt($(this).val())).attr('data-isRequested') == "false") {
                newRequestCount++;
            }
            else {
                alreadyRequestedCount++;
            }
        });
        if (alreadyRequestedCount == 0) {
            $('#search-bulk-button').show();
            $('#search-bulk-button').html(actionOn + count + selected + "<span class=\"caret\"></span>");
        }
        else {
            showErrorMessage(alreadyReceived);

        }
    }
    else {
        $('#supplier-search-table-body .check-color').removeAttr("checked");
        $('#search-bulk-button').hide();
    }
});
$(document).on('click', '#supplier-search-table-body .check-color', function () {
    var isChecked = false;
    if ($(this).is(":checked")) {
        $(this).prop("checked", true);
        isChecked = true;
    }
    else {
        $(this).removeAttr("checked");
    }
    var columnCheck = false;
    var checkCount = 0;
    var totalCount = 0;
    var unCheckCount = 0;
    var newRequestCheckCount = 0;
    var newRequestUnCheckCount = 0;
    var totalNewRequestCount = 0;
    $("#supplier-search-table-body  input:checkbox").each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($('#request-discussion-' + parseInt($(this).val())).attr('data-isRequested') == "false") {
                newRequestCheckCount++;
                totalNewRequestCount++;
                columnCheck = true;
            }
            checkCount++;
        }
        else {
            if ($('#request-discussion-' + parseInt($(this).val())).attr('data-isRequested') == "false") {
                newRequestUnCheckCount++;
                totalNewRequestCount++;
            }
            unCheckCount++;
        }
        totalCount++;
    });
    if (newRequestCheckCount > 1 && isChecked == true) {
        columnCheck = true;

    }
    else if (newRequestCheckCount == 0 && isChecked == false) {
        columnCheck = false;
    }
    $('#search-bulk-button').html(actionOn + checkCount + selected + " <span class=\"caret\"></span>");
    if (!columnCheck) {

        $('#search-bulk-button').hide();
    }
    else {

        $('#search-bulk-button').show();

    }
    if (totalCount == checkCount) {
        $('#search-request-answer-all').prop("checked", true);
    }
    else {
        $('#search-request-answer-all').removeAttr("checked");
    }
});
var searchSelectedSuppliers = [];
function RequestAnswerBySingleSupplier(companyId) {
    searchSelectedSuppliers = [];
    var newRequestCount = 1;
    searchSelectedSuppliers.push(parseInt(companyId));
    $('#request-send-button').attr('data-bulk', false);
    $.ajax({
        type: 'post',
        url: '/Common/GetRequestCount',
        success: function (count) {
            if (count != undefined) {
                if (count == buyerMaxRequestCount || (count + newRequestCount) > buyerMaxRequestCount) {
                    showErrorMessage(youAlreadyRequested + " " + buyerMaxRequestCount + " " + suppliersThisCalenderMonth);
                    return false;
                }
                else {

                    $('#request-remaining-count').html(buyerMaxRequestCount - count - newRequestCount);
                    $('#modal-search-bulk-request-message').modal('show');
                }
            }
        }
    });
}

$(document).on('click', '#go-search-bulk-requests', function () {
    //var selectedTypes = $('#complianceTypes option:selected');

    //$(selectedTypes).each(function (index, selectedTypes) {
    //    selectedList.push(parseInt($(this).val()));
    //});
    //if (selectedList.length == 0) {
    //    showErrorMessage("Please select the Compliance area to be requested");
    //    return false;
    //}
    searchSelectedSuppliers = [];
    var newRequestCount = 0;
    var alreadyRequestedCount = 0;
    $("#supplier-search-table-body  input:checkbox").each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            searchSelectedSuppliers.push(parseInt($(this).val()));
            if ($('#request-discussion-' + parseInt($(this).val())).attr('data-isRequested') == "false") {
                newRequestCount++;
            }
            else {
                alreadyRequestedCount++;
            }
        }
    });
    $('#request-send-button').attr('data-bulk', true);
    $.ajax({
        type: 'post',
        url: '/Common/GetRequestCount',
        success: function (count) {
            if (count != undefined) {
                if (count == buyerMaxRequestCount) {
                    showErrorMessage(youalreadyRequested + " " + buyerMaxRequestCount + " " + suppliersThisCalenderMonth);
                }
                else {
                    if (searchSelectedSuppliers.length == 0) {
                        showErrorMessage(selectSuppliersToBeRequested);
                        $('#search-request-answer-all').removeAttr('checked');
                        $('#search-bulk-button').hide();
                        return false;
                    }
                    else if (alreadyRequestedCount > 0) {
                        showErrorMessage(suppliersAlreadyReceivedRequest);
                        return false;
                    }
                    else if ((count + newRequestCount) > buyerMaxRequestCount) {
                        var remainingCount = 0;
                        remainingCount = (buyerMaxRequestCount - count);
                        showErrorMessage(youOnlyHave + " " + remainingCount + " " + deselectAndTryAgain);
                        return false;
                    }
                    else {
                        $('#request-remaining-count').html(buyerMaxRequestCount - count - newRequestCount);
                        $('#modal-search-bulk-request-message').modal('show');
                    }
                }
            }
        }
    });
});

function GoForRequestInSearch() {
    var message = $('#search-bulk-request-message').val();
    var isBulk = $('#request-send-button').attr('data-bulk');
    if (message == "") {
        showErrorMessage(pleaseAddAMessage);
        return false;
    }
    $('#modal-search-bulk-request-message').modal('hide');
    $.ajax({
        type: 'post',
        data: { selectedSuppliers: searchSelectedSuppliers, message: message },
        url: '/Buyer/AddBulkRequest',
        traditional: true,
        success: function (data) {
            $('#search-bulk-request-message').val('');
            if (data) {
                searchSelectedSuppliers = [];
                if (isBulk == "true") {
                    showSuccessMessage(suppliersRequestedSuccessfully);
                    $('#search-request-answer-all').removeAttr('checked');
                    $('#search-bulk-button').hide();
                }
                else {
                    showSuccessMessage(messageSent);
                }
                $('#search-bulk-request-message').val('');
                PagerLinkClick(pageNo, "GetSuppliers", "#hdnCurrentPage", sortParameter, sortDirection);
            }
            else {

                showErrorMessage(somethingWentWrong);
            }
        }
    });

}
function DoPaginationForSearch() {
    $('#search-request-answer-all').removeAttr('checked');
    $('#search-bulk-button').hide();
    PagerLinkClick($('#hdnCurrentPage').val(), "GetSuppliers", "#hdnCurrentPage", sortParameter, sortDirection);
}
$(document).on('click', '#view-search-supplier-profile', function () {
    companyIdForProfileDetails = parseInt($(this).attr('data-companyId'));
    var url = "/Profile/" + companyIdForProfileDetails;
    Navigate("companyProfile", url);
});

function SearchFilterCollpasablePanel() {
    var width = $(document).width();
    if (width > 768) {
        $('#supplier-search-filter-panel').removeClass('panel-default');
        $('#supplier-search-filter-icon').parent().removeClass('clickable');
        $('#supplier-search-filter-icon').hide();
        $('#supplier-search-filter-icon').parents('.panel').find('.panel-body').slideDown();
        $('#supplier-search-filter-icon').removeClass('panel-collapsed');
        $('#supplier-search-filter-icon').find('i').removeClass('glyphicon-chevron-right').addClass('glyphicon-chevron-down');

    }
    else {
        $('#supplier-search-filter-panel').addClass('panel-default');
        $('#supplier-search-filter-icon').parent().addClass('clickable');
        $('#supplier-search-filter-icon').show();
        $('#supplier-search-filter-icon').parents('.panel').find('.panel-body').slideUp();
        $('#supplier-search-filter-icon').addClass('panel-collapsed');
        $('#supplier-search-filter-icon').find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-right');
    }
}
$(window).resize(function () {
    SearchFilterCollpasablePanel();
});

function RedirectToSupplierProfileFromSearch(key) {
    var companyName = $('#supplierNameUlIntellisense #supplierName-' + key).html();
    if (companyName != "") {
        $.ajax({
            type: 'post',
            data: { companyName: companyName },
            dataType: "json",
            url: '/Common/GetCompanyPartyIdBasedOnCompanyName',
            success: function (data) {
                if (data.CompanyId > 0) {
                    var url = "/Profile/" + data.CompanyId;
                    window.location.href = url;
                }
            }
        });

    }
    return false;
}

$(window).scroll(function () {
    FixedTableHeaderWithPagination('suppSearchTable');
});
$(window).resize(function () {
    FixedTableHeaderWithPagination('suppSearchTable');

});

$(document).on('click', '#export-supplier-search-result', function () {
    if (TotalCountOfSuppliers < 10000) {
        var SupplierType = [];
        $('#search-filter-child-2  input:checkbox').each(function () {
            var $this = $(this);
            if ($this.is(":checked")) {
                SupplierType.push(parseInt($(this).val()));
            }
        });
        if (SupplierType.length > 0) {
            var model = GetFilterModel(1, 0, "CompanyName", 1);
            window.location.href = "/Buyer/ExportSuppliersForBuyer/?val=" + JSON.stringify(model);

            //$.ajax({
            //    type: 'post',
            //    data: JSON.stringify(model),
            //    dataType: "json",
            //    contentType: "application/json; charset=utf-8",
            //    url: '/Buyer/ExportSuppliersForBuyer',
            //    success: function (data) {
            //    }
            //});

        }
        else {
            showErrorMessage(exportOnlyAvailable);
            return false;
        }

    }
    else {
        showErrorMessage(exportOnlyAvailable);

    }
    return false;

});

function ShowToolTip(e) {
    var value = $(e).attr("value");
    var content = "<div class='row padding-top-0px'><div class='col-xs-11 padding-bottom-10px padding-right-zero padding-top-0px'><b>" + suppliersList[value].CompanyName + "</b></div></div><div class='row'><div class='col-xs-6 padding-right-zero text-align-left'><div class='col-xs-12 padding-right-zero'><b>" + address + "</b></div><div class='col-xs-12 padding-right-zero'>" + suppliersList[value].AddressLine1 + "</div><div class='col-xs-12 padding-right-zero'>" + suppliersList[value].AddressLine2 + "</div><div class='col-xs-12 padding-right-zero'>" + suppliersList[value].City + ((suppliersList[value].State != null && suppliersList[value].State != "") ? ("</div><div class='col-xs-12 padding-right-zero'>" + suppliersList[value].State) : "") + "</div><div class='col-xs-12 padding-right-zero'>" + suppliersList[value].PostCode + "</div><div class='col-xs-12 padding-right-zero'>" + suppliersList[value].Country + "</div></div><div class='col-xs-5 padding-right-zero'></div><div class='col-xs-6 padding-right-zero text-align-left'><div class='col-xs-12 padding-right-zero'><b>" + regNumber + "</b></div><div class='col-xs-12 padding-right-zero'>" + ((suppliersList[value].Registrationnumber != "" && suppliersList[value].Registrationnumber != null) ? (suppliersList[value].Registrationnumber) : "-") + "</div><div class='col-xs-12 padding-right-zero'><b>" + VATNumber + "</b></div><div class='col-xs-12 padding-right-zero'>" + ((suppliersList[value].VATnumber != "" && suppliersList[value].VATnumber != null) ? (suppliersList[value].VATnumber) : "-") + "</div><div class='col-xs-12 padding-right-zero'><b>DUNS number</b></div><div class='col-xs-12 padding-right-zero'>" + ((suppliersList[value].DUNSnumber != "" && suppliersList[value].DUNSnumber != null) ? (suppliersList[value].DUNSnumber) : "-") + "</div></div></div>";
    $(e).tipso({
        position: 'top',
        showArrow: false,
        useTitle: false
    });
    $(e).tipso('update', 'content', content);
    $(e).tipso('update', 'background', 'rgb(250, 166, 52)');
    $(e).tipso('update', 'width', '400px');
    $(e).tipso('update', 'color', 'rgb(255, 255, 255)');

    $(e).tipso('show');
}

function HideToolTip(e) {
    $(e).tipso('hide');
    $(e).tipso('remove');
    $(e).tipso('destroy');
}

$("#button-toggle-filter").click(function () {
    var divHeight = $('#supplier-search-filter-panel').height();
    var marginHeight = (divHeight - 50) / 2;
    if ($("#supplier-search-filter-panel").attr("data-toggle") == 1) {

        $("#Filter-container").hide();
        $("#button-toggle-filter").hide();
        $("#container").animate({ width: "94%" }, 200);
        $("#supplier-search-filter-panel").animate({ width: "70px" }, 200, function () { $("#button-toggle-filter").show(); });
        $("#supplier-search-filter-panel").attr("data-toggle", 0);
        $("#container-for-filter").css("height", divHeight + "px");
        $("#container-for-filter .vertical-text").css("margin-top", (marginHeight - 5) + "px");
        $('#button-toggle-filter').css("top", marginHeight + "px");
        $("#vertical-panel-heading").show();
        $('#suppSearchTable').footable();
        $('#suppSearchTable').trigger('footable_redraw');
        $("#button-toggle-filter").addClass("display-inline-block");
    }
    else if ($("#supplier-search-filter-panel").attr("data-toggle") == 0) {
        $("#button-toggle-filter").hide();
        $("#supplier-search-filter-panel").animate({ width: "25%" }, 200);
        $("#container").animate({ width: "75%" }, 200, function () {
            $("#supplier-search-filter-panel").attr("data-toggle", 1);
            $("#container-for-filter").css("height", "");
            $("#vertical-panel-heading").hide();
            $("#Filter-container").show();
            $("#button-toggle-filter").show();
            $('#suppSearchTable').footable();
            $('#suppSearchTable').trigger('footable_redraw');
            $('#button-toggle-filter').css("top", "");
            $("#button-toggle-filter").addClass("display-inline-block");
        });
    }
});

$(window).resize(function () {
    WindowSizeCheck();
});

function WindowSizeCheck() {
    var toggle = $("#supplier-search-filter-panel").attr("data-toggle");
    if ($(window).width() < 975) {
        $("#supplier-search-filter-panel").removeAttr("style");
        $("#container").removeAttr("style");
        $("#supplier-search-filter-panel").attr("data-toggle", 1);
        $("#container-for-filter").removeClass("filter-panel-button");
        $('#suppSearchTable').footable();
        $('#suppSearchTable').trigger('footable_redraw');
        $("#container-for-filter").css("height", "");
        $("#Filter-container").show();
        $("#vertical-panel-heading").hide();
        $("#button-toggle-filter").removeClass("display-inline-block");
        $("#button-toggle-filter").hide();
    }
    else if ($(window).width() > 975) {
        $("#container-for-filter").addClass("filter-panel-button");
        $("#button-toggle-filter").addClass("display-inline-block");
        $("#button-toggle-filter").show();

        $("#container-for-filter").addClass("filter-panel-button");
        if ($(window).width() < 1250 && $(window).width() > 992 && toggle == 0) {
            $("#container").css("width", "92%");
        }
        else if ($(window).width() > 1250 && toggle == 0) {
            $("#container").animate({ width: "94%" }, 500);
        }
    }
}