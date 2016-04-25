
// saveAssignmentType - saves the form for an assignmentType
// formSeraialize - the form data serialized
// resourceUrl - where to send this request 
var saveAssignmentType = function (formSerialize, resourceUrl) {
    return abp.ajax({
        url: resourceUrl,
        data: JSON.stringify(formSerialize)
    });
}

// removeStudent - removes the student from a class
// resourceUrl - where to send the request to remove the student. Include the classId as query parameters
var removeAnnouncement = function (resourceUrl) {
    return abp.ajax({
        url: resourceUrl
    });
}

$(function () {

    var classId = $("#ClassId").val();
    // datatable default options
    tableOptions.defaults();

    var $announcementsTable = $("#classAnnouncementsTable");
    var dataTableUrl = $announcementsTable.data("datatableurl");
    var $announcementsModal = $("#classAnnouncementsModal");
    var announcementsDataTable = null;

    // bind events
    // saving an announcement
    $("#postAnnouncementSaveBtn").on('click', function (e) {
        e.preventDefault();

        var resourceUrl = $("#announcementForm").attr("action");
        var enrollmentObj = $("#announcementForm").serializeFormToObject();


        abp.ui.setBusy($announcementsModal, saveAssignmentType(enrollmentObj, resourceUrl).done(function (response) {
            if (!response.error) {
                abp.notify.success(response.msg, "");

                abp.ui.setBusy($announcementsTable);
                announcementsDataTable.ajax.reload();
                abp.ui.clearBusy($announcementsTable);
                $announcementsModal.modal('hide');
            }
        }));
    });

    // Editing an assignment type
    $announcementsTable.on('click', '.fa-pencil-square-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = announcementsDataTable.row(clickedRow).data();

        var query = {
            classId: classId,
            id: rowData.id
        }

        loadForm("Edit Announcement", $announcementsModal, query);

    });

    // Removing an assignment type
    $announcementsTable.on('click', '.fa-trash-o', function (e) {
        var clickedRow = $(this).closest('tr');
        var rowData = announcementsDataTable.row(clickedRow).data();

        abp.message.confirm("Are you sure?", "This will delete the following announcement", function (confirm) {
            if (confirm) {
                var resourceUrl = '/Announcement/DeleteAnnouncement?id=' + rowData.id;
                abp.ui.setBusy($announcementsTable, removeAnnouncement(resourceUrl).done(function (response) {
                    abp.notify.success(response.msg, "");
                    announcementsDataTable.ajax.reload();
                }));
            }
        });



    });

    // adding an assignment type
    $("#createNewAnnouncementBtn").on('click', function (e) {
        e.preventDefault();

        var query = {
            classId: classId
        }
        loadForm("Add Announcement", $announcementsModal, query);
    });



    $('a[href="#dashboard"]').on('shown.bs.tab', function () {
        // check to make sure we only init a datatable once
        if (!$.fn.dataTable.isDataTable("#classAnnouncementsTable")) {

            announcementsDataTable = $announcementsTable.DataTable({
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
                "scrollY": "250px",
                "columns": [
                    { "data": "title", "orderable": true },
                    { "data": "datePosted", "orderable": true },
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

    // trigger a shown.bs.tab for the dashboard table to load
    $('a[href="#dashboard"]').trigger("shown.bs.tab");

});