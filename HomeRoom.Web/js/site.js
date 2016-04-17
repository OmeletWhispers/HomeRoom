// Form Methods
// loadForm - loads a form into the specified modal 
// title - the title of the form
// $modal - the modal selector (jQuery object)
// query - the query variales to add to the request for the form
var loadForm = function (title, $modal, query) {
    var resourceUrl = $modal.data('resourceurl');

    $.get(resourceUrl, query, function (data) {
        $modal.find('.modal-title').text(title);
        $modal.find('.modal-footer').find(':button').show();
        $modal.find('.modal-body').empty().html(data);
        $modal.modal('show');
    });
}