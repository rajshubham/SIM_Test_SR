/*
======================================
Name of Plugin :: Intellensene
Author         :: Raghav
Created Date   :: 05 June 2015 
=====================================
*/


(function ($) {
    $.fn.multiintellisense = function (options) {
        //return this.each(function () {        
        var settings = $.extend({
            top: "33",
            maxheight: '400',
            width: '',
            textBoxId: '',
            url: '',
            actionOnSelect: '',
            maxItems: 8,
            actionOnEnter: null,
            IsBothActionsSame: false,
        }, options);
        var privateVar = {
            enterkey: 13,
            upkey: 38,
            downkey: 40,
        };
        //Register key up event
        if ($("#" + this.selector + "").prop("tagName").toLowerCase() == 'input') {
            settings.textBoxId = this.selector;

            //$("#" + this.selector + "").blur(function (e) {
            //    
            //   removeEventListener
            //});
            $("#" + this.selector + "").keyup(function (e) {
                if (e.target.value == "" && e.which != privateVar.enterkey) {
                    methods.RemoveExistingElements();
                }
                else {
                    switch (e.which) {
                        case privateVar.enterkey:
                            methods.OnEnter(e);
                            break;
                        case privateVar.upkey:
                            methods.OnKeyUp(e);
                            break;
                        case privateVar.downkey:
                            methods.OnKeyDown(e);
                            break;
                        default:
                            methods.GetData(e);
                            break;
                    }
                }

            });
        }


        var methods = {

            GetSelectedText: function () {

                var selectedElement = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected");
                var isSelectedElementHeader = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected").attr('data-IsHeader');
                  
                if (selectedElement.length > 0) {
                    if (isSelectedElementHeader == "false") {
                        $("#" + settings.textBoxId).val(selectedElement.text());
                    }
                }
                methods.RemoveExistingElements();
            },

            FillSelectedText: function (text) {
                $("#" + settings.textBoxId).val(text);
                methods.RemoveExistingElements();

            },

            TriggerOnClick: function () {
                var selectedElement = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected");
                var isSelectedElementHeader = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected").attr('data-IsHeader');
                if (selectedElement.length > 0) {
                    if (isSelectedElementHeader == "false") {
                        $("#" + settings.textBoxId).val(selectedElement.text());
                    }
                    selectedElement.click();
                }
                // methods.RemoveExistingElements();
            },

            GetData: function (e) {
                $.ajax({
                    type: 'post',
                    url: settings.url,
                    data: { text: e.target.value },
                    async: false,
                    success: function (response) {
                        methods.RemoveExistingElements();
                        if (typeof (response) != "undefined") {
                            methods.CreateElemets(response);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {multi-div-Intellisense
                        methods.RemoveExistingElements();
                    }
                });
            },

            CreateElemets: function (source) {
                if (settings.actionOnSelect != '') {
                    var intellisenseContainer = "";
                    if (settings.width == '') {
                        intellisenseContainer = "<div class=\"col-md-11 col-sm-11 col-xs-12 multi-intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px\"  id=\"" + settings.textBoxId + "-multi-div-Intellisense\">" +
                                                 "<ul id=\"" + settings.textBoxId + "-multi-ui-Intellisense\" class=\"multi-intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\">";
                    }
                    else {
                        intellisenseContainer = "<div class=\"multi-intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px;width:" + settings.width + "%\"  id=\"" + settings.textBoxId + "-multi-div-Intellisense\">" +
                                                 "<ul id=\"" + settings.textBoxId + "-multi-ui-Intellisense\" class=\"multi-intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\">";

                    }
                    $.each(source, function (key, value) {
                        if (value.Children.length > 0) {
                            intellisenseContainer += "<li  class='multi-intellisense-li' id=\"" + settings.textBoxId + "-" + key + "\" data-IsHeader=\"true\" onclick=" + settings.actionOnSelect + "(true,'" + value.HeaderId + "','" + encodeURIComponent(value.Header) + "',0)><b>" + value.Header + "</b></li>";
                            $.each(value.Children, function (childKey, childValue) {
                                intellisenseContainer += "<li style=\"padding-left:20px;\" class='multi-intellisense-li' id=\"" + settings.textBoxId + "-" + childKey + "\" data-IsHeader=\"false\" onclick=" + settings.actionOnSelect + "(false,'" + value.HeaderId + "','" + encodeURIComponent(childValue.Value) + "','" + childValue.Id + "')>" + childValue.Value + "</li>";
                            });
                            }
                    });
                    intellisenseContainer += "</ul></div>";
                    $("#" + settings.textBoxId + "").after(intellisenseContainer);
                    $('.multi-intellisense-li').click(function (e) {
                        var isHeader = $(this).attr('data-IsHeader');
                        if (isHeader == "false") {
                            methods.FillSelectedText($(this).text());
                        }
                    });
                }
                else if (source.length > 0) {
                    var intellisenseContainer = "";
                    if (settings.width == '') {
                        intellisenseContainer = "<div class=\"col-md-11 col-sm-11 col-xs-12 multi-intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px\" id=\"" + settings.textBoxId + "-multi-div-Intellisense\">" +
                                            "<ul id=\"" + settings.textBoxId + "-multi-ui-Intellisense\" class=\"multi-intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\" >";
                    }
                    else {
                        intellisenseContainer = "<div class=\"multi-intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px;width:" + settings.width + "%\" id=\"" + settings.textBoxId + "-multi-div-Intellisense\">" +
                                          "<ul id=\"" + settings.textBoxId + "-multi-ui-Intellisense\" class=\"multi-intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\" >";

                    }
                    //TODO :: Set maximtes
                    var maxItems = source.length > settings.maxItems ? settings.maxItems : source.length;
                    
                    $.each(source, function (key, value) {
                        if (value.Children.length > 0) {
                            intellisenseContainer += "<li  class='multi-intellisense-li' id=\"" + settings.textBoxId + "-" + key + "\" data-IsHeader=\"true\"><b>" + value.Header + "</b></li>";

                            $.each(value.Children, function (childKey, childValue) {
                                intellisenseContainer += "<li style=\"padding-left:20px;\" class='multi-intellisense-li' id=\"" + settings.textBoxId + "-" + childKey + "\" data-IsHeader=\"false\">" + childValue.Value + "</li>";
                            });
                        }
                    });
                    intellisenseContainer += "</ul></div>";
                    $("#" + settings.textBoxId + "").after(intellisenseContainer);
                    $('.multi-intellisense-li').click(function (e) {
                        var isHeader = $(this).attr('data-IsHeader');
                        if (isHeader == "false") {
                            methods.FillSelectedText($(this).text());
                        }
                    });
                }
            },

            RemoveExistingElements: function () {
                $("#" + settings.textBoxId + "-multi-div-Intellisense").remove();
            },

            OnKeyUp: function (e) {
                var selectedElement = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected");
                var text = e.target.value;
                if (selectedElement.length > 0) {
                    var selectedElement = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected");
                    text = selectedElement.prev().text();
                    selectedElement.prev().addClass('selected');
                    selectedElement.removeClass('selected');
                }
                else {
                    $("#" + settings.textBoxId + "-multi-ui-Intellisense li:first").addClass('selected');
                    text = $("#" + settings.textBoxId + "-multi-ui-Intellisense li:first").text();
                }
                var isSelectedElementHeader = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected").attr('data-IsHeader');
                if (isSelectedElementHeader == "false") {
                    $("#" + settings.textBoxId + "").val(text);
                }

            },

            OnKeyDown: function (e) {
                var selectedElement = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected");
                var text = e.target.value;
                if (selectedElement.length > 0) {
                    var selectedElement = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected");
                    text = selectedElement.next().text();
                    selectedElement.next().addClass('selected');
                    selectedElement.removeClass('selected');
                }
                else {
                    $("#" + settings.textBoxId + "-multi-ui-Intellisense li:first").addClass('selected');
                    text = $("#" + settings.textBoxId + "-multi-ui-Intellisense li:first").text();
                }
                var isSelectedElementHeader = $("#" + settings.textBoxId + "-multi-ui-Intellisense li.selected").attr('data-IsHeader');
                if (isSelectedElementHeader == "false") {
                    $("#" + settings.textBoxId + "").val(text);
                }
            },

            OnEnter: function (e) {
                if (!settings.IsBothActionsSame) {
                    if ($.isFunction(settings.actionOnEnter)) {
                        methods.RemoveExistingElements();
                        settings.actionOnEnter.call(this);
                    }
                    else {
                        methods.GetSelectedText();
                    }
                }
                else {
                    methods.TriggerOnClick();

                }
            }
        }
    }
}(jQuery));


