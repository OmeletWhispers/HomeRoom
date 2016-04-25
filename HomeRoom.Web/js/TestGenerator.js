//ajax functions

// saveTest - saves the test and it's questions to the server
function saveTest(testJson) {
    var resourceUrl = '/TestGenerator/CreateTest/';

    return abp.ajax({
        type: 'post',
        url: resourceUrl,
        data: testJson,
        dataType: 'json'
    });
}

// getCategoryList - grabs the category select list for the selected subject
function getCategoryList(subjectId) {
    var resourceUrl = '/TestGenerator/GetCategories?subjectId=' + subjectId;

    return abp.ajax({
        type: "get",
        url: resourceUrl,
        dataType: "html"
    });
}

// getAssignment - grabs all the assignment questions and questions from the question bank
function getAssignment(assignmentId) {
    var resourceUrl = '/TestGenerator/GetTestGenerator?assignmentId=' + assignmentId;

    return abp.ajax({
        type: "get",
        url: resourceUrl,
        dataType: "html"
    });
}

// applyFilteres - apply the filters to get questions only for this category
function applyFilters(categoryId) {
    var resourceUrl = '/TestGenerator/GetQuestions/?categoryId=' + categoryId;

    return abp.ajax({
        type: "get",
        url: resourceUrl
    });
}


// initDragula - initalizes the dragula and binds all the events that are needed
function initDragula() {
    var generatorDragula = dragula([document.getElementById("questionBankPanel"), document.getElementById("testGeneratorPanel")]);
}

// generateQuestion - generates a question that can be dragged
function generateQuestion(question) {
    var questionDiv = abp.utils.formatString('<div class="question" id="{0}">{1}<input type="text" class="form-control" placeholder="Points Value"/></div>',
        question.questionId, question.question);

    return questionDiv;
}

// generateQuestionObj - generates a question obj that can be serialized to be sent to server
function generateQuestionObj(questionDiv) {
    var questionId = $(questionDiv).attr('id');
    var pointsValue = $(questionDiv).children()[0];


    var question = {
        questionId: parseInt(questionId),
        pointValue: parseInt($(pointsValue).val())
    }

    return question;
}


//submitAssignmentQuestions - runs the logic to submit the assignment questions
function submitAssignmentQuestions(assignmentId) {
    var assignmentQuestions = $("#testGeneratorPanel").children();

    var questions = [];
    $.each(assignmentQuestions, function () {
        questions.push(generateQuestionObj(this));
    });

    var testObj = {
        assignmentId: assignmentId,
        questions: questions
    }

    console.log(questions);
    var formStringify = JSON.stringify(testObj);
    saveTest(formStringify).done(function (response) {
        abp.notify.success(response.msg);
    });
}

$(function () {
    // objects to cache
    var $categoryFilterDiv = $("#categoryFilterDiv");
    var $subjectFilterList = $("#subjectFilterSelectList");
    var $assignmentSelectList = $("#assignmentSelectList");
    var $testGeneratorDiv = $("#testGeneratorDiv");

    // bind events
    // applying filters
    $("#applyFilters").on('click', function (e) {
        var $questionBankPanel = $("#questionBankPanel");

        var category = $("#categorySelectList").val();
        applyFilters(category).done(function (response) {
                $questionBankPanel.empty();
                $.each(response, function () {
                    var question = generateQuestion(this);
                    $questionBankPanel.append(question);
                });
        });
    });

    // saving an assignment
    $("#saveTestGeneratorBtn").on('click', function(e) {
        var assignmentId = parseInt($assignmentSelectList.val());
        submitAssignmentQuestions(assignmentId);
    });

    // changing the subject list
    $subjectFilterList.on('change', function (e) {
        var value = $(this).val();
        getCategoryList(value).done(function (response) {

            $categoryFilterDiv.empty().html(response);
        });
    });

    // when changing assignments
    $assignmentSelectList.on('change', function (e) {
        debugger;
        var value = $(this).val();
        var previousValue = $(this).data("prev");


        var assignmentQuestions = $("#testGeneratorPanel").children();
        // if their are any assignment questions and this was not the previous value auto save them for this assignment
        if (assignmentQuestions.length > 0 && previousValue !== value) {
            console.log("auto save assignment...");
            submitAssignmentQuestions(previousValue);
        }

        abp.ui.setBusy($("#testGeneratorDiv"), getAssignment(value).done(function(response) {
            $testGeneratorDiv.empty().html(response);
            initDragula();

        }));
        $(this).data("prev", value);
    });

        $assignmentSelectList.data("prev", $assignmentSelectList.val());
        // trigger a change for the subject filter to go ahead and grab a category for the default selected subject
        $subjectFilterList.trigger('change');
        // trigger a change for the assignment filter to go ahead and grab an assignment for the test generator
        $assignmentSelectList.trigger('change');

});