function deleteDishes() {
    var checkboxes = $("input:checkbox:checked");
    let arr = [];
    $.each(checkboxes, function (index, value) {
       arr.push(value.id);
    });
        $.ajax({
            url: "http://localhost:5000/Dishes/Delete",
            type: "Delete",
            data: {ids:arr},
            success: function (data) {
                $("#result").html(data);
                $(".show").hide();
            }
        })
}