$(function() {
    // datatable default options
    tableOptions.defaults();

    var $parentStudentTable = $("#parentStudentsTable");
    var dataTableUrl = $parentStudentTable.data("datatableurl");
    var $parentStudentModal = $("#parentStudentModal");

    // setup the datatable
    var parentStudentDataTable = $parentStudentTable.DataTable({
        "ajax": function(data, callback, settings) {
            $.post(dataTableUrl, data, function(response) {
                callback({
                    recordsTotal: response.result.recordsTotal,
                    recordsFiltered: response.result.recordsFiltered,
                    draw: response.result.draw,
                    data: response.result.data
                });
            });
        },
        "columns": [
            { "data": "studentName", "orderable": true },
            {
                data: null,
                orderable: false,
                render: function(data) {
                    return "<span class='glyphicon glyphicon-new-window' style='cursor: pointer;'></span>";
                }
            }
        ]
    }).on('preXhr.dt', function(e, settings, data) {

    });

    $parentStudentTable.on('click', '.glyphicon-new-window', function() {
        var clickedRow = $(this).closest('tr');
        var rowData = parentStudentDataTable.row(clickedRow).data();

        var query = {
            id: rowData.id
        }

        loadForm("Student Classes", $parentStudentModal, query);
    });
})