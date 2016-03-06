
$(function () {
    tableOptions.defaults();

    var $userTable = $("#usersTable");
    var dataTableUrl = $userTable.data("datatableurl");

    // setup the datatable
    $userTable.DataTable({
        "ajax": function (data, callback, settings) {
            $.post(dataTableUrl, data, function (response) {
                debugger;
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

});
