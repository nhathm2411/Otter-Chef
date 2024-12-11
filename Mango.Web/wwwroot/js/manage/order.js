var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    } else if (url.includes("readyforpickup")) {
        loadDataTable("readyforpickup");
    } else if (url.includes("cancelled")) {
        loadDataTable("cancelled");
    } else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else {
        loadDataTable("all");
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        order: [[0, 'desc']],
        "ajax": { url: "/order/getall?status=" + status },
        "columns": [
            { data: 'orderHeaderId', "width": "5%", "className": "text-left" },
            { data: 'email', "width": "20%" },
            { data: 'name', "width": "20%" },
            { data: 'phone', "width": "10%", "className": "text-left" },
            { data: 'status', "width": "15%" },
            { data: 'orderTotal', "width": "10%", "className": "text-left" },
            {
                data: null,
                "render": function (data) {
                    let retryPaymentButton = '';
                    if (data.status === "Pending" && userRole === "customer") {
                        retryPaymentButton = `<a href="/cart/RetryPayment?orderId=${data.orderHeaderId}" class="btn btn-warning mx-2">Pay</a>`;
                    }
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/order/orderDetail?orderId=${data.orderHeaderId}" class="text-success mx-2">
                        <i class="bi bi-pencil-square"></i>
                        </a>
                        ${retryPaymentButton}
                    </div>`;
                },
                "width": "10%"
            }
        ]
    });
}

