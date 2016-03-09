$("document").ready(function () {
    verifyCampaign();
});

function verifyCampaign() {
    BindPreRegCampaignDetails(1, "", "");
}

$('#ddlDisplayCriteria').change(function () {
    BindPreRegCampaignDetails(1, "", "");
});

function BindPreRegCampaignDetails(index, sortParameter, sortDirection) {
    //$('#divNoPreRegSupplier').hide();
    //$('#tblVendorMasterBody').show();
    var campaignId = GetCampaignIdFromUrl();
    if (campaignId != 0) {
        var obj = { id: campaignId, filterCriteria: $("#ddlDisplayCriteria").val(), index: index };
        $.ajax({
            type: "post",
            contentType: "application/json; charset=utf-8",
            url: "/Campaign/GetPreRegSupplier",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                if (data.data.length == 0) {
                    //$('#divNoPreRegSupplier').show();
                    $('#tblVendorMasterBody').html("<tr><td colspan='4'>" + noRecordsFound + "</td></tr>");
                    $('#divOptions').show();
                    $('#preRegSupplierPaginator').hide();
                }
                else {
                    $('#preRegSupplierPaginator').show();
                    $('#divOptions').show();
                    $('#tblVendorMasterWorksheet').find("tr:gt(0)").remove();
                    for (var i = 0; i < data.data.length; i++) {
                        var trClass = "";
                        if (i % 2 != 0) {
                            trClass = "odd";
                        }
                        else {
                            trClass = "even";
                        }
                        var tableRow = "<tr id='tr" + i + "' class=" + trClass
                            + "><td style='display:none'><label class='recordID'>" + data.data[i].PreRegSupplierId +
                            "</label></td><td><label>" + data.data[i].SupplierName +
                            "</label></td><td><label>" + data.data[i].LoginId +
                            "</label></td><td><label class='comments'>" + data.data[i].InvalidSupplierComments +
                            "</label></td><td style=\"text-align: center;\"><button class='btn btn-color' onclick='DeletePreRegSupplier(" + data.data[i].PreRegSupplierId + ")'>" + delet + "</button></td></tr>";
                        $("#tblVendorMasterBody").append(tableRow);
                    }
                    $('#preRegSupplierPaginator').html(displayLinks($('#hdnCurrentPage').val(), Math.ceil(data.total / 10), "", "", "BindPreRegCampaignDetails", "#hdnCurrentPage"));
                }
            },
            error: function (result) {
                //alert("Error");
            }
        });
    }
}

function GetCampaignIdFromUrl() {
    var pageURL = window.location.href;
    var indexOfLastSlash = pageURL.lastIndexOf("/");

    if (indexOfLastSlash > 0 && pageURL.length - 1 != indexOfLastSlash)
        return pageURL.substring(indexOfLastSlash + 1);
    else
        return 0;
}

function DeletePreRegSupplier(preRegSupplierId) {
    var obj = { preRegSupplierId: preRegSupplierId };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Campaign/DeletePreRegSupplier",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (data) {
            if (data.result) {
                showSuccessMessage(preRegSupplierDeletedSuccessfully);
                updatePage($('#hdnCurrentPage').val(), "BindPreRegCampaignDetails", '#hdnCurrentPage', "", "");
            }
            else {
                showErrorMessage(unableToDeletePreRegSupplier);
            }
        },
        error: function (result) {
            //alert("Error");
        }
    });
}

var validFilesTypes = ["xlsx", "xls"];
$('#btnUpload').click(function () {
    var files = $("#PreRegFile").get(0).files;
    if (files.length > 0) {
        var file = document.getElementById("PreRegFile");
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
            var fileMessage = String.format(fileExtensionValidation, ext);
            showWarningMessage(fileMessage);
            return false;
        }
        else {
            return true;
        }
    }
    else {
        showErrorMessage(fileToUploadError);
        return false;
    }
});

$('#btnShowMessage').click(function () {
    var campaignId = $(this).attr('data-id');
    $('#btnSubmitCampaign').attr('data-id', campaignId);
    $('#dialogMessage').modal('show');
});

//$('#btnExport').click(function () {
//    var campaignId = $(this).attr('data-id');
//    window.location.href = '/Campaign/Export/?id=' + campaignId;
//});

$('#btnEditSuppliers').click(function () {
    var campaignId = $('#CampaignId').val();
    window.location.href = '/Campaign/DownloadEditSuppliers/?id=' + campaignId;
});

$('#btnSubmitCampaign').click(function () {
    if ($('#CampaignId').val() != null && $('#CampaignId').val() != undefined) {
        var obj = { campaignId: $('#CampaignId').val() };
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Campaign/SubmitCampaign",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    $('#dialogMessage').modal('hide');
                    showSuccessMessage(campaignSubmittedSuccessfully);
                    location.href = "/Admin/Home";
                }
                else {
                    showErrorMessage(unableToSubmitCampaign);
                }
            },
            error: function (result) {
                //alert("Error");
            }
        });
    }
    else {
        showErrorMessage(defaultErrorMessage);
    }
});