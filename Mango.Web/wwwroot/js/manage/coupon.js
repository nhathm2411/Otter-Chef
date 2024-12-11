var dataTable;

$(document).ready(function () {
    loadCouponTable();
});

function loadCouponTable() {
    dataTable = $('#tblCouponData').DataTable({
        order: [[0, 'asc']],
        "ajax": { url: "/coupon/getall", type: "GET" },
        "columns": [
            { data: 'couponCode', "width": "25%" },
            { data: 'discountAmount', "render": $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "15%" },
            { data: 'minAmount', "render": $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "15%" },
            { data: 'quantity', "width": "10%" },
            {
                data: 'couponId',
                "render": function (data, type, row) {
                    if (row.isActive) {
                        return `<div class="btn-group" role="group">
                            <a href="/coupon/couponDelete?couponId=${data}" class="btn btn-danger mx-2">
                            <i class="bi bi-trash"></i>
                            </a>
                        </div>`;
                    } else {
                        return `<span>Deleted</span>`;
                    }
                },
                "width": "10%"
            }
        ]
    });
}
