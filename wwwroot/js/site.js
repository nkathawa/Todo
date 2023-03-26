function remove(id) {
    var todoItem = document.getElementById(id);
    todoItem.remove();
    $.ajax({
        type: 'DELETE',
        url: '/api/todo/' + id,
        contentType: 'application/json'
    }).done(function () {
        console.log('Success');
    }).fail(function () {
        console.log('Error');
    });
}

function changeStatus(json, status) {
    var id = json["Id"];
    json["Status"] = status;
    var json = JSON.stringify(json);
    $.ajax({
        type: 'PUT',
        url: '/api/todo/' + id,
        data: json,
        contentType: 'application/json'
    }).done(function () {
        console.log('Success');
        location.reload();
    }).fail(function () {
        console.log('Error');
    });
}

function addNewItem() {
    var dialog = document.getElementById('addNewItem');
    dialog.style.display = 'block';
}