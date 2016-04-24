// submitAnswers - submits all the answers for a teacher to grade
function submitAnswers(answerJson) {
    var resourceUrl = '/TestGenerator/SubmitAssignment';

    return abp.ajax({
        type: 'post',
        url: resourceUrl,
        data: answerJson,
        dataType: 'json'
    });
}

// generateShortAnswerObj - generates the object used for short answers
function generateShortAnswerObj(assignmentId, studentId, element) {
    return {
        assignmentId: assignmentId,
        studentId: studentId,
        answerChoiceId: null,
        text: $(element).val()
    }
}

// generateMultiChoiceObj - generates the object used for multiple choice
function generateMultiChoiceObj(assignmentId, studentId, element) {
    return {
        assignmentId: parseInt(assignmentId),
        studentId: parseInt(studentId),
        answerChoiceId: parseInt($(element).val()),
        text: ""
    }
}

$(function() {
    // event to submit an assignment
    $("#submitAnswersBtn").click(function() {
        // grab the assignment id
        var assignmentId = $("#takenAssignmentId").val();
        var studentId = $("#studentUserId").val();

        // grab all the short answers and get the student answers
        var shortAnswers = $(".shortAnswer");
        // grab all the multiple choice answers
        var multipleChoiceAnswers = $("input:radio:checked");

        console.log(shortAnswers);
        console.log(multipleChoiceAnswers);

        var answerChoices = [];
        $.each(shortAnswers, function() {
            answerChoices.push(generateShortAnswerObj(assignmentId, studentId, this));
        });

        $.each(multipleChoiceAnswers, function() {
            answerChoices.push(generateMultiChoiceObj(assignmentId, studentId, this));
        });

        console.log(answerChoices);

        // ask the user to confirm first
        abp.message.confirm("Are you sure?", "Once you submit your answers will be final", function(confirm) {
            if (confirm) {
                var answerJson = JSON.stringify(answerChoices);
                submitAnswers(answerJson);
            } else {
                return false;
            }
        });
    });
});