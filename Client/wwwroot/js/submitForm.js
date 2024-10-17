///<reference path="../../wwwroot/lib/jquery/dist/jquery.js" />
///<reference path="../../wwwroot/lib/jquery-validation/dist/jquery.validate.js" />

var form = $("form");
var textNode = $("#MessageText");
var textLenghtNode = $("#messageTextLenght");
var indexNode = $("#MessageIndex");
var submitButton = $("#submitButton");

textNode.on("input propertychange", function () {
    var messageLenght = $(this).val().length;
    textLenghtNode.text(`${messageLenght} / 128`);
});

$(function () {
    form.validate({
        messages: {
            MessageText: "Пожалуйста, введите текст сообщения."
        },
        errorElement: "div",
        errorPlacement: function (error, element) {
            error.addClass("invalid-feedback");
            error.insertAfter(element);
        },
        highlight: function (element) {
            $(element).removeClass('is-valid').addClass('is-invalid');
        },
        unhighlight: function (element) {
            $(element).removeClass('is-invalid').addClass('is-valid');
        },
        submitHandler: function () {
            $.ajax({
                type: "post",
                url: form.attr("action"),
                crossDomain: true,
                data: form.serialize(),
                beforeSend: function () {
                    $("#submitButton").hide();
                    $("#submitSpinner").show();
                },
                success: function (response) {
                    setTimeout(() => {
                        $("#submitButton").show();
                        $("#submitSpinner").hide();
                    }, 300);

                    $("#messageHistory").append(`<div class="card mb-2"><div class="card-header">${new Date(response).toLocaleString(undefined, {})} &ndash; ${indexNode.val()}</div><div class="card-body">${textNode.val()}</div></div>`);

                    var newIndex = parseInt(indexNode.val()) + 1;

                    indexNode.val(newIndex);
                    textNode.val(null);
                    textNode.trigger("input");
                },
                error: function (response) {
                    setTimeout(() => {
                        $("#submitButton").show();
                        $("#submitSpinner").hide();
                    }, 300);

                    textLenghtNode.text(`${Object.values(JSON.parse(response.responseText).errors)[0]}`);
                    console.log(JSON.parse(response.responseText));
                },
            });
            return false;
        }
    });
});