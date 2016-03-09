/*
======================================
Name of Plugin :: Intellensene
Author         :: Naveen Motwani
Created Date   :: 05 June 2015 
=====================================
*/


(function ($) {
    $.fn.intellisense = function (options) {
        //return this.each(function () {        
        var settings = $.extend({
            top: "33",
            maxheight: '350',
            width:'',
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
                
                var selectedElement = $("#" + settings.textBoxId + "UlIntellisense li.selected");
                if (selectedElement.length > 0) {
                    $("#" + settings.textBoxId).val(selectedElement.text());
                }
                methods.RemoveExistingElements();
            },

            FillSelectedText: function (text) {
                $("#" + settings.textBoxId).val(text);
                methods.RemoveExistingElements();

            },

            TriggerOnClick: function () {
                    var selectedElement = $("#" + settings.textBoxId + "UlIntellisense li.selected");
                    if (selectedElement.length > 0) {
                        $("#" + settings.textBoxId).val(selectedElement.text());
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
                    error: function (jqXHR, textStatus, errorThrown) {
                        methods.RemoveExistingElements();
                    }
                });
            },

            CreateElemets: function (source) {
                if (settings.actionOnSelect != '') {
                    var intellisenseContainer = "";
                    if (settings.width == '') {
                        intellisenseContainer =  "<div class=\"col-md-11 col-sm-11 col-xs-12 intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px\"  id=\"" + settings.textBoxId + "DivIntellisense\">" +
                                                 "<ul id=\"" + settings.textBoxId + "UlIntellisense\" class=\"intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\">";
                    }
                    else {
                       intellisenseContainer = "<div class=\"intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px;width:"+ settings.width+"%\"  id=\"" + settings.textBoxId + "DivIntellisense\">" +
                                                "<ul id=\"" + settings.textBoxId + "UlIntellisense\" class=\"intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\">";

                    }
                    $.each(source, function (key, value) {
                        
                        //console.log(key + "" + value);
                        intellisenseContainer += "<li class='intellisense-li' id=\""+settings.textBoxId+"-"+key+"\" onclick=" + settings.actionOnSelect + "('" + key + "')>" + value + "</li>";
                    });
                    intellisenseContainer += "</ul></div>";
                    $("#" + settings.textBoxId + "").after(intellisenseContainer);
                    $('.intellisense-li').click(function (e) {
                        methods.FillSelectedText($(this).text());
                    });
                }
                else if (source.length > 0) {
                    var intellisenseContainer = "";
                    if (settings.width == '') {
                        intellisenseContainer = "<div class=\"col-md-11 col-sm-11 col-xs-12 intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px\" id=\"" + settings.textBoxId + "DivIntellisense\">" +
                                            "<ul id=\"" + settings.textBoxId + "UlIntellisense\" class=\"intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\" >";
                    }
                    else {
                        intellisenseContainer = "<div class=\"intellisense-container\" style=\"top:" + settings.top + "px;max-height:" + settings.maxheight + 20 + "px;width:" + settings.width + "%\" id=\"" + settings.textBoxId + "DivIntellisense\">" +
                                          "<ul id=\"" + settings.textBoxId + "UlIntellisense\" class=\"intellisense-ul\" style=\"maxheight:" + settings.maxheight + "px;\" >";

                    }
                    //TODO :: Set maximtes
                    var maxItems = source.length > settings.maxItems ? settings.maxItems : source.length;
                    for (var i = 0; i < maxItems; i++) {
                        intellisenseContainer += "<li style='' class='intellisense-li'>" + source[i] + "</li>";
                    }

                    intellisenseContainer += "</ul></div>";
                    $("#" + settings.textBoxId + "").after(intellisenseContainer);
                    $('.intellisense-li').click(function (e) {
                        

                        methods.FillSelectedText($(this).text());
                    });
                }
            },

            RemoveExistingElements: function () {
                $("#" + settings.textBoxId + "DivIntellisense").remove();
            },

            OnKeyUp: function (e) {
                var selectedElement = $("#" + settings.textBoxId + "UlIntellisense li.selected");
                var text = e.target.value;
                if (selectedElement.length > 0) {
                    var selectedElement = $("#" + settings.textBoxId + "UlIntellisense li.selected");
                    text = selectedElement.prev().text();
                    selectedElement.prev().addClass('selected');
                    selectedElement.removeClass('selected');
                }
                else {
                    $("#" + settings.textBoxId + "UlIntellisense li:first").addClass('selected');
                    text = $("#" + settings.textBoxId + "UlIntellisense li:first").text();
                }
                $("#" + settings.textBoxId + "").val(text);

            },

            OnKeyDown: function (e) {
                var selectedElement = $("#" + settings.textBoxId + "UlIntellisense li.selected");
                var text = e.target.value;
                if (selectedElement.length > 0) {
                    var selectedElement = $("#" + settings.textBoxId + "UlIntellisense li.selected");
                    text = selectedElement.next().text();
                    selectedElement.next().addClass('selected');
                    selectedElement.removeClass('selected');
                }
                else {
                    $("#" + settings.textBoxId + "UlIntellisense li:first").addClass('selected');
                    text = $("#" + settings.textBoxId + "UlIntellisense li:first").text();
                }
                $("#" + settings.textBoxId + "").val(text);
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


