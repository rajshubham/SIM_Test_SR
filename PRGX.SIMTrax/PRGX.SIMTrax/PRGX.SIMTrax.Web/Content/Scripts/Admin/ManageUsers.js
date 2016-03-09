var GV = (function () {
    return {
        emailIdAvailable: true
    };
}());

$(document).ready(function () {
    $("txtUserVSS").intellisense({
        url: '/Admin/GetUserName',
        actionOnEnter: SearchUserOnEnter
    });
});

$(document).ready(function () {
    $("txtUserEmailVSS").intellisense({
        url: '/Admin/GetUserEmail',
        actionOnEnter: SearchUserOnEnter
    });
});

$(document).ready(function () {
    CheckPermission("Users", function () {
        SelectMenu("AdminTeamTab");
        manageUsers();
        $('input[placeholder]').simplePlaceholder();
    });
});

function manageUsers() {
    if (localStorage.UserNameFromBuyerHomeSearch != undefined && localStorage.UserNameFromBuyerHomeSearch != "") {
        $("#txtUserVSS").val(localStorage.UserNameFromBuyerHomeSearch);
        localStorage.removeItem("UserNameFromBuyerHomeSearch");
    }
    if (localStorage.LoginIdFromBuyerHomeSearch != undefined && localStorage.LoginIdFromBuyerHomeSearch != "") {
        $("#txtUserEmailVSS").val(localStorage.LoginIdFromBuyerHomeSearch);
        localStorage.removeItem("LoginIdFromBuyerHomeSearch");
    }
    $("#hdn-manage-users-current-page").val(1);
    var sortDirection = $(".nameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "FirstName";
    }
    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
}

$('#pageSizeManageUsers').change(function () {
    $("#hdn-manage-users-current-page").val(1);
    var sortDirection = $(".nameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "FirstName";
    }
    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
});

$('.manageUserSearchFilter').change(function () {
    $("#hdn-manage-users-current-page").val(1);
    var sortDirection = $(".nameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "FirstName";
    }
    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
});

function ManageUsersList(index, sortParameter, sortDirection) {
    $("#manage-user-header-text").html(manageUsersButton);
    $("#btnCreateAuditor,#divUsers,#ManageUserFilters,#divSearchSupplierID,#divSearchSupplierEmail,#ManageUsersSearchResults").show();
    $('#create-auditor-footer').hide();
    $("#divCreateAuditorContainer").hide();
    $("#manageAuditorBreadCrumb").show();
    $("#createAuditorBreadCrumb").hide();
    $("#manage-auditor-header").show();
    var userName = ($("#txtUserVSS").val() != $('input[id=txtUserVSS]').attr('placeholder')) ? $("#txtUserVSS").val() : "";
    var pageSizeManageUsers = $("#pageSizeManageUsers").val();
    var status = $("#ddlStatus").val();
    var source = parseInt($('#ddlSource').val());
    var userType = parseInt($('#ddlUserType').val());
    var loginId = ($("#txtUserEmailVSS").val() != $('input[id=txtUserEmailVSS]').attr('placeholder')) ? $("#txtUserEmailVSS").val() : "";
    var obj = { loginId: loginId, userName: userName, userType: userType, status: status, source: source, currentPage: index, pageSize: pageSizeManageUsers, sortDirection: sortDirection }
    $.ajax({
        cache: false,
        type: 'post',
        url: '/Admin/GetAllUsers',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#tblManageUsers').find("tr:gt(0)").remove();
            if (data.usersList.length > 0) {
                for (var i = 0; i < data.usersList.length; i++) {
                    if (i % 2 == 0) {
                        trClass = "odd";
                    }
                    else {
                        trClass = "even";
                    }

                    var tableRow = "<tr class=" + trClass + ">"
                        + "<td><span>" + data.usersList[i].UserName
                        + "</span></td><td>" + data.usersList[i].LoginId
                        + "</td><td>" + data.usersList[i].ActiveStatus
                        + "</td><td>" + data.usersList[i].UserType
                        + "</td><td>" + data.usersList[i].LatestTermsVersion
                        + "</td><td>" + data.usersList[i].LastLoginString
                        + "</td><td>" + data.usersList[i].ProjectSource
                        + "</td><td  class=\"text-align-center\">";
                    if (data.usersList[i].UserType == adminAuditor || data.usersList[i].UserType == auditor) {
                        if (CanEditUser || CanDeleteAuditor || CanChangePassword || CanAssignUserRole || CanDeleteAuditor) {
                            tableRow += "<div class=\"btn-group pull-right\" id=\"ManageUsersActionDropdown\"><button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionsButton + "<span class=\"caret\"></span></button> <ul class=\"dropdown-menu\" >" +
                                "<li class='AssignUserRoles'><a onclick='AssignUserRoles(\"" + data.usersList[i].UserId + "\")'>" + assignRolesButton + "</a></li>" +
                                "<li><a class=\"edit-user-profile\"  data-userId=\"" + data.usersList[i].UserId + "\">" + editUsersButton + "</a></li><li><a class=\"edit-password\" data-userId=\"" + data.usersList[i].UserId + "\" data-loginId=\"" + data.usersList[i].LoginId + "\">" + changePasswordButton + "</a></li><li class='DeleteAuditorConfirmation'><a onclick='DeleteAuditorConfirmation(\"" + data.usersList[i].UserId + "\")')\">" + deleteAuditorButton + "</a></li></ul></div>";
                        }
                    }
                    else {
                        if (CanEditUser || CanChangePassword) {
                            tableRow += "<div class=\"btn-group pull-right\" id=\"ManageUsersActionDropdown\"> <button class=\"btn btn-color dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\">" + actionsButton + "<span class=\"caret\"></span></button> <ul class=\"dropdown-menu\" ><li><a class=\"edit-user-profile\"  data-userId=\"" + data.usersList[i].UserId + "\">" + editUsersButton + "</a></li><li><a class=\"edit-password\" data-userId=\"" + data.usersList[i].UserId + "\" data-loginId=\"" + data.usersList[i].LoginId + "\">" + changePasswordButton + "</a></li></ul></div>";
                        }
                    }
                    tableRow += "</td></tr>";
                    $('#tblManageUsersBody').append(tableRow);

                    $('#tblManageUsers').footable();
                    $('#tblManageUsers').trigger('footable_redraw');
                }
            }
            else {
                $('#tblManageUsersBody').html("<tr><td colspan='10'>" + noRecordsFound + "</td></tr>");
                $('#paginator').remove();
            }
            $('.manageUsersPaginator').html(displayLinks($('#hdn-manage-users-current-page').val(), Math.ceil(data.total / pageSizeManageUsers), '', sortDirection, "ManageUsersList", "#hdn-manage-users-current-page"));
            var contentHtml = "";
            var currentPage = parseInt($('#hdn-manage-users-current-page').val());
            var lastPage = Math.ceil(data.total / pageSizeManageUsers);
            if (data.usersList.length > 0) {
                if (currentPage < lastPage) {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeManageUsers) + 1), (pageSizeManageUsers * currentPage), data.total);
                }
                else {
                    contentHtml = String.format(searchPageData, (((currentPage - 1) * pageSizeManageUsers) + 1), data.total, data.total);
                }
            }
            $('#search-page-data-manageUsers').html(contentHtml);

            if (!CanEditUser) {
                $('.edit-user-profile').hide();
            }
            if (!CanChangePassword) {
                $('.edit-password').hide();
            }
            if (!CanAssignUserRole) {
                $('.AssignUserRoles').hide();
            }
            if (!CanDeleteAuditor) {
                $('.DeleteAuditor').hide();
            }
            ScrollToTop();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert('Failed to retrieve Supplier info.');
        }
    });
}

$(document).on('click', '#btnCancelProfileEdit', function () {
    $('#EditUserDetails').find('form')[0].reset();
    $(".field-validation-valid").html('');
});

$(document).on('click', '#btnSavePasswordChanges', function () {
    $.validator.unobtrusive.parse($('#userPasswordChange'));
    if (!$('#userPasswordChange').valid()) {
        return false;
    }
    $.ajax({
        type: 'post',
        url: '/Admin/ChangeUserPassword',
        data: $('#userPasswordChange').serialize(),
        dataType: "json",
        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#ChangeUserPassword').modal('hide');
                $("#hdn-manage-users-current-page").val(1);
                var sortDirection = $(".nameSort").attr('data-sortdirection');
                var sortParameter = "";
                if (sortDirection != 3) {
                    sortParameter = "FirstName";
                }
                PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
            }
            else
                showErrorMessage(response.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
});

function CreateAuditor(userId) {
    $('#manage-auditor-header').hide();
    $.ajax({
        type: "POST",
        data: { userId: userId },
        url: "/Admin/GetCreateAuditorTemplate",
        success: function (response) {
            if (response && typeof (response) != "undefined") {
                if (response == Common.LogoutAction) {
                    Logout();
                }
                else {
                    $("#createAuditorBreadCrumb").show();
                    $("#ManageUserFilters,#divUsers,#divSearchSupplierID,#divSearchSupplierEmail,#create-user-header,#btnCreateAuditor,#ManageUsersSearchResults").hide();
                    $("#divCreateAuditorContainer").show();
                    $("#divCreateAuditor").html(response);
                    $("#manageAuditorBreadCrumb").hide();
                    $('#create-auditor-footer').show();
                    $("#manage-user-header-text").html(createAuditorButton);
                }
            }
        },
        error: function (response) {
        }
    });
}

//$(document).on('change', '#txtEmail', function (e) {
//    var logId = $('#txtEmail').val();
//    $.ajax({
//        type: 'post',
//        url: '/Admin/GetLoginId',
//        data: { loginId: logId },
//        dataType: "json",
//        async: false,
//        success: function (response) {
//            if (response.id == logId) {
//                showErrorMessage(response.message);
//                $("#btnSaveAuditorDetails").prop('disabled', true);
//            }
//            else {
//                $("#btnSaveAuditorDetails").prop('disabled', false);
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//        }

//    });
//    return false;
//});

$(document).on('click', '#btnResetUserProfile', function () {
    var logId = $('#txtUserloginID').val();
    $('#EditUserDetails').find('form')[0].reset();
    $('.field-validation-valid span').html('');
    $('#txtUserloginID').val(logId);
    $("#txtUserPrimaryFirstName").val("");
    $("#txtUserPrimaryEmail").val("");
    $('#txtUserJobTitle').val("");
    $('#txtUserFirstName').val("");
    $('#txtUserLastName').val("");
    $("#txtUserPrimaryLastName").val("");
    $("#txtUserTelephone").val("");
    $("#hdnProfileUserId").val("");
    $('#hdncompanyID').val("");
});

function SaveAuditorDetails() {
    $.validator.unobtrusive.parse($('#formCreateAuditor'))
    var selectedRoles = $('#ddlRoles option:selected');
    var selected = "";
    $(selectedRoles).each(function (index, selectedRoles) {
        selected += $(this).val() + ",";
    });

    $("#SelectedRoles").val(selected);
    if (!$('#formCreateAuditor').valid()) {
        showErrorMessage(requiredFieldsMessage);
        ScrollToTop();
    }
    else if (!GV.emailIdAvailable) {
        showErrorMessage(emailErrorMessage);
        ScrollToTop();
    }
    else {
        var data = $("#formCreateAuditor").serialize();
        $.ajax({
            type: "POST",
            url: "/Admin/AddOrUpdateAuditorUser",
            data: data,
            success: function (response) {
                if (response && typeof (response) != "undefined") {
                    if (response == Common.LogoutAction) {
                        Logout();
                    }
                    else {
                        //TODO:: Display message
                        if (response.success == true || response.success == false) {
                            (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.message);
                            if (response.success) {
                                BackToAuditorList();
                                $("#hdn-manage-users-current-page").val(1);
                                var sortDirection = $(".nameSort").attr('data-sortdirection');
                                var sortParameter = "";
                                if (sortDirection != 3) {
                                    sortParameter = "FirstName";
                                }
                                PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
                            }
                        }
                    }
                }
                else
                    showErrorMessage(errorMessage);
            },
            error: function (response) {
            }
        })
    }
}

function BackToAuditorList() {
    $("#manage-user-header-text").html(manageUsersButton);
    $("#btnCreateAuditor,#divUsers,#ManageUserFilters,#divSearchSupplierID,#divSearchSupplierEmail,#ManageUsersSearchResults").show();
    $('#create-auditor-footer').hide();
    $("#divCreateAuditorContainer").hide();
    $("#manageAuditorBreadCrumb").show();
    $("#createAuditorBreadCrumb").hide();
    $("#manage-auditor-header").show();
    $('html, body').animate({
        scrollTop: 0
    }, 'slow');
}

$(document).on('click', '.edit-password', function () {
    $('#ChangeUserPassword').find('form')[0].reset();
    $('.field-validation-valid').html('');
    var userId = $(this).attr('data-userId');
    var loginId = $(this).attr('data-loginId');
    OpenPasswordPopUp(userId, loginId);
});

$(document).on('click', '#btnSaveUserDetails', function () {
    $.validator.unobtrusive.parse($('#editUserProfile'));
    if (!$('#editUserProfile').valid()) {
        return false;
    }
    $.ajax({
        type: 'post',
        url: '/Admin/UpdateUserProfile',
        data: $('#editUserProfile').serialize(),
        dataType: "json",
        success: function (response) {
            if (response) {
                showSuccessMessage(response.message);
                $('#EditUserDetails').modal('hide');
                var sortDirection = $(".nameSort").attr('data-sortdirection');
                var sortParameter = "";
                if (sortDirection != 3) {
                    sortParameter = "FirstName";
                }
                PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
            }
            else
                showErrorMessage(response.message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return false;
});

function OpenPasswordPopUp(userId, loginId) {
    $('#ChangeUserPassword').modal('show');
    $("#tbloginIDEditPassword").val(loginId);
    $("#hdnUserId").val(userId);
}

$(document).on('click', '#btnResetPasswordDetails', function () {
    var loginId = $('#tbloginIDEditPassword').val();
    $('#ChangeUserPassword').find('form')[0].reset();
    $('.field-validation-valid').html('');
    var userId = $('#hdnUserId').val();
    OpenPasswordPopUp(userId, loginId);
});

$(document).on('click', '#btnCancelPasswordChange', function () {
    $('#ChangeUserPassword').find('form')[0].reset();
    $('.field-validation-valid').html('');
})

$(document).on('click', '.edit-user-profile', function () {
    $('#EditUserDetails').find('form')[0].reset();
    $('.field-validation-valid').html('');
    var userID = $(this).attr('data-userId');
    $.ajax({
        type: 'post',
        url: '/Admin/EditUserProfile',
        data: { userId: userID },
        dataType: "json",
        success: function (data) {
            var items = data.users;
            $("#txtUserloginID").val(items.LoginId);
            $("#txtUserFirstName").val(items.FirstName);
            $("#txtUserLastName").val(items.LastName);
            $("#txtUserPrimaryFirstName").val(items.PrimaryFirstName);
            $("#txtUserPrimaryEmail").val(items.PrimaryEmail);
            $("#txtUserJobTitle").val(items.JobTitle);
            $("#txtUserPrimaryLastName").val(items.PrimaryLastName);
            $("#txtUserTelephone").val(items.Telephone);
            $("#hdnProfileUserId").val(items.UserId);
            $("#hdnPrimaryContactPartyId").val(items.PrimaryContactPartyId);
            $("#hdnOrganizationPartyId").val(items.OrganizationPartyId);
            $('#EditUserDetails').modal('show');
        },
        error: function (data) {
        }

    });
    return false;
});

function AssignUserRoles(userId) {
    GetUserRole(userId);
    $('#modalAssignRoles').modal('show');
}

function GetUserRole(userId) {
    $.ajax({
        type: "POST",
        url: "/Admin/GetAuditorDetails",
        data: { userId: userId },
        success: function (response) {

            if (response && typeof (response) != "undefined") {
                if (response == Common.LogoutAction) {
                    Logout();
                }
                else {
                    var roles = "", tbody = "";

                    if (response.auditorModel.Roles) {
                        for (var i = 0; i < response.auditorModel.Roles.length; i++) {
                            roles += "<option value=" + response.auditorModel.Roles[i].Id + ">" + response.auditorModel.Roles[i].Name + "</option>";
                        }
                    }
                    if (response.auditorModel) {
                        tbody += " <div class='row padding-top-10px'><div class='col-md-12'><div class='col-md-2'>" + nameButton + " :</div>"
                            + "</div><div class='col-md-12'><div class='col-md-8'>" + response.auditorModel.FirstName + "  " + response.auditorModel.LastName
                            + "</div></div></div><div class='row padding-top-10px'><div class='col-md-12'><div class='col-md-2 required'>" + rolesButton + "</div>"
                            + "</div><div class='col-md-12'><div class='col-md-8'><select id='ddlUserRoles' multiple='multiple' class='form-control'>" + roles
                            + "</select></div></div></div><input type='hidden' id='hdnUserId' />";
                    }
                    $("#divAssignRoles").html(tbody);
                    $('#ddlUserRoles').multiselect({
                        includeSelectAllOption: true,
                        enableCaseInsensitiveFiltering: true,
                        buttonWidth: '80%',
                        maxHeight: 200
                    });

                    $("#hdnUserId").val(userId);
                    $('#ddlUserRoles').multiselect('select', response.auditorModel.SelectedRoles.split(','));
                    $('#ddlUserRoles').multiselect('refresh');
                }
            }
            else
                showErrorMessage(errorMessage);
        },
        error: function (response) {

        }
    })
}

function UpdateAuditorRoles() {
    var selectedRoles = $('#divAssignRoles option:selected');
    var userId = $("#hdnUserId").val();
    var selected = "";
    $(selectedRoles).each(function (index, selectedRoles) {
        selected += $(this).val() + ",";
    });
    $.ajax({
        type: "POST",
        url: "/Admin/UpdateAuditorRoles",
        data: { userId: userId, selectedRoles: selected },
        success: function (response) {

            if (response && typeof (response) != "undefined") {
                if (response == Common.LogoutAction) {
                    Logout();
                }
                else {
                    //TODO:: Display message
                    if (response.success == true || response.success == false) {
                        if (response.success) {
                            showSuccessMessage(response.message);
                        }
                        else {
                            showErrorMessage(response.message);
                        }
                        if (response.success == true) {
                            $('#modalAssignRoles').modal('hide');
                        }
                    }
                }
            }
            else
                showErrorMessage(errorMessage);
        },
        error: function (response) {

        }
    })
}

//function ResetSearchFilters() {
//    $('#txtUserVSS').val("");
//    $('#txtCompanyName').val("");
//    $('#ddlStatus').get(0).selectedIndex = 0;
//    $('#ddlUserType').get(0).selectedIndex = 0;
//    $("#hdn-manage-users-current-page").val(1);
//    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", '', 3);
//}

$("#ddlUserType").change(function () {
    $('#search-manager-filter-header-clear').show();
});

$("#ddlSource").change(function () {
    $('#search-manager-filter-header-clear').show();
});

$("#ddlStatus").change(function () {
    $('#search-manager-filter-header-clear').show();
});

$(document).on('click', '#search-manager-filter-header-clear', function () {
    $('#ddlStatus').get(0).selectedIndex = 0;
    $('#ddlSource').get(0).selectedIndex = 0;
    $('#ddlUserType').get(0).selectedIndex = 0;
    $('#search-manager-filter-header-clear').hide();
    $("#hdn-manage-users-current-page").val(1);
    $('.fa-sort-asc').addClass('fa-sort').removeClass('fa-sort-asc');
    $('.fa-sort-desc').addClass('fa-sort').removeClass('fa-sort-desc');
    $(".nameSort").attr('data-sortDirection', '3');
    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", '', 3);
});

//$('#accountStatusSourceCheck').click(function () {
//    $("#hdn-manage-users-current-page").val(1);
//    var sortDirection = $(".nameSort").attr('data-sortdirection');
//    var sortParameter = "";
//    if (sortDirection != 3) {
//        sortParameter = "FirstName";
//    }
//    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
//});

//$('#accountStatusCIPSCheck').click(function () {
//    $("#hdn-manage-users-current-page").val(1);
//    var sortDirection = $(".nameSort").attr('data-sortdirection');
//    var sortParameter = "";
//    if (sortDirection != 3) {
//        sortParameter = "FirstName";
//    }
//    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
//});

function SearchUserOnEnter() {
    $("#hdn-manage-users-current-page").val(1);
    var sortDirection = $(".nameSort").attr('data-sortdirection');
    var sortParameter = "";
    if (sortDirection != 3) {
        sortParameter = "FirstName";
    }
    PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
    $('.intellisense-container').hide();
}

$(document).on('click', '.nameSort', function () {
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
        PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", '', sortDirection);
    }
    else {
        PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", 'FirstName', sortDirection);
    }
});

$('#btnManageUsersExport').click(function () {
    var userName = ($("#txtUserVSS").val() != $('input[id=txtUserVSS]').attr('placeholder')) ? $("#txtUserVSS").val() : "";
    var loginId = ($("#txtUserEmailVSS").val() != $('input[id=txtUserEmailVSS]').attr('placeholder')) ? $("#txtUserEmailVSS").val() : "";
    var userType = $('#ddlUserType').val();
    var status = $('#ddlStatus').val();
    var source = $('#ddlSource').val();
    window.location.href = "/Admin/ManageUsersDownload/?loginId=" + loginId + "&userName=" + userName + "&userType=" + userType + "&status=" + status + "&source=" + source;
});

$(window).scroll(function () {
    FixedTableHeader('tblManageUsers');
});

$(window).resize(function () {
    FixedTableHeader('tblManageUsers');

});

$(window).scroll(function () {
    FixedTableHeaderWithPagination('tblManageUsers');
});

$(window).resize(function () {
    FixedTableHeaderWithPagination('tblManageUsers');

});

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

function DeleteAuditorConfirmation(userId) {
    $('#auditorDeleteConfirmation').modal('show');
    $("#hdnAuditorId").val(userId);
}

function DeleteAuditor() {
    var userId = $("#hdnAuditorId").val();
    if (userId != "") {
        $.ajax({
            type: "POST",
            data: { userId: userId },
            url: "/Admin/DeleteAuditor",
            success: function (response) {
                if (response && typeof (response) != "undefined") {
                    if (response == Common.LogoutAction) {
                        Logout();
                    }
                    else {
                        if (response.success == true || response.success == false) {
                            (response.success) ? showSuccessMessage(response.message) : showErrorMessage(response.message);
                            $('#auditorDeleteConfirmation').modal('hide');
                            if (response.success == true) {
                                $("#hdn-manage-users-current-page").val(1);
                                var sortDirection = $(".nameSort").attr('data-sortdirection');
                                var sortParameter = "";
                                if (sortDirection != 3) {
                                    sortParameter = "FirstName";
                                }
                                PagerLinkClick('1', "ManageUsersList", "#hdn-manage-users-current-page", sortParameter, sortDirection);
                            }
                        }
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