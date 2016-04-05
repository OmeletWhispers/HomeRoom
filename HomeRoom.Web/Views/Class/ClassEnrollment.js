
// enrollStudentForm - saves the form for enrolling a student
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var enrollStudentForm = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
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
                    }
                ]
            }).on('preXhr.dt', function (e, settings, data) {
                data.classId = classId;
            });
        }

        // enrolling a student into the class
        $("#enrollStudentSaveBtn").on('click', function(e) {
            e.preventDefault();

            var resourceUrl = $("#enrollStudentForm").attr("action");
            var enrollmentObj = $("#enrollStudentForm").serializeFormToObject();


            abp.ui.setBusy($studentModal, enrollStudentForm(enrollmentObj, resourceUrl).done(function (response) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($enrollmentsTable);
                enrollmentDataTable.ajax.reload();
                abp.ui.clearBusy($enrollmentsTable);
                $studentModal.modal('hide');
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
        $("#enrollStudentBtn").on('click', function(e) {
            e.preventDefault();

            var query = {
                classId: classId
            }
            loadForm("Enroll Student", $studentModal, query);
        });
    });
    
});