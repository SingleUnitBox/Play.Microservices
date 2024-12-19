'use strict';

(function () {
    const $userId = document.getElementById("userId");
    const $connect = document.getElementById("connect");
    const $messages = document.getElementById("messages");

    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/playHub') // Replace with your SignalR hub URL
        .configureLogging(signalR.LogLevel.Information)
        .build();

    $connect.onclick = function () {
        const userId = $userId.value.trim();
        if (!userId) {
            alert("User ID cannot be empty.");
            return;
        }

        appendMessage("Connecting to PlayHub...");
        connection.start()
            .then(() => {
                console.log("SignalR connection started.");
                return connection.invoke("InitializeAsync", userId); // Invoke InitializeAsync with the userId
            })
            .catch(err => {
                console.error("SignalR connection failed: ", err);
                appendMessage("Connection failed: " + err, "danger");
            });
    };

    connection.on("connected", () => appendMessage("Connected.", "primary"));
    connection.on("disconnected", () => appendMessage("Disconnected: invalid user ID.", "danger"));
    connection.on("operation_pending", (operation) => appendMessage("Operation pending.", "light", operation));
    connection.on("operation_completed", (operation) => appendMessage("Operation completed.", "success", operation));
    connection.on("operation_rejected", (operation) => appendMessage("Operation rejected.", "danger", operation));

    function appendMessage(message, type = "info", data = null) {
        const listItem = document.createElement("li");
        listItem.className = `list-group-item list-group-item-${type}`;
        listItem.innerHTML = `${message}`;
        if (data) {
            listItem.innerHTML += `<div>${JSON.stringify(data)}</div>`;
        }
        $messages.appendChild(listItem);
    }
})();
