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
                });               
            }
        }, () => { });

    }
}, () => { });