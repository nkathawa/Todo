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

function showEditForm(id, title, description, priority, date, status) {
    $('#editModal').modal('show');
    var d = new Date(date);
    var formattedDate = d.getFullYear() + '-' + ('0' + (d.getMonth()+1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);

    document.getElementById('editFormTitle').value = title;
    document.getElementById('editFormDescription').value = description;
    document.getElementById('editFormPriority').value = priority;
    document.getElementById('editFormDate').value = formattedDate;

    document.getElementById('editFormId').value = id;
    document.getElementById('editFormStatus').value = status;
}

function submitEditForm() {
    var id = document.getElementById('editFormId').value;
    var title = document.getElementById('editFormTitle').value;
    var description = document.getElementById('editFormDescription').value;
    var priority = document.getElementById('editFormPriority').value;
    var date = document.getElementById('editFormDate').value;
    var status = document.getElementById('editFormStatus').value;

    updateTodoItem(id, title, description, priority, date, status);

    document.getElementById('editForm').style.display = 'none';
}

function submitFilterForm(event, priority, date) {
    console.log("hi");
    event.preventDefault();
    var cards = document.getElementsByClassName("card");

    date = document.getElementById("date-input").value;

    let prioritySelect = document.getElementById("priority-select");
    priority = prioritySelect.value;
    console.log(priority);

    for (var i = 0; i < cards.length; i++) {
        var card = cards[i];
        var priorityElement = card.querySelector("img");
        console.log(priorityElement.getAttribute("src"));
        if(date) {
            console.log(date);
            var cardDate = card.dataset.date;
            console.log(cardDate);
            var dateParts = cardDate.split("/");
            var year = dateParts[2];
            var month = dateParts[0];
            var day = dateParts[1];
            var newCardDate = year + "-" + month + "-" + day;
            console.log(newCardDate);

            if(newCardDate == date) {
                card.style.display = "flex";
            } else {
                card.style.display = "none";
            }
            continue;
        }
        if(priority === 1) {
            console.log("yooooooooo11111111");
            if (priorityElement.getAttribute("src").includes('high')) {
                card.style.display = "none";
            } else {
                card.style.display = "flex";
            }
        } else if(priority === 0) {
            console.log("yooooooooo");
            if (priorityElement.getAttribute("src").includes('high')) {
                card.style.display = "flex";
            } else {
                card.style.display = "none";
            }
        }
    }
}

function updateTodoItem(id, title, description, priority, date, status) {
    var highImg = new Image();
    highImg.src = "../images/high.png";

    var lowImg = new Image();
    lowImg.src = "../images/low.png";

    var todoItem = document.getElementById(id);

    todoItem.getElementsByClassName('card-title')[0].innerHTML = title;
    todoItem.getElementsByClassName('card-text')[0].innerHTML = description;
    
    var img = todoItem.getElementsByClassName('card-text')[2].getElementsByTagName('img')[0];
    console.log(priority);
    img.src = priority == "LOW" ? lowImg.src : highImg.src;
    img.width = 24;
    img.height = 24;

    todoItem.getElementsByClassName('card-text')[1].innerHTML = date;

    updateTodoItemInDatabase(id, title, description, priority, date, status);
}

function updateTodoItemInDatabase(id, title, description, priority, date, status) {
    if(status == "OPEN") {
        statusNum = 0;
    } else if(status == "ARCHIVED") {
        statusNum = 1;
    } else {
        statusNum = 2;
    }
    var json = {
        "Id": id,
        "Title": title,
        "Description": description,
        "Priority": priority == "LOW" ? 1 : 0,
        "Date": date ? date : null,
        "Status": statusNum,
        "UserId": "b5453c33-014c-4757-a8e3-f7732954c04c"
    };
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

function cancelEditForm() {
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