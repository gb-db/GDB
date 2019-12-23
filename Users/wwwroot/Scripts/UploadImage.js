$(document).ready(function () {
        //@ sourceURL=~/Scripts/UploadImage.js

    $(".uploadingUser").autocomplete({
        //appendTo: '#aaa',
        source: function (request, response) {
            $.ajax({
                type: "GET",
                url: "/News/SearechUsers/?term=" + request.term,
                contentType: "application/json ; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    response($.map(msg, function (item) {
                        return {
                            "label": item.label,
                            "value": item.value
                        }
                    }))
                },
                error: function (msg) {
                    //
                }
            });
        },
        select: function (event, ui) {
            var label_ = ui.item.label;
            var val_ = ui.item.value;
            ui.item.value = label_;
            $("#User_Id").attr("value", val_);
        },
        minLength: 1
    });
});