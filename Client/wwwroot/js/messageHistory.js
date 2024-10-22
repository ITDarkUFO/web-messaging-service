///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />
var connectionStatus = $("#connectionStatus");
var messageHistory = $("#messageHistory");

$(function () {
    var timeNow = new Date();
    var timeTenMinutesAgo = new Date(timeNow);
    timeTenMinutesAgo.setMinutes(timeTenMinutesAgo.getMinutes() - 10);

    timeNowISO = timeNow.toISOString().split('.')[0];
    timeTenMinutesAgoISO = timeTenMinutesAgo.toISOString().split('.')[0];

    console.log(timeNow.toISOString());
    console.log(timeTenMinutesAgo.toISOString());

    var url = new URL(`https://localhost:7061/messages?startDate=${timeTenMinutesAgoISO}&endDate=${timeNowISO}`);

    $.ajax({
        type: "get",
        url: url,
        crossDomain: true,
        beforeSend: function () {
            connectionStatus.text("Получение сообщений...");
        },
        success: function (response) {
            connectionStatus.text("Загрузка...");

            var messages = JSON.parse(response);
            console.log(messages);

            connectionStatus.text("Сообщения загружены.");
            //var message = $("<div></div>").addClass(["card", "mb-2"]);
            //var messageHeader = $("<div></div>").addClass("card-header").text(`${new Date(response).toLocaleString(undefined, {})} \u2013 ${indexNode.val()}`);
            //var messageBody = $("<div></div>").addClass("card-body").text(`${textNode.val()}`);

            //message.append(messageHeader).append(messageBody).hide().fadeIn(500);
            //messageHistory.append(message);
        },
        error: function (response) {
            if (response.readyState == 0) {
                connectionStatus.text("Не удалось подключиться к серверу.");
                console.error("Произошла ошибка при подключении к серверу.\nПроверьте соединение интернета и попробуйте еще раз.");
            }
            else {
                connectionStatus.text("Произошла неизвестная ошибка.");
                console.log(JSON.parse(response.responseText));
            }
        }
    });
});