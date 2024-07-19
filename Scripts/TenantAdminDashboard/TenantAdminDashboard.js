var newpagesize = 5;
var maincol = {
    7: ""
};

var childcol = {   
    2: "",
    3: "",
    4:""
};

function exportToExcel(gridTable, fileName = "examOn_file", sheetName = "examOn") {
    $('th[data-field="SubscptionStartFrom"]').addClass('childGrid1');
    $('tfoot tr').addClass('examOn_excludeRow');
    $("#" + gridTable).table2excel({
        exclude: ".examOn_excludeRow",
        filename: fileName,
        fileext: ".xlsx",
        preserveColors: true,
        exclude_img: false,
        exclude_links: false,
        exclude_inputs: true,
        sheetName: sheetName + "_Data",
        maincolumnsum: maincol,
        childcolumnsum: childcol
    });
}

function getSubscriptionHistoryFullDetails(e) {
    let oldText = $(e).text();
    $(e).text('Processing, Please Wait.');
    ServerData("/TenantAdminDashboard/GetTenantSubscriptionFullHistoryPDF", "GET", null, (data) => {
        var a = document.createElement('a');
        var url = window.URL.createObjectURL(data);
        a.href = url;
        a.download = 'Subscrption_details_All.pdf';
        document.body.append(a);
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
        $(e).text(oldText);
    }, () => {
        $(e).text(oldText);
    }, 'application/json; charset=utf-8', false, 'blob');
}
function loadSubscriptionGrid() {
    $('#gridSubscptionEnd').grid('destroy', true, true);
    ServerData("/TenantAdminDashboard/GetTenantSubscriptionDetails", "Post", null, (data) => {
        $.each(data.Data, function (key, value) {
            try {
                value.SubscriptionEndDate = toShowSqlDateinUI(value.SubscriptionEndDate);
                value.LastRechargeOn = toShowSqlDateinUI(value.LastRechargeOn);
            }
            catch (error) { console.error(error); }
        });
        var receiptButtonRender = function (value, record, $cell, $displayEl) {
            var $btn = $('<button type="button" class="ignoreContent btn btn-danger">Receipt</button>').on('click', function () {
                if (record.id) {
                    //chane this button text to download
                    $(this).text('Processing, Please Wait.');
                    ServerData("/TenantAdminDashboard/GetTenantSubscriptionHistoryPDF?tid=" + record.id, "GET", null, (data) => {
                        var a = document.createElement('a');
                        var url = window.URL.createObjectURL(data);
                        a.href = url;
                        a.download = 'Subscrption_details.pdf';
                        document.body.append(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
                        a.remove();
                        $(this).text('Renew');
                    }, () => {
                        $(this).text('Renew');
                    }, 'application/json; charset=utf-8', false, 'blob');
                }
            });
            $displayEl.empty().append($btn);
        };

        $('#gridSubscptionEnd').grid({
            primaryKey: 'ID',
            uiLibrary: "bootstrap4",
            dataSource: data.Data,
            iconsLibrary: 'fontawesome',
            headerFilter: true,
            columns: [
                { field: 'id', width: 50, hidden: true, sortable: true },
                { field: 'TenantName', title: 'Name', sortable: true },
                { field: 'TenantEmail', title: 'Email', sortable: true },
                { field: 'TenantMobile', title: 'Mobile', sortable: true },
                { field: 'SubscriptionEndDate', title: 'Subscription End', sortable: true },
                { field: 'LastRechargeOn', title: 'Last Recharge On', sortable: true },
                { field: 'RechargeAmount', title: 'Last Recharge Amount', sortable: true }
            ],
            pager: { limit: 5, sizes: [5, 10, 20, 100, 500, 1000, 10000] },
            detailTemplate: '<div data-exclude="true"><table class="table table-striped table-sm table-responsive-sm"/></div>'
        });
        $('#gridSubscptionEnd').grid().on('pageSizeChange', function (e, newSize) {
            if (newSize && typeof (newSize) == 'number') {
                newpagesize = newSize;
            }
        });

        $('#gridSubscptionEnd').grid().on('detailExpand', function (e, $detailWrapper, id) {
            ServerData("/TenantAdminDashboard/GetTenantSubscriptionHistory?tid=" + $('#gridSubscptionEnd').grid().get(id).id, "Post", null, (data) => {
                if (data && data.StatusCode == "1") {
                    $.each(data.Data, function (key, value) {
                        try {
                            value.SubscptionStartFrom = toShowSqlDateinUI(value.SubscptionStartFrom);
                            value.SubscriptionEndAt = toShowSqlDateinUI(value.SubscriptionEndAt);
                            value.CreatedDate = toShowSqlDatetimeinUI(value.CreatedDate);
                        }
                        catch (error) { console.error(error); }
                    });
                    $detailWrapper.find('table').grid({
                        dataSource: data.Data, columns: [
                            { field: 'id', width: 50, hidden: true, sortable: true },
                            { field: 'SubscptionStartFrom', title: 'Subscription Start', sortable: true, cssClass: 'childGrid1' },
                            { field: 'SubscriptionEndAt', title: 'Subscription End', sortable: true, cssClass: 'childGrid1' },
                            { field: 'RechargeAmount', title: 'Recharge Amount', sortable: true, cssClass: 'childGrid1' },
                            { field: 'CreatedDate', title: 'Date', sortable: true, cssClass: 'childGrid1' },
                            { field: '', cssClass: 'childGrid1', renderer: receiptButtonRender }
                        ],
                        pager: { limit: newpagesize, sizes: [5, 10, 20, 100, 500, 1000, 10000] }
                    });
                } else {
                    $detailWrapper.find('table').grid('destroy', true, true);
                }
            });
        });
        $('#gridSubscptionEnd').grid().on('detailCollapse', function (e, $detailWrapper, id) {
            $detailWrapper.find('table').grid('destroy', true, true);
        });
        $('#gridSubscptionEnd').grid().on('rowSelect', function (e, $row, id, record) {
        });

    }, () => { });
}

function loadRegisteredStudentsGrid() {
    $('#gridSubscptionEnd').grid('destroy', true, true);
    ServerData("/TenantAdminDashboard/GetloadRegisteredStudentsGrid", "Post", null, (data) => {
        $.each(data.Data, function (key, value) {
            try {
                value.CreatedOn = toShowSqlDatetimeinUI(value.CreatedOn);
            }
            catch (error) { console.error(error); }
        });
        var editButtonRender = function (value, record, $cell, $displayEl) {
            var $btn = $('<button type="button" class="ignoreContent btn btn-danger">Edit</button>').on('click', function () {
                if (record.id) {}
            });
            $displayEl.empty().append($btn);
        };
        var inactiveButtonRender = function (value, record, $cell, $displayEl) {
            var $btn = $('<button type="button" class="ignoreContent btn btn-danger">Inactive</button>').on('click', function () {
                if (record.id) { }
            });
            $displayEl.empty().append($btn);
        };

        var blockButtonRender = function (value, record, $cell, $displayEl) {
            var $btn = $('<button type="button" class="ignoreContent btn btn-danger">Block</button>').on('click', function () {
                if (record.id) { }
            });
            $displayEl.empty().append($btn);
        };

        $('#gridSubscptionEnd').grid({
            primaryKey: 'ID',
            uiLibrary: "bootstrap4",
            dataSource: data.Data,
            iconsLibrary: 'fontawesome',
            headerFilter: true,
            columns: [
                { field: 'Id', title: 'Sno', width: 40, filterable: false, sortable: true, hidden: true },
                { field: 'UserName', title: 'User Name', sortable: true },
                { field: 'RealName', title: 'Student Name', sortable: true },
                { field: 'Mobile', title: 'Mobile', sortable: true },
                { field: 'EmailId', title: 'Email', sortable: true },
                { field: 'Active', title: 'User Active', sortable: true },
                { field: 'BlockLogin', title: 'User Blocked', sortable: true },
                { field: 'CreatedOn', title: 'Create Date', sortable: true },
                { field: 'Address', title: 'Address', sortable: true },
                { field: 'City', title: 'City', sortable: true },
                { field: 'State', title: 'State', sortable: true },
                { field: '', title: '', sortable: false, filterable: false, renderer: editButtonRender },
                { field: 'Active', title: '', sortable: false, filterable: false, renderer: inactiveButtonRender },
                { field: 'BlockLogin', title: '', sortable: false, filterable: false, renderer: blockButtonRender }
            ],
            pager: { limit: 5, sizes: [5, 10, 20, 100, 500, 1000, 10000] },
            detailTemplate: '<div data-exclude="true"><table class="table table-striped table-sm table-responsive-sm"/></div>'
        });
        $('#gridSubscptionEnd').grid().on('pageSizeChange', function (e, newSize) {
            if (newSize && typeof (newSize) == 'number') {
                newpagesize = newSize;
            }
        });

        $('#gridSubscptionEnd').grid().on('detailExpand', function (e, $detailWrapper, id) {
            ServerData("/TenantAdminDashboard/GetTenantSubscriptionHistory?tid=" + $('#gridSubscptionEnd').grid().get(id).id, "Post", null, (data) => {
                if (data && data.StatusCode == "1") {
                    $.each(data.Data, function (key, value) {
                        try {
                            value.SubscptionStartFrom = toShowSqlDateinUI(value.SubscptionStartFrom);
                            value.SubscriptionEndAt = toShowSqlDateinUI(value.SubscriptionEndAt);
                            value.CreatedDate = toShowSqlDatetimeinUI(value.CreatedDate);
                        }
                        catch (error) { console.error(error); }
                    });
                    $detailWrapper.find('table').grid({
                        dataSource: data.Data, columns: [
                            { field: 'id', width: 50, hidden: true, sortable: true },
                            { field: 'SubscptionStartFrom', title: 'Subscription Start', sortable: true, cssClass: 'childGrid1' },
                            { field: 'SubscriptionEndAt', title: 'Subscription End', sortable: true, cssClass: 'childGrid1' },
                            { field: 'RechargeAmount', title: 'Recharge Amount', sortable: true, cssClass: 'childGrid1' },
                            { field: 'CreatedDate', title: 'Date', sortable: true, cssClass: 'childGrid1' },
                            { field: '', cssClass: 'childGrid1', renderer: receiptButtonRender }
                        ],
                        pager: { limit: newpagesize, sizes: [5, 10, 20, 100, 500, 1000, 10000] }
                    });
                } else {
                    $detailWrapper.find('table').grid('destroy', true, true);
                }
            });
        });
        $('#gridSubscptionEnd').grid().on('detailCollapse', function (e, $detailWrapper, id) {
            $detailWrapper.find('table').grid('destroy', true, true);
        });
        $('#gridSubscptionEnd').grid().on('rowSelect', function (e, $row, id, record) {
        });

    }, () => { });
}