var PAGESIZE = 3;
//var DATEFORMAT = "mm/dd/yy";
//var ORDER_NONE = "";
var ORDER_ASC = "asc";
var ORDER_DESC = "desc";

var totalItems, totalPages, currentPage, startPage, endPage, startIndex, endIndex, totalRowRequest;
var sortByColumn, sortByOrder;

$(document).ready(function () {
    totalRowRequest = false;
    sortByColumn = "";
    sortByOrder = "";
   // $("#buttonSearch").click(function () { buttonSearchClicked() });
    //$("#dateStart").datepicker({ dateFormat: DATEFORMAT, changeMonth: true, changeYear: true });

    //add following code to prevent multiple modals overlay issue
    //$(document).on('show.bs.modal', '.modal', function (event) {
    //    var zIndex = 1040 + (10 * $('.modal:visible').length);
    //    $(this).css('z-index', zIndex);
    //    setTimeout(function () { $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack'); }, 0);
    //});

    ////add following code to show missing "close" button in dialog page
    //var btn = $.fn.button.noConflict() // reverts $.fn.button to jqueryui btn
    //$.fn.btn = btn // assigns bootstrap button functionality to $.fn.btn

    initialize();
});

function initialize() {
    totalRowRequest = true;
    totalPages = 0;
    setPage(1);
}

function setPage(page) {
    if (totalPages > 0 && (page < 1 || page > totalPages)) {
        return;
    }

    currentPage = page;
    loadPage(page);
    return false;
}

function loadPage(page) {
    var url = "/Home/LoadPagination";
    $.ajax({
        method: "POST",
        url: url,
        data: getSearchCriteria(page),
        async: true,
        beforeSend: function () {
            $('#ajax-loader').show();
        },
        complete: function () {
            $('#ajax-loader').hide();
        },
        success: function (result) {
            if (result.errorMessage == "") {
                totalRowRequest = false;
                showRequisitionList(result);
            } else {
                //showMessageModal("error", result.errorMessage, false);
                getErrorModal(result.errorMessage)
            }
        },
        error: function (xhr, thrownError) {
            checkAjaxError(url, xhr, thrownError);
        }
    });
}

function showRequisitionList(result) {
    if (result.totalRow != -1) {
        totalItems = result.totalRow;
    }

    $("#newsList tbody").html("");
    if (result.news.length == 0) {
        $("#newsList").append("<tr><td colspan='5'><br /><br /><br /><br /><br /><br /><br /><br /><div style='font-size: 28px; color: red; text-align:center;'>No record found</div></td></tr>");
    } else {
        $.each(result.news, function (n, news_) {
            var trClass = "";
            if (n % 2 == 1) {
                trClass = " class='active'";
            }
            var tr = "<tr" + trClass + ">";
            //tr += "<td>" + getPatientInfo(news_.patientRecordId, news_.patientName) + "</td>";
            //if (news_.appointmentId > 0) {
            //    tr += "<td><a href='schedule/daysheet?OfficeId=" + news_.officeId + "&Date=" + news_.requisitionTime + "'>" + news_.requisitionTime + "</a></td>";
            //} else {
            //    tr += "<td>" + news_.requisitionTime + "</td>";
            //}
            tr += "<td>" + news_.name + "</td>";
            tr += "<td>" + news_.description + "</td>";
            tr += "<td>" + news_.path + "</td>";
            tr += "</tr>";
            $("#newsList").append(tr);
        });
    }
    showPagination();
    //initPopover();
}

function sortBy(columnName) {
    if (sortByColumn == columnName) {
        if (sortByOrder == ORDER_DESC) {
            sortByOrder = ORDER_ASC;
        } else if (sortByOrder == ORDER_ASC) {
            sortByOrder = ORDER_DESC;
        } else {
            sortByOrder = ORDER_DESC;
        }
    } else {
        sortByColumn = columnName;
        sortByOrder = ORDER_DESC;
    }

    var sortingHeaders = ["imageLName", "imageDescription", "imagePath"]
    $.each(sortingHeaders, function (index, sortingHeader) {
        var imageClass = "fa fa-sort-asc";
        if (sortingHeader.toLowerCase().indexOf(sortByColumn.toLowerCase()) > -1) {
            if (sortByOrder == ORDER_DESC) {
                imageClass = "fa fa-sort-desc";
            } else if (sortByOrder == ORDER_ASC) {
                imageClass = "fa fa-sort-asc";
            }
        }
        $("#" + sortingHeader).attr("class", imageClass);
    });

    loadPage(currentPage);
}

function showPagination() {
    // calculate total pages
    totalPages = Math.ceil(totalItems / PAGESIZE);

    if (totalPages <= 10) {
        // less than 10 total pages so show all
        startPage = 1;
        endPage = totalPages;
    } else {
        // more than 10 total pages so calculate start and end pages
        if (currentPage <= 6) {
            startPage = 1;
            endPage = 10;
        } else if (currentPage + 4 >= totalPages) {
            startPage = totalPages - 9;
            endPage = totalPages;
        } else {
            startPage = currentPage - 5;
            endPage = currentPage + 4;
        }
    }

    // calculate start and end item indexes
    //startIndex = (currentPage - 1) * PAGESIZE;
    //endIndex = Math.min(startIndex + PAGESIZE - 1, totalItems - 1);

    // create html code of pages
    $("#pagination li").remove();
    if (totalPages <= 1) {
        $("#pagination").hide();
    } else {
        $("#pagination").show();
        var liClass = "";
        if (totalPages > 10) {
            if (currentPage == 1) {
                liClass = " class='disabled'";
            }
            $("#pagination").append("<li" + liClass + "><a href='#' onclick='return setPage(1)' style='padding-left: 4px; padding-right: 4px;'><span class='fa fa-angle-double-left'></span></a></li>");
            $("#pagination").append("<li" + liClass + "><a href='#' onclick='return setPage(" + (currentPage - 1) + ")' style='padding-left: 4px; padding-right: 4px;'><span class='fa fa-angle-left'></span></a></li>");
        }

        for (var i = startPage; i <= endPage; i++) {
            liClass = "";
            if (currentPage == i) {
                liClass = " class='active'";
            }
            $("#pagination").append("<li" + liClass + "><a href='#' onclick='return setPage(" + i + ")' style='padding-left: 4px; padding-right: 4px;'>" + i + "</a></li>");
        }

        if (totalPages > 10) {
            liClass = "";
            if (currentPage == totalPages) {
                liClass = " class='disabled'";
            }
            $("#pagination").append("<li" + liClass + "><a href='#' onclick='return setPage(" + (currentPage + 1) + ")' style='padding-left: 4px; padding-right: 4px;'><span class='fa fa-angle-right'></span></a></li>");
            $("#pagination").append("<li" + liClass + "><a href='#' onclick='return setPage(" + totalPages + ")' style='padding-left: 4px; padding-right: 4px;'><span class='fa fa-angle-double-right'></span></a></li>");
        }
        $("#pagination").append("<li><a style='width: 120px;'>Page " + currentPage + " of " + totalPages + "</a></li>");
    }
}

function addTextFilter(filters, filterName) {
    if ($("#" + filterName).val() != "") {
        filters.push({ text: $("#" + filterName).val(), value: filterName });
    }
    return filters;
}

function addDropdownFilter(filters, filterName, filterName2) {
    if ($("#" + filterName).children("option:first-child").is(":selected")) {
        return filters;
    }
    if (filterName2 != "" && $("#" + filterName2).children("option:first-child").is(":selected")) {
        return filters;
    }

    var filter = $("#" + filterName + " > :selected").map(function () { return this.value }).get().join(",");
    if (filterName2 != "") {
        filter += "," + $("#" + filterName2 + " > :selected").map(function () { return this.value }).get().join(",");
        filter = filter.replace(/^,|,$/g, '');
    }
    if (filter != "") {
        filters.push({ text: filter, value: filterName });
    }

    return filters;
}

function getSearchCriteria(page) {
    //var filters = [];
    //filters = addTextFilter(filters, "dateStart");
    //filters = addTextFilter(filters, "dateEnd");
    //filters = addDropdownFilter(filters, "practiceDoctorIds", "");
    //filters = addDropdownFilter(filters, "officeIds", "");
    //filters = addDropdownFilter(filters, "requisitionTypeIds", "");
    //filters = addDropdownFilter(filters, "requisitionStatusIds", "");

    var data = {
        page: page,
        rowStart: (page - 1) * PAGESIZE,
        pagesize: PAGESIZE,
        totalRowRequest: totalRowRequest,
        sortByColumn: sortByColumn,
        sortByOrder: sortByOrder
        //,
        //filters: filters
    }

    return data;
}

