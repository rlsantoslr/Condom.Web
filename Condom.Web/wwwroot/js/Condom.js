function showMessage(type, message) {

    if (type.length > 1) type = type.substring(0, 1);

    switch (type) {
        case 'S':
            toastr.success(message);
            break;
        case 'I':
            toastr.info(message);
            break;
        case 'E':
            toastr.error(message);
            break;
        case 'W':
            toastr.warnign(message);
            break;

    }
}

function showTracker(track) {
    track.logs.forEach((log) => {
        showMessage(log.type, log.message);
    });
}
function spinner(enable) {
    if (enable) {
        $("#main-screen-spinner").show();
    } else {
        $("#main-screen-spinner").hide();
    }
}
function login() {
    $.post("identity/login", $("#login-form").serialize(), function (resp) {
        showTracker(resp);

        if (!resp.hasError)  window.location.href = '/dashboard';
    });
}
function logout() {
    $.post("identity/logout", function (resp) {
        showTracker(resp);

        if (!resp.hasError) window.location.href = '/account/login';
    });
}