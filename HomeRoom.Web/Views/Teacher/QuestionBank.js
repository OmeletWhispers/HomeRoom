// saveAssignmentType - saves the form for an assignmentType
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveQuestion = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}

// generateHiddenAnswerInput - generates the hidden input form the answer choices
var generateHiddenAnswerInput = function(value, isCorrect, index) {
    var choiceHtml = abp.utils.formatString('<input type="hidden" value="{0}" name="question.answerChoices[{1}].answer"/>', value, index);
    var isCorrectHtml = abp.utils.formatString('<input type="hidden" value="{0}" name="question.answerChoices[{1}].isCorrect"/>', isCorrect, index);

    return abp.utils.formatString("{0}{1}", choiceHtml, isCorrectHtml);
}

// generateIsCorrectCheckbox - generates the checkbox to show that the answer choice is correct or not
var generateIsCorrectCheckbox = function(isCorrect) {
    if (isCorrect === "true") {
        return "<input type='checkbox' checked='checked' disabled='disabled'/>";
    }

    return "<input type='checkbox' disabled='disabled'/>";
}

// eventQuestionTypeChange - event callback function for when the question type has been changed
// event - the event that is triggered
var eventQuestionTypeChange = function(event) {
    var $questionType = $(this);
    var questionType = parseInt($questionType.val());
    var $answerChoiceLabel = $("#answerChoiceLabel");
    var $answerChoices = $("#mutlipleAnswerChoiceDiv");
    var $isCorrect = $("#answerIsCorrectDiv");

    // selected short answer
    if (questionType === 0) {
        // change the label text
        $answerChoiceLabel.text("Answer ");
        // hide the is correct checkbox and the aswer choices
        $answerChoices.hide();
        $isCorrect.hide();
    }
    else {
        // change the label text
        $answerChoiceLabel.text("Answer Choice");
        // show the is correct checkbox and the aswer choices
        $answerChoices.show();
        $isCorrect.show();
    }
}

$(function () {

    // datatable default options
    tableOptions.defaults();

    var $questionsTable = $("#questionsTable");
    var dataTableUrl = $questionsTable.data("datatableurl");
    var $questionModal = $("#questionModal");
    var questionsDataTable = null;
    var totalAnswerChoices = 0;

    // bind events
    // changing the questiontype
    $(document).on('change', '#questionType', eventQuestionTypeChange);

    $(document).on('change', '#isPublicCheckBox', function(e) {
        var value = this.checked === true ? "true" : "false";

        $(this).val(value);
    });

    // adding an answer choice to this question
    $(document).on('click', '#addAnswerChoice', function(e) {
        e.preventDefault();
        var $answerValue = $("#questionAnswerValue");
        var $answerChoiceDiv = $("#answerChoicesDiv");
        var value = $answerValue.val();
        var isCorrectElm = document.getElementById("Answer_IsCorrect");
        var isCorrect = isCorrectElm.checked ? 'true' : 'false';

        if (value === "") {
            abp.notify.error("The answer choice can not be blank", "");
            return;
        }

        var valueString = abp.utils.formatString("{0}&nbsp;{1}<br/>", value, generateIsCorrectCheckbox(isCorrect));
        $answerChoiceDiv.append(valueString);
        $answerChoiceDiv.append(generateHiddenAnswerInput(value, isCorrect, totalAnswerChoices));
        $answerChoiceDiv.show();

        // clear out the answer value
        $answerValue.val("");
        $('#Answer_IsCorrect').prop('checked', false);
        totalAnswerChoices++;
    });

    // saving a question
    $("#createQuestion").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#questionForm").attr("action");
        var categoryObj = $("#questionForm").serializeFormToObject();

        $("#answerChoicesDiv").empty();

        abp.ui.setBusy($questionModal, saveQuestion(categoryObj, resourceUrl).done(function (response) {
            if (!response.error) {
                totalAnswerChoices = 0;
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($questionsTable);
                questionsDataTable.ajax.reload();
                abp.ui.clearBusy($questionsTable);
                $questionModal.modal('hide');
            }
        }));
    });

    // Editing an assignment type
    $questionsTable.on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = questionsDataTable.row(clickedRow).data();

        var query = {
            id: rowData.id
        }

        loadForm("Edit Category", $questionModal, query);

    });

    // adding an assignment type
    $("#addQuestion").on('click', function (e) {
        e.preventDefault();

        loadForm("Add Question", $questionModal);
    });

    // when the form initially shows
    $("#questionModal").on('show.bs.modal', function() {
        // trigger an event change for the default value for question type
        $("#questionType").trigger('change');
        //reset the total number of answer choices
        totalAnswerChoices = 0;
    });

    $('a[href="#questionBank"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#questionsTable")) {

            questionsDataTable = $questionsTable.DataTable({
                "ajax": function (data, callback, settings) {
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
                    { "data": "question", "orderable": true },
                    { "data": "category", "orderable": true },
                    { "data": "questionType", "orderable": true },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            if (data.isPublic) {
                                return "<input disabled='disabled' type='checkbox' value='true' checked='checked' />";
                            } else {
                                return "<input disabled='disabled' type='checkbox' value='false'/>";
                            }
                        }
                    }
                ]
            }).on('preXhr.dt', function (e, settings, data) {
            });
        }
    });

});