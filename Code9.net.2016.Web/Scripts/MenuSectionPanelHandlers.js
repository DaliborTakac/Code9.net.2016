$(function () {
    $(".menu-item").each(function () {
        var element = $(this);
        var amount = element.find(".quantity");
        var display = element.find(".quantity-display");

        var increase = element.find(".increase");
        var decrease = element.find(".decrease");

        var quantity = new Quantity(0, 0, 20, function (val) {
            if (val == 0) {
                increase.removeClass("disabled");
                decrease.addClass("disabled");
            }
            else if (val == 20) {
                increase.addClass("disabled");
                decrease.removeClass("disabled");
            }
            else {
                increase.removeClass("disabled");
                decrease.removeClass("disabled");
            }
            display.html(val);
            amount.val(val);
        });

        increase.on("click", function (event) {
            event.preventDefault();
            quantity.increase();
        });
        decrease.on("click", function (event) {
            event.preventDefault();
            quantity.decrease();
        });

    });
})
