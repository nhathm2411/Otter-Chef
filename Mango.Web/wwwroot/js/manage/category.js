var dataTable;

$(document).ready(function () {
    loadCategoryTable();
});

function loadCategoryTable() {
    dataTable = $('#tblCategoryData').DataTable({
        order: [[0, 'asc']],
        "ajax": { url: "/category/getall", type: "GET" },
        "columns": [
            { data: 'categoryName', "width": "75%" },
            {
                data: 'categoryId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/category/categoryEdit?categoryId=${data}" class="btn btn-success mx-2">
                        <i class="bi bi-pencil-square"></i>
                        </a>
                        <a href="/category/categoryDelete?categoryId=${data}" class="btn btn-danger mx-2">
                        <i class="bi bi-trash"></i>
                        </a>
                    </div>`;
                },
                "width": "25%"
            }
        ]
    });
}
