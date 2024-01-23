ServerData("/StudDashboard/GetStatesInfo", "Post", null, (data) => {
    if (data && data.StatusCode == "1") {
        $('#txtState').html('');
        $('#txtState').append($("<option selected></option>").attr("value", "").text('Choose State'))
        $.each(data.Data, function (key, value) {
            $('#txtState').append($("<option></option>").attr("value", key).text(value));
        });

        ServerData("/StudDashboard/GetUserProfileDate", "Post", null, (data) => {
            if (data && data.StatusCode == "1") {
                data.Data[0].forEach((item) => {
                    if (item.Key == "username") {
                        $("#txtUserName").val(item.Value);
                        $("#txtUserNamePost").val(item.Value);
                    }
                    if (item.Key == "RealName") {
                        $("#txtProfileName").val(item.Value);
                    }
                    if (item.Key == "Mobile") {
                        $("#txtMobile").val(item.Value);
                    }
                    if (item.Key == "TypeName") {
                        $("#txtType").val(item.Value);
                    }
                    if (item.Key == "EmailId") {
                        $("#txtEMail").val(item.Value);
                    }
                    if (item.Key == "address") {
                        $("#txtAddress").val(item.Value);
                    }
                    if (item.Key == "City") {
                        $("#txtCity").val(item.Value);
                    }
                    if (item.Key == "State") {
                        $("#txtState option[value='" + item.Value+"']").attr('selected','selected');
                    }
                });               
            }
        }, () => { });

    }
}, () => { });

function CheckEmailValid(email) {
    return email.match(
        /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
}

function UpdateProfile() {
    let errorMessage = "Please enter required fields (कृपया आवश्यक फ़ील्ड दर्ज करें)- ";
    let requiredFieldError = '';
    const arr = ["#txtProfileName", "#txtEMail", "#txtMobile", "#txtAddress", "#txtCity", "#txtState"];
    arr.forEach((field) => {
        if (!$(field).val()) {
            requiredFieldError += "<br/> " + $(field).parent().attr('error-For');
        }
    });
    if (requiredFieldError) {
        SwalFire('ExamOn - Alert', "", 'error', '', () => { }, errorMessage + " " + requiredFieldError);
    } else {
        //update now
        var objectPass = {
            UserName: $("#txtUserNamePost").val(),
            ProfileName: $("#txtProfileName").val(),
            Email: $("#txtEMail").val(),
            Mobile: $("#txtMobile").val(),
            Address: $("#txtAddress").val(),
            City: $("#txtCity").val(),
            State: $("#txtState").val()
        };
    }
}