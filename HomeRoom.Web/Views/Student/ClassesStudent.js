// classes ajax functions
// saveClassForm - saves the form for creating/editing a class
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveClassForm = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}

// deleteClass - request to delete the class from the database
// resourceUrl - where to send the request to delete the class. Include the classId as query parameters
var deleteClass = function (resourceUrl) {
    return abp.ajax({
        url: resourceUrl
    });
}

$(function () {
    // datatable default options
    tableOptions.defaults();

    var $classesTable = $("#studentClassesTable");
    //var $classModal = $("#classesModal");
    var dataTableUrl = $classesTable.data("datatableurl");


    // setup the datatable
    var classesDataTable = $classesTable.DataTable({
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
            { "data": "className", "orderable": true },
            { "data": "subject", "orderable": true },
            { "data": "teacher", "orderable": true },
            {
                data: null,
                orderable: false,
                render: function (data) {
                    return "<a href=" + data.courseUrl +"><span class='glyphicon glyphicon-new-window' style='cursor: pointer;'></span></a>";
                }
            },
        ]
    }).on('preXhr.dt', function (e, settings, data) {

    });


    // editing a class
    $("#classesTable").on('click', '.fa-pencil-square-o', function(e) {
        var clickedRow = $(this).closest('tr');
        var rowData = classesDataTable.row(clickedRow).data();

        var query = {
            classId: rowData.id
        }

        loadForm("Edit Class", $classModal, query);

    });

   
});