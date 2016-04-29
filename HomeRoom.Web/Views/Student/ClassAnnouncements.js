
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
                            return "";
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return "";
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