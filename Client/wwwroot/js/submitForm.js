///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />
///<reference path="../../wwwroot/lib/jquery-validation/dist/jquery.validate.js" />

import { displayMessage } from "./modules/messenger.js";

var form = $("form");
var textNode = $("#MessageText");
var textLenghtNode = $("#messageTextLenght");
var indexNode = $("#MessageIndex");
var submitButton = $("#submitButton");
var submitSpinner = $("#submitSpinner");
var formErrors = $("#formErrors");
var messageHistory = $("#messageHistory");

textNode.on("input propertychange", function () {
    var messageLenght = $(this).val().length;
    textLenghtNode.text(`${messageLenght} / 128`);

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
                required: "Пожалуйста, введите текст сообщения.",
                maxlength: "Сообщение должно быть не длиннее 128 символов."
            }
        },
        errorElement: "div",
        errorPlacement: function (error, element) {
            error.addClass("invalid-feedback");
            error.css("user-select", "none")
            error.on("click", function (e) {
                element.trigger("focus");
            });

            formErrors.html(error);
        },
        highlight: function (element) {
            submitButton.attr("disabled", true);
            $(element).removeClass('is-valid').addClass('is-invalid');
        },
        unhighlight: function (element) {
            submitButton.removeAttr("disabled");
            $(element).removeClass('is-invalid').addClass('is-valid');
        },
        submitHandler: function () {
            $.ajax({
                type: "post",
                url: form.attr("action"),
                crossDomain: true,
                data: form.serialize(),
                beforeSend: function () {
                    formErrors.html("");
                    textNode.attr("disabled", true);
                    submitButton.attr("disabled", true);
                    submitSpinner.show();
                    console.debug("Отправка сообщения на сервер...");
                },
                success: function (response) {
                    setTimeout(() => {
                        textNode.removeAttr("disabled");
                        submitButton.removeAttr("disabled");
                        submitSpinner.hide();
                    }, 300);

                    console.debug("Сообщение успешно отправлено");

                    var message = {
                        "MessageIndex": indexNode.val(),
                        "MessageTimestamp": response,
                        "MessageText": textNode.val()
                    };

                    console.debug(message);

                    displayMessage(message, messageHistory);

                    var newIndex = parseInt(indexNode.val()) + 1;
                    indexNode.val(newIndex);
                    textNode.val(null);

                    form.data("validator").resetForm();
                    textNode.removeClass('is-invalid').removeClass('is-valid');
                    textNode.trigger("input");
                },
                error: function (response) {
                    setTimeout(() => {
                        textNode.removeAttr("disabled");
                        submitButton.removeAttr("disabled");
                        submitSpinner.hide();
                    }, 300);

                    if (response.readyState == 0) {
                        var error = $("<div></div>")
                            .addClass("text-danger")
                            .text("Не удалось подключиться к серверу.")
                            .css({ "font-size": ".875em", });
                        formErrors.append(error);

                        console.error("Произошла ошибка при подключении к серверу.\nПроверьте соединение интернета и попробуйте еще раз.");
                    }
                    else {
                        var error = $("<div></div>")
                            .addClass("text-danger")
                            .text(response.responseText)
                            .css({ "font-size": ".875em" });
                        formErrors.append(error);

                        console.error(response.responseText);
                    }

                    textNode.valid();
                }
            });

            return false;
        }
    });
});