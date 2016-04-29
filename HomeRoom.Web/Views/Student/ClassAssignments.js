
// assignments ajax functions
// viewAssignment - view an assignment details
var viewAssignment = function (formSerialize, resourceUrl) {
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
    var $assignmentsModal = $("#viewAssignmentsModal");
    var assignmentsDataTable = null;

    $assignmentsTable.on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = assignmentsDataTable.row(clickedRow).data();

        var query = {
            assignmentId: rowData.id
        }

        loadForm("View Assignment", $assignmentsModal, query);

    });

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
                    { "data": "status", "orderable": true },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return '<i style="cursor: pointer;" data-toggle="tooltip" data-placement="bottom" title="View Assignment Details" class="fa fa-pencil-square-o" aria-hidden="true"></i>';
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            if (data.canView) {
                                var url = "/TestGenerator/TakeAssignment?assignmentId=" + data.id;
                                return "<a href=" + url + "><span data-toggle='tooltip' data-placement='bottom' title='Do Assignment' class='glyphicon glyphicon-new-window' style='cursor: pointer;'></span></a>";
                            }
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