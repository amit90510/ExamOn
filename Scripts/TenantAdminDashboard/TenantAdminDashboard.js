var newpagesize = 5;
var maincol = {
    7: ""
};

var childcol = {
    1: "",
    2: "",
    3: ""
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
        $('#gridSubscptionEnd').grid({
            primaryKey: 'ID',
            uiLibrary: "bootstrap4",
            dataSource: data.Data,
            iconsLibrary: 'fontawesome',
            headerFilter: true,
            columns: [
                { field: 'id', width: 50, sortable: true },
                { field: 'TenantName', sortable: true },
                { field: 'TenantEmail', sortable: true },
                { field: 'TenantMobile', sortable: true },
                { field: 'SubscriptionEndDate', sortable: true },
                { field: 'LastRechargeOn', sortable: true },
                { field: 'RechargeAmount', sortable: true }
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
                            { field: 'SubscptionStartFrom', sortable: true, cssClass: 'childGrid1' },
                            { field: 'SubscriptionEndAt', sortable: true, cssClass: 'childGrid1' },
                            { field: 'RechargeAmount', sortable: true, cssClass: 'childGrid1' },
                            { field: 'CreatedDate', sortable: true, cssClass: 'childGrid1' }
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