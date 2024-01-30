function ServerData(url, type, jsonData, successcallBack, errorCallback, contentType = 'application/json; charset=utf-8', processData = true, responeType = '') {
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
        contentType: contentType,
        processData: processData,
        xhrFields: {
            responseType: responeType
        },
        data: (jsonData && processData) ? JSON.stringify(jsonData) : jsonData,
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

//below 2 function is for saving date and datetime to sql DB
function toSaveSqlDBDateFormat(dt, sourceformat = 'dd-mm-yyyy', targetformat = 'YYYY-MM-DD') {
    if (dt) {
        return moment(dt, sourceformat).format(targetformat)
    }
}

function toSaveSqlDBDateTimeFormat(dtime, sourceformat = 'dd/mm/yyyy hh:mm a', targetdtformat = 'YYYY-MM-DD', sourceTimeformat = 'hh:mm a', targetTimeformat = 'THH:mm:ss') {
    if (dtime) {
        let splitTime = dtime.split(' ');
        let convertedDate = moment(dtime, sourceformat).format(targetdtformat);
        let convertedTime = moment(splitTime[1] + ' ' + splitTime[2], sourceTimeformat).format(targetTimeformat);

        return convertedDate + convertedTime;
    }
}


//below 2 function is for showing date and datetime from sql DB to UI
function toShowSqlDateinUI(dt, sourceformat = 'YYYY-MM-DD', targetformat = 'dd-mm-yyyy') {
    if (dt) {
        return toSaveSqlDBDateFormat(dt, sourceformat, targetformat);
    }
}

function toShowSqlDatetimeinUI(dtime, sourceFormat = 'YYYY-MM-DDTHH:mm:ss', targetdateFormat = 'DD-MM-YYYY hh:mm a') {
    if (dtime) {
        return toSaveSqlDBDateFormat(dtime, sourceFormat, targetdateFormat);
    }
}