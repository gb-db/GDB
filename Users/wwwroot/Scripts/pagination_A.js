var ORDER_ASC = "asc";
var ORDER_DESC = "desc";

//var totalItems, totalPages, currentPage, startPage, endPage, startIndex, endIndex, totalRowRequest;
var sortByColumn, sortByOrder;

$(document).ready(function () {
    sortByColumn = $("#sortByColumn").val();
    sortByOrder = $("#sortByOrder").val();

    putSortImage(sortByOrder);
});


function sortBy_A(columnName) {
    //for first call
    if (sortByColumn == "") {
        sortByColumn = columnName;
    }

    if (sortByColumn == columnName) {
        if (sortByOrder == ORDER_DESC) {
            sortByOrder = ORDER_ASC;
        } else if (sortByOrder == ORDER_ASC) {
            sortByOrder = ORDER_DESC;
        } else {
            sortByOrder = ORDER_ASC;
        }
    } else {
        sortByColumn = columnName;
        sortByOrder = ORDER_ASC;
    }

    $("#sortByColumn").val(sortByColumn);
    $("#sortByOrder").val(sortByOrder);

    //var sortingHeaders = ["imageLName", "imageDescription", "imagePath"]
    //$.each(sortingHeaders, function (index, sortingHeader) {
    //    var imageClass = "fa fa-sort-asc";
    //    if (sortingHeader.toLowerCase().indexOf(sortByColumn.toLowerCase()) > -1) {
    //        if (sortByOrder == ORDER_DESC) {
    //            imageClass = "fa fa-sort-desc";
    //        } else if (sortByOrder == ORDER_ASC) {
    //            imageClass = "fa fa-sort-asc";
    //        }
    //        $("#href_" + sortingHeader).attr("href", $("#href_" + sortingHeader).attr("href") + "&sortByColumn=" + sortByColumn + "&sortByOrder=" + sortByOrder);
    //    }
    //    $("#" + sortingHeader).attr("class", imageClass);
    //});

    putSortImage(sortByOrder);


    return true;
    //loadPage(currentPage);
}

function putSortImage(sortByOrder) {
    if (sortByOrder == "") {
        sortByOrder = ORDER_ASC; 
    }
    var sortingHeaders = ["imageLName", "imageDescription", "imagePath"]
    $.each(sortingHeaders, function (index, sortingHeader) {
        var imageClass = "fa fa-sort-asc";
        //if (sortingHeader.toLowerCase().indexOf(sortByColumn.toLowerCase()) > -1) {
        if (sortByColumn != undefined && sortingHeader.toLowerCase().indexOf(sortByColumn.toLowerCase()) > -1) {
            if (sortByOrder == ORDER_DESC) {
                imageClass = "fa fa-sort-desc";
            } else if (sortByOrder == ORDER_ASC) {
                imageClass = "fa fa-sort-asc";
            }

            var hh = $("#href_" + sortingHeader).attr("href");
            if (hh.indexOf('&') > 0) {
            hh = hh.substr(0, hh.indexOf('&'));
            }

            $("#href_" + sortingHeader).attr("href", hh + "&sortByColumn=" + sortByColumn + "&sortByOrder=" + sortByOrder);

            //$("#href_" + sortingHeader).attr("href", $("#href_" + sortingHeader).attr("href") + "&sortByColumn=" + sortByColumn + "&sortByOrder=" + sortByOrder);
        }
        $("#" + sortingHeader).attr("class", imageClass);
    });
}