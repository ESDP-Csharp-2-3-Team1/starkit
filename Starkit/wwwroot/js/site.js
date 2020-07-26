function deleteDishes() {
    var checkboxes = $("#checkbox-td input:checkbox:checked");
    let arr = [];
    $.each(checkboxes, function (index, value) {
       arr.push(value.id);
    });
    if (arr.length > 0){
        $.ajax({
            url: "http://localhost:5000/Dishes/Delete",
            type: "Delete",
            data: {ids:arr},
            success: function (data) {
                $("#result").html(data);
            }
        })   
    }
}