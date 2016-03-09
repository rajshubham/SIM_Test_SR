// currentPage is currently active page and pages is total number of pages i.e, maximum page
function displayLinks(currentPage, pages, sortParameter, sortDirection, dataBindingMethodName, hdnCurrentPageNumberId) {
    //If the number of pages is less than 10: Always display links 1 2 3 4..... When someone clicks on 1 2 3 4 5 6.... Get the index of the clicked on element,
    //Switch the selected class to the clicked on element and load page data according to the index of the clicked on elemen
    if (window.innerWidth < 768) {
        if (pages == 1 || pages == 0) {
            var pagers = "<div class='pagination-container'><ul class='pagination'></ul><div style='clear:both;'></div></div>";
            //$("#paginator").remove();
            return pagers;
        }
        if (pages <= 3) {
            var pagers = "<div class='pagination-container'><ul class='pagination'>";
            if (currentPage > 1) {
                pagers += "<li ><a style='cursor: pointer;' onclick='PagerLinkClick(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
            }
            for (i = 1; i <= pages; i++) {
                if (i == currentPage) {
                    pagers += "<li class='active'><a style='cursor: pointer;' onclick='PagerLinkClick(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                } else {
                    pagers += "<li><a style='cursor: pointer;' onclick='PagerLinkClick(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                }
            }
            if (currentPage != pages) {
                pagers += "<li ><a style='cursor: pointer;' onclick='PagerLinkClick(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
            }
            pagers += "</ul><div style='clear:both;'></div></div>";
            //$('#paginator').remove();

            // return html for pagination div
            return pagers;
        }
            // If the number of pages is more than 10:
            // We are going to have three different cases:                
        else {
            if (currentPage <= 3) {
                // Draw the first 5 then have ... link to last
                var pagers = "<div class='pagination-container'><ul class='pagination'>";
                if (currentPage > 1) {
                    pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                }
                for (i = 1; i <= 3; i++) {
                    if (i == currentPage) {
                        pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    } else {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    }
                }
              
                if (parseInt(pages - 1) != 3) {
                    pagers += "<li><a style='cursor: pointer;' onclick='ShowNextRecordAndUpdatePageLinks(4,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                   pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + parseInt(pages - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + parseInt(pages - 1) + "</a></li>";
               }
               pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + pages + "</a></li>";
               if (currentPage != pages) {
                   pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
               }
               // pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>"
                
                pagers += "</ul><div style='clear:both;'></div></div>";

                //$("#paginator").remove();
                return pagers;
            }
            else {
                if (pages <= 6) {
                    // Draw the first 5 then have ... link to last
                    var pagers = "<div class='pagination-container'><ul class='pagination'>";
                   
                    var startNum = pages - 3 + 1;
                   
                   // pagers += "<li><a style='cursor: pointer;' onclick='ShowFirstRecordAndUpdatePageLinks(1,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>««</a></li>"
                    var prevRecordPageNum = startNum - 1;
                    if (currentPage > 1) {
                        pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                    }
                    if (startNum != 1) {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 1 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 1 + "</a></li>";
                        if (startNum != 2) {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 2 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 2 + "</a></li>";
                            pagers += "<li><a style='cursor: pointer;' onclick='ShowPreviousRecordAndUpdatePageLinks(" + prevRecordPageNum + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                        }
                     } for (i = startNum; i <= startNum + 5; i++) {
                        if (i == currentPage) {
                            pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        } else {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        }
                    }
                    if (currentPage != pages) {
                        pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                    }
                    //  pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>"
                    pagers += "</ul><div style='clear:both;'></div></div>";
                    //$("#paginator").remove();
                    return pagers;
                }
                else {
                    // first ... link and last ...link
                    var pagers = "<div class='pagination-container'><ul class='pagination'>";
                    var startNum = (Math.floor((currentPage - 1) / 2) * 2) + 1;
                    var endNum = startNum + 2 - 1;

                    if (endNum > pages) {
                        endNum = pages;
                    }
                    var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";
                  //  pagers += "<li><a style='cursor: pointer;' onclick='ShowFirstRecordAndUpdatePageLinks(1,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>««</a></li>"
                    if (currentPage > 1) {
                        prevPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                        pagers += prevPage;

                    }
                    if (startNum != 1) {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 1 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 1 + "</a></li>";
                        if (startNum != 2) {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 2 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 2 + "</a></li>";
                            pagers += "<li><a style='cursor: pointer;' onclick='ShowPreviousRecordAndUpdatePageLinks(" + (startNum - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                        }
                        } for (i = startNum; i <= endNum; i++) {
                        if (i == currentPage) {
                            pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        } else {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        }
                    }
                   
                    if (endNum < pages) {
                        
                      // pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>";
                        if (endNum != (parseInt(pages - 1))) {
                            pagers += "<li><a style='cursor: pointer;' onclick='ShowNextRecordAndUpdatePageLinks(" + (endNum + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                         pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(pages - 1)) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + parseInt(pages - 1) + "</a></li>";
                     }
                     pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + pages + "</a></li>";
                    }
                    var nextpage = "<li ><a style='cursor: default;' disabled>Next</a></li>";
                     if (currentPage != pages) {
                         nextpage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                         pagers += nextpage;

                     }
                    pagers += "</ul><div style='clear:both;'></div></div>";
                    //$("#paginator").remove();
                    return pagers;
                }
            }
        }
    }

    else {
        if (pages == 1 || pages == 0) {
            var pagers = "<div class='pagination-container'><ul class='pagination'></ul><div style='clear:both;'></div></div>";
            //$("#paginator").remove();
            return pagers;
        }
        if (pages <= 6) {
            var pagers = "<div class='pagination-container'><ul class='pagination'>";
            var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";
            if (currentPage > 1) {
                prevPage = "<li ><a style='cursor: pointer;' onclick='PagerLinkClick(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
            }
            pagers += prevPage;
            for (i = 1; i <= pages; i++) {
                if (i == currentPage) {
                    pagers += "<li class='active'><a style='cursor: pointer;' onclick='PagerLinkClick(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                } else {
                    pagers += "<li><a style='cursor: pointer;' onclick='PagerLinkClick(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                }
            }
            var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";
            if (currentPage != pages) {
                nextPage = "<li ><a style='cursor: pointer;' onclick='PagerLinkClick(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
            }
            pagers += nextPage;
            pagers += "</ul><div style='clear:both;'></div></div>";
            //$('#paginator').remove();

            // return html for pagination div
            return pagers;
        }
            // If the number of pages is more than 10:
            // We are going to have three different cases:                
        else {
            if (currentPage <= 6) {
                // Draw the first 10 then have ... link to last
                var pagers = "<div class='pagination-container'><ul class='pagination'>";
                var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";

                if (currentPage > 1) {
                    prevPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                }
                pagers += prevPage;
                var endNum = 6;
                for (i = 1; i <= 6; i++) {
                    if (i == currentPage) {
                        pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    } else {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    }
                }
              //  pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>"
                if (parseInt(pages - 1) != 6) {
                    pagers += "<li><a style='cursor: pointer;' onclick='ShowNextRecordAndUpdatePageLinks(7,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                    pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + parseInt(pages - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + parseInt(pages - 1) + "</a></li>";
                }
                pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + pages + "</a></li>";
                var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";

                if (currentPage != pages) {
                    nextPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                }
                pagers += nextPage;
                pagers += "</ul><div style='clear:both;'></div></div>";

                //$("#paginator").remove();
                return pagers;
            }
            else {
                if (pages <= 12) {
                    // Draw the first 10 then have ... link to last
                    var pagers = "<div class='pagination-container'><ul class='pagination'>";
                   
                    var startNum = pages - 6 + 1;
                    //  pagers += "<li><a style='cursor: pointer;' onclick='ShowFirstRecordAndUpdatePageLinks(1,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>««</a></li>"
                    var prevRecordPageNum = startNum - 1;
                    var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";

                    if (currentPage > 1) {
                        prevPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                    }
                    pagers += prevPage;
                    if (startNum != 1) {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 1 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 1 + "</a></li>";
                        if (startNum != 2) {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 2 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 2 + "</a></li>";
                        pagers += "<li><a style='cursor: pointer;' onclick='ShowPreviousRecordAndUpdatePageLinks(" + prevRecordPageNum + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                        }

                        }
                    for (i = startNum; i <= pages; i++) {
                        if (i == currentPage) {
                            pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        } else {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        }
                    }
                    var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";

                    if (currentPage != pages) {
                        nextPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                    }
                    pagers += nextPage;
                    pagers += "</ul><div style='clear:both;'></div></div>";
                    //$("#paginator").remove();
                    return pagers;
                }
                else {
                    // first ... link and last ...link
                    var pagers = "<div class='pagination-container'><ul class='pagination'>";
                    var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";

                    if (currentPage > 1) {
                        prevPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                    }
                    pagers += prevPage;
                    var startNum = (Math.floor((currentPage - 1) / 6) * 6) + 1;
                    var endNum = startNum + 6 - 1;

                    if (endNum > pages) {
                        endNum = pages;
                    }
                    // pagers += "<li><a style='cursor: pointer;' onclick='ShowFirstRecordAndUpdatePageLinks(1,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>««</a></li>"
                    if(startNum != 1){
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 1 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 1 + "</a></li>";
                        if (startNum != 2) {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 2 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 2 + "</a></li>";
                     pagers += "<li><a style='cursor: pointer;' onclick='ShowPreviousRecordAndUpdatePageLinks(" + (startNum - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                        }

                        }
                for (i = startNum; i <= endNum; i++) {
                        if (i == currentPage) {
                            pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        } else {
                            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                        }
                    }
                    if (endNum < pages) {
                      //  pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>";
                        if (endNum != (parseInt(pages - 1))) {
                            pagers += "<li><a style='cursor: pointer;' onclick='ShowNextRecordAndUpdatePageLinks(" + (endNum + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                           pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(pages - 1)) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + parseInt(pages - 1) + "</a></li>";
                       }
                       pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + pages + "</a></li>";

                    }
                    var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";

                    if (currentPage != pages) {
                        nextPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                    }
                    pagers += nextPage;
                    pagers += "</ul><div style='clear:both;'></div></div>";
                    //$("#paginator").remove();
                    return pagers;
                }
            }
        }
    }
}

function ShowNextRecordAndUpdatePageLinks(NextRecordPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection) {
    updatePage(NextRecordPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection);
}

function ShowPreviousRecordAndUpdatePageLinks(PrevRecordPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection) {
    updatePage(PrevRecordPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection);
}

function ShowFirstRecordAndUpdatePageLinks(index, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection) {
    updatePage(index, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection);
}

function ShowLastRecordAndUpdatePageLinks(lastPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection) {
    updatePage(lastPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection);
}

function updatePage(clickedPageNumber, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection) {
    $(hdnCurrentPageNumberId).val(clickedPageNumber);
    this[dataBindingMethodName](clickedPageNumber, sortParameter, sortDirection);
    return false;
}

function PagerLinkClick(index, dataBindingMethodName, hdnCurrentPageNumberId, sortParameter, sortDirection) {
    $(hdnCurrentPageNumberId).val(index);
    this[dataBindingMethodName](index, sortParameter, sortDirection);
    $(".pagor").removeClass("selected");
    $(this).addClass("selected");
}
function displayLinksForSmallContainers(currentPage, pages, sortParameter, sortDirection, dataBindingMethodName, hdnCurrentPageNumberId) {
    //If the number of pages is less than 10: Always display links 1 2 3 4..... When someone clicks on 1 2 3 4 5 6.... Get the index of the clicked on element,
    //Switch the selected class to the clicked on element and load page data according to the index of the clicked on elemen
    if (pages == 1 || pages == 0) {
        var pagers = "<div class='pagination-container'><ul class='pagination'></ul><div style='clear:both;'></div></div>";
        //$("#paginator").remove();
        return pagers;
    }
    if (pages <= 3) {
        var pagers = "<div class='pagination-container'><ul class='pagination'>";
        var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";
        if (currentPage > 1) {
            pagers += "<li ><a style='cursor: pointer;' onclick='PagerLinkClick(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
        }else 
            pagers += prevPage;
        for (i = 1; i <= pages; i++) {
            if (i == currentPage) {
                pagers += "<li class='active'><a style='cursor: pointer;' onclick='PagerLinkClick(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
            } else {
                pagers += "<li><a style='cursor: pointer;' onclick='PagerLinkClick(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
            }
        }
        var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";
        if (currentPage != pages) {
            pagers += "<li ><a style='cursor: pointer;' onclick='PagerLinkClick(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
        }
        else
            pagers += nextPage;
        pagers += "</ul><div style='clear:both;'></div></div>";
        //$('#paginator').remove();

        // return html for pagination div
        return pagers;
    }
        // If the number of pages is more than 10:
        // We are going to have three different cases:                
    else {
        if (currentPage <= 3) {
            // Draw the first 5 then have ... link to last
            var pagers = "<div class='pagination-container'><ul class='pagination'>";
            var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";
            if (currentPage > 1) {
                pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
            }
            else
                pagers += prevPage;
            for (i = 1; i <= 3; i++) {
                if (i == currentPage) {
                    pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                } else {
                    pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                }
            }

            if (parseInt(pages - 1) != 3) {
                pagers += "<li><a style='cursor: pointer;' onclick='ShowNextRecordAndUpdatePageLinks(4,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + parseInt(pages - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + parseInt(pages - 1) + "</a></li>";
            }
            pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + pages + "</a></li>";
            var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";
            if (currentPage != pages) {
                pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
            }
            // pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>"
            else
                pagers += nextPage;
            pagers += "</ul><div style='clear:both;'></div></div>";
            
            //$("#paginator").remove();
            return pagers;
        }
        else {
            if (pages <= 6) {
                // Draw the first 5 then have ... link to last
                var pagers = "<div class='pagination-container'><ul class='pagination'>";

                var startNum = pages - 3 + 1;

                // pagers += "<li><a style='cursor: pointer;' onclick='ShowFirstRecordAndUpdatePageLinks(1,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>««</a></li>"
                var prevRecordPageNum = startNum - 1;
                var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";
                if (currentPage > 1) {
                    pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                }
                else
                    pagers += prevPage;
                if (startNum != 1) {
                    pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 1 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 1 + "</a></li>";
                    if (startNum != 2) {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 2 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 2 + "</a></li>";
                        pagers += "<li><a style='cursor: pointer;' onclick='ShowPreviousRecordAndUpdatePageLinks(" + prevRecordPageNum + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                    }
                } for (i = startNum; i <= startNum + 2; i++) {
                    if (i == currentPage) {
                        pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    } else {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    }
                }
                var nextPage = "<li ><a style='cursor: default;' disabled>Next</a></li>";

                if (currentPage != pages) {
                    pagers += "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                }
                //  pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>"
                else
                    pagers += nextPage;

                pagers += "</ul><div style='clear:both;'></div></div>";
                //$("#paginator").remove();
                return pagers;
            }
            else {
                // first ... link and last ...link
                var pagers = "<div class='pagination-container'><ul class='pagination'>";
                var startNum = (Math.floor((currentPage - 1) / 2) * 2) + 1;
                var endNum = startNum + 2 - 1;

                if (endNum > pages) {
                    endNum = pages;
                }
                var prevPage = "<li ><a style='cursor: default;' disabled>Prev</a></li>";
                //  pagers += "<li><a style='cursor: pointer;' onclick='ShowFirstRecordAndUpdatePageLinks(1,\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>««</a></li>"
                if (currentPage > 1) {
                    prevPage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Prev</a></li>";
                    pagers += prevPage;
                }
                if (startNum != 1) {
                    pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 1 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 1 + "</a></li>";
                    if (startNum != 2) {
  //                      pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + 2 + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + 2 + "</a></li>";
                        pagers += "<li><a style='cursor: pointer;' onclick='ShowPreviousRecordAndUpdatePageLinks(" + (startNum - 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
                    }
                } for (i = startNum; i <= endNum; i++) {
                    if (i == currentPage) {
                        pagers += "<li class='active'><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    } else {
                        pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + i + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + i + "</a></li>";
                    }
                }

                if (endNum < pages) {

                    // pagers += "<li><a style='cursor: pointer;' onclick='ShowLastRecordAndUpdatePageLinks(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>»»</a></li>";
                    if (endNum != (parseInt(pages - 1))) {
                        pagers += "<li><a style='cursor: pointer;' onclick='ShowNextRecordAndUpdatePageLinks(" + (endNum + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>...</a></li>";
           //             pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(pages - 1)) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + parseInt(pages - 1) + "</a></li>";
                    }
                    pagers += "<li><a style='cursor: pointer;' onclick='updatePage(" + pages + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>" + pages + "</a></li>";
                }
                var nextpage = "<li ><a style='cursor: default;' disabled>Next</a></li>";
                if (currentPage != pages) {
                    nextpage = "<li ><a style='cursor: pointer;' onclick='updatePage(" + (parseInt(currentPage) + 1) + ",\"" + dataBindingMethodName + "\",\"" + hdnCurrentPageNumberId + "\",\"" + sortParameter + "\",\"" + sortDirection + "\")'>Next</a></li>";
                }
                    pagers += nextpage;
                pagers += "</ul><div style='clear:both;'></div></div>";
                //$("#paginator").remove();
                return pagers;
            }
        }
    }   
}