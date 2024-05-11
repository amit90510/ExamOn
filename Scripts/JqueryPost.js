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
function toSaveSqlDBDateFormat(dt, sourceformat = 'DD-MM-YYYY', targetformat = 'YYYY-MM-DD') {
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
function toShowSqlDateinUI(dt, sourceformat = 'YYYY-MM-DD', targetformat = 'DD-MM-YYYY') {
    if (dt) {
        return toSaveSqlDBDateFormat(moment(examOn_dateParse_util(dt)).format(sourceformat), sourceformat, targetformat);
    }
}

function toShowSqlDatetimeinUI(dtime, sourceFormat = 'YYYY-MM-DDTHH:mm:ss', targetdateFormat = 'DD-MM-YYYY hh:mm a') {
    if (dtime) {
        return toSaveSqlDBDateFormat(moment(examOn_dateParse_util(dtime)).format(sourceFormat), sourceFormat, targetdateFormat);
    }
}

function examOn_dateParse_util(val) {
    if (val) {
        var mydate = val;
        var dateVal = parseInt(mydate.substr(6));
        return new Date(dateVal);
    }
}

function NumberCountersAnimation(ele, finalval) {
    if (ele) {
        try {
            const counter = document.getElementById(ele);
            const speed = 500000000;
            const animate = () => {
                const value = finalval;
                const data = +counter.innerText;

                const time = value / speed;
                if (data < value) {
                    counter.innerText = Math.ceil(data + time);
                    setTimeout(animate, 1);
                } else {
                    counter.innerText = finalval;
                }
            }
            animate();
        }
        catch {
            counter.innerText = finalval;
        }
    }
}