
function showDropdown(value){
    let el;
    if (value === 0){
        $('#dropdown2').hide();
        $('#dropdown1').hide();
        el = document.getElementById('dropdown0');
    }
    else if (value === 1){
        $('#dropdown2').hide();
        $('#dropdown0').hide();
        el = document.getElementById('dropdown1');
    }
    else if (value === 2){
        $('#dropdown1').hide();
        $('#dropdown0').hide();
        el = document.getElementById('dropdown2');
    }
    if (el.style.display === '') 
        el.style.display = 'none';
        
    el.style.display === 'none' ? el.style.display = 'initial' : el.style.display = 'none';
}