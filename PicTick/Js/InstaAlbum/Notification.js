function showMessage(msg, type) {
    $("#notificationBlock").removeClass("bg-danger");
    $("#notificationBlock").removeClass("bg-success");
    $("#notificationBlock").removeClass("bg-warning");
    $("#notificationBlock").removeClass("bg-info");

    $("#notificationBlock").addClass("display-block");
    $("#notificationContaner").fadeIn(500);
    $("#notificationBlock").addClass("bg-" + type);
    $("#notificationMsg").html(msg);

    setTimeout(function () {
        hideMessage();
    }, 3000);

}

function hideMessage() {
    var selectedEffect = 'blind';
    $("#notificationBlock").removeClass("display-block");
    $("#notificationContaner").fadeOut(10000);
}

function showSuccessMessage(msg) {
    var newMsg = titleCase(msg);
    showMessage(newMsg, "success");
}

function showErrorMessage(msg) {
    var newMsg = titleCase(msg);
    showMessage(newMsg, "danger");
}

function showWarningMessage(msg) {
    var newMsg = titleCase(msg);
    showMessage(newMsg, "warning");
}

function titleCase(str) {
    var newstr = str.split(" ");
    for (i = 0; i < newstr.length; i++) {
        var copy = newstr[i].substring(1).toLowerCase();
        newstr[i] = newstr[i][0].toUpperCase() + copy;
    }
    newstr = newstr.join(" ");
    return newstr;
}

function showLoader() {
    $("#loaderBlock").addClass("display-block");
    $("#loaderContaner").fadeIn(500);
}

function hideLoader() {
    var selectedEffect = 'blind';
    $("#loaderBlock").removeClass("display-block");
    $("#loaderContaner").fadeOut(3000);
}