// saveAssignmentType - saves the form for an assignmentType
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveCategory = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}


$(function () {

    // datatable default options
    tableOptions.defaults();

    var $categoriesTable = $("#categoriesTable");
    var dataTableUrl = $categoriesTable.data("datatableurl");
    var $categoriesModal = $("#categoryModal");
    var categoriesDataTable = null;

    // bind events
    // saving a subject
    $("#createCategoryBtn").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#categoryForm").attr("action");
        var categoryObj = $("#categoryForm").serializeFormToObject();


        abp.ui.setBusy($categoriesModal, saveSubject(categoryObj, resourceUrl).done(function (response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($categoriesTable);
                categoriesDataTable.ajax.reload();
                abp.ui.clearBusy($categoriesTable);
                $categoriesModal.modal('hide');
            }
        }));
    });

    // Editing an assignment type
    $categoriesTable.on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = categoriesDataTable.row(clickedRow).data();

        var query = {
            id: rowData.id
        }

        loadForm("Edit Category", $categoriesModal, query);

    });

    // adding an assignment type
    $("#addCategoryBtn").on('click', function (e) {
        e.preventDefault();

        loadForm("Add Category", $categoriesModal);
    });

    $('a[href="#categories"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#categoriesTable")) {

            categoriesDataTable = $categoriesTable.DataTable({
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
                    { "data": "categoryName", "orderable": true },
                    { "data": "subjectName", "orderable": true },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return "<i class='fa fa-pencil-square-o' style='cursor: pointer;'></i>";
                        }
                    }
                ]
            }).on('preXhr.dt', function (e, settings, data) {
            });
        }
    });

});