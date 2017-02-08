// validate if the user has checked some check box when he wants to delete an object

function validate() {
    if ((checkObjectSolved.checked == false) && (checkUserCreateObject.checked == false)) {
        alert('Please check at least one of the option.');
        return false;
    }
    else {
        if ((checkObjectSolved.checked == true) && (checkUserCreateObject.checked == false)) {
            if (document.forms["form"]["nameContact"].value == "") {
                alert('Please write the contact name');
                return false;
            }
            return true;
        }
        else {
            if ((checkObjectSolved.checked == false) && (checkUserCreateObject.checked == true)) {
                return true; //checkUserCreateObject I wanna forget it
            } else {
                alert('Please check only one of the option.');
                return false;
            }
        }

    }
}