$(document).ready(function () {
    
    $('#otherdeviceinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#monitorinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#networkinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#pcinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#printerinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#scannerinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#upsinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#branchwisedeviceinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $('#generatorinfo tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="' + title + '" />');
    });
    $("#otherdeviceinfo, #monitorinfo, #networkinfo, #pcinfo, #printerinfo, #scannerinfo, #upsinfo, #branchwisedeviceinfo, #generatorinfo").DataTable({
        "searching": true, // false to disable search (or any other option)
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        initComplete: function () {
            // Apply the search
            this.api().columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change clear', function () {
                    if (that.search() !== this.value) {
                        that
                            .search(this.value)
                            .draw();
                    }
                });
            });
        }
    });
   $('.dataTables_length').addClass('bs-select');
   
});