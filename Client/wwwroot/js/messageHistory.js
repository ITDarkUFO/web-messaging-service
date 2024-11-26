///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />

import { displayMessages } from "./modules/messenger.js";

var connectionStatus = $("#connectionStatus");
var messageHistory = $("#messageHistory");

$(function () {
    var timeNow = new Date();
    var timeTenMinutesAgo = new Date(timeNow);
    timeTenMinutesAgo.setMinutes(timeTenMinutesAgo.getMinutes() - 10);

    var timeNowISO = timeNow.toISOString();
    var timeTenMinutesAgoISO = timeTenMinutesAgo.toISOString();

    console.debug(`Сообщения показываются с ${timeTenMinutesAgo.toISOString()} по ${timeNow.toISOString()}`);

    var url = new URL(`https://localhost:7061/messages?startDate=${timeTenMinutesAgoISO}&endDate=${timeNowISO}`);

    $.ajax({
        type: "get",
        url: url,
        crossDomain: true,
        beforeSend: function () {
            console.debug("Получение сообщений с сервера...");
            connectionStatus.text("Получение сообщений...");
        },
        success: function (response) {
            console.debug("Обработка сообщений...");
            connectionStatus.text("Загрузка сообщений...");

            if (response == undefined) {
                connectionStatus.text("За последние 10 минут сообщений нет");
                return;
            }

            var messages = JSON.parse(response);
            console.debug(messages);

            displayMessages(messages, messageHistory);

            console.debug("Сообщения загружены");
            connectionStatus.text("Сообщения загружены");
        },
        error: function (response) {
            if (response.readyState == 0) {
                console.error("Произошла ошибка при подключении к серверу.\nПроверьте соединение интернета и попробуйте еще раз.");
                connectionStatus.text("Не удалось подключиться к серверу");
            }
            else {
                console.error(response.responseText);
                connectionStatus.text(response.responseText);
            }
        }
    });
});