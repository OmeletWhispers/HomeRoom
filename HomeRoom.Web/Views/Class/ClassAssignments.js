
// assignments ajax functions
// saveAssignmentForm - saves the form for creating/editing am assignment
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveAssignmentForm = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}

$(function () {

    var classId = $("#ClassId").val();
    // datatable default options
    tableOptions.defaults();

    var $assignmentsTable = $("#classAssignmentTable");
    var dataTableUrl = $assignmentsTable.data("datatableurl");
    var $assignmentsModal = $("#manageAssignmentsModal");
    var assignmentsDataTable = null;

    $('a[href="#manageAssignments"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#classAssignmentTable")) {

            assignmentsDataTable = $assignmentsTable.DataTable({
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
                    { "data": "assignmentName", "orderable": true },
                    { "data": "assignmentType", "orderable": true},
                    { "data": "startDate", "orderable": true},
                    { "data": "dueDate", "orderable": true},
                    { "data": "status", "orderable": true},
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

        // removing a student from a class
        $assignmentsTable.on('click', '.fa-trash-o', function (e) {
            var clickedRow = $(this).closest('tr');
            var rowData = assignmentsDataTable.row(clickedRow).data();
            var studentId = rowData.id;
            var studentName = rowData.studentName;

            var resourceUrl = abp.utils.formatString("/Class/RemoveStudent?classId={0}&studentId={1}", classId, studentId);

            abp.message.confirm(studentName + " will be removed from the class.", "Are you sure?", function (confirm) {
                if (confirm) {
                    // send request to delete student from class
                    abp.ui.setBusy($assignmentsTable, removeStudent(resourceUrl).done(function (response) {
                        abp.notify.success(response.msg);
                        assignmentsDataTable.ajax.reload();
                    }));
                }
            });

        });

        // enrolling a student into the class
        $("#manageAssignmentsSaveBtn").on('click', function (e) {
            e.preventDefault();

            var resourceUrl = $("#classAssignmentForm").attr("action");
            var assignmentObj = $("#classAssignmentForm").serializeFormToObject();


            abp.ui.setBusy($assignmentsModal, saveAssignmentForm(assignmentObj, resourceUrl).done(function (response) {
                if (!response.error) {
                    abp.notify.success(response.msg, "");

                    abp.ui.setBusy($assignmentsTable);
                    assignmentsDataTable.ajax.reload();
                    abp.ui.clearBusy($assignmentsTable);
                    $assignmentsModal.modal('hide');
                } else {
                    abp.message.error(response.msg, "Error saving the assignment.");
                }
            }));
        });

        // editing a student inside a class
        $assignmentsTable.on('click', '.fa-pencil-square-o', function (e) {
            var clickedRow = $(this).closest('tr');
            var rowData = assignmentsDataTable.row(clickedRow).data();

            var query = {
                classId: classId,
                assignmentId: rowData.id
            }

            loadForm("Edit Assignment", $assignmentsModal, query);

        });

        // adding a student to a class
        $("#addAssignmentBtn").on('click', function (e) {
            e.preventDefault();

            var query = {
                classId: classId
            }
            loadForm("New Assignment", $assignmentsModal, query);
        });
    });

    // initialize the date time pickers for the assignment form when the modal shows
    $assignmentsModal.on('show.bs.modal', function() {
        $("#assignmentStartDate").datetimepicker();
        $("#assignmentDueDate").datetimepicker();
    });

});