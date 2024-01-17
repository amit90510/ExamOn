function ServerData(url, type, jsonData, successcallBack, errorCallback) {
    var token = $('[name=__RequestVerificationToken]').val();
    var KeyId = $('#sRCookie').val();
    var headers = {
        "__RequestVerificationToken" : token,
        "__RequestVerificationSRKey" : KeyId
    }
    $.ajax({
        type: type,
        url: url,
        cache: false,
        headers: headers,
        contentType: 'application/json; charset=utf-8',
        data: jsonData ? JSON.stringify(jsonData) : jsonData,
        success: function (data) {
            successcallBack(data);
        },
        error: function (err) {
            errorCallback(err);
        }
    });
}

function SwalFire(title, text, icon, footer, thenCallback, html='') {
    Swal.fire({
        title: title,
        text: text,
        html: html,
        icon: icon,
        footer: '<p>©ExamOn, Inc. All rights reserved.</p>'
    }).then(() => {
        thenCallback();
    });
}