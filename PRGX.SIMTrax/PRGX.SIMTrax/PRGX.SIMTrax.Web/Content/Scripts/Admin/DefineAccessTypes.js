$(document).ready(function () {
    CheckPermission("Users", function () {
        SelectMenu("AdminTeamTab");
        buyerAndAuditorAccessRights();
    });
});

function buyerAndAuditorAccessRights() {
    GetAllAccessDetails(1, '', 1);
}

$('#pageSizeAccessRights').change(function () {
    $("#hdn-access-rights-current-page").val(1);
    PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
});

function GetAllAccessDetails(index, sortParameter, sortDirection) {
    $('.error-text').hide();
    RemoveBorderColor('#buyer-role-name');
    RemoveBorderColor('#buyer-role-description');
    $('#buyer-access-bread-crumb,#create-buyer-role-bread-crumb,#edit-buyer-role-bread-crumb,#create-auditor-role-bread-crumb,#edit-auditor-role-bread-crumb').hide();
    $('#access-header,#create-buyer-role-header,#create-auditor-roles-header,#accessDropDown').hide();
    $('#access-table,#create-buyer-access-container').hide();
    $('#create-buyer-role-footer,#create-auditor-roles-footer').hide();
    $('#divCreateContainer').hide();
    $('#buyer-access-bread-crumb').show();
    $('#access-header,#accessDropDown,#AccessContainer,#AccessFilters').show();
    $('#access-table').show();
    var pageSizeAccessRights = $('#pageSizeAccessRights').val();
    $.ajax({
        type: 'post',
        url: '/Admin/GetAllRoleDetails',
        data: { type: $('#ddlAccessType').val(), pageSize: pageSizeAccessRights, index: index },
        traditional: true,
        success: function (data) {
            var html = "";
            var total = 0;
            if (data.roles != undefined && data.roles.length > 0) {
                for (var i = 0; i < data.roles.length; i++) {
                    var cls = "odd";
                    if (i % 2 == 0) {
                        cls = "even";
                    }
                    if (data.roles[i].ExistingRoleName == "Buyer Access") {
                        html += "<tr class=\"" + cls + "\"><td>" + data.roles[i].ExistingRoleName + "</td><td>" + data.roles[i].PotentialRoleName + "</td><td>" + data.roles[i].PotentialRoleDescription + "</td><td style=\"text-align:center\"><div class=\"btn-group buyerRoleEdit\" id=\"accessTypeDropDown\"> <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionsButton + "<span class=\"caret\"></span></button><ul class=\"dropdown-menu\" ><li><a class=\"edit-buyer-role\" data-roleId=\"" + data.roles[i].PotentialRoleId + "\">" + editBuyerRoleButton + "</a></li><li><a class=\"delete-buyer-access-role\" onclick='DeleteBuyerRoleConfirmation(\"" + data.roles[i].PotentialRoleId + "\")' data-roleId=\"" + data.roles[i].PotentialRoleId + "\">" + deleteBuyerRoleButton + "</a></li></ul></div></td></tr>";
                    }
                    else if (data.roles[i].ExistingRoleName == "Auditor Role") {
                        html += "<tr class=\"" + cls + "\"><td>" + data.roles[i].ExistingRoleName + "</td><td>" + data.roles[i].PotentialRoleName + "</td><td>" + data.roles[i].PotentialRoleDescription + "</td><td style=\"text-align:center\"><div class=\"btn-group auditorRoleEdit\" id=\"accessTypeDropDown\"> <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionsButton + "<span class=\"caret\"></span></button><ul class=\"dropdown-menu\" ><li><a  class=\"edit-auditor-access-role\" data-roleId=\"" + data.roles[i].PotentialRoleId + "\">" + editAuditorRoleButton + "</a></li><li><a onclick='DeleteAuditorRoleConfirmation(\"" + data.roles[i].PotentialRoleId + "\")'>" + deleteAuditorRoleButton + "</a></li></ul></div></td></tr>";
                    }
                }
            }
            total = data.total;
            $('#access-table-body').html(html);
            if (total == 0) {
                html = "<tr><td colspan=\"3\">" + noRecordsFound + "</td></tr>";
                $('#access-table-body').html(html);
            }
            $('.accessRightsPaginator').html(displayLinks($('#hdn-access-rights-current-page').val(), Math.ceil(total / pageSizeAccessRights), "", sortDirection, "GetAllAccessDetails", "#hdn-access-rights-current-page"));
            var contentHtml = "";
            var currentPage = parseInt($('#hdn-access-rights-current-page').val());
            var lastPage = Math.ceil(total / pageSizeAccessRights);
            if (total > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeAccessRights) + 1), (pageSizeAccessRights * currentPage), total);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeAccessRights) + 1), total, total);
                }
            }
            $('#search-page-data-accessRights').html(contentHtml);
            if (!CanEditBuyerRole) {
                $('.buyerRoleEdit').hide();
            }
            if (!CanEditAuditorRole) {
                $('.auditorRoleEdit').hide();
            }
            ScrollToTop();
            $('#accessTable').footable();
            $('#accessTable').trigger('footable_redraw');
        }
    });
}

function DeleteAuditorRoleConfirmation(roleId) {
    $('#auditorRoleDeleteConfirmation').modal('show');
    $("#hdnAuditorRoleId").val(roleId);
}

function DeleteBuyerRoleConfirmation(roleId) {
    $('#buyerRoleDeleteConfirmation').modal('show');
    $("#hdnBuyerRoleId").val(roleId);
}

function DeleteAuditorRole() {
    var roleId = $("#hdnAuditorRoleId").val();
    if (roleId != '') {
        $.ajax({
            type: "POST",
            data: { roleId: roleId },
            url: "/Admin/DeleteRole",
            success: function (response) {
                if (response && typeof (response) != "undefined") {
                    if (response == Common.LogoutAction) {
                        Logout();
                    }
                    else {
                        $('#auditorRoleDeleteConfirmation').modal('hide');
                        (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.message);
                        PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
                    }
                }
            },
            error: function (response) { }
        });
    }
}

function DeleteBuyerRole() {
    var roleId = $("#hdnBuyerRoleId").val();
    if (roleId != '') {
        $.ajax({
            type: "POST",
            data: { roleId: roleId },
            url: "/Admin/DeleteRole",
            success: function (response) {
                if (response && typeof (response) != "undefined") {
                    if (response == Common.LogoutAction) {
                        Logout();
                    }
                    else {
                        $('#buyerRoleDeleteConfirmation').modal('hide');
                        (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.messge);
                        PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
                    }
                }
            },
            error: function (response) { }
        });
    }
}

function BackToAccessList() {
    PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
    $('html, body').animate({
        scrollTop: 0
    }, 'slow');
}

$(document).on('click', '.btn-create-buyer-role', function () {
    $('.error-text').hide();
    RemoveBorderColor('#buyer-role-name');
    RemoveBorderColor('#buyer-role-description');
    $('#buyer-access-bread-crumb,#edit-buyer-role-bread-crumb,#AccessFilters,#AccessContainer').hide();
    $('#access-header').hide();
    $('#accessDropDown').hide();
    $('#access-table').hide();
    $('#create-buyer-role-header-text').html(createBuyerRoleButton);
    $('#create-buyer-role-bread-crumb').show();
    $('#create-buyer-role-header').show();
    $('#create-buyer-access-container').show();
    $('#create-buyer-role-footer').show();
    ClearBuyerRoleForm();
});

function SaveBuyerAccessRole() {
    if (!ValidateBuyerRole()) {
        return false;
    }
    var type = [];
    $("#buyer-access-list-container input:checkbox").each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            type.push(parseInt($(this).val()));
        }
    });
    var roleId = parseInt($('#buyer-role-id').val());
    var roleName = $('#buyer-role-name').val();
    var roleDescription = $('#buyer-role-description').val();
    $.ajax({
        type: 'post',
        data: { roleId: roleId, roleName: roleName, roleDescription: roleDescription, permissions: type },
        url: '/Admin/AddorUpdateBuyerRole',
        traditional: true,
        success: function (data) {
            if (data) {
                ClearBuyerRoleForm();
                PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
                $('html, body').animate({
                    scrollTop: 0
                }, 'slow');
                if (roleId == 0) {
                    showSuccessMessage(addBuyerRoleSuccessMessage);
                }
                else {
                    showSuccessMessage(updateBuyerRoleSuccessMessage);
                }
            }
            else {
                showErrorMessage(errorMessage);
            }
        }
    });
}

function ClearBuyerRoleForm() {
    $('#existing-buyer-role-id').val(4);
    $('#buyer-role-id').val(0);
    $('#buyer-role-name').val("");
    $('#buyer-role-description').val("");
    $("#buyer-access-list-container input:checkbox").removeAttr('checked');
}

function ValidateBuyerRole() {
    var result = true;
    $('.error-text').hide();
    RemoveBorderColor('#buyer-role-name');
    RemoveBorderColor('#buyer-role-description');
    var roleName = $('#buyer-role-name').val();
    if (roleName == "") {
        $('#buyer-role-name-error').show();
        SetBorderColor("#buyer-role-name", "red");
        result = false;
    }
    var roleDescription = $('#buyer-role-description').val();
    if (roleDescription == "") {
        $('#buyer-role-description-error').show();
        SetBorderColor("#buyer-role-description", "red");
        result = false;
    }
    var type = [];
    $("#buyer-access-list-container input:checkbox").each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            type.push(parseInt($(this).val()));
        }
    });
    if (type.length == 0) {
        $('#buyer-access-list-error').show();
        result = false;
    }
    return result;
}

$('#buyer-role-name').keyup(function () {
    if ($('#buyer-role-name-error').attr('display') != "none") {
        if ($('#buyer-role-name').val() != "") {
            $('#buyer-role-name-error').hide();
            RemoveBorderColor('#buyer-role-name');
        }
    }
});

$('#buyer-role-description').keyup(function () {
    if ($('#buyer-role-description-error').attr('display') != "none") {
        if ($('#buyer-role-description').val() != "") {
            $('#buyer-role-description-error').hide();
            RemoveBorderColor('#buyer-role-description');
        }
    }
});

$('#buyer-access-list-container input').click(function () {
    $("#buyer-access-list-error").hide();
    return true;
});

$(document).on('click', '.edit-buyer-role', function () {
    var roleId = parseInt($(this).attr('data-roleId'));
    ClearBuyerRoleForm();
    if (roleId > 0) {
        $.ajax({
            type: 'post',
            data: { roleId: roleId },
            url: '/Admin/GetBuyerRoleBasedOnRoleId',
            traditional: true,
            success: function (data) {
                if (data != undefined) {
                    $('#buyer-access-bread-crumb,#edit-buyer-role-bread-crumb,#AccessFilters,#AccessContainer').hide();
                    $('#access-header').hide();
                    $('#accessDropDown').hide();
                    $('#access-table').hide();
                    $('#edit-buyer-role-bread-crumb').show();
                    $('#create-buyer-role-header').show();
                    $('#create-buyer-access-container').show();
                    $('#create-buyer-role-footer').show();
                    $('#create-buyer-role-header-text').html(editBuyerRoleButton);
                    if (data.Id > 0) {
                        $('#buyer-role-id').val(data.Id);
                        $('#buyer-role-name').val(data.Name);
                        $('#buyer-role-description').val(data.Description);
                        if (data.RolePermissions != null && data.RolePermissions.length > 0) {
                            for (var i = 0 ; i < data.RolePermissions.length ; i++) {
                                var item = data.RolePermissions[i].PermissionId;
                                $("#buyer-access-" + item).prop("checked", true);
                            }
                        }
                    }
                    $('html, body').animate({
                        scrollTop: 0
                    }, 'slow');
                }
                else {
                    showErrorMessage(errorMessage);
                }
            }
        });
    }
});

$(document).on('click', '.edit-auditor-access-role', function () {
    var roleId = parseInt($(this).attr('data-roleId'));
    $.ajax({
        type: "POST",
        data: { roleId: roleId },
        url: "/Admin/GetAuditorRoleBasedOnRoleId",
        success: function (response) {
            if (response && typeof (response) != "undefined") {
                if (response == Common.LogoutAction) {
                    Logout();
                }
                else {
                    $('#buyer-access-bread-crumb,#edit-buyer-role-bread-crumb,#AccessFilters,#AccessContainer').hide();
                    $('#access-header').hide();
                    $('#accessDropDown').hide();
                    $('#access-table').hide();
                    $("#btnCreate,#divRoleTable").hide();
                    $("#divCreateContainer").show();
                    $("#manageRoleBreadCrumb").hide();
                    $("#editRoleBreadCrumb").show()
                    $("#createRoleBreadCrumb").hide();
                    $("#divCreateRoles").html(response);
                    $("#edit-auditor-role-bread-crumb").show();
                    $("#create-auditor-roles-header").show();
                    $('#create-auditor-roles-footer').show();
                    $("#create-auditor-roles-header-text").html(editAuditorRoleButton);
                    $('html, body').animate({
                        scrollTop: 0
                    }, 'slow');
                }
            }
        },
        error: function (response) { }
    });
});

$(document).on('click', '.btn-create-auditor-role', function () {
    var existingRoleId = parseInt($(this).attr('data-existing-role-id'));
    var roleId = 0;
    if (existingRoleId != "" || existingRoleId != "undefined")
    $.ajax({
        type: "POST",
        url: "/Admin/GetCreateRoleForm",
        data: { roleId: roleId, existingRoleId: existingRoleId },
        success: function (response) {
            if (response && typeof (response) != "undefined") {
                if (response == Common.LogoutAction) {
                    Logout();
                }
                else {
                    $('#buyer-access-bread-crumb,#edit-buyer-role-bread-crumb,#AccessFilters,#AccessContainer,#editRoleBreadCrumb,#manageRoleBreadCrumb,#access-header,#accessDropDown,#access-table').hide();
                    $('#create-auditor-role-bread-crumb,#divCreateContainer,#create-auditor-roles-header,#create-auditor-roles-footer').show();
                    $("#divCreateRoles").html(response);
                    $("#create-auditor-roles-header-text").html(createAuditorRoleButton);
                    $('html, body').animate({
                        scrollTop: 0
                    }, 'slow');
                }
            }
        },
        error: function (response) {
        }
    });
});

function SaveAuditorRole() {
    $.validator.unobtrusive.parse($('#frmRole'))
    if (!$('#frmRole').valid()) {
        showErrorMessage(requiredFieldsMessage);
        ScrollToTop();
    }
    else {
        var data = $("#frmRole").serialize();
        $.ajax({
            type: "POST",
            url: "/Admin/AddOrUpdateAuditorRole",
            data: data,
            success: function (response) {
                if (response && typeof (response) != "undefined") {
                    if (response == Common.LogoutAction) {
                        Logout();
                    }
                    else {
                        if (response.success == true || response.success == false) {
                            (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.messge);
                        }
                        PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
                        $('html, body').animate({
                            scrollTop: 0
                        }, 'slow');
                    }
                }
                else
                    showErrorMessage(errorMessage);
            },
            error: function (response) {
            }
        });
    }
}

$("#ddlAccessType").change(function () {
    $('#search-access-types-filter-header-clear').show();
    $('#hdn-access-rights-current-page').val(1);
    PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);

});

$(document).on('click', '#search-access-types-filter-header-clear', function () {
    $('#ddlAccessType').get(0).selectedIndex = 0;
    $('#search-access-types-filter-header-clear').hide();
    PagerLinkClick(1, "GetAllAccessDetails", "#hdn-access-rights-current-page", "", 1);
});


$(window).scroll(function () {
    FixedTableHeaderWithPagination('accessTable');
});

$(window).resize(function () {
    FixedTableHeaderWithPagination('accessTable');
});

$(window).scroll(function () {
    FixedTableHeader('accessTable');
});

$(window).resize(function () {
    FixedTableHeader('accessTable');
});
