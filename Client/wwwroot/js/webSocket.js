///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />

import { displayMessage } from "./modules/messenger.js";

var connectionStatus = $("#connectionStatus");
var messageHistory = $("#messageHistory");

$(function () {
    var url = new URL("wss://localhost:7061/ws");
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
    var message = JSON.parse(event.data);
    console.debug("Получено сообщение:", message);

    displayMessage(message, messageHistory);
}

function onError(event) {
    console.error("Произошла ошибка:", event);
    connectionStatus.text("Не удалось подключиться к серверу");
};
