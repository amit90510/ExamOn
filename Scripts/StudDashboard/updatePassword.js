function UpdatePassword() {
    let errorMessage = "Please enter required fields (कृपया आवश्यक फ़ील्ड दर्ज करें)- ";
    let requiredFieldError = '';
    const arr = ["#txtOldPass", "#txtNewPass", "#txtretypeNewPass"];
    arr.forEach((field) => {
        if (!$(field).val()) {
            requiredFieldError += "<br/> " + $(field).parent().attr('error-For');
        }
    });
    if (requiredFieldError) {
        SwalFire('ExamOn - Alert', "", 'error', '', () => { }, errorMessage + " " + requiredFieldError);
    } else {
        if ($("#txtNewPass").val() != $("#txtretypeNewPass").val()) {
            SwalFire('ExamOn - Alert', "", 'error', '', () => { $("#txtretypeNewPass").val(''); }, "New password does not match with retype password <br/> नया पासवर्ड पुनः टाइप किये गये पासवर्ड से मेल नहीं खाता");
        }
        else {
            //good to go
            var loginObj = {
                "UserName": $("#txtOldPass").val().trim(),
                "ProfileName": $("#txtNewPass").val().trim()
            }

            ServerData("/StudDashboard/UpdateUserProfilePassword", "Post", loginObj, (data) => {
                if (data && data.StatusCode == "1") {
                    setTimeout(() => { location.href = "signout" }, 6000);
                }
            }, () => { });
        }
    }
}