///<reference path="../../../wwwroot/lib/jquery/dist/jquery.js" />

function displayMessage(message, messagesListNode) {
    var messageNode = $("<div></div>").addClass(["card", "mb-2"]);
    var messageHeader = $("<div></div>").addClass("card-header").text(`${new Date(message.MessageTimestamp).toLocaleString(undefined, {})} \u2013 ${message.MessageIndex}`);
    var messageBody = $("<div></div>").addClass("card-body").text(`${message.MessageText}`);

    messageNode.append(messageHeader).append(messageBody).hide().fadeIn(500);
    messagesListNode.append(messageNode);
}

function displayMessages(messages, messagesListNode) {
    messages.forEach((message) => {
        displayMessage(message, messagesListNode);
    });
}

export { displayMessage, displayMessages };