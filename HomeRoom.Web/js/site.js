// Form Methods
// loadForm - loads a form into the specified modal 
// resourceUrl - where the get the from from
// title - the title of the form
// $modal - the modal selector (jQuery object) 
var loadForm = function (resourceUrl, title, $modal) {
    $.get(resourceUrl, function (data) {
        $modal.find('.modal-title').text(title);
        $modal.find('.modal-footer').find(':button').show();
        $modal.find('.modal-body').empty().html(data);
        $modal.modal('show');
    });
}