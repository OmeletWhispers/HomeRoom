var saveUser = function(user, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(user)
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
            { "data": "email", "orderable": true }
        ]
    }).on('preXhr.dt', function (e, settings, data) {

    });


    $("#createBtn").on('click', function(e) {
        e.preventDefault();

        loadForm("Create User", $("#usersModal"));
    });

    $("#createUser").on('click', function(e) {
        e.preventDefault();

        var resourceUrl = $("#usersForm").attr("action");
        var formData = $("#usersForm").serializeFormToObject();

        abp.ui.setBusy($userModal, saveUser(formData, resourceUrl).done(function(response) {
            abp.notify.success(response.msg);
            userDataTable.ajax.reload();
            $userModal.modal('hide');
        }));
    });

});
