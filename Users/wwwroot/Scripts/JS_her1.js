
function her(id, user_id, parent_level, parent_number, level, number, last_child,
    her_src, her_fn, her_ln, her_birthDate, her_endDate, info_data, alb, alb_big,
    heratigeUser_Id, pattern, flagImgPath, flagImgPathBP, countryName, countryNameBP, isOpend, isEditable) {
    this.id = id;
    this.user_id = user_id;
    this.parent_level = parent_level;
    this.parent_number = parent_number;
    this.level = level;
    this.number = number;
    this.last_child = last_child;
    this.her_src = her_src;
    this.her_fn = her_fn;
    this.her_ln = her_ln;
    this.her_birthDate = her_birthDate;
    this.her_endDate = her_endDate;
    this.info_data = info_data;
    this.alb = alb;
    this.alb_big = alb_big;
    this.heratigeUser_Id = heratigeUser_Id;
    this.pattern = pattern;
    this.flagImgPath = flagImgPath;
    this.flagImgPathBP = flagImgPathBP;
    this.countryName = countryName;
    this.countryNameBP = countryNameBP;
    this.isOpend = isOpend;
    this.isEditable = isEditable;
}

var img_desc = {
    path: "",
    description: ""
}

var Heritage = new Object();
Heritage.hers = [];

function InfoItem(id, type, comment, width, height, path) {
    this.id = id;
    this.type = type;
    this.comment = comment;
    this.width = width;
    this.height = height;
    this.path = path;
}

var InfoObject = new Object();
InfoObject.info_items = []

$(function () {
    $('#mainDiv').parent().parent().addClass('forHerBody');


    $('body').on("click", ".onClickAddClass, #ButtonNew", addNewHeritage);

    $('body').on("click", ".onClickDeleteClass", deleteHeritageLineFrom);

    $('body').on("click", ".rules-group-container", function (event) {
        var checkArr = []; 
        checkArr.push($(event.target).hasClass('fa-edit'));
        checkArr.push($(event.target).hasClass('get-her-upload-image'));
        checkArr.push($(event.target).hasClass('fa-save'));
        checkArr.push($(event.target).hasClass('fa-image_i'));
        checkArr.push($(event.target).hasClass('fa-trash'));
        checkArr.push($(event.target).hasClass('forEscape'));
        var chk = $.inArray(true, checkArr);

        if (chk >= 0) {
            return;
        }


        event.stopImmediatePropagation();
        var div_ = event.currentTarget;
        $(div_).toggleClass("backgrpound_div");

    });

});

//begin edit heritage
$(function () {
    $(document).on('click', '.get-edit-heritage', getEditHeritage);
    $(document).on('submit', '#get-edit-heritage', postEditHeritage);
    $(document).on('change', '.upload-file-input', uploadFileInput);


    $(document).on('click', '.get-her-upload-image , .forEscape', getHerUploadImage);
    $(document).on('submit', '#post-her-upload-image', postHerUploadImage);

    $(document).on('click', '.save_photo', savePhoto);


    $(document).on('click', '.get-her-data-image', getHerData_Image);
    $(document).on('submit', '#post-her-comment-image', postHerImage);


    $(document).on('click', '.comment-main', comment_main);
    $(document).on('click', '.cloud-main', cloud_main);

    $(document).on('click', '.fa_save_image', fa_save_image);
    // $(document).on('click', '.fa_save_comment', fa_save_comment);
    //$(document).on('click', '.fa_save_pencil', fa_save_pencil);   new-record-insert


    //the mains
    $(document).on('click', '.theMainClass', the_Main_New);
    $(document).on('click', '.searchHreitage', get_Hreitage_ById);

    //Permissions
    $(document).on('click', '.set-permission', get_setHerPermission);
    //$(document).on('submit', '#set-her-permission', post_setHerPermission);
});

//New Heritage   --- begin -----
function the_Main_New(e) {
    e.preventDefault();
    theMain();
}

function theMain() {
    var uId = parseInt($("#user_id").val(), 10);
    if (isNaN(uId)) {
        alert("Only Registered Users Can Create New Heritage Line.");
        return false;
    }

    addMainAncestor();
}

function addMainAncestor() {
    var herLocal = null;

    //userId_heratigeUser_Id_parentLevel_parentNumber_level_number
    var id_ = "" + $("#user_id").val() + "_" + $("#heratigeUser_Id").val() + "_0_0_0_0";
    var patt = "" + $("#user_id").val() + "_0_0_0_0";
    herLocal = new her(id_, $("#user_id").val(), 0, 0, 0, 0, -1, "/Images/Heritage/empty.png", "", "", "", "", null, "alb_" + id_, "alb_big_" + id_, $("#heratigeUser_Id").val(), patt, "", "", "", "", false, false);

    addPersonData(herLocal);
}

function addPersonData(herLocal) {
    $("#heritage").tmpl(herLocal).appendTo($("#mainAncestor"));
    $("#mainAncestor > .card").data("information", JSON.stringify(herLocal));
}

function addNewHeritage(event) {
    var herV = new her("", "", 0, 0, 0, 0, -1, "/Images/Heritage/empty.png", "", "", "", "", null, "", "", 0, "", "", "", "", "", false, false);
    var currDiv = $(event.currentTarget).closest('.card');
    herV.parent_level = currDiv.attr('level');
    herV.parent_number = currDiv.attr('number');
    herV.level = parseInt(currDiv.attr('level')) + 1;

    var collection = $(event.currentTarget).closest('#dl_logical_group_0').find(".card:not([parent_level='0'][parent_number='0'][level='0'][number='0'])");
    var collection1 = collection.filter(function () {
        return $(this).attr("parent_level") == currDiv.attr('level') && $(this).attr("parent_number") == currDiv.attr('number');
    });
    if (collection1.length == 0) {
        herV.number = 0;
    }
    else {
        var lastNumber = parseInt($(collection1).last().attr("number")) + 1;
        herV.number = lastNumber;
    }

    //herV.flagImgPath = "";
    //herV.flagImgPathGP = "";
    //herV.countryName = "";
    //herV.countryNameBP = "";
    herV.user_id = $("#user_id").val();
    herV.heratigeUser_Id = $("#heratigeUser_Id").val();
    herV.id = "" + $("#user_id").val() + "_" + $("#heratigeUser_Id").val() + "_" + herV.parent_level + "_" + herV.parent_number + "_" + herV.level + "_" + herV.number;
    herV.alb = "alb_" + herV.id;
    herV.alb_big = "alb_big_" + herV.id;
    herV.pattern = currDiv.attr('pattern') + "_" + herV.parent_level + "_" + herV.parent_number + "_" + herV.level + "_" + herV.number;
    addNew(event, herV);
}

function addNew(event, herV) {
    event.stopImmediatePropagation();

    var strGrp = "<dl class='rules-group-container' id='dl_logical_group_' style='margin-left:130px;'>" +
        "<dt class='rules-group-header'>" +
        "</dt>" +
        "<dd class='rules-group-body' style='margin-bottom: 0px;'>" +
        "<ul class='rules-list' ></ul>" +
        "</dd>" +
        "</dl>";

    var ul_ = $(event.target).closest('dl').children('dd').children('ul:first');
    var strG = $(strGrp);
    ul_.append(strG);


    var li__ = ul_.children('dl:last').children('dd').find("ul").append("<li id='li_logical_rule_' class='rule-container' style='height:68px;' ></li>").find("li");
    $("#heritage").tmpl(herV).appendTo(li__[0]);




    //ul_.find('#' + herV.id).data("data-information", JSON.stringify(herV));
    ul_.find('#' + herV.id).data("information", JSON.stringify(herV));

    $('div[data-toggle="tooltip"]').tooltip({
        animated: 'fade',
        placement: 'bottom',
        html: true
    });

    //$('.popo[data-toggle="popover-hover"]').popover({ trigger: 'hover'});
    $('.popo[data-toggle="popover"]').popover({
        html: true
    }).on('click', function (e) {
        e.stopImmediatePropagation();

        $(".popo").attr('data-content', '<div style="background-color:aquamarine;width:400px;height:100px;"><div>This is your div content</div></div >');
        // $(".popo").attr('data-content', '<div><b>Example popover</b> - content</div>');
    })





    //$('.popo[data-toggle="popover"]').on('show.bs.popover', function (e) {
    //    e.preventDefault(');
    //})

    var a = "   Քոչարյանն արդեն ներկայացավ ԱԱԾ: Քոչարյանին ուղեկցում էր որդին՝ Լևոն Քոչարյանը: \n Վերջինս լրագրողների հետ զրույցում խոսեց հոր կալանավորման մասին:\n Ամբողջական հոդվածը կարող եք կարդալ այս\n հասցեով՝ https://armlur.am/910994/ ";
    //$(".popo").attr('data-content', '<p>' + a + '</p>');
    //$(".popo").attr('data-content', a);


    return false;
};

//New Heritage   --- end -----

// Heritage Editor (Add & Update) ------ Begin ---------
function getEditHeritage(e) {
    e.preventDefault();
    var modalID = '#heritageModal';

    var r = $(e.currentTarget).closest('.card').data('information');
    var clickedCard = $(e.currentTarget).closest('.card');

    var url = '/Heritage/HerEdit?id=' + clickedCard.prop("id");

    if ($(modalID).length <= 0) {

        $("#ajax-loader").show();
        $.ajax({
            url: url,
            type: 'GET',
            async: false,
            complete: function () {
                $("#ajax-loader").hide();
            },
            success: function (data) {
                loadCBModal(modalID, data);
                $('#heritageModal').find('form #data_information').val(r);
                fireDatePicker($('#heritageModal'));

                $('#heritageModal').on('hide.bs.modal', function (e) {
                    updateCardInformation(e, clickedCard);
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }
}

function postEditHeritage(e) {
    e.preventDefault();

    if ($("#countryId").val() == "0") {
        alert("Country must be selected!");

        return false;
    }

    //var card_ = $(e.currentTarget).closest('.card');
    var dataInfoDiv = $("#get-edit-heritage input[id='data_information']");

    var modalID = "#heritageModal";
    var modalContent = "#heritageModal-ct";
    var ser = $("#get-edit-heritage").serialize();

    $("#ajax-loader").show();
    $.ajax({
        url: this.action,
        type: this.method,
        data: ser,
        //contentType: "application/json",
        async: false,
        complete: function () {
            $('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
        },


        success: function (result) {
            if (result.success) {
                $("#heratigeUser_Id").val(result.heratigeUser_Id);
                $("#herEditLabel").text(result.message);

                var arr = $("#mainAncestor > .card").prop("id").split("_");
                var newIds = "";
                if (arr != null && arr.length == 6) {
                    for (var v = 0; v < arr.length; v++) {
                        if (v == 1) {
                            newIds += (result.heratigeUser_Id).toString() + "_";
                        }
                        else {
                            newIds += v != 5 ? arr[v] + "_" : arr[v];
                        }
                    }
                }
                $("#mainAncestor > .card").prop("id", newIds);
                var herLocal = JSON.parse($("#mainAncestor > .card").data("information"));
                herLocal.id = newIds;
                $("#mainAncestor > .card").data("information", JSON.stringify(herLocal));

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
}

// Heritage Editor (Add & Update) ------ End ---------

function deleteHeritageLineFrom(event) {
    event.preventDefault();

    var names = $(event.currentTarget).parent().parent().parent().find("div.theNames").text();
    if (confirm("You are going to delete " + names + " with descedant(s)!\n This action is unrecoverable")) {
        txt = "You pressed OK!";
    } else {
        txt = "You pressed Cancel!";

        return false;
    }

    var divClass = $(event.currentTarget).closest(".card");

    var deleteData = {
        user_id: $("#user_id").val(),
        heratigeUser_Id: $("#heratigeUser_Id").val(),
        parent_level: divClass.attr("parent_level"),
        parent_number: divClass.attr("parent_number"),
        level: divClass.attr("level"),
        number: divClass.attr("number"),
        pattern: divClass.attr("pattern"),
    };

    $("#ajax-loader").show();
    $.ajax({
        url: "/api/HeritageApi/DeleteHeritageLineFrom",
        type: "POST",
        data: JSON.stringify(deleteData),
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        complete: function () {
            $("#ajax-loader").hide();
        },
        cache: false,
        //contentType: false,
        processData: false,

        success: function (result) {
            if (result.success) {

                showNotificationMessage("success", result.message);
            }
            else {
                $("#uploadLabel").text("There was problem to Delete heritage!");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
}

// Upload Photo --- begin -----
function getHerUploadImage(e) {
    e.preventDefault();
    if ($("#heratigeUser_Id").val() === "0" || $("#user_id").val() === "") {
        alert("Register Heritage before uploading photo.");
        return false;
    }

    //var card_ = $(e.currentTarget).closest(".card").data('data-information');
    var card_ = $(e.currentTarget).closest(".card").data('information');
    var info = JSON.parse(card_);


    var url = '/Heritage/HerUploadImage/';
    var modalID = '#herUploadImageModal';

    if ($(modalID).length > 0) {
        $(modalID).remove();
    }

    //if ($(modalID).length <= 0) {

        $("#ajax-loader").show();
        $.ajax({
            url: url,
            type: 'GET',
            complete: function () {
                $("#ajax-loader").hide();
            },
            success: function (data) {
                loadCBModal(modalID, data);
                $("#herUploadImageModal .id_alb").val(info.alb);
                $("#herUploadImageModal .id_alb_big").val(info.alb_big);
                $("#herUploadImageModal #heritageIds").val(info.id);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    //}
}

function postHerUploadImage(e) {
    e.preventDefault();
    //classes for uniqueness of photos
    var alb = "." + $(e.currentTarget).children('.id_alb').val();
    var alb_big = "." + $(e.currentTarget).children('.id_alb_big').val();

    var modalID = "#herUploadImageModal";
    var modalContent = "#herUploadImageModal-ct";
    var fd = new FormData($('form[id="post-her-upload-image"]')[0]);

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
                insertPhoto(result, alb, alb_big);

                showNotificationMessage("success", result.message);
                $('div[data-toggle="tooltip"]').tooltip({
                    animated: 'fade',
                    placement: 'bottom',
                    html: true
                });
            }
            else {
                $(modalContent).html(result);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
}

function insertPhoto(result, alb, alb_big) {
    $("#uploadLabel").text(result.message);
    $(alb + " .get-her-upload-image").prop("src", result.path);
    $(alb).width(result.width).height(result.height);
    $(alb).css("maxWidth", result.width);
    $(alb_big).prop("data-original-title", "");
    $(alb_big).attr("data-original-title", "");
    $(alb_big).prop("title", "");
    $(alb_big).attr("data-original-title", "<img src='" + result.path + "' style='width:" + result.width * 5 + "px !important;height:" + result.height * 5 + "px; !important' />");

    var mObj = $.parseJSON($(alb_big).closest('.card').data('information'));
    mObj.her_src = result.path;
    ($(alb_big).closest('.card')).data("information", JSON.stringify(mObj));
}

function savePhoto(e) {
    e.preventDefault();
    var url = '/api/HeritageApi/RemovePhoto';

    var modalID = "#heritageModal";
    var modalContent = "#heritageModal-ct";
    var cardData = $(e.currentTarget).closest('.card').data('information');
    Heritage.hers[0] = JSON.parse(cardData);

    $("#ajax-loader").show();
    $.ajax({
        url: url,
        type: "post",
        data: JSON.stringify(Heritage),
        contentType: "application/json",
        Accept: "application / json",
        complete: function () {
            $('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
        },

        //cache: false,
        //contentType: false,
        //processData: false,

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
}
// Upload Photo --- end -----

//Begin edit heritage
function getHerData_Image(e) {
    e.preventDefault();
    if ($("#heratigeUser_Id").val() === "0" || $("#user_id").val() === "") {
        alert("Register Heritage before uploading Image.");
        return false;
    }

    //var card_ = $(e.currentTarget).closest(".card").data('data-information');
    var card_ = $(e.currentTarget).closest(".card").data('information');
    var info = JSON.parse(card_);
    var card_id = $(e.currentTarget).closest(".card").prop("id");


    var url = '/Heritage/GetHerData_Image?card_id=' + card_id;
    var modalID = '#herData_ImageeModal';

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
                $("#post-her-upload-image-data .fa-pencil").on("click", toggleEditDiv);
                $(document).on('click', '.fa_save_pencil', fa_save_pencil);
                $(document).on('click', '.fa_save_comment', fa_save_comment);
                $(document).on('click', '.fa_save_image', fa_save_image);
                //$("#herUploadImageModal .id_alb_big").val(info.alb_big);
                $("#herData_ImageeModal #heritageIds").val(info.id);
                var listInfo = $("#heritageIds").data('image_comment_list');

                for (var v = 0; v < listInfo.length; v++) {
                    var str = "";
                    if (listInfo[v].type == "image") {
                        str = getDivForImage(listInfo[v].width, listInfo[v].height, listInfo[v].path, listInfo[v].order_number, listInfo[v].card_ids);
                    }
                    else if (listInfo[v].type == "comment") {
                        str = getDivForComment(listInfo[v].comment, listInfo[v].order_number, listInfo[v].card_ids);
                    }

                    $(str).insertBefore(".abo");

                    if (listInfo[v].type == "image") {
                        $(document).on("click", ".image_to_trash", image_to_trash);
                    }
                    else if (listInfo[v].type == "comment") {
                        $(document).on("click", ".comment_to_trash", comment_to_trash);
                    }
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }
}

function postHerImage(e) {
    e.preventDefault();
    $("#uploadLabel").text("");

    var alb = "";
    var alb_big = "";
    var modalID = "#postHerImageModal";
    var modalContent = "#postHerImageModal-ct";

    var fd = new FormData($('form[id="post-her-comment-image"]')[0]);

    $("#ajax-loader").show();
    $.ajax({
        url: this.action,
        type: this.method,
        data: fd,
        complete: function () {
            //$('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
            // $(".albert").width(35).height(45);
        },


        cache: false,
        contentType: false,
        processData: false,

        success: function (result) {
            if (result.success) {
                //hideModal(modalID);

                insertCommentImage(result, alb, alb_big);

                $(".cloud-main").trigger("click");
                $("#uploadLabel").text("Image was saved!");
                showNotificationMessage("success", result.message);
                //$('div[data-toggle="tooltip"]').tooltip({
                //    animated: 'fade',
                //    placement: 'bottom',
                //    html: true
                //});
            }
            else {
                $(modalContent).html(result);
                $("#uploadLabel").text("There was a problem to save the file!");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
}

function insertCommentImage(result, alb, alb_big) {
    $("#uploadLabel").text(result.message);
    var str1 = getDivForImage(result.width, result.height, result.path, result.order_number, result.card_ids);

    $(str1).insertBefore("#herData_ImageeModal-ct .inserted___ .abo");
    $(document).on("click", ".image_to_trash", image_to_trash);

    if (result.img_comment != "" && result.img_comment != null) {
        var str2 = '<div class="row pl-2 pr-2 inserted" style="width:765px; margin:auto; position:relative;border-color:rgb(169, 169, 169);border-width:1px;border-style: solid;margin-top: 5px;margin-bottom: 5px;flex-direction: column;">' +
            '<div class="com1 com_15_33_750" style="left:750px !Important;top:0% !Important;"><i class="fa fa-pencil pointer fa_save_pencil"></i></div>' +
            '<div class="com1 com_50_33_700 fa_pencil fa_pencil_toggle" style="left:700px !important;top:0% !Important;">' + '<i class="fa fa-trash fa-2x pointer" ></i ></div>' +
            '<p>' + result.img_comment + '</p>' +
            '</div> ';

        $(str2).insertBefore("#herData_ImageeModal-ct .inserted___ .abo");

    }
}

function getDivForImage(width, height, path, order_number, card_ids) {
    var str = '<div class="row pl-2 pr-2 inserted" style="margin:auto;margin-bottom:10px; background-color:antiquewhite;position:relative;display:table-cell; vertical-align:middle;flex-direction: column !important; padding-left: 0px !important; padding-right: 0px !important;"  order_number=' + order_number + ' card_ids=' + card_ids + ' >' +

        '<div class="" style="width:' + width + 'px; height:' + height + 'px; margin: auto; position: relative; background-color: blueviolet;" >' +
        '<img src="' + path + '" style="width:100%; height:100%;" alt=" Upload Img" class="center" />' +
        '</div> ' +
        '<div class="com1 com_15_33_750"><i class="fa fa-pencil pointer  fa_save_pencil"></i></div>' +
        '<div id = "a" class="com1 com_50_33_700 fa_pencil fa_pencil_toggle">' + '<i class="fa fa-trash fa-2x image_to_trash pointer" ></i ></div>' +
        '</div> ';

    return str;
}

function fa_save_comment(e) {
    e.preventDefault();
    $("#uploadLabel").text("");

    var s_tr = { str: $("#img_comment").val(), heritageIds: $("#heritageIds").val() };
    //$.post("/api/HeritageApi/PostHerData", { textarea: $("#img_comment").val() } , function (result) {
    //    var v = result;
    //});

    $("#ajax-loader").show();
    $.ajax({
        url: "/api/HeritageApi/PostHerData",
        type: "POST",
        data: JSON.stringify(s_tr),
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        complete: function () {
            $("#ajax-loader").hide();
        },
        cache: false,
        //contentType: false,
        processData: false,

        success: function (result) {
            if (result.success) {
                if (result.sendStr != "") {
                    var str1 = getDivForComment(result.sendStr, result.order_number, result.card_ids);
                    $(str1).insertBefore(".abo");

                    $(document).on("click", ".comment_to_trash", comment_to_trash);
                    // var vn =$._data($(".comment_to_trash")[0], "events" );////???????????????????????
                    $("#img_comment").val("");
                    $("#uploadLabel").text("Comment was saved!");
                    $(".comment-main").trigger("click");
                }

                showNotificationMessage("success", result.message);
                //$('div[data-toggle="tooltip"]').tooltip({
                //    animated: 'fade',
                //    placement: 'bottom',
                //    html: true
                //});
            }
            else {
                $("#uploadLabel").text("There was problem to save comment!");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
}

function getDivForComment(sendStr, order_number, card_ids) {
    var str = '<div class="row pl-2 pr-2 inserted" style="width:765px; margin:auto; position:relative;border-color:rgb(169, 169, 169);border-width:1px;border-style: solid;margin-top: 5px;margin-bottom: 5px;flex-direction: column;"  order_number=' + order_number + ' card_ids=' + card_ids + '>' +
        '<div class="com1 com_15_33_750" style="left:750px !Important;top:0% !Important;"><i class="fa fa-pencil pointer fa_save_pencil"></i></div>' +
        '<div class="com1 com_50_33_700 fa_pencil fa_pencil_toggle" style="left:700px !important;top:0% !Important;">' + '<i class="fa fa-trash fa-2x comment_to_trash  pointer" ></i ></div>' +
        sendStr +
        '</div> ';

    return str;
}

function toggleEditDiv(e) {//??????????????????????????????????????????????????????????????????????????????????????????????
    if ($("#post-her-upload-image-data .fa_pencil").is(":visible")) {
        $("#post-her-upload-image-data .fa_pencil").hide();
    }
    else {
        $("#post-her-upload-image-data .fa_pencil").show();
    }
}


function comment_main(e) {
    $("#comment-main-id").find("textarea").val("");
    $("#comment-main-id").toggle();
}
function cloud_main(e) {
    $(".upload-file-input").val("");
    $(".file-upload-filenames").text("");
    $("#cloud-main-id").toggle();
}
function fa_save_pencil(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    $(e.currentTarget).parent().parent().children("div.fa_pencil_toggle").toggle();
}

function fa_save_image(e) {
    e.preventDefault()
    e.stopImmediatePropagation();
    $("#btn_submit").trigger("click");
}

function image_to_trash(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    var card_ids = $(e.currentTarget).closest(".inserted").attr("card_ids");
    var order_number = $(e.currentTarget).closest(".inserted").attr("order_number");
    var retVal = delete_image_comment(card_ids, order_number, $(e.currentTarget).closest(".inserted"));
    if (retVal) {
        $(e.currentTarget).closest(".inserted").remove();
    }
}

function comment_to_trash(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    var card_ids = $(e.currentTarget).closest(".inserted").attr("card_ids");
    var order_number = $(e.currentTarget).closest(".inserted").attr("order_number");
    var retVal = delete_image_comment(card_ids, order_number, $(e.currentTarget).closest(".inserted"));
    if (retVal) {
        $(e.currentTarget).closest(".inserted").remove();
    }
}

function delete_image_comment(card_ids, order_number, divForRemove) {
    $("#uploadLabel").text("");

    var s_tr = { str: order_number, heritageIds: card_ids };
    var retVal = false;

    $("#ajax-loader").show();
    $.ajax({
        url: "/api/HeritageApi/DeleteImageComment",
        type: "POST",
        data: JSON.stringify(s_tr),
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        complete: function () {
            $("#ajax-loader").hide();
        },
        cache: false,
        //contentType: false,
        processData: false,

        success: function (result) {
            if (result.success) {
                $("#uploadLabel").text("Item was deleted!");
                divForRemove.remove();

                showNotificationMessage("success", result.message);
            }
            else {
                $("#uploadLabel").text("There was problem to delete Item!");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
    return retVal;
}

//End edit heritage

function get_Hreitage_ById(e) {
    e.preventDefault();

    var herUserId = parseInt($(e.currentTarget).parent().parent().find("input[type='hidden']").val());
    if (isNaN(herUserId) || herUserId == 0) {
        alert("Select heritage Id before search.");
        return false;
    }

    var user_id = parseInt($("#user_id").val());
    if (user_id == NaN || user_id == 0) {
        alert("Select heritage user before search.");
        return false;
    }


    getHreitageById(user_id, herUserId);

}

function getHreitageById(user_id, herUserId) {
    var url = '/api/HeritageApi/GetHreitageById?user_id=' + user_id + '&herUserId=' + herUserId;

    $("#ajax-loader").show();
    $.ajax({
        url: url,
        type: 'GET',
        complete: function () {
            $("#ajax-loader").hide();
        },
        success: function (data) {
            if (data.success) {
                var herArr = data.heritage.listArr;
                if (herArr != null) {
                    var her_0_Arr = herArr.shift();
                    var her_0 = her_0_Arr[0];

                    //var her1 = her[0];
                    addHerMain(her_0);
                    var divStr = getDivString_0(her_0);
                    $("#mainAncestor").find("div.card").find("div[id='fn_ln_bdt']").empty().append(divStr);
                    var parentClickedObj = $("#mainAncestor").find("div.card").find(".fa-plus-square");

                    if (herArr.length > 0) {
                        var parentClickedObjArray = [parentClickedObj];
                        addThisLevelHers(0, herArr, parentClickedObjArray, herArr.length);
                    }
                }
            }
            else {
                alert(data.message);
                return false;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
}

function addHerMain(herObj) {
    var herV = new her("", "", 0, 0, 0, 0, -1, "/Images/Heritage/empty.png", "", "", "", "", null, "", "", 0, "", "", "", "", "", false, false);
    herV.parent_level = herObj.parent_level;
    herV.parent_number = herObj.parent_number;
    herV.level = herObj.level;
    herV.number = herObj.number;
    herV.user_id = herObj.user_id;
    herV.heratigeUser_Id = herObj.heratigeUser_Id;
    herV.her_src = herObj.her_src;

    herV.id = "" + herV.user_id + "_" + herV.heratigeUser_Id + "_" + herV.parent_level + "_" + herV.parent_number + "_" + herV.level + "_" + herV.number;
    //userId_heratigeUser_Id_parentLevel_parentNumber_level_number
    herV.pattern = "" + herV.user_id + "_" + herV.parent_level + "_" + herV.parent_number + "_" + herV.level + "_" + herV.number;
    herV.alb = "alb_" + herV.id;
    herV.alb_big = "alb_big_" + herV.id;
    herV.flagImgPath = herObj.flagImgPath;
    herV.flagImgPathBP = herObj.flagImgPathBP;
    herV.countryName = herObj.countryName;
    herV.countryNameBP = herObj.countryNameBP;
    herV.isOpend = herObj.isOpend;
    herV.isEditable = herObj.isEditable;

    $("#mainUL").empty();
    $("#heritage").tmpl(herV).appendTo($("#mainAncestor").empty());

    $("#heratigeUser_Id").val(herV.heratigeUser_Id);
    $("#mainAncestor").find("div.card").data("information", JSON.stringify(herV));
    var result = {
        message: "",
        path: herObj.her_src,
        width: herObj.tt_width,
        height: herObj.tt_height
    }
    var alb = "." + herV.alb;
    var alb_big = "." + herV.alb_big;

    insertPhoto(result, alb, alb_big);

    $('div[data-toggle="tooltip"]').tooltip({
        animated: 'fade',
        placement: 'bottom',
        html: true
    });

    //$('.popo[data-toggle="popover-hover"]').popover({ trigger: 'hover'});
    $('.popo[data-toggle="popover"]').popover({
        html: true
    }).on('click', function (e) {
        e.stopImmediatePropagation();

        $(".popo").attr('data-content', '<div style="background-color:aquamarine;width:400px;height:100px;"><div>This is your div content</div></div >');
        // $(".popo").attr('data-content', '<div><b>Example popover</b> - content</div>');
    })

    var a = "   Քոչարյանն արդեն ներկայացավ ԱԱԾ: Քոչարյանին ուղեկցում էր որդին՝ Լևոն Քոչարյանը: \n Վերջինս լրագրողների հետ զրույցում խոսեց հոր կալանավորման մասին:\n Ամբողջական հոդվածը կարող եք կարդալ այս\n հասցեով՝ https://armlur.am/910994/ ";

    return false;
};

function addThisLevelHers(i, herArr, parentClickedObjArray, length_) {
    var objClicedArr = [];
    var HerArr = herArr[i];
    console.log("i ---- " + i);
    console.log("objClicedArr " + objClicedArr);
    console.log("HerArr " + HerArr);
    for (var j = 0; j < parentClickedObjArray.length; j++) {
        var level = parentClickedObjArray[j].closest(".card").attr("level");
        var number = parentClickedObjArray[j].closest(".card").attr("number");
        console.log('level ' + level);
        console.log("number " + number);

        var pat_j = parentClickedObjArray[j].closest(".card").attr("pattern");


        for (var k = 0; k < HerArr.length; k++) {
            console.log("HerArr[k].parent_level   " + HerArr[k].parent_level);
            console.log("HerArr[k].parent_number   " + HerArr[k].parent_number);
            var pattCreated = pat_j + "_" + HerArr[k].parent_level + "_" + HerArr[k].parent_number + "_" + HerArr[k].level + "_" + HerArr[k].number;;

            console.log("pattCreated   " + pattCreated);
            console.log("HerArr[k].pattern   " + HerArr[k].pattern);

            if (pattCreated == HerArr[k].pattern) {
                var retClick = addFromDbHeritage(HerArr[k], parentClickedObjArray[j]);
                objClicedArr.push(retClick);
            }




            //if (level == HerArr[k].parent_level && number == HerArr[k].parent_number) {
            //    var retClick = addFromDbHeritage(HerArr[k], parentClickedObjArray[j]);
            //    objClicedArr.push(retClick);
            //}
        }
    }

    i += 1;

    if (i < length_) {
        console.log("i --addThisLevelHers-- " + i);
        addThisLevelHers(i, herArr, objClicedArr, length_);
    }
}
function herFilter(arr, arrQ) {
    return arr.filter(function (el) {
        return (el.parent_level == arrQ[0] && el.parent_number == arrQ[1] && el.level == arrQ[2]);
    })
}

function addFromDbHeritage(herObj, parentClickedObj) {
    var herV = new her("", "", 0, 0, 0, 0, -1, "/Images/Heritage/empty.png", "", "", "", "", null, "", "", 0, "", "", "", "", "", false, false);
    //var currDiv = $(event.currentTarget).closest('.card');
    herV.parent_level = herObj.parent_level;
    herV.parent_number = herObj.parent_number;
    herV.level = herObj.level;
    herV.number = herObj.number;
    herV.user_id = herObj.user_id;
    herV.heratigeUser_Id = herObj.heratigeUser_Id;
    herV.her_ln = herObj.her_ln;
    herV.her_fn = herObj.her_fn;
    herV.her_birthDate = herObj.her_birthDate;
    herV.her_endDate = herObj.her_endDate;
    herV.her_src = herObj.her_src;
    herV.tt_width = herObj.tt_width;
    herV.tt_height = herObj.tt_height;
    herV.flagImgPath = herObj.flagImgPath;
    herV.flagImgPathBP = herObj.flagImgPathBP;
    herV.countryName = herObj.countryName;
    herV.countryNameBP = herObj.countryNameBP;
    herV.isOpend = herObj.isOpend;
    herV.isEditable = herObj.isEditable;

    herV.id = "" + herV.user_id + "_" + herV.heratigeUser_Id + "_" + herV.parent_level + "_" + herV.parent_number + "_" + herV.level + "_" + herV.number;
    herV.pattern = herObj.pattern;
    herV.alb = "alb_" + herV.id;
    herV.alb_big = "alb_big_" + herV.id;

    var retParentObj = addFromDb(event, herV, parentClickedObj);

    return retParentObj;
}

function addFromDb(event, herV, parentClickedObj) {
    //event.stopImmediatePropagation();

    var strGrp = "<dl class='rules-group-container' id='dl_logical_group_' style='margin-left:130px;'>" +
        "<dt class='rules-group-header'>" +
        "</dt>" +
        "<dd class='rules-group-body' style='margin-bottom: 0px;'>" +
        "<ul class='rules-list' ></ul>" +
        "</dd>" +
        "</dl>";

    //var ul_ = $(event.target).closest('dl').children('dd').children('ul:first');
    var ul_ = parentClickedObj.closest('dl').children('dd').children('ul:first');
    var strG = $(strGrp);
    ul_.append(strG);


    var li__ = ul_.children('dl:last').children('dd').find("ul").append("<li id='li_logical_rule_' class='rule-container' style='height:68px;' ></li>").find("li");
    $("#heritage").tmpl(herV).appendTo(li__[0]);



    //var parentClickedObj = $("#mainAncestor").find("div.card").find(".fa-plus-square");
    //ul_.find('#' + herV.id).data("data-information", JSON.stringify(herV));
    ul_.find('#' + herV.id).data("information", JSON.stringify(herV));
    //ul_.find('#' + herV.id).attr('last_child', herV.last_child);
    var parentClickedObj_Ret = ul_.find('#' + herV.id).find(".fa-plus-square");

    var divStr = getDivString_0(herV);
    ul_.find('#' + herV.id).find("div[id='fn_ln_bdt']").empty().append(divStr);

    var result = {
        message: "",
        path: herV.her_src,
        width: herV.tt_width,
        height: herV.tt_height
    }
    var alb = "." + herV.alb;
    var alb_big = "." + herV.alb_big;

    if (herV.tt_width > 0 && herV.tt_height > 0) {
        insertPhoto(result, alb, alb_big);
    }







    $('div[data-toggle="tooltip"]').tooltip({
        animated: 'fade',
        placement: 'bottom',
        html: true
    });

    //$('.popo[data-toggle="popover-hover"]').popover({ trigger: 'hover'});
    $('.popo[data-toggle="popover"]').popover({
        html: true
    }).on('click', function (e) {
        e.stopImmediatePropagation();

        $(".popo").attr('data-content', '<div style="background-color:aquamarine;width:400px;height:100px;"><div>This is your div content</div></div >');
        // $(".popo").attr('data-content', '<div><b>Example popover</b> - content</div>');
    })





    //$('.popo[data-toggle="popover"]').on('show.bs.popover', function (e) {
    //    e.preventDefault(');
    //})

    var a = "   Քոչարյանն արդեն ներկայացավ ԱԱԾ: Քոչարյանին ուղեկցում էր որդին՝ Լևոն Քոչարյանը: \n Վերջինս լրագրողների հետ զրույցում խոսեց հոր կալանավորման մասին:\n Ամբողջական հոդվածը կարող եք կարդալ այս\n հասցեով՝ https://armlur.am/910994/ ";
    //$(".popo").attr('data-content', '<p>' + a + '</p>');
    //$(".popo").attr('data-content', a);


    return parentClickedObj_Ret;
};
//End ---  Searched Heritage  ---

// autocomplates and inserts --- begin ---
$(document).ready(function () {
    $(document).on("click", ".a_new_id", clickForNewRecord);
    $(document).on("click", ".auto-dropdown-click", autoDropdownClick);

    //@ sourceURL=~/Scripts/UploadImage.js
});

function attachAutocomplate() {
    $(".auto_complates").autocomplete({
        //appendTo: '#aaa',
        source: function (request, response) {
            var input = this.element;
            //countryId_provinceId_cityId_churchId_countryBPId_provinceBPId_cityBPId
            var allTextBoxIds = getAllTextBoxIds(input);
            var chk = checkTextBoxSequance(input);
            if (chk != "" && chk != undefined) {
                alert(chk);
                input.removeClass('spinner');
                return false;
            }

            $.ajax({
                type: "GET",
                url: "/api/HeritageApi/Autocomplate?term=" + request.term + "&className=" + $(this.element).attr('name') + "&allTextBoxIds=" + allTextBoxIds,
                contentType: "application/json ; charset=utf-8",
                dataType: "json",
                complete: function () {
                    var r = 0;
                },
                success: function (msg) {
                    response($.map(msg, function (item) {
                        return {
                            "label": item.label,
                            "value": item.value
                        }
                    }))
                },
                error: function (msg) {
                    var v = msg;
                }
            });
        },
        select: function (event, ui) {
            var label_ = ui.item.label;
            var val_ = ui.item.value;
            ui.item.value = label_;

            $(event.target).parent().children("input[type='hidden']").val(val_);
        },
        search: function (event, ui) {
            $(this).parent().children("input[type='text']").addClass('spinner');
        },
        response: function (event, ui) {
            $(this).parent().children("input[type='text']").removeClass('spinner');
        },
        minLength: 1
    });

    $.widget("custom.tablecomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> li:not(.ui-autocomplete-header)");
        },
        _renderMenu(ul, items) {
            var self = this;
            ul.addClass("container");
            ul.addClass("forUL");

            let header = {
                country: "Country",
                province: "Province",
                city: "City",
                lN_FN: "Name",
                isOpend: "",
                isheader: true
            };
            self._renderItemData(ul, header);
            $.each(items, function (index, item) {
                self._renderItemData(ul, item);
            });

        },
        _renderItemData(ul, item) {
            return this._renderItem(ul, item).data("ui-autocomplete-item", item);
        },
        _renderItem(ul, item) {
            var titel_ = "";
            var titel_BP = "";
            var $li = $("<li class='ui-menu-item' role='presentation'></li>");
            if (item.isheader) {
                $li = $("<li class='ui-autocomplete-header'  style='font-weight:bold !important;'></li>");
                var $content = "<div class='row ui-menu-item-wrapper'>" +
                    "<div style='width:130px;float:left;' heritageIds='" + item.heritageIds + "' >" + item.country + "</div>" +
                    "<div style='width:130px;float:left;'>" + item.province + "</div>" +
                    "<div style='width:130px;float:left;'>" + item.city + "</div>" +
                    "<div style='width:400px;float:left;'>" + item.lN_FN + "</div>" +
                    "<div style='width:50px;float:left;'>" + "</div>" +
                    "<div style='width:50px;float:left;'>" + "</div>" +
                    "<div style='float:left;' isOpend='" + item.isOpend + "' >" + "</div>" +
                    "</div>";
            }
            else {
                var $li = $("<li class='ui-menu-item auto-dropdown-click' role='presentation'></li>");

                titel_ = (item.flagImgPath != undefined && item.flagImgPath != "" && item.flagImgPath != "-") ? "Country person is living: " + item.country + " - " + item.province + " - " + item.city : "";
                titel_BP = (item.flagImgPathBP != undefined && item.flagImgPathBP != "" && item.flagImgPathBP != "-") ? "Countyr person was born: " + item.countryBP + " - " + item.provinceBP + " - " + item.cityBP : "";
                var $content = "<div class='row ui-menu-item-wrapper'>" +
                    "<div style='width:130px;float:left;' heritageIds='" + item.heritageIds + "' >" + item.country + "</div>" +
                    "<div style='width:130px;float:left;'>" + item.province + "</div>" +
                    "<div style='width:130px;float:left;'>" + item.city + "</div>" +
                    "<div style='width:400px;float:left;'>" + item.lN_FN + "</div>" +
                    "<div style='width:50px;float:left;'>" + "<img src='" + item.flagImgPathBP + "' title='" + titel_BP + "' />" + "</div>" +
                    "<div style='width:50px;float:left;'>" + "<img src='" + item.flagImgPath + "' title='" + titel_ + "' />" + "</div>" +
                    "<div style='float:left;' isOpend='" + item.isOpend + "' isEditable='" + item.isEditable + "' >" + (item.isOpend == true ? "<i class='fa fa-lock'></i>" : "") + "</div>" +
                    "</div>";
            }

            $li.html($content);

            return $li.appendTo(ul);
        }
    });

    $(".auto_complates_main").tablecomplete({
        source: function (request, response) {
            var input = this.element;
            //countryId_provinceId_cityId_churchId_countryBPId_provinceBPId_cityBPId
            var allTextBoxIds = getAllTextBoxIds(input);
            var chk = checkTextBoxSequance(input);
            if (chk != "" && chk != undefined) {
                alert(chk);
                input.removeClass('spinner');
                return false;
            }

            $.ajax({
                type: "GET",
                url: "/api/HeritageApi/AutocomplateMain?term=" + request.term + "&className=" + $(this.element).attr('name') + "&allTextBoxIds=" + allTextBoxIds,
                contentType: "application/json ; charset=utf-8",
                dataType: "json",
                complete: function () {
                },
                success: function (msg) {
                    response(msg);
                },
                error: function (msg) {
                    var v = msg;
                }
            });
        },
        select: function (event, ui) {
            var label_ = ui.item.label;
            var val_ = ui.item.value;
            ui.item.value = label_;

            $(event.target).parent().children("input[type='hidden']").val(val_);
        },
        search: function (event, ui) {
            $(this).parent().children("input[type='text']").addClass('spinner');
        },
        response: function (event, ui) {
            debugger;
            $(this).parent().children("input[type='text']").removeClass('spinner');
        },
        minLength: 1
    });

}

function clickForNewRecord(event) {
    var obj = $(event.currentTarget).parent().children("input[type='text']").attr("name");
    createDiv(event, obj);
    $(document).on('click', '.new-record-insert', new_record_insert);
    $(document).on('click', '.new-record-remove', new_record_remove);
}

function createDiv(event, obj) {
    var objDid = $(event.currentTarget).closest(".form-group");
    var height = objDid.height();
    var width = objDid.width();

    $("<div style='width:" + width + "px;height:40px;background-color:red;posotion:relative;' >"
        + "<div style='background-color:blue;float:left;height:40px;width:" + (width - 50) + "px;'>"
        + "<input type='text' class='input_record' placeholder='New " + obj + " name'/>"
        + "</div>"
        + "<div style='float:right;height:40px;width:40px;'><i class='fa fa-save pointer new-record-insert'></i><i class='fa fa-window-close pointer ml_2 new-record-remove'></i></div> "
        + "</div > ").appendTo(objDid).position({
            my: "left top",
            at: "left top",
            of: objDid.find("input.form-control")
        });

    objDid.css("height", height);
}

function new_record_insert(event) {
    event.preventDefault()
    event.stopImmediatePropagation();

    var this_ = $(event.currentTarget).parent().parent().find("input[class='input_record']");

    var obj = $(event.currentTarget).closest(".form-group").children("input[type='text']").attr("name");
    var name_ = $(event.currentTarget).parent().parent().find("input[class='input_record']").val();

    if (name_ == undefined || name_ == null || $.trim(name_) == "") {
        alert("Name can not be empty.");
        return false;
    }

    var validation = CheckValidation(event, obj);
    if (validation.val == false) {
        alert(validation.message);
        return false;
    }

    validation.name = name_;
    validation.objName = obj;

    $("#ajax-loader").show();
    $.ajax({
        url: "/api/HeritageApi/InsertNewRecord",
        type: "POST",
        //data: JSON.stringify({ name: name_, objName: obj }),
        data: JSON.stringify(validation),
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        complete: function () {
            $("#ajax-loader").hide();
        },
        cache: false,
        //contentType: false,
        processData: false,

        success: function (result) {
            if (result.success) {
                this_.parent().parent().remove();
                var obj = result.obj;
                $("#herEditLabel").text("New " + obj + " was saved!");
                showNotificationMessage("success", "New " + obj + " was saved!");
                //$('div[data-toggle="tooltip"]').tooltip({
                //    animated: 'fade',
                //    placement: 'bottom',
                //    html: true
                //});
            }
            else {
                $("#herEditLabel").text("There was problem to save!" + result.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            checkAjaxError(jqXHR);
        }
    });
}

function new_record_remove(event) {
    $(event.currentTarget).parent().parent().find("input[class='input_record']").parent().parent().remove();
}

function CheckValidation(event, obj) {
    var form_ = $(event.currentTarget).closest("form[id='get-edit-heritage']");
    var countryId = form_.find("input[id='countryId']").val();
    var provinceId = form_.find("input[id='provinceId']").val();
    var cityId = form_.find("input[id='cityId']").val();
    var churchId = form_.find("input[id='churchId']").val();

    var retVal = {
        val: true,
        name: "",
        objName: "",
        countryId: countryId,
        provinceId: provinceId,
        cityId: cityId,
        churchId: churchId,
        message: ""
    };

    if (obj === "country") {
    }
    else if (obj === "province") {
        if (countryId == 0) {
            retVal.val = false;
            retVal.message = "Country must be selected before inserting province.";
        }
    }
    else if (obj === "city") {
        if (countryId == 0) {
            retVal.val = false;
            retVal.message = "Country must be selected before inserting city.";
        }
        if (provinceId == 0) {
            retVal.val = false;
            retVal.message = "Province must be selected before inserting city.";
        }
    }
    else if (obj === "church") {
        if (countryId == 0) {
            retVal.val = false;
            retVal.message = "Country must be selected before inserting church.";
        }
        if (provinceId == 0) {
            retVal.val = false;
            retVal.message = "Province must be selected before inserting church.";
        }
        if (cityId == 0) {
            retVal.val = false;
            retVal.message = "City must be selected before inserting church.";
        }
    }
    else {
        retVal.val = false;
        retVal.message = "There was wrong value of pbj.";
    }

    return retVal;
}

function checkTextBoxSequance(input) {

    var inpName = input.attr("name");
    var fluidDiv = input.closest(".container-fluid");

    if (fluidDiv.length > 0) {
        if (inpName == "province" && fluidDiv.find("#countryId").val() == 0) {
            return "Select Country befor selecting Province.";
        }

        if (inpName == "city" && (fluidDiv.find("#countryId").val() == 0 || fluidDiv.find("#provinceId").val() == 0)) {
            return "Select Country and Province befor selecting City.";
        }

        if (inpName == "church" && (fluidDiv.find("#countryId").val() == 0 || fluidDiv.find("#provinceId").val() == 0 || fluidDiv.find("#cityId").val() == 0)) {
            return "Select Country, Province and City befor selecting Church.";
        }


        if (inpName == "provinceBP" && fluidDiv.find("#countryBPId").val() == 0) {
            return "Select Country(Birth Place) befor selecting Province(Birth Place).";
        }

        if (inpName == "cityBP" && (fluidDiv.find("#countryBPId").val() == 0 || fluidDiv.find("#provinceBPId").val() == 0)) {
            return "Select Country(Birth Place) and Province(Birth Place) befor selecting City(Birth Place).";
        }
    }
    else {
        return "";
    }

}

function getAllTextBoxIds(input) {
    var retVal = "";
    var inpName = input.attr("name");
    var fluidDiv = input.closest(".container-fluid");
    //countryId_provinceId_cityId_curchId_countryBPId_provinceBPId_cityBPId_userId_heratigeUserId
    retVal = fluidDiv.find("#countryId").val() + "_" +
        fluidDiv.find("#provinceId").val() + "_" +
        fluidDiv.find("#cityId").val() + "_" +
        fluidDiv.find("#churchId").val() + "_" +
        fluidDiv.find("#countryBPId").val() + "_" +
        fluidDiv.find("#provinceBPId").val() + "_" +
        fluidDiv.find("#cityBPId").val() + "_" +
        $("#user_id").val() + "_" +
        $("#heratigeUser_Id").val();

    return retVal;
}

function autoDropdownClick(e) {
    var this_ = $(e.currentTarget).find("div[isOpend]").attr("isOpend");
    var ids = $(e.currentTarget).find("div[heritageIds]").attr("heritageIds");
    if (ids == undefined || ids == "") {
        alert("Identities are missing!");
        return false;
    }

    if (this_ == "false") {
        var arr = ids.split("_");
        if (arr != null && arr.length == 2) {
            if (arr[0] == $("#user_id").val()) {
                getHreitageById(arr[0], arr[1]);
            }
            else {
                if (confirm("To access this heritage line you have to get permission from creator.")) {
                    askHerPermission(e, ids, false);
                } else {
                    return false;
                }

            }
        }
        else {
            alert("Identities are missing!");
            return false;
        }
    }
    else {
        var arr = ids.split("_");
        if (arr != null && arr.length == 2) {
            getHreitageById(arr[0], arr[1]);
        }
        else {
            alert("Identities are missing!");
            return false;
        }
    }
}

function askHerPermission(e, ids, isEditable) {
    e.preventDefault();
    //if ($("#heratigeUser_Id").val() === "0" || $("#user_id").val() === "") {
    //    alert("Register Heritage before uploading photo.");
    //    return false;
    //}


    var url = '/Heritage/AskHerPermission?ids=' + ids + "&isEditable=" + isEditable;
    var modalID = '#getHerPermissionModal';

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
                //$("#herUploadImageModal .id_alb").val(info.alb);
                //$("#herUploadImageModal .id_alb_big").val(info.alb_big);
                //$("#herUploadImageModal #heritageIds").val(info.id);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }
}

// autocomplates and inserts --- end ---

//Permissions  ---- begin  ------- setPermission
function get_setHerPermission(e) {
    e.preventDefault();
    if ($("#user_id").val() === "") {
        alert("Register Users have access to Permissions.");
        return false;
    }


    var url = '/Heritage/SetHerPermission?user_id=' + $("#user_id").val();
    var modalID = '#setHerPermissionModal';

    if ($(modalID).length <= 0) {

        $("#ajax-loader").show();
        $.ajax({
            url: url,
            type: 'GET',
            complete: function () {
                $("#ajax-loader").hide();
            },
            success: function (data) {
                loadCBModal(modalID, data,"modal-xl");
                //$("#herUploadImageModal .id_alb").val(info.alb);
                //$("#herUploadImageModal .id_alb_big").val(info.alb_big);
                //$("#herUploadImageModal #heritageIds").val(info.id);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                checkAjaxError(jqXHR);
            }
        });
    }
}

function post_setHerPermission(e) {
    e.preventDefault();

    //if ($("#countryId").val() == "0") {
    //    alert("Country must be selected!");

    //    return false;
    //}

    //var card_ = $(e.currentTarget).closest('.card');
    //var dataInfoDiv = $("#get-edit-heritage input[id='data_information']");
    var fd = new FormData($('form[id="post-her-comment-image"]')[0]);

    var modalID = "#post_setHerPermModal";
    var modalContent = "#post_setHerPermModal-ct";
    var ser = $("#set-her-permission").serialize();

    $("#ajax-loader").show();
    $.ajax({
        url: this.action,
        type: this.method,
        data: ser,
        //contentType: "application/json",
        async: false,
        complete: function () {
            $('.modal-submit-btn').prop('disabled', false);
            $("#ajax-loader").hide();
        },


        success: function (result) {
            if (result.success) {
                $("#heratigeUser_Id").val(result.heratigeUser_Id);
                $("#herEditLabel").text(result.message);


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
}

//Permissions  ---- end  -------

//Helper Methods ----------------- Begin  --------------------
function updateCardInformation(e, clickedCard) {
    var updateData1 = $('#heritageModal').find('form');

    var divStr = getDivString(updateData1);
    clickedCard.find("div[id='fn_ln_bdt']").empty().append(divStr);
    var heratigeUser_Id = $("#heratigeUser_Id").val();

    var card_data = JSON.parse(clickedCard.data("information"));
    card_data.heratigeUser_Id = $("#heratigeUser_Id").val();
    clickedCard.data("information", JSON.stringify(card_data));


    var arr = clickedCard.prop("id").split("_");
    if (arr[1] == "0" && heratigeUser_Id != "") {
        var newId = arr[0] + "_" + heratigeUser_Id + "_" + arr[2] + "_" + arr[3] + "_" + arr[4] + "_" + arr[5];
        clickedCard.prop("id", newId);
    }
}

function getDivString(updateData1) {
    var DOB_str = "";
    var DOB_arr = (updateData1.find("input[id='DOB_str']").val()).split("/");
    if (DOB_arr.length > 2) {
        DOB_str = DOB_arr[2];
    }

    var PWD_str = "";
    var PWD_arr = (updateData1.find("input[id='PWD_str']").val()).split("/");
    if (PWD_arr.length > 2) {
        PWD_str = PWD_arr[2];
    }

    var elemStr = "<div class='fl_c'><div class='in_bl theNames' title='" + updateData1.find("input[id='fName']").val() + " " + updateData1.find("input[id='lName']").val() + "' >" + updateData1.find("input[id='fName']").val() + " " + updateData1.find("input[id='lName']").val() + "</div></div>" +
        "<div class='fl_c'><div class='in_bl'>" + DOB_str + " " + (PWD_str == "" ? "" : "- " + PWD_str) + "</div></div>";

    return elemStr;
}

function getDivString_0(her) {
    //var DOB_str = "";
    //var PWD_str = "";
    var elemStr = "<div class='fl_c'><div class='in_bl theNames' title='" + (her.her_ln == undefined ? "" : her.her_ln) + " " + (her.her_fn == undefined ? "" : her.her_fn) + "' >" + (her.her_ln == undefined ? "" : her.her_ln) + " " + (her.her_fn == undefined ? "" : her.her_fn) + "</div></div>" +
        "<div class='fl_c'><div class='in_bl'>" + (her.her_birthDate == undefined ? "" : her.her_birthDate) + " " + (her.her_endDate == undefined ? "" : her.her_endDate) + "</div></div>";

    return elemStr;
}

function fireDatePicker(picker) {
    picker.on('shown.bs.modal', function (e) {
        $(this).find(".dateClass").datepicker({
            dateFormat: "dd/mm/yy",
            startDate: "01-01-1855",
            endDate: "01-01-2120",
            todayBtn: "linked",
            autoclose: true,
            todayHighlight: true,
            yearRange: "c-17:c+17",
            changeYear: true,
            changeMonth: true
        });
    })
}

function uploadFileInput(e) {
    var file = this.files;
    var res = '';
    $.each(file, function (index, value) {
        res += '-  ' + value.name + ',\n ';
    });
    $('.file-upload-filenames').text(res);
    if (file.size > 1024) {
        alert('max upload size is 1k');
    }
}

//Helper Methods ---------------- End ------------------------