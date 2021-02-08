Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);

        $(".add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_up").click(Cart.incrementItem);
        $(".cart_quantity_down").click(Cart.addToCart);
        $(".cart_quantity_delete").click(Cart.addToCart);
    },

    addToCart: function (event) {
        //отменяем стандартное поведение: переход при клике по ссылке
        event.preventDefault();

        var button = $(this);

        //через специальный механизм преобразования атрибутов с префиксом data- (например data-productId) в словарик
        //получаем идентификатор товара 
        const id = button.data("productId");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { Cart.logError("addToCart failed") });
    },

    showToolTip: function (button) {
        //Формируем всплывающую подсказку и показываем её
        button.tooltip({ title: "Добавлено в корзину" }).tooltip("show");

        //А через 500 мс удалим его
        setTimeout(function () {
            button.tooltip("destroy");
        }, 500);
    },

    refreshCartView: function (button) {
        $.get(Cart._properties.getCartViewLink)
            .done(function (cartHtml) {
                $(".cart-container").each(function () {
                    $(this).html(cartHtml);
                });
            })
            .fail(function () {
                Cart.logError("refreshCartView failed");
            });
    },

    logError: function (message) {
        console.log(message);
    },


    incrementItem: function (event) {
        //отменяем стандартное поведение: переход при клике по ссылке
        event.preventDefault();

        var button = $(this);
        const id = button.data("productId");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { Cart.logError("addToCart failed") });
    },

}