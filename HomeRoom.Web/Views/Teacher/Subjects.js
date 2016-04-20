// saveAssignmentType - saves the form for an assignmentType
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveAssignmentType = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}


$(function () {

    // datatable default options
    tableOptions.defaults();

    var $subjectsTable = $("#subjectsTable");
    var dataTableUrl = $subjectsTable.data("datatableurl");
    var $subjectsModal = $("#subjectsModal");
    var subjectsDataTable = null;

    // bind events
    // saving a subject
    $("#createSubjectBtn").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#subjectForm").attr("action");
        var enrollmentObj = $("#subjectForm").serializeFormToObject();


        abp.ui.setBusy($subjectsModal, saveAssignmentType(enrollmentObj, resourceUrl).done(function (response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($subjectsTable);
                subjectsDataTable.ajax.reload();
                abp.ui.clearBusy($subjectsTable);
                $subjectsModal.modal('hide');
            }
        }));
    });

    // Editing an assignment type
    $subjectsTable.on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = subjectsDataTable.row(clickedRow).data();

        var query = {
            id: rowData.id
        }

        loadForm("Edit Subject", $subjectsModal, query);

    });

    // adding an assignment type
    $("#addSubjectBtn").on('click', function (e) {
        e.preventDefault();

        loadForm("Add Subject", $subjectsModal);
    });

    $('a[href="#subjects"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#subjectsTable")) {

            subjectsDataTable = $subjectsTable.DataTable({
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