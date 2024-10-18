    var url = new URL("wss://localhost:7061/ws");
﻿///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />
var connectionStatus = $("#connectionStatus");
var messageHistory = $("#messageHistory");

$(function () {
    var webSocket = new WebSocket(url);
    
    webSocket.onopen = onOpen;
    webSocket.onmessage = onMessage;
    webSocket.onerror = onError;

    console.log("Подключение к серверу...");
    connectionStatus.text("Попытка подключения к серверу...");
});

function onOpen() {
    console.log("Подключение установлено.");
    connectionStatus.text("Подключение активно");
}

function onMessage(event) {
    var messageData = JSON.parse(event.data);
    console.log(messageData);

    var message = $("<div></div>").addClass(["card", "mb-2"]);
    var messageHeader = $("<div></div>").addClass("card-header").text(`${new Date(messageData.MessageTimestamp).toLocaleString(undefined, {})} \u2013 ${messageData.MessageIndex}`);
    var messageBody = $("<div></div>").addClass("card-body").text(`${messageData.MessageText}`);

    message.append(messageHeader).append(messageBody).hide().fadeIn(500);
    messageHistory.append(message);
}

function onError(event) {
    console.error("Произошла ошибка:", event);
    connectionStatus.text("Не удалось подключиться к серверу.");
};
