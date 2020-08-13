
function showDropdown(value){
    let el;
    if (value === 1){
        $('#dropdown2').hide();
        el = document.getElementById('dropdown1');
    }
    else if (value === 2){
        $('#dropdown1').hide();
        el = document.getElementById('dropdown2');
    }
    el.style.display === 'none' ? el.style.display = 'initial' : el.style.display = 'none';

}