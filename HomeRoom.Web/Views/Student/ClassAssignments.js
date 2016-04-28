
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

    var $assignmentsTable = $("#studentClassAssignmentTable");
    var dataTableUrl = $assignmentsTable.data("datatableurl");
    var $assignmentsModal = $("#manageAssignmentsModal");
    var assignmentsDataTable = null;

    $('a[href="#manageAssignments"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#studentClassAssignmentTable")) {

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
                            debugger;
                            if (data.canView)
                                return "<a href=" + data.url + " target='_blank'><span class='glyphicon glyphicon-new-window' style='cursor: pointer;'></span></a>";
                            else
                                return "";
                        }
                    }
                ]
            }).on('preXhr.dt', function (e, settings, data) {
                data.classId = classId;
            });
        }
    });

    // initialize the date time pickers for the assignment form when the modal shows
    $assignmentsModal.on('show.bs.modal', function() {
        $("#assignmentStartDate").datetimepicker();
        $("#assignmentDueDate").datetimepicker();
    });

});