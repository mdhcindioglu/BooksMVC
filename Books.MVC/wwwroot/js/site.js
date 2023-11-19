
window.onload = function () {
    setFocusByClass('autofocus');
};

function setFocusByClass(className) {
    var elements = document.getElementsByClassName(className);
    if (elements !== undefined && elements.length > 0) {
        elements[0].focus();
    }
}

function setFocusById(id) {
    var element = document.getElementsById(id);
    if (element !== undefined) {
        element.focus();
    }
}

function showModal(url, title) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#modal .modal-title").html(title);
            $("#modal .modal-body").html(res);
            var model = document.getElementById('modal');
            model.addEventListener('shown.bs.modal', function (event) {
                setFocusByClass('autofocus');
            })
            $("#modal").modal('show');
        },
        error: function (err) {
            console.log(err)
        }
    });
}

jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isSuccess) {
                    $('#modal .modal-body').html('');
                    $('#modal .modal-title').html('');
                    $('#modal').modal('hide');
                    refresh();
                }
                else {
                    $('#modal .modal-body').html(res.html);
                    setFocusByClass('autofocus');
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'DELETE',
                    url: form.action,
                    data: new FormData(form),
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        refresh();
                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
            } catch (ex) {
                console.log(ex)
            }
        }
    });

    //prevent default form submit event
    return false;
}