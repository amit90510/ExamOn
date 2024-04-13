var detaildLevel = 1;
var totalColumn = 0;
function CustomExportMain(JquerygridObject) {
    let tableRows = [];
    let tempRows = "";
    let additionalStyles = "";
    let compStyle = null;
    var gijgoGrid = JquerygridObject;
    gijgoGrid.find("thead").eq(0).find("tr[data-role='caption']").each((i, headElement) => {
        compStyle = getComputedStyle(headElement);
        additionalStyles += (compStyle && compStyle.backgroundColor ? "background-color: " + compStyle.backgroundColor + ";" : "");
        additionalStyles += (compStyle && compStyle.color ? "color: " + compStyle.color + ";" : "");
        tempRows += "<tr style='font-weight:bold;text-align:center;" + additionalStyles + "'>";
        $(headElement).find("th div").each((j, headTiltle) => {
            if ($(headTiltle).html()) {
                tempRows += "<td>" + encodeSpecialCharacters($(headTiltle).html()) + "</td>";
            }
        });
        tempRows += "</tr>";
    });
    tableRows.push(tempRows);
    totalColumn = 0;
    gijgoGrid.find("tbody").eq(0).find("tr").each((p, rowtdElement) => {
        tempRows = "";
        if ($(rowtdElement).attr('data-role') == 'row') {
            compStyle = getComputedStyle(rowtdElement);
            additionalStyles += (compStyle && compStyle.backgroundColor ? "background-color: " + compStyle.backgroundColor + ";" : "");
            additionalStyles += (compStyle && compStyle.color ? "color: " + compStyle.color + ";" : "");
            let ischildRow = false;           
                $(rowtdElement).find("td").each((u, isChildElement) => {
                    if ($(isChildElement).hasClass("childGrid1")) {
                        ischildRow = true;
                        return false;
                    }
                });
            if (!ischildRow) {
                tempRows += "<tr style='" + additionalStyles + "'>";
                $(rowtdElement).find("td").each((q, rowchildTiltle) => {
                    try {
                        if ($($(rowchildTiltle).find("div").eq(0).html()).hasClass('fa-angle-right') || $($(rowchildTiltle).find("div").eq(0).html()).hasClass('fa-angle-down')) {
                        }
                        else {
                            tempRows += "<td type='string'>" + encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html()) + "</td>";
                        }
                    }
                    catch {
                        tempRows += "<td type='string'>" + encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html()) + "</td>";
                    }
                });
                tempRows += "</tr>";
                tableRows.push(tempRows);
                totalColumn++;
            }
        } else if ($(rowtdElement).attr('data-role') == 'details') {
            let emptyRow = "<tr>";
            for (let tColumn = 0; tColumn < totalColumn; tColumn++) {
                emptyRow += "<td></td>";
            }
            emptyRow += "</tr>"
            tableRows.push(emptyRow);
            let trows = getChildRows(($($(rowtdElement).find('table').eq(0))));
            trows.forEach((ti) => {
                tableRows.push(ti);
            });
            emptyRow = "<tr>";
            for (let tColumnNew = 0; tColumnNew < totalColumn; tColumnNew++) {
                emptyRow += "<td></td>";
            }
            emptyRow += "</tr>"
            tableRows.push(emptyRow);
        }
    });

    return tableRows;
}

function getChildRows(JquerygridObject) {
    let tableRows = [];
    let tempRows = "";
    let additionalStyles = "";
    let compStyle = null;
    var gijgoGrid = JquerygridObject;
    gijgoGrid.find("thead").eq(0).find("tr[data-role='caption']").each((i, headElement) => {
        compStyle = getComputedStyle(headElement);
        additionalStyles += (compStyle && compStyle.backgroundColor ? "background-color: " + compStyle.backgroundColor + ";" : "");
        additionalStyles += (compStyle && compStyle.color ? "color: " + compStyle.color + ";" : "");
        tempRows += "<tr style='font-weight:bold;text-align:center;" + additionalStyles + "'>";
        for (let dl = 0; dl < detaildLevel; dl++) {
            tempRows += "<td> -> </td>";
        }
        $(headElement).find("th div").each((j, headTiltle) => {
            if ($(headTiltle).html()) {
                tempRows += "<td>" + encodeSpecialCharacters($(headTiltle).html()) + "</td>";
            }
        });
        tempRows += "</tr>";
    });
    tableRows.push(tempRows);

    gijgoGrid.find("tbody").eq(0).find("tr").each((p, rowtdElement) => {
        tempRows = "";
        compStyle = getComputedStyle(rowtdElement);
        additionalStyles += (compStyle && compStyle.backgroundColor ? "background-color: " + compStyle.backgroundColor + ";" : "");
        additionalStyles += (compStyle && compStyle.color ? "color: " + compStyle.color + ";" : "");
        tempRows += "<tr style='" + additionalStyles + "'>";
        for (let rdl = 0; rdl < detaildLevel; rdl++) {
            tempRows += "<td>  </td>";
        }
        $(rowtdElement).find("td").each((q, rowchildTiltle) => {
            try {
                if ($($(rowchildTiltle).find("div").eq(0).html()).hasClass('fa-angle-right') || $($(rowchildTiltle).find("div").eq(0).html()).hasClass('fa-angle-down')) {
                }
                else {
                    tempRows += "<td type='string'>" + encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html()) + "</td>";
                }
            }
            catch {
                tempRows += "<td type='string'>" + encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html()) + "</td>";
            }
        });
        tempRows += "</tr>";
        tableRows.push(tempRows);
    });

    return tableRows;
}

function getExcelExport(gridId) {
    let getRows = CustomExportMain($('#' + gridId));
    let AllGridRows = [];
    getRows.forEach((allrows) => {
        AllGridRows.push(allrows);
    });
    return AllGridRows;
}

function encodeSpecialCharacters(text) {
    return text.replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');
}