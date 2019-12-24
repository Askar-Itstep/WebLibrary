//4)без хелперов-работ.!
$(document).ready(function () {
    $('.read').on("click", function (e) {   
        var id = $(this).parent().parent().children(':first').text();
        e.preventDefault();

        $.ajax({
            url: '/Book/_UsersReadThisBook',
            method: 'GET',
            data: { id: id },
            success: function (data) {
                $('#myResult').html(data)
            },
            error: function (e) {
                console.log("error")
            }
        });
    });
})