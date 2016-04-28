// gradebook ajax functions

// getDataTableColumns - grabs the columsn for the datatable
// resourceUrl - the url to use
function getDataTableColumns(resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        type: "get"
    });
}

// insertGrades - inserts grades to a listof students for a specific 
// formSerialize - the form serialized
// resourceUrl - resources url
function insertGrades(formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}


$(function() {
    var classId = $("#ClassId").val();
    // datatable default options
    tableOptions.defaults();

    var $gradeBookTable = $("#classGradeBookTable");
    var dataTableUrl = $gradeBookTable.data("datatableurl");
    var $gradeBookModal = $("#manageGradeBookModal");
    var $studentGradeBookModal = $("#studentGradeBookModal");
    var $manageStudentGradesModal = $("#manageStudentGradeModal");
    var gradebookDataTable = null;

    // only when we show this tab
    $('a[href="#manageGradebook"]').on('shown.bs.tab', function () {
        // only init the datatable if it hasn't been inited yet and only if we have actually received our columns
        if (!$.fn.dataTable.isDataTable("#classGradeBookTable")) {
            gradebookDataTable = $gradeBookTable.DataTable({
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
                    {
                        data: null,
                        orderable: true,
                        render: function (data) {
                            return "<button class='btn btn-link'>" + data.studentName + "</button>";
                        }
                    },
                    { "data": "currentGrade", "orderable": true },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return '<i class="fa fa-pencil" aria-hidden="true", style="cursor: pointer;"></i>';
                        }
                    }
                ]
            }).on('pre.Xhr', function (e, settings, data) {
                data.classId = classId;
            });
        }
    });

    $("#addGradeBtn").on('click', function(e) {
        e.preventDefault();


        var query = {
            classId: classId
        }
        loadForm("New Grade", $gradeBookModal, query);
    });

    // clicked on a student to view their grades
    $gradeBookTable.on('click', '.btn-link', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = gradebookDataTable.row(clickedRow).data();

        var query = {
            studentId: rowData.id,
            classId: classId
        }

        loadForm("Student Grades", $studentGradeBookModal, query);
    });

    // clicked on a editing a students assignments grades
    $gradeBookTable.on('click', '.fa-pencil', function(e) {
        var clickedRow = $(this).closest('tr');
        var rowData = gradebookDataTable.row(clickedRow).data();
        var studentName = rowData.studentName;

        var query = {
            studentId: rowData.id,
            classId: classId
        }

        loadForm(studentName, $manageStudentGradesModal, query);
    });

    // saving a gradebook entry
    $("#saveGradeFormBtn").on('click', function(e) {
        e.preventDefault();

        var $gradeBookForm = $("#gradeBookForm");

        var resourceUrl = $gradeBookForm.attr('action');
        var formSerialize = $gradeBookForm.serializeFormToObject();

        abp.ui.setBusy($gradeBookModal, insertGrades(formSerialize, resourceUrl).done(function(response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($gradeBookTable);
                gradebookDataTable.ajax.reload();
                abp.ui.clearBusy($gradeBookTable);
                $gradeBookModal.modal('hide');
            } else {
                abp.message.error(response.msg, "Error saving the grades for this assignment.");
            }
        }));
    });

    // saving a student grades
    $("#saveStudentGradesBtn").on('click', function(e) {
        e.preventDefault();

        var $manageStudentGradesForm = $("#manageStudentGradesForm");
        var resourceUrl = $manageStudentGradesForm.attr('action');
        var formSerialize = $manageStudentGradesForm.serializeFormToObject();

        abp.ui.setBusy($manageStudentGradesModal, insertGrades(formSerialize, resourceUrl).done(function (response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($gradeBookTable);
                gradebookDataTable.ajax.reload();
                abp.ui.clearBusy($gradeBookTable);
                $manageStudentGradesModal.modal('hide');
            } else {
                abp.message.error(response.msg, "Error saving the grades for this student.");
            }
        }));
    });
});