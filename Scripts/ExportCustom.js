var detaildLevel = 1;
var totalColumn = 0;

var MainTableColumn = 0;
var MainTableRecords = 0;
var ChildTableColumn = 0;
var ChildTableRecords = 0;
function CustomExportMain(JquerygridObject, maincolumnsum, childcolumnsum) {
    let tableRows = [];
    let tempRows = "";
    let additionalStyles = "";
    let compStyle = null;
    var gijgoGrid = JquerygridObject;
    MainTableColumn = 0;
    MainTableRecords = 0;
    detaildLevel = 1;
    totalColumn = 0;


    gijgoGrid.find("thead").eq(0).find("tr[data-role='caption']").each((i, headElement) => {
        compStyle = getComputedStyle(headElement);
        additionalStyles += (compStyle && compStyle.backgroundColor ? "background-color: " + compStyle.backgroundColor + ";" : "");
        additionalStyles += (compStyle && compStyle.color ? "color: " + compStyle.color + ";" : "");
        tempRows += "<tr style='font-weight:bold;text-align:center;'>";
        $(headElement).find("th div[data-role='title']").each((j, headTiltle) => {
            if ($(headTiltle).html()) {
                tempRows += "<td style='background-color: #f5f5f5;border: 1px solid;'>" + encodeSpecialCharacters($(headTiltle).html()) + "</td>";
                MainTableColumn++;
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
                            let writeVal = encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html());
                            tempRows += "<td type='string'>" + writeVal + "</td>";
                            updateColumnSum(maincolumnsum, writeVal, q);
                        }
                    }
                    catch {
                        let writeVal = encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html());
                        tempRows += "<td type='string'>" + writeVal + "</td>";
                        updateColumnSum(maincolumnsum, writeVal, q);
                    }
                });
                tempRows += "</tr>";
                tableRows.push(tempRows);
                totalColumn++;
                MainTableRecords++;
            }
        } else if ($(rowtdElement).attr('data-role') == 'details') {
            let emptyRow = "<tr>";
            //for (let tColumn = 0; tColumn < totalColumn; tColumn++) {
            //    emptyRow += "<td></td>";
            //}
            //emptyRow += "</tr>"
            //tableRows.push(emptyRow);
            let trows = getChildRows(($($(rowtdElement).find('table').eq(0))), childcolumnsum);
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

    if (MainTableRecords > 0) {
        if (maincolumnsum && typeof (maincolumnsum) === 'object') {
            let sumRow = "<tr>";
            try {
                for (let mtloop = 1; mtloop <= MainTableColumn; mtloop++) {
                    sumRow += maincolumnsum[mtloop] ? "<td style='font-weight:bold;text-align:center;background-color:yellow;text-align:right;'>" + maincolumnsum[mtloop] + "</td>" : "<td></td>";
                }
                sumRow += "</tr>";
                tableRows.push(sumRow);
            }
            catch {
            }
        }
        let rowEmpty = "<tr><td style='font-weight:bold;text-align:center;' colspan='" + MainTableColumn + "'>--End of Records--</td></tr>";
        tableRows.push(rowEmpty);
        rowEmpty = "<tr><td style='font-weight:bold;text-align:center;background-color:yellow;' colspan='" + MainTableColumn + "'>Total Number of Records - " + MainTableRecords + "</td></tr>";
        tableRows.push(rowEmpty);
    }

    return tableRows;
}

function updateColumnSum(colobj, currentval, index1) {
    if (colobj && colobj != null && colobj.hasOwnProperty(index1)) {
        try {

            let getVal = colobj[index1] ? colobj[index1] : "0";
            let finalValue = BigInt(getVal) + BigInt(currentval);
            colobj[index1] = finalValue.toString();
        }
        catch { }
    }
}

function getChildRows(JquerygridObject, childcolsum) {
    let tableRows = [];
    let tempRows = "";
    let additionalStyles = "";
    let compStyle = null;
    var gijgoGrid = JquerygridObject;
    ChildTableColumn = 0;
    ChildTableRecords = 0;
    gijgoGrid.find("thead").eq(0).find("tr[data-role='caption']").each((i, headElement) => {
        compStyle = getComputedStyle(headElement);
        additionalStyles += (compStyle && compStyle.backgroundColor ? "background-color: " + compStyle.backgroundColor + ";" : "");
        additionalStyles += (compStyle && compStyle.color ? "color: " + compStyle.color + ";" : "");
        tempRows += "<tr style='font-weight:bold;text-align:center;'>";
        tempRows += "<td style='text-align:center;' colspan='" + detaildLevel + "'> -> </td>";
        $(headElement).find("th div[data-role='title']").each((j, headTiltle) => {
            if ($(headTiltle).html()) {
                tempRows += "<td style='background-color: #f5f5f5;border: 1px solid;'>" + encodeSpecialCharacters($(headTiltle).html()) + "</td>";
                ChildTableColumn++;
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
        let maincount = 0;
        for (let rdl = 0; rdl < detaildLevel; rdl++) {
            tempRows += "<td>  </td>";
        }
        $(rowtdElement).find("td").each((q, rowchildTiltle) => {
            maincount = q;
            try {
                if ($($(rowchildTiltle).find("div").eq(0).html()).hasClass('fa-angle-right') || $($(rowchildTiltle).find("div").eq(0).html()).hasClass('fa-angle-down')) {
                }
                else {
                    let writeVal = encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html());
                    tempRows += "<td type='string'>" + writeVal + "</td>";
                    updateColumnSum(childcolsum, writeVal, (++maincount));
                }
            }
            catch {
                let writeVal = encodeSpecialCharacters($(rowchildTiltle).find("div").eq(0).html());
                tempRows += "<td type='string'>" + writeVal + "</td>";
                updateColumnSum(childcolsum, writeVal, (++maincount));
            }
        });
        tempRows += "</tr>";
        tableRows.push(tempRows);
        ChildTableRecords++;
    });
    if (ChildTableRecords > 0) {
        //let rowEmpty = "<tr><td></td><td style='font-weight:bold;text-align:center;' colspan='" + ChildTableColumn + "'>--End of Records--</td></tr>";
        //tableRows.push(rowEmpty);

        if (childcolsum && typeof (childcolsum) === 'object') {
            let sumRow = "<tr><td></td>";
            try {
                for (let mtloop = 1; mtloop <= ChildTableColumn; mtloop++) {
                    sumRow += childcolsum[mtloop] ? "<td style='font-weight:bold;text-align:center;background-color:yellow;text-align:right;'>" + childcolsum[mtloop] + "</td>" : "<td></td>";
                }
                sumRow += "</tr>";
                tableRows.push(sumRow);
            }
            catch {
            }
        }


        let rowEmpty = "<tr><td></td><td style='font-weight:bold;text-align:center;background-color:yellow;' colspan='" + ChildTableColumn + "'>Total Number of Records - " + ChildTableRecords + "</td></tr>";
        tableRows.push(rowEmpty);
    }
    return tableRows;
}

function getExcelExport(gridId, maincolumnsum, childcolumnsum) {
    let getRows = CustomExportMain($('#' + gridId), maincolumnsum, childcolumnsum);
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