var dataTable;

$(document).ready(function () {
    loadProductTable();
});

function loadProductTable() {
    dataTable = $('#tblProductData').DataTable({
        order: [[0, 'asc']],
        "ajax": { url: "/product/getall", type: "GET" },
        "columns": [
            { data: 'name', "width": "35%", "className": "text-left" },
            { data: 'category.categoryName', "width": "25%", "className": "text-left" },
            { data: 'price', "render": $.fn.dataTable.render.number(',', '.', 2, '$'), "width": "20%", "className": "text-left" },
            {
                data: 'productId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/product/productEdit?productId=${data}" class="mx-2 text-success">
                        <i class="bi bi-pencil-square"></i>
                        </a>
                        <a href="/product/productDelete?productId=${data}" class="text-danger mx-2">
                        <i class="fa-solid fa-link-slash"></i>
                        </a>
                    </div>`;
                },
                "width": "20%",
                "className": "text-left"
            }
        ]
    });
}
