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

function edit(id) {
    var dialog = document.getElementById('editItem');
    var todoItem = document.getElementById(id);
    var title = todoItem.getElementsByClassName('title')[0].innerHTML;
    var description = todoItem.getElementsByClassName('description')[0].innerHTML;
    var status = todoItem.getElementsByClassName('status')[0].innerHTML;
    var json = {
        "Id": id,
        "Title": title,
        "Description": description,
        "Status": status
    };
    json = JSON.stringify(json);
    document.getElementById('editItem').setAttribute('data-json', json);
}

function showEditForm(title, description, priority, date) {
    // Set the form's display style to "block" to show it
    document.getElementById('editForm').style.display = 'block';

    // Set the values of the form's fields
    document.getElementById('editFormTitle').value = title;
    document.getElementById('editFormDescription').value = description;
    document.getElementById('editFormPriority').value = priority;
    document.getElementById('editFormDate').value = date;
}

function submitEditForm() {
    // Get the values from the form fields
    var title = document.getElementById('editFormTitle').value;
    var description = document.getElementById('editFormDescription').value;
    var priority = document.getElementById('editFormPriority').value = priority;
    var date = document.getElementById('editFormDate').value;

    // Call a function to update the todo item with the new values
    updateTodoItem(title, description, priority, date);

    // Hide the edit form
    document.getElementById('editForm').style.display = 'none';
}

function cancelEditForm() {
    // Hide the edit form
    document.getElementById('editForm').style.display = 'none';
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