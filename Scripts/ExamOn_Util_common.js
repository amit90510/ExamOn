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
