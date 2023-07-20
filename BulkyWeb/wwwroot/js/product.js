// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        scrollX: true,
        "ajax": { url: '/admin/product/GetAll' },
        "columns": [
            { data: 'Title', "width": "25%" },
            { data: 'ISBN', "width": "15%" },
            { data: 'Price', "width": "10%" },
            { data: 'Author', "width": "15%" },
            { data: 'Category.Name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a href="/admin/product/delete?id=${data}" class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

