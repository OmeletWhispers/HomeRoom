
// saveAssignmentType - saves the form for an assignmentType
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveAssignmentType = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}

// removeStudent - removes the student from a class
// resourceUrl - where to send the request to remove the student. Include the classId as query parameters
var removeStudent = function (resourceUrl) {
    return abp.ajax({
        url: resourceUrl
    });
}

$(function () {

    var classId = $("#ClassId").val();
    // datatable default options
    tableOptions.defaults();

    var $assignmentTypeTable = $("#classAssignmentTypeTable");
    var dataTableUrl = $assignmentTypeTable.data("datatableurl");
    var $assignmentTypeModal = $("#manageAssignmentTypesModal");
    var assignmentTypeDataTable = null;

    // bind events
    // saving an assignment type
    $("#manageAssignmentTypesSaveBtn").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#assignmentTypeForm").attr("action");
        var enrollmentObj = $("#assignmentTypeForm").serializeFormToObject();


        abp.ui.setBusy($assignmentTypeModal, saveAssignmentType(enrollmentObj, resourceUrl).done(function (response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($assignmentTypeTable);
                assignmentTypeDataTable.ajax.reload();
                abp.ui.clearBusy($assignmentTypeTable);
                $assignmentTypeModal.modal('hide');
            }
        }));
    });

    // Editing an assignment type
    $assignmentTypeTable.on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = assignmentTypeDataTable.row(clickedRow).data();

        var query = {
            classId: classId,
            id: rowData.id
        }

        loadForm("Edit Assignment Type", $assignmentTypeModal, query);

    });

    // adding an assignment type
    $("#addAssignmentTypeBtn").on('click', function (e) {
        e.preventDefault();

        var query = {
            classId: classId
        }
        loadForm("Add Assignment Type", $assignmentTypeModal, query);
    });

    $('a[href="#manageAssignmentTypes"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#classAssignmentTypeTable")) {

            assignmentTypeDataTable = $assignmentTypeTable.DataTable({
                "ajax": function (data, callback, settings) {
                    data.classId = classId;
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
                    { "data": "name", "orderable": true },
                    { "data": "percentage", "orderable": true },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return "<i class='fa fa-pencil-square-o' style='cursor: pointer;'></i>";
                        }
                    }
                ]
            }).on('preXhr.dt', function (e, settings, data) {
                data.classId = classId;
            });
        }
    });

});