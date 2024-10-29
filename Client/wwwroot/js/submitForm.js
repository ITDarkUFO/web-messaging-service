///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />
///<reference path="../../wwwroot/lib/jquery-validation/dist/jquery.validate.js" />

import { displayMessage } from "./modules/messenger.js";

var form = $("form");
var textNode = $("#MessageText");
var textLengthNode = $("#messageTextLength");
var indexNode = $("#MessageIndex");
var submitButton = $("#submitButton");
var submitSpinner = $("#submitSpinner");
var formErrors = $("#formErrors");
var messageHistory = $("#messageHistory");

textNode.on("input propertychange", function () {
    var messageLength = $(this).val().length;
    textLengthNode.text(`${messageLength} / 128`);

    formErrors.html("");
});

$(function () {
    form.validate({
        rules: {
            MessageText: {
                required: true,
                maxlength: 128
            }
        },
        messages: {
            MessageText: {
                required: "Пожалуйста, введите текст сообщения",
                maxlength: "Сообщение должно быть не длиннее 128 символов"
            }
        },
        errorElement: "div",
        errorPlacement: showInputError,
        highlight: highlightElement,
        unhighlight: unhighlightElement,
        submitHandler: submitForm
    });
});

function showInputError(error, element) {
    error.addClass("invalid-feedback");
    error.css("user-select", "none");
    element.trigger("focus");
    formErrors.append(error);
}

function showFormError(text) {
    var error = $("<div></div>")
        .addClass("text-danger")
        .text(text)
        .css({ "font-size": ".875em", });
    formErrors.append(error);
}

function highlightElement(element) {
    submitButton.attr("disabled", true);
    $(element).removeClass('is-valid').addClass('is-invalid');
}

function unhighlightElement(element) {
    submitButton.removeAttr("disabled");
    $(element).removeClass('is-invalid').addClass('is-valid');
}

function disableFormWhileSubmitting() {
    textNode.attr("disabled", true);
    submitButton.attr("disabled", true);
    submitSpinner.show();
}

function enableFormAfterSubmitting() {
    setTimeout(() => {
        textNode.removeAttr("disabled");
        submitButton.removeAttr("disabled");
        submitSpinner.hide();
    }, 300);
}

function incrementMessageIndex() {
    var newIndex = parseInt(indexNode.val()) + 1;
    indexNode.val(newIndex);
    textNode.val(null);
}

function clearForm() {
    form.data("validator").resetForm();
    textNode.removeClass('is-invalid').removeClass('is-valid');
    textNode.trigger("input");
}

function submitForm() {
    $.ajax({
        type: "post",
        url: form.attr("action"),
        crossDomain: true,
        data: form.serialize(),
        beforeSend: function () {
            formErrors.html("");
            disableFormWhileSubmitting();
            console.debug("Отправка сообщения на сервер...");
        },
        success: function (response) {
            enableFormAfterSubmitting();
            console.debug("Сообщение успешно отправлено");

            try {
                var message = JSON.parse(response);
                console.debug(message);

                displayMessage(message, messageHistory);
                incrementMessageIndex();
                clearForm();
            }
            catch (error) {
                console.error("Ошибка при парсинге ответа:", error);
                showFormError("Не удалось обработать ответ от сервера");
            }
        },
        error: function (response) {
            enableFormAfterSubmitting();

            if (response && response.status == 0) {
                console.error("Произошла ошибка при подключении к серверу.\nПроверьте соединение интернета и попробуйте еще раз.");
                showFormError("Не удалось подключиться к серверу");
            }
            else if (response && response.responseText) {
                console.error(response.responseText);
                showFormError(response.responseText);
            }
            else {
                console.error("Возникла неизвестная ошибка");
                showFormError("Возникла неизвестная ошибка");
            }

            textNode.valid();
        }
    });
}