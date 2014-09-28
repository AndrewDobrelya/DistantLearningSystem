function submitForm() {
    console.log('enter submit form!');
    var name = $("#Name");
    var lastName = $("#LastName");
    var mail = $("#Email");
    var passWord = $("#Password");
    var confirmPassword = $("#ConfirmPassword");
    var imageUpload = $("#imageUpload");

    console.log(name.val());
    if (name.val() == "" ||
        mail.val() == "" ||
        lastName.val() == "" ||
        passWord.val() == "" ||
        confirmPassword.val() == "") {
        document.getElementById("information").innerHTML = 'Заполнены не все поля';
        $("#info").show();
        return;
    }

    console.log(passWord.val());
    console.log(confirmPassword.val());

    if (confirmPassword.val() != passWord.val()) {
        document.getElementById("information").innerHTML = 'Пароли не совпадают';
        $("#info").show();
    }
    else {
        alert('submitting main form!');
        $("#main_form").submit();
    }
}

function submitEditForm() {

    var passWord = $("#Password");
    var confirmPassword = $("#ConfirmPassword");

    console.log(window.Name.val());

    console.log(passWord.val());
    console.log(confirmPassword.val());

    if (confirmPassword.val() != passWord.val()) {
        document.getElementById("information").innerHTML = 'Пароли не совпадают';
        $("#info").show();
    }
    else {
        console.log('submitting form');
        $("#main_form").submit();
    }
}

function univerChange() {
    console.log('univerChange starts!');
    //clearAll();
    clearFaculties();
    var val = document.getElementById("univer").value;
    console.log(val);
    $.ajax({
        url: "/Json/Faculties", data: { data: val }, success: function (data) {

            var obj = document.getElementById('faculty');
            for (var i = 0; i < data.Length; i++) {
                obj.options[i] = new Option(data.Data[i], data.Value[i]);
            }

            facultyChange();
        }
    });
    console.log('univerChange ends!');
}

function clearAll() {
    console.log('clear all starts!');
    clearDepartments();
    clearFaculties();
    console.log('clear all ends!');

}

function clearFaculties() {
    console.log('clearFaculties starts!');
    //var obj = document.getElementById('faculty');
    //var length = obj.options.length;
    $("#faculty").empty();
    //for (var i = 0; i < length; i++) {
    //    obj.options[i] = null;
    //}

    console.log('clearFaculties ends!');
}

function clearDepartments() {

    console.log('clearDepartments starts!');
    //var obj = document.getElementById('department');
    //var length = obj.options.length;
    $("#department").empty();
    //console.log(length);

    //for (var i = 0; i < length; i++) {
    //    obj.options[i] = null;
    //}

    console.log('clearDepartments ends!');
}

function facultyChange() {

    console.log('facultyChange starts');
    clearDepartments();
    var val = document.getElementById("faculty").value;
    $.ajax({
        url: "/Json/Departments", data: { data: val }, success: function (data) {
            var obj = document.getElementById('department');
            for (var i = 0; i < data.Length; i++) {
                obj.options[i] = new Option(data.Data[i], data.Value[i]);
            }
            departmentChange();
        }
    });

    console.log('facultyChange ends');
}

function departmentChange() {
    console.log('enter dept change');
    var val = $("#department").val();
    var obj = document.getElementById('groupId');
    if (obj == null)
        return;
    clearGroups();
    $.ajax({
        url: "/Json/Groups", data: { data: val }, success: function (data) {
            for (var i = 0; i < data.Length; i++) {
                obj.options[i] = new Option(data.Data[i], data.Value[i]);
            }
            console.log('Exit dept change');
        }
    });
}

function clearGroups() {
    $("#groupId").empty();
}

function onChange() {
    var val = $('#whoareu').val();

    $.ajax({
        url: "/User/AjaxRegistrationFormLoad",
        data: { data: val },
        //datatype: "html",
        //type: "GET",
        success: function (data) {
            $("#form").replaceWith(data);
            univerChange();
        }
    });
}