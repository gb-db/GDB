$(function () {
    $('#modal______').modal('toggle');
});

//begin create User

$(function () {
    // boostrap 4 load modal example from docs

    $('#modal-container').on('show.bs.modal', function (event) {

        var button = $(event.relatedTarget); // Button that triggered the modal

        var url = button.attr("href");

        var modal = $(this);

        // note that this will replace the content of modal-content everytime the modal is opened
        var v = modal.find('.modal-content');
        modal.find('.modal-content').load(url);

    });

    $('#modal-container').on('hidden.bs.modal', function () {

        // remove the bs.modal data attribute from it

        $(this).removeData('bs.modal');

        // and empty the modal-content element

        $('#modal-container .modal-content').empty();

    });
});

$(document).on("click", function (event) {
    ////if you click on anything except the modal itself or the "open modal" link, close the modal
    //if ($(event.target).closest(".modal,.js-open-modal").length) {
    //    $("body").find(".modal").trigger('close');
    //}
});

$(document).on('click', '.get-create-user', function (e) {
    e.preventDefault();
    var url = '/Admin/create/';
    var modalID = '#createUserModal';

    if ($(modalID).length <= 0) {

        $("#ajax-loader").show();
        $.ajax({
            url: url,
            type: 'GET',
            complete: function () {
                $("#ajax-loader").hide();
            },
            success: function (data) {
                loadCBModal(modalID, data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }

});

$(document).on('submit', '#post-create-user', function (e) {
    e.preventDefault();
    //removeErrorNotifications();

    var modalID = "#createUserModal";
    var modalContent = "#createUserModal-ct";

    //$('.modal-submit-btn').prop('disabled', true);
    $("#ajax-loader").show();
    $.ajax({
        url: this.action,
        type: this.method,
        data: $(this).serialize(),
        complete: function () {
            $('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
        },
        success: function (result) {
            if (result.success) {
                hideModal(modalID);
                showNotificationMessage("success", result.message);
            }
            else {
                $(modalContent).html(result);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });


});

//End create User

//bootstrap carusel begin
$(document).on('shown.bs.modal', '#ModalCarousel', function () {
    alert("Hello World!");

    var html_ = $("#carousel").html();

    $("#carousel-modal-demo").html(html_);
});

$(document).on('click', '.get-carusel-images', function (e) {
    e.preventDefault();
    var url = '/Home/GetCaruselImages/';
    var modalID = '#getCaruselImagesModal';

    if ($(modalID).length <= 0) {

        $("#ajax-loader").show();
        $.ajax({
            url: url,
            type: 'GET',
            complete: function () {
                $("#ajax-loader").hide();
            },
            success: function (data) {
                loadCBModal(modalID, data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }

});
//bootstrap carusel end

//begin upload news
$(document).on('click', '.get-upload-news', function (e) {
    e.preventDefault();
    var url = '/News/UploadNews/';
    var modalID = '#uploadNewsModal';

    if ($(modalID).length <= 0) {

        $("#ajax-loader").show();
        $.ajax({
            url: url,
            type: 'GET',
            complete: function () {
                $("#ajax-loader").hide();
            },
            success: function (data) {
                loadCBModal(modalID, data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }

});

$(document).on('submit', '#post-upload-news', function (e) {
    e.preventDefault();
    //removeErrorNotifications();

    var modalID = "#uploadNewsModal";
    var modalContent = "#uploadNewsModal-ct";

    //$('.modal-submit-btn').prop('disabled', true);
    //var ser = $(this).serialize();
    var fd = new FormData($('form')[0]);
    var aa = "AAAAAAAAAAAAA  DDDDDDDDD  VVVVVVVVVVVVVV";
    var obj = new Object();
    obj.files = fd;
    obj.abo = aa;

    $("#ajax-loader").show();
    $.ajax({
        url: this.action,
        type: this.method,
        data: fd,
        complete: function () {
            $('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
        },

        cache: false,
        contentType: false,
        processData: false,

        success: function (result) {
            if (result.success) {
                //hideModal(modalID);
                $("#uploadLabel").text(result.message);
                showNotificationMessage("success", result.message);
            }
            else {
                $(modalContent).html(result);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });


});

//$(':file').on('change', function () {
$(document).on('change', '.upload-file-input', function () {
    var file = this.files;
    var res = '';
    $.each(file, function (index, value) {
        res += '-  ' + value.name + ',\n ';
    });
    $('.file-upload-filenames').text(res);
    if (file.size > 1024) {
        alert('max upload size is 1k');
    }
});
//End upload news

// Autocomplete    begin

// Autocomplete    End