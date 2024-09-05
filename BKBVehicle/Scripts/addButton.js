
$(document).ready(function () {
    // Add more rows
    $(document).on("click", ".addMoreData", function () {
        var testRow = document.getElementById("addPortion").innerHTML;
        $('.MoreItemAdd').append(testRow);
    });

    // Delete row
    $(document).on("click", ".deleteData", function () {
        $(this).closest('.row').remove();
    });
});
