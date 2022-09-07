//var dataTable;

//$(document).ready(function () {
//    loadDataTable();
//});

//function loadDataTable() {
//    $(document).ready(function () {
//        let obj = {
//            data: data,
//            columns: [
//                { data: 'title' },
//                { data: 'isbn' },
//                { data: 'price' }
//            ]
//        }
//        $('#tblData').DataTable({
//            "ajax": {
//                "url": "/admin/product/getall"
//            },
//            'columns': [
//                { data: 'title', "width": "15%" },
//                { data: 'isbn', "width": "15%" },
//                { data: 'price', "width": "15%" }
//            ]
//        });
//    });
//}

var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category", "width": "15%" }
        ]
    });
}