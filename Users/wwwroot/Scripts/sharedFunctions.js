function showNotificationMessage(messageType, message) {
    var htmlMessage;
    $('#notification-placeholder').html('');

    if (messageType.toLowerCase() == "success") {
        htmlMessage = '<div class="alert alert-success alert-dismissible action-message" role="alert">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            '<span class="glyphicon glyphicon-ok-sign"></span> <strong>Success</strong> ' + message + '</div>';
    }
    else if (messageType.toLowerCase() == "error") {

        htmlMessage = '<div class="alert alert-danger alert-dismissible action-message" role="alert">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            '<span class="glyphicon glyphicon-remove-sign"></span> <strong>Error</strong> ' + message + '</div>';
    }
    else if (messageType.toLowerCase() == "warning") {
        htmlMessage = '<div class="alert alert-warning alert-dismissible action-message" role="alert">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            '<span class="glyphicon glyphicon-exclamation-sign"></span> <strong>Warning</strong> ' + message + '</div>';
    }

    if (htmlMessage != null && htmlMessage.length > 0) {

        $('#notification-placeholder').html(htmlMessage);
        $('.action-message').fadeOut(7000, function () {
            $(this).remove();
        });
    }
}

function textInputFocus(element) {
    var input = $(element).find('input:text:visible:first');

    if (input.length) {
        //input.focusTextToEnd();
    }

}

function hideAjaxLoader() {
    $("#ajax-loader").hide();
}

function checkAjaxError(xhr, modalID) {
    $("#ajax-loader").hide();

    if (modalID != null && modalID.length > 0) {
        $(modalID).remove();
    }

    if (xhr.status == 401) {
        var baseUrl = $('base').attr('href');
        var url = baseUrl + 'account/login';
        sessionStorage.TimeOut = true;
        window.location = '/account/login';
    }
    else if (xhr.status == 403) {
        getErrorModal(xhr.statusText);
    }
    else if (xhr.status == 404) {
        getErrorModal("Resource was not found.");
    }
    else {
        getErrorModal();
        console.log("error text: " + xhr.responseText);
    }
}

function getErrorModal(errorMessage) {

    hideAjaxLoader();

    var message = "An error occurred while processing your request.";
    if (errorMessage != null && errorMessage.length > 0) { message = errorMessage; }
    var modalID = "#errorModal";
    var modalContent = "#errorModal-ct";
    var modalSizeClass = "modal-dialog";

    var modalHtml = '<div id="' + modalID.replace('#', '') + '" tabindex="-1" class="modal fade in"><div class="' + modalSizeClass + '"><div class="modal-content"><div id="' + modalContent.replace('#', '') + '"></div></div></div></div>';

    $(modalID).remove();
    addModalHtml(modalHtml);

    $(modalContent).html('<div class="modal-header">' +
        '<h4 class="modal-title" style="margin:auto;">Error</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
        '</div>' +
        '<div class="modal-body">' +
        '<div class="alert alert-danger">' +
        '<h3>' + message + '</h3>' +
        '</div></div>' +
        '<div class="modal-footer"><button class="btn btn-default btn-sm" data-dismiss="modal">Close</button></div>');

    $(modalID).on('hidden.bs.modal', function () {
        $(this).remove();
    });

    $(modalID).modal({
        keyboard: false,
        backdrop: 'static'
    }, 'show');
}

function hideModal(modalID) {
    if (modalID != null) {
        $(modalID).modal("hide");
    }
}

function addModalHtml(modalHtml) {
    if ($('#cb-modal-container').length) {
        $('#cb-modal-container').append(modalHtml);
    }
    else {
        $('body').append('<div id="cb-modal-container"></div>');
        $('#cb-modal-container').append(modalHtml);
    }
}

function loadCBModal(modalID, htmlContent, modalSize) {

    if (modalSize == null) {
        modalSize = "modal-lg";
    }
    var modalContent = modalID + "-ct";
    var modalSizeClass = "modal-dialog " + modalSize;

    var modalHtml = '<div id="' + modalID.replace('#', '') + '" tabindex="-1" class="modal fade in" data-backdrop="static"><div class="' + modalSizeClass + '"><div class="modal-content"><div id="' + modalContent.replace('#', '') + '"></div></div></div></div>';

    addModalHtml(modalHtml);
    $(modalContent).html(htmlContent);

    $(modalID).on('shown.bs.modal', function () {
        textInputFocus(this);
    });

    $(modalID).on('hidden.bs.modal', function () {
        $(this).remove();
    });

    $(modalID).modal({
        keyboard: false,
        backdrop: 'static'
    }, 'show');

}




// Pagination --- begin ---
$(document).on('click', ".btn-pagination", function () {
    //var varr = $(this);
    $('.hidd-btn-pagination').val($(this).val());
});

$(document).on('submit', '#post-pagination', function (e) {
    e.preventDefault();
    //$('.hidd-btn-pagination').val

    $.ajax({
        url: "/News/GetMyViewComponent",
        type: this.method,
        data: $(this).serialize(),
        complete: function () {
            $('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
        },
        success: function (result) {
            $("#pagination-id").html(result);
            //if (result.success) {
            //    //hideModal(modalID);
            //    //showNotificationMessage("success", result.message);
            //}
            //else {
            //    //
            //}
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });


});
// Pagination --- end ---