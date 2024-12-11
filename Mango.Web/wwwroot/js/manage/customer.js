var dataTable;

$(document).ready(function () {
    loadUserTable();
});

function loadUserTable() {
    dataTable = $('#tblUserData').DataTable({
        order: [[1, 'asc']],
        "ajax": { url: "/Auth/GetAllCustomer", type: "GET" },
        "columns": [
            { data: 'firstName', "width": "10%" },
            { data: 'lastName', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "10%", "className": "text-left" },
            { data: 'gender', "render": function (data) { return data ? 'Male' : 'Female'; }, "width": "5%" },
            { data: 'address', "width": "15%" },
            { data: 'birthday', "render": function (data) { return new Date(data).toLocaleDateString(); }, "width": "10%" },
            {
                data: 'email',
                "render": function (data, type, row) {
                    if (row.isActive) {
                        return `<div class="btn-group" role="group">
                            <a href="javascript:void(0);" onclick="confirmAction('/Auth/BanAccount?email=${data}', 'Are you sure you want to ban this account?')" class="text-danger mx-2">
                               <i class="fa-solid fa-lock"></i>
                            </a>
                        </div>`;
                    } else {
                        return `<div class="btn-group" role="group">
                            <a href="javascript:void(0);" onclick="confirmAction('/Auth/UnBanAccount?email=${data}', 'Are you sure you want to unban this account?')" class="text-warning mx-2">
                               <i class="fa-solid fa-unlock"></i>
                            </a>
                        </div>`;
                    }
                },
                "width": "10%"
            }
        ]
    });
}

function confirmAction(url, message) {
    if (confirm(message)) {
        window.location.href = url;
    }
}
