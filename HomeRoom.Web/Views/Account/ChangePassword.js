(function () {
    // requests to change a password
    // data - the form data to submit
    // resourceUrl - where to submit the form
    function changePasswordForm(data, resourceUrl) {
        return abp.ajax({
            url: resourceUrl,
            data: JSON.stringify(data)
        });
    }

    $(function () {
        $("#changePasswordBtn").on('click', function (e) {
            e.preventDefault();

            debugger;
            var $passwordForm = $("#changePasswordForm");
            var $passwordModal = $("#changePasswordModal");
            var formData = $passwordForm.serializeFormToObject();
            var resourceUrl = $passwordForm.attr("action");

            abp.ui.setBusy($passwordModal, changePasswordForm(formData, resourceUrl).done(function (response) {
                console.log(response);
                // no error, password changed
                if (!response.error) {
                    abp.notify.success(response.msg, "");

                    $passwordModal.modal('hide');
                }
                else {
                    abp.notify.error(response.msg, "Error");
                }
            }));
        });
    });
})();