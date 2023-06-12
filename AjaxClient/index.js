const URL = "http://10.182.127.200:2119/MyRestService.svc";

const Person = {
    Id: 0,
    Name: "",
    Height: 0,
    Birthdate: "",
    Email: ""
}

function toggleForm() {
    const formContainer = document.getElementById('form-container');
    formContainer.style.display = 'flex';
}

function hideForm() {
    const formContainer = document.getElementById('form-container');
    formContainer.style.display = 'none';
}

function clearForm() {
    const form = document.getElementById("person-form");
    const inputs = form.getElementsByTagName("input");

    for (let i = 0; i < inputs.length; i++) {
        inputs[i].value = "";
    }
}

function removeInputsFromForm() {
    const form = document.getElementById("inputs-container");
    while (form.firstChild) {
        form.firstChild.remove();
    }
}

function createInputLabel(labelText, inputId, inputType, isRequired, min, max) {
    const inputsContainer = document.getElementById("inputs-container");
    const label = document.createElement("label");
    label.setAttribute("for", inputId);
    label.textContent = labelText;
    inputsContainer.appendChild(label);

    const input = document.createElement("input");
    input.setAttribute("type", inputType);
    input.setAttribute("id", inputId);
    input.required = isRequired;
    if (min || min===0) {
        input.min = min;
    }
    if (max) {
        input.max = max;
    }
    inputsContainer.appendChild(input);

    inputsContainer.appendChild(document.createElement("br"));
}

function setTableHeaderToDefault() {
    const tableHeader = document.getElementById("persons-table-header");
    tableHeader.innerHTML = "";
    const headerRow = document.createElement("tr");
    headerRow.innerHTML = `<th>Id</th><th>Name</th><th>Height</th><th>Birthdate</th><th>Email</th>`;
    tableHeader.appendChild(headerRow);
}


function getAllPeople() {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/persons";
    xhr.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            const response = JSON.parse(this.responseText);
            console.log(response);
            setTableHeaderToDefault();

            const tableBody = document.getElementById("persons-list");
            tableBody.innerHTML = "";

            for (let i = 0; i < response.length; i++) {
                const person = response[i];
                const row = document.createElement("tr");
                row.innerHTML = `<td>${person.Id}</td><td>${person.Name}</td><td>${person.Height}</td>
                    <td>${person.Birthdate}</td><td>${person.Email}</td>`;
                tableBody.appendChild(row);
            }
        }
    };
    xhr.open("GET", endpoint, true);
    xhr.send();
}


document.getElementById("getAllBtn").addEventListener("click", function() {
    hideForm();
    getAllPeople();
});


function getPersonById() {
    const xhr = new XMLHttpRequest();
    const id = document.getElementById("id").value;
    const endpoint = URL + "/persons/" + id;
    console.log(endpoint);
    xhr.onreadystatechange = function() {
        if (this.readyState === 4) {
            if (this.status === 200) {
                const response = JSON.parse(this.responseText);
                console.log(response);
                showMessage('success', `Person with id=${id} found.`)
                setTableHeaderToDefault();

                const tableBody = document.getElementById("persons-list");
                tableBody.innerHTML = "";

                const row = document.createElement("tr");
                row.innerHTML = `<td>${response.Id}</td><td>${response.Name}</td><td>${response.Age}</td>
                    <td>${response.Birthdate}</td><td>${response.Email}</td>`;
                tableBody.appendChild(row);
            } else {
                setTableHeaderToDefault();
                const tableBody = document.getElementById("persons-list");
                tableBody.innerHTML = "";
                if (this.status === 404) {
                    showMessage('error', `A person with this id=${id} does not exist.`);
                }
                else {
                    showMessage('error', 'An error occurred. Please try again later.');
                }
            }
        }
    };
    xhr.open("GET", endpoint, true);
    xhr.send();
}


document.getElementById("getByIdBtn").addEventListener("click", function() {
    hideForm();
    removeInputsFromForm();
    document.getElementById("form-title").textContent = "Get Person by Id";
    createInputLabel( "Id", "id", "number", true);
    toggleForm();
});


function getPeopleByName() {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/persons/name/" + document.getElementById("name").value;
    console.log(endpoint);
    xhr.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            const response = JSON.parse(this.responseText);
            console.log(response);
            setTableHeaderToDefault();

            const tableBody = document.getElementById("persons-list");
            tableBody.innerHTML = "";

            for (let i = 0; i < response.length; i++) {
                const person = response[i];
                const row = document.createElement("tr");
                row.innerHTML = `<td>${person.Id}</td><td>${person.Name}</td><td>${person.Height}</td>
                    <td>${person.Birthdate}</td><td>${person.Email}</td>`;
                tableBody.appendChild(row);
            }
        }
    };
    xhr.open("GET", endpoint, true);
    xhr.send();
}


document.getElementById("filterByNameBtn").addEventListener("click", function() {
    hideForm();
    removeInputsFromForm();
    document.getElementById("form-title").textContent = "Filter People by Name";
    createInputLabel("Name", "name", "text", true);
    toggleForm();
});


function getNumberOfPeople() {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/persons/size";
    console.log(endpoint);
    xhr.onreadystatechange = function() {
        if (this.readyState === 4 && this.status === 200) {
            const response = JSON.parse(this.responseText);
            console.log(response);

            const tableHeader = document.getElementById("persons-table-header");
            tableHeader.innerHTML = "";
            const headerRow = document.createElement("tr");
            headerRow.innerHTML = `<th>Number of People</th>`;
            tableHeader.appendChild(headerRow);

            const tableBody = document.getElementById("persons-list");
            tableBody.innerHTML = "";

            const row = document.createElement("tr");
            row.innerHTML = `<td>${response}</td>`;
            tableBody.appendChild(row);
        }
    };
    xhr.open("GET", endpoint, true);
    xhr.send();
}


document.getElementById("countBtn").addEventListener("click", function() {
    hideForm();
    getNumberOfPeople();
});


function addPerson(person) {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/persons";
    console.log(endpoint);
    console.log(person);
    console.log(JSON.stringify(person));
    xhr.onreadystatechange = function() {
        if (this.readyState === 4) {
            getAllPeople();
            if (this.status === 200 || this.status === 201) {
                const response = JSON.parse(this.responseText);
                console.log(response);
                showMessage('success', `Person added successfully!`);
            } else {
                if (this.status === 400) {
                    showMessage('error', 'Bad request. Please check the data.');
                } else if (this.status === 409) {
                    showMessage('error', `A person with name=${person.Name}, height=${person.Height}, 
                        birthdate=${person.Birthdate}, email=${person.Email} already exists.`);
                }
                else {
                    showMessage('error', 'An error occurred. Please try again later.');
                }
            }
        }
    };
    xhr.open("POST", endpoint, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send(JSON.stringify(person));
}


document.getElementById("addBtn").addEventListener("click", function() {
    hideForm();
    removeInputsFromForm();
    document.getElementById("form-title").textContent = "Add Person";
    createInputLabel("Name", "name", "text", true);
    createInputLabel("Height", "height", "number", true, 0, 230);
    createInputLabel("Birthdate", "birthdate", "date", true, "1900-01-01", Date.now());
    createInputLabel("Email", "email", "email", true);
    toggleForm();
});


function updatePerson(person) {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/persons";
    console.log(endpoint);
    console.log(person);
    console.log(JSON.stringify(person));
    xhr.onreadystatechange = function() {
        if (this.readyState === 4) {
            getAllPeople();
            if (this.status === 200) {
                const response = JSON.parse(this.responseText);
                console.log(response);
                showMessage('success', `Person with id=${person.Id} updated successfully!`);
            } else {
                if (this.status === 404) {
                    showMessage('error', `A person with id=${person.Id} does not exist.`);
                }
                else {
                    showMessage('error', 'An error occurred. Please try again later.');
                }
            }
        }
    };
    xhr.open("PUT", endpoint, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send(JSON.stringify(person));
}


document.getElementById("updateBtn").addEventListener("click", function() {
    hideForm();
    removeInputsFromForm()
    document.getElementById("form-title").textContent = "Update Person";
    createInputLabel( "Id", "id", "number", true);
    createInputLabel("Name", "name", "text", true);
    createInputLabel("Height", "height", "number", true, 0, 230);
    createInputLabel("Birthdate", "birthdate", "date", true, "1900-01-01", Date.now());
    createInputLabel("Email", "email", "email", true);
    toggleForm();
});


function deletePerson(id) {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/persons/" + id;
    console.log(endpoint);
    xhr.onreadystatechange = function() {
        if (this.readyState === 4) {
            getAllPeople();
            if (this.status === 200) {
                const response = JSON.parse(this.responseText);
                console.log(response);
                showMessage('success', `Person with id=${id} deleted successfully!`);
            } else {
                if (this.status === 404) {
                    showMessage('error', `A person with id=${id} does not exist.`);
                }
                else {
                    showMessage('error', 'An error occurred. Please try again later.');
                }
            }
        }
    };
    xhr.open("DELETE", endpoint, true);
    xhr.send();
}


document.getElementById("deleteBtn").addEventListener("click", function() {
    hideForm();
    removeInputsFromForm()
    document.getElementById("form-title").textContent = "Delete Person";
    createInputLabel( "Id", "id", "number", true);
    toggleForm();
});


document.getElementById("submitBtn").addEventListener("click", function() {
    if (document.getElementById("form-title").textContent === "Get Person by Id") {
        const id = document.getElementById("id").value;
        getPersonById(id);
    }
    else if (document.getElementById("form-title").textContent === "Filter People by Name") {
        const name = document.getElementById("name").value;
        getPeopleByName(name);
    }
    else if (document.getElementById("form-title").textContent === "Add Person") {
        const person = Person;
        person.Name = document.getElementById("name").value;
        person.Height = document.getElementById("height").value;
        person.Birthdate = document.getElementById("birthdate").value;
        person.Email = document.getElementById("email").value;
        addPerson(person);
    }
    else if (document.getElementById("form-title").textContent === "Update Person") {
        const person = Person;
        person.Id = document.getElementById("id").value;
        person.Name = document.getElementById("name").value;
        person.Height = document.getElementById("height").value;
        person.Birthdate = document.getElementById("birthdate").value;
        person.Email = document.getElementById("email").value;
        updatePerson(person);
    }
    else if (document.getElementById("form-title").textContent === "Delete Person") {
        const id = document.getElementById("id").value;
        deletePerson(id);
    }
    hideForm();
});


function showMessage(type, text) {
    const message = document.getElementById('message');
    message.textContent = text;
    message.classList.remove('error-message', 'success-message');
    message.classList.add(type + '-message');
    message.classList.add('show');

    setTimeout(() => {
        hideMessage();
    }, 4000);
}

function hideMessage() {
    const message = document.getElementById('message');
    message.classList.remove('show');
}

function myDataInfo() {
    console.log("Zuzanna Sikorska, 260464");
    console.log("Piotr Łazik, 260371");
    console.log(new Date().toLocaleString("pl-PL"));
    console.log(navigator.userAgent);
    console.log(navigator.platform);
    console.log();
}

myDataInfo();

document.getElementById("printAuthorsBtn").addEventListener("click", function() {
    hideForm();
    printAuthors();
});


function printAuthors() {
    const xhr = new XMLHttpRequest();
    const endpoint = URL + "/authors";
    console.log(endpoint);
    xhr.onreadystatechange = function() {
        if (this.readyState === 4) {
            if (this.status === 200) {
                const response = JSON.parse(this.responseText);
                console.log(response);

                const tableHeader = document.getElementById("persons-table-header");
                tableHeader.innerHTML = "";
                const headerRow = document.createElement("tr");
                headerRow.innerHTML = `<th>Authors</th>`;
                tableHeader.appendChild(headerRow);

                const tableBody = document.getElementById("persons-list");
                tableBody.innerHTML = "";

                const row = document.createElement("tr");
                row.innerHTML = `<td>${response}</td>`;
                tableBody.appendChild(row);
            } else {
                showMessage('error', 'An error occurred. Please try again later.');
            }
        }
    };
    xhr.open("GET", endpoint, true);
    xhr.send();
}

