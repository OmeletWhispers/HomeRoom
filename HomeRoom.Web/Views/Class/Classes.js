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

    var $classesTable = $("#classesTable");
    var $classModal = $("#classesModal");
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
            { "data": "students", "orderable": true },
            {
                data: null,
                orderable: false,
                render: function(data) {
                    return "<i class='fa fa-pencil-square-o' style='cursor: pointer;'></i>";
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data) {
                    return "<span class='glyphicon glyphicon-new-window' style='cursor: pointer;'></span>";
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

    });


    // want to create a class
    $("#createBtn").on('click', function (e) {
        e.preventDefault();

        loadForm("New Class", $classModal);
    });


    // class form submit
    $("#createClass").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#classForm").attr("action");
        var classObj = $("#classForm").serializeFormToObject();

        abp.ui.setBusy($classModal, saveClassForm(classObj, resourceUrl).done(function (response) {
            abp.notify.success(response.msg);

            // set the classes table to busy to show that we are relading the data
            abp.ui.setBusy($classesTable);
            classesDataTable.ajax.reload();
            abp.ui.clearBusy($classesTable);
            
            $classModal.modal('hide');
        }));
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

    // delete a class
    $("#classesTable").on('click', '.fa-trash-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = classesDataTable.row(clickedRow).data();
        var classId = rowData.id;
        var className = rowData.className;
        var resourceUrl = 'Class/DeleteClass?classId=' + classId;


        abp.message.confirm(className + " will be deleted", "Are you sure?", function (confirm) {
            if (confirm) {
                // send request to delete user
                abp.ui.setBusy($classesTable, deleteClass(resourceUrl).done(function (response) {
                    abp.notify.success(response.msg);
                    classesDataTable.ajax.reload();
                }));
            }
        });
    });
});