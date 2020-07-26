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
            $(".modal").hide();
        }
    });   
}
    
$("#delete").click(function (event) {
    event.preventDefault();
    $(".button-delete").show();
    $(".button-hide").hide();
    var checkboxes = $("input:checkbox:checked");
    if (checkboxes.length > 0){
        $(".modal").show();
        $("#message").hide();
    }
    else{
        $("#message").show();
    }
});

function closeModal() {
    $(".modal").hide();
}

$("#hide").click(function (event) {
    event.preventDefault();
    $(".button-delete").hide();
    $(".button-hide").show();
    var checkboxes = $("input:checkbox:checked");
    if (checkboxes.length > 0){
        $(".modal").show();
        $("#message").hide();
    }
    else{
        $("#message").show();
    }
});

function hideDishes() {
    var checkboxes = $("input:checkbox:checked");
    let arr = [];
    $.each(checkboxes, function (index, value) {
        arr.push(value.id);
    });
    $.ajax({
        url: "http://localhost:5000/Dishes/Hide",
        type: "Put",
        data: {ids:arr},
        success: function (data) {
            $("#result").html(data);
            $(".modal").hide();
        }
    });
}