$(document).ready(function () {
    supplierHome();
    if (registerMessage != null && registerMessage != "") {
        showSuccessMessage(registerMessage);
    }
});

function supplierHome() {
    $('#supplierHome').show();
    $('#flagged-answers').hide();
    //if (Common.IsPassWordChanged) {
    //    showSuccessMessage(passwordChangedSuccessfully);
    //    Common.IsPassWordChanged = false;
    //}
    var chartColor = '#709B6B';
    SelectMenu("SupplierHomeTab");
    StartLoding();
    $.ajax({
        type: "POST",
        url: "/Supplier/GetSellerProfilePercentage",
        success: function (response) {
            if (response != null && typeof (response) != "undefined") {
                if (response == Common.Logout) {
                    Logout();
                }
                else {
                    var totalPercentage = parseInt(response.TotalScore);
                    $("#GeneralInfoProgressChart").html('');
                    if (totalPercentage < 30) {
                        chartColor = '#8A2529';
                    }
                    else if (totalPercentage > 30 && totalPercentage < 70) {
                        chartColor = '#E7A614';
                    }
                    else {
                        chartColor = '#709B6B';
                    }
                    if (!IsOldBrowser()) {
                        $("#GeneralInfoProgressChart").drawDoughnutChart([
                        { title: "General information & contacts", value: totalPercentage, color: chartColor },
                        { title: "None", value: (100 - totalPercentage), color: "#D2D2D2" },
                        ], [{ width: 160, height: 160 }]);
                        $('#GeneralInfoProgressChart .doughnutSummaryNumber').html("<span class='chart-per'>" + totalPercentage + "%</span></br><span class='chart-text'>" + answered + "</span>");
                    }
                    else {
                        var s1 = [['General information & contacts', totalPercentage], ['None', (100 - totalPercentage)]];
                        var plot3 = $.jqplot('GeneralInfoProgressChart', [s1],
                            {
                                grid: { drawBorder: false, shadow: false },
                                seriesDefaults: {
                                    // make this a donut chart.
                                    seriesColors: [chartColor, '#D2D2D2'],
                                    renderer: $.jqplot.DonutRenderer,
                                    rendererOptions: {
                                        // Donut's can be cut into slices like pies.
                                        sliceMargin: 0,
                                        // Pies and donuts can start at any arbitrary angle.
                                        startAngle: 0,
                                        showDataLabels: false,
                                        // By default, data labels show the percentage of the donut/pie.
                                        // You can show the data 'value' or data 'label' instead.
                                        dataLabels: 'value',
                                        diameter: 120,
                                        shadowAlpha: 0, thickness: 16
                                    }
                                }
                            });
                        $('#GeneralInfoProgressChart').append("<div class='doughnutSummaryNumber'  style='top:46px !important;'><span class='chart-per'>" + totalPercentage + "% </span></br><span class='chart-text'>" + answered + "</span></div>");

                    }
                }
            }
        },
        error: function (response) {
        }
    });
    //$.ajax({
    //    type: "POST",
    //    url: "/Supplier/GetSummary",
    //    success: function (response) {
    //        if (response != null && typeof (response) != "undefined") {
    //            if (response == Common.Logout) {
    //                Logout();
    //            }
    //            else {
    //                var requiredSections = response.required.length;
    //                var submittedCount = 0;
    //                var awaitingPayment = 0;
    //                var awaitingPaymentAndIsRequired = 0;
    //                var textcomplianceInfoAboveDonut = "";
    //                var textcomplianceInfoBelowDonut = "";
    //                if (response.supplierProducts != "undefined" && response.supplierProducts.length > 0) {
    //                    for (var i = 0; i < response.supplierProducts.length; i++) {
    //                        if (response.supplierProducts[i].ProductId != 3) {
    //                            if ($.inArray(response.supplierProducts[i].ProductId, response.required) > -1 && response.supplierProducts[i].Status >= 2)
    //                            { submittedCount++; }
    //                            if (response.supplierProducts[i].Status == 1)
    //                            { awaitingPayment++; }
    //                            if ($.inArray(response.supplierProducts[i].ProductId, response.required) > -1 && response.supplierProducts[i].Status == 1)
    //                            { awaitingPaymentAndIsRequired++; }
    //                        }
    //                    }
    //                }
    //                var notSubmitted = requiredSections - submittedCount;

    //                $('#required-sections-supplier-banner').html(notSubmitted);
    //                $('#awaiting-payment-supplier-banner').html(awaitingPayment);
    //                $('#flagged-answers-supplier-banner').html(response.flaggedAnswerCount);
    //                $('#inbox-unread-supplier-banner').html(response.unReadCount);
    //                $('#awaiting-verify-supplier-banner').html(response.verifiedCount);

    //                if (notSubmitted == 1)
    //                    $('#required-sections-supplier-banner-text').html(reqdSection + '<br />' + needingAction);
    //                else
    //                    $('#required-sections-supplier-banner-text').html(reqdSections + '<br />' + needingAction);

    //                if (awaitingPayment == 1)
    //                    $('#awaiting-payment-supplier-banner-text').html(section);
    //                else
    //                    $('#awaiting-payment-supplier-banner-text').html(sections);
    //                if (response.verifiedCount == 1)
    //                    $('#awaiting-verify-supplier-banner-text').html(section);
    //                else
    //                    $('#awaiting-verify-supplier-banner-text').html(sections);
    //                if (response.flaggedAnswerCount == 1)
    //                    $('#flagged-answers-supplier-banner-text').html(flaggedAnswer);
    //                else
    //                    $('#flagged-answers-supplier-banner-text').html(flaggedAnswers);
    //                if (response.unReadCount == 1)
    //                    $('#inbox-unread-supplier-banner-text').html(unreadMessage);
    //                else
    //                    $('#inbox-unread-supplier-banner-text').html(unreadMessages);

    //                if (notSubmitted > 0) {
    //                    if (notSubmitted == 1) {
    //                        textcomplianceInfoAboveDonut = "<label class='submission-status color-red'>" + notSubmitted + " " + reqdSection + " " + needingAction + "</label>";
    //                    }
    //                    else {
    //                        textcomplianceInfoAboveDonut = "<label class='submission-status color-red'>" + notSubmitted + " " + reqdSections + " " + needingAction + "</label>";
    //                    }
    //                }
    //                else {
    //                    textcomplianceInfoAboveDonut = "<label class='submission-status'>" + noReqdSectionsNeedingAction + "</label>";

    //                    //TO:DO
    //                    // Ask paul whether we need to hide required label section or not if there are no required sections.
    //                    //if (requiredSections > 0) {
    //                    //    textcomplianceInfoAboveDonut = "<label class='submission-status'>No required sections needing action</label>";
    //                    //}
    //                    //else
    //                    //{
    //                    //    $('#divEmpty').show();
    //                    //    $('#complianceInfoAboveDonut').hide();
    //                    //}
    //                }


    //                //if (requiredSections > 0) {
    //                //    if (awaitingPaymentAndIsRequired > 0) {
    //                //        textcomplianceInfoAboveDonut = "<label class='submission-status color-red'>" + awaitingPaymentAndIsRequired + " required sections needing action </label>";
    //                //    }
    //                //    else if (awaitingPaymentAndIsRequired == 0) {
    //                //        textcomplianceInfoAboveDonut = "<label class='submission-status'>No required sections needing action</label>";
    //                //    }
    //                //}
    //                //else {
    //                //    $('#divEmpty').show();
    //                //    $('#complianceInfoAboveDonut').hide();
    //                //}

    //                //if (awaitingPayment > 0) {
    //                //    textcomplianceInfoBelowDonut = "<label class='submission-status color-red'>" + awaitingPayment + " section(s) awaiting payment</label>";
    //                //}
    //                //else {
    //                //    textcomplianceInfoBelowDonut = "<label class='submission-status'>Payment completed for all sections</label>";
    //                //}
    //                $('#complianceInfoAboveDonut').html(textcomplianceInfoAboveDonut);
    //                $('#complianceInfoBelowDonut').html(textcomplianceInfoBelowDonut);
    //                //alert(notSubmitted + " required sections not submitted</br>" + awaitingPayment + " awaiting payment");
    //                var riskPercentage = parseInt((parseInt(response.summary.FITPercentage) + parseInt(response.summary.HSPercentage) + parseInt(response.summary.DSPercentage)) / 3);
    //                if (riskPercentage < 30) {
    //                    chartColor = '#8A2529';
    //                }
    //                else if (riskPercentage > 30 && riskPercentage < 70) {
    //                    chartColor = '#E7A614';
    //                }
    //                else {
    //                    chartColor = '#709B6B';
    //                }

    //                $("#HomeRiskChart").html('');
    //                //$("#divRiskPercentage").html(riskPercentage + "% Completed");
    //                //$("#divRiskPercentageBar").css("width", riskPercentage + "%");
    //                if (!IsOldBrowser()) {

    //                    $("#HomeRiskChart").drawDoughnutChart([
    //                        { title: questionnaireComplete, value: riskPercentage, color: chartColor },
    //                        { title: questionnairePending, value: (100 - riskPercentage), color: "#D2D2D2" },
    //                    ], [{ width: 160, height: 160 }]);
    //                    $('#HomeRiskChart .doughnutSummaryNumber').html("<span class='chart-per'>" + riskPercentage + "% </span></br><span class='chart-text'>" + answered + "</span>");
    //                }
    //                else {
    //                    var s1 = [[questionnaireComplete, riskPercentage], [questionnairePending, (100 - riskPercentage)]];
    //                    var plot3 = $.jqplot('HomeRiskChart', [s1], {
    //                        grid: { drawBorder: false, shadow: false },
    //                        seriesDefaults: {
    //                            // make this a donut chart.
    //                            seriesColors: [chartColor, '#D2D2D2'],
    //                            renderer: $.jqplot.DonutRenderer,
    //                            rendererOptions: {
    //                                // Donut's can be cut into slices like pies.
    //                                sliceMargin: 0,
    //                                // Pies and donuts can start at any arbitrary angle.
    //                                startAngle: 0,
    //                                showDataLabels: false,
    //                                // By default, data labels show the percentage of the donut/pie.
    //                                // You can show the data 'value' or data 'label' instead.
    //                                dataLabels: 'value',
    //                                diameter: 120,
    //                                shadowAlpha: 0, thickness: 16
    //                            }
    //                        }
    //                    });

    //                    $('#HomeRiskChart').append("<div class='doughnutSummaryNumber' style='top:46px !important;'><span class='chart-per'>" + riskPercentage + "% </span></br><span class='chart-text'>" + answered + "</span></div>");

    //                }
    //                if (riskPercentage == 100 && (response.summary.CompanyStatus == "Submitted" || response.summary.CompanyStatus == "Evaluation" || response.summary.CompanyStatus == "Notified" || response.summary.CompanyStatus == "Published")) {
    //                    $('#RiskQuestionarrieButton').html(goToQuestionnaire);
    //                }
    //                else {
    //                    $("#RiskQuestionarrieButton").html(goToQuestionnaire);
    //                }
    //            }
    //        }
    //    },
    //    error: function (response) {
    //    }
    //});
    //$.ajax({
    //    type: "POST",
    //    url: "/Supplier/GetClientQuestionSummaryPercentage",
    //    success: function (response) {
    //        GetAllQuestionsetsOfMappedBuyers(response)
    //    },
    //    error: function (response) {
    //    }
    //});
    EndLoading("divProduct");
}


function GetAllQuestionsetsOfMappedBuyers(response) {
    $.ajax({
        type: "POST",
        url: "/Common/GetAllQuestionsetsOfMappedBuyers",
        async: false,
        success: function (Buyers) {
            for (var i = 0; i < Buyers.length; i++) {
                //var buyerID = Buyers[i].BuyerId;
                SetClientQuestionSummary(Buyers[i], response);
            }
        },
        error: function (error) {
        }
    });
}


function SetClientQuestionSummary(buyer, response) {
    if (response != null && typeof (response) != "undefined") {
        if (response == Common.Logout) {
            Logout();
        }
        else {


            var QuestionsetsMappedToBuyer = [];
            for (var j = 0; j < buyer.BuyerCompany.BuyerQuestionSetMappings.length; j++) {
                QuestionsetsMappedToBuyer.push(buyer.BuyerCompany.BuyerQuestionSetMappings[j].QuestionSetId);
            }


            var percentage = response.Summary;
            var count = 0;
            var sum = 0;
            var i;
            var completedCount = 0;
            for (var i = 0 ; i < percentage.length ; i++) {

                if ($.inArray(percentage[i].QuestionSetId, QuestionsetsMappedToBuyer) > -1) {
                    count++;
                    sum += parseInt(percentage[i].Percentage);
                    if (parseInt(percentage[i].Percentage) == 100 && percentage[i].Status == 1) {
                        completedCount += 1;

                    }
                }
            }
            var clientSpecificPercentage = (count > 0) ? parseInt(sum / count) : 0;
            //$("#divClientPercentage").html(clientSpecificPercentage + "% Completed");
            //$("#divClientPercentageBar").css("width", clientSpecificPercentage + "%");

            var textclientInfoAboveDonut = "";

            if (completedCount == count) {
                textclientInfoAboveDonut = "<label class='submission-status'>" + noReqdSectionsNeedingAction + "</label>";
            }
            else {
                textclientInfoAboveDonut = "<label class='submission-status color-red'> " + (count - completedCount) + " " + required + " " + (((count - completedCount) == 1) ? section : sections) + " " + needingAction + "</label>";
            }

            $('#clientInfoAboveDonut-' + buyer.BuyerId).html(textclientInfoAboveDonut);

            $("#ClientComplianceProgressChart-" + buyer.BuyerId).html('');
            if (clientSpecificPercentage < 30) {
                chartColor = '#8A2529';
            }
            else if (clientSpecificPercentage > 30 && clientSpecificPercentage < 70) {
                chartColor = '#E7A614';
            }
            else {
                chartColor = '#709B6B';
            }
            if (!IsOldBrowser()) {

                $("#ClientComplianceProgressChart-" + buyer.BuyerId).drawDoughnutChart([
                    { title: "Questionnaire Complete", value: clientSpecificPercentage, color: chartColor },
                    { title: "Questionnaire Pending", value: (100 - clientSpecificPercentage), color: "#D2D2D2" },
                ], [{ width: 160, height: 160 }]);
                $("#ClientComplianceProgressChart-" + buyer.BuyerId + " .doughnutSummaryNumber").html("<span class='chart-per'>" + clientSpecificPercentage + "% </span></br><span class='chart-text'>" + answered + "</span>");
            }
            else {
                if ($('#ClientComplianceProgressChart-' + buyer.BuyerId).length > 0) {
                    var s1 = [['Questionnaire Complete', clientSpecificPercentage], ['Questionnaire Pending', (100 - clientSpecificPercentage)]];
                    var plot3 = $.jqplot('ClientComplianceProgressChart-' + buyer.BuyerId, [s1], {
                        grid: { drawBorder: false, shadow: false },
                        seriesDefaults: {
                            // make this a donut chart.
                            seriesColors: [chartColor, '#D2D2D2'],
                            renderer: $.jqplot.DonutRenderer,
                            rendererOptions: {
                                // Donut's can be cut into slices like pies.
                                sliceMargin: 0,
                                // Pies and donuts can start at any arbitrary angle.
                                startAngle: 0,
                                showDataLabels: false,
                                // By default, data labels show the percentage of the donut/pie.
                                // You can show the data 'value' or data 'label' instead.
                                dataLabels: 'value',
                                diameter: 120,
                                shadowAlpha: 0, thickness: 16
                            }
                        }
                    });

                    $('#ClientComplianceProgressChart-' + buyer.BuyerId).append("<div class='doughnutSummaryNumber' style='top:46px !important;'><span class='chart-per'>" + clientSpecificPercentage + "% </span></br><span class='chart-text'>" + answered + "</span></div>");

                }
            }
            if (clientSpecificPercentage == 100 && (response.Status == 'Submitted' || response.Status == 'Evaluated')) {
                $("#ClientQuestionarrieButton").html(goToQuestionnaire);
            }
            else {
                $("#ClientQuestionarrieButton").html(goToQuestionnaire);
            }
            //if (LV.companyStatus == "Submitted") {
            //    if (LV.isAuditor == true) {
            //        $(".questionnaire").html("Continue Evaluation").show();
            //        if (response.Percentage == 100)
            //            $("#divEvaluationBox").show();
            //    }
            //    else {
            //        //$(".btn").hide();
            //        $(".hdBtn").hide();
            //        $("#divSubmissionBox").show();
            //        $("#divSubmissionSummary").html("You have already submitted your Answers. We will get back to you as soon as we complete auditing.");
            //        $(".questionnaire").html("View Questionnaire").show();
            //    }
            //}
            //else if (LV.companyStatus == "Published") {
            //    $("#divSubmissionBox").hide();
            //    $(".questionnaire").html("View Questionnaire").show();
            //}
            //else {
            //    if (response.Percentage == 100)
            //        $("#divSubmissionBox").show();
            //}
        }
    }
}



function StartLoding(hideDiv) {
    $("#divProduct").hide();
    $("#divLoading").show();
}

$('#btnCloseVerificationMessage').click(function () {
    $('#verificationMessage').html('');
    $('#verificationMessage').hide();
});

function EndLoading(showDiv) {
    $("#" + showDiv + "").show()
    $("#divLoading").hide();
}

$(document).on('click', '.show-flagged-answers', function () {
    $('#supplierHome').hide();
    $('#flagged-answers').show();
    GetFlaggedAnswers(1, "", 1);
});
$(document).on('click', '#export-supplier-flagged-answers', function () {
    window.location.href = "/Supplier/FlaggedAnswersDownload";
});
function GetFlaggedAnswers(pageNo, sortParameter, sortDirection) {
    $('#flagged-answer-table-body').html('');
    $.ajax({
        type: "POST",
        url: "/Supplier/GetFlaggedAnswers",
        data: { pageNo: pageNo },
        success: function (response) {
            if (response != undefined) {
                if (response.total > 0) {
                    var tableHtml = "";
                    for (var i = 0 ; i < response.flaggedList.length; i++) {
                        var item = response.flaggedList[i];
                        var cls = "odd";
                        if (i % 2 == 0) {
                            cls = "even";
                        }
                        var ansUrl = "";
                        var viewName = "";
                        var title = "";
                        if (item.QuestionarrieName == "General Information") {
                            ansUrl = "/supplier/questionnaire/general-info";
                            viewName = "editProfile";
                            title = generalInfoAndContacts;
                        }
                        else if (item.QuestionarrieName == "Compliance Checks") {
                            ansUrl = "/supplier/questionnaire/compliance-checks";
                            viewName = "questionnaire";
                            title = complianceChecks;
                        }
                        tableHtml += "<tr class=\"" + cls + "\"><td>" + item.QuestionarrieName + "</td><td>" + item.SectionName + "</td><td><a class='viewAnswerLink SICCodeLink' data-url=" + ansUrl + " data-section-id=\"" + item.SectionName + "\" data-question-id=" + item.QuestionNumber + " data-viewname=\"" + viewName + "\" data-title=\"" + title + "\">" + item.Question + "</a></td><td style=\"text-align: center\"><button class=\"btn btn-color viewAnswer\" data-url=" + ansUrl + " data-section-id=\"" + item.SectionName + "\" data-question-id=" + item.QuestionNumber + ">" + viewAnswerInNewTab + "</button></td></tr>";
                    }
                    $('#flagged-answer-table-body').html(tableHtml);
                    $('#flagged-answer-table').footable();
                    $('#flagged-answer-table').trigger('footable_redraw');
                }
                else {
                    var tableRow = "<tr><td colspan=\"3\">" + noRecordsFound + "</td></tr>";
                    $('#flagged-answer-table-body').append(tableRow);
                }
                $('#flagged-answer-table').after(displayLinks($('#hdn-flagged-answer-page').val(), Math.ceil(response.total / 10), sortParameter, sortDirection, "GetFlaggedAnswers", "#hdn-flagged-answer-page"));


            }

        }
    });
}

$(document).on('click', '.back-to-supplier-home', function () {
    supplierHome();
});

$(document).on('click', '.viewAnswer', function () {
    var url = "";
    if ($(this).attr('data-url') != undefined && $(this).attr('data-section-id') !== undefined) {
        url = $(this).attr('data-url') + "?section=" + $(this).attr('data-section-id').replace(/ /g, '-').replace(/,/g, '').replace(/&/g, '').replace(/--/g, '-'); //+ "&questionId=" + $(this).attr('data-question-id');
    }
    if (url != "") {
        window.open(url, '_blank');
    }
});

$(document).on('click', '.viewAnswerLink', function () {
    var url = "";
    if ($(this).attr('data-url') != undefined && $(this).attr('data-section-id') !== undefined && $(this).attr('data-question-id') != undefined) {
        url = $(this).attr('data-url') + "?section=" + $(this).attr('data-section-id').replace(/ /g, '-').replace(/,/g, '').replace(/&/g, '').replace(/--/g, '-') + "&questionId=" + $(this).attr('data-question-id');
    }
    var title = $(this).attr('data-title');
    var view = $(this).attr('data-viewname');
    if (title != undefined && view != undefined && url != "") {
        Navigate(view, url, title);
    }
});