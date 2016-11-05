function validateForm(id, option) {
    option = $.extend({}, {
        highlight: function (element) {
            $(element).parent().parent().addClass('has-error');
        },
        success: function (element) {
            $(element).parent().parent().removeClass('has-error');
            $(element).remove();
        },
        errorPlacement: function (err, element) {
            err.addClass("help-block text-danger");
            element.parent().next().append(err);
        },
        errorElement: "span"
    }, option)

    console.log(option);

    $(id).validate(option);
}