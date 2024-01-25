ServerData("/StudDashboard/GetStatesInfo", "Post", null, (data) => {
    if (data && data.StatusCode == "1") {
        $('#txtState').html('');
        $('#txtState').append($("<option selected></option>").attr("value", "").text('Choose State'))
        $.each(data.Data, function (key, value) {
            $('#txtState').append($("<option></option>").attr("value", key).text(value));
        });
        GetProfileData();
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
        let fileInput = $('#imageInput')[0];
        let file = fileInput ? fileInput.files[0] : null;
        let objectPass = new FormData();
        formData.append('ProfileImage', file);
        formData.append('UserName', $("#txtUserNamePost").val());
        formData.append('ProfileName', $("#txtProfileName").val());
        formData.append('Email', $("#txtEMail").val());
        formData.append('Mobile', $("#txtMobile").val());
        formData.append('Address', $("#txtAddress").val());
        formData.append('City', $("#txtCity").val());
        formData.append('State', $("#txtState").val());
        ServerData("/StudDashboard/UserProfileDataUpdate", "Post", objectPass, (data) => {
            if (data && data.StatusCode == "1") {
                GetProfileData();
            }
        }, () => { }, false, false);
    }
}

function GetProfileData() {
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
                    $("#txtState option[value='" + item.Value + "']").attr('selected', 'selected');
                }
            });
        }
    }, () => { });
}

function previewImage(input) {
    var reader = new FileReader();
    reader.onload = function (e) {
        $('#imageDisplay').attr('src', e.target.result).show();
    };
    reader.readAsDataURL(input.files[0]);
}

$('#imageInput').change(function (event) {
    const fileSizeLimit = 500 * 1024; // 500Kb in bytes
    if (this.files && this.files[0]) {
        if (this.files[0].size > fileSizeLimit) {
            event.target.value = '';
            SwalFire('ExamOn- Alert', '', 'error', '', () => { },'Please choose image below 500Kb in size. <br/> कृपया 500kb से कम आकार की छवि चुनें।');
        } else {
            previewImage(this);
        }
    }
    else {
        event.target.value = '';
    }
});