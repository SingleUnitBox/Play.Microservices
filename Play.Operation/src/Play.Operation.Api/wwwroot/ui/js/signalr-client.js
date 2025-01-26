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

    // Updated Event Listeners
    connection.on("connected", () => appendMessage("Connected.", "primary"));
    connection.on("disconnected", () => appendMessage("Disconnected: invalid user ID.", "danger"));

    connection.on("OperationStatusUpdated", (userId, correlationId, status, reason) => {
        console.log("Received OperationStatusUpdated:", { correlationId, status, reason });
        appendMessage(`Status updated: ${status}`, "info", { correlationId, reason });
    });

    connection.on("operation_pending", (correlationId, status)=>
        appendMessage("Operation pending.", "light", { correlationId, status }));
    connection.on("operation_completed", (correlationId, status) =>
        appendMessage("Operation completed.", "success", { correlationId, status }));
    connection.on("operation_rejected", (correlationId, status) =>
        appendMessage("Operation rejected.", "danger", { correlationId, status }));

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
