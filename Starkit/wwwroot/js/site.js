
function showDropdown(value){
    console.log(value)
    let el;
    if (value === 1){
        $('#dropdown2').hide();
        el = document.getElementById('dropdown1');
    }
    else if (value === 2){
        $('#dropdown1').hide();
        el = document.getElementById('dropdown2');
    }
    console.log(el.style.display)
    if (el.style.display === '') 
        el.style.display = 'none';
        
    el.style.display === 'none' ? el.style.display = 'initial' : el.style.display = 'none';
}