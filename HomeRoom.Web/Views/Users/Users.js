// user ajax functions
// saveUserForm - saves the form for creating/editing a user
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveUserForm = function(formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: formSerialize,
        contentType: "application/x-www-form-urlencoded"
    });
}

// deleteUser - request to delete the user from the database
// resourceUrl - where to send the request to delete the user. Include the userId as query parameters
var deleteUser = function(resourceUrl) {
    return abp.ajax({
        url: resourceUrl
    });
}

$(function () {
    tableOptions.defaults();

    var $userTable = $("#usersTable");
    var dataTableUrl = $userTable.data("datatableurl");
    var $userModal = $("#usersModal");

    // setup the datatable
    var userDataTable = $userTable.DataTable({
        "ajax": function (data, callback, settings) {
            $.post(dataTableUrl, data, function (response) {
                callback({
                    recordsTotal: response.result.recordsTotal,
                    recordsFiltered: response.result.recordsFiltered,
                    draw: response.result.draw,
                    data: response.result.data
                });
            });
        },
        "columns": [
            { "data": "firstName", "orderable": true },
            { "data": "lastName", "orderable": true },
            { "data": "email", "orderable": true },
            {
                data: null,
                orderable: false,
                render: function(data) {
                    return "<span class='glyphicon glyphicon-user' style='cursor: pointer;'></span>";
                }
            },
            {
                data: null,
                orderable: false,
                render: function(data) {
                    return "<i class='fa fa-trash-o' style='cursor: pointer;'></i>";
                }
            }
        ]
    }).on('preXhr.dt', function (e, settings, data) {

    });

    // want to create a user
    $("#createBtn").on('click', function(e) {
        e.preventDefault();

        loadForm("Create User", $("#usersModal"));
    });


    // user form submit
    $("#createUser").on('click', function(e) {
        e.preventDefault();

        var resourceUrl = $("#usersForm").attr("action");
        var formSerialize = $("#usersForm").serialize();

        abp.ui.setBusy($userModal, saveUserForm(formSerialize, resourceUrl).done(function(response) {
            abp.notify.success(response.msg);
            userDataTable.ajax.reload();
            $userModal.modal('hide');
        }));
    });

    // viewing/editing user
    $("#usersTable").on('click', '.glyphicon-user', function(e) {
        e.preventDefault();
        var clickedRow = $(this).closest('tr');
        var rowData = userDataTable.row(clickedRow).data();
        var query = {
            userId: rowData.id
        };

        loadForm("Edit User", $("#usersModal"), query);

    });

    // deleting a user
    $("#usersTable").on('click', '.fa-trash-o', function (e) {
        e.preventDefault();
        var clickedRow = $(this).closest('tr');
        var rowData = userDataTable.row(clickedRow).data();
        var firstName = rowData.firstName;
        var resourceUrl = 'Users/DeleteUser?userId=' + rowData.id;

        abp.message.confirm(firstName + " will be deleted", "Are you sure?", function(confirm) {
            if (confirm) {
                // send request to delete user
                abp.ui.setBusy($userTable, deleteUser(resourceUrl).done(function(response) {
                    abp.notify.success(response.msg);
                    userDataTable.ajax.reload();
                }));
            }
        });

    });

});
