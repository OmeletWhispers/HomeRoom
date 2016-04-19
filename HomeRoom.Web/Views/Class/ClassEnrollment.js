
// enrollStudentForm - saves the form for enrolling a student
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var enrollStudentForm = function (formSerialize, resourceUrl) {
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

    var $enrollmentsTable = $("#classEnrollmentTable");
    var dataTableUrl = $enrollmentsTable.data("datatableurl");
    var $studentModal = $("#enrollStudentModal");
    var enrollmentDataTable = null;

    // bind events
    // removing a student from a class
    $("#classEnrollmentTable").on('click', '.fa-trash-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = enrollmentDataTable.row(clickedRow).data();
        var studentId = rowData.id;
        var studentName = rowData.studentName;

        var resourceUrl = abp.utils.formatString("/Class/RemoveStudent?classId={0}&studentId={1}", classId, studentId);

        abp.message.confirm(studentName + " will be removed from the class.", "Are you sure?", function (confirm) {
            if (confirm) {
                // send request to delete student from class
                abp.ui.setBusy($enrollmentsTable, removeStudent(resourceUrl).done(function (response) {
                    abp.notify.success(response.msg);
                    enrollmentDataTable.ajax.reload();
                }));
            }
        });

    });

    // enrolling a student into the class
    $("#enrollStudentSaveBtn").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#enrollStudentForm").attr("action");
        var enrollmentObj = $("#enrollStudentForm").serializeFormToObject();


        abp.ui.setBusy($studentModal, enrollStudentForm(enrollmentObj, resourceUrl).done(function (response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($enrollmentsTable);
                enrollmentDataTable.ajax.reload();
                abp.ui.clearBusy($enrollmentsTable);
                $studentModal.modal('hide');
            } else {
                //abp.notify.error(response.msg, "Could not Enroll Student");
                abp.message.error(response.msg, "Could not Enroll Student");
            }
        }));
    });

    // editing a student inside a class
    $("#classEnrollmentTable").on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = enrollmentDataTable.row(clickedRow).data();

        var query = {
            classId: classId,
            studentId: rowData.id
        }

        loadForm("Edit Class", $studentModal, query);

    });

    // adding a student to a class
    $("#enrollStudentBtn").on('click', function (e) {
        e.preventDefault();

        var query = {
            classId: classId
        }
        loadForm("Enroll Student", $studentModal, query);
    });

    $('a[href="#manageStudents"]').on('shown.bs.tab', function() {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#classEnrollmentTable")) {

            enrollmentDataTable = $enrollmentsTable.DataTable({
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
                    { "data": "studentName", "orderable": true, "width": "100%" },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return "<i class='fa fa-pencil-square-o' style='cursor: pointer;'></i>";
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return "<i class='fa fa-trash-o' style='cursor: pointer;'></i>";
                        }
                    }
                ]
            }).on('preXhr.dt', function (e, settings, data) {
                data.classId = classId;
            });
        }
    });
    
});