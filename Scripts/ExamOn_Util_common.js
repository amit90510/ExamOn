document.addEventListener('DOMContentLoaded', () => {
    try {
        const tooltipTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(tooltipTriggerEl => {
            if (tooltipTriggerEl.getAttribute("examon_tooltip")) {
                new bootstrap.Tooltip(tooltipTriggerEl, { title: tooltipTriggerEl.getAttribute("examon_tooltip"), placement: "top" });
            }
        });
    } catch{
    }

    try {
        const emailValidatorFields = document.querySelectorAll('[examon_ftype="Email"]');
        emailValidatorFields.forEach(emailInput => {
            $(emailInput).on('focusout', () => {
                let emailValue = $(emailInput).val().trim();
                if (emailValue && !EmailValidator(emailValue)) {
                    $(emailInput).val('');
                    $(emailInput).addClass('emailValidator');
                } else {
                    $(emailInput).removeClass('emailValidator');
                }
            });
        });
    } catch {
    }

    try {
        const ReqValidatorFields = document.querySelectorAll('[examon_fRequired="Yes"]');
        ReqValidatorFields.forEach(emailInput => {
            switch ($(emailInput).prop('tagName')) {
            case "SELECT":
                // Code to execute if expression === value1
                    $(emailInput).on('blur', () => {
                        let fValue = $(emailInput).val();
                        let txtVal = $(emailInput).text();
                        let htmlVal = $(emailInput).html();
                        if (fValue || txtVal || htmlVal) {
                            $(emailInput).removeClass('emailValidator');
                        } else {
                            $(emailInput).addClass('emailValidator');
                        }
                    });
                break;
            default:
                    $(emailInput).on('focusout', () => {
                        let fValue = $(emailInput).val();
                        let txtVal = $(emailInput).text();
                        let htmlVal = $(emailInput).html();
                        if (fValue || txtVal || htmlVal) {
                            $(emailInput).removeClass('emailValidator');
                        } else {
                            $(emailInput).addClass('emailValidator');
                        }
                    });
                    break;
            }            
        });
    } catch {
    }
});

function loadExamOn_Tooltip() {
    try {
        const tooltipTriggerList = Array.from(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(tooltipTriggerEl => {
            if (tooltipTriggerEl.getAttribute("examon_tooltip")) {
                new bootstrap.Tooltip(tooltipTriggerEl, { title: tooltipTriggerEl.getAttribute("examon_tooltip"), placement: "top" });
            }
        });
    } catch { }
}

function EmailValidator(email) {
    return email.match(
        /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
}
