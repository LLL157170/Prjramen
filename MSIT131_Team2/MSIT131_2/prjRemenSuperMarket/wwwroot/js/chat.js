"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
/*document.getElementById("sendButton").disabled = true;*/

connection.on("ReceiveMessage", function (user, message) {
    var userx = document.getElementById("userInput").value;
    var li = document.createElement("li");
    let inp = document.getElementById('messageInput')
    document.getElementById("messagesList").appendChild(li);
    if (user == userx) {
        li.textContent = `${user} : ${message}`;
        li.style = "color:green"
    } else {
        li.textContent = `${user} : ${message}`;
        li.style = "text-align:right;color:blue"
    }
    inp.value = ""
});
document.getElementById("chat").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    connection.invoke("save", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (id) {
        document.getElementById("connectionId").innerText = id;
    });
/*    document.getElementById("sendToUser").disabled = false;*/
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendToUser").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value; 
    var recivename = document.getElementById("recivename").value;
    if (message == "") {
        return
    } else {
        connection.invoke("SendToUser", user, message, recivename)
            .catch(function (err) {
                return console.error(err.toString());
            });
    }

    event.preventDefault();
});