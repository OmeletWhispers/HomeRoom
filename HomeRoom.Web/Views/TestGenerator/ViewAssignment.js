// getAnswers - get the answers that the student submitted
function getAnswers(studentId, assignmentId) {
    var resourceUrl = '/TestGenerator/GetAnswers?studentId=' + studentId + "&assignmentId=" + assignmentId;

    return abp.ajax({
        url: resourceUrl,
        dataType: "html"
    });
}

// gradeAssignment - grades the assignment
function gradeAssignment(grades) {
    var resourceUrl = '/TestGenerator/GradeAssignment/';

    return abp.ajax({
        url: resourceUrl,
        data: grades,
        dataType: 'json'
    });
}

// generateGradeObj - generates a grade obj
function generateGradeObj(assignmentId, studentId, element) {
    var pointsWorth = parseInt($(element).data("points"));
    var pointsReceived = element.checked ? pointsWorth : 0;

    return {
        assignmentId: assignmentId,
        studentId: studentId,
        pointsWorth: pointsWorth,
        pointsReceived: pointsReceived
    };
}

$(function() {
    var $studentList = $("#studentAssignmentSelectList");
    var assignmentId = $("#assignmentId").val();

    // bind events
    $studentList.on('change', function (e) {
        var $assignmentDiv = $("#studentAssignmentAnswersDiv");
        var value = $(this).val();
        

        abp.ui.setBusy($assignmentDiv, getAnswers(value, assignmentId).done(function(response) {
            $assignmentDiv.empty();
            $assignmentDiv.html(response);
        }));
    });

    // grading the assignment
    $("#gradeAssignment").on('click', function () {
        var studentId = parseInt($("#studentAssignmentSelectList").val());
        // get all the question answers
        var questionAnswers = $(".questionAnswer");
        var grades = [];

        $.each(questionAnswers, function() {
            grades.push(generateGradeObj(assignmentId, studentId, this));
        });

        var gradesJson = JSON.stringify(grades);

        abp.ui.setBusy(null, gradeAssignment(gradesJson).done(function (response) {
            abp.notify.success(response.msg);
        }));
    });

    // trigger the change on page load to get the first student
    $studentList.trigger('change');
});