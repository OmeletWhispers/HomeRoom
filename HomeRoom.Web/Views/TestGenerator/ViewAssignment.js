// getAnswers - get the answers that the student submitted
function getAnswers(studentId, assignmentId) {
    var resourceUrl = '/TestGenerator/GetAnswers?studentId=' + studentId + "&assignmentId=" + assignmentId;

    return abp.ajax({
        url: resourceUrl,
        dataType: "html"
    });
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

    // trigger the change on page load to get the first student
    $studentList.trigger('change');
});