// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.

$(document).ready(function () {
    if (sessionStorage.getItem("currentpage") == null) {
        changePartialView("partialview_Home");
    }
    else {
        changePartialView(sessionStorage.getItem("currentpage"));
    }
    $("#searchDropdown").on("keyup", function () {
        var value = $(this).val().toLowerCase(); // Get the search term (lowercase)
        $(".dropdown-menu a").filter(function () {
            var text = $(this).text().toLowerCase();
            $(this).toggle(text.indexOf(value) > -1); // Filter and toggle visibility
        });
    });

});

/*Services*/
function openRemoveModel(modelType, id, courseName) {

    $('#removeModelHeader').text("Delete Item");
    $('#removeModelBody').text("Do you really want to delete " + courseName + "?");
    $('#removeModelButton').attr("onclick","deleteData("+modelType+","+id+")");
    $("#removeAlertModel").modal("show");
}

function openLinkedCoursesModel(id, transType, name) {
    if (id != null && transType != "") {
        $.ajax({
            url: '/Home/FetchCoursesPerStudent',
            method: 'POST',
            data: { 'id': id, 'transType': transType },
            async: true,
            success: function (response) {
                if (response != null) {
                    if (transType == "CoursePerStudent") {
                        $("#LinkedDataTitle").text("Course selected by "+ name);
                        $("#LinkedDataList").empty();
                        if (response.coursesPerStudent.length != 0) {
                            response.coursesPerStudent.forEach(function (model) {
                                const tr = $('<tr id="' + model.id + '"></tr>')
                                    .append($('<td></td>').append($('<h5></h5>').text(model.courseCode)))
                                    .append($('<td></td>').append($('<h5></h5>').text(model.courseName)))
                                    .append($('<td></td>').append($('<div></div>')
                                        .append($('<a class="btn btn-danger">Remove</a>').attr('onclick', 'removeStudentFromCourse(' + model.id + ',' + id + ',"' + transType + '")'))));


                                $("#LinkedDataList").append(tr);
                            });
                        }
                        else {
                            const tr = $('<tr></tr>')
                                .append($('<td></td>').append($('<h5></h5>').text("No courses selected yet.")));

                            $("#LinkedDataList").append(tr);

                        }
                        $("#LinkedAddDataList").empty();
                        if (response.courses.length != 0) {
                            response.courses.forEach(function (model) {
                                const li = $('<li id="' + model.id + '"></li>')
                                    .append($('<a></a>').attr("onclick", "addStudentFromCourse(" + model.id + "," + id + ",'" + transType + "')").text(model.courseName));


                                $("#LinkedAddDataList").append(li);
                            });
                        }
                        $("#LinkedData").modal("show");
                    }
                    else {
                        $("#LinkedDataTitle").text("Students in " + name);
                        $("#LinkedDataList").empty();
                        if (response.studentPerCourse.length != 0) {
                            response.studentPerCourse.forEach(function (model) {

                                const tr = $('<tr id="' + model.id + '"></tr>')
                                    .append($('<td></td>').append($('<h5></h5>').text(model.id)))
                                    .append($('<td></td>').append($('<h5></h5>').text(model.firstName + " " + model.surName)))
                                    .append($('<td></td>').append($('<div></div>')
                                        .append($('<a class="btn btn-danger">Remove</a>').attr('onclick', 'removeStudentFromCourse(' + id + ',' + model.id + ',"' + transType + '")'))));

                                $("#LinkedDataList").append(tr);
                            });
                        }
                        else {
                            const tr = $('<tr id="' + model.id + '"></tr>')
                                .append($('<td></td>').append($('<h5></h5>').text("No courses selected yet.")));

                            $("#LinkedDataList").append(tr);

                        }
                        $("#LinkedAddDataList").empty();
                        if (response.students.length != 0) {
                            response.students.forEach(function (model) {
                                const li = $('<li id="' + model.id + '"></li>')
                                    .append($('<a></a>').attr("onclick", "addStudentFromCourse(" + id + "," + model.id + ",'" + transType + "')").text(model.id + " "+model.firstName + " " + model.surName));


                                $("#LinkedAddDataList").append(li);
                            });
                        }
                        $("#LinkedData").modal("show");
                       
                    }
                }


            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Error:", textStatus, errorThrown);
                $("#response").html("Error: " + textStatus);
            }
        });
    }

}

function openModel(model, modeltype, id) {
    var keyData = {
        id: id,
        modelType: modeltype.toString(),
        modelTransType: model.toString()
    }
    var today = new Date();
    
    if (model == "coursesModel") {
        $('#' + model + "Label").text(modeltype + " Course");
        $('#Courses_CourseCode').val("");
        $('#Courses_CourseCode').val("");
        $('#Courses_CourseName').val("");
        $('#Courses_TeacherName').val("");
        $('#Courses_StartDate').val(today.getFullYear + "-" + today.getMonth + "-" + today.getDate);
        $('#Courses_EndDate').val(today.getFullYear + "-" + today.getMonth + "-" + today.getDate);
        $('#submitCourseBtn').attr("onclick", "submitForm(CoursesForm, 'Add', '0')");

    }
    else {
        $('#' + model + "Label").text(modeltype + " Student");
        $('#Student_FirstName').val("");
        $('#Student_SurName').val("");
        $('#Student_DOB').val(today.getFullYear + "-" + today.getMonth + "-" + today.getDate);
        $('#Student_Address1').val("");
        $('#Student_Address2').val("");
        $('#Student_Address3').val("");
        $('#submitStudentBtn').attr("onclick", "submitForm(StudentForm, 'Add', '0')");

    }

    if (modeltype == "Add") {
        $("#" + model).modal("show");
    }
    else {
        $.ajax({
            url: '/Home/FetchOne',
            method: 'POST',
            data: keyData,
            success: function (response) {
                if (model == "coursesModel") {
                    $('#Courses_CourseCode').val(response.courseCode);
                    $('#Courses_CourseCode').val(response.courseCode);
                    $('#Courses_CourseName').val(response.courseName);
                    $('#Courses_TeacherName').val(response.teacherName)
                    $('#Courses_StartDate').val(response.startDate.split('T')[0]);
                    $('#Courses_EndDate').val(response.endDate.split('T')[0]);
                    $('#submitCourseBtn').attr("onclick", "submitForm(CoursesForm, 'Edit'," + response.id + ")");

                }
                else {
                    $('#Student_FirstName').val(response.firstName);
                    $('#Student_SurName').val(response.surName);
                    $('#Student_DOB').val(response.dob.split('T')[0]);
                    $('#Student_Address1').val(response.address1);
                    $('#Student_Address2').val(response.address2);
                    $('#Student_Address3').val(response.address3);
                    $('#' + response.gender).prop('checked', true);
                    $('#submitStudentBtn').attr("onclick", "submitForm(StudentForm, 'Edit'," + response.id + ")");
                }
                $("#" + model).modal("show");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Error:", textStatus, errorThrown);
                $("#response").html("Error: " + textStatus);
            }
        });
    }
    
    
}

//-------------------------------------------------------------------------------------------//
/*Controller Pipelines*/
function changePartialView(partialview) {
    const partialviewContainer = document.getElementById('partialview')
    partialviewContainer.hidden = true;
    sessionStorage.setItem("currentpage", partialview);
    $.ajax({
        url: '/Home/ChangeView',
        method: 'POST',
        data: { 'name': partialview },
        async: true,
        success: function (response) {
            switch (partialview) {
                case "partialview_Home":
                    $('#title-index').html('Home Page');
                    $('#addItemButton').hide();
                    break;
                case "partialview_Students":
                    $('#title-index').text('Student Panel');
                    $('#addItemButton').attr('onclick', 'openModel("studentsModel","Add")');
                    $('#addItemButton').show();
                    break;
                case "partialview_Courses":
                    $('#title-index').text('Course Panel');
                    $('#addItemButton').attr('onclick', 'openModel("coursesModel","Add")');
                    $('#addItemButton').show();
                    break;
            }
            partialviewContainer.hidden = false;
            partialviewContainer.innerHTML = response;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error:", textStatus, errorThrown);
            $("#response").html("Error: " + textStatus);
        }
    })
}

function submitForm(form,modelTransType, id) {
    var formData = {}
    var table = document.getElementById(form.id).getElementsByTagName("table")[0];
    for (var i = 0; i < table.rows.length; i++) {
        var row = table.rows[i];

        var rowname = row.cells[1].children[0].id;
        var rowdata = row.cells[1].children[0].value;

        formData[rowname.split('_')[1]] = rowdata;
    }
    if (modelTransType == "Add") {
        if (form.id == "StudentForm") {
            formData["Gender"] = $("input[name='Gender']:checked")[0].id;
            $.ajax({
                url: '/Home/SubmitStudent',
                method: 'POST',
                data: formData,
                async: true,
                success: function (response) {
                    $('#NotificationToast').toast('show');
                    $("#studentsModel").modal("hide");
                    changePartialView(sessionStorage.getItem("currentpage"));
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error:", textStatus, errorThrown);
                    $("#response").html("Error: " + textStatus);
                }
            });
        }
        else {
            $.ajax({
                url: '/Home/SubmitCourse',
                method: 'POST',
                data: formData,
                async: true,
                success: function (response) {
                    $('#NotificationToast').toast('show');
                    $("#coursesModel").modal("hide");
                    changePartialView(sessionStorage.getItem("currentpage"));
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error:", textStatus, errorThrown);
                    $("#response").html("Error: " + textStatus);
                }
            });
        }
        
    }
    else {
        if (form.id == "StudentForm") {
            formData["Id"] = id;
            formData["Gender"] = $("input[name='Gender']:checked")[0].id;
            $.ajax({
                url: '/Home/UpdateStudent',
                method: 'POST',
                data: formData,
                async: true,
                success: function (response) {
                    $('#NotificationToast').toast('show');
                    $("#studentsModel").modal("hide");
                    changePartialView(sessionStorage.getItem("currentpage"));
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error:", textStatus, errorThrown);
                    $("#response").html("Error: " + textStatus);
                }
            });
        }
        else {
            formData["Id"] = id;
            $.ajax({
                url: '/Home/UpdateCourse',
                method: 'POST',
                data: formData,
                async: true,
                success: function (response) {
                    $('#NotificationToast').toast('show');
                    $("#coursesModel").modal("hide");
                    changePartialView(sessionStorage.getItem("currentpage"));
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("Error:", textStatus, errorThrown);
                    $("#response").html("Error: " + textStatus);
                }
            });
        }
    }
    
    
}

function deleteData(modelType, id) {
    var itemKey = {
        id: id,
        modelType: modelType.id
    } 
    $.ajax({
        url: '/Home/DeleteData',
        method: 'POST',
        data: itemKey,
        success: function (response) {
            $('#NotificationToast').toast('show');
            $("#removeAlertModel").modal("hide");
            changePartialView(sessionStorage.getItem("currentpage"));
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error:", textStatus, errorThrown);
            $("#response").html("Error: " + textStatus);
        }
    }); 
}

function addStudentFromCourse(courseid, studentid,transType) {
    var itemKey = {
        courseid: courseid,
        studentid: studentid,
        transType: transType
    }
    $.ajax({
        url: '/Home/AddCourseToStudent',
        method: 'POST',
        data: itemKey,
        success: function (response) {

            if (transType == 'StudentPerCourse') {
                const tr = $('<tr id="' + response.id + '"></tr>')
                    .append($('<td></td>').append($('<h5></h5>').text(response.id)))
                    .append($('<td></td>').append($('<h5></h5>').text(response.firstName + " " + response.surName)))
                    .append($('<td></td>').append($('<div></div>')
                        .append($('<a class="btn btn-danger">Remove</a>').attr('onclick', 'removeStudentFromCourse(' + courseid + ',' + response.id + ',"' + transType + '")'))));


                $("#LinkedDataList").append(tr);
                $('#LinkedAddDataList li[id="' + studentid + '"]').remove();
            }
            else {
                const tr = $('<tr id="' + response.id + '"></tr>')
                    .append($('<td></td>').append($('<h5></h5>').text(response.courseCode)))
                    .append($('<td></td>').append($('<h5></h5>').text(response.courseName)))
                    .append($('<td></td>').append($('<div></div>')
                        .append($('<a class="btn btn-danger">Remove</a>').attr('onclick', 'removeStudentFromCourse(' + response.id + ',' + studentid + ',"' + transType + '")'))));


                $("#LinkedDataList").append(tr);
                $('#LinkedAddDataList li[id="' + courseid + '"]').remove();
            }
            
           
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error:", textStatus, errorThrown);
            $("#response").html("Error: " + textStatus);
        }
    });
}

function removeStudentFromCourse(courseid, studentid,transType) {
    var itemKey = {
        courseid: courseid,
        studentid: studentid,
        transType: transType
    }
    $.ajax({
        url: '/Home/RemoveCourseToStudent',
        method: 'POST',
        data: itemKey,
        success: function (response) {
            if (transType == 'StudentPerCourse') {
                $('#LinkedDataList tr[id="' + studentid + '"]').remove();

                const li = $('<li id="' + response.id + '"></li>')
                    .append($('<a></a>').attr("onclick", "addStudentFromCourse(" + courseid + "," + response.id + ",'" + transType + "')").text(response.id + " " + response.firstName + " " + response.surName));


                $("#LinkedAddDataList").append(li);
            }
            else {
                $('#LinkedDataList tr[id="' + courseid + '"]').remove();

                const li = $('<li id="' + courseid + '"></li>')
                    .append($('<a></a>').attr("onclick", "addStudentFromCourse(" + courseid + "," + studentid + ",'" + transType + "')").text(response.courseName));


                $("#LinkedAddDataList").append(li);
            }
            

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error:", textStatus, errorThrown);
            $("#response").html("Error: " + textStatus);
        }
    });
}

