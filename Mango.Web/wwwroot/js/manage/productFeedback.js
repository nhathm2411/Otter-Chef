var dataTable;

$(document).ready(function () {
    loadProductTable();
});

function loadProductTable() {
    dataTable = $('#tblProductFeedbackData').DataTable({
        order: [[0, 'asc']],
        "ajax": { url: "/feedback/getall", type: "GET" },
        "columns": [
            { data: 'name', "width": "50%" },
            { data: 'price', "render": $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "20%" },
            { data: 'feedbackCount', "width": "10%" },
            {
                data: 'productId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/feedback/FeedbackList?productId=${data}" class="btn btn-success mx-2">
                        <i class="bi bi-pencil-square"></i>
                        </a>
                    </div>`;
                },
                "width": "20%"
            }
        ]
    });
}
