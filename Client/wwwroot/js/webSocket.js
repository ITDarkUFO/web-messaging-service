$(function () {
    var url = new URL("wss://localhost:7061/ws");
    var webSocket = new WebSocket(url);
    
    webSocket.onopen = onOpen;
    webSocket.onmessage = onMessage;
    webSocket.onerror = onError;
});

function onOpen() {
    console.log("Connected");
    $("#connectionStatus").text("Подключение активно");
}

function onMessage(event) {
    console.log(event.data);
}

function onError(event) {
    console.error("WebSocket error observed:", event);
};
