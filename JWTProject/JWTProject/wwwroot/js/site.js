let todos = [];
const spinner = document.getElementById("loaderbody");
function getItems() {
    spinner.className = "show";
    fetch("https://localhost:44390/api/employee/getemployees")
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    spinner.className = "show";
    const addNameTextbox = document.getElementById('Name');
    const addSalaryTextbox = parseInt(document.getElementById('Salary').value);
    const item = {
        name: addNameTextbox.value.trim(),
        salary: addSalaryTextbox

    };

    fetch("https://localhost:44390/api/employee/create", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addSalaryTextbox = 0;
        })
        .catch(error => console.error('Unable to add item.', error));
    closeaddForm();

}

function deleteItem(id) {
    spinner.className = "show";
    fetch(`https://localhost:44390/api/employee/delete/${id}`,
    {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    closeaddForm();
    const item = todos.find(item => item.id === id);
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-salary').value = item.salary;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    spinner.className = "show";
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        salary: parseInt(document.getElementById('edit-salary').value)
    };

    fetch("https://localhost:44390/api/employee/edit", {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function closeaddForm() {
    document.getElementById('addForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Employee' : 'Employees';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {


       let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.className = "btn btn-success";
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.className = "btn btn-danger";
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(item.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode1 = document.createTextNode(item.salary);
        td2.appendChild(textNode1);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });
    spinner.className = "hide";
    todos = data;
}



