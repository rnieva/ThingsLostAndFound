// Add validation for uploaded file, size and extension
var flag = true; // If the user doesn´t upload any file
function validate_fileupload(fileName) {
    var allowed_extensions = new Array("jpg", "png", "gif", "jpeg", "bmp");
    var file_extension = fileName.split('.').pop(); // split function will split the filename by dot(.), and pop function will pop the last element from the array which will give you the extension as well. If there will be no extension then it will return the filename.
    for (var i = 0; i <= allowed_extensions.length; i++) {
        if (allowed_extensions[i] == file_extension) {
            var fileUpload = document.getElementById("file");
            if (typeof (fileUpload.files) != "undefined") {
                var size = parseFloat(fileUpload.files[0].size / 1024).toFixed(2);
                if (size > 4000) {
                    flag = false;
                    alert("File too much big " + size + " KB.");
                    return false;
                }
            }
            flag = true;
            return true; // valid file extension
        }
    }
    alert("Upload Images only");
    flag = false;
    return false;
}

function validate() {
    if (flag == true) {
        return true;
    } else {
        alert("Wrong File");
        return false;
    }
}