document.addEventListener("DOMContentLoaded", function (event) {

    const showNavbar = (toggleId, navId, bodyId, headerId) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId)

        // Validate that all variables exist
        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', () => {
                // show navbar
                nav.classList.toggle('show1')
                // change icon
                toggle.classList.toggle('fa-arrow-circle-o-left')
                // add padding to body
                bodypd.classList.toggle('body-pd')
                // add padding to header
                headerpd.classList.toggle('body-pd')
            })
        }
    }

    showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')
});

$(document).ready(function () {
    var hub = $.connection.notificationHub;
    hub.client.addNewMessageToPage = function (isHeader, header, finalBody, showProgress, showFooter, closeModal) {
        if ($('#signalRModal') && $('#signalRModal').is(':visible')) {
            handleModal(isHeader, header, finalBody, showProgress, showFooter, closeModal);
        }
        else {
            var myModal = new bootstrap.Modal(document.getElementById('signalRModal'), {
                keyboard: false,
                backdrop: false,
                focus: false
            });
            handleModal(isHeader, header, finalBody, showProgress, showFooter, closeModal);
            myModal.toggle();            
        }
    };
    $.connection.hub.start()
        .done(function () {
            $("#sRCookie").val(hub.connection.id);
            //console.log("Realtime hub is connected and started successfully!!");
        })
        .fail(function () {
            //console.log("Realtime hub can not be connected!");
            SwalFire("Browser-SignalR Error", "There is some issue with your browser, Please use latest chrome/Mozilla. <br/> (आपके ब्राउज़र में कुछ समस्या है, कृपया नवीनतम क्रोम/मोज़िला का उपयोग करें)", "error", "", () => { });
        });

    function handleModal(isHeader, header, finalBody, showProgress, showFooter, closeModal) {
        $("#signalRModalTitle").text(header);
        $("#signalRModalBody").html(finalBody);
        isHeader ? $("#signalRModalTitleVisible").show() : $("#signalRModalTitleVisible").hide();
        showProgress ? $("#signalRModalSpinner").show() : $("#signalRModalSpinner").hide();
        showFooter ? $("#signalRModalBodyFooterVisible").show() : $("#signalRModalBodyFooterVisible").hide();
        if (closeModal) { $("#signalRModal").hide(); }
    }
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
    GetProfileImage('dashboardProfileImage', '/Content/images/UserProfile.jpg');
});

function examOn_dateParse(val) {
    if (val) {
        var mydate = val;
        var dateVal = parseInt(mydate.substr(6));
        return new Date(dateVal);
    }
    return new Date();
}

function loadViews(url, jsonData, menuoptionActive,scriptPath,closeMenu = true, targetdiv = 'pageViewId') {
    const animate = ["animate__bounce", "animate__flash", "animate__pulse", "animate__headShake", "animate__backInDown", "animate__backInUp", "animate__backInLeft", "animate__bounceInRight", "animate__bounceInRight","animate__fadeInDownBig"];
    ServerData(url, "POST", jsonData, (data) => {
        $('#' + targetdiv).html('');
        $('#' + targetdiv).hide('fast');
        $('#' + targetdiv).removeClass();
        if ($('#menuOptions').find('a')) {
            $('#menuOptions a').removeClass('active');
        }
        $("#" + menuoptionActive).addClass('active');
        document.getElementById(targetdiv).innerHTML = data;
        if (scriptPath) {
            try {
                var newScript = document.createElement("script");
                newScript.src = window.location.origin + "/Scripts/" + scriptPath;
                document.getElementById(targetdiv).appendChild(newScript);
            }
            catch {
                SwalFire('ExamOn - Alert', 'There is some issue with getting information of this page', 'error', '', () => { location.href = "signOut"; }, 'इस पृष्ठ की जानकारी प्राप्त करने में कुछ समस्या है');
            }
        }
        try {
            const random = Math.floor(Math.random() * animate.length);
            $('#' + targetdiv).addClass('animate__animated  ' + animate[random])
            if (closeMenu && $("#nav-bar").hasClass('show1')) {
                closeMenuBar();
            }
        }
        catch { }
        $('#' + targetdiv).show('');
    }, () => { });
}

function closeMenuBar() {
    $("#body-pd").removeClass();
    $("#header").removeClass('body-pd');
    $("#nav-bar").removeClass('show1');
    $("#header-toggle").removeClass('fa-arrow-circle-o-left');
}

function GetProfileImage(elementToSet, altPath) {
    if (elementToSet) {
        ServerData("/StudDashboard/GetUserProfileImage", "Post", null, (data) => {
            if (data && data.size > 0) {
                let imageUrl = URL.createObjectURL(data);
                $('#' + elementToSet).attr('src', imageUrl);
            }
            else if (altPath) {
                $('#' + elementToSet).attr('src', altPath);
            }
        }, () => { }, true, true,'blob');
    }
}