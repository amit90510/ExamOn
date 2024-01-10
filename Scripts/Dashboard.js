
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
                nav.classList.toggle('show')
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
        if ($('#signalRModal') && $('#signalRModal').hasClass('show')) {
            handleModal(isHeader, header, finalBody, showProgress, showFooter, closeModal);
        }
        else {
            var myModal = new bootstrap.Modal(document.getElementById('signalRModal'), {
                keyboard: false,
                backdrop: false,
                focus: false
            });
            handleModal(isHeader, header, finalBody, showProgress, showFooter, closeModal);
            myModal.show('fast');
        }
    };
    $.connection.hub.start()
        .done(function () {
            $("#sRCookie").val(hub.connection.id);
            console.log("Realtime hub is connected and started successfully!!");
        })
        .fail(function () {
            console.log("Realtime hub can not be connected!");
            SwalFire("Browser-SignalR Error", "There is some issue with your browser, Please use latest chrome/Mozilla. <br/> (आपके ब्राउज़र में कुछ समस्या है, कृपया नवीनतम क्रोम/मोज़िला का उपयोग करें)", "error", "", () => { });
        });

    function handleModal(isHeader, header, finalBody, showProgress, showFooter, closeModal) {
        $("#signalRModalTitle").text(header);
        $("#signalRModalBody").html(finalBody);
        isHeader ? $("#signalRModalTitleVisible").show() : $("#signalRModalTitleVisible").hide();
        showProgress ? $("#signalRModalSpinner").show() : $("#signalRModalSpinner").hide();
        showFooter ? $("#signalRModalBodyFooterVisible").show() : $("#signalRModalBodyFooterVisible").hide();
        if (closeModal) { document.getElementById('signalRModal').hide(); }
    }
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
});